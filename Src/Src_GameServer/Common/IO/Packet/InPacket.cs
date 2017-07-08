using GrandChase.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using GrandChase.Security;

namespace GrandChase.IO.Packet
{
    public class InPacket : PacketBase
    {
        private byte[] _buffer;

        public override int Length
        {
            get { return _buffer.Length; }
        }

        public int Available
        {
            get
            {
                return _buffer.Length - _index;
            }
        }

        public InPacket(byte[] packet)
        {
            _buffer = packet;
            _index = 0;
        }

        public void Back(int count = 1)
        {
            _index -= count;
        }

        public void Decrypt(byte[] key)
        {
            _buffer = CryptoFunctions.DecryptPacket(_buffer, key);
            
            if (_buffer.Length > 12)
            {
                if ( _buffer[11] == 0x78 && _buffer[12] == 0x01)
                {
                    _buffer = Compression.UncompressPacket(_buffer);
                }
            }
        }

        private void CheckLength(int length)
        {
            if (_index + length > _buffer.Length || length < 0)
                throw new PacketReadException(string.Format("Not enough space (Total: {0}, Now: {1}, Len: {2}", _buffer.Length, _index, length));
        }

        public bool ReadBool()
        {
            return ReadByte() == 1;
        }

        public byte ReadByte()
        {
            CheckLength(1);
            return _buffer[_index++];
        }
        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            CheckLength(count);
            var temp = new byte[count];
            Buffer.BlockCopy(_buffer, _index, temp, 0, count);
            _index += count;
            return temp;
        }

        public byte[] ReadBytesFlipped(int count)
        {
            CheckLength(count);
            var temp = new byte[count];
            Buffer.BlockCopy(_buffer, _index, temp, 0, count);
            EndianConvert.EndianConverter(temp);
            _index += count;
            return temp;
        }

        public unsafe short ReadShort()
        {
            CheckLength( 2 );

            short value;

            fixed ( byte* ptr = _buffer )
            {
                value = *(short*)( ptr + _index );
            }

            value = BitConverter.ToInt16( EndianConvert.EndianConverter( BitConverter.GetBytes( value ) ), 0 );

            _index += 2;

            return value;
        }

        public unsafe short ReadShortNoEndianConvert()
        {
            CheckLength( 2 );

            short value;

            fixed ( byte* ptr = _buffer )
            {
                value = *(short*)( ptr + _index );
            }

            _index += 2;

            return value;
        }

        public ushort ReadUShort()
        {
            return (ushort)ReadShort();
        }

        public unsafe int ReadInt()
        {
            CheckLength( 4 );

            int value;

            fixed ( byte* ptr = _buffer )
            {
                value = *(int*)( ptr + _index );
            }

            value = BitConverter.ToInt32( EndianConvert.EndianConverter( BitConverter.GetBytes( value ) ), 0 );

            _index += 4;

            return value;
        }


        public unsafe int ReadIntNoEndianConvert()
        {
            CheckLength( 4 );

            int value;

            fixed ( byte* ptr = _buffer )
            {
                value = *(int*)( ptr + _index );
            }

            _index += 4;

            return value;
        }

        public uint ReadUInt()
        {
            return (uint)ReadInt();
        }

        public uint ReadUIntNoEndianConvert()
        {
            return (uint)ReadIntNoEndianConvert();
        }

        public unsafe long ReadLong()
        {
            CheckLength(8);

            long value;

            fixed (byte* ptr = _buffer)
            {
                value = *(long*)(ptr + _index);
            }

            value = BitConverter.ToInt64(EndianConvert.EndianConverter(BitConverter.GetBytes(value)), 0);

            _index += 8;

            return value;
        }
        public ulong ReadULong()
        {
            return (ulong)ReadLong();
        }

        public string ReadString(int length)
        {
            CheckLength(length);

            return System.Text.Encoding.GetEncoding(949).GetString(ReadBytes(length));
        }

        public string ReadUnicodeString(int length)
        {
            CheckLength(length);

            return System.Text.Encoding.Unicode.GetString(ReadBytes(length));
        }

        public System.Net.IPEndPoint ReadEndpoint()
        {
            var ip = ReadBytes(4);
            var port = ReadUShort();

            return new System.Net.IPEndPoint(new System.Net.IPAddress(ip), port);
        }

        public byte[] ReadLeftoverBytes()
        {
            return this.ReadBytes(this.Available);
        }

        public void Skip(int count)
        {
            CheckLength(count);
            _index += count;
        }

        public void Reset(int position)
        {
            if (position < 0 || position > Length)
                throw new ArgumentException("Position not in bounds.");
            _index = position;
        }

        public override byte[] ToArray()
        {
            var final = new byte[_buffer.Length];
            Buffer.BlockCopy(_buffer, 0, final, 0, _buffer.Length);
            return final;
        }

        public uint LastKartCryptoKey { get; private set; }
        public byte[] ReadCompressedData()
        {
            return null;
        }

        private int Adler32(byte[] bytes)
        {
            const uint a32mod = 65521;
            uint s1 = 1, s2 = 0;
            foreach (byte b in bytes)
            {
                s1 = (s1 + b) % a32mod;
                s2 = (s2 + s1) % a32mod;
            }
            return unchecked((int)((s2 << 16) + s1));
        }
    }
}
