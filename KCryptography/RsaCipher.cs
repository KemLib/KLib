using System.Security.Cryptography;

namespace KCryptography
{
    public class RsaCipher : ICipher, IDisposable
    {
        #region Properties
        public const int KEY_SIZE = 256,
            KEY_SIZE_MIN = 64,
            KEY_SIZE_MAX = 2048;
        public const RsaPaddingMode DEFAULT_PADDING_MODE = RsaPaddingMode.OaepSHA256;
        private static readonly byte[] ARRAY_EMPTY = [];

        public readonly RsaPaddingMode Encryption;
        private readonly RSAEncryptionPadding encryptionPadding;
        private readonly RSA rsa;
        private bool isDispose;

        public CipherAlgorithmName AlgorithmName => CipherAlgorithmName.Rsa;
        public bool IsDispose => isDispose;
        #endregion

        #region Construction
        public RsaCipher(RsaPaddingMode encryption = DEFAULT_PADDING_MODE)
        {
            Encryption = encryption;
            encryptionPadding = Convert(encryption);
            rsa = RSA.Create();
            isDispose = false;
        }
        ~RsaCipher()
        {
            Dispose(false);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (isDispose)
                return;
            isDispose = true;
            //
            if (disposing)
            {

            }
            //
            try
            {
                rsa.Clear();
            }
            catch (Exception)
            {

            }
            finally
            {
                rsa.Dispose();
            }
        }
        #endregion

        #region Method
        public static bool TryCreate([NotNullWhen(true)] out RsaCipher? rsa, RsaPaddingMode encryption = DEFAULT_PADDING_MODE)
        {
            try
            {
                rsa = new RsaCipher(encryption);
                return true;
            }
            catch (Exception)
            {
                rsa = null;
                return false;
            }
        }
        #endregion

