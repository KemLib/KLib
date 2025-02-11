namespace KLibStandard
{
    public interface IResult
    {
        #region Properties
        const string ERROR_MESSAGE_UNKNOWN = "Unknown";

        /// <summary>
        /// True if successful, False if failed
        /// </summary>
        bool IsSuccess
        {
            get;
        }
        /// <summary>
        /// Get a message describing the current error.
        /// </summary>
        string ErrorMessage
        {
            get;
        }
        #endregion

        #region Method

        #endregion
    }
}
