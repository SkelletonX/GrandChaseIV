using GrandChase.Net;
using GrandChase.Utilities;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using GrandChase.Security;

namespace GrandChase.IO.Packet
{
    public class OutPacket : PacketBase, IDisposable
    {
        private byte[] _buffer;
        private byte[] _buffer_before_assemble;
        private int _length;
        private bool m_disposed;
        
        public int Opcode { get; private set; }

        public override int Length
        {
            get { return _length; }
        }

        public bool Disposed
        {
            get
            {
                return m_disposed;
            }
        }

        public OutPacket(CenterOpcodes nOpcode)
        {
            Opcode = (int)nOpcode;
        }

        public OutPacket(GameOpcodes nOpcode)
        {
            Opcode = (int)nOpcode;
        }

        public byte[] getBuffer() {
            return _buffer;
        }

        private void ExpandIfNeeded(int sizeOfBytesToWrite)
        {
            if (_length < _index + sizeOfBytesToWrite)
            {
                _length += sizeOfBytesToWrite + 10;
                Array.Resize<byte>(ref _buffer, _length);
            }
        }

        // NOTE: Credits to Shoftee (LittleEndianByteConverter class).
        private void Append(long value, int byteCount)
        {
            for (int i = 0; i < byteCount; i++)
            {
                WriteByte((byte)value);
                value >>= 8;
            }
        }

        public void WriteBool(bool value)
        {
            ThrowIfDisposed();
            WriteByte(value ? (byte)1 : (byte)0);
        }

        public void WriteByte(byte value = 0)
        {
            ThrowIfDisposed();
            ExpandIfNeeded(1);
            _buffer[_index++] = value;
        }
        public void WriteSByte(sbyte value = 0)
        {
            WriteByte((byte)value);
        }

        public void WriteBytes(params byte[] value)
        {
            ThrowIfDisposed();
            ExpandIfNeeded(value.Length);
            Buffer.BlockCopy(value, 0, _buffer, _index, value.Length);
            _index += value.Length;
        }

        public void WriteShort(short value = 0)
        {
            ThrowIfDisposed();

            WriteBytes(EndianConvert.EndianConverter(BitConverter.GetBytes(value)));
        }
        public void WriteUShort(ushort value = 0)
        {
            WriteShort((short)value);
        }

