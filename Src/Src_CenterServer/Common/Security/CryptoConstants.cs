namespace GrandChase.Security
{
    /// <summary>
    /// Provides the basic cryptographic constants used in Grand Chase Networking
    /// </summary>
    public static class CryptoConstants
    {
        /// <summary>
        /// Default DES encryption key used at the start of the Grand Chase connection
        /// </summary>
        public static readonly byte[] GC_DES_KEY = { 0xC7, 0xD8, 0xC4, 0xBF, 0xB5, 0xE9, 0xC0, 0xFD };

        /// <summary>
        /// Default HMAC MD5 key used at the start of the Grand Chase connection
        /// </summary>
        public static readonly byte[] GC_HMAC_KEY = { 0xC0, 0xD3, 0xBD, 0xC3, 0xB7, 0xCE, 0xB8, 0xB8 };

        /// <summary>
        /// Size of the truncated generated HMAC 
        /// </summary>
        public static readonly byte GC_HMAC_SIZE = 10;
    }
}
