using System;
using System.Collections.Generic;
using System.Linq;
using starikcetin.UnityCommon.Utils.Common;

namespace starikcetin.UnityCommon.Utils.Sampling.Vector2
{
    /// <summary>
    /// Grid based sampling of Vector2s.
    /// </summary>
    public class GridBasedSampling
    {
        private readonly ListShuffle _listShuffler;
        private readonly List<UnityEngine.Vector2> _allNodes;
        private List<UnityEngine.Vector2> _availableNodes;

        /// <summary>
        /// Generates random Vector2 samples from a grid.
        /// </summary>
        /// <param name="grid">The grid to make sampling on.</param>
        /// <param name="seed">Seed.</param>
        public GridBasedSampling(IList<UnityEngine.Vector2> grid, int seed)
        {
            _listShuffler = new ListShuffle(seed);

            _allNodes = new List<UnityEngine.Vector2>(grid);
            _listShuffler.Shuffle(_allNodes);

            _availableNodes = new List<UnityEngine.Vector2>(_allNodes);
        }

        /// <summary>
        /// Makes all nodes available again. If <paramref name="reShuffle"/> is true, also reshuffles the nodes.
        /// </summary>
        public void ResetSampleHistory(bool reShuffle)
        {
            if (reShuffle)
            {
                _listShuffler.Shuffle(_allNodes);
            }

            _availableNodes = new List<UnityEngine.Vector2>(_allNodes);
        }

        /// <summary>
        /// Tries to get an available node. Returns true if a node is found, returns false otherwise.
        /// </summary>
        /// <param name="sample">The sample node.</param>
        /// <returns>True if a node is found, returns false otherwise.</returns>
        public bool TrySample(out UnityEngine.Vector2 sample)
        {
            if (_availableNodes.Count == 0)
            {
                sample = UnityEngine.Vector2.zero;
                return false;
            }

            sample = _availableNodes[0];
            _availableNodes.RemoveAt(0);
            return true;
        }

        /// <summary>
        /// Gets an available node. Throws an exception if no node is available.
        /// </summary>
        public UnityEngine.Vector2 Sample()
        {
            UnityEngine.Vector2 sample;
            if (TrySample(out sample))
            {
                return sample;
            }

            throw new Exception("No avaliable nodes left!");
        }

        /// <summary>
        /// Tries to sample an amount of nodes. The result may be lower than <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">The amount of samples. The result may be lower.</param>
        public List<UnityEngine.Vector2> Sample(int amount)
        {
            List<UnityEngine.Vector2> nodes = _availableNodes.Take(amount).ToList();
            _availableNodes.RemoveAll(nodes.Contains);
            return nodes;
        }
    }
}
