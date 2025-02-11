using System;

namespace KLibStandard
{
    public interface ITime
    {
        /// <summary>
        /// time the start function is called.
        /// </summary>
        public DateTime StartTime { get; }
        /// <summary>
        /// current frame time.
        /// </summary>
        public DateTime FrameTime { get; }
        /// <summary>
        /// The total number of frames since the start.
        /// </summary>
        public int FrameCount { get; }
        /// <summary>
        /// The time at the beginning of the current frame in seconds since the start.
        /// </summary>
        public float Time { get; }
        /// <summary>
        /// The scale at which time passes.
        /// </summary>
        public float ScaleDeltaTime
        {
            get;
            set;
        }
        /// <summary>
        /// The timeScale-independent interval in seconds from the last frame to the current one.
        /// </summary>
        public float UnScaleDeltaTime { get; }
        /// <summary>
        /// The interval in seconds from the last frame to the current one.
        /// </summary>
        public float DeltaTime { get; }
    }
}
