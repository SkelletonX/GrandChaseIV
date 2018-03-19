using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace GameServer.network
{
    public class Readers: IDisposable    
    {
        public byte[] req;
        public int index = 7;
        public int size;

        public void Dispose()
        {
            req = null;
            index = 7;
            size = 0;
        }

        public byte[] Bytes(int u)        
        {
            var temp = new byte[u];
            Buffer.BlockCopy(req, index, temp, 0, u);
            index += u;
            return temp;
        }

        public unsafe Int32 Int()
        { 
            byte[] temp = new byte[4];
            temp[0] = req[index];
            temp[1] = req[index+1];
            temp[2] = req[index+2];
            temp[3] = req[index+3];

            Array.Reverse(temp);

            Int32 len = BitConverter.ToInt32(temp, 0);
            index += 4;
            return len;
        }

        public unsafe short Short()
        {
            byte[] temp = new byte[2];

            temp[0] = req[index];
            temp[1] = req[index + 1];            

            Array.Reverse(temp);

            short len = BitConverter.ToInt16(temp, 0);
            index += 2;
            return len;
        }

        public byte Byte()
        {
            byte temp;
            temp = req[index];
            index += 1;
            return temp;
        }

        public string UString()
        {
            int len = Int();
                       
            byte[] temp = new byte[len];

            for (int i =0;i < len; i++)
            {
                temp[i] = req[index+i];
            }

            string result = System.Text.Encoding.Unicode.GetString(temp);
            index += len;
            return result;
        }

        public unsafe string String()
        {
            int len = Int();

            byte[] temp = new byte[len];

            for (int i =0;i < len; i++)
            {
                temp[i] = req[index+i];
            }

            string result = System.Text.Encoding.GetEncoding(949).GetString(temp);
            index += len;
            return result;
        }        
    }
}
