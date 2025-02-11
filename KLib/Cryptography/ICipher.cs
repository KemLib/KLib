namespace KLib.Cryptography
{
    public interface ICipher
    {
        #region Properties
        internal static readonly byte[] DATA_EMPTY = [];
        /// <summary>
        /// Cipher Algorithm Name.
        /// </summary>
        public CipherName Algorithm
        {
            get;
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        public ResultDecrypt Decrypt(byte[]? ciphertext);
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        public ResultDecrypt Decrypt(ReadOnlySpan<byte> ciphertext);
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        public Task<ResultDecrypt> DecryptAsync(byte[]? ciphertext);
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        public Task<ResultDecrypt> DecryptAsync(ReadOnlyMemory<byte> ciphertext);
        /// <summary>
        /// Try decrypts ciphertext.
        /// </summary>
        public bool TryDecrypt(byte[]? ciphertext, [NotNullWhen(true)] out byte[]? plaintext);
        /// <summary>
        /// Try decrypts ciphertext.
        /// </summary>
        public bool TryDecrypt(ReadOnlySpan<byte> ciphertext, [NotNullWhen(true)] out byte[]? plaintext);
        #endregion

        #region Encrypt
        /// <summary>
        /// Encrypt plaintext.
        /// </summary>
        public ResultEncrypt Encrypt(byte[]? plaintext);
        /// <summary>
        /// Encrypt plaintext.
        /// </summary>
        public ResultEncrypt Encrypt(ReadOnlySpan<byte> plaintext);
        /// <summary>
        /// Encrypt plaintext.
        /// </summary>
        public Task<ResultEncrypt> EncryptAsync(byte[]? plaintext);
        /// <summary>
        /// Encrypt plaintext.
        /// </summary>
        public Task<ResultEncrypt> EncryptAsync(ReadOnlyMemory<byte> plaintext);
        /// <summary>
        /// Try encrypt plaintext.
        /// </summary>
        public bool TryEncrypt(byte[]? plaintext, [NotNullWhen(true)] out byte[]? ciphertext);
        /// <summary>
        /// Try encrypt plaintext.
        /// </summary>
        public bool TryEncrypt(ReadOnlySpan<byte> plaintext, [NotNullWhen(true)] out byte[]? ciphertext);
        #endregion
    }
}
