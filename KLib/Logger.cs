namespace KLib
{
    public class Logger : ILogger
    {
        #region Properties
        private readonly Action<LogData> onLog;

        #endregion

        #region Construction
        public Logger(Action<LogData> onLog)
        {
            this.onLog = onLog;
        }
        #endregion

        #region Method
        public ILogger? CreateLogger()
        {
            return new Logger(onLog);
        }
        public void Log(LogData log)
        {
            onLog?.Invoke(log);
        }
        #endregion
    }
}
