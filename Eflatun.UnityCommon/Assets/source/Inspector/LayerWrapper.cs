using System;
using UnityEngine;

namespace starikcetin.Eflatun.UnityCommon.Inspector
{
    /// <summary>
    /// Wraps around an integer field to represent an EXISTING Unity Layer. <para/>
    /// The property drawer draws a drop-down list of all available layers to choose from.
    /// </summary>
    [System.Serializable]
    public struct LayerWrapper
    {
        [SerializeField] private int _layerIndex;

        /// <summary>
        /// Index of the Layer. (Unity Layers are in range of 0-31 [both inclusive])
        /// </summary>
        public int Index
        {
            get { return _layerIndex; }
        }

        /// <summary>
        /// Name of Layer.
        /// </summary>
        public string Name
        {
            get { return LayerMask.LayerToName(_layerIndex); }
        }

        /// <summary>
        /// Returns a Layer mask that includes only this layer.
        /// </summary>
        public int AsMask
        {
            get { return 1 << _layerIndex; }
        }

        /// <summary>
        /// Returns a Layer mask that includes all layers but this one.
        /// </summary>
        public int AsMaskReverse
        {
            get { return ~AsMask; }
        }

        /// <summary>
        /// Wraps around an integer field to represent an EXISTING Unity Layer. <para/>
        /// The property drawer draws a drop-down list of available layers to choose from.
        /// </summary>
        public LayerWrapper(int layerIndex)
        {
            _layerIndex = layerIndex;
        }

        /// <summary>
        /// Set with layer index. Will throw an exception if layer is not in range of 0-31 (both inclusive).
        /// </summary>
        public void Set(int layerIndex)
        {
            if (layerIndex < 0 || layerIndex > 31)
            {
                throw new ArgumentOutOfRangeException("layerIndex", layerIndex,
                    "A Unity Layer index must be in range of 0-31 (both inclusive).");
            }

            _layerIndex = layerIndex;
        }

        /// <summary>
        /// Set with layer name. Will throw an exception if no layer found with given name.
        /// </summary>
        public void Set(string layerName)
        {
            var layerIndex = LayerMask.NameToLayer(layerName);

            if (layerIndex < 0 || layerIndex > 31)
            {
                throw new ArgumentOutOfRangeException("layerName", layerName, "No layer found with given name.");
            }

            _layerIndex = layerIndex;
        }
    }
}
