namespace KLibStandard.Cryptography
{
    /// <summary>
    /// Specifies the padding mode and parameters to use with RSA encryption or decryption operations.
    /// </summary>
    public enum RsaPaddingMode
    {
        OaepSHA1,
        OaepSHA256,
        OaepSHA384,
        OaepSHA512,
        Pkcs1,
    }
}
