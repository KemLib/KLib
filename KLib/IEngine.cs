namespace KLib
{
    public interface IEngine
    {
        #region Properties
        public bool IsRun
        {
            get;
        }
        public bool IsStopping
        {
            get;
        }
        public bool IsDisposed
        {
            get;
        }
        #endregion

        #region Method
        public void Starting();
        public void Stopping();
        public void Stopped();
        public Task StoppedAsync();

        public Result Start();
        public void Update();
        public Task<Result> StartAsync();
        public Task UpdateAsync();
        #endregion
    }
}
