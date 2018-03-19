using System;
using System.Security.Cryptography;

namespace GameServer
{
    /// <summary>
    /// Handles DES encryption operations.
    /// </summary>
    public static class DESEncryption
    {
        /// <summary>
        /// Gets the cryptography provider used in the Grand Chase's encryption operations.
        /// </summary>
        private static DESCryptoServiceProvider DESProvider = new DESCryptoServiceProvider()
        {
            Mode = CipherMode.CBC,
            Padding = PaddingMode.None
        };

        /// <summary>
        /// Encrypts the specified byte array.
        /// </summary>
        /// <param name="data">The array of bytes to be encrypted.</param>
        /// <param name="iv">The initialization vector (IV).</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The encrypted data.</returns>
        public static byte[] EncryptData(byte[] data, byte[] iv, byte[] key)
        {
            ICryptoTransform encryptor = DESProvider.CreateEncryptor(key, iv);
            return encryptor.TransformFinalBlock(data, 0, data.Length);
        }

        /// <summary>
        /// Decrypts the specified byte array.
        /// </summary>
        /// <param name="data">The array of bytes to be decrypted.</param>
        /// <param name="iv">The initialization vector (IV).</param>
        /// <param name="key">The decryption key.</param>
        /// <returns>The decrypted data.</returns>
        public static byte[] DecryptData(byte[] data, byte[] iv, byte[] key)
        {
            ICryptoTransform decryptor = DESProvider.CreateDecryptor(key, iv);
            return decryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
}
