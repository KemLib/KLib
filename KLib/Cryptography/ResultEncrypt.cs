namespace KLib.Cryptography
{
    public readonly struct ResultEncrypt : IResult
    {
        #region Properties
        internal static readonly ResultEncrypt RESULT_EMPTY = new(ICipher.DATA_EMPTY);

        private readonly bool isSuccess;
        private readonly string errorMessage;
        private readonly byte[] ciphertext;

        public bool IsSuccess => isSuccess;
        public string ErrorMessage => errorMessage;
        public byte[] CipherText => ciphertext;
        #endregion

        #region Construction
        public ResultEncrypt(string errorMessage)
        {
            isSuccess = false;
            this.errorMessage = string.IsNullOrEmpty(errorMessage) ? IResult.ERROR_MESSAGE_UNKNOWN : errorMessage;
            ciphertext = ICipher.DATA_EMPTY;
        }
        public ResultEncrypt(byte[] ciphertext)
        {
            isSuccess = true;
            errorMessage = string.Empty;
            this.ciphertext = ciphertext;
        }
        #endregion
    }
}
