using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KLibStandard.Cryptography
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
        private static readonly byte[] ARRAY_EMPTY = new byte[0];

        public readonly AesCipherMode Cipher;
        public readonly AesPaddingMode Padding;
        private readonly Aes aes;
        private ICryptoTransform decryptorTransform,
            encryptorTransform;

        private bool isDispose;

        public CipherName Algorithm => CipherName.Aes;
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
            aes.Mode = Convert(cipher);
            aes.Padding = Convert(padding);
            decryptorTransform = aes.CreateDecryptor();
            encryptorTransform = aes.CreateEncryptor();
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
                decryptorTransform.Dispose();
                encryptorTransform.Dispose();
                aes.Dispose();
            }
        }
        #endregion

        #region Method
        public static bool TryCreate([NotNullWhen(true)] out AesCipher aes, AesCipherMode cipher = DEFAULT_CIPHER_MODE, AesPaddingMode padding = DEFAULT_PADDING_MODE)
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
            decryptorTransform = aes.CreateDecryptor();
            encryptorTransform = aes.CreateEncryptor();
        }
        public bool TrySetKey(byte[] key)
        {
            try
            {
                aes.Key = key;
                decryptorTransform = aes.CreateDecryptor();
                encryptorTransform = aes.CreateEncryptor();
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
            decryptorTransform = aes.CreateDecryptor();
            encryptorTransform = aes.CreateEncryptor();
        }
        public bool TrySetIv(byte[] iv)
        {
            try
            {
                aes.IV = iv;
                decryptorTransform = aes.CreateDecryptor();
                encryptorTransform = aes.CreateEncryptor();
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
        public ResultDecrypt Decrypt(byte[] ciphertext)
        {
            if (ciphertext == null || ciphertext.Length == 0)
                return ResultDecrypt.RESULT_EMPTY;
            //
            try
            {
                using MemoryStream msDecrypt = new MemoryStream(ciphertext);
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptorTransform, CryptoStreamMode.Read);
                List<byte> listData = new List<byte>();
                while (true)
                {
                    int data = csDecrypt.ReadByte();
                    if (data < 0)
                        break;
                    listData.Add((byte)data);
                }
                return new ResultDecrypt(listData.ToArray());
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
                using MemoryStream msDecrypt = new MemoryStream(ciphertext.ToArray());
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptorTransform, CryptoStreamMode.Read);
                List<byte> listData = new List<byte>();
                while (true)
                {
                    int data = csDecrypt.ReadByte();
                    if (data < 0)
                        break;
                    listData.Add((byte)data);
                }
                return new ResultDecrypt(listData.ToArray());
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
                using MemoryStream msDecrypt = new MemoryStream(ciphertext);
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptorTransform, CryptoStreamMode.Read);
                List<byte> listData = new List<byte>();
                while (true)
                {
                    int data = csDecrypt.ReadByte();
                    if (data < 0)
                        break;
                    listData.Add((byte)data);
                }
                return new ResultDecrypt(listData.ToArray());
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
                using MemoryStream msDecrypt = new MemoryStream(ciphertext.ToArray());
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptorTransform, CryptoStreamMode.Read);
                List<byte> listData = new List<byte>();
                while (true)
                {
                    int data = csDecrypt.ReadByte();
                    if (data < 0)
                        break;
                    listData.Add((byte)data);
                }
                return new ResultDecrypt(listData.ToArray());
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
                plaintext = ARRAY_EMPTY;
                return true;
            }
            //
            try
            {
                using MemoryStream msDecrypt = new MemoryStream(ciphertext);
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptorTransform, CryptoStreamMode.Read);
                List<byte> listData = new List<byte>();
                while (true)
                {
                    int data = csDecrypt.ReadByte();
                    if (data < 0)
                        break;
                    listData.Add((byte)data);
                }
                plaintext = listData.ToArray();
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
                using MemoryStream msDecrypt = new MemoryStream(ciphertext.ToArray());
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptorTransform, CryptoStreamMode.Read);
                List<byte> listData = new List<byte>();
                while (true)
                {
                    int data = csDecrypt.ReadByte();
                    if (data < 0)
                        break;
                    listData.Add((byte)data);
                }
                plaintext = listData.ToArray();
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
                using MemoryStream msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptorTransform, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plaintext);
                }
                return new ResultEncrypt(msEncrypt.ToArray());
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
                using MemoryStream msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptorTransform, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plaintext);
                }
                return new ResultEncrypt(msEncrypt.ToArray());
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
                using MemoryStream msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptorTransform, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plaintext);
                }
                return new ResultEncrypt(msEncrypt.ToArray());
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
                using MemoryStream msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptorTransform, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plaintext.ToArray());
                }
                return new ResultEncrypt(msEncrypt.ToArray());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResultEncrypt(ex.Message));
            }
        }
        public bool TryEncrypt(byte[] plaintext, [NotNullWhen(true)] out byte[] ciphertext)
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
                using MemoryStream msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptorTransform, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plaintext);
                }
                ciphertext = msEncrypt.ToArray();
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
                using MemoryStream msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptorTransform, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plaintext);
                }
                ciphertext = msEncrypt.ToArray();
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
                AesCipherMode.CTS => CipherMode.CTS,
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
