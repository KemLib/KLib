using System.Security.Cryptography;

namespace KCryptography
{
    public class AesCipher : ICipher, IDisposable
    {
        #region Properties
        public const int BLOCK_SIZE = 16,
            BLOCK_SIZE_MIN = 16,
            BLOCK_SIZE_MAX = 16,
            KEY_SIZE_MIN = 16,
            KEY_SIZE_MAX = 32,
            IV_SIZE = 16;
        public const AesCipherMode DEFAULT_CIPHER_MODE = AesCipherMode.CBC;
        public const AesPaddingMode DEFAULT_PADDING_MODE = AesPaddingMode.PKCS7;
        public const int DEFAULT_KEY_SIZE = KEY_SIZE_MIN;
        private static readonly byte[] ARRAY_EMPTY = [];

        public readonly AesCipherMode Cipher;
        public readonly AesPaddingMode Padding;
        private readonly CipherMode cipherMode;
        private readonly PaddingMode paddingMode;
        private readonly Aes aes;
        private bool isDispose;

        public CipherAlgorithmName AlgorithmName => CipherAlgorithmName.Aes;
        public bool IsDispose => isDispose;
        public byte[] Key => aes.Key;
        public byte[] Iv => aes.IV;
        #endregion

        #region Construction
        public AesCipher(AesCipherMode cipher = DEFAULT_CIPHER_MODE, AesPaddingMode padding = DEFAULT_PADDING_MODE)
        {
            Cipher = cipher;
            Padding = padding;
            aes = Aes.Create();
            isDispose = false;
            //
            cipherMode = aes.Mode = Convert(cipher);
            paddingMode = aes.Padding = Convert(padding);
        }
        ~AesCipher()
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
                aes.Clear();
            }
            catch (Exception)
            {

            }
            finally
            {
                aes.Dispose();
            }
        }
        #endregion

        #region Method
        public static bool TryCreate([NotNullWhen(true)] out AesCipher? aes, AesCipherMode cipher = DEFAULT_CIPHER_MODE, AesPaddingMode padding = DEFAULT_PADDING_MODE)
        {
            try
            {
                aes = new AesCipher(cipher, padding);
                return true;
            }
            catch (Exception)
            {
                aes = null;
                return false;
            }
        }
        #endregion

        #region Key
        public void SetKey(byte[] key)
        {
            aes.Key = key;
        }
        public bool TrySetKey(byte[] key)
        {
            try
            {
                aes.Key = key;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void SetIv(byte[] iv)
        {
            aes.IV = iv;
        }
        public bool TrySetIv(byte[] iv)
        {
            try
            {
                aes.IV = iv;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void GenerateIV()
        {
            aes.GenerateIV();
        }
        public void GenerateKey()
        {
            aes.GenerateKey();
        }
        #endregion

        #region Decrypt
        public byte[] Decrypt(byte[]? ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                return ARRAY_EMPTY;
            //
            return cipherMode switch
            {
                CipherMode.CBC => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                CipherMode.CFB => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                CipherMode.ECB => aes.DecryptEcb(ciphertext, paddingMode),
                _ => aes.DecryptCbc(ciphertext, Iv, paddingMode),
            };
        }
        public byte[] Decrypt(ReadOnlySpan<byte> ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                return ARRAY_EMPTY;
            //
            return cipherMode switch
            {
                CipherMode.CBC => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                CipherMode.CFB => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                CipherMode.ECB => aes.DecryptEcb(ciphertext, paddingMode),
                _ => aes.DecryptCbc(ciphertext, Iv, paddingMode),
            };
        }
        public async Task<byte[]> DecryptAsync(byte[]? ciphertext)
        {
            byte[] plaintext;
            if (ciphertext == null || ciphertext.Length == 0)
                plaintext = ARRAY_EMPTY;
            else
                plaintext = cipherMode switch
                {
                    CipherMode.CBC => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                    CipherMode.CFB => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                    CipherMode.ECB => aes.DecryptEcb(ciphertext, paddingMode),
                    _ => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                };
            return await Task.FromResult(plaintext);
        }
        public async Task<byte[]> DecryptAsync(ReadOnlyMemory<byte> ciphertext)
        {
            byte[] plaintext;
            if (ciphertext.Length == 0)
                plaintext = ARRAY_EMPTY;
            else
                plaintext = cipherMode switch
                {
                    CipherMode.CBC => aes.DecryptCbc(ciphertext.Span, Iv, paddingMode),
                    CipherMode.CFB => aes.DecryptCbc(ciphertext.Span, Iv, paddingMode),
                    CipherMode.ECB => aes.DecryptEcb(ciphertext.Span, paddingMode),
                    _ => aes.DecryptCbc(ciphertext.Span, Iv, paddingMode),
                };
            return await Task.FromResult(plaintext);
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
                plaintext = cipherMode switch
                {
                    CipherMode.CBC => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                    CipherMode.CFB => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                    CipherMode.ECB => aes.DecryptEcb(ciphertext, paddingMode),
                    _ => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                };
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
            if (isDispose)
            {
                plaintext = null;
                return false;
            }
            //
            if (ciphertext == null || ciphertext.Length == 0)
            {
                plaintext = ARRAY_EMPTY;
                return true;
            }
            try
            {
                plaintext = cipherMode switch
                {
                    CipherMode.CBC => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                    CipherMode.CFB => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                    CipherMode.ECB => aes.DecryptEcb(ciphertext, paddingMode),
                    _ => aes.DecryptCbc(ciphertext, Iv, paddingMode),
                };
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
            return cipherMode switch
            {
                CipherMode.CBC => aes.EncryptCbc(plaintext, Iv, paddingMode),
                CipherMode.CFB => aes.EncryptCfb(plaintext, Iv, paddingMode),
                CipherMode.ECB => aes.EncryptEcb(plaintext, paddingMode),
                _ => aes.EncryptCbc(plaintext, Iv, paddingMode)
            };
        }
        public byte[] Encrypt(ReadOnlySpan<byte> plaintext)
        {
            if (plaintext == null || plaintext.Length == 0)
                return ARRAY_EMPTY;
            //
            return cipherMode switch
            {
                CipherMode.CBC => aes.EncryptCbc(plaintext, Iv, paddingMode),
                CipherMode.CFB => aes.EncryptCfb(plaintext, Iv, paddingMode),
                CipherMode.ECB => aes.EncryptEcb(plaintext, paddingMode),
                _ => aes.EncryptCbc(plaintext, Iv, paddingMode)
            };
        }
        public async Task<byte[]> EncryptAsync(byte[]? plaintext)
        {
            byte[] ciphertext;
            if (plaintext == null || plaintext.Length == 0)
                ciphertext = ARRAY_EMPTY;
            else
                ciphertext = cipherMode switch
                {
                    CipherMode.CBC => aes.EncryptCbc(plaintext, Iv, paddingMode),
                    CipherMode.CFB => aes.EncryptCfb(plaintext, Iv, paddingMode),
                    CipherMode.ECB => aes.EncryptEcb(plaintext, paddingMode),
                    _ => aes.EncryptCbc(plaintext, Iv, paddingMode)
                };
            return await Task.FromResult(ciphertext);
        }
        public async Task<byte[]> EncryptAsync(ReadOnlyMemory<byte> plaintext)
        {
            byte[] ciphertext;
            if (plaintext.Length == 0)
                ciphertext = ARRAY_EMPTY;
            else
                ciphertext = cipherMode switch
                {
                    CipherMode.CBC => aes.EncryptCbc(plaintext.Span, Iv, paddingMode),
                    CipherMode.CFB => aes.EncryptCfb(plaintext.Span, Iv, paddingMode),
                    CipherMode.ECB => aes.EncryptEcb(plaintext.Span, paddingMode),
                    _ => aes.EncryptCbc(plaintext.Span, Iv, paddingMode)
                };
            return await Task.FromResult(ciphertext);
        }
        public bool TryEncrypt(byte[]? plaintext, [NotNullWhen(true)] out byte[]? ciphertext)
        {
            if (isDispose)
            {
                ciphertext = null;
                return false;
            }
            //
            if (plaintext == null || plaintext.Length == 0)
            {
                ciphertext = ARRAY_EMPTY;
                return true;
            }
            //
            try
            {
                ciphertext = cipherMode switch
                {
                    CipherMode.CBC => aes.EncryptCbc(plaintext, Iv, paddingMode),
                    CipherMode.CFB => aes.EncryptCfb(plaintext, Iv, paddingMode),
                    CipherMode.ECB => aes.EncryptEcb(plaintext, paddingMode),
                    _ => aes.EncryptCbc(plaintext, Iv, paddingMode)
                };
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
            if (isDispose)
            {
                ciphertext = null;
                return false;
            }
            //
            if (plaintext == null || plaintext.Length == 0)
            {
                ciphertext = ARRAY_EMPTY;
                return true;
            }
            //
            try
            {
                ciphertext = cipherMode switch
                {
                    CipherMode.CBC => aes.EncryptCbc(plaintext, Iv, paddingMode),
                    CipherMode.CFB => aes.EncryptCfb(plaintext, Iv, paddingMode),
                    CipherMode.ECB => aes.EncryptEcb(plaintext, paddingMode),
                    _ => aes.EncryptCbc(plaintext, Iv, paddingMode)
                };
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
        public static CipherMode Convert(AesCipherMode cipherMode)
        {
            return cipherMode switch
            {
                AesCipherMode.CBC => CipherMode.CBC,
                AesCipherMode.ECB => CipherMode.ECB,
                AesCipherMode.CFB => CipherMode.CFB,
                _ => CipherMode.CBC
            };
        }
        public static PaddingMode Convert(AesPaddingMode paddingMode)
        {
            return paddingMode switch
            {
                AesPaddingMode.None => PaddingMode.None,
                AesPaddingMode.PKCS7 => PaddingMode.PKCS7,
                AesPaddingMode.Zeros => PaddingMode.Zeros,
                AesPaddingMode.ANSIX923 => PaddingMode.ANSIX923,
                AesPaddingMode.ISO10126 => PaddingMode.ISO10126,
                _ => PaddingMode.PKCS7
            };
        }
        #endregion
    }
}
