namespace KLibStandard
{
    public interface IResult
    {
        #region Properties
        public const string ERROR_MESSAGE_UNKNOWN = "Unknown";

        public bool IsSuccess
        {
            get;
        }
        public string ErrorMessage
        {
            get;
        }
        #endregion

        #region Method

        #endregion
    }
}
