using System;

namespace GrandChase.Utilities
{
    public static class BytesUtil
    {
        /// <summary>
        /// Returns a specified block of bytes from the source array starting at a defined offset
        /// </summary>
        /// <param name="source">Source byte array from where the block will be read</param>
        /// <param name="offset">Index from where the reading will begin</param>
        /// <param name="length">Number of bytes that will be read</param>
        public static byte[] ReadBytes(byte[] source, int offset, int length)
        {
            byte[] outputBytes = new byte[length];
            Buffer.BlockCopy(source, offset, outputBytes, 0, length);

            return outputBytes;
        }

        /// <summary>
        /// Concatenates two array of bytes
        /// </summary>
        /// <param name="firstBytes">The first byte array to be concatenated</param>
        /// <param name="secondBytes">The second byte array to be concatenated</param>
        /// <returns></returns>
        public static byte[] ConcatBytes(byte[] firstBytes, byte[] secondBytes)
        {
            byte[] outputBytes = new byte[firstBytes.Length + secondBytes.Length];

            Buffer.BlockCopy(firstBytes, 0, outputBytes, 0, firstBytes.Length);
            Buffer.BlockCopy(secondBytes, 0, outputBytes, firstBytes.Length, secondBytes.Length);

            return outputBytes;
        }
    }
}
