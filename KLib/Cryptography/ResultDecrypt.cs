using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib.Cryptography
{
    public readonly struct ResultDecrypt : IResult
    {
        #region Properties
        internal static readonly ResultDecrypt RESULT_EMPTY = new(ICipher.DATA_EMPTY);

        private readonly bool isSuccess;
        private readonly string errorMessage;
        private readonly byte[] plaintext;

        public bool IsSuccess => isSuccess;
        public string ErrorMessage => errorMessage;
        public byte[] CipherText => plaintext;
        #endregion

        #region Construction
        public ResultDecrypt(string errorMessage)
        {
            isSuccess = false;
            this.errorMessage = string.IsNullOrEmpty(errorMessage) ? IResult.ERROR_MESSAGE_UNKNOWN : errorMessage;
            plaintext = ICipher.DATA_EMPTY;
        }
        public ResultDecrypt(byte[] plaintext)
        {
            isSuccess = true;
            errorMessage = string.Empty;
            this.plaintext = plaintext;
        }
        #endregion
    }
}
