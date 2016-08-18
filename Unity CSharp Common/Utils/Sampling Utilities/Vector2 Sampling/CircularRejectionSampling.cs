using System;
using System.Collections.Generic;
using System.Linq;
using UnityCSCommon.Utils.Common;
using UnityCSCommon.Utils.RandomUtils;
using UnityEngine;

namespace UnityCSCommon.Utils.Sampling.Vector2Sampling
{
    /// <summary>
    /// Rejection sampling for a circular area.
    /// </summary>
    public class CircularRejectionSampling
    {
        private readonly Vector2 _origin;
        private readonly float _radius;
        private readonly int _seed;
        private readonly List<Vector2> _returnedSamples;

        private BetterRandom _random;

        /// <summary>
        /// Generates random Vector2s in a circular area with rejection sampling.
        /// </summary>
        /// <param name="origin">Origin of the circle.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="seed">Seed.</param>
        public CircularRejectionSampling(Vector2 origin, float radius, int seed)
        {
            _origin = origin;
            _radius = radius;
            _seed = seed;
            _random = new BetterRandom(seed);
            _returnedSamples = new List<Vector2>();
        }

        /// <summary>
        /// Clears returned samples' list.
        /// </summary>
        public void ResetSampleHistory()
        {
            _returnedSamples.Clear();
        }

        /// <summary>
        /// Re-initializes the internal random number generator.
        /// </summary>
        public void ResetRNG()
        {
            _random = new BetterRandom (_seed);
        }

        /// <summary>
        /// Tries to sample a single point with a minimum distance from previous samples.
        /// Returns true if a suitable sample is found, returns false otherwise.
        /// </summary>
        /// <param name="maxTrials">Maximum attempts to find a suitable sample.</param>
        /// <param name="minDist">Minimum distance from all previous samples.</param>
        /// <param name="sample">The sample.</param>
        /// <returns>True if a suitable sample is found, false otherwise.</returns>
        public bool TrySampleMin(int maxTrials, float minDist, out Vector2 sample)
        {
            for (var trials = 0; trials < maxTrials; trials++)
            {
                var rndVector2 = _random.Vector2.InCircle(_radius) + _origin;

                if (_returnedSamples.TrueForAll(a => a.TestDistanceMin(rndVector2, minDist)))
                {
                    sample = rndVector2;
                    _returnedSamples.Add (sample);
                    return true;
                }
            }

            sample = Vector2.zero;
            return false;
        }

        /// <summary>
        /// Tries to sample a single point with a maximum distance from at least one of the previous samples.
        /// Returns true if a suitable sample is found, returns false otherwise.
        /// </summary>
        /// <param name="maxTrials">Maximum attempts to find a suitable sample.</param>
        /// <param name="maxDist">Maximum distance from at least one of the previous samples.</param>
        /// <param name="sample">The sample.</param>
        /// <returns>True if a suitable sample is found, false otherwise.</returns>
        public bool TrySampleMax(int maxTrials, float maxDist, out Vector2 sample)
        {
            for (var trials = 0; trials < maxTrials; trials++)
            {
                var rndVector2 = _random.Vector2.InCircle(_radius) + _origin;

                if (_returnedSamples.Any(a => a.TestDistanceMax(rndVector2, maxDist)))
                {
                    sample = rndVector2;
                    _returnedSamples.Add(sample);
                    return true;
                }
            }

            sample = Vector2.zero;
            return false;
        }

        /// <summary>
        /// Tries to sample a single point with a minimum distance from all previous samples and a maximum distance from at least one of the previous samples.
        /// Returns true if a suitable sample is found, returns false otherwise.
        /// </summary>
        /// <param name="maxTrials">Maximum attempts to find a suitable sample.</param>
        /// <param name="minDist">Minimum distance from all previous samples.</param>
        /// <param name="maxDist">Maximum distance from at least one of the previous samples.</param>
        /// <param name="sample">The sample.</param>
        /// <returns>True if a suitable sample is found, false otherwise.</returns>
        public bool TrySampleMinMax(int maxTrials, float minDist, float maxDist, out Vector2 sample)
        {
            for (var trials = 0; trials < maxTrials; trials++)
            {
                var rndVector2 = _random.Vector2.InCircle(_radius) + _origin;

                if (_returnedSamples.Any(a => a.TestDistanceMax(rndVector2, maxDist))) //max
                {
                    if (_returnedSamples.TrueForAll(a => a.TestDistanceMin(rndVector2, minDist))) //min
                    {
                        sample = rndVector2;
                        _returnedSamples.Add(sample);
                        return true;
                    }
                }
            }

            sample = Vector2.zero;
            return false;
        }

