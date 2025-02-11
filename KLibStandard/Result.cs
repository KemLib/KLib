namespace KLibStandard
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
        public Result(bool isSuccess, string error)
        {
            this.isSuccess = isSuccess;
            if (isSuccess)
                this.error = string.Empty;
            else
                this.error = (string.IsNullOrEmpty(error) ? IResult.ERROR_MESSAGE_UNKNOWN : error);
        }
        #endregion

        #region Method
        public static Result Success()
        {
            return new Result(true, string.Empty);
        }
        public static Result Failure(string error) 
        { 
            return new Result(false, error);
        }
        #endregion
    }
}
