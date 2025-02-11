namespace KLib
{
    public class Logger
    {
        #region Properties
        private readonly Action<LogData>? onLog;

        #endregion

        #region Construction
        public Logger(Action<LogData>? onLog)
        {
            this.onLog = onLog;
        }
        #endregion

        #region Method
        public virtual Logger? CreateLogger()
        {
            return new Logger(onLog);
        }
        public virtual void LogMessage(string? source = null, string? title = null, string? message = null)
        {
            LogData log = new(LogType.Message, source, title, message);
            try
            {
                onLog?.Invoke(log);
            }
            catch (Exception)
            {

            }
        }
        public virtual void LogWarning(string? source = null, string? title = null, string? message = null)
        {
            LogData log = new(LogType.Warning, source, title, message);
            try
            {
                onLog?.Invoke(log);
            }
            catch (Exception)
            {

            }
        }
        public virtual void LogError(string? source = null, string? title = null, string? message = null)
        {
            LogData log = new(LogType.Error, source, title, message);
            try
            {
                onLog?.Invoke(log);
            }
            catch (Exception)
            {

            }
        }
        public virtual void Log(LogData log)
        {
            try
            {
                onLog?.Invoke(log);
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