        /// <summary>
        /// Samples a single point with a minimum distance from previous samples.
        /// If no sample found, throws an exception.
        /// </summary>
        /// <param name="maxTrials">Maximum attempts to find a suitable sample.</param>
        /// <param name="minDist">Minimum distance from all previous samples.</param>
        public Vector2 SampleMin(int maxTrials, float minDist)
        {
            Vector2 sample;
            if (TrySampleMin(maxTrials, minDist, out sample))
            {
                //The addition to returned samples is done in TrySampleMin method.
                return sample;
            }

            throw new Exception("None of the sample trials were meeting conditions.");
        }

        /// <summary>
        /// Samples a single point with a maximum distance from at least one of the previous samples.
        /// If no sample found, throws an exception.
        /// </summary>
        /// <param name="maxTrials">Maximum attempts to find a suitable sample.</param>
        /// <param name="maxDist">Maximum distance from at least one of the previous samples.</param>
        public Vector2 SampleMax(int maxTrials, float maxDist)
        {
            Vector2 sample;
            if (TrySampleMax(maxTrials, maxDist, out sample))
            {
                //The addition to returned samples is done in TrySampleMax method.
                return sample;
            }

            throw new Exception("None of the sample trials were meeting conditions.");
        }

        /// <summary>
        /// Samples a single point with a minimum distance from all previous samples and a maximum distance from at least one of the previous samples.
        /// If no sample found, throws an exception.
        /// </summary>
        /// <param name="maxTrials">Maximum attempts to find a suitable sample.</param>
        /// <param name="minDist">Minimum distance from all previous samples.</param>
        /// <param name="maxDist">Maximum distance from at least one of the previous samples.</param>
        public Vector2 SampleMinMax(int maxTrials, float minDist, float maxDist)
        {
            Vector2 sample;
            if (TrySampleMinMax(maxTrials, minDist, maxDist, out sample))
            {
                //The addition to returned samples is done in TrySampleMinMax method.
                return sample;
            }

            throw new Exception("None of the sample trials were meeting conditions.");
        }

        /// <summary>
        /// Tries to sample an amount of samples with each one from a minimum distance from all previous samples.
        /// The result may be lower than <paramref name="amount"/> parameter.
        /// </summary>
        /// <param name="amount">The amount of samples . The result may be lower than this number.</param>
        /// <param name="maxTrialsEach">Maximum attempts to find a suitable sample for each try.</param>
        /// <param name="minDist">Minimum distance of each sample from all previous samples.</param>
        public List<Vector2> SampleMin (int amount, int maxTrialsEach, float minDist)
        {
            List<Vector2> samples = new List<Vector2>();
            for (var i = 0; i < amount; i++)
            {
                Vector2 sample;
                if (TrySampleMin (maxTrialsEach, minDist, out sample))
                {
                    //The addition to returned samples is done in TrySampleMin method.
                    samples.Add (sample);
                }
            }

            return samples;
        }

        /// <summary>
        /// Tries to sample an amount of samples with each one from a maximum distance from at least one previous sample.
        /// The result may be lower than <paramref name="amount"/> parameter.
        /// </summary>
        /// <param name="amount">The amount of samples. The result may be lower than this number.</param>
        /// <param name="maxTrialsEach">Maximum attempts to find a suitable sample for each try.</param>
        /// <param name="maxDist">Maximum distance of each sample from at least one of the previous samples.</param>
        public List<Vector2> SampleMax (int amount, int maxTrialsEach, float maxDist)
        {
            List<Vector2> samples = new List<Vector2>();
            for (var i = 0; i < amount; i++)
            {
                Vector2 sample;
                if (TrySampleMax (maxTrialsEach, maxDist, out sample))
                {
                    //The addition to returned samples is done in TrySampleMax method.
                    samples.Add (sample);
                }
            }

            return samples;
        }

        /// <summary>
        /// Tries to sample an amount of samples with each one from a minimum distance from all previous samples and a maximum
        /// distance from at least one previous sample. The result may be lower than <paramref name="amount"/> parameter.
        /// </summary>
        /// <param name="amount">The amount of samples. The result may be lower than this number.</param>
        /// <param name="maxTrialsEach">Maximum attempts to find a suitable sample for each try.</param>
        /// <param name="minDist">Minimum distance of each sample from all previous samples.</param>
        /// <param name="maxDist">Maximum distance of each sample from at least one of the previous samples.</param>
        public List<Vector2> SampleMinMax (int amount, int maxTrialsEach, float minDist, float maxDist)
        {
            List<Vector2> samples = new List<Vector2>();
            for (var i = 0; i < amount; i++)
            {
                Vector2 sample;
                if (TrySampleMinMax (maxTrialsEach, minDist, maxDist, out sample))
                {
                    //The addition to returned samples is done in TrySampleMinMax method.
                    samples.Add (sample);
                }
            }

            _returnedSamples.AddRange(samples);
            return samples;
        }
    }
}