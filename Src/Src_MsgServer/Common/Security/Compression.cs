using System.IO;
using Ionic.Zlib;
using GrandChase.Utilities;

namespace GrandChase.Security
{
    class Compression
    {
        /// <summary>
        /// Returns the compressed packet from the input data
        /// </summary>
        /// <param name="dataToCompress">Packet data to be compressed</param>
        public static byte[] CompressPacket(byte[] dataToCompress)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZlibStream compressor =
                    new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.Level1))
                {
                    compressor.Write(dataToCompress, 11, (dataToCompress.Length - 11));
                }
                return BytesUtil.ConcatBytes(BytesUtil.ReadBytes(dataToCompress, 0, 11), memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Returns the uncompressed packet from the input data
        /// </summary>
        /// <param name="packetToUncompress">Packet data to be uncompressed</param>
        /// <returns></returns>
        public static byte[] UncompressPacket(byte[] packetToUncompress)
        {
            return BytesUtil.ConcatBytes(
                BytesUtil.ReadBytes(packetToUncompress, 0, 11),
                ZlibStream.UncompressBuffer(BytesUtil.ReadBytes(packetToUncompress, 11, (packetToUncompress.Length - 11))));
        }

        public static byte[] Compress(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZlibStream compressor =
                    new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.Default))
                {
                    compressor.Write(data, 0, data.Length);
                }
                return memoryStream.ToArray();
            }
        }
    }
}
