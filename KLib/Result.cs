namespace KLib
{
    public readonly struct Result : IResult
    {
        #region Properties
        private readonly bool isSuccess;
        private readonly string error;

        public bool IsSuccess => isSuccess;
        public string ErrorMessage => error;
        #endregion

        #region Construction
        public Result(string? error)
        {
            isSuccess = false;
            this.error = (string.IsNullOrEmpty(error) ? IResult.ERROR_MESSAGE_UNKNOWN : error);
        }
        public Result()
        {
            isSuccess = true;
            error = string.Empty;
        }
        #endregion
    }
}
