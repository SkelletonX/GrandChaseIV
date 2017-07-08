using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrandChase.Utilities;
using GrandChase.IO.Packet;
using System.Net.Sockets;
using Common;
using GrandChase.IO;
using System.Net;
using GrandChase.Security;
using GrandChase.Function;
using Manager.Factories;

namespace GrandChase.Net.Client
{
    public class ClientSession : Session
    {
        // Security START
        public byte[] CRYPT_KEY { get; set; }
        public byte[] CRYPT_HMAC { get; set; }
        public byte[] CRYPT_PREFIX = new byte[2];
        public int CRYPT_COUNT;
        // Security END

       public MsgUsers MsgUser = new MsgUsers();

        public int LoginUID;
        public string Login;

        public int LastHeartBeat { get; set; }
        public uint IP { get; set; }
        public ushort Port { get; set; }

        public ClientSession(Socket pSocket) : base(pSocket)
        {
            IP = BitConverter.ToUInt32(IPAddress.Parse(GetIP()).GetAddressBytes(), 0);

            InitiateReceive(2, true);

            // 최초 연결시 보안키 설정 패킷
            CRYPT_KEY = CryptoGenerators.GenerateKey();
            CRYPT_HMAC = CryptoGenerators.GenerateKey();
            byte[] TEMP_PREFIX = CryptoGenerators.GeneratePrefix(); // Prefix 설정

            LogFactory.GetLog("KEY").LogInfo(""+CRYPT_KEY);
            LogFactory.GetLog("HMAC").LogInfo("" + CRYPT_HMAC);

            using (OutPacket oPacket = new OutPacket(CenterOpcodes.SET_SECURITY_KEY_NOT))
            {
                oPacket.WriteBytes(TEMP_PREFIX);
                oPacket.WriteInt((int)8);
                oPacket.WriteBytes(CRYPT_HMAC);
                oPacket.WriteInt((int)8);
                oPacket.WriteBytes(CRYPT_KEY);
                oPacket.WriteHexString("00 00 00 01 00 00 00 00 00 00 00 00");

                oPacket.Assemble(CryptoConstants.GC_DES_KEY, CryptoConstants.GC_HMAC_KEY, CRYPT_PREFIX, ++CRYPT_COUNT);
                Send(oPacket);
            }

            // Prefix
            CRYPT_PREFIX = TEMP_PREFIX;

            using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_WAIT_TIME_NOT))
            {
                oPacket.WriteHexString("00 00 27 10");

                oPacket.Assemble(CRYPT_KEY, CRYPT_HMAC, CRYPT_PREFIX, ++CRYPT_COUNT);
                Send(oPacket);
            }
        }

        public string GetIP()
        {
            if( _socket == null ) return "0.0.0.0";

            IPEndPoint remoteIpEndPoint = _socket.RemoteEndPoint as IPEndPoint;
            return ( remoteIpEndPoint.Address.ToString() );
        }

        public override void OnPacket( InPacket iPacket )
        {
            try
            {

                iPacket.Decrypt(CRYPT_KEY);

                GameOpcodes uOpcode = (GameOpcodes)iPacket.ReadShort();
                int uSize = iPacket.ReadInt();
                bool isCompress = iPacket.ReadBool();
                int cSize = 0;
                if (isCompress == true)
                {
                    cSize = iPacket.ReadInt();
                    LogFactory.GetLog("Main").LogInfo("Pacote comprimido {0}({1})", (int)uOpcode, uOpcode.ToString());
                }
                else
                {
                    LogFactory.GetLog("Main").LogInfo("Packet {0}({1})", (int)uOpcode, uOpcode.ToString());
                }

                LogFactory.GetLog("Main").LogHex("Pacote", iPacket.ToArray());

                switch ( uOpcode )
                {
                    case GameOpcodes.HEART_BIT_NOT:
                        OnHeartBeatNot();
                        break;
                    case GameOpcodes.EVENT_INVITE_REQ: //31 EVENT_FRIEND_LIST_REQ
                        MsgUser.FriendList(this,iPacket);
                        break;
                    case GameOpcodes.EVENT_LEAVE_ROOM_ACK: //34 ADD_FRIEND
                        MsgUser.Add(this, iPacket);
                        break;
                        
                    default:
                        {
                            LogFactory.GetLog("Main").LogWarning("Pacote indefinido foi recebida. Opcode: {0}({1})", (int)uOpcode, uOpcode.ToString() );
                            LogFactory.GetLog("Main").LogHex("Pacote", iPacket.ToArray());
                            break;
                        }
                }
            }
            catch( Exception e )
            {
                LogFactory.GetLog("Main").LogFatal(e);
                Close();
            }
        }

        public override void OnDisconnect()
        {
            TSingleton<ClientHolder>.Instance.DestoryAccount(this);
        }

        public void OnHeartBeatNot()
        {
            LastHeartBeat = Environment.TickCount;
        }

        public void OnClientPingConfig()
        {
            using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_CLIENT_PING_CONFIG_ACK))
            {
                oPacket.WriteHexString("00 00 0F A0 00 00 0F A0 00 00 0F A0 00 00 00 01 00 FF FF FF FF 00 00 00 00");
                oPacket.Assemble(CRYPT_KEY, CRYPT_HMAC, CRYPT_PREFIX, ++CRYPT_COUNT);
                Send(oPacket);
            }
        }
    }
}
