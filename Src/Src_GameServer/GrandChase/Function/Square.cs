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
    public class Square
    {
        public string ServerName = "Grand Chase Hero";
        public string ServerIP;
        public int MaxPlayers = 50 ;

        public struct InfoPlayers
        {
            public int guildidimage;
            public string guildimage;
            public string guildname;
            public string playerName;
            public int charid;
            public string nick;
            public string loginuid;
        }

        public List<ClientSession> PlayersList { get; set; }
        public ushort CurrentPlayers
        {
            get { return (ushort)(PlayersList == null ? 0 : PlayersList.Count); }
        }

        public Square()
        { 
            PlayersList = new List<ClientSession>();
        }

        int TotalServers = Settings.GetInt("Squares/TotalServers");
        
        
        public void SquareList(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_SQUARE_LIST_ACK))
            {                    
                    LogFactory.GetLog("MAIN").LogInfo("PLAYERS IN SQUARE: " + CurrentPlayers);
                    ServerIP = Settings.GetString("Squares/ServerIP");

                    oPacket.WriteInt(1); //total de servers                    
                    oPacket.WriteInt(1);
                    oPacket.WriteInt(ServerName.Length * 2);
                    oPacket.WriteUnicodeString(ServerName);
                    oPacket.WriteInt(MaxPlayers);//00 00 00 32
                    oPacket.WriteInt(CurrentPlayers);//00 00 00 02
                    oPacket.WriteInt(0);//00 00 00 00
                    oPacket.WriteIPFromString(Server.UDPRelayIP, true);
                    oPacket.WriteShort(Server.TCPRelayPort);//2D 50 0C C6 25 E4
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 01");
                    oPacket.WriteInt(ServerIP.Length * 2);
                    oPacket.WriteUnicodeString(ServerIP);
                    oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                    cs.Send(oPacket);
            }
        }

        public void enterSquare(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_ENTER_SQUARE_ACK))
            {
                PlayersList.Add(cs);
                oPacket.WriteInt(0);//00 00 00 00
                oPacket.WriteInt(CurrentPlayers);//00 00 00 04
                /*for (int i = 0; i < CurrentPlayers; i++)
                {
                    oPacket.WriteInt(1);//00 00 00 01
                    oPacket.WriteInt(4);//00 00 00 04
                    oPacket.WriteInt(1273);//00 00 04 F9
                    oPacket.WriteInt("1273_1.png".Length * 2);//00 00 00 14
                    oPacket.WriteUnicodeString("1273_1.png");//31 00 32 00 37 00 33 00 5F 00 31 00 2E 00 70 00 6E 00 67 00
                    oPacket.WriteInt("BrokenLimit".Length * 2);//00 00 00 16
                    oPacket.WriteUnicodeString("BrokenLimit");//42 00 72 00 6F 00 6B 00 65 00 6E 00 4C 00 69 00 6D 00 69 00 74 00
                    oPacket.WriteInt(cs.LoginUID);//00 01 59 58
                    oPacket.WriteInt(cs.Nick.Length * 2);//00 00 00 08
                    oPacket.WriteUnicodeString(cs.Nick);//41 00 6C 00 79 00 73 00
                    oPacket.WriteByte(4);//04 //CharID
                    oPacket.WriteInt(1);//00 00 00 01
                    oPacket.WriteInt(7);//Length Equips
                    for (int j = 0; j < 7; j++)
                    {
                        oPacket.WriteInt(380070);//itemID                      
                    }
                    oPacket.WriteHexString("00 00 00 0D 00 00 00 00 00 00 00 01 40 F1 99 9A 3E 8A 3D 71 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 12 C8 F2 00 00 E5 74 00 01 BD E6 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                }*/

                foreach (ClientSession u in PlayersList)
                {
                    oPacket.WriteInt(1);//00 00 00 01
                    oPacket.WriteInt(4);//00 00 00 04
                    oPacket.WriteInt(u.MyGuilds.GuildID);//00 00 04 F9
                    oPacket.WriteInt(u.MyGuilds.MarkName.Length * 2);//00 00 00 14
                    oPacket.WriteUnicodeString(u.MyGuilds.MarkName);//31 00 32 00 37 00 33 00 5F 00 31 00 2E 00 70 00 6E 00 67 00
                    oPacket.WriteInt(u.MyGuilds.GuildName.Length * 2);//00 00 00 16
                    oPacket.WriteUnicodeString(u.MyGuilds.GuildName);//42 00 72 00 6F 00 6B 00 65 00 6E 00 4C 00 69 00 6D 00 69 00 74 00
                    oPacket.WriteInt(u.LoginUID);//00 01 59 58
                    oPacket.WriteInt(u.Nick.Length * 2);//00 00 00 08
                    oPacket.WriteUnicodeString(u.Nick);//41 00 6C 00 79 00 73 00
                    int MyCharPos = -1;
                    for (int t = 0; t < u.MyCharacter.MyChar.Length; t++)
                        if (u.MyCharacter.MyChar[t].CharType == u.CurrentChar)
                            MyCharPos = t;
					oPacket.WriteByte((byte)u.MyCharacter.MyChar[MyCharPos].CharType);//0F//CharID        
                    /*oPacket.WriteByte((byte)u.CurrentChar);//0F//CharID                    
                    //oPacket.WriteInt(0);//Equips
                    int MyCharPos = -1;
                    for (int t = 0; t < cs.MyCharacter.MyChar.Length; t++)
                        if (cs.MyCharacter.MyChar[t].CharType == u.CurrentChar)
                            MyCharPos = t;*/
                    oPacket.WriteInt(u.MyCharacter.MyChar[MyCharPos].Level);//00 00 00 01//Level
                    oPacket.WriteInt(u.MyCharacter.MyChar[MyCharPos].Equip.Length);
                    for (int y = 0; y < u.MyCharacter.MyChar[MyCharPos].Equip.Length; y++)
                    {
                        oPacket.WriteInt(u.MyCharacter.MyChar[MyCharPos].Equip[y].ItemID);
                    }
                        //oPacket.WriteHexString("00 00 00 03 00 0D 1D D0 00 0D 1D DA 00 0D 1D E4");
                        oPacket.WriteHexString("00 00 00 04 00 00 00 00 00 00 00 01 40 F1 99 9A 3E 8A 3D 71 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 12 C8 F2 00 00 E5 74 00 01 BD E6 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                    //("00 00 00 04 00 00 00 00 00 00 00 01 41 14 00 00 3E 8A 3D 71 02 00 00 00 00 00 00 00 00 00 00 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 E5 6A 00 00 E5 88 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 06 00 00 13 FB 00 00 00 1E 64 00 65 00 66 00 61 00 75 00 6C 00 74 00 6D 00 61 00 72 00 6B 00 2E 00 70 00 6E 00 67 00 00 00 00 14 45 00 6C 00 69 00 74 00 65 00 4D 00 61 00 66 00 69 00 61 00 00 05 F1 A4 00 00 00 16 42 00 72 00 61 00 69 00 6E 00 4D 00 61 00 73 00 74 00 65 00 72 00 0E 00 00 00 4F 00 00 00 20 00 07 DF 6E 00 0C 43 EC 00 0A E2 4A 00 0A DD C2 00 0A DC F0 00 0A DC FA 00 0A D0 52 00 0A D0 3E 00 0A D0 66 00 0A D0 5C 00 0A D0 48 00 0A D0 70 00 0A DD 72 00 12 4C 24 00 0A DD 36 00 0A DB 06 00 12 4C 2E 00 12 4C 38 00 12 4C 10 00 12 4C 1A 00 12 4C 42 00 12 56 B0 00 0A DD 7C 00 12 56 CE 00 12 4E D6 00 0A DE 1C 00 12 56 A6 00 10 5D BA 00 10 57 16 00 0A DE 12 00 12 56 C4 00 05 0F 6E 00 00 00 04 00 00 00 00 00 00 00 01 40 F1 99 9A 3E 8A 3D 71 0B 00 00 00 00 00 00 13 AF EC 00 00 00 10 50 00 72 00 69 00 6E 00 73 00 69 00 6F 00 6E 00 00 21 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 04 00 12 C8 F2 00 00 E5 74 00 01 BD F0 00 01 BE 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0B 00 13 E6 CE 00 00 00 01 31 FD D9 2D 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E6 D8 00 00 00 01 31 FD D9 2E 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E6 E2 00 00 00 01 31 FD D9 2F 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E6 EC 00 00 00 01 31 FD D9 30 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E8 22 00 00 00 01 31 FD D9 31 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E3 4A 00 00 00 01 31 FD D9 C0 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E3 54 00 00 00 01 31 FD D9 C1 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E3 5E 00 00 00 01 31 FD D9 C2 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E3 68 00 00 00 01 31 FD D9 C3 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E3 72 00 00 00 01 31 FD D9 C4 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 E3 7C 00 00 00 01 31 FD D9 C5 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03");
                }
                oPacket.WriteIPFromString(Server.TCPRelayIP, true);
                oPacket.WriteShort(Server.TCPRelayPort);//2D 50 0C C6 25 E4
                oPacket.WriteHexString("00 00 00 00 00 00 00 01 41 14 00 00 3E 8A 3D 71");
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }


        public void LeaveSquare(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_LEAVE_SQUARE_ACK))
            {
                PlayersList.Remove(cs);
                oPacket.WriteInt(0);
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

    }
}
