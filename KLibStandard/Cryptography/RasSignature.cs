using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace KLibStandard.Cryptography
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
        /// <summary>
        /// Computes the hash value of the specified data and signs it.
        /// </summary>
        public byte[] SignData(byte[] data)
        {
            return rsa.SignData(data, hashAlgorithm, signaturePadding);
        }
        /// <summary>
        /// Computes the hash value of the specified data and signs it.
        /// </summary>
        public byte[] SignData(ReadOnlySpan<byte> data)
        {
            return rsa.SignData(data.ToArray(), hashAlgorithm, signaturePadding);
        }
        /// <summary>
        /// Try computes the hash value of the specified data and signs it.
        /// </summary>
        public bool TrySignData(byte[] data, [NotNullWhen(true)] out byte[] signature)
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
        /// <summary>
        /// Try computes the hash value of the specified data and signs it.
        /// </summary>
        public bool TrySignData(ReadOnlySpan<byte> data, [NotNullWhen(true)] out byte[] signature)
        {
            try
            {
                signature = rsa.SignData(data.ToArray(), hashAlgorithm, signaturePadding);
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
        /// <summary>
        /// When overridden in a derived class, computes the signature for the specified hash value.
        /// </summary>
        public byte[] SignHash(byte[] hash)
        {
            return rsa.SignHash(hash, hashAlgorithm, signaturePadding);
        }
        /// <summary>
        /// When overridden in a derived class, computes the signature for the specified hash value.
        /// </summary>
        public byte[] SignHash(ReadOnlySpan<byte> hash)
        {
            return rsa.SignHash(hash.ToArray(), hashAlgorithm, signaturePadding);
        }
        /// <summary>
        /// When overridden in a derived class, try computes the signature for the specified hash value.
        /// </summary>
        public bool TrySignHash(byte[] hash, [NotNullWhen(true)] out byte[] signature)
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
        /// <summary>
        /// When overridden in a derived class, try computes the signature for the specified hash value.
        /// </summary>
        public bool TrySignHash(ReadOnlySpan<byte> hash, [NotNullWhen(true)] out byte[] signature)
        {
            try
            {
                signature = rsa.SignHash(hash.ToArray(), hashAlgorithm, signaturePadding);
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
        /// <summary>
        /// Verifies that a digital signature is valid by calculating the hash value of the specified data using the specified hash algorithm and padding, and comparing it to the provided signature.
        /// </summary>
        public bool VerifyData(byte[] data, byte[] signature)
        {
            return rsa.VerifyData(data, signature, hashAlgorithm, signaturePadding);
        }
        /// <summary>
        /// Verifies that a digital signature is valid by calculating the hash value of the specified data using the specified hash algorithm and padding, and comparing it to the provided signature.
        /// </summary>
        public bool VerifyData(ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature)
        {
            return rsa.VerifyData(data, signature, hashAlgorithm, signaturePadding);
        }
        /// <summary>
        /// Try verifies that a digital signature is valid by calculating the hash value of the specified data using the specified hash algorithm and padding, and comparing it to the provided signature.
        /// </summary>
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
        /// <summary>
        /// Try verifies that a digital signature is valid by calculating the hash value of the specified data using the specified hash algorithm and padding, and comparing it to the provided signature.
        /// </summary>
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
        /// <summary>
        /// Verifies that a digital signature is valid by determining the hash value in the signature using the specified hash algorithm and padding, and comparing it to the provided hash value.
        /// </summary>
        public bool VerifyHash(byte[] hash, byte[] signature)
        {
            return rsa.VerifyHash(hash, signature, hashAlgorithm, signaturePadding);
        }
        /// <summary>
        /// Verifies that a digital signature is valid by determining the hash value in the signature using the specified hash algorithm and padding, and comparing it to the provided hash value.
        /// </summary>
        public bool VerifyHash(ReadOnlySpan<byte> hash, ReadOnlySpan<byte> signature)
        {
            return rsa.VerifyHash(hash, signature, hashAlgorithm, signaturePadding);
        }
        /// <summary>
        /// Try verifies that a digital signature is valid by determining the hash value in the signature using the specified hash algorithm and padding, and comparing it to the provided hash value.
        /// </summary>
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
        /// <summary>
        /// Try verifies that a digital signature is valid by determining the hash value in the signature using the specified hash algorithm and padding, and comparing it to the provided hash value.
        /// </summary>
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
                _ => HashAlgorithmName.SHA256
            };
        }
        #endregion
    }
}
