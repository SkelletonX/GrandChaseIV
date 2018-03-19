using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GCNet.CoreLib;
using GCNet.PacketLib;
using CenterServer.Packets;

namespace CenterServer.network
{
    public class User
    {
        public int HeartCount { get; set; }
        public bool active = false;
        public Socket socket = null;
        public byte ServerMaster = 0;
        public short clientID = 0;
        public byte[] buffer = null;
        public byte[] IV = new byte[8];
        public byte[] Hmac = new byte[8];
        public short prefix;
        public int count;
        Initialize initialize = new Initialize();
        Readers readers = new Readers();
        Log log = new Log();

        public User(Socket socket, short ClientID, byte ServerMaster)
        {
            try
            {                
                this.clientID = ClientID;
                this.active = true;
                this.socket = socket;
                this.ServerMaster = ServerMaster;                                
                initialize.Vetor(this);
                this.buffer = new byte[1024];                
                this.socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, this.Read, null);
                readers.req = this.buffer;                                
            }
            catch (Exception e)
            {
                this.close();
                log.Error("{1} \n\n {0}", e.Message, e.StackTrace);
            }
        }

        public void Read(IAsyncResult read)
        {
            try
            {
                if (this.active == true)
                {
                    byte[] sizer = new byte[2];
                    sizer[0] = this.buffer[0];
                    sizer[1] = this.buffer[1];
                    Array.Reverse(sizer);
                    short Resize = BitConverter.ToInt16(this.buffer, 0);
                    if (Resize > 0)
                    {
                        Array.Resize(ref this.buffer, Resize);
                        Array.Resize(ref this.readers.req, Resize);                        
                        this.readers.req = this.buffer;
                        this.buffer = initialize.decrypt(this.buffer, this.IV, this.Hmac);
                        byte[] getOp = new byte[2];
                        getOp[0] = this.buffer[0];
                        getOp[1] = this.buffer[1];
                        Array.Reverse(getOp);
                        short opcode= BitConverter.ToInt16(getOp, 0);
                        if ((byte)this.buffer[6] == 1)
                        {
                            byte[] temp = new byte[0];
                            byte[] lenCompress = new byte[2];
                            lenCompress[0] = this.buffer[4];
                            lenCompress[1] = this.buffer[5];
                            Array.Reverse(lenCompress);
                            short len = BitConverter.ToInt16(lenCompress, 0);
                            Array.Resize(ref temp, len - 4);
                            Array.Copy(this.buffer, 11, temp, 0, len - 4);
                            byte[] temp1 = ZLib.DecompressData(temp);
                            byte[] bufferDecompress = new byte[0];
                            Array.Resize(ref bufferDecompress, temp1.Length + 7);
                            Array.Copy(temp1,0,bufferDecompress,7,temp1.Length);
                            Array.Copy(this.buffer, 0, bufferDecompress, 0, 7);

                            Array.Resize(ref this.buffer, bufferDecompress.Length);
                            Array.Resize(ref this.readers.req, bufferDecompress.Length);

                            this.readers.req = bufferDecompress;
                            this.buffer = bufferDecompress;
                            Packets packets = new Packets(opcode, readers, this, bufferDecompress);
                        }
                        else
                        {
                            Packets packets = new Packets(opcode, readers, this, this.buffer);
                        }
                        
                    }
                    else
                    {
                        this.close();
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("{1} \n {0}", e.Message, e.StackTrace);
            }
            finally
            {
                if (this.active)
                {
                    this.buffer = new byte[1024];
                    this.readers.req = new byte[1024];
                    this.socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, this.Read, null);                    
                }
            }
        }
        public void close()
        {
            try
            {
                if (this.active)
                {
                    this.active = false;
                    this.socket.Close();
                    this.socket = null;
                    configserver.servers[this.ServerMaster].users[this.clientID] = null;
                }
            }
            catch (Exception e)
            {
                log.Error("{0} \n {1}", e.Message, e.StackTrace);
            }
        }
        public void HBNot() //Heart
        {
            HeartCount = Environment.TickCount;
        }


        public void Send(byte[] data)
        {
            CryptoHandler Crypto = new CryptoHandler();
            AuthHandler Auth = new AuthHandler();
            Crypto.Key = this.Hmac;
            Auth.HmacKey = this.IV;
            Random getRndm = new Random();
            this.prefix = (short)getRndm.Next(9999);
            this.count = getRndm.Next(9999);
            Int16 sizePackage = Convert.ToInt16(data.Length - 12);
            byte[] size = new byte[2];
            size = BitConverter.GetBytes(sizePackage);
            Array.Reverse(size);
            Array.Copy(size, 0, data, 4, size.Length);
            byte[] opcode = new byte[2];
            Array.Copy(data, 0, opcode, 0, opcode.Length);
            Array.Resize(ref data, data.Length - 5);
            OutPacket GetPacket = new OutPacket(data, Crypto, Auth, prefix, count);
            this.socket.Send(GetPacket.PacketData, 0, GetPacket.PacketData.Length, 0);
            //log.Hex("Enviando, PacketID {" + Convert.ToInt32(BitConverter.ToString(opcode).Replace("-", "")) + "} Size {"+BitConverter.ToString(size)+"} Payload : ",data);
        }

        public void Clear(PacketManager PM)
        {
            PM.ack = null;
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
    }        


    public static class configserver
    {
        //Configuraçoes do Servidor
        public static string[] hosts = {"127.0.0.1"};
        public static Server[] servers = null;
        public static int MaxConnection = 700;
        public static int Port = 0;


        //Configuraçoes MySQL
        public static string server = "127.0.0.1";//Server MySQL
        public static string database = "gc";
        public static string uid = "root";
        public static string password = "root";
    }
}
