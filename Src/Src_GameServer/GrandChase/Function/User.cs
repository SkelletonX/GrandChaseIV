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
        Quests missions = new Quests();
        // 서버 리스트 전송.
        // 하드코딩 주의

        public void LoadServerList()
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT  * FROM  `gc`.`Servidores`");
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
                    oPacket.WriteInt(i + 1);
                    oPacket.WriteInt(i + 1);
                    oPacket.WriteInt(serverlist[j].ServerName.Length * 2);
                    oPacket.WriteUnicodeString(serverlist[j].ServerName);
                    oPacket.WriteInt(serverlist[j].ServerIP.Length);
                    oPacket.WriteString(serverlist[j].ServerIP);
                    oPacket.WriteShort((short)serverlist[j].ServerPort);
                    oPacket.WriteInt(serverlist[j].Users); 
                    oPacket.WriteInt(serverlist[j].MaxUsers); 
                    oPacket.WriteInt(serverlist[j].Flag);//00 00 01 43 //323
                    oPacket.WriteHexString("FF FF FF FF FF FF FF FF");
                    oPacket.WriteInt(serverlist[j].ServerIP.Length);
                    oPacket.WriteString(serverlist[j].ServerIP);
                    oPacket.WriteInt(serverlist[j].ServerDesc.Length * 2);
                    oPacket.WriteUnicodeString(serverlist[j].ServerDesc);
                    oPacket.WriteInt(serverlist[j].ServerType);
                }

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public byte roomid;
        public int guid;
        public int gamecategory;
        public int gamemode;
        public int subgamemode;
        public int randommap;
        public int map;
        public int matchmode;
        public uint UsersInRoom;
        public byte unk2;
        public uint MaxPlayers;
        public byte[] freeslots;
        public bool isPlay;
        public bool RandomMap;
        public byte CharType;

        //Account Infos
        public byte TutorialCheck = 0;


        public void updateLevel(int Level, int CharID, int Exp, int LoginUID)
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "UPDATE   `character` SET  `Level` = '{0}',`Exp` = '{1}' WHERE `LoginUID` = '{2}'  AND `CharType` = '{3}'", Level,Exp, LoginUID, CharID);            
        }

        public void OnCreateRoom(ClientSession cs, InPacket ip)
        {
            ushort blank = ip.ReadUShort();

            int RoomNameLen = ip.ReadInt();
            string RoomName = ip.ReadUnicodeString(RoomNameLen);
            int roomid = ip.ReadByte();
            //ip.ReadBytes(2);
            bool unk1 = false;
            bool Guild = ip.ReadBool();
            int RoomPassLen = ip.ReadInt();
            string RoomPass = ip.ReadUnicodeString(RoomPassLen);
            UsersInRoom = ip.ReadUShort();
            MaxPlayers = ip.ReadUShort();
            isPlay = ip.ReadBool();
            unk2 = ip.ReadByte();
            matchmode = ip.ReadByte();
            gamemode = ip.ReadInt();
            int readMatch = ip.ReadInt();
            RandomMap = ip.ReadBool();
            map = ip.ReadInt();
            uint unkUint = ip.ReadUInt();//00 00 00 12

            byte[] jump1 = ip.ReadBytes(97);//skip 97 bytes
            int LenLoginP = ip.ReadInt();
            string LoginP = ip.ReadString(LenLoginP);
            int LoginUID = ip.ReadInt();
            int lenNick = ip.ReadInt();
            string nick = ip.ReadString(lenNick);
            int indexChar = ip.ReadInt();
            CharType = ip.ReadByte();
            byte classe = ip.ReadByte();
            ip.Skip(23);
            int TotalStages = ip.ReadInt();
            for (int ml = 0; ml < 96; ml++ )
            {
                ip.ReadInt();
                int pr = ip.ReadInt();
                if (pr == 0)
                {
                    ip.Skip(7);
                }
                if(pr == 1)
                {
                    ip.Skip(8);
                }
            }

            ip.ReadByte();
            ip.ReadInt();
            ip.ReadByte();

            int lenGuildID = ip.ReadInt();
            LogFactory.GetLog("GUILD").LogInfo("GUILDID: "+lenGuildID);
            if (lenGuildID > 0)
            {                
                string GuilID = ip.ReadUnicodeString(lenGuildID);
                LogFactory.GetLog("GUILD").LogInfo("GUILDID: " + GuilID);
            }
            int lenGuildName = ip.ReadInt();
            LogFactory.GetLog("GUILD").LogInfo("GUILDID: " + lenGuildName);
            if (lenGuildName > 0)
            {
                string GuilName = ip.ReadUnicodeString(lenGuildName);
                LogFactory.GetLog("GUILD").LogInfo("GUILDNAME: " + GuilName);
            }

            int lenChars = ip.ReadInt();
            LogFactory.GetLog("CHARS").LogInfo("TOTAL: " + lenChars);
            for (int kl = 0; kl < lenChars; kl++)
            {
                byte CharID = ip.ReadByte();
                ip.ReadByte();
                ip.ReadByte();
                int unkk = ip.ReadInt();
                ip.ReadInt();
                int EXP = ip.ReadInt();
                int Level = ip.ReadInt();
                cs.MyCharacter.MyChar[kl].Level = Level;
                cs.MyCharacter.MyChar[kl].Exp = EXP;
                LogFactory.GetLog("MAIN").LogInfo("CharID: " + CharID);
                LogFactory.GetLog("MAIN").LogInfo("Classe: " + classe);                
                LogFactory.GetLog("MAIN").LogInfo("XP: " + EXP);
                LogFactory.GetLog("MAIN").LogInfo("LEVEL: " + Level);
                ip.ReadInt();
                ip.ReadInt();
                ip.ReadInt();
                ip.Skip(191);
                int unkk2 = ip.ReadInt();
                LogFactory.GetLog("MAIN").LogInfo("UNK2: " + unkk2);
                if (unkk2 > 0)
                {
                    ip.ReadByte();
                    int unkk3 = ip.ReadInt();
                    for (int v1 = 0; v1 < unkk3; v1++)
                    {
                        ip.ReadInt();
                        ip.ReadInt();
                        ip.ReadInt();
                    }
                }
                updateLevel(Level, CharID,EXP, cs.LoginUID);
                int MyCharPos = -1;
                for (int t = 0; t < cs.MyCharacter.MyChar.Length; t++)
                    if (cs.MyCharacter.MyChar[t].CharType == cs.CurrentChar)
                        MyCharPos = t;
                cs.MyCharacter.MyChar[MyCharPos].Level = Level;
                ip.Skip(96);                
            }           


            LogFactory.GetLog("MAIN").LogInfo("RoomName: " + RoomName);
            LogFactory.GetLog("MAIN").LogInfo("RoomID: " + roomid);
            LogFactory.GetLog("MAIN").LogInfo("RoomPass: " + RoomPass);
            LogFactory.GetLog("MAIN").LogInfo("Players: " + UsersInRoom);
            LogFactory.GetLog("MAIN").LogInfo("MaxPlayers: " + MaxPlayers);
            LogFactory.GetLog("MAIN").LogInfo("Playing: " + isPlay);
            LogFactory.GetLog("MAIN").LogInfo("MATCH MODE: "+matchmode);
            LogFactory.GetLog("MAIN").LogInfo("GAME MODE: " + gamemode);
            LogFactory.GetLog("MAIN").LogInfo("MAP: " + map);
            LogFactory.GetLog("MAIN").LogInfo("JUMP: " + jump1);
            LogFactory.GetLog("MAIN").LogInfo("LOGIN: " + LoginP);
            LogFactory.GetLog("MAIN").LogInfo("UID: " + LoginUID);
            LogFactory.GetLog("MAIN").LogInfo("Nick: " + nick);
            LogFactory.GetLog("MAIN").LogInfo("INDEX: " + indexChar);

            int Channel = ip.ReadInt();

            ushort usEmptyRoomSlot = cs.CurrentChannel.GetEmptyRoom();

            Room room = new Room();

            lock (cs.CurrentChannel._lock)
            {
                cs.CurrentChannel.RoomsList.Add(room);
                cs.CurrentChannel.RoomsMap.Add(usEmptyRoomSlot, room);
            }

            room.ID = usEmptyRoomSlot;
            room.RoomName = RoomName;
            room.RoomPass = RoomPass;
            room.GameCategory = (int)matchmode;
            room.GameMode = (int)gamemode;
            room.ItemMode = (int)Room.eItemMode.GM_NOITEM;
            room.RandomMap = false;
            room.GameMap = (int)map;
            room.Kick = 3;
            room.Playing = isPlay;
            cs.CurrentChar = CharType;
            for(int i = 0; i < MaxPlayers; i++)
            {
                room.Slot[i].Active = false;
                room.Slot[i].Open = true;
                room.Slot[i].cs = null;
                room.Slot[i].LoadStatus = 0;
                room.Slot[i].State = 0;
                room.Slot[i].AFK = false;
                room.Slot[i].Leader = false;
            }

            room.Slot[0].Active = true;
            room.Slot[0].cs = cs;
            room.Slot[0].Leader = true;
            room.Slot[0].Open = false;

            cs.CurrentRoom = room;

            
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_CREATE_ROOM_ACK))
            {
                oPacket.WriteInt(0);
                oPacket.WriteInt(cs.Login.Length * 2);
                oPacket.WriteUnicodeString(cs.Login);
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(cs.Nick.Length * 2);
                oPacket.WriteUnicodeString(cs.Nick);
                oPacket.WriteInt(room.FindSlotIndex(cs));//room.FindSlotIndex(cs)
                oPacket.WriteByte((byte)cs.CurrentChar);//cs.CurrentChar

                oPacket.WriteHexString("00 FF 00 FF 00 FF 00 00 00 00 00 01 00 00 00 64 00 00");
                oPacket.WriteInt(cs.GamePoint);//00 00 07 D0
                oPacket.WriteBool(cs.PCBang);//00
                oPacket.WriteByte(cs.PCBangType);//00
                oPacket.WriteHexString("00 00 00 4E 00 00 00 07 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 13 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 14 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 15 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 16 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 17 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 18 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 19 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1D 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 24 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 27 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 28 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 29 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 2B 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2C 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2D 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2E 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 30 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 31 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 32 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 33 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 34 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 35 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 36 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 37 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 38 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 39 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3C 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3D 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 40 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 43 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 44 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 45 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 46 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 47 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 48 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 49 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4C 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 50 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 51 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 52 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 53 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 54 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 55 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 56 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 57 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 58 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 59 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5C 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5D 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 01 01 00 00");

                oPacket.WriteBool(cs.IsHost);
                oPacket.WriteBool(cs.IsLive);
                oPacket.WriteInt(cs.MyGuilds.MarkName.Length *2);
                oPacket.WriteUnicodeString(cs.MyGuilds.MarkName);
                oPacket.WriteInt(cs.MyGuilds.GuildName.Length * 2);
                oPacket.WriteUnicodeString(cs.MyGuilds.GuildName);
                oPacket.WriteInt(cs.MyCharacter.MyChar.Length);
                for( int i = 0; i < cs.MyCharacter.MyChar.Length; i++)
                {
                    oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].CharType);
                    oPacket.WriteInt(0);
                    oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].Promotion);
                    oPacket.WriteInt(0);
                    oPacket.WriteByte(0);
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].Exp);
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].Level);
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].Win);
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].Loss);

                    // 장비
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip.Length);
                    for(int j = 0; j < cs.MyCharacter.MyChar[i].Equip.Length; j++)
                    {
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip[j].ItemID);
                        oPacket.WriteInt(1);
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip[j].ItemUID);
                        oPacket.WriteInt(0);
                        oPacket.WriteInt(0);
                        oPacket.WriteInt(0);
                        oPacket.WriteInt(0);
                        oPacket.WriteByte(0);
                        oPacket.WriteByte(0);
                        oPacket.WriteByte(0);
                    }

                    //oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 01 00 00 00 00 00 00 00 00 02 00 00 00 A0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07");
                    // 이 패킷이 아래 끝까지 분리됐음. ▼▼
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 01 00");

                    // 장착한 스킬
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].skillinfo.Length);
                    for (int j = 0; j < cs.MyCharacter.MyChar[i].skillinfo.Length; j++)
                    {
                        oPacket.WriteInt(0);
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].skillinfo[j].SkillGroup);
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].skillinfo[j].SkilliD);
                    }
                    //oPacket.WriteInt(0);

                    // FF가 스킬포인트 일지도.                    
                    oPacket.WriteInt(255);//255 Total
                    oPacket.WriteInt(160);//160 My SP//160 Max
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 FF 00 00 00 00 00 00");

                    // 배운 스킬
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].skillinfo.Length);
                    for (int j = 0; j < cs.MyCharacter.MyChar[i].skillinfo.Length; j++)
                    {
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].skillinfo[j].SkilliD);
                    }
                    //oPacket.WriteInt(0);

                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07");
                    // ▲▲
                }

                oPacket.WriteHexString("00 00 00 03 01 58 A8 C0 01 8E A8 C0");
                oPacket.WriteIPFromString(cs.GetIP(), false);
                oPacket.WriteHexString("00 00 00 01 7E FE 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 E5 6A 00 00 00 01 31 7F 24 36 00 00 00 00 01 00 00 E5 88 00 00 00 01 31 7F 24 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 01 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

                oPacket.WriteInt(cs.CurrentRoom.ID);
                oPacket.WriteInt(cs.CurrentRoom.RoomName.Length * 2);
                oPacket.WriteUnicodeString(cs.CurrentRoom.RoomName);
                oPacket.WriteShort(0);
                oPacket.WriteInt(cs.CurrentRoom.RoomPass.Length * 2);
                oPacket.WriteUnicodeString(cs.CurrentRoom.RoomPass);

                oPacket.WriteShort((short)room.GetPlayerCount());
                oPacket.WriteShort((short)(room.GetPlayerCount() + room.GetFreeSlot()));

                oPacket.WriteShort(1);
                oPacket.WriteByte((byte)room.GameCategory);
                oPacket.WriteInt(room.GameMode);                
                oPacket.WriteInt(room.ItemMode);
                oPacket.WriteBool(room.RandomMap);
                oPacket.WriteInt(room.GameMap);

                oPacket.WriteInt(12);
                for (int i = 0; i < 6; i++)
                    oPacket.WriteBool(room.Slot[i].Open);
                oPacket.WriteHexString("FF FF FF FF 00 00 00 00 00 00 00 00 01");

                oPacket.WriteIPFromString(Server.UDPRelayIP, true);
                oPacket.WriteShort(Server.UDPRelayPort);
                oPacket.WriteIPFromString(Server.TCPRelayIP, true);
                oPacket.WriteShort(Server.TCPRelayPort);

                oPacket.WriteHexString("01 00 01 00 00 01 2C 00 00 00 14 00 00 0B BF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 06 01 00 00 00 01");//00 00 00 00

                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void SendAccTime(ClientSession cs)
        {
            int ano = DateTime.Now.Year;
            int hora = DateTime.Now.Hour;
            int mes = DateTime.Now.Month;
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_ACC_TIME_NOT))
            {
                oPacket.WriteShort((short)hora);//00 00 /Dia
                oPacket.WriteShort((short)mes);  //00 02 Mes
                oPacket.WriteShort((short)ano);//07 E0 /Ano
                oPacket.WriteHexString("0A 03 01");

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void ListUsers(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_USER_LIST_ACK))
            {
                oPacket.WriteInt((int)cs.CurrentChannel.CurrentUsers);
                foreach (ClientSession u in cs.CurrentChannel.UsersList)
                {
                    oPacket.WriteInt(cs.LoginUID);
                    oPacket.WriteInt(cs.Nick.Length*2);
                    oPacket.WriteUnicodeString(cs.Nick);
                    oPacket.WriteByte(11);
                    oPacket.WriteByte(0);
                }
                //oPacket.WriteHexString("00 00 00 01 00 04 71 80 00 00 00 18 46 00 31 00 32 00 43 00 79 00 62 00 65 00 72 00 6C 00 69 00 6F 00 6E 00 0B 00");
                
                /*
                  foreach (ClientSession u in UsersList)
                    {                        
                          op.Assemble(u.CRYPT_KEY, u.CRYPT_HMAC, u.CRYPT_PREFIX, u.CRYPT_COUNT);
                          u.Send(op);
                          op.CancelAssemble();
                    }
                */

                int packetSize = oPacket.ToArray().Length;
                oPacket.CompressBuffer();
                byte[] Conteudo = oPacket.getBuffer();

                oPacket.InitBuffer();
                oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 01");
                oPacket.WriteInt(4 + Conteudo.Length);
                oPacket.WriteByte(1);
                oPacket.WriteBytes(BitConverter.GetBytes(packetSize));
                oPacket.WriteBytes(Conteudo);

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
                LogFactory.GetLog("LIST INVITE").LogHex("",oPacket.getBuffer());
            }
        }

        public void OnCharSelectJoin(ClientSession cs, InPacket ip)
        {

            cs.MyCommon.SendNoInvenItemList(cs);
            cs.MyCommon.SendStrengthMaterialInfo(cs);
            cs.MyCommon.SendWeeklyrewardList(cs);
            cs.MyCommon.SendMonthlyrewardList(cs);
            cs.MyCommon.SendMatchRankReward(cs);
            cs.MyCommon.SendHeroDungeonInfo(cs);
            cs.MyCommon.SendUserWeaponChange(cs);
            cs.MyCommon.SendNewCharCardInfo(cs);
            cs.MyCommon.SendVirtualCashLimitRatio(cs);
            cs.MyCommon.SendBadUserInfo(cs);
            cs.MyCommon.SendCollectionMission(cs);
            cs.MyCommon.SendHellTicketFreeMode(cs);
            cs.MyCommon.SendVIPItemList(cs);
            cs.MyCommon.SendCapsuleList(cs);
            cs.MyCommon.SendMissionPackList(cs);
            cs.MyCommon.SendVirtulCash(cs);
        }


        public void LoadVirtualCash(ClientSession cs,int LoginUID)
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT Cash FROM `currentcashvirtual` WHERE LoginUID = '{0}'",LoginUID);
            if (ds.Tables[0].Rows.Count == 0)
            {
                cs.VirtualCash = 0;
            }
            else
            {
                cs.VirtualCash = Convert.ToInt32(ds.Tables[0].Rows[0]["Cash"].ToString());
            }
        }

        public void OnRegisterNick(ClientSession cs, InPacket ip)
        {
            int GetNickLength = ip.ReadInt();
            string GetNick = ip.ReadUnicodeString(GetNickLength);

            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT * FROM `account` WHERE Nick = '{0}'", GetNick);

            if (ds.Tables[0].Rows.Count == 0)
            {
                // DB 갱신
                Database.ExecuteScript("UPDATE `account` SET Nick = '{0}' WHERE LoginUID = '{1}'", GetNick, cs.LoginUID);

                // 세션 갱신
                cs.Nick = GetNick;

                SendAccTime(cs);

                // 성공 패킷
                using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_REGISTER_NICKNAME_ACK))
                {
                    oPacket.WriteInt(0); // 0 = 성공인가?
                    oPacket.WriteInt(GetNickLength);
                    oPacket.WriteUnicodeString(GetNick);

                    oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                    cs.Send(oPacket);
                }
            }
            else
            {
                // 실패 패킷
                // 값 가져와야함.
                using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_REGISTER_NICKNAME_ACK))
                {
                    oPacket.WriteInt(-1); // 1 = 오류. 어쨌든 안 만들어짐
                    oPacket.WriteInt(GetNickLength);
                    oPacket.WriteUnicodeString(GetNick);

                    oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                    cs.Send(oPacket);
                }
            }
        }

        public void obtendoIPdosPlayers(ClientSession cs,string Account,string Pass)
        {
            DataSet obterIP = new DataSet();
            Database.Query(ref obterIP, "UPDATE `gc`.`account` SET  `Ip` = '{2}' WHERE Login = '{0}' AND Passwd = '{1}'", Account, Pass, cs.GetIP());
        }
        public static void StatusOnline(ClientSession cs, string Account, int Status, int Opc)
        {
            DataSet ds = new DataSet();
            if (Opc == 1)
            {
                Database.Query(ref ds, "UPDATE `gc`.`account` SET  `Online` = '{1}' WHERE Login = '{0}'", Account, Status);
                LogFactory.GetLog("StatusOnline").LogSuccess("Status do Player {0} alterado para {1}", Account, Status);
            }
            else if (Opc == 2)
            {
                Database.Query(ref ds, "UPDATE `gc`.`account` SET  `Online` = '{0}'", Status);
                LogFactory.GetLog("StatusOnline").LogSuccess("Todas as contas estão com Status {0} alterado", Status);

            }

        }
        public void OnLogin(ClientSession cs, InPacket ip)
        {
            int IDLength = ip.ReadInt();
            string ID = ip.ReadString(IDLength);
            int PWLength = ip.ReadInt();
            string PW = ip.ReadString(PWLength);

            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT * FROM `account` WHERE Login = '{0}' AND Passwd = '{1}'", ID, PW);

            obtendoIPdosPlayers(cs,ID,PW);

            if (ds.Tables[0].Rows.Count == 0)
            {
                // 로그인 실패
                LogFactory.GetLog("MAIN").LogInfo("{0} falha ao tentar logar.", ID);

                using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_VERIFY_ACCOUNT_ACK))
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
                // 로그인 성공
                LogFactory.GetLog("MAIN").LogInfo("{0} Usuario Logado.", ID);

                // 기본 데이터 가져오기
                cs.Login = ID;
                cs.LoginUID = Convert.ToInt32(ds.Tables[0].Rows[0]["LoginUID"].ToString());
                cs.Nick = ds.Tables[0].Rows[0]["Nick"].ToString();
                cs.GamePoint = Convert.ToInt32(ds.Tables[0].Rows[0]["Gamepoint"].ToString());
                cs.isBan = Convert.ToInt32(ds.Tables[0].Rows[0]["isBan"].ToString());
                cs.AuthLevel = Convert.ToInt32(ds.Tables[0].Rows[0]["AuthLevel"].ToString());
                cs.BonusLife = Convert.ToInt32(ds.Tables[0].Rows[0]["BonusLifes"].ToString());
                cs.Online = Convert.ToInt32(ds.Tables[0].Rows[0]["Online"].ToString());
                cs.MyInventory.InventorySize = Convert.ToInt32(ds.Tables[0].Rows[0]["sizeInventory"].ToString());
                cs.UserAuthLevel = Convert.ToInt32(ds.Tables[0].Rows[0]["UserAuthLevel"].ToString());

                if (cs.Online == 1)
                {
                    LogFactory.GetLog("Main").LogWarning("Tentativa de Logar na mesma conta {0}", ID);
                    using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_USER_AUTH_CHECK_ACK))
                    {
                        oPacket.WriteInt(1);
                        oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                        cs.Send(oPacket);
                    }
                    cs.Close();
                    return;

                }
                if (cs.isBan == 1)
                {
                    using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_VERIFY_ACCOUNT_ACK))
                    {
                        oPacket.WriteInt(ID.Length*2);
                        oPacket.WriteUnicodeString(ID);
                        oPacket.WriteHexString("00 00 00 00 05 00 11 3E 0F 28 04 1B 40 40 04 1B 77 01 31 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FD 00 00 00 64 01 7C 00 00 00 00 D1 C0 00 03 53 29 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 29 00 00 00 07 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 13 00 00 00 01 07 00 00 01 00 00 00 00 00 00 00 14 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 15 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 16 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 17 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 18 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 19 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 1A 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 1B 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 1E 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 24 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 27 00 00 00 01 03 00 00 00 00 00 00 00 00 00 00 28 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 29 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2A 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 2B 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2C 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2D 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2E 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2F 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 30 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 31 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 32 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 33 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 34 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 35 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 36 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 38 00 00 00 01 06 00 00 00 00 02 00 00 00 00 00 3E 00 00 00 01 01 00 00 01 00 00 00 00 0F 3C 08 8D 00 00 00 00 EC 46 08 8D 40 64 02 52 A2 00 7E E0 00 00 00 80 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 56 AD 8F E4 56 D3 E8 74 00 00 00 00 00 00 00 00 00 00 00 00 01 11 40 7E EE 00 00 00 00 40 64 02 52 A2 3C 7E E0 01 01 00 00 00 01 61 D0 B2 C0 FF 08 FF FF FF BC 02 50 EF C4 08 8D 11 00 00 00 00 00 7E EE A2 00 00 00 00 C0 00 00 00 00 00 00 00 00 00 00 00 04 7E F4 BA 01 00 00 00 00 00 00 00 00 00 00 00 00 11 34 08 8D FD FD 00 59 44 DD 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 29 00 7C 90 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01");
                        oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                        cs.Send(oPacket);
                        LogFactory.GetLog("HACKER").LogInfo("TENTATIVA DE LOGIN DE CONTA BANIDA: "+ID);
                    }                    
                    cs.Close();
                    return;
                }
                else
                {
                    // 로딩
                    cs.MyCharacter.LoadCharacter(cs);
                    cs.MyInventory.LoadInventory(cs);
                    cs.MyPet.LoadPet(cs);
                    cs.MyGuilds.CurrentGuild(cs);

                    // 기타 데이터 먼저 전송
                    cs.MyCommon.SendExpTable(cs);
                    cs.MyInventory.SendInventory(cs);
                    missions.LoadQuests(cs);

                    //LogFactory.GetLog("MAIN").LogInfo("로그인 성공 메시지를 전송합니다.", ID);
                    StatusOnline(cs, ID, 1, 1);
                    using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_VERIFY_ACCOUNT_ACK))
                    {
                        // 아이디
                        oPacket.WriteInt(cs.Login.Length * 2);
                        oPacket.WriteUnicodeString(cs.Login);

                        // 닉네임
                        oPacket.WriteInt(cs.Nick.Length * 2);
                        if (cs.Nick.Length > 0)
                            oPacket.WriteUnicodeString(cs.Nick);

                        oPacket.WriteByte(0);

                        // GP
                        oPacket.WriteInt(cs.GamePoint);
                        oPacket.WriteHexString("E0 04 A9 40 10 04 A9 60 01");
                        oPacket.WriteIPFromString(cs.GetIP(), false);
                        oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                        oPacket.WriteByte((byte)cs.UserAuthLevel);//00
                        oPacket.WriteInt(100);//00 00 00 64
                        oPacket.WriteBool(false);
                        oPacket.WriteByte(cs.PCBangType);//00

                        // 캐릭터
                        oPacket.WriteInt(cs.MyCharacter.MyChar.Length);
                        for (int i = 0; i < cs.MyCharacter.MyChar.Length; i++)
                        {
                            oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].CharType);
                            oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].CharType);
                            oPacket.WriteHexString("00 00 00 00");
                            oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].Promotion);
                            oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].Promotion);
                            oPacket.WriteHexString("00 00 00 00");
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].Exp);
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].Win);
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].Loss);
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].Win);
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].Loss);
                            oPacket.WriteHexString("00 00 00 00");
                            //oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].Exp);
                            oPacket.WriteHexString("00 00 00");
                            oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].Level);

                            // 장비
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip.Length);
                            for (int j = 0; j < cs.MyCharacter.MyChar[i].Equip.Length; j++)
                            {
                                oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip[j].ItemID);
                                oPacket.WriteInt(1);
                                oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip[j].ItemUID);
                                oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                            }

                            // 펫
                            if (cs.MyCharacter.MyChar[i].Pet > 0)
                                oPacket.WriteInt(1);
                            else
                                oPacket.WriteInt(0);
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].Pet);

                            oPacket.WriteInt(255);//00 00 00 FF / SP LEFT
                            oPacket.WriteInt(160);//00 00 00 A0
                            oPacket.WriteInt(1);//00 00 00 01
                            oPacket.WriteInt(0);//00 00 00 00
                            oPacket.WriteByte(0);//00
                            oPacket.WriteLong(100);//00 00 00 00 00 00 00 64
                            oPacket.WriteLong(100);                            

                            // 무기체인지
                            if (cs.MyCharacter.MyChar[i].WeaponChange == true && cs.MyCharacter.MyChar[i].WeaponChangeID > 0)
                            {
                                // 아이템고유번호로 아이템번호 가져오기
                                int WeaponChangeItemID = cs.MyInventory.FindItemIDbyUID(cs.MyCharacter.MyChar[i].WeaponChangeID);
                                oPacket.WriteInt(WeaponChangeItemID);
                                oPacket.WriteInt(1);
                                oPacket.WriteInt(cs.MyCharacter.MyChar[i].WeaponChangeID);
                                oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                            }
                            else
                            {
                                oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                            }

                            oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07");
                        }

                        oPacket.WriteHexString("24 B9"); // 서버 포트인거같음
                        //oPacket.WriteHexString("24 C4");
                        oPacket.WriteInt(cs.LoginUID);

                        // 서버 이름
                        string ServerName = Settings.GetString("GameServer/ServerName");

                        oPacket.WriteInt(ServerName.Length * 2);
                        oPacket.WriteUnicodeString(ServerName);

                        // 사용하지 않으면 지울것.
                        string TempMsg = Settings.GetString("GameServer/WelcomeMsg");
                        if (TempMsg.Length > 0)
                        {
                            oPacket.WriteInt(1);
                            oPacket.WriteInt(1);
                            oPacket.WriteInt(TempMsg.Length * 2);
                            oPacket.WriteUnicodeString(TempMsg);
                        }
                        else
                        {
                            oPacket.WriteInt(0);
                            oPacket.WriteInt(0);
                        }

                        // 열린 게임모드인거같음.
                        //oPacket.WriteHexString("00 00 00 4E 00 00 00 07 00 00 00 00 00 00 00 00 00 00 00 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 09 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 10 00 00 00 00 00 00 00 00 00 00 00 00 00 00 11 00 00 00 00 00 00 00 00 00 00 00 00 00 00 12 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 00 00 00 00 00 00 00 00 00 00 00 00 00 00 14 00 00 00 00 00 00 00 00 00 00 00 00 00 00 15 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 00 00 00 00 00 00 00 00 00 00 00 00 00 00 17 00 00 00 00 00 00 00 00 00 00 00 00 00 00 18 00 00 00 00 00 00 00 00 00 00 00 00 00 00 19 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 24 00 00 00 00 00 00 00 00 00 00 00 00 00 00 27 00 00 00 00 00 00 00 00 00 00 00 00 00 00 28 00 00 00 00 00 00 00 00 00 00 00 00 00 00 29 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 30 00 00 00 00 00 00 00 00 00 00 00 00 00 00 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 33 00 00 00 00 00 00 00 00 00 00 00 00 00 00 34 00 00 00 00 00 00 00 00 00 00 00 00 00 00 35 00 00 00 00 00 00 00 00 00 00 00 00 00 00 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 38 00 00 00 00 00 00 00 00 00 00 00 00 00 00 39 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 00 00 00 00 00 00 00 00 00 00 00 00 00 00 46 00 00 00 00 00 00 00 00 00 00 00 00 00 00 47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 00 00 00 00 00 00 00 00 00 00 00 00 00 00 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 53 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 00 00 00 00 00 00 00 00 00 00 00 00 00 00 55 00 00 00 00 00 00 00 00 00 00 00 00 00 00 56 00 00 00 00 00 00 00 00 00 00 00 00 00 00 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 00 00 00 00 00 00 00 00 00 59 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 0F 60 02 71 00 00 00 00 00 80 00 18 00 5A 00 00 00 5A 00 00 00 00 00 80 00 00 00 00 00 00 00 00");
                        //oPacket.WriteHexString("00 00 00 4E 00 00 00 07 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 13 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 14 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 15 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 16 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 17 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 18 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 19 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 24 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 27 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 28 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 29 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 2B 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2C 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2D 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2E 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 30 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 31 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 32 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 33 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 34 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 35 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 36 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 38 00 00 00 00 00 00 00 00 00 00 00 00 00 00 39 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 00 00 00 00 00 00 00 00 00 00 00 00 00 00 46 00 00 00 00 00 00 00 00 00 00 00 00 00 00 47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 00 00 00 00 00 00 00 00 00 00 00 00 00 00 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 53 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 00 00 00 00 00 00 00 00 00 00 00 00 00 00 55 00 00 00 00 00 00 00 00 00 00 00 00 00 00 56 00 00 00 00 00 00 00 00 00 00 00 00 00 00 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 00 00 00 00 00 00 00 00 00 59 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 12 F3 76 D3 00 00 00 0D 00 07 0D BE 00 00 00 01 00 98 96 81 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 0D C8 00 00 00 01 00 98 96 82 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 0D D2 00 00 00 01 00 98 96 83 00 00 00 00 56 7B 21 D0 56 79 D0 50 00 00 00 00 00 07 0D DC 00 00 00 01 00 98 96 84 00 00 00 00 56 7B 21 D0 56 79 D0 50 00 00 00 00 00 07 19 08 00 00 00 01 00 98 96 81 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 19 12 00 00 00 01 00 98 96 82 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 22 18 00 00 00 01 00 98 97 69 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 22 2C 00 00 00 01 00 98 97 6B 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 22 90 00 00 00 01 00 98 97 75 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 24 52 00 00 00 01 00 98 96 81 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 24 5C 00 00 00 01 00 98 96 82 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 24 66 00 00 00 01 00 98 96 83 00 00 00 00 56 7B 91 24 56 7A 3F A4 00 00 00 00 00 07 24 70 00 00 00 01 00 98 96 84 00 00 00 00 56 7B 91 24 56 7A 3F A4 00 00 00 00 00 00 01 18 00 00 00 00 00 5A 00 00 00 00 00 80 00 00 00 00 00 00 00 00");

                        oPacket.WriteHexString("00 00 00 4E 00 00 00 07 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 13 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 14 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 15 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 16 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 17 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 18 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 19 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1D 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 24 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 27 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 28 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 29 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 2B 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2C 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2D 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2E 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 30 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 31 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 32 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 33 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 34 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 35 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 36 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 37 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 38 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 39 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3C 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3D 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 3F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 40 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 43 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 44 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 45 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 46 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 47 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 48 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 49 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4C 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 4F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 50 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 51 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 52 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 53 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 54 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 55 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 56 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 57 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 58 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 59 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5C 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5D 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 12 F3 76 D3");
                        //00 00 00 0D 00 07 0D BE 00 00 00 01 00 98 96 81 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 0D C8 00 00 00 01 00 98 96 82 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 0D D2 00 00 00 01 00 98 96 83 00 00 00 00 56 7B 21 D0 56 79 D0 50 00 00 00 00 00 07 0D DC 00 00 00 01 00 98 96 84 00 00 00 00 56 7B 21 D0 56 79 D0 50 00 00 00 00 00 07 19 08 00 00 00 01 00 98 96 81 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 19 12 00 00 00 01 00 98 96 82 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 22 18 00 00 00 01 00 98 97 69 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 22 2C 00 00 00 01 00 98 97 6B 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 22 90 00 00 00 01 00 98 97 75 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 24 52 00 00 00 01 00 98 96 81 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 24 5C 00 00 00 01 00 98 96 82 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 24 66 00 00 00 01 00 98 96 83 00 00 00 00 56 7B 91 24 56 7A 3F A4 00 00 00 00 00 07 24 70 00 00 00 01 00 98 96 84 00 00 00 00 56 7B 91 24 56 7A 3F A4 00 00 00 00 00 00 01 18 00 00 00 00 00 5A 00 00 00 00 00 80 00 00 00 00 00 00 00 00");
                        oPacket.WriteInt(missions.missions.Length);
                        for (int a = 0; a < missions.missions.Length; a++)
                        {
                            oPacket.WriteInt(missions.missions[a].MissionID);//00 07 0D BE
                            oPacket.WriteInt(missions.missions[a].Progress);//00 00 00 01
                            oPacket.WriteInt(missions.missions[a].MissionUID);
                            oPacket.WriteHexString("00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00");
                        }
                        oPacket.WriteHexString("00 00 01 18 00 00 00 00 00 5A 00 00 00 00 00 80 00 00 00 00 00 00 00 00");

                        // 메신저서버
                        string MsgServerName = "MsgServer_05";
                        string MsgServerIP = Server.MsgServerIP;
                        short MsgServerPort = Server.MsgServerPort;
                        oPacket.WriteInt(MsgServerName.Length * 2);
                        oPacket.WriteUnicodeString(MsgServerName);
                        oPacket.WriteInt(MsgServerIP.Length);
                        oPacket.WriteString(MsgServerIP);
                        oPacket.WriteShort(MsgServerPort);

                        // 뭔지 모르겠는데 현재 접속자 수로 예상.
                        oPacket.WriteHexString("00 00 01 24");
                        oPacket.WriteHexString("00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF");

                        // 정체 불명의 아이피를 또 뿌림
                        oPacket.WriteInt(MsgServerIP.Length);
                        oPacket.WriteString(MsgServerIP);

                        oPacket.WriteHexString("00 00 00 00 00 00 00 00 03 57 F1 73 AC 57 F1 73 AC 00 00 00 00");

                        // 펫 정보 전달
                        oPacket.WriteInt(cs.MyPet.pet.Length);
                        for (int i = 0; i < cs.MyPet.pet.Length; i++)
                        {
                            oPacket.WriteInt(1);
                            oPacket.WriteInt(cs.MyPet.pet[i].PetUID);
                            oPacket.WriteInt(1);
                            oPacket.WriteInt(cs.MyPet.pet[i].PetUID);
                            oPacket.WriteInt(cs.MyPet.pet[i].PetItemID);
                            oPacket.WriteInt(cs.MyPet.pet[i].Name.Length * 2);
                            oPacket.WriteUnicodeString(cs.MyPet.pet[i].Name);
                            oPacket.WriteInt(3);
                            oPacket.WriteByte(0);
                            oPacket.WriteInt(cs.MyPet.pet[i].Exp);
                            oPacket.WriteHexString("01 00 00 00 64 02 00 00 00 64");
                            oPacket.WriteInt(cs.MyPet.pet[i].Exp);
                            oPacket.WriteInt(cs.MyPet.pet[i].Level);
                            oPacket.WriteBool(cs.MyPet.pet[i].Evo);

                            if (cs.MyPet.GetPetTransformInfo(cs.MyPet.pet[i].PetItemID) == cs.MyPet.pet[i].PetItemID)
                                oPacket.WriteHexString("FF FF FF FF");
                            else
                                oPacket.WriteInt(cs.MyPet.GetPetTransformInfo(cs.MyPet.pet[i].PetItemID));

                            oPacket.WriteInt(cs.MyPet.pet[i].Health);
                            oPacket.WriteInt(cs.MyPet.pet[i].Health);

                            // 쓸때없이 이걸 두번함 - 1

                            if (cs.MyPet.pet[i].Slot1 > 0 && cs.MyPet.pet[i].Slot2 > 0)
                                oPacket.WriteInt(2);
                            else if (cs.MyPet.pet[i].Slot1 == 0 && cs.MyPet.pet[i].Slot2 == 0)
                                oPacket.WriteInt(0);
                            else
                                oPacket.WriteInt(1);

                            if (cs.MyPet.pet[i].Slot2 > 0)
                            {
                                int ItempID = cs.MyInventory.FindItemIDbyUID(cs.MyPet.pet[i].Slot2);
                                oPacket.WriteInt(ItempID);
                                oPacket.WriteInt(1);
                                oPacket.WriteInt(cs.MyPet.pet[i].Slot2);
                                oPacket.WriteByte(0);
                            }

                            if (cs.MyPet.pet[i].Slot1 > 0)
                            {
                                int ItempID = cs.MyInventory.FindItemIDbyUID(cs.MyPet.pet[i].Slot1);
                                oPacket.WriteInt(ItempID);
                                oPacket.WriteInt(1);
                                oPacket.WriteInt(cs.MyPet.pet[i].Slot1);
                                oPacket.WriteByte(0);
                            }

                            // 쓸때없이 이걸 두번함 - 2

                            if (cs.MyPet.pet[i].Slot1 > 0 && cs.MyPet.pet[i].Slot2 > 0)
                                oPacket.WriteInt(2);
                            else if (cs.MyPet.pet[i].Slot1 == 0 && cs.MyPet.pet[i].Slot2 == 0)
                                oPacket.WriteInt(0);
                            else
                                oPacket.WriteInt(1);

                            if (cs.MyPet.pet[i].Slot2 > 0)
                            {
                                int ItempID = cs.MyInventory.FindItemIDbyUID(cs.MyPet.pet[i].Slot2);
                                oPacket.WriteInt(ItempID);
                                oPacket.WriteInt(1);
                                oPacket.WriteInt(cs.MyPet.pet[i].Slot2);
                                oPacket.WriteByte(0);
                            }

                            if (cs.MyPet.pet[i].Slot1 > 0)
                            {
                                int ItempID = cs.MyInventory.FindItemIDbyUID(cs.MyPet.pet[i].Slot1);
                                oPacket.WriteInt(ItempID);
                                oPacket.WriteInt(1);
                                oPacket.WriteInt(cs.MyPet.pet[i].Slot1);
                                oPacket.WriteByte(0);
                            }

                            oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00");
                            oPacket.WriteShort((short)cs.MyPet.pet[i].Bind);
                        }

                        oPacket.WriteHexString("00 00 00 00 01");
                        oPacket.WriteInt(cs.MyInventory.InventorySize);
                        oPacket.WriteHexString("00 00 00 00");
                        oPacket.WriteInt(cs.BonusLife);//00 00 00 09
                        oPacket.WriteHexString("00 00 00 00 00 01 00 00 00 01 61 D0 B2 C0 00 64 7E EE E2 C0 07 E7 10 6B 7C 92 A0 00 00 00 00 A4 72 93 E0 57 EF 5E F0 00 00 00 00");
                        oPacket.WriteInt(20);//Total de Chars
                        for (int j = 0; j < 20; j++ )
                        {
                            oPacket.WriteInt(j);
                            oPacket.WriteInt(j);
                            oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00");
                        }
                        oPacket.WriteInt(2);//00 00 00 02
                        oPacket.WriteInt(30);//00 00 00 1E
                        oPacket.WriteInt(779510);//00 0B E4 F6
                        oPacket.WriteInt(31);//00 00 00 1F
                        oPacket.WriteInt(1404170);//00 15 6D 0A
                        oPacket.WriteInt(400);//00 00 01 90
                        oPacket.WriteByte(0);//01 Check tutorial

                        oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                        cs.Send(oPacket);
                    }

                    // 서버 시간 전송
                    cs.MyCommon.SendServerTime(cs);

                    // ???
                    //cs.MyCommon.SendDungeonTicketList(cs);
                    cs.MyCommon.SendPetVestedItem(cs);
                    cs.MyCommon.SendGraduateCharInfo(cs);
                    cs.MyCommon.SendJumpingCharInfo(cs);
                    cs.MyCommon.SendSlotInfo(cs);
                    cs.MyCommon.SendGuideCompleteInfo(cs);
                    cs.MyCommon.SendFullLookInfo(cs);
                    cs.MyCommon.SendFairyTreeBuff(cs);
                    cs.MyCommon.SendAgitInitSeed(cs);
                    LoadVirtualCash(cs, cs.LoginUID);
                }
            }
        }


        public void OnEnterChannel(ClientSession cs, InPacket ip)
        {
            // ----- 1 = 대전, 6 = 던전
            int Channel = ip.ReadInt();
            cs.ServerType = Channel;

            // 혹시 모르니 이미 접속중인 채널 빠져나가도록.
            OnLeaveChannel(cs);

            // ----- 채널 입장 하드코딩
            string chName = Channel == 1 ? "대전" : "던전";
            Channel ch = TSingleton<ChannelManager>.Instance.GetChannel(chName);

            //LogFactory.GetLog("MAIN").LogInfo("플레이어 {0}가 {1}채널로 입장했습니다.", cs.Login, chName);

            if (ch != null)
                if (!ch.UsersMap.ContainsKey(cs.Login))
                {
                    lock (ch._lock)
                    {
                        ch.UsersList.Add(cs);
                        ch.UsersMap.Add(cs.Login, cs);

                        cs.CurrentChannel = ch;
                        //br2
                    }
                }
            // -----

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_ENTER_CHANNEL_ACK))
            {
                oPacket.WriteHexString("00 00 00 00 00 59 23 DD F0 59 25 2F 6F");

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
              //  LogFactory.GetLog("MAIN").LogInfo("PLAYERS NO LOBBY: "+ch.CurrentUsers);
            }
        }
        
        public void OnLeaveChannel(ClientSession cs)
        {
            // 채널 떠나기
            if (cs.CurrentChannel == null)
                return;

            Channel ch;
            ch = TSingleton<ChannelManager>.Instance.GetChannel("대전");
            lock (cs.CurrentChannel._lock)
            {
                if (ch.UsersList.Contains(cs))
                    ch.UsersList.Remove(cs);

                if (ch.UsersMap.ContainsKey(cs.Login))
                    ch.UsersMap.Remove(cs.Login);
            }

            ch = TSingleton<ChannelManager>.Instance.GetChannel("던전");
            lock (cs.CurrentChannel._lock)
            {
                if (ch.UsersList.Contains(cs))
                    ch.UsersList.Remove(cs);

                if (ch.UsersMap.ContainsKey(cs.Login))
                    ch.UsersMap.Remove(cs.Login);
            }

            cs.CurrentChannel = null;

            /*
            if (cs.CurrentChannel != null)
            {
                lock (cs.CurrentChannel._lock)
                {
                    cs.CurrentChannel.UsersList.Remove(cs);
                    cs.CurrentChannel.UsersMap.Remove(cs.Login);

                    cs.CurrentChannel = null;
                }
            }
            */
        }
    }
}
