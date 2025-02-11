using System;

namespace KLibStandard
{
    public class Logger
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
        public Logger CreateLogger()
        {
            return new Logger(onLog);
        }
        public void LogMessage(string source = null, string title = null, string message = null)
        {
            LogData log = new LogData(LogType.Message, source, title, message);
            try
            {
                onLog?.Invoke(log);
            }
            catch (Exception)
            {

            }
        }
        public void LogWarning(string source = null, string title = null, string message = null)
        {
            LogData log = new LogData(LogType.Warning, source, title, message);
            try
            {
                onLog?.Invoke(log);
            }
            catch (Exception)
            {

            }
        }
        public void LogError(string source = null, string title = null, string message = null)
        {
            LogData log = new LogData(LogType.Error, source, title, message);
            try
            {
                onLog?.Invoke(log);
            }
            catch (Exception)
            {

            }
        }
        public void Log(LogData log)
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
