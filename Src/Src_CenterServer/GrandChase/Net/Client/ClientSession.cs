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
using Manager;

namespace GrandChase.Net.Client
{
    public class ClientSession : Session
    {
        // Security START
        public byte[] CRYPT_KEY { get; set; }
        public byte[] CRYPT_HMAC { get; set; }
        public byte[] CRYPT_PREFIX  = new byte[2];
        public int CRYPT_COUNT;
        // Security END

        public Loading MyLoading = new Loading();
        public User MyUser = new User();

        public int LoginUID;

        public int LastHeartBeat { get; set; }
        public uint IP { get; set; }
        public ushort Port { get; set; }

        public ClientSession(Socket pSocket) : base(pSocket)
        {
            IP = BitConverter.ToUInt32(IPAddress.Parse(GetIP()).GetAddressBytes(), 0);

            InitiateReceive(2, true);

            CRYPT_KEY = CryptoGenerators.GenerateKey();
            CRYPT_HMAC = CryptoGenerators.GenerateKey();
            byte[] TEMP_PREFIX = CryptoGenerators.GeneratePrefix();
            LogFactory.GetLog("KEY").LogHex("IV: ",CRYPT_KEY);
            LogFactory.GetLog("KEY").LogHex("HMAC: ", CRYPT_HMAC);
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

                CenterOpcodes uOpcode = (CenterOpcodes)iPacket.ReadShort();
                int uSize = iPacket.ReadInt();
                bool isCompress = iPacket.ReadBool();
                int cSize = 0;
                if (isCompress == true)
                {
                    cSize = iPacket.ReadInt();
                    LogFactory.GetLog("Main").LogWarning("Recebido Pacote Compress  {0}({1})", (int)uOpcode, uOpcode.ToString());
                }
                else
                {
                    LogFactory.GetLog("Main").LogInfo("Pacote Recebido {0}({1})", (int)uOpcode, uOpcode.ToString());
                }

                LogFactory.GetLog("Main").LogHex(uOpcode + ": ", iPacket.ToArray());

                switch ( uOpcode )
                {
                    case CenterOpcodes.HEART_BIT_NOT:
                        OnHeartBeatNot();
                        break;
                    case CenterOpcodes.ENU_CLIENT_CONTENTS_FIRST_INIT_INFO_REQ:
                        MyLoading.NotifyContentInfo(this,iPacket);
                        break;
                    case CenterOpcodes.ENU_SHAFILENAME_LIST_REQ:
                        MyLoading.NotifySHAFile(this,iPacket);
                        break;
                    case CenterOpcodes.ENU_VERIFY_ACCOUNT_REQ:
                        MyUser.OnLogin(this, iPacket);
                        break;
                    case CenterOpcodes.ENU_CLIENT_PING_CONFIG_REQ:
                        OnClientPingConfig();
                        break;
                    case CenterOpcodes.ENU_GUIDE_BOOK_LIST_REQ:
                        MyUser.OnGuideBookList(this);
                        break;

                    default:
                        {
                            LogFactory.GetLog("Main").LogWarning( "Pacote Desconhecido Recebido. Opcode: {0}({1})", (int)uOpcode, uOpcode.ToString() );
                            LogFactory.GetLog("Main").LogHex("Pacote", iPacket.ToArray());
                            break;
                        }
                }
            }
            catch( Exception e )
            {
                LogFactory.GetLog("Main").LogError(e.ToString());
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
