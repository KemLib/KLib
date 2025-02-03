namespace KLib
{
    public readonly struct LogData
    {
        #region Properties
        public const string LOG_FORMAT = "[{0}][{1}]",
            LOG_FORMAT_MESSAGE = "[{0}][{1}] {2}",
            LOG_FORMAT_TITLE = "[{0}][{1}] {2}",
            LOG_FORMAT_TITLE_MESSAGE = "[{0}][{1}] {2}: {3}",
            LOG_FORMAT_SOURCE = "[{0}][{1}][{2}]",
            LOG_FORMAT_SOURCE_MESSAGE = "[{0}][{1}][{2}] {3}}",
            LOG_FORMAT_SOURCE_TITLE = "[{0}][{1}][{2}] {3}",
            LOG_FORMAT_SOURCE_TITLE_MESSAGE = "[{0}][{1}][{2}] {3}: {4}";
        private const string TIME_CULTURE_INFO = "dd/MM/yyyy HH:mm:ss.fff";

        public readonly LogType Type;
        public readonly string Source,
            Message,
            Title;
        public readonly DateTime Time;
        #endregion

        #region Construction
        public LogData(LogType type, string? source = null, string? title = null, string? message = null)
        {
            Type = type;
            Source = source ?? string.Empty;
            Title = title ?? string.Empty;
            Message = message ?? string.Empty;
            Time = DateTime.Now;
        }
        #endregion

        #region Method
        public override readonly string ToString()
        {
            if (string.IsNullOrEmpty(Source))
            {
                if (string.IsNullOrEmpty(Title))
                {
                    if (string.IsNullOrEmpty(Message))
                        return string.Format(LOG_FORMAT, Time.ToString(TIME_CULTURE_INFO), Type);
                    else
                        return string.Format(LOG_FORMAT_MESSAGE, Time.ToString(TIME_CULTURE_INFO), Type, Message);
                }
                else
                {
                    if (string.IsNullOrEmpty(Message))
                        return string.Format(LOG_FORMAT_TITLE, Time.ToString(TIME_CULTURE_INFO), Type, Title);
                    else
                        return string.Format(LOG_FORMAT_TITLE_MESSAGE, Time.ToString(TIME_CULTURE_INFO), Type, Title, Message);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Title))
                {
                    if (string.IsNullOrEmpty(Message))
                        return string.Format(LOG_FORMAT_SOURCE, Time.ToString(TIME_CULTURE_INFO), Type, Source);
                    else
                        return string.Format(LOG_FORMAT_SOURCE_MESSAGE, Time.ToString(TIME_CULTURE_INFO), Type, Source, Message);
                }
                else
                {
                    if (string.IsNullOrEmpty(Message))
                        return string.Format(LOG_FORMAT_SOURCE_TITLE, Time.ToString(TIME_CULTURE_INFO), Type, Source, Title);
                    else
                        return string.Format(LOG_FORMAT_SOURCE_TITLE_MESSAGE, Time.ToString(TIME_CULTURE_INFO), Type, Source, Title, Message);
                }
            }
        }
        #endregion
    }
}
