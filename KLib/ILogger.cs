namespace KLib
{
    public interface ILogger
    {
        public ILogger? CreateLogger();
        public void LogMessage(string? source = null, string? title = null, string? message = null)
        {
            LogData log = new(LogType.Message, source, title, message);
            try
            {
                Log(log);
            }
            catch (Exception)
            {

            }
        }
        public void LogWarning(string? source = null, string? title = null, string? message = null)
        {
            LogData log = new(LogType.Warning, source, title, message);
            try
            {
                Log(log);
            }
            catch (Exception)
            {

            }
        }
        public void LogError(string? source = null, string? title = null, string? message = null)
        {
            LogData log = new(LogType.Error, source, title, message);
            try
            {
                Log(log);
            }
            catch (Exception)
            {

            }
        }
        public void Log(LogData log);
    }
}
