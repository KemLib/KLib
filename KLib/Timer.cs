namespace KLib
{
    public class Timer : ITime
    {
        #region Properties
        private DateTime startTime,
            frameTime;
        private int frameCount;
        private float time;
        private float scaleDeltaTime,
            unscaleDeltaTime,
            deltaTime;

        public DateTime StartTime => startTime;
        public DateTime FrameTime => frameTime;
        public int FrameCount => frameCount;
        public float Time => time;
        public float ScaleDeltaTime
        {
            get => scaleDeltaTime;
            set => scaleDeltaTime = Math.Max(0, value);
        }
        public float UnScaleDeltaTime => unscaleDeltaTime;
        public float DeltaTime => deltaTime;
        #endregion

        #region Construction
        public Timer()
        {
            Start();
        }
        #endregion

        #region Method
        /// <summary>
        /// Start is called at the beginning.
        /// </summary>
        public void Start()
        {
            startTime = DateTime.Now;
            frameTime = startTime;
            frameCount = 0;
            time = 0;
            ScaleDeltaTime = 1;
            unscaleDeltaTime = 0;
            deltaTime = unscaleDeltaTime * ScaleDeltaTime;
        }
        /// <summary>
        /// Update is called every frame.
        /// </summary>
        public void Update()
        {
            DateTime currentTime = DateTime.Now;
            frameCount++;
            time = (float)(currentTime - startTime).TotalSeconds;
            unscaleDeltaTime = (float)((currentTime - frameTime).TotalSeconds);
            deltaTime = unscaleDeltaTime * ScaleDeltaTime;
            frameTime = currentTime;
        }
        /// <summary>
        /// Stop is called at the ending.
        /// </summary>
        public void Stop()
        {
            Update();
        }
        #endregion
    }
}
