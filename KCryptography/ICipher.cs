namespace KCryptography
{
    public interface ICipher
    {
        #region Properties
        public CipherAlgorithmName AlgorithmName
        {
            get;
        }
        #endregion

        #region Decrypt
        public byte[] Decrypt(byte[]? ciphertext);
        public byte[] Decrypt(ReadOnlySpan<byte> ciphertext);
        public Task<byte[]> DecryptAsync(byte[]? ciphertext);
        public Task<byte[]> DecryptAsync(ReadOnlyMemory<byte> ciphertext);
        public bool TryDecrypt(byte[]? ciphertext, [NotNullWhen(true)] out byte[]? plaintext);
        public bool TryDecrypt(ReadOnlySpan<byte> ciphertext, [NotNullWhen(true)] out byte[]? plaintext);
        #endregion

        #region Encrypt
        public byte[] Encrypt(byte[]? plaintext);
        public byte[] Encrypt(ReadOnlySpan<byte> plaintext);
        public Task<byte[]> EncryptAsync(byte[]? plaintext);
        public Task<byte[]> EncryptAsync(ReadOnlyMemory<byte> plaintext);
        public bool TryEncrypt(byte[]? plaintext, [NotNullWhen(true)] out byte[]? ciphertext);
        public bool TryEncrypt(ReadOnlySpan<byte> plaintext, [NotNullWhen(true)] out byte[]? ciphertext);
        #endregion
    }
}