        public void WriteInt(int value = 0)
        {
            ThrowIfDisposed();

            WriteBytes(EndianConvert.EndianConverter(BitConverter.GetBytes(value)));
        }
        public void WriteUInt(uint value = 0)
        {
            WriteInt((int)value);
        }
        public void WriteLong(long value = 0)
        {
            ThrowIfDisposed();

            WriteBytes(EndianConvert.EndianConverter(BitConverter.GetBytes(value)));
        }
        public void WriteULong(ulong value = 0)
        {
            WriteLong((long)value);
        }
        public void WriteFloat(float value = 0)
        {
            this.WriteBytes(BitConverter.GetBytes(value));
        }
        public void WriteTicks(long ticks)
        {
            ushort[] value = TimeUtil.GetDateTime(ticks);

            this.WriteUShort(value[0]);
            this.WriteUShort(value[1]);
        }
        public void Skip(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.WriteByte();
            }
        }
        public void WriteString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            byte[] bytes = Encoding.GetEncoding(949).GetBytes(value);
            WriteBytes(bytes);
        }
        public void WriteUnicodeString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            byte[] bytes = Encoding.Unicode.GetBytes(value);
            WriteBytes(bytes);
        }
        public void WriteHexString(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            value = value.Replace(" ", "");

            for (int i = 0; i < value.Length; i += 2)
            {
                WriteByte(byte.Parse(value.Substring(i, 2),  NumberStyles.HexNumber));
            }
        }
        
        public void WriteEndpoint(System.Net.IPEndPoint endpoint)
        {
            WriteEndpoint(endpoint.Address, (ushort)endpoint.Port);
        }

        public void WriteEndpoint(System.Net.IPAddress ip, ushort port)
        {
            WriteBytes(ip.GetAddressBytes());
            WriteUShort(port);
        }

        public void WriteIPFromString(string IP, bool reverse)
        {
            byte[] temp = System.Net.IPAddress.Parse(IP).GetAddressBytes();
            if (reverse == true)
                Array.Reverse(temp);
            WriteBytes(temp);
        }

        private void ThrowIfDisposed()
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        #region Write Reversed Data
        /// <summary>
        /// Writes the reversed specified byte array to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be reversed and written</param>
        public void WriteReversedData(byte[] valueToWrite)
        {
            Array.Reverse(valueToWrite);
            WriteBytes(valueToWrite);
        }

        /// <summary>
        /// Writes the reversed specified short to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be reversed and written</param>
        public void WriteReversedData(short valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);
        }

        /// <summary>
        /// Writes the reversed specified integer to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be reversed and written</param>
        public void WriteReversedData(int valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);

            Console.WriteLine(BitConverter.ToString(bytesToWrite).Replace('-', ' '));
        }

        /// <summary>
        /// Writes the reversed specified long to the packet
        /// </summary>
        /// <param name="valueToWrite">Value to be reversed and written</param>
        public void WriteReversedData(long valueToWrite)
        {
            byte[] bytesToWrite = BitConverter.GetBytes(valueToWrite);

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytesToWrite);
            }
            WriteBytes(bytesToWrite);
        }
        #endregion

        public override byte[] ToArray()
        {
            ThrowIfDisposed();

            // Try to resize to real size
            Array.Resize<byte>(ref _buffer, _index);

            return _buffer;
        }

        public void Dispose()
        {
            m_disposed = true;

            _buffer = null;
        }

        /// <summary>
        /// 패킷을 보낼 수 있도록 가공한다.
        /// </summary>
        /// <param name="key">Encryption key</param>
        /// <param name="hmacKey">HMAC generation key</param>
        /// <param name="prefix">The 6 bytes between the packet size and the IV</param>
        public void Assemble(byte[] key, byte[] hmacKey, byte[] prefix, int count)
        {
            // 버퍼를 복구할 수 있도록.
            _buffer_before_assemble = _buffer;

            byte[] Op = BitConverter.GetBytes((short)Opcode);
            byte[] Len = BitConverter.GetBytes((int)_buffer.Length); // 버퍼와 압축여부
            Array.Reverse(Op);
            Array.Reverse(Len);

            byte[] TempData;
            TempData = BytesUtil.ConcatBytes(Op, Len);
            TempData = BytesUtil.ConcatBytes(TempData, BitConverter.GetBytes((bool)false)); // 압축 아님
            _buffer = BytesUtil.ConcatBytes(TempData, _buffer);

            byte[] IV = CryptoGenerators.GenerateIV();

            byte[] dataToAssemble = BytesUtil.ConcatBytes(
                BytesUtil.ConcatBytes(prefix, BitConverter.GetBytes(count)),
                BytesUtil.ConcatBytes(IV, CryptoFunctions.EncryptPacket(_buffer, key, IV)));

            _buffer = CryptoFunctions.ClearPacket(dataToAssemble, hmacKey);
        }

        /// <summary>
        /// 패킷을 보낼 수 있도록 가공한다. (zlib 압축 데이터 여기서 생성)
        /// </summary>
        /// <param name="key">Encryption key</param>
        /// <param name="hmacKey">HMAC generation key</param>
        /// <param name="prefix">The 6 bytes between the packet size and the IV</param>
        public void CompressAndAssemble(byte[] key, byte[] hmacKey, byte[] prefix, int count)
        {
            // 버퍼를 복구할 수 있도록.
            _buffer_before_assemble = _buffer;

            byte[] data = Compression.Compress(_buffer);

            byte[] Op = BitConverter.GetBytes((short)Opcode);
            byte[] Len = BitConverter.GetBytes((int)data.Length + 4); // 버퍼 + 압축내용
            Array.Reverse(Op);
            Array.Reverse(Len);

            byte[] TempData;
            TempData = BytesUtil.ConcatBytes(Op, Len);
            TempData = BytesUtil.ConcatBytes(TempData, BitConverter.GetBytes((bool)true)); // 압축임
            TempData = BytesUtil.ConcatBytes(TempData, BitConverter.GetBytes((int)_buffer.Length)); // 실제 크기
            _buffer = BytesUtil.ConcatBytes(TempData, data);

            byte[] IV = CryptoGenerators.GenerateIV();

            byte[] dataToAssemble = BytesUtil.ConcatBytes(
                BytesUtil.ConcatBytes(prefix, BitConverter.GetBytes(count)),
                BytesUtil.ConcatBytes(IV, CryptoFunctions.EncryptPacket(_buffer, key, IV)));
            _buffer = CryptoFunctions.ClearPacket(dataToAssemble, hmacKey);
        }

        public void CancelAssemble()
        {
            // 버퍼 복구
            _buffer = _buffer_before_assemble;
        }

        // 버퍼를 초기화한다.
        public void InitBuffer()
        {
            Array.Resize(ref _buffer, 0);
            _index = 0;
            _length = 0;
        }

        // 버퍼의 데이터를 압축한다. 필요할떄가 있음.
        public void CompressBuffer()
        {
            _buffer = Compression.Compress(_buffer);
        }
    }
}
