namespace KLib
{
    public interface IResult
    {
        #region Properties
        const string ERROR_MESSAGE_UNKNOWN = "Unknown";

        bool IsSuccess
        {
            get;
        }
        string ErrorMessage
        {
            get;
        }
        #endregion

        #region Method

        #endregion
    }
}
