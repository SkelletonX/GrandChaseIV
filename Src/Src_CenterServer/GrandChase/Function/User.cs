using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrandChase.IO.Packet;
using GrandChase.Net.Client;
using GrandChase.Data;
using GrandChase.Security;
using GrandChase.IO;
using Manager.Factories;
using Manager;

namespace GrandChase.Function
{
    public class User
    {
        public struct ServerList
        {
            public string ServerName;
            public string ServerDesc;
            public string ServerIP;
            public int ServerPort;
            public int Users;
            public int MaxUsers;
            public int Flag;
            public int ServerType;
        }
        public ServerList[] serverlist = new ServerList[0];
        public static int MaxLevel;

        public static int SetMaxLevel()
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT   `MaxLevel` FROM  `maxlevel`");

            MaxLevel = Convert.ToInt32(ds.Tables[0].Rows[0]["MaxLevel"].ToString());
            return MaxLevel;
        }

        public void LoadServerList()
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT  * FROM  `Servidores`");
            Array.Resize(ref serverlist, ds.Tables[0].Rows.Count);
            for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
            {
                serverlist[a].ServerName = Convert.ToString(ds.Tables[0].Rows[a]["ServerName"].ToString());
                serverlist[a].ServerDesc = Convert.ToString(ds.Tables[0].Rows[a]["ServerDesc"].ToString());
                serverlist[a].ServerIP = Convert.ToString(ds.Tables[0].Rows[a]["ServerIP"].ToString());
                serverlist[a].ServerPort = Convert.ToInt32(ds.Tables[0].Rows[a]["ServerPort"].ToString());
                serverlist[a].Users = Convert.ToInt32(ds.Tables[0].Rows[a]["Users"].ToString());
                serverlist[a].MaxUsers = Convert.ToInt32(ds.Tables[0].Rows[a]["MaxUsers"].ToString());
                serverlist[a].Flag = Convert.ToInt32(ds.Tables[0].Rows[a]["Flag"].ToString());
                serverlist[a].ServerType = Convert.ToInt32(ds.Tables[0].Rows[a]["Servertype"].ToString());
            }
        }
        public void SendServerList(ClientSession cs)
        {
            /*string ServerName = Settings.GetString("GameServer/ServerName");
            string ServerDesc = Settings.GetString("GameServer/ServerDesc");
            string ServerIP = Settings.GetString("GameServer/ExternalIP");
            short ServerPort = Settings.GetShort("GameServer/Port");

            string TestServerName = Settings.GetString("GameServerTest/ServerName");
            string TestServerDesc = Settings.GetString("GameServerTest/ServerDesc");
            string TestServerIP = Settings.GetString("GameServerTest/ExternalIP");
            short TestServerPort = Settings.GetShort("GameServerTest/Port");*/

            using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_SERVER_LIST_NOT))
            {
                LoadServerList();
                oPacket.WriteInt(serverlist.Length); // 서버 개수
                int i = 0;
                for (int j = 0; j < serverlist.Length; j++)
                {
                    oPacket.WriteInt(i+1); // 서버 번호
                    oPacket.WriteInt(i+1); // 서버 번호 2
                    oPacket.WriteInt(serverlist[j].ServerName.Length * 2);
                    oPacket.WriteUnicodeString(serverlist[j].ServerName);
                    oPacket.WriteInt(serverlist[j].ServerIP.Length);
                    oPacket.WriteString(serverlist[j].ServerIP);
                    oPacket.WriteShort((short)serverlist[j].ServerPort);
                    oPacket.WriteInt(serverlist[j].Users); // 현재 접속중인 인원
                    oPacket.WriteInt(serverlist[j].MaxUsers); // 최대 접속가능 인원
                    oPacket.WriteInt(serverlist[j].Flag);//00 00 01 43 //323
                    oPacket.WriteHexString("FF FF FF FF FF FF FF FF");
                    oPacket.WriteInt(serverlist[j].ServerIP.Length); // 아이피를 또 보낸다.
                    oPacket.WriteString(serverlist[j].ServerIP); // ㅄ
                    oPacket.WriteInt(serverlist[j].ServerDesc.Length * 2);
                    oPacket.WriteUnicodeString(serverlist[j].ServerDesc);
                    oPacket.WriteInt(serverlist[j].ServerType);
                }

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void SendChannelNews(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_CHANNEL_NEWS_NOT))
            {
                oPacket.WriteHexString("00 00 00 00 00 00 00 00 01");

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public struct EventOpenInfo
        { 
            public int EventID;
            public int EventMBoxID;
            public string EventTextureFileName;
            public int EventItem1;
            public int EventItem2;
        }
        public EventOpenInfo[] eventsinfo = new EventOpenInfo[0];
        public void LoadContents()
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT * FROM EventOpenInfo");

            Array.Resize(ref eventsinfo, ds.Tables[0].Rows.Count);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                eventsinfo[i].EventID = Convert.ToInt32(ds.Tables[0].Rows[i]["EventID"].ToString());
                eventsinfo[i].EventMBoxID = Convert.ToInt32(ds.Tables[0].Rows[i]["EventMBoxID"].ToString());
                eventsinfo[i].EventTextureFileName = Convert.ToString(ds.Tables[0].Rows[i]["EventTextureFileName"].ToString());
                eventsinfo[i].EventItem1 = Convert.ToInt32(ds.Tables[0].Rows[i]["EventItem1"].ToString());
                eventsinfo[i].EventItem2 = Convert.ToInt32(ds.Tables[0].Rows[i]["EventItem2"].ToString());
            }
        }

        public struct CPPromotion
        {
            public int CharID;
            public int Promotion;
        }

        public struct CPCharacter
        {
            public int UID;
            public int CharID;
            public CPPromotion[] CCPPromotion;
        }
        public CPCharacter[] CCPCharacter = new CPCharacter[0];


        public void LoadCharacters()
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT  * FROM  `gc`.`cpcharacter` ORDER BY UID ASC");

            Array.Resize(ref CCPCharacter, ds.Tables[0].Rows.Count);

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                CCPCharacter[j].UID = Convert.ToInt32(ds.Tables[0].Rows[j]["uid"].ToString());
                CCPCharacter[j].CharID= Convert.ToInt32(ds.Tables[0].Rows[j]["CharID"].ToString());

                DataSet ds2 = new DataSet();
                Database.Query(ref ds2, "SELECT   `CharID`,  `Promotion` FROM  `gc`.`cppromotion` WHERE `CharID`='{0}'", CCPCharacter[j].CharID);

                Array.Resize(ref CCPCharacter[j].CCPPromotion, ds2.Tables[0].Rows.Count);
                for (int k = 0; k < ds2.Tables[0].Rows.Count; k++)
                {                    
                    CCPCharacter[j].CCPPromotion[k].CharID = Convert.ToInt32(ds2.Tables[0].Rows[k]["CharID"].ToString());
                    CCPCharacter[j].CCPPromotion[k].Promotion = Convert.ToInt32(ds2.Tables[0].Rows[k]["Promotion"].ToString());
                }
            }
        }
        public void SendClientContentOpen(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_NEW_CLIENT_CONTENTS_OPEN_NOT))
            {
                LoadContents();
                LoadCharacters();
                //oPacket.WriteHexString("00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 09 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 09 00 00 00 01 00 00 00 01 00 00 00 0A 00 00 00 01 00 00 00 01 00 00 00 0E 00 00 00 01 00 00 00 01 00 00 00 12 00 00 00 01 00 00 00 01 00 00 00 14 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 01 00 00 00 03 00 00 00 03 00 00 00 01 00 00 00 00 00 00 00 15 00 00 00 01 00 00 00 0D 00 00 00 08 00 00 00 00 00 00 00 02 00 00 00 01 00 00 00 02 00 00 00 01 00 00 00 01 00 00 00 05 00 00 00 03 00 00 00 01 00 00 00 06 00 00 00 04 00 00 00 01 00 00 00 00 00 00 00 05 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 02 00 00 00 42 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1E 00 00 00 24 00 00 00 27 00 00 00 28 00 00 00 29 00 00 00 2A 00 00 00 2B 00 00 00 2C 00 00 00 2D 00 00 00 2E 00 00 00 2F 00 00 00 30 00 00 00 31 00 00 00 32 00 00 00 37 00 00 00 38 00 00 00 39 00 00 00 3A 00 00 00 3B 00 00 00 3C 00 00 00 3D 00 00 00 3E 00 00 00 3F 00 00 00 40 00 00 00 46 00 00 00 49 00 00 00 4A 00 00 00 4C 00 00 00 4E 00 00 00 4F 00 00 00 50 00 00 00 51 00 00 00 53 00 00 00 54 00 00 00 55 00 00 00 56 00 00 00 57 00 00 00 58 00 00 00 59 00 00 00 5A 00 00 00 5B 00 00 00 5C 00 00 00 5D 00 00 00 5E 00 00 00 5F 00 00 00 0B 00 00 00 01 00 00 00 26 00 00 00 0D 00 00 00 01 00 00 00 42 00 00 00 06 00 00 00 04 00 00 00 00 01 00 00 00 63 00 00 00 00 01 00 00 00 10 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 2B 00 00 00 2F 00 00 00 01 01 00 00 00 0E 00 00 00 0A 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 09 00 00 00 04 00 00 00 06 00 00 00 05 00 00 00 08 00 00 00 07 00 00 00 0B 00 00 00 05 01 00 00 00 10 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 2B 00 00 00 2F 00 00 00 03 01 00 00 00 09 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 09 00 00 00 0B 00 00 00 02 00 00 00 0A 00 00 00 04 00 00 00 0B 00 00 00 00 10 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 2B 00 00 00 2F 00 00 00 06 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 55 00 00 00 14 00 00 00 00 00 00 00 04 00 01 02 03 00 00 00 01 00 00 00 04 00 01 02 03 00 00 00 02 00 00 00 04 00 01 02 03 00 00 00 03 00 00 00 04 00 01 02 03 00 00 00 04 00 00 00 04 00 01 02 03 00 00 00 05 00 00 00 04 00 01 02 03 00 00 00 06 00 00 00 04 00 01 02 03 00 00 00 07 00 00 00 04 00 01 02 03 00 00 00 08 00 00 00 04 00 01 02 03 00 00 00 09 00 00 00 04 00 01 02 03 00 00 00 0A 00 00 00 04 00 01 02 03 00 00 00 0B 00 00 00 04 00 01 02 03 00 00 00 0C 00 00 00 04 00 01 02 03 00 00 00 0D 00 00 00 04 00 01 02 03 00 00 00 0E 00 00 00 04 00 01 02 03 00 00 00 0F 00 00 00 02 00 01 00 00 00 10 00 00 00 02 00 01 00 00 00 11 00 00 00 02 00 01 00 00 00 12 00 00 00 02 00 01 00 00 00 13 00 00 00 01 00 00 00 00 13 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 00 00 00 00 02 00 00 00 01 00 00 00 02 00 00 00 02 00 00 00 02 00 00 00 03 00 00 00 01 00 00 00 04 00 00 00 01 00 00 00 05 00 00 00 01 00 00 00 06 00 00 00 01 00 00 00 07 00 00 00 01 00 00 00 08 00 00 00 01 00 00 00 09 00 00 00 01 00 00 00 0A 00 00 00 01 00 00 00 0B 00 00 00 01 00 00 00 0C 00 00 00 01 00 00 00 0D 00 00 00 01 00 00 00 0E 00 00 00 00 00 00 00 0F 00 00 00 01 00 00 00 10 00 00 00 01 00 00 00 11 00 00 00 00 00 00 00 12 00 00 00 00 00 00 00 12 00 00 00 00 00 01 83 3F 00 00 00 01 00 01 83 40 00 00 00 02 00 01 83 41 00 00 00 03 00 01 83 42 00 00 00 04 00 01 83 43 00 00 00 05 00 01 83 44 00 00 00 06 00 01 83 45 00 00 00 07 00 01 83 46 00 00 00 08 00 01 83 47 00 00 00 09 00 01 83 48 00 00 00 0A 00 01 83 49 00 00 00 0B 00 01 83 4A 00 00 00 0C 00 01 83 4B 00 00 00 0D 00 01 83 4C 00 00 00 0E 00 01 83 4D 00 00 00 0F 00 01 83 4E 00 00 00 10 00 01 83 4F 00 00 00 11 00 01 E3 32 00 00 00 43 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 26 00 00 00 27 00 00 00 28 00 00 00 29 00 00 00 2A 00 00 00 2B 00 00 00 2C 00 00 00 2D 00 00 00 2E 00 00 00 2F 00 00 00 30 00 00 00 31 00 00 00 32 00 00 00 33 00 00 00 34 00 00 00 39 00 00 00 3A 00 00 00 3B 00 00 00 3C 00 00 00 3D 00 00 00 3E 00 00 00 3F 00 00 00 40 00 00 00 41 00 00 00 42 00 00 00 43 00 00 00 44 00 00 00 45 00 00 00 46 00 00 00 47 00 00 00 02 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 13 00 00 00 18 00 00 00 03 00 00 22 56 00 00 00 8E 00 00 00 3C 74 00 65 00 78 00 5F 00 67 00 63 00 5F 00 6D 00 62 00 6F 00 78 00 5F 00 67 00 61 00 77 00 69 00 62 00 61 00 77 00 69 00 62 00 6F 00 5F 00 64 00 6C 00 67 00 2E 00 64 00 64 00 73 00 00 00 00 01 00 06 4E F6 00 00 25 40 00 00 00 B3 00 00 00 36 74 00 65 00 78 00 5F 00 67 00 63 00 5F 00 6D 00 62 00 6F 00 78 00 5F 00 66 00 72 00 69 00 65 00 6E 00 64 00 5F 00 67 00 69 00 66 00 74 00 2E 00 64 00 64 00 73 00 00 00 00 01 00 0C C5 D8 00 00 27 D2 00 00 00 BF 00 00 00 38 74 00 65 00 78 00 5F 00 67 00 63 00 5F 00 6D 00 62 00 6F 00 78 00 5F 00 73 00 6F 00 6E 00 67 00 6B 00 72 00 61 00 6E 00 5F 00 64 00 6C 00 67 00 2E 00 64 00 64 00 73 00 00 00 00 01 00 0D 5E 44");                
                oPacket.WriteInt(0);//00 00 00 00

                oPacket.WriteInt(3);//00 00 00 03 00 00 00 00 00 00 00 01 00 00 00 02
                for (int c = 0; c < 3; c++)
                {
                    oPacket.WriteInt(c);
                }
                oPacket.WriteHexString("00 00 00 09 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 09 00 00 00 01 00 00 00 01 00 00 00 0A 00 00 00 01 00 00 00 01 00 00 00 0E 00 00 00 01 00 00 00 01 00 00 00 12 00 00 00 01 00 00 00 01 00 00 00 14 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 01 00 00 00 03 00 00 00 03 00 00 00 01 00 00 00 00 00 00 00 15 00 00 00 01 00 00 00 0D 00 00 00 08 00 00 00 00 00 00 00 02 00 00 00 01 00 00 00 02 00 00 00 01 00 00 00 01 00 00 00 05 00 00 00 03 00 00 00 01 00 00 00 06 00 00 00 04 00 00 00 01 00 00 00 00 00 00 00 05 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 02 00 00 00 42 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1E 00 00 00 24 00 00 00 27 00 00 00 28 00 00 00 29 00 00 00 2A 00 00 00 2B 00 00 00 2C 00 00 00 2D 00 00 00 2E 00 00 00 2F 00 00 00 30 00 00 00 31 00 00 00 32 00 00 00 37 00 00 00 38 00 00 00 39 00 00 00 3A 00 00 00 3B 00 00 00 3C 00 00 00 3D 00 00 00 3E 00 00 00 3F 00 00 00 40 00 00 00 46 00 00 00 49 00 00 00 4A 00 00 00 4C 00 00 00 4E 00 00 00 4F 00 00 00 50 00 00 00 51 00 00 00 53 00 00 00 54 00 00 00 55 00 00 00 56 00 00 00 57 00 00 00 58 00 00 00 59 00 00 00 5A 00 00 00 5B 00 00 00 5C 00 00 00 5D 00 00 00 5E 00 00 00 5F 00 00 00 0B 00 00 00 01 00 00 00 26 00 00 00 0D 00 00 00 01 00 00 00 42 00 00 00 06 00 00 00 04 00 00 00 00 01 00 00 00 63 00 00 00 00 01 00 00 00 10 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 2B 00 00 00 2F 00 00 00 01 01 00 00 00 0E 00 00 00 0A 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 09 00 00 00 04 00 00 00 06 00 00 00 05 00 00 00 08 00 00 00 07 00 00 00 0B 00 00 00 05 01 00 00 00 10 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 2B 00 00 00 2F 00 00 00 03 01 00 00 00 09 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 09 00 00 00 0B 00 00 00 02 00 00 00 0A 00 00 00 04 00 00 00 0B 00 00 00 00 10 00 00 00 63 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 2B 00 00 00 2F");

                oPacket.WriteInt(6);//00 00 00 06 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05
                for (int c = 0; c < 6; c++)
                {
                    oPacket.WriteInt(c);
                }

                oPacket.WriteInt(SetMaxLevel());//00 00 00 55 
                oPacket.WriteInt(CCPCharacter.Length);//13
                for (int a = 0; a < CCPCharacter.Length; a++)
                {
                    oPacket.WriteInt(CCPCharacter[a].CharID);
                    oPacket.WriteInt(CCPCharacter[a].CCPPromotion.Length);
                    for (int b = 0; b < CCPCharacter[a].CCPPromotion.Length; b++)
                    {
                        oPacket.WriteByte((byte)CCPCharacter[a].CCPPromotion[b].Promotion);
                    }
                }
                oPacket.WriteHexString("00 00 00 13 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13"); 
                oPacket.WriteHexString("00 00 00 00 00 00 00 02 00 00 00 01 00 00 00 02 00 00 00 02 00 00 00 02 00 00 00 03 00 00 00 01 00 00 00 04 00 00 00 01 00 00 00 05 00 00 00 01 00 00 00 06 00 00 00 01 00 00 00 07 00 00 00 01 00 00 00 08 00 00 00 01 00 00 00 09 00 00 00 01 00 00 00 0A 00 00 00 01 00 00 00 0B 00 00 00 01 00 00 00 0C 00 00 00 01 00 00 00 0D 00 00 00 01 00 00 00 0E 00 00 00 00 00 00 00 0F 00 00 00 01 00 00 00 10 00 00 00 01 00 00 00 11 00 00 00 00 00 00 00 12 00 00 00 00");
                oPacket.WriteHexString("00 00 00 12 00 00 00 00 00 01 83 3F 00 00 00 01 00 01 83 40 00 00 00 02 00 01 83 41 00 00 00 03 00 01 83 42 00 00 00 04 00 01 83 43 00 00 00 05 00 01 83 44 00 00 00 06 00 01 83 45 00 00 00 07 00 01 83 46 00 00 00 08 00 01 83 47 00 00 00 09 00 01 83 48 00 00 00 0A 00 01 83 49 00 00 00 0B 00 01 83 4A 00 00 00 0C 00 01 83 4B 00 00 00 0D 00 01 83 4C 00 00 00 0E 00 01 83 4D 00 00 00 0F 00 01 83 4E 00 00 00 10 00 01 83 4F 00 00 00 11 00 01 E3 32");
                oPacket.WriteHexString("00 00 00 43 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 26 00 00 00 27 00 00 00 28 00 00 00 29 00 00 00 2A 00 00 00 2B 00 00 00 2C 00 00 00 2D 00 00 00 2E 00 00 00 2F 00 00 00 30 00 00 00 31 00 00 00 32 00 00 00 33 00 00 00 34 00 00 00 39 00 00 00 3A 00 00 00 3B 00 00 00 3C 00 00 00 3D 00 00 00 3E 00 00 00 3F 00 00 00 40 00 00 00 41 00 00 00 42 00 00 00 43 00 00 00 44 00 00 00 45 00 00 00 46 00 00 00 47");
                oPacket.WriteHexString("00 00 00 02 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 13");
                oPacket.WriteInt(Enum.GetNames(typeof(MenuItems.list)).Length);//00 00 00 18
                oPacket.WriteInt(eventsinfo.Length);
                LogFactory.GetLog("MAIN").LogInfo("Events Infos: "+eventsinfo.Length);
                for (int a = 0; a < eventsinfo.Length; a++)
                {
                    oPacket.WriteInt(eventsinfo[a].EventID);
                    oPacket.WriteInt(eventsinfo[a].EventMBoxID);
                    oPacket.WriteInt(eventsinfo[a].EventTextureFileName.Length*2);
                    oPacket.WriteUnicodeString(eventsinfo[a].EventTextureFileName);
                    oPacket.WriteInt(eventsinfo[a].EventItem1);
                    oPacket.WriteInt(eventsinfo[a].EventItem2);
                }
                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void SendSocketTableInfo(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_SOCKET_TABLE_INFO_NOT))
            {
                oPacket.WriteHexString("00 00 00 65 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 02 00 00 00 01 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 03 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 04 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 05 00 00 00 02 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 08 00 00 00 03 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 09 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 0A 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 0B 00 00 00 04 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 0C 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 0D 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 0E 00 00 00 05 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 0F 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 10 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 11 00 00 00 06 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 12 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 13 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 14 00 00 00 07 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 15 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 16 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 17 00 00 00 08 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 18 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 19 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 1A 00 00 00 09 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 1B 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 1C 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 1D 00 00 00 0A 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 1E 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 1F 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 20 00 00 00 0B 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 21 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 22 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 23 00 00 00 0C 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 24 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 25 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 26 00 00 00 0D 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 27 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 28 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 29 00 00 00 0E 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 2A 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 2B 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 2C 00 00 00 0F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 2D 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 2E 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 2F 00 00 00 10 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 30 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 31 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 32 00 00 00 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 33 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 34 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 35 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 36 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 37 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 38 00 00 00 13 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 39 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 3A 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 3B 00 00 00 14 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 3C 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 3D 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 3E 00 00 00 15 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 3F 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 40 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 41 00 00 00 16 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 42 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 43 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 44 00 00 00 17 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 45 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 46 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 47 00 00 00 18 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 48 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 49 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 4A 00 00 00 19 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 4B 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 4C 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 4D 00 00 00 1A 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 4E 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 4F 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 50 00 00 00 1B 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 51 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 52 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 53 00 00 00 1C 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 54 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 55 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 56 00 00 00 1D 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 57 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 58 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 59 00 00 00 1E 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 5A 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 5B 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 5C 00 00 00 1F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 5D 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 5E 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 5F 00 00 00 20 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 60 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 61 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 62 00 00 00 21 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 63 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 25 00 00 00 64 00 00 00 22 00 00 00 23 00 00 00 24 00 00 00 25 00 00 00 65 00 00 00 00 00 00 00 BE 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 00 01 00 00 00 BE 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 00 02 00 00 00 BE 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 00 03 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 00 04 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 00 05 00 00 00 FA 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 00 06 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 00 07 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 00 08 00 00 01 4A 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 00 09 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 00 0A 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 00 0B 00 00 01 D6 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 00 0C 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 00 0D 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 00 0E 00 00 03 52 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 00 0F 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 00 10 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 00 11 00 00 08 98 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 00 12 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 00 13 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 00 14 00 00 0E 74 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 00 15 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 00 16 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 00 17 00 00 17 0C 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 00 18 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 00 19 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 00 1A 00 00 22 C4 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 00 1B 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 00 1C 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 00 1D 00 00 31 9C 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 00 1E 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 00 1F 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 00 20 00 00 55 F0 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 00 21 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 00 22 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 00 23 00 00 6C 98 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 00 24 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 00 25 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 00 26 00 00 84 08 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 00 27 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 00 00 28 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 00 00 29 00 00 9C 40 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 00 00 2A 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 00 00 2B 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 00 00 2C 00 00 B9 28 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 00 00 2D 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 01 47 58 00 00 00 2E 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 01 47 58 00 00 00 2F 00 00 D8 CC 00 00 F8 70 00 01 1D 28 00 01 47 58 00 00 00 30 00 00 F8 70 00 01 1D 28 00 01 47 58 00 01 7D 40 00 00 00 31 00 00 F8 70 00 01 1D 28 00 01 47 58 00 01 7D 40 00 00 00 32 00 00 F8 70 00 01 1D 28 00 01 47 58 00 01 7D 40 00 00 00 33 00 01 1D 28 00 01 47 58 00 01 7D 40 00 01 B7 74 00 00 00 34 00 01 1D 28 00 01 47 58 00 01 7D 40 00 01 B7 74 00 00 00 35 00 01 1D 28 00 01 47 58 00 01 7D 40 00 01 B7 74 00 00 00 36 00 01 47 58 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 00 00 37 00 01 47 58 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 00 00 38 00 01 47 58 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 00 00 39 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 00 00 3A 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 00 00 3B 00 01 7D 40 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 00 00 3C 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 00 00 3D 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 00 00 3E 00 01 B7 74 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 00 00 3F 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 03 15 10 00 00 00 40 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 03 15 10 00 00 00 41 00 02 00 E4 00 02 50 F8 00 02 AC C4 00 03 15 10 00 00 00 42 00 02 50 F8 00 02 AC C4 00 03 15 10 00 03 BB 78 00 00 00 43 00 02 50 F8 00 02 AC C4 00 03 15 10 00 03 BB 78 00 00 00 44 00 02 50 F8 00 02 AC C4 00 03 15 10 00 03 BB 78 00 00 00 45 00 02 AC C4 00 03 15 10 00 03 BB 78 00 04 27 48 00 00 00 46 00 02 AC C4 00 03 15 10 00 03 BB 78 00 04 27 48 00 00 00 47 00 02 AC C4 00 03 15 10 00 03 BB 78 00 04 27 48 00 00 00 48 00 03 15 10 00 03 BB 78 00 04 27 48 00 04 AC E0 00 00 00 49 00 03 15 10 00 03 BB 78 00 04 27 48 00 04 AC E0 00 00 00 4A 00 03 15 10 00 03 BB 78 00 04 27 48 00 04 AC E0 00 00 00 4B 00 03 BB 78 00 04 27 48 00 04 AC E0 00 05 32 78 00 00 00 4C 00 03 BB 78 00 04 27 48 00 04 AC E0 00 05 32 78 00 00 00 4D 00 03 BB 78 00 04 27 48 00 04 AC E0 00 05 32 78 00 00 00 4E 00 04 27 48 00 04 AC E0 00 05 32 78 00 05 B8 10 00 00 00 4F 00 04 27 48 00 04 AC E0 00 05 32 78 00 05 B8 10 00 00 00 50 00 04 27 48 00 04 AC E0 00 05 32 78 00 05 B8 10 00 00 00 51 00 04 AC E0 00 05 32 78 00 05 B8 10 00 06 3D A8 00 00 00 52 00 04 AC E0 00 05 32 78 00 05 B8 10 00 06 3D A8 00 00 00 53 00 04 AC E0 00 05 32 78 00 05 B8 10 00 06 3D A8 00 00 00 54 00 05 32 78 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 00 00 55 00 05 32 78 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 00 00 56 00 05 32 78 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 00 00 57 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 07 48 74 00 00 00 58 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 07 48 74 00 00 00 59 00 05 B8 10 00 06 3D A8 00 06 C3 40 00 07 48 74 00 00 00 5A 00 06 3D A8 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 00 00 5B 00 06 3D A8 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 00 00 5C 00 06 3D A8 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 00 00 5D 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 00 00 5E 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 00 00 5F 00 06 C3 40 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 00 00 60 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 00 00 61 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 00 00 62 00 07 48 74 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 00 00 63 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 09 5E D4 00 00 00 64 00 07 CE 0C 00 08 53 A4 00 08 D9 3C 00 09 5E D4 00 04 61 54");

                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void SendCashbackRatio(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_CASHBACK_RATIO_INFO_NOT))
            {
                oPacket.WriteHexString("00 00 00 00 00 00 00 00");

                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }             

        public void OnLogin(ClientSession cs, InPacket ip)
        {
            int IDLength = ip.ReadInt();
            string ID = ip.ReadString(IDLength);
            int PWLength = ip.ReadInt();
            string PW = ip.ReadString(PWLength);
            ip.Skip(21);
            int crc32len = ip.ReadInt();
            string crc32 = ip.ReadString(crc32len);
            LogFactory.GetLog("CRC").LogInfo("LENGTH: "+crc32len+" CRC: " + crc32);

            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT * FROM account WHERE Login = '{0}' AND Passwd = '{1}'", ID, PW);


            if (ds.Tables[0].Rows.Count == 0)
            {
                // 로그인 실패
                LogFactory.GetLog("MAIN").LogInfo("{0}의 로그인이 실패하였습니다.", ID);

                using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_VERIFY_ACCOUNT_ACK))
                {
                    oPacket.WriteHexString("00 00 00 14");
                    oPacket.WriteInt(IDLength * 2);
                    oPacket.WriteUnicodeString(ID);
                    oPacket.WriteHexString("00 00 00 00 00");

                    oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                    cs.Send(oPacket);
                }
            }
            else
            {
                SendServerList(cs);
                SendChannelNews(cs);
                //SendShop(cs);
                SendClientContentOpen(cs);
                SendSocketTableInfo(cs);
                SendCashbackRatio(cs);

                using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_VERIFY_ACCOUNT_ACK))
                {
                    oPacket.WriteHexString("00 00 00 00");
                    oPacket.WriteInt(IDLength * 2);
                    oPacket.WriteUnicodeString(ID);
                    oPacket.WriteInt(PWLength);
                    oPacket.WriteString(PW);

                    oPacket.WriteHexString("00 00 00 00 14 00 8E A7 C5 01 00 00 00 00 00 00 02 4B 52 00 05 A3 BD 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00");

                    oPacket.WriteInt(cs.MyLoading.GuildMarkURL.Length * 2);
                    oPacket.WriteUnicodeString(cs.MyLoading.GuildMarkURL);
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00");

                    //checklist
                    int SHAi = cs.MyLoading.checklist.Length;
                    oPacket.WriteInt(SHAi);
                    for (int i = 0; i <= cs.MyLoading.checklist.Length - 1; i++)
                    {
                        oPacket.WriteInt(cs.MyLoading.checklist[i].nFile.Length * 2);
                        oPacket.WriteUnicodeString(cs.MyLoading.checklist[i].nFile);
                        oPacket.WriteBool(true);
                    }
                    oPacket.WriteHexString("00 00 00 01 00 00 00 03 00 00 00 00 00 00 00 00 64 01 00 00 00 00 00 00 00 64 02 00 00 00 00 00 00 00 64 01 BF 80 00 00 FC 04 97 FF 00 00 00 00 00 00 00 00 00 00 00 00");

                    oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                    cs.Send(oPacket);
                }
            }
        }

        // 가이드북 리스트라고 써있는데 뭔지 모름
        public void OnGuideBookList(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(CenterOpcodes.ENU_GUIDE_BOOK_LIST_ACK))
            {
                oPacket.WriteHexString("00 00 00 01 00 00 00 00");
                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }
    }
}
