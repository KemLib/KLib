﻿using System.Security.Cryptography;

namespace KCryptography
{
    public class RasSignature : IDisposable
    {
        #region Properties
        public readonly RsaSignaturePadding Signature;
        public readonly RsaHashAlgorithm Hash;
        private readonly RSASignaturePadding signaturePadding;
        private readonly HashAlgorithmName hashAlgorithm;
        private readonly RSA rsa;
        private bool isDispose;
        #endregion

        #region Construction
        public RasSignature(RsaSignaturePadding signature = RsaSignaturePadding.Pkcs1, RsaHashAlgorithm hash = RsaHashAlgorithm.SHA256)
        {
            Signature = signature;
            Hash = hash;
            signaturePadding = Convert(signature);
            hashAlgorithm = Convert(hash);
            rsa = RSA.Create();
            isDispose = false;
        }
        ~RasSignature()
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

        #region Sign Data
        public byte[] SignData(byte[] data)
        {
            return rsa.SignData(data, hashAlgorithm, signaturePadding);
        }
        public byte[] SignData(ReadOnlySpan<byte> data)
        {
            return rsa.SignData(data, hashAlgorithm, signaturePadding);
        }
        public bool TrySignData(byte[] data, [NotNullWhen(true)] out byte[]? signature)
        {
            try
            {
                signature = rsa.SignData(data, hashAlgorithm, signaturePadding);
                return true;
            }
            catch (Exception)
            {
                signature = null;
                return false;
            }
        }
        public bool TrySignData(ReadOnlySpan<byte> data, [NotNullWhen(true)] out byte[]? signature)
        {
            try
            {
                signature = rsa.SignData(data, hashAlgorithm, signaturePadding);
                return true;
            }
            catch (Exception)
            {
                signature = null;
                return false;
            }
        }
        #endregion

        #region Sign Hash
        public byte[] SignHash(byte[] hash)
        {
            return rsa.SignHash(hash, hashAlgorithm, signaturePadding);
        }
        public byte[] SignHash(ReadOnlySpan<byte> hash)
        {
            return rsa.SignHash(hash, hashAlgorithm, signaturePadding);
        }
        public bool TrySignHash(byte[] hash, [NotNullWhen(true)] out byte[]? signature)
        {
            try
            {
                signature = rsa.SignHash(hash, hashAlgorithm, signaturePadding);
                return true;
            }
            catch (Exception)
            {
                signature = null;
                return false;
            }
        }
        public bool TrySignHash(ReadOnlySpan<byte> hash, [NotNullWhen(true)] out byte[]? signature)
        {
            try
            {
                signature = rsa.SignHash(hash, hashAlgorithm, signaturePadding);
                return true;
            }
            catch (Exception)
            {
                signature = null;
                return false;
            }
        }
        #endregion

        #region Verify Data
        public bool VerifyData(byte[] data, byte[] signature)
        {
            return rsa.VerifyData(data, signature, hashAlgorithm, signaturePadding);
        }
        public bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature)
        {
            return rsa.VerifyData(data, signature, hashAlgorithm, signaturePadding);
        }
        public bool TryVerifyData(byte[] data, byte[] signature)
        {
            try
            {
                return rsa.VerifyData(data, signature, hashAlgorithm, signaturePadding);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryVerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature)
        {
            try
            {
                return rsa.VerifyData(data, signature, hashAlgorithm, signaturePadding);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Verify Hash
        public bool VerifyHash(byte[] hash, byte[] signature)
        {
            return rsa.VerifyHash(hash, signature, hashAlgorithm, signaturePadding);
        }
        public bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature)
        {
            return rsa.VerifyHash(hash, signature, hashAlgorithm, signaturePadding);
        }
        public bool TryVerifyHash(byte[] hash, byte[] signature)
        {
            try
            {
                return rsa.VerifyHash(hash, signature, hashAlgorithm, signaturePadding);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryVerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature)
        {
            try
            {
                return rsa.VerifyHash(hash, signature, hashAlgorithm, signaturePadding);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Convert
        public static RSASignaturePadding Convert(RsaSignaturePadding signaturePadding)
        {
            return signaturePadding switch
            {
                RsaSignaturePadding.Pkcs1 => RSASignaturePadding.Pkcs1,
                RsaSignaturePadding.Pss => RSASignaturePadding.Pss,
                _ => RSASignaturePadding.Pkcs1
            };
        }
        public static HashAlgorithmName Convert(RsaHashAlgorithm hashAlgorithm)
        {
            return hashAlgorithm switch
            {
                RsaHashAlgorithm.MD5 => HashAlgorithmName.MD5,
                RsaHashAlgorithm.SHA1 => HashAlgorithmName.SHA1,
                RsaHashAlgorithm.SHA256 => HashAlgorithmName.SHA256,
                RsaHashAlgorithm.SHA384 => HashAlgorithmName.SHA384,
                RsaHashAlgorithm.SHA512 => HashAlgorithmName.SHA512,
                RsaHashAlgorithm.SHA3_256 => HashAlgorithmName.SHA3_256,
                RsaHashAlgorithm.SHA3_384 => HashAlgorithmName.SHA3_384,
                RsaHashAlgorithm.SHA3_512 => HashAlgorithmName.SHA3_512,
                _ => HashAlgorithmName.SHA256
            };
        }
        #endregion
    }
}
