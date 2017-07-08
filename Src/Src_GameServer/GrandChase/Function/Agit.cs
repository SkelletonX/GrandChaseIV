using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrandChase.IO.Packet;
using GrandChase.Net;
using GrandChase.Net.Client;
using GrandChase.Data;
using GrandChase.Security;
using GrandChase.IO;
using GrandChase.Function;
using Manager.Factories;
using Manager;
namespace GrandChase.Function
{
    public class Agit
    {
        public void EnterAgit(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_ENTER_AGIT_ACK))
            {
                oPacket.WriteInt(0);
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 06 00 00 00 15 00 00 00 15 00 00 00 15 1D 00 00 00 00 00 00 00 09 00 09 3C 60 00 00 00 01 30 A5 3E C4 FF FF FF FF FF FF FF FF 00 09 3C 60 00 00 00 01 30 A5 3E C5 FF FF FF FF FF FF FF FF 00 09 3D 82 00 00 00 01 30 A5 3E CB FF FF FF FF FF FF FF FF 00 09 3C 56 00 00 00 01 30 A5 3E CC FF FF FF FF FF FF FF FF 00 09 3C 7E 00 00 00 01 30 A5 3E CD FF FF FF FF FF FF FF FF 00 09 3D BE 00 00 00 01 30 A5 3E CE FF FF FF FF FF FF FF FF 00 09 3D D2 00 00 00 01 30 A5 3E CF FF FF FF FF FF FF FF FF 00 0A 05 DC 00 00 00 01 30 A5 3E D0 FF FF FF FF FF FF FF FF 00 0A 05 E6 00 00 00 01 30 A5 3E D1 FF FF FF FF FF FF FF FF 00 00 00 00 57 A6 DC 70 00 00 00 00");
                oPacket.WriteIPFromString(Server.UDPRelayIP, true);
                oPacket.WriteShort(Server.TCPRelayPort);//2D 50 0C C6 25 E4
                oPacket.WriteIPFromString(Server.UDPRelayIP, true);
                oPacket.WriteShort(Server.UDPRelayPort);
                oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 58 29 FB DC 59 21 3A F0 00 59 21 9F DB 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 1C BD");
                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void AgitLoadComplete(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_AGIT_VISITER_COUNT_BROAD))
            {
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(22);
                oPacket.WriteInt(22);
                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_AGIT_LOADING_COMPLETE_ACK))
            {
                oPacket.WriteInt(0);
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(1);//Users in Agit //Length Visiter Count
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(4);
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(cs.Nick.Length*2);
                oPacket.WriteUnicodeString(cs.Nick);
                oPacket.WriteByte((byte)cs.CurrentChar);
                int MyCharPos = -1;
                for (int t = 0; t < cs.MyCharacter.MyChar.Length; t++)
                    if (cs.MyCharacter.MyChar[t].CharType == cs.CurrentChar)
                        MyCharPos = t;
                oPacket.WriteByte((byte)cs.MyCharacter.MyChar[MyCharPos].Promotion);
                oPacket.WriteInt(cs.MyCharacter.MyChar[MyCharPos].Level);
                oPacket.WriteInt(0);
                oPacket.WriteHexString("00 00 00 01 00 0A 9A 2E 00 00 00 01 33 F5 2A 7B 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 E4 82 00 00 00 1A 53 00 71 00 75 00 69 00 72 00 65 00 20 00 47 00 61 00 69 00 6B 00 6F 00 7A 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 FF DB 30 31 C8 67 2D 00 00 00 01 00 00 00 00 06 00 00 00 00 00 00 00 01 00 00 03 4F 00 00 00 00 00 00 00 02 00 00 03 51 00 00 00 00 00 00 00 05 00 00 03 4A 00 00 00 00 00 00 00 06 00 00 03 50 00 00 00 00 00 00 00 07 00 00 03 4D 00 00 00 00 00 00 00 0B 00 00 03 4E 00 00 00 00 00 00 00 00");
                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

    }
}
