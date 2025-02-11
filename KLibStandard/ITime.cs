using System;

namespace KLibStandard
{
    public interface ITime
    {
        /// <summary>
        /// time the start function is called.
        /// </summary>
        DateTime StartTime { get; }
        /// <summary>
        /// current frame time.
        /// </summary>
        DateTime FrameTime { get; }
        /// <summary>
        /// The total number of frames since the start.
        /// </summary>
        int FrameCount { get; }
        /// <summary>
        /// The time at the beginning of the current frame in seconds since the start.
        /// </summary>
        float Time { get; }
        /// <summary>
        /// The scale at which time passes.
        /// </summary>
        float ScaleDeltaTime
        {
            get;
            set;
        }
        /// <summary>
        /// The timeScale-independent interval in seconds from the last frame to the current one.
        /// </summary>
        float UnScaleDeltaTime { get; }
        /// <summary>
        /// The interval in seconds from the last frame to the current one.
        /// </summary>
        float DeltaTime { get; }
    }
}
