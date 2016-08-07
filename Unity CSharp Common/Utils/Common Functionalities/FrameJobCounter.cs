using UnityEngine;

namespace UnityCSharpCommon.Utils.Common
{
    /// <summary>
    /// A job counter for jobs that has limits per frame.
    /// </summary>
    public class FrameJobCounter
    {
        private int _lastFrame;

        public int JobLimit { get; private set; }
        public int JobCount { get; private set; }

        /// <summary>
        /// Indicates if job limit is 'not' reached for this frame.
        /// </summary>
        public bool CanWork
        {
            get
            {
                // Get the current frame number.
                int currentFrame = Time.frameCount;

                // If the last frame number and current frame number are different (which means at least one frame has been passed)...
                if (_lastFrame != currentFrame)
                {
                    // Reset the job counter.
                    JobCount = 0;

                    // Save the frame number.
                    _lastFrame = currentFrame;
                }

                // Return if job limit is *not* reached.
                return JobCount < JobLimit;
            }
        }

        public FrameJobCounter(int jobLimit)
        {
            JobLimit = jobLimit;
            JobCount = 0;
        }

        /// <summary>
        /// Call when a job is finished.
        /// </summary>
        public void JobDone()
        {
            JobCount++;
        }
    }
}