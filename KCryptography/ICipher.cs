namespace KCryptography
{
    public interface ICipher
    {
        #region Properties
        /// <summary>
        /// Cipher Algorithm Name.
        /// </summary>
        public CipherAlgorithmName AlgorithmName
        {
            get;
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        public byte[] Decrypt(byte[]? ciphertext);
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        public byte[] Decrypt(ReadOnlySpan<byte> ciphertext);
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        public Task<byte[]> DecryptAsync(byte[]? ciphertext);
        /// <summary>
        /// Decrypts ciphertext.
        /// </summary>
        public Task<byte[]> DecryptAsync(ReadOnlyMemory<byte> ciphertext);
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
        public byte[] Encrypt(byte[]? plaintext);
        /// <summary>
        /// Encrypt plaintext.
        /// </summary>
        public byte[] Encrypt(ReadOnlySpan<byte> plaintext);
        /// <summary>
        /// Encrypt plaintext.
        /// </summary>
        public Task<byte[]> EncryptAsync(byte[]? plaintext);
        /// <summary>
        /// Encrypt plaintext.
        /// </summary>
        public Task<byte[]> EncryptAsync(ReadOnlyMemory<byte> plaintext);
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
