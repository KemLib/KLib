using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KLibStandard.Cryptography
{
    public class RsaCipher : ICipher, IDisposable
    {
        #region Properties
        public const int KEY_SIZE = 256,
            KEY_SIZE_MIN = 64,
            KEY_SIZE_MAX = 2048;
        public const RsaPaddingMode DEFAULT_PADDING_MODE = RsaPaddingMode.OaepSHA256;

        public readonly RsaPaddingMode Encryption;
        private readonly RSAEncryptionPadding encryptionPadding;
        private readonly RSA rsa;
        private bool isDispose;

        public CipherName Algorithm => CipherName.Rsa;
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
        public static bool TryCreate([NotNullWhen(true)] out RsaCipher rsa, RsaPaddingMode encryption = DEFAULT_PADDING_MODE)
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

        #region Private Key
        public byte[] ExportPrivateKey()
        {
            return rsa.ExportRSAPrivateKey();
        }
        public bool TryExportPrivateKey([NotNullWhen(true)] out byte[] privateKey)
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
        public bool TryExportPublicKey([NotNullWhen(true)] out byte[] publicKey)
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
        public ResultDecrypt Decrypt(byte[] ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                return ResultDecrypt.RESULT_EMPTY;
            //
            try
            {
                byte[] plaintext = rsa.Decrypt(ciphertext, encryptionPadding);
                return new ResultDecrypt(plaintext);
            }
            catch (Exception ex)
            {
                return new ResultDecrypt(ex.Message);
            }
        }
        public ResultDecrypt Decrypt(ReadOnlySpan<byte> ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                return ResultDecrypt.RESULT_EMPTY;
            //
            try
            {
                byte[] plaintext = rsa.Decrypt(ciphertext.ToArray(), encryptionPadding);
                return new ResultDecrypt(plaintext);
            }
            catch (Exception ex)
            {
                return new ResultDecrypt(ex.Message);
            }
        }
        public async Task<ResultDecrypt> DecryptAsync(byte[] ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                return ResultDecrypt.RESULT_EMPTY;
            //
            try
            {
                byte[] plaintext = rsa.Decrypt(ciphertext, encryptionPadding);
                return new ResultDecrypt(plaintext);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResultDecrypt(ex.Message));
            }
        }
        public async Task<ResultDecrypt> DecryptAsync(ReadOnlyMemory<byte> ciphertext)
        {
            if (ciphertext.Length == 0)
                return ResultDecrypt.RESULT_EMPTY;
            //
            try
            {
                byte[] plaintext = rsa.Decrypt(ciphertext.ToArray(), encryptionPadding);
                return new ResultDecrypt(plaintext);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResultDecrypt(ex.Message));
            }
        }
        public bool TryDecrypt(byte[] ciphertext, [NotNullWhen(true)] out byte[] plaintext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
            {
                plaintext = ICipher.DATA_EMPTY;
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
        public bool TryDecrypt(ReadOnlySpan<byte> ciphertext, [NotNullWhen(true)] out byte[] plaintext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
            {
                plaintext = ICipher.DATA_EMPTY;
                return true;
            }
            //
            try
            {
                plaintext = rsa.Decrypt(ciphertext.ToArray(), encryptionPadding);
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
        public ResultEncrypt Encrypt(byte[] plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
                return ResultEncrypt.RESULT_EMPTY;
            //
            try
            {
                byte[] ciphertext = rsa.Encrypt(plaintext, encryptionPadding);
                return new ResultEncrypt(ciphertext);
            }
            catch (Exception ex)
            {
                return new ResultEncrypt(ex.Message);
            }
        }
        public ResultEncrypt Encrypt(ReadOnlySpan<byte> plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
                return ResultEncrypt.RESULT_EMPTY;
            //
            try
            {
                byte[] ciphertext = rsa.Encrypt(plaintext.ToArray(), encryptionPadding);
                return new ResultEncrypt(ciphertext);
            }
            catch (Exception ex)
            {
                return new ResultEncrypt(ex.Message);
            }
        }
        public async Task<ResultEncrypt> EncryptAsync(byte[] plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
                return ResultEncrypt.RESULT_EMPTY;
            //
            try
            {
                byte[] ciphertext = rsa.Encrypt(plaintext, encryptionPadding);
                return new ResultEncrypt(ciphertext);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResultEncrypt(ex.Message));
            }
        }
        public async Task<ResultEncrypt> EncryptAsync(ReadOnlyMemory<byte> plaintext)
        {
            if (plaintext.Length == 0)
                return ResultEncrypt.RESULT_EMPTY;
            //
            try
            {
                byte[] ciphertext = rsa.Encrypt(plaintext.ToArray(), encryptionPadding);
                return new ResultEncrypt(ciphertext);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResultEncrypt(ex.Message));
            }
        }
        public bool TryEncrypt(byte[] plaintext, [NotNullWhen(true)] out byte[] ciphertext)
        {
            if (plaintext == null || plaintext.Length == 0)
            {
                ciphertext = ICipher.DATA_EMPTY;
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
        public bool TryEncrypt(ReadOnlySpan<byte> plaintext, [NotNullWhen(true)] out byte[] ciphertext)
        {
            if (plaintext == null || plaintext.Length == 0)
            {
                ciphertext = ICipher.DATA_EMPTY;
                return true;
            }
            //
            try
            {
                ciphertext = rsa.Encrypt(plaintext.ToArray(), encryptionPadding);
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
                RsaPaddingMode.Pkcs1 => RSAEncryptionPadding.Pkcs1,
                _ => RSAEncryptionPadding.OaepSHA256
            };
        }
        #endregion
    }
}
