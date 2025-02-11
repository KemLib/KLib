namespace KLib.Cryptography
{
    /// <summary>
    /// Specifies the type of padding to apply when the message data block is shorter than the full number of bytes needed for a cryptographic operation.
    /// </summary>
    public enum AesPaddingMode
    {
        None,
        PKCS7,
        Zeros,
        ANSIX923,
        ISO10126,
    }
}
