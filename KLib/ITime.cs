namespace KLib
{
    public interface ITime
    {
        public DateTime StartTime { get; }
        public DateTime FrameTime { get; }
        public int FrameCount { get; }
        public float Time { get; }
        public float ScaleDeltaTime
        {
            get;
            set;
        }
        public float UnScaleDeltaTime { get; }
        public float DeltaTime { get; }
    }
}