        #region Key Pem
        public string ExportPrivateKeyPem()
        {
            return rsa.ExportRSAPrivateKeyPem();
        }
        public bool TryExportPrivateKeyPem([NotNullWhen(true)] out string? privateKeyPem)
        {
            try
            {
                privateKeyPem = rsa.ExportRSAPrivateKeyPem();
                return true;
            }
            catch (Exception)
            {
                privateKeyPem = null;
                return false;
            }
        }
        public string ExportPublicKeyPem()
        {
            return rsa.ExportRSAPublicKeyPem();
        }
        public bool TryExportPublicKeyPem([NotNullWhen(true)] out string? publicKeyPem)
        {
            try
            {
                publicKeyPem = rsa.ExportRSAPublicKeyPem();
                return true;
            }
            catch (Exception)
            {
                publicKeyPem = null;
                return false;
            }
        }
        public void ImportFromPem(ReadOnlySpan<char> keyPem)
        {
            rsa.ImportFromPem(keyPem);
        }
        public bool TryImportFromPem(ReadOnlySpan<char> keyPem)
        {
            try
            {
                rsa.ImportFromPem(keyPem);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Private Key
        public byte[] ExportPrivateKey()
        {
            return rsa.ExportRSAPrivateKey();
        }
        public bool TryExportPrivateKey([NotNullWhen(true)] out byte[]? privateKey)
        {
            try
            {
                privateKey = rsa.ExportRSAPrivateKey();
                return true;
            }
            catch (Exception)
            {
                privateKey = null;
                return false;
            }
        }
        public void ImportPrivateKey(ReadOnlySpan<byte> privateKey, out int bytesRead)
        {
            rsa.ImportRSAPrivateKey(privateKey, out bytesRead);
        }
        public bool TryImportPrivateKey(ReadOnlySpan<byte> privateKey, out int bytesRead)
        {
            try
            {
                rsa.ImportRSAPrivateKey(privateKey, out bytesRead);
                return true;
            }
            catch (Exception)
            {
                bytesRead = 0;
                return false;
            }
        }
        #endregion

        #region Public Key
        public byte[] ExportPublicKey()
        {
            return rsa.ExportRSAPublicKey();
        }
        public bool TryExportPublicKey([NotNullWhen(true)] out byte[]? publicKey)
        {
            try
            {
                publicKey = rsa.ExportRSAPublicKey();
                return true;
            }
            catch (Exception)
            {
                publicKey = null;
                return false;
            }
        }
        public void ImportPublicKey(ReadOnlySpan<byte> publicKey, out int bytesRead)
        {
            rsa.ImportRSAPublicKey(publicKey, out bytesRead);
        }
        public bool TryImportPublicKey(ReadOnlySpan<byte> publicKey, out int bytesRead)
        {
            try
            {
                rsa.ImportRSAPublicKey(publicKey, out bytesRead);
                return true;
            }
            catch (Exception)
            {
                bytesRead = 0;
                return false;
            }
        }
        #endregion

        #region Decrypt
        public byte[] Decrypt(byte[]? ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                return ARRAY_EMPTY;
            //
            return rsa.Decrypt(ciphertext, encryptionPadding);
        }
        public byte[] Decrypt(ReadOnlySpan<byte> ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                return ARRAY_EMPTY;
            //
            return rsa.Decrypt(ciphertext, encryptionPadding);
        }
        public Task<byte[]> DecryptAsync(byte[]? ciphertext)
        {
            byte[] plaintext;
            if (ciphertext == null || ciphertext.Length == 0)
                plaintext = ARRAY_EMPTY;
            else
                plaintext = rsa.Decrypt(ciphertext, encryptionPadding);
            return Task.FromResult(plaintext);
        }
        public Task<byte[]> DecryptAsync(ReadOnlyMemory<byte> ciphertext)
        {
            byte[] plaintext;
            if (ciphertext.Length == 0)
                plaintext = ARRAY_EMPTY;
            else
                plaintext = rsa.Decrypt(ciphertext.Span, encryptionPadding);
            return Task.FromResult(plaintext);
        }
        public bool TryDecrypt(byte[]? ciphertext, [NotNullWhen(true)] out byte[]? plaintext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
            {
                plaintext = ARRAY_EMPTY;
                return true;
            }
            //
            try
            {
                plaintext = rsa.Decrypt(ciphertext, encryptionPadding);
                return true;
            }
            catch (Exception)
            {
                plaintext = null;
                return false;
            }
        }
        public bool TryDecrypt(ReadOnlySpan<byte> ciphertext, [NotNullWhen(true)] out byte[]? plaintext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
            {
                plaintext = ARRAY_EMPTY;
                return true;
            }
            //
            try
            {
                plaintext = rsa.Decrypt(ciphertext, encryptionPadding);
                return true;
            }
            catch (Exception)
            {
                plaintext = null;
                return false;
            }
        }
        #endregion

        #region Encrypt
        public byte[] Encrypt(byte[]? plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
                return ARRAY_EMPTY;
            //
            return rsa.Encrypt(plaintext, encryptionPadding);
        }
        public byte[] Encrypt(ReadOnlySpan<byte> plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
                return ARRAY_EMPTY;
            //
            return rsa.Encrypt(plaintext, encryptionPadding);
        }
        public Task<byte[]> EncryptAsync(byte[]? plaintext)
        {
            byte[] ciphertext;
            if (plaintext == null || plaintext.Length == 0)
                ciphertext = ARRAY_EMPTY;
            else
                ciphertext = rsa.Encrypt(plaintext, encryptionPadding);
            return Task.FromResult(ciphertext);
        }
        public Task<byte[]> EncryptAsync(ReadOnlyMemory<byte> plaintext)
        {
            byte[] ciphertext;
            if (plaintext.Length == 0)
                ciphertext = ARRAY_EMPTY;
            else
                ciphertext = rsa.Encrypt(plaintext.Span, encryptionPadding);
            return Task.FromResult(ciphertext);
        }
        public bool TryEncrypt(byte[]? plaintext, [NotNullWhen(true)] out byte[]? ciphertext)
        {
            if (plaintext == null || plaintext.Length == 0)
            {
                ciphertext = ARRAY_EMPTY;
                return true;
            }
            //
            try
            {
                ciphertext = rsa.Encrypt(plaintext, encryptionPadding);
                return true;
            }
            catch (Exception)
            {
                ciphertext = null;
                return false;
            }
        }
        public bool TryEncrypt(ReadOnlySpan<byte> plaintext, [NotNullWhen(true)] out byte[]? ciphertext)
        {
            if (plaintext == null || plaintext.Length == 0)
            {
                ciphertext = ARRAY_EMPTY;
                return true;
            }
            //
            try
            {
                ciphertext = rsa.Encrypt(plaintext, encryptionPadding);
                return true;
            }
            catch (Exception)
            {
                ciphertext = null;
                return false;
            }
        }
        #endregion

        #region Convert
        public static RSAEncryptionPadding Convert(RsaPaddingMode encryptionPadding)
        {
            return encryptionPadding switch
            {
                RsaPaddingMode.OaepSHA1 => RSAEncryptionPadding.OaepSHA1,
                RsaPaddingMode.OaepSHA256 => RSAEncryptionPadding.OaepSHA256,
                RsaPaddingMode.OaepSHA384 => RSAEncryptionPadding.OaepSHA384,
                RsaPaddingMode.OaepSHA512 => RSAEncryptionPadding.OaepSHA512,
                RsaPaddingMode.OaepSHA3_256 => RSAEncryptionPadding.OaepSHA3_256,
                RsaPaddingMode.OaepSHA3_384 => RSAEncryptionPadding.OaepSHA3_384,
                RsaPaddingMode.OaepSHA3_512 => RSAEncryptionPadding.OaepSHA3_512,
                RsaPaddingMode.Pkcs1 => RSAEncryptionPadding.Pkcs1,
                _ => RSAEncryptionPadding.OaepSHA256
            };
        }
        #endregion
    }
}
