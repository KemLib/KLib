namespace KCryptography
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
        OaepSHA3_256,
        OaepSHA3_384,
        OaepSHA3_512,
        Pkcs1,
    }
}
