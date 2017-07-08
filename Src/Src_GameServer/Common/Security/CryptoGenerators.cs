using System;
using System.Security.Cryptography;
using GrandChase.Utilities;

namespace GrandChase.Security
{
    /// <summary>
    /// Provides the generation functions used in Grand Chase Networking
    /// </summary>
    public static class CryptoGenerators
    {
        /// <summary>
        /// Generates an encryption IV (initialization vector)
        /// </summary>
        public static byte[] GenerateIV()
        {
            byte[] outputIV = new byte[8];

            // The byte that will fill all the IV
            byte ivByte;

            Random random = new Random();
            ivByte = (byte)random.Next(0x00, 0xFF);

            for (int i = 0; i < outputIV.Length; i++)
            {
                outputIV[i] = ivByte;
            }
            return outputIV;
        }

        public static byte[] GeneratePrefix()
        {
            byte[] prefix = new byte[2];

            // The byte that will fill all the IV
            byte ivByte;

            Random random = new Random();
            ivByte = (byte)random.Next(0x00, 0xFF);

            for (int i = 0; i < prefix.Length; i++)
            {
                prefix[i] = ivByte;
            }
            return prefix;
        }

        /// <summary>
        /// Generates a key which may be used in the packet encryption or in the HMAC generation
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateKey()
        {
            byte[] outputKey = new byte[8];

            using (RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(outputKey);
            }
            return outputKey;
        }

        /// <summary>
        /// Generates an HMAC hash to the encrypted packet data
        /// </summary>
        /// <param name="data">Packet data (excluding the packet size and the HMAC hash)</param>
        /// <param name="hmacKey">HMAC Key</param>
        internal static byte[] GenerateHmac(byte[] data, byte[] hmacKey)
        {
            using (HMACMD5 hmac = new HMACMD5(hmacKey))
            {
                // Generates the hash, truncates it to 10 bytes (HMAC_SIZE) and returns it
                return BytesUtil.ReadBytes(hmac.ComputeHash(data, 0, data.Length),
                    0, CryptoConstants.GC_HMAC_SIZE);
            }
        }
    }
}