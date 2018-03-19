using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Globalization;

namespace GameServer.network
{
    public class PacketManager : IDisposable
    {
        public byte[] ack;
        private int index;
        private int size;

        public void Dispose()
        {
            ack = null;
            index = 0;
            size = 0;
        }

        public void Byte2(byte x)
        {
            int temp = 1;
            if (size < index + temp)
            {
                size += temp;
                Array.Resize(ref ack, size);
            }
            ack[index++] = x;
        }

        public void Byte(byte x)
        {
            int temp = 1;
            if (size < index + temp)
            {
                temp = temp + 10;
                size += temp;
                Array.Resize(ref ack, size);
            }
            ack[index++] = x;
        }

        public void Boolean(bool get)
        {
            int temp = 1;
            if (size < index + temp)
            {
                temp = temp + 10;
                size += temp;
                Array.Resize(ref ack, size);
            }
            if (get == true)
            {
                ack[index++] = 1;
            }
            else
            {
                ack[index++] = 0;
            }
        }

        public void Int(Int32 u)
        {
            Int32 temp = 4;
            if (size < index + temp)
            {
                size += temp;
                Array.Resize(ref ack, size);
            }
            byte[] init = BitConverter.GetBytes((Int32)u);
            Array.Reverse(init);
            Array.Copy(init,0,ack,index,4);
            index += 4;
        }


        public void Str(string u)
        {
            Int(u.Length);
            byte[] buffer = Encoding.Default.GetBytes(u);
            Bytes(buffer);
        }

        public void UStr(string u)
        {
            Int(u.Length*2);
            byte[] buffer = Encoding.Unicode.GetBytes(u);
            Bytes(buffer);
        }


        public void Hex(string hex)
        {            
            hex = hex.Replace(" ", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                Byte(raw[i]);
            }
        }

        public void Bytes(params byte[] u)
        {
             int temp = u.Length;
             if (size < index + temp)
             {
                 temp = temp + 10;
                 size += temp;
                 Array.Resize(ref ack, size);
             }
             Buffer.BlockCopy(u, 0, ack, index, u.Length);
             index += u.Length;
        }

        public void Short(short u)
        {
            int temp = 2;
            if (size < index + temp)
            {
                temp = temp + 10;
                size += temp;
                Array.Resize(ref ack, size);
            }
            byte[] init = BitConverter.GetBytes(u);
            Array.Reverse(init);
            Buffer.BlockCopy(init, 0, ack, index, 2);
            index += 2;            
        }

        public void OP(short u)
        {
            int temp = 2;
            if (size < index + temp)
            {
                temp = temp + 10;
                size += temp;
                Array.Resize(ref ack, size);
            }
            byte[] init = BitConverter.GetBytes(u);
            Array.Reverse(init);
            Buffer.BlockCopy(init, 0, ack, index, 2);
            index += 2;
            Header();
        }

        public void UShort(ushort u)
        {
            int temp = 3;
            if (size < index + temp)
            {
                temp = temp + 10;
                size += temp;
                Array.Resize(ref ack, size);
            }
            byte[] init = BitConverter.GetBytes(u);
            Array.Reverse(init);
            Buffer.BlockCopy(init, 0, ack, index, 3);
            index += 3;  
        }

        public static byte[] StringFromHex(string hex)
        {
            hex = hex.Replace(" ", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        private void Header() 
        {
            int temp = 7;
            if (size < index + temp)
            {
                temp = temp + 10;
                size += temp;
                Array.Resize(ref ack, size);
            }
            byte[] init = StringFromHex("00 00 00 00 00");
            Bytes(init);
        }       
    }
}
