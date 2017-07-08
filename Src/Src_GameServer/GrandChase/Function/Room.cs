using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GrandChase.IO.Packet;
using GrandChase.Net.Client;
using GrandChase.Security;
using GrandChase.IO;
using GrandChase.Data;
using GrandChase.Utilities;
using Manager.Factories;
using Manager;

namespace GrandChase.Function
{
    public class Room
    {
        public const int MAX_USERS = 6;
        public enum eGameMode
        {
            GC_GM_TUTORIAL = 0,
            GM_TEAM = 1,
            GM_SURVIVAL = 2,
            GC_GM_TAG_TEAM = 3,
            GC_GM_TAG_SURVIVAL = 4,
            GC_GM_GUILD_BATTLE = 5,
            GC_GM_INDIGO_TEAM = 6
        }
        public enum eItemMode
        {
            GM_ITEM = 0,
            GM_NOITEM = 1
        }
        public enum eGameCategory
        {
            GMC_MATCH = 0,
            GMC_GUILD_BATTLE = 1,
            GMC_DUNGEON = 2,
            GMC_INDIGO = 3,
            GMC_TUTORIAL = 4,
            GMC_TAG_MATCH = 5,
            GMC_MONSTER_CRUSADER = 6,
            GMC_MONSTER_HUNT = 7,
            GMC_DEATHMATCH = 8,
            GMC_MINIGAME = 9,
            GMC_ANGELS_EGG = 10,
            GMC_CAPTAIN = 11,
            GMC_DOTA = 12,
            GC_GMC_AGIT = 13,
            GC_GMC_AUTOMATCH = 14,
            GC_GMC_FATAL_DEATHMATCH = 15,
            GC_GMC_MONSTERMATCH = 16
        }
        public enum eGameStage
        {
            GC_GS_FOREST_OF_ELF = 0,
            GC_GS_SWAMP_OF_OBLIVION = 1,
            GC_GS_FLYING_SHIP = 2,
            GC_GS_VALLEY_OF_OATH = 3,
            GC_GS_FOGOTTEN_CITY = 4,
            GC_GS_BABEL_OF_X_MAS = 5,
            GC_GS_TEMPLE_OF_FIRE = 6,
            GC_GM_QUEST0 = 7, GC_GM_QUEST1 = 8, GC_GM_QUEST2 = 9, GC_GM_QUEST3 = 10, GC_GM_QUEST4 = 11, GC_GM_QUEST5 = 12, GC_GM_QUEST6 = 13, GC_GM_QUEST7 = 14, GC_GM_QUEST8 = 15, GC_GM_QUEST9 = 16, GC_GM_QUEST10 = 17, GC_GM_QUEST11 = 18, GC_GM_QUEST12 = 19, GC_GM_QUEST13 = 20, GC_GM_QUEST14 = 21, GC_GM_QUEST15 = 22, GC_GM_QUEST16 = 23, GC_GM_QUEST17 = 24, GC_GM_QUEST18 = 25, GC_GM_QUEST19 = 26, GC_GM_QUEST20 = 27, GC_GM_MONSTER_CRUSADER = 28, GC_GM_MONSTER_HUNT = 29, GC_GM_QUEST21 = 30, GC_GM_DEATH_TEAM = 31, GC_GM_DEATH_SURVIVAL = 32, GC_GM_MINIGAME_TREEDROP = 33, GC_GM_MINIGAME_BALLOON = 34, GC_GM_MINIGAME_BUTTERFRY = 35, GC_GM_QUEST22 = 36, GC_GM_ANGELS_EGG = 37, GC_GM_CAPTAIN = 38, GC_GM_QUEST23 = 39, GC_GM_QUEST24 = 40, GC_GM_QUEST25 = 41, GC_GM_QUEST26 = 42, GC_GM_QUEST27 = 43, GC_GM_QUEST28 = 44, GC_GM_QUEST29 = 45, GC_GM_QUEST30 = 46, GC_GM_QUEST31 = 47, GC_GM_QUEST32 = 48, GC_GM_QUEST33 = 49, GC_GM_QUEST34 = 50, GC_GM_QUEST35 = 51, GC_GM_QUEST36 = 52, GC_GM_QUEST37 = 53, GC_GM_QUEST38 = 54, GC_GM_QUEST39 = 55, GC_GM_QUEST40 = 56, GC_GM_QUEST41 = 57, GC_GM_QUEST42 = 58, GC_GM_QUEST43 = 59, GC_GM_QUEST44 = 60, GC_GM_QUEST45 = 61, GC_GM_QUEST46 = 62, GC_GM_QUEST47 = 63, GC_GM_QUEST48 = 64, GC_GM_DOTA = 65, GC_GM_AGIT = 66, GC_GM_QUEST49 = 67, GC_GM_QUEST50 = 68, GC_GM_QUEST51 = 69, GC_GM_QUEST52 = 70, GC_GM_QUEST53 = 71, GC_GM_QUEST54 = 72, GC_GM_QUEST55 = 73, GC_GM_QUEST56 = 74, GC_GM_QUEST57 = 75, GC_GM_QUEST58 = 76, GC_GM_AUTOMATCH_TEAM = 77, GC_GM_QUEST59 = 78, GC_GM_QUEST60 = 79, GC_GM_QUEST61 = 80, GC_GM_QUEST62 = 81, GC_GM_QUEST63 = 82, GC_GM_QUEST64 = 83, GC_GM_QUEST65 = 84, GC_GM_QUEST66 = 85, GC_GM_QUEST67 = 86, GC_GM_QUEST68 = 87, GC_GM_QUEST69 = 88, GC_GM_QUEST70 = 89, GC_GM_QUEST71 = 90, GC_GM_QUEST72 = 91, GC_GM_QUEST73 = 92, GC_GM_QUEST74 = 93, GC_GM_QUEST75 = 94, GC_GM_QUEST76 = 95, GC_GM_FATAL_DEATH_TEAM = 96, GC_GM_FATAL_DEATH_SURVIVAL = 97, GC_GM_QUEST77 = 98, GC_GM_QUEST78 = 99, GC_GM_QUEST79 = 100, GC_GM_QUEST80 = 101, GC_GM_QUEST81 = 102, GC_GM_QUEST82 = 103, GC_GM_MONSTER_TEAM = 104, GC_GM_MONSTER_SURVIVAL = 105, GC_GM_QUEST83 = 106
        }

        public struct sSlot
        {
            public bool Active; 
            public bool Open; 
            public ClientSession cs;
            public int LoadStatus;
            public byte State; 
            public bool AFK;
            public int Spree;
            public bool Leader;
        }

        public ushort ID;
        public string RoomName;
        public string RoomPass;
        public int GameCategory;
        public int GameMode;
        public int ItemMode;
        public int GameMap;
        public bool RandomMap;
        public sSlot[] Slot = new sSlot[6];
        public int Kick;
        public bool Playing;

        public int FindSlotIndex(ClientSession cs)
        {
            for (int i = 0; i < 6; i++)
                if (Slot[i].cs == cs && Slot[i].Active == true)
                    return i;
            return -1;
        }

        public int GetPlayerCount()
        {
            int temp = 0;
            for (int i = 0; i < 6; i++)
                if (Slot[i].Active == true)
                    temp++;
            return temp;
        }

        public int GetFreeSlot()
        {
            int temp = 0;
            for (int i = 0; i < 6; i++)
                if (Slot[i].Open == true)
                    temp++;
            return temp;
        }

        public int GetTeamByCS(ClientSession cs)
        {
            for( int i = 0; i < 6; i++)
            {
                if (Slot[i].Active == true && Slot[i].cs == cs)
                    return i / 3; // 0~2 = 0, 3~5 = 1
            }
            return 0;
        }

        public int GetSlotPosByCS(ClientSession cs)
        {
            for (int i = 0; i < 6; i++)
            {
                if (Slot[i].Active == true && Slot[i].cs == cs)
                    return i;
            }
            return 0;
        }

        public ClientSession GetRoomLeaderCS()
        {
            for (int i = 0; i < 6; i++)
            {
                if (Slot[i].Leader == true)
                    return Slot[i].cs;
            }
            return null;
        }

        public void SendJoinRoomInfoDivide(ClientSession cs)
        {
            int TempCount = -1;
            for (int i = 0; i < 6; i++)
            {
               
                if (Slot[i].Active == false)
                    continue;

                TempCount++;

                using (OutPacket op = new OutPacket(GameOpcodes.EVENT_JOIN_ROOM_INFO_DIVIDE_ACK))
                {
                    op.WriteInt(0);
                    op.WriteInt(GetPlayerCount());
                    op.WriteInt(TempCount);
                    op.WriteInt(Slot[i].cs.Login.Length * 2);
                    op.WriteUnicodeString(Slot[i].cs.Login);
                    op.WriteInt(Slot[i].cs.LoginUID);
                    op.WriteInt(Slot[i].cs.Nick.Length * 2);
                    op.WriteUnicodeString(Slot[i].cs.Nick);
                    op.WriteInt(i);
                    op.WriteByte((byte)Slot[i].cs.CurrentChar);
                    op.WriteHexString("00 FF 00 FF 00 FF 00 00 00 00");
                    op.WriteByte((byte)GetTeamByCS(Slot[i].cs));
                    op.WriteHexString("01 00 00 00 0D 00 00 00 00 10 F4 00 00 00 00 00 4E 00 00 00 07 00 00 00 00 00 00 00 00 00 00 00 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 09 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 10 00 00 00 00 00 00 00 00 00 00 00 00 00 00 11 00 00 00 00 00 00 00 00 00 00 00 00 00 00 12 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 00 00 00 00 00 00 00 00 00 00 00 00 00 00 14 00 00 00 00 00 00 00 00 00 00 00 00 00 00 15 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 00 00 00 00 00 00 00 00 00 00 00 00 00 00 17 00 00 00 00 00 00 00 00 00 00 00 00 00 00 18 00 00 00 00 00 00 00 00 00 00 00 00 00 00 19 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 24 00 00 00 00 00 00 00 00 00 00 00 00 00 00 27 00 00 00 00 00 00 00 00 00 00 00 00 00 00 28 00 00 00 00 00 00 00 00 00 00 00 00 00 00 29 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 30 00 00 00 00 00 00 00 00 00 00 00 00 00 00 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 33 00 00 00 00 00 00 00 00 00 00 00 00 00 00 34 00 00 00 00 00 00 00 00 00 00 00 00 00 00 35 00 00 00 00 00 00 00 00 00 00 00 00 00 00 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 38 00 00 00 00 00 00 00 00 00 00 00 00 00 00 39 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 00 00 00 00 00 00 00 00 00 00 00 00 00 00 46 00 00 00 00 00 00 00 00 00 00 00 00 00 00 47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 00 00 00 00 00 00 00 00 00 00 00 00 00 00 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 53 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 00 00 00 00 00 00 00 00 00 00 00 00 00 00 55 00 00 00 00 00 00 00 00 00 00 00 00 00 00 56 00 00 00 00 00 00 00 00 00 00 00 00 00 00 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 00 00 00 00 00 00 00 00 00 59 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00");
                    if (GetRoomLeaderCS() == Slot[i].cs)
                        op.WriteByte(1);
                    else
                        op.WriteByte(0);
                    op.WriteHexString("01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

                    op.WriteByte((byte)Slot[i].cs.MyCharacter.MyChar.Length);
                    //LogFactory.GetLog("MAIN").LogInfo("캐릭터수: {0}", Slot[i].cs.MyCharacter.MyChar.Length);
                    for (int j = 0; j < Slot[i].cs.MyCharacter.MyChar.Length; j++)
                    {
                        //LogFactory.GetLog("MAIN").LogInfo("    슬롯{0} 캐릭터idx{1}", i, j);

                        op.WriteByte((byte)Slot[i].cs.MyCharacter.MyChar[j].CharType);
                        op.WriteInt(0);
                        op.WriteByte((byte)Slot[i].cs.MyCharacter.MyChar[j].Promotion);
                        op.WriteInt(0);
                        op.WriteByte(0);
                        op.WriteInt(Slot[i].cs.MyCharacter.MyChar[j].Exp);
                        op.WriteByte(0);
                        op.WriteByte(0);
                        op.WriteByte(0);
                        op.WriteByte((byte)Slot[i].cs.MyCharacter.MyChar[j].Level);
                        op.WriteInt(0);
                        op.WriteInt(0);

                        op.WriteInt(Slot[i].cs.MyCharacter.MyChar[j].Equip.Length);
                        for (int k = 0; k < Slot[i].cs.MyCharacter.MyChar[j].Equip.Length; k++)
                        {
                            op.WriteInt(Slot[i].cs.MyCharacter.MyChar[j].Equip[k].ItemID);
                            op.WriteInt(1);
                            op.WriteInt(Slot[i].cs.MyCharacter.MyChar[j].Equip[k].ItemUID);
                            op.WriteInt(0);
                            op.WriteInt(0);
                            op.WriteInt(0);
                            op.WriteInt(0);
                            op.WriteByte(0);
                            op.WriteByte(0);
                            op.WriteByte(0);
                        }

                        //oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 01 00 00 00 00 00 00 00 00 02 00 00 00 A0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 01 FF 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07");
                        // 이 패킷이 아래 끝까지 분리됐음. ▼▼
                        op.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 01 00");

                        // 장착한 스킬
                        /*op.WriteInt(Slot[i].cs.MyCharacter.MyChar[j].EquipSkill.Length);
                        for (int k = 0; j < Slot[i].cs.MyCharacter.MyChar[j].EquipSkill.Length; j++)
                        {
                            op.WriteInt(0);
                            op.WriteInt(Slot[i].cs.MyCharacter.MyChar[j].EquipSkill[k].SkillGroup);
                            op.WriteInt(Slot[i].cs.MyCharacter.MyChar[j].EquipSkill[k].SkillID);
                        }*/
                        op.WriteInt(0);

                        op.WriteHexString("00 00 00 FF 00 00 00 A0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 01 FF 00 00 00 00 01 00");


                        /*oPacket.WriteInt(cs.MyCharacter.MyChar[i].MySkill.Length);
                        for (int j = 0; j < cs.MyCharacter.MyChar[i].MySkill.Length; j++)
                        {
                            oPacket.WriteInt(cs.MyCharacter.MyChar[i].MySkill[j].SkillID);
                        }*/
                        op.WriteInt(0);

                        op.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07");
                        // ▲▲
                    }

                    op.WriteHexString("00 00 00 04 13 00 A8 C0 01 EC A8 C0 9B BA FE A9");
                    op.WriteIPFromString(Slot[i].cs.GetIP(), false);
                    op.WriteHexString("00 00 00 01 7E F5 00 00 00");
                    op.WriteByte(Slot[i].State);
                    op.WriteHexString("00 00 00 00 00 00 00 02 00 00 00 00 00 00 E5 6A 00 00 00 01 2C BD 52 5A 00 00 00 00 01 00 00 E5 88 00 00 00 01 2C BD 52 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 56 86 32 00 56 87 6E D4 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

                    op.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                    cs.Send(op);
                }
            }
        }

        public void OnChangeRoomInfo(ClientSession cs, InPacket ip)
        {
            bool isLeader = false;
            for(int i = 0; i < 6; i++)
            {
                if (Slot[i].Leader == true && Slot[i].cs == cs)
                    isLeader = true;
            }

            
            if (isLeader == false)
                return;

            int Flag = ip.ReadInt(); 
            byte[] Unknown1 = ip.ReadBytes(3);
            int NewGameCategory = ip.ReadByte();
            int NewGameMode = ip.ReadInt();
            int NewItemMode = ip.ReadInt();
            bool NewRandomMap = ip.ReadBool();
            int NewMap = ip.ReadInt();
            int Unknown2 = ip.ReadInt();
            int Unknown3 = ip.ReadInt(); 
            int Unknown4 = ip.ReadInt();
            int Unknown5 = ip.ReadInt();
            int Unknown6 = ip.ReadInt();
            int Unknown7 = ip.ReadInt();
            int Unknown8 = ip.ReadInt();

            if (Flag == 0)
            {
                GameCategory = NewGameCategory;
                GameMode = NewGameMode;
                ItemMode = NewItemMode;
                RandomMap = NewRandomMap;
                GameMap = NewMap;
            }


            int ChangeSlotNum = (int)ip.ReadByte();
            byte[] TempSlotInfo = null;
            if (ChangeSlotNum > 0) 
            {

                TempSlotInfo = BytesUtil.ReadBytes(ip.ToArray(), ip.Position, ChangeSlotNum * 2);

                for (byte i = 0; i < ChangeSlotNum; i++)
                {
                    byte TargetSlot = ip.ReadByte();
                    bool TargetSlotState = ip.ReadBool();

                    Slot[TargetSlot].Open = TargetSlotState;

                }
            }

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_CHANGE_ROOM_INFO_BROAD))
            {
                oPacket.WriteHexString("00 00 00 00 00 00 00");
                oPacket.WriteByte((byte)GameCategory);
                oPacket.WriteInt(GameMode);
                oPacket.WriteInt(ItemMode);
                oPacket.WriteBool(RandomMap);
                oPacket.WriteInt(GameMap);
                oPacket.WriteInt(0);
                oPacket.WriteHexString("FF FF FF FF 00 00 00 00 00 00 00");
                oPacket.WriteShort((short)GetPlayerCount());
                oPacket.WriteShort((short)GetFreeSlot());

                for (int i = 0; i < 6; i++)
                    oPacket.WriteBool(Slot[i].Open);

                oPacket.WriteInt(ChangeSlotNum);

                if (ChangeSlotNum > 0)
                    oPacket.WriteBytes(TempSlotInfo);

                if(ChangeSlotNum == 1)
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00");
                else if( ChangeSlotNum == 2)
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00");
                else
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01");
                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true) {
                        oPacket.Assemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }

        public void OnChangeUserInfo(ClientSession cs, InPacket ip)
        {
            
            int unknown1 = ip.ReadInt();
            int LoginUID = ip.ReadInt();
            byte flag = ip.ReadByte();
            int LoginLen = ip.ReadInt();
            string Login = ip.ReadUnicodeString(LoginLen);
            int Team = ip.ReadInt();
            int unknown3 = ip.ReadInt();
            byte Character = ip.ReadByte();
            byte unknown4 = ip.ReadByte(); // FF 00 FF 00
            ip.ReadByte();
            ip.ReadByte();
            ip.ReadByte();
            int unknown5 = ip.ReadInt(); // FF 00 00 00
            byte unknown6 = ip.ReadByte(); // 00
            byte state = ip.ReadByte(); 

            if (Login != cs.Login)
            {
                return;
            }
                

            /*// 없는 캐릭터를 요청했으므로 무시.
            if (cs.MyCharacter.isHaveChar(Character) == false)
            {
                return;
            }
             */
            // 캐릭터 변경한다.
            if (unknown4 == 255)
            {
            cs.CurrentChar = Character;
            }
           
            if (GetTeamByCS(cs) != Team)
            {
                int newpos = -1;
                if (Team == 0)
                {
                    for (int i = 0; i <= 2; i++) {
                        if (Slot[i].Active == false && Slot[i].Open == true)
                        {
                            newpos = i;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 3; i <= 5; i++)
                    {
                        if (Slot[i].Active == false && Slot[i].Open == true)
                        {
                            newpos = i;
                            break;
                        }
                    }
                }

                if (newpos == -1)
                    return;

                int oldpos = GetSlotPosByCS(cs);

                Slot[newpos].Active = Slot[oldpos].Active;
                Slot[newpos].Open = Slot[oldpos].Open;
                Slot[newpos].cs = Slot[oldpos].cs;
                Slot[newpos].Leader = Slot[oldpos].Leader;
                Slot[newpos].LoadStatus = Slot[oldpos].LoadStatus;
                Slot[newpos].State = Slot[oldpos].State;

                Slot[oldpos].Active = false;
                Slot[oldpos].Open = true;
                Slot[oldpos].cs = null;
                Slot[oldpos].Leader = false;
                Slot[oldpos].LoadStatus = 0;
                Slot[oldpos].State = 0;
            }

            int mypos = GetSlotPosByCS(cs);

            if (flag == 4)
                Slot[mypos].State = state;

            
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_CHANGE_ROOMUSER_INFO_BROAD))
            {
                oPacket.WriteInt(0);
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteByte(4); 
                oPacket.WriteInt(cs.Login.Length * 2);
                oPacket.WriteUnicodeString(cs.Login);
                oPacket.WriteInt(mypos / 3); 
                oPacket.WriteByte(0);
                oPacket.WriteByte(0);
                oPacket.WriteByte(0);
                oPacket.WriteByte((byte)mypos);
                oPacket.WriteByte((byte)cs.CurrentChar);
                oPacket.WriteHexString("FF 00 FF 00 FF 00 00 00 00");
                oPacket.WriteByte(Slot[mypos].State);
                oPacket.WriteByte(0);

                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.Assemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket);
                        oPacket.CancelAssemble();
                    }
                }
            }
        }

        public void ProcessLeaveRoom(ClientSession cs)
        {
            int mypos = GetSlotPosByCS(cs);

            bool isLeader = Slot[mypos].Leader;

            Slot[mypos].Active = false;
            Slot[mypos].Open = true;
            Slot[mypos].cs = null;
            Slot[mypos].Leader = false;
            cs.CurrentRoom = null;

            if (GetPlayerCount() > 0)
            {
                using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_INFORM_USER_LEAVE_ROOM_NOT))
                {
                    oPacket.WriteInt(cs.Login.Length * 2);
                    oPacket.WriteUnicodeString(cs.Login);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(cs.LoginUID);
                    oPacket.WriteInt(3);

                    for (int i = 0; i < 6; i++)
                    {
                        if (Slot[i].Active == true)
                        {
                            oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                            Slot[i].cs.Send(oPacket); 
                            oPacket.CancelAssemble();
                        }
                    }
                }


                if(isLeader == true)
                {
                    int newleaderpos = mypos;
                    for (int i = 0; i < 6; i++)
                    {
                        newleaderpos++;
                        if (newleaderpos == 6) newleaderpos -= 6; 
                        if (Slot[newleaderpos].Active == true)
                        {
                            break;
                        }
                    }

                    Slot[newleaderpos].Leader = true;
                    Slot[newleaderpos].State = 0;

                    using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_HOST_MIGRATED_NOT))
                    {
                        oPacket.WriteInt(Slot[newleaderpos].cs.LoginUID);
                        oPacket.WriteByte(1);

                        for (int i = 0; i < 6; i++)
                        {
                            if (Slot[i].Active == true)
                            {
                                oPacket.Assemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                                Slot[i].cs.Send(oPacket); 
                                oPacket.CancelAssemble(); 
                            }
                        }
                    }
                }
            }

            if (GetPlayerCount() == 0)
            {
                lock (cs.CurrentChannel._lock)
                {
                    cs.CurrentChannel.RoomsList.Remove(this);
                    cs.CurrentChannel.RoomsMap.Remove(this.ID);
                }
            }
        }

        public void OnLeaveRoom(ClientSession cs, InPacket ip)
        {
            int Unknown1 = ip.ReadInt(); // 19

            ProcessLeaveRoom(cs);

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_LEAVE_ROOM_ACK))
            {
                oPacket.WriteInt(0);

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void OnLeaveGame(ClientSession cs, InPacket ip)
        {
            int Unknown1 = ip.ReadInt(); // 19

            ProcessLeaveRoom(cs);

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_LEAVE_GAME_ACK))
            {
                oPacket.WriteInt(0);

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void OnGameStart(ClientSession cs, InPacket ip)
        {
            int mypos = GetSlotPosByCS(cs);

            if (Slot[mypos].Leader == false)
                return;

            int NeedReady = GetPlayerCount() - 1; 
            int NowReady = 0;
            for( int i = 0; i < 6; i++)
                if (Slot[i].State == 1 && Slot[i].Active == true)
                    NowReady++;


            if (NeedReady < NowReady)
                return;


            Playing = true;

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_START_GAME_BROAD))
            {
                oPacket.WriteInt(0);
                oPacket.WriteHexString("52 3A E9 A2");
                oPacket.Skip(20); // 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
                oPacket.WriteInt(GetPlayerCount());
                for (int i = 0; i < 6; i++)
                    if (Slot[i].Active == true)
                        oPacket.WriteInt(Slot[i].cs.LoginUID);
                oPacket.WriteHexString("00 02 46 FE");
                oPacket.WriteInt(GetPlayerCount());
                for (int i = 0; i < 6; i++)
                    if (Slot[i].Active == true)
                    {
                        oPacket.WriteInt(Slot[i].cs.LoginUID);
                        oPacket.WriteHexString("00 00 01 04 00 00 00 6A");
                    }
                oPacket.WriteInt(0);

                oPacket.WriteInt(GetRoomLeaderCS().LoginUID);
                oPacket.Skip(7); // 00 00 00 00 00 00 00

                oPacket.WriteByte((byte)GameCategory);
                oPacket.WriteInt(GameMode);
                oPacket.WriteInt(ItemMode);
                oPacket.WriteBool(RandomMap);
                oPacket.WriteInt(GameMap);
                oPacket.WriteHexString("00 00 00 00 FF FF FF FF 00 00 00 01 00 00 00");
                oPacket.WriteShort((short)GetPlayerCount());
                oPacket.WriteShort((short)GetFreeSlot());
                for (int i = 0; i < 6; i++)
                    if (Slot[i].Active == true)
                        oPacket.WriteBool(Slot[i].Open);
                oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                oPacket.WriteInt(GetPlayerCount());
                for (int i = 0; i < 6; i++)
                    if(Slot[i].Active == true)
                    {
                        oPacket.WriteInt(Slot[i].cs.LoginUID);
                        oPacket.WriteByte((byte)Slot[i].cs.CurrentChar);
                        oPacket.WriteHexString("00 00 03 E8");
                    }
                oPacket.Skip(17); // 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00

                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble();
                    }
                }
            }
        }
        public void OnLoadState(ClientSession cs, InPacket ip)
        {
            int ID = ip.ReadInt();
            int LoadPercent = ip.ReadInt();

            for (int i = 0; i < 6; i++)
                if (Slot[i].Active == true)
                    if (Slot[i].cs == cs)
                    {
                        Slot[i].LoadStatus = LoadPercent;
                        LogFactory.GetLog("MAIN").LogInfo("{0}의 로딩 상태 변경: {1}", i, LoadPercent);
                    }

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_RELAY_LOADING_STATE))
            {
                oPacket.WriteInt(ID);
                oPacket.WriteInt(LoadPercent);

                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.Assemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }

        public void OnLoadComplete(ClientSession cs, InPacket ip)
        {
            for (int i = 0; i < 6; i++)
                if (Slot[i].Active == true)
                    if (Slot[i].LoadStatus < 17)
                    {
                        LogFactory.GetLog("MAIN").LogInfo("OnLoadComplete - {0}은 아직 로딩되지 않았음. (현재 {1})", i, Slot[i].LoadStatus);
                        if (GetPlayerCount() >= 2)
                        {
                            return;
                        }
                    }


            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_LOAD_COMPLETE_BROAD))
            {
                oPacket.WriteInt(0);

                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }

        public void OnStageLoadComplete(ClientSession cs, InPacket ip)
        {
            for (int i = 0; i < 6; i++)
                if (Slot[i].Active == true)
                    if (Slot[i].LoadStatus < 17)
                    {
                        if (GetPlayerCount() >= 2)
                        {
                            return;
                        }
                        //return;
                    }


           
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_STAGE_LOAD_COMPLETE_BROAD))
            {
                oPacket.WriteInt(0);

                
                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket);
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }

        public void checkRoomUser(ClientSession cs)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_CHECK_ROOM_USERLIST_NOT))
            {
                oPacket.WriteInt(GetPlayerCount());
                for (int i = 0; i < 6; i++)
                 if (Slot[i].Active == true)
                 {
                      oPacket.WriteInt(Slot[i].cs.LoginUID);
                      oPacket.WriteInt(GetPlayerCount());
                      oPacket.WriteInt(Slot[i].cs.LoginUID);                      
                 }

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void statLoadingTime(ClientSession cs, InPacket ip)
        {
            int unk = ip.ReadInt();
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_STAT_LOADING_TIME_NOT))
            {
                oPacket.WriteInt(unk);
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }

        public void OnPingInfo(ClientSession cs, InPacket ip)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_ROOM_MEMBER_PING_INFO_ACK))
            {
                oPacket.WriteInt(GetPlayerCount());
                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.WriteInt(Slot[i].cs.LoginUID);
                        oPacket.WriteInt(0);
                    }

                }
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
                oPacket.CancelAssemble(); 
            }
        }

        public void OnIdleInfo(ClientSession cs, InPacket ip)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_GET_ROOMUSER_IDLE_STATE_ACK))
            {
                oPacket.WriteInt(GetPlayerCount());
                for (int i = 0; i < 6; i++)
                    if (Slot[i].Active == true)
                    {
                        oPacket.WriteInt(Slot[i].cs.LoginUID);

                       
                        oPacket.WriteByte(0);
                        oPacket.WriteByte(0);
                        oPacket.WriteByte(0);
                        oPacket.WriteBool(Slot[i].AFK);
                    }

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }
        public void RewardItem(ClientSession cs,InPacket ip)
        {
            ushort SelectSlot = 0;
            for (int i = 0; i < GetPlayerCount(); i++)
            {
                ip.ReadInt();
                ip.ReadByte();
                ip.ReadInt();
                ip.ReadInt();
                SelectSlot = ip.ReadUShort();
            }
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_SPECIAL_REWARD_BROAD))
            {
                oPacket.WriteInt(GetPlayerCount()); // 00 00 00 02
                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == false)
                        continue;
                    oPacket.WriteInt(Slot[i].cs.LoginUID);
                    oPacket.WriteByte((byte)Slot[i].cs.CurrentChar);
                    oPacket.WriteInt(1); //00 00 00 01
                    oPacket.WriteUShort(SelectSlot);//slots de iems
                    oPacket.WriteInt(0);//00 00 00 00
                    oPacket.WriteInt(2154);//00 00 08 6A
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(4);
                    Random rnd = new Random();
                    Random rnd2 = new Random();
                    int resu = rnd2.Next(1);
                    for (ushort a = 0; a < 4; a++)
                    {
                        oPacket.WriteUShort(a);
                        int item = rnd2.Next(SpecialReward.items.Length);
                        oPacket.WriteInt(SpecialReward.items[item]);
                        if (a == SelectSlot)
                        {
                            DataSet ds2 = new DataSet();
                            Database.Query(ref ds2, "INSERT INTO `gc`.`inventory` (  `LoginUID`,  `ItemID`,  `Quantity`) VALUES  (    '{0}',    '{1}',    '1'  )", cs.LoginUID, item);
                        }
                        if (resu == 0)
                        {
                            oPacket.WriteInt(1);
                        }
                        else
                        {
                            oPacket.WriteInt(-1);
                        }
                        oPacket.WriteHexString("FF FF FF FF 00 FF 00 00 00 00 00 00");
                    }
                    //INSERT INTO `gc`.`inventory` (  `LoginUID`,  `ItemID`,  `Quantity`) VALUES  (    '{0}',    '{1}',    '1'  ) 
                    //oPacket.WriteHexString("00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 FF 00 00 00 00 00 00 00 05 00 00 00 00 FF FF FF FF FF FF FF FF 00 FF 00 00 00 00 00 EF 00 02 00 05 D3 C2 FF FF FF FF FF FF FF FF 00 FF 00 00 00 00 00 00 00 03 00 00 E5 92 00 00 00 01 FF FF FF FF 00 FF 00 00 00 00 00 00");
                }
                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket);
                        oPacket.CancelAssemble();
                    }
                }
            }
        }

        public void OnGameEnd(ClientSession cs, InPacket ip)
        {
         
            Playing = false;
            ip.ReadInt();
            int lenlogin = ip.ReadInt();
            string login = ip.ReadUnicodeString(lenlogin);
            ip.ReadInt();
            ip.Skip(79);
            int unk = ip.ReadInt();
            for (int j = 0; j < unk; j++ )
            {
                int array = ip.ReadInt();
            }
            int unk2 = ip.ReadInt();
            for (int j2 = 0; j2 < unk2; j2++)
            {
                int array2 = ip.ReadInt();
            }
            ip.Skip(43);
            int unk3 = ip.ReadInt();
            for (int j3 = 0; j3 < unk3; j3++)
            {
                ip.ReadInt();//00 00 00 07
                ip.ReadInt();//00 00 00 00
                ip.ReadInt();//00 00 00 00
                ip.ReadInt();//00 00 00 46
                ip.ReadInt();//00 00 00 00
                ip.ReadInt();//00 00 00 00
                ip.ReadInt();//00 00 00 00
                ip.ReadInt();//00 00 00 00
            }
            ip.Skip(586);
            LogFactory.GetLog("FINISH GAME").LogHex("DATA: ", ip.ToArray());
            int CharIndex = ip.ReadInt();
            LogFactory.GetLog("FINISH GAME").LogInfo("CHAR: " + CharIndex);
            int Level = ip.ReadInt();
            int Promotion = ip.ReadInt();
            LogFactory.GetLog("FINISH GAME").LogInfo("LEVEL: " + Level);            

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_END_GAME_NOT))
            {                
                oPacket.WriteInt(570317);
                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket);
                        oPacket.CancelAssemble(); 
                    }
                }
            }
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_PREMIUM_NOT))
            {
                oPacket.WriteInt(GetPlayerCount()); // 00 00 00 02
                for (int i = 0; i < GetPlayerCount(); i++)
                {
                    if (Slot[i].Active == false)
                        continue;
                    oPacket.WriteInt(Slot[i].cs.LoginUID);
                    oPacket.WriteInt(0);
                }
                //oPacket.WriteHexString("00 00 00 01 00 02 D5 2D 00 00 00 00");
                for (int i = 0; i < GetPlayerCount(); i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket);
                        oPacket.CancelAssemble(); 
                    }
                }
            }
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_SERVER_TIME_NOT))
            {
                oPacket.WriteHexString("59 18 F4 EA 00 00 07 E1 00 00 00 05 00 00 00 0E 00 00 00 11 00 00 00 17 00 00 00 06");
                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_END_GAME_BROAD))
            {
                DataSet ds = new DataSet();
                Database.Query(ref ds, "UPDATE   `character` SET  `Level` = '{2}' WHERE `LoginUID` = '{0}' AND  `CharType` = '{1}'", cs.LoginUID, CharIndex, Level);
                
                 
                oPacket.WriteInt(cs.LoginUID);                
                oPacket.WriteInt(GetPlayerCount()); // 00 00 00 02                
                for (int i = 0; i < GetPlayerCount(); i++)
                {
                    if (Slot[i].Active == false)
                        continue;

                    oPacket.WriteInt(Slot[i].cs.Login.Length * 2);
                    oPacket.WriteUnicodeString(Slot[i].cs.Login);
                    oPacket.WriteInt(Slot[i].cs.LoginUID);
                    oPacket.WriteHexString("00 00 9F C0 00 00 01 38 00 00 00 2E 00 00 00 04 00 00 00 00 3D 4C CC CD 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 04 3D CC CC CD 00 00 00 00 00 00 00 00 00 00 00 01");
                    oPacket.WriteByte((byte)Slot[i].cs.CurrentChar);
                    oPacket.WriteHexString("00 00 00 00 00 00 00 01");
                    oPacket.WriteByte((byte)Slot[i].cs.CurrentChar);
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                    //Stages
                    oPacket.WriteHexString("00 00 00 4E 00 00 00 07 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 13 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 14 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 15 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 16 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 17 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 18 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 19 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 24 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 27 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 28 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 29 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 2B 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2C 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2D 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2E 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 30 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 31 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 32 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 33 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 34 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 35 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 36 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 38 00 00 00 00 00 00 00 00 00 00 00 00 00 00 39 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 00 00 00 00 00 00 00 00 00 00 00 00 00 00 46 00 00 00 00 00 00 00 00 00 00 00 00 00 00 47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 00 00 00 00 00 00 00 00 00 00 00 00 00 00 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 53 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 55 00 00 00 00 00 00 00 00 00 00 00 00 00 00 56 00 00 00 00 00 00 00 00 00 00 00 00 00 00 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 00 00 00 00 00 00 00 00 00 59 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 00");
                    oPacket.WriteHexString("00 00 00 00 00 00 00 01");
                    oPacket.WriteByte((byte)Slot[i].cs.CurrentChar);
                    oPacket.WriteByte((byte)Slot[i].cs.CurrentChar);//0F 0F
                    int MyCharPos = -1;
                    for (int t = 0; t < Slot[i].cs.MyCharacter.MyChar.Length; t++)
                        if (Slot[i].cs.MyCharacter.MyChar[t].CharType == Slot[i].cs.CurrentChar)
                            MyCharPos = t;
                    Slot[i].cs.MyCharacter.MyChar[MyCharPos].Level = Level;
                    oPacket.WriteLong(Slot[i].cs.MyCharacter.MyChar[MyCharPos].Exp);//00 00 00 00 00 00 00 C7
                    oPacket.WriteLong(Slot[i].cs.MyCharacter.MyChar[MyCharPos].Exp);//00 00 00 00 00 00 00 D5
                    oPacket.WriteHexString("00 00 00 00 00 00 00 0A 00 00 00 6E 00 00 00 0A 00 00 00 6E 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 04 3D CC CC CD 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 04 3D CC CC CD 00 00 00 08 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 8A 00 00 00 00 00 00 00 00 00 00 00 00 0C 00 07 0D BE 00 00 00 01 00 98 96 81 00 00 00 00 59 24 CE 28 59 23 7C A8 00 00 00 00 00 07 0D C8 00 00 00 01 00 98 96 82 00 00 00 00 59 24 CE 28 59 23 7C A8 00 00 00 00 00 07 19 08 00 00 00 01 00 98 96 81 00 00 00 00 59 24 CE 28 59 23 7C A8 00 00 00 00 00 07 19 12 00 00 00 01 00 98 96 82 00 00 00 00 59 24 CE 28 59 23 7C A8 00 00 00 00 00 07 24 52 00 00 00 01 00 98 96 81 00 00 00 00 59 24 CE 28 59 23 7C A8 00 00 00 00 00 07 24 5C 00 00 00 01 00 98 96 82 00 00 00 00 59 24 CE 28 59 23 7C A8 00 00 00 00 00 0A E8 58 00 00 00 01 00 98 96 81 00 00 00 00 59 24 CE A8 59 23 7D 28 00 00 00 00 00 0A E8 62 00 00 00 01 00 98 96 82 00 00 00 00 59 24 CE A8 59 23 7D 28 00 00 00 00 00 12 9D FA 00 00 00 01 00 98 98 0F 00 00 00 00 59 24 CE CB 59 23 7D 4B 00 00 00 00 00 07 46 30 00 00 00 01 00 98 96 81 00 00 00 00 59 24 D0 3F 59 23 7E BF 00 00 00 00 00 07 46 3A 00 00 00 01 00 98 96 82 00 00 00 00 59 24 D0 3F 59 23 7E BF 00 00 00 00 00 07 7E A2 00 00 00 01 00 98 96 BC 00 00 00 00 59 24 D1 74 59 23 7F F4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 84 00 00 78 6E 00 00 00 01 00 00 00 01 00 00 00 14 00 00 00 00 00 00 00 01 00 00 00 14 00 00 00 00 00 00 78 78 00 00 00 01 00 00 00 01 00 00 00 15 00 00 00 00 00 00 00 01 00 00 00 15 00 00 00 00 00 00 78 82 00 00 00 01 00 00 00 01 00 00 00 16 00 00 00 00 00 00 00 01 00 00 00 16 00 00 00 00 00 00 78 8C 00 00 00 01 00 00 00 01 00 00 00 17 00 00 00 00 00 00 00 01 00 00 00 17 00 00 00 00 00 00 78 96 00 00 00 01 00 00 00 01 00 00 00 18 00 00 00 00 00 00 00 01 00 00 00 18 00 00 00 00 00 00 78 A0 00 00 00 01 00 00 00 01 00 00 00 19 00 00 00 00 00 00 00 01 00 00 00 19 00 00 00 00 00 00 78 AA 00 00 00 01 00 00 00 01 00 00 00 1A 00 00 00 00 00 00 00 01 00 00 00 1A 00 00 00 00 00 00 78 B4 00 00 00 01 00 00 00 01 00 00 00 1B 00 00 00 00 00 00 00 01 00 00 00 1B 00 00 00 00 00 00 78 BE 00 00 00 01 00 00 00 01 00 00 00 1C 00 00 00 00 00 00 00 01 00 00 00 1C 00 00 00 00 00 00 78 C8 00 00 00 01 00 00 00 01 00 00 00 1D 00 00 00 00 00 00 00 01 00 00 00 1D 00 00 00 00 00 00 78 D2 00 00 00 01 00 00 00 01 00 00 00 1E 00 00 00 00 00 00 00 01 00 00 00 1E 00 00 00 00 00 00 78 DC 00 00 00 01 00 00 00 01 00 00 00 1F 00 00 00 00 00 00 00 01 00 00 00 1F 00 00 00 00 00 00 78 E6 00 00 00 01 00 00 00 01 00 00 00 20 00 00 00 00 00 00 00 01 00 00 00 20 00 00 00 00 00 00 78 F0 00 00 00 01 00 00 00 01 00 00 00 21 00 00 00 00 00 00 00 01 00 00 00 21 00 00 00 00 00 00 78 FA 00 00 00 01 00 00 00 01 00 00 00 22 00 00 00 00 00 00 00 01 00 00 00 22 00 00 00 00 00 00 79 04 00 00 00 01 00 00 00 01 00 00 00 23 00 00 00 00 00 00 00 01 00 00 00 23 00 00 00 00 00 00 79 0E 00 00 00 01 00 00 00 01 00 00 00 24 00 00 00 00 00 00 00 01 00 00 00 24 00 00 00 00 00 00 79 18 00 00 00 01 00 00 00 01 00 00 00 25 00 00 00 00 00 00 00 01 00 00 00 25 00 00 00 00 00 00 79 22 00 00 00 01 00 00 00 01 00 00 00 26 00 00 00 00 00 00 00 01 00 00 00 26 00 00 00 00 00 00 79 2C 00 00 00 01 00 00 00 01 00 00 00 28 00 00 00 00 00 00 00 01 00 00 00 28 00 00 00 00 00 00 79 36 00 00 00 01 00 00 00 01 00 00 00 2A 00 00 00 00 00 00 00 01 00 00 00 2A 00 00 00 00 00 00 79 40 00 00 00 01 00 00 00 01 00 00 00 2C 00 00 00 00 00 00 00 01 00 00 00 2C 00 00 00 00 00 00 79 4A 00 00 00 01 00 00 00 01 00 00 00 2E 00 00 00 00 00 00 00 01 00 00 00 2E 00 00 00 00 00 00 85 C0 00 00 00 01 00 00 00 01 00 00 00 30 00 00 00 00 00 00 00 01 00 00 00 30 00 00 00 00 00 00 85 CA 00 00 00 01 00 00 00 01 00 00 00 32 00 00 00 00 00 00 00 01 00 00 00 32 00 00 00 00 00 00 85 D4 00 00 00 01 00 00 00 01 00 00 00 42 00 00 00 00 00 00 00 01 00 00 00 42 00 00 00 00 00 00 85 DE 00 00 00 01 00 00 00 01 00 00 00 44 00 00 00 00 00 00 00 01 00 00 00 44 00 00 00 00 00 00 85 E8 00 00 00 01 00 00 00 01 00 00 00 46 00 00 00 00 00 00 00 01 00 00 00 46 00 00 00 00 00 00 85 F2 00 00 00 01 00 00 00 01 00 00 00 48 00 00 00 00 00 00 00 01 00 00 00 48 00 00 00 00 00 00 85 FC 00 00 00 01 00 00 00 01 00 00 00 4A 00 00 00 00 00 00 00 01 00 00 00 4A 00 00 00 00 00 00 86 06 00 00 00 01 00 00 00 01 00 00 00 4C 00 00 00 00 00 00 00 01 00 00 00 4C 00 00 00 00 00 00 86 10 00 00 00 01 00 00 00 01 00 00 00 50 00 00 00 00 00 00 00 01 00 00 00 50 00 00 00 00 00 00 86 1A 00 00 00 01 00 00 00 01 00 00 00 52 00 00 00 00 00 00 00 01 00 00 00 52 00 00 00 00 00 00 86 24 00 00 00 01 00 00 00 01 00 00 00 56 00 00 00 00 00 00 00 01 00 00 00 56 00 00 00 00 00 01 45 8C 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 01 45 96 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 01 45 A0 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 00 00 01 45 AA 00 00 00 01 00 00 00 01 00 00 00 03 00 00 00 00 00 00 00 01 00 00 00 03 00 00 00 00 00 01 45 B4 00 00 00 01 00 00 00 01 00 00 00 04 00 00 00 00 00 00 00 01 00 00 00 04 00 00 00 00 00 01 45 BE 00 00 00 01 00 00 00 01 00 00 00 05 00 00 00 00 00 00 00 01 00 00 00 05 00 00 00 00 00 01 45 C8 00 00 00 01 00 00 00 01 00 00 00 06 00 00 00 00 00 00 00 01 00 00 00 06 00 00 00 00 00 01 45 D2 00 00 00 01 00 00 00 01 00 00 00 07 00 00 00 00 00 00 00 01 00 00 00 07 00 00 00 00 00 01 45 DC 00 00 00 01 00 00 00 01 00 00 00 08 00 00 00 00 00 00 00 01 00 00 00 08 00 00 00 00 00 01 45 E6 00 00 00 01 00 00 00 01 00 00 00 09 00 00 00 00 00 00 00 01 00 00 00 09 00 00 00 00 00 01 45 F0 00 00 00 01 00 00 00 01 00 00 00 0A 00 00 00 00 00 00 00 01 00 00 00 0A 00 00 00 00 00 01 45 FA 00 00 00 01 00 00 00 01 00 00 00 0B 00 00 00 00 00 00 00 01 00 00 00 0B 00 00 00 00 00 01 46 04 00 00 00 01 00 00 00 01 00 00 00 0C 00 00 00 00 00 00 00 01 00 00 00 0C 00 00 00 00 00 01 46 0E 00 00 00 01 00 00 00 01 00 00 00 0D 00 00 00 00 00 00 00 01 00 00 00 0D 00 00 00 00 00 01 46 18 00 00 00 01 00 00 00 01 00 00 00 0E 00 00 00 00 00 00 00 01 00 00 00 0E 00 00 00 00 00 01 46 22 00 00 00 01 00 00 00 01 00 00 00 0F 00 00 00 00 00 00 00 01 00 00 00 0F 00 00 00 00 00 01 46 2C 00 00 00 01 00 00 00 01 00 00 00 10 00 00 00 00 00 00 00 01 00 00 00 10 00 00 00 00 00 01 46 36 00 00 00 01 00 00 00 01 00 00 00 11 00 00 00 00 00 00 00 01 00 00 00 11 00 00 00 00 00 01 46 40 00 00 00 01 00 00 00 01 00 00 00 12 00 00 00 00 00 00 00 01 00 00 00 12 00 00 00 00 00 01 46 4A 00 00 00 01 00 00 00 01 00 00 00 13 00 00 00 00 00 00 00 01 00 00 00 13 00 00 00 00 00 01 9B 18 00 00 00 01 00 00 00 01 00 00 00 27 00 00 00 00 00 00 00 01 00 00 00 27 00 00 00 00 00 01 FF 40 00 00 00 01 00 00 00 01 00 00 00 29 00 00 00 00 00 00 00 01 00 00 00 29 00 00 00 00 00 01 FF 4A 00 00 00 01 00 00 00 01 00 00 00 2B 00 00 00 00 00 00 00 01 00 00 00 2B 00 00 00 00 00 02 1E 9E 00 00 00 01 00 00 00 01 00 00 00 2D 00 00 00 00 00 00 00 01 00 00 00 2D 00 00 00 00 00 02 29 A2 00 00 00 01 00 00 00 01 00 00 00 2F 00 00 00 00 00 00 00 01 00 00 00 2F 00 00 00 00 00 02 41 08 00 00 00 01 00 00 00 01 00 00 00 31 00 00 00 00 00 00 00 01 00 00 00 31 00 00 00 00 00 02 CA 56 00 00 00 01 00 00 00 01 00 00 00 33 00 00 00 00 00 00 00 01 00 00 00 33 00 00 00 00 00 02 CA 60 00 00 00 01 00 00 00 01 00 00 00 34 00 00 00 00 00 00 00 01 00 00 00 34 00 00 00 00 00 02 CA 6A 00 00 00 01 00 00 00 01 00 00 00 35 00 00 00 00 00 00 00 01 00 00 00 35 00 00 00 00 00 02 CA 74 00 00 00 01 00 00 00 01 00 00 00 36 00 00 00 00 00 00 00 01 00 00 00 36 00 00 00 00 00 02 D1 2C 00 00 00 01 00 00 00 01 00 00 00 37 00 00 00 00 00 00 00 01 00 00 00 37 00 00 00 00 00 02 D1 36 00 00 00 01 00 00 00 01 00 00 00 38 00 00 00 00 00 00 00 01 00 00 00 38 00 00 00 00 00 02 D1 40 00 00 00 01 00 00 00 01 00 00 00 39 00 00 00 00 00 00 00 01 00 00 00 39 00 00 00 00 00 02 D1 4A 00 00 00 01 00 00 00 01 00 00 00 3A 00 00 00 00 00 00 00 01 00 00 00 3A 00 00 00 00 00 02 E0 54 00 00 00 01 00 00 00 01 00 00 00 3B 00 00 00 00 00 00 00 01 00 00 00 3B 00 00 00 00 00 02 E0 5E 00 00 00 01 00 00 00 01 00 00 00 3C 00 00 00 00 00 00 00 01 00 00 00 3C 00 00 00 00 00 02 E0 68 00 00 00 01 00 00 00 01 00 00 00 3D 00 00 00 00 00 00 00 01 00 00 00 3D 00 00 00 00 00 02 E0 72 00 00 00 01 00 00 00 01 00 00 00 3E 00 00 00 00 00 00 00 01 00 00 00 3E 00 00 00 00 00 02 E0 7C 00 00 00 01 00 00 00 01 00 00 00 3F 00 00 00 00 00 00 00 01 00 00 00 3F 00 00 00 00 00 02 E0 86 00 00 00 01 00 00 00 01 00 00 00 40 00 00 00 00 00 00 00 01 00 00 00 40 00 00 00 00 00 03 4A 76 00 00 00 01 00 00 00 01 00 00 00 41 00 00 00 00 00 00 00 01 00 00 00 41 00 00 00 00 00 03 4A 80 00 00 00 01 00 00 00 01 00 00 00 43 00 00 00 00 00 00 00 01 00 00 00 43 00 00 00 00 00 03 4A 8A 00 00 00 01 00 00 00 01 00 00 00 45 00 00 00 00 00 00 00 01 00 00 00 45 00 00 00 00 00 03 4A 94 00 00 00 01 00 00 00 01 00 00 00 47 00 00 00 00 00 00 00 01 00 00 00 47 00 00 00 00 00 04 89 86 00 00 00 01 00 00 00 01 00 00 00 49 00 00 00 00 00 00 00 01 00 00 00 49 00 00 00 00 00 04 89 90 00 00 00 01 00 00 00 01 00 00 00 4B 00 00 00 00 00 00 00 01 00 00 00 4B 00 00 00 00 00 05 0F 6E 00 00 00 01 00 00 00 01 00 00 00 4D 00 00 00 00 00 00 00 01 00 00 00 4D 00 00 00 00 00 05 0F 78 00 00 00 01 00 00 00 01 00 00 00 4E 00 00 00 00 00 00 00 01 00 00 00 4E 00 00 00 00 00 05 9A 42 00 00 00 01 00 00 00 01 00 00 00 4F 00 00 00 00 00 00 00 01 00 00 00 4F 00 00 00 00 00 06 E2 3A 00 00 00 01 00 00 00 01 00 00 00 51 00 00 00 00 00 00 00 01 00 00 00 51 00 00 00 00 00 08 33 1A 00 00 00 01 00 00 00 01 00 00 00 53 00 00 00 00 00 00 00 01 00 00 00 53 00 00 00 00 00 08 33 24 00 00 00 01 00 00 00 01 00 00 00 54 00 00 00 00 00 00 00 01 00 00 00 54 00 00 00 00 00 09 54 66 00 00 00 01 00 00 00 01 00 00 00 55 00 00 00 00 00 00 00 01 00 00 00 55 00 00 00 00 00 0A 1E 28 00 00 00 01 00 00 00 01 00 00 00 5F 00 00 00 00 00 00 00 01 00 00 00 5F 00 00 00 00 00 0A 1E 32 00 00 00 01 00 00 00 01 00 00 00 60 00 00 00 00 00 00 00 01 00 00 00 60 00 00 00 00 00 0C 55 08 00 00 00 01 00 00 00 01 00 00 00 61 00 00 00 00 00 00 00 00 00 0C 55 12 00 00 00 01 00 00 00 01 00 00 00 62 00 00 00 00 00 00 00 00 00 0D 72 94 00 00 00 01 00 00 00 01 00 00 00 63 00 00 00 00 00 00 00 00 00 0D 72 9E 00 00 00 01 00 00 00 01 00 00 00 64 00 00 00 00 00 00 00 00 00 0E E9 E4 00 00 00 01 00 00 00 01 00 00 00 65 00 00 00 00 00 00 00 00 00 0E E9 EE 00 00 00 01 00 00 00 01 00 00 00 66 00 00 00 00 00 00 00 00 00 0E E9 F8 00 00 00 01 00 00 00 01 00 00 00 67 00 00 00 00 00 00 00 00 00 0E EA 02 00 00 00 01 00 00 00 01 00 00 00 68 00 00 00 00 00 00 00 00 00 0E EA 0C 00 00 00 01 00 00 00 01 00 00 00 6B 00 00 00 00 00 00 00 00 00 0E EA 16 00 00 00 01 00 00 00 01 00 00 00 6B 00 00 00 00 00 00 00 00 00 0F 85 98 00 00 00 01 00 00 00 01 00 00 00 69 00 00 00 00 00 00 00 00 00 0F 85 A2 00 00 00 01 00 00 00 01 00 00 00 6A 00 00 00 00 00 00 00 00 00 10 49 60 00 00 00 01 00 00 00 01 00 00 00 6C 00 00 00 00 00 00 00 00 00 10 49 6A 00 00 00 01 00 00 00 01 00 00 00 6D 00 00 00 00 00 00 00 00 00 10 6A 3A 00 00 00 01 00 00 00 01 00 00 00 6E 00 00 00 00 00 00 00 00 00 10 6A 44 00 00 00 01 00 00 00 01 00 00 00 6F 00 00 00 00 00 00 00 00 00 10 A5 18 00 00 00 01 00 00 00 01 00 00 00 70 00 00 00 00 00 00 00 00 00 10 A5 22 00 00 00 01 00 00 00 01 00 00 00 71 00 00 00 00 00 00 00 00 00 10 E6 E0 00 00 00 01 00 00 00 01 00 00 00 72 00 00 00 00 00 00 00 00 00 10 E6 EA 00 00 00 01 00 00 00 01 00 00 00 73 00 00 00 00 00 00 00 00 00 12 6A A6 00 00 00 01 00 00 00 01 00 00 00 74 00 00 00 00 00 00 00 00 00 12 6A B0 00 00 00 01 00 00 00 01 00 00 00 75 00 00 00 00 00 00 00 00 00 12 6A BA 00 00 00 01 00 00 00 01 00 00 00 76 00 00 00 00 00 00 00 00 00 12 6A C4 00 00 00 01 00 00 00 01 00 00 00 77 00 00 00 00 00 00 00 00 00 12 6A CE 00 00 00 01 00 00 00 01 00 00 00 78 00 00 00 00 00 00 00 00 00 12 6A D8 00 00 00 01 00 00 00 01 00 00 00 79 00 00 00 00 00 00 00 00 00 12 9F 26 00 00 00 01 00 00 00 01 00 00 00 7A 00 00 00 00 00 00 00 00 00 12 9F 30 00 00 00 01 00 00 00 01 00 00 00 7B 00 00 00 00 00 00 00 00 00 12 9F 3A 00 00 00 01 00 00 00 01 00 00 00 7C 00 00 00 00 00 00 00 00 00 12 9F 44 00 00 00 01 00 00 00 01 00 00 00 7D 00 00 00 00 00 00 00 00 00 12 9F 4E 00 00 00 01 00 00 00 01 00 00 00 7E 00 00 00 00 00 00 00 00 00 13 8C 24 00 00 00 01 00 00 00 01 00 00 00 7F 00 00 00 00 00 00 00 00 00 13 8C 2E 00 00 00 01 00 00 00 00 00 00 00 00 00 13 8C 38 00 00 00 01 00 00 00 01 00 00 00 80 00 00 00 00 00 00 00 00 00 13 8C 42 00 00 00 01 00 00 00 01 00 00 00 85 00 00 00 00 00 00 00 00 00 13 8C 4C 00 00 00 01 00 00 00 01 00 00 00 81 00 00 00 00 00 00 00 00 00 13 8C 56 00 00 00 01 00 00 00 01 00 00 00 86 00 00 00 00 00 00 00 00 00 13 8C 60 00 00 00 01 00 00 00 01 00 00 00 82 00 00 00 00 00 00 00 00 00 13 8C 6A 00 00 00 01 00 00 00 01 00 00 00 87 00 00 00 00 00 00 00 00 00 13 8C 74 00 00 00 01 00 00 00 01 00 00 00 83 00 00 00 00 00 00 00 00 00 13 8C 7E 00 00 00 01 00 00 00 01 00 00 00 88 00 00 00 00 00 00 00 00 00 13 8C 88 00 00 00 01 00 00 00 01 00 00 00 84 00 00 00 00 00 00 00 00 00 13 8C 92 00 00 00 01 00 00 00 01 00 00 00 89 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F");
                    oPacket.WriteInt(1);//KEY WEAPON //99
                    oPacket.WriteHexString("00 00 00 00 00 00 00 00 14 00 00 00 00");                                        
                }
                oPacket.WriteHexString("00 3E F2 00 00 00 00 56 00 06 02 D4 C0 18 05 00 00 00 00 00 00 00 00 06 01 01 00 00 00 00 00 00 00 00 00 01 00 00 00 00 01 00 00 00 00");

                for (int i = 0; i < GetPlayerCount(); i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }

        public void RewardEXP(ClientSession cs, InPacket Ip)
        {
            int XP = Ip.ReadInt();
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_DUNGEON_REWARD_EXP_ACK))
            {
                //oPacket.WriteHexString("00 00 00 01 00 05 3A 91 00 00 00 00 00 00 00 08 1E 00 00 00 00 00 00 08 1E 00 00 00 06 00 00 00 06 41 0C CC CD 00 00 00 0C 00 00 00 A0");
                oPacket.WriteInt(cs.CurrentRoom.GetPlayerCount());
                for (int i = 0; i < cs.CurrentRoom.GetPlayerCount(); i++)
                {
                    if (Slot[i].Active == false)
                        continue;
                    oPacket.WriteInt(Slot[i].cs.LoginUID);
                    oPacket.WriteByte(0);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(XP);
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(XP);
                    int MyCharPos = -1;
                    for (int t = 0; t < Slot[i].cs.MyCharacter.MyChar.Length; t++)
                        if (Slot[i].cs.MyCharacter.MyChar[t].CharType == Slot[i].cs.CurrentChar)
                            MyCharPos = t;
                    oPacket.WriteInt(Slot[i].cs.MyCharacter.MyChar[MyCharPos].Level);
                    oPacket.WriteInt(Slot[i].cs.MyCharacter.MyChar[MyCharPos].Level);
                    oPacket.WriteHexString("41 0C CC CD 00 00 00 0C 00 00 00 A0");
                    DataSet ds = new DataSet();
                    Database.Query(ref ds, "UPDATE   `character` SET  `Exp` = '{2}' WHERE `LoginUID` = '{0}' AND  `CharType` = '{1}'", Slot[i].cs.LoginUID, Slot[i].cs.MyCharacter.MyChar[MyCharPos].CharType, Slot[i].cs.MyCharacter.MyChar[MyCharPos].Exp + XP);
                    this.Slot[i].cs.MyCharacter.MyChar[MyCharPos].Exp = Slot[i].cs.MyCharacter.MyChar[MyCharPos].Exp + XP;
                }                
                for (int i = 0; i < cs.CurrentRoom.GetPlayerCount(); i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble();
                    }
                }                
            }
        }

        //LogFactory.GetLog("EXP").LogInfo("RESULTADO: " + Slot[i].cs.MyCharacter.MyChar[MyCharPos].Exp);
        /*oPacket.WriteInt(XP);
        oPacket.WriteHexString("00 00 00 00");
        oPacket.WriteInt(XP);
        oPacket.WriteHexString("00 00 00 04 00 00 00 05 42 04 00 00 00 00 00 0A 00 00 00 A0");
        */
        //LogFactory.GetLog("EXP").LogHex("EXP: ", Ip.ToArray());
        //LogFactory.GetLog("EXP").LogInfo("CHARTYPE: " + Slot[i].cs.MyCharacter.MyChar[MyCharPos].CharType);
        //LogFactory.GetLog("EXP").LogInfo("ATUAL EXP: " + Slot[i].cs.MyCharacter.MyChar[MyCharPos].Exp);
        //LogFactory.GetLog("EXP").LogInfo("XP: " + XP);
        //LogFactory.GetLog("EXP").LogInfo("CALCULO: " + Slot[i].cs.MyCharacter.MyChar[MyCharPos].Exp + XP);                    
        /*public void OnGameEnd(ClientSession cs, InPacket ip)
        {
            Playing = false;

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_END_GAME_BROAD))
            {
                oPacket.WriteInt(1); // 00 00 00 01

                oPacket.WriteInt(GetPlayerCount()); // 00 00 00 02
                for( int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == false)
                        continue;

                    oPacket.WriteInt(Slot[i].cs.Login.Length * 2);
                    oPacket.WriteUnicodeString(Slot[i].cs.Login);
                    oPacket.WriteInt(1); // 00 00 00 01
                    oPacket.WriteInt(8295); // 00 00 20 67
                    oPacket.WriteInt(150); // 00 00 00 96
                    oPacket.WriteInt(0); // 00 00 00 00
                    oPacket.WriteInt(4); // 00 00 00 04
                    oPacket.WriteInt(0); // 00 00 00 00
                    oPacket.WriteInt(0); // 00 00 00 00
                    oPacket.WriteHexString("00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 0E 00 00 00 01 00 00 00 01 0E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 01 18 8C 00 00 00 01 2C DD 78 9F 00 00 00 01 00 00 00 01 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00");
                    oPacket.WriteHexString("00 00 00 4E 00 00 00 07 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 13 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 14 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 15 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 16 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 17 00 00 00 01 07 01 00 01 00 00 00 00 00 00 00 18 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 19 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1B 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 24 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 27 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 28 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 29 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2A 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 2B 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2C 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2D 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2E 00 00 00 01 03 01 00 00 00 01 00 00 00 00 00 2F 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 30 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 31 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 32 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 33 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 34 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 35 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 36 00 00 00 01 07 01 00 01 00 02 00 00 00 00 00 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 38 00 00 00 00 00 00 00 00 00 00 00 00 00 00 39 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 00 00 00 00 00 00 00 00 00 00 00 00 00 00 46 00 00 00 00 00 00 00 00 00 00 00 00 00 00 47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 00 00 00 00 00 00 00 00 00 00 00 00 00 00 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 53 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 55 00 00 00 00 00 00 00 00 00 00 00 00 00 00 56 00 00 00 00 00 00 00 00 00 00 00 00 00 00 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 00 00 00 00 00 00 00 00 00 59 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 0E 0E 00 00 00 00 00 00 01 CB 00 00 00 00 00 1B 0A F1 00 00 00 18 00 00 00 04 00 00 00 A0 00 00 00 04 00 00 00 A0 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 04 00 00 00 00 00 00 00 30 00 00 00 00 00 00 00 30 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 14 00 07 0D BE 00 00 00 01 00 98 96 81 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 0D C8 00 00 00 01 00 98 96 82 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 0D D2 00 00 00 01 00 98 96 83 00 00 00 00 56 7B 21 D0 56 79 D0 50 00 00 00 00 00 07 0D DC 00 00 00 01 00 98 96 84 00 00 00 00 56 7B 21 D0 56 79 D0 50 00 00 00 00 00 07 19 08 00 00 00 01 00 98 96 81 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 19 12 00 00 00 01 00 98 96 82 00 00 00 00 56 76 25 68 56 74 D3 E8 00 00 00 00 00 07 22 18 00 00 00 01 00 98 97 69 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 22 2C 00 00 00 01 00 98 97 6B 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 22 90 00 00 00 01 00 98 97 75 00 00 00 00 56 76 25 E0 56 74 D4 60 00 00 00 00 00 07 24 52 00 00 00 01 00 98 96 81 00 00 00 01 56 76 25 68 56 74 D3 E8 00 00 00 01 00 07 24 5C 00 00 00 01 00 98 96 82 00 00 00 01 56 76 25 68 56 74 D3 E8 00 00 00 01 00 07 24 8E 00 00 00 01 00 98 96 87 00 00 00 00 56 7E 0A 30 56 7C B8 B0 00 00 00 00 00 07 24 98 00 00 00 01 00 98 96 88 00 00 00 00 56 7E 0A 30 56 7C B8 B0 00 00 00 00 00 07 24 A2 00 00 00 01 00 98 96 89 00 00 00 00 56 7E 0B D4 56 7C BA 54 00 00 00 00 00 07 24 AC 00 00 00 01 00 98 96 8A 00 00 00 00 56 7E 0B D4 56 7C BA 54 00 00 00 00 00 0A E8 58 00 00 00 01 00 98 96 81 00 00 00 00 56 7D 9E 60 56 7C 4C E0 00 00 00 00 00 0A E8 62 00 00 00 01 00 98 96 82 00 00 00 00 56 7D 9E 60 56 7C 4C E0 00 00 00 00 00 0A E8 6C 00 00 00 01 00 98 96 83 00 00 00 00 56 7E 07 9C 56 7C B6 1C 00 00 00 00 00 0A E8 76 00 00 00 01 00 98 96 84 00 00 00 00 56 7E 07 9C 56 7C B6 1C 00 00 00 00 00 12 9D FA 00 00 00 01 00 98 98 0F 00 00 00 01 56 85 69 24 56 84 17 A4 00 00 00 01 00 00 00 01 00 01 18 8C 00 00 00 01 00 00 00 00");
                    oPacket.WriteInt(0);
                    oPacket.WriteInt(75); // 00 00 00 4B
                    oPacket.WriteInt(132); // 00 00 00 84
                    oPacket.WriteUnicodeString("00 00 78 6E 00 00 00 01 00 00 00 01 00 00 00 14 00 00 00 00 00 00 00 01 00 00 00 14 00 00 00 00 00 00 78 78 00 00 00 01 00 00 00 01 00 00 00 15 00 00 00 00 00 00 00 01 00 00 00 15 00 00 00 00 00 00 78 82 00 00 00 01 00 00 00 01 00 00 00 16 00 00 00 00 00 00 00 01 00 00 00 16 00 00 00 00 00 00 78 8C 00 00 00 01 00 00 00 01 00 00 00 17 00 00 00 00 00 00 00 01 00 00 00 17 00 00 00 00 00 00 78 96 00 00 00 01 00 00 00 01 00 00 00 18 00 00 00 00 00 00 00 01 00 00 00 18 00 00 00 00 00 00 78 A0 00 00 00 01 00 00 00 01 00 00 00 19 00 00 00 00 00 00 00 01 00 00 00 19 00 00 00 00 00 00 78 AA 00 00 00 01 00 00 00 01 00 00 00 1A 00 00 00 00 00 00 00 01 00 00 00 1A 00 00 00 00 00 00 78 B4 00 00 00 01 00 00 00 01 00 00 00 1B 00 00 00 00 00 00 00 01 00 00 00 1B 00 00 00 00 00 00 78 BE 00 00 00 01 00 00 00 01 00 00 00 1C 00 00 00 00 00 00 00 01 00 00 00 1C 00 00 00 00 00 00 78 C8 00 00 00 01 00 00 00 01 00 00 00 1D 00 00 00 00 00 00 00 01 00 00 00 1D 00 00 00 00 00 00 78 D2 00 00 00 01 00 00 00 01 00 00 00 1E 00 00 00 00 00 00 00 01 00 00 00 1E 00 00 00 00 00 00 78 DC 00 00 00 01 00 00 00 01 00 00 00 1F 00 00 00 00 00 00 00 01 00 00 00 1F 00 00 00 00 00 00 78 E6 00 00 00 01 00 00 00 01 00 00 00 20 00 00 00 00 00 00 00 01 00 00 00 20 00 00 00 00 00 00 78 F0 00 00 00 01 00 00 00 01 00 00 00 21 00 00 00 00 00 00 00 01 00 00 00 21 00 00 00 00 00 00 78 FA 00 00 00 01 00 00 00 01 00 00 00 22 00 00 00 00 00 00 00 01 00 00 00 22 00 00 00 00 00 00 79 04 00 00 00 01 00 00 00 01 00 00 00 23 00 00 00 00 00 00 00 01 00 00 00 23 00 00 00 00 00 00 79 0E 00 00 00 01 00 00 00 01 00 00 00 24 00 00 00 00 00 00 00 01 00 00 00 24 00 00 00 00 00 00 79 18 00 00 00 01 00 00 00 01 00 00 00 25 00 00 00 00 00 00 00 01 00 00 00 25 00 00 00 00 00 00 79 22 00 00 00 01 00 00 00 01 00 00 00 26 00 00 00 00 00 00 00 01 00 00 00 26 00 00 00 00 00 00 79 2C 00 00 00 01 00 00 00 01 00 00 00 28 00 00 00 00 00 00 00 01 00 00 00 28 00 00 00 00 00 00 79 36 00 00 00 01 00 00 00 01 00 00 00 2A 00 00 00 00 00 00 00 01 00 00 00 2A 00 00 00 00 00 00 79 40 00 00 00 01 00 00 00 01 00 00 00 2C 00 00 00 00 00 00 00 01 00 00 00 2C 00 00 00 00 00 00 79 4A 00 00 00 01 00 00 00 01 00 00 00 2E 00 00 00 00 00 00 00 01 00 00 00 2E 00 00 00 00 00 00 85 C0 00 00 00 01 00 00 00 01 00 00 00 30 00 00 00 00 00 00 00 01 00 00 00 30 00 00 00 00 00 00 85 CA 00 00 00 01 00 00 00 01 00 00 00 32 00 00 00 00 00 00 00 01 00 00 00 32 00 00 00 00 00 00 85 D4 00 00 00 01 00 00 00 01 00 00 00 42 00 00 00 00 00 00 00 01 00 00 00 42 00 00 00 00 00 00 85 DE 00 00 00 01 00 00 00 01 00 00 00 44 00 00 00 00 00 00 00 01 00 00 00 44 00 00 00 00 00 00 85 E8 00 00 00 01 00 00 00 01 00 00 00 46 00 00 00 00 00 00 00 01 00 00 00 46 00 00 00 00 00 00 85 F2 00 00 00 01 00 00 00 01 00 00 00 48 00 00 00 00 00 00 00 01 00 00 00 48 00 00 00 00 00 00 85 FC 00 00 00 01 00 00 00 01 00 00 00 4A 00 00 00 00 00 00 00 01 00 00 00 4A 00 00 00 00 00 00 86 06 00 00 00 01 00 00 00 01 00 00 00 4C 00 00 00 00 00 00 00 01 00 00 00 4C 00 00 00 00 00 00 86 10 00 00 00 01 00 00 00 01 00 00 00 50 00 00 00 00 00 00 00 01 00 00 00 50 00 00 00 00 00 00 86 1A 00 00 00 01 00 00 00 01 00 00 00 52 00 00 00 00 00 00 00 01 00 00 00 52 00 00 00 00 00 00 86 24 00 00 00 01 00 00 00 01 00 00 00 56 00 00 00 00 00 00 00 01 00 00 00 56 00 00 00 00 00 01 45 8C 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 01 45 96 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 01 45 A0 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 00 00 01 45 AA 00 00 00 01 00 00 00 01 00 00 00 03 00 00 00 00 00 00 00 01 00 00 00 03 00 00 00 00 00 01 45 B4 00 00 00 01 00 00 00 01 00 00 00 04 00 00 00 00 00 00 00 01 00 00 00 04 00 00 00 00 00 01 45 BE 00 00 00 01 00 00 00 01 00 00 00 05 00 00 00 00 00 00 00 01 00 00 00 05 00 00 00 00 00 01 45 C8 00 00 00 01 00 00 00 01 00 00 00 06 00 00 00 00 00 00 00 01 00 00 00 06 00 00 00 00 00 01 45 D2 00 00 00 01 00 00 00 01 00 00 00 07 00 00 00 00 00 00 00 01 00 00 00 07 00 00 00 00 00 01 45 DC 00 00 00 01 00 00 00 01 00 00 00 08 00 00 00 00 00 00 00 01 00 00 00 08 00 00 00 00 00 01 45 E6 00 00 00 01 00 00 00 01 00 00 00 09 00 00 00 00 00 00 00 01 00 00 00 09 00 00 00 00 00 01 45 F0 00 00 00 01 00 00 00 01 00 00 00 0A 00 00 00 00 00 00 00 01 00 00 00 0A 00 00 00 00 00 01 45 FA 00 00 00 01 00 00 00 01 00 00 00 0B 00 00 00 00 00 00 00 01 00 00 00 0B 00 00 00 00 00 01 46 04 00 00 00 01 00 00 00 01 00 00 00 0C 00 00 00 00 00 00 00 01 00 00 00 0C 00 00 00 00 00 01 46 0E 00 00 00 01 00 00 00 01 00 00 00 0D 00 00 00 00 00 00 00 01 00 00 00 0D 00 00 00 00 00 01 46 18 00 00 00 01 00 00 00 01 00 00 00 0E 00 00 00 00 00 00 00 01 00 00 00 0E 00 00 00 00 00 01 46 22 00 00 00 01 00 00 00 01 00 00 00 0F 00 00 00 00 00 00 00 01 00 00 00 0F 00 00 00 00 00 01 46 2C 00 00 00 01 00 00 00 01 00 00 00 10 00 00 00 00 00 00 00 01 00 00 00 10 00 00 00 00 00 01 46 36 00 00 00 01 00 00 00 01 00 00 00 11 00 00 00 00 00 00 00 01 00 00 00 11 00 00 00 00 00 01 46 40 00 00 00 01 00 00 00 01 00 00 00 12 00 00 00 00 00 00 00 01 00 00 00 12 00 00 00 00 00 01 46 4A 00 00 00 01 00 00 00 01 00 00 00 13 00 00 00 00 00 00 00 01 00 00 00 13 00 00 00 00 00 01 9B 18 00 00 00 01 00 00 00 01 00 00 00 27 00 00 00 00 00 00 00 01 00 00 00 27 00 00 00 00 00 01 FF 40 00 00 00 01 00 00 00 01 00 00 00 29 00 00 00 00 00 00 00 01 00 00 00 29 00 00 00 00 00 01 FF 4A 00 00 00 01 00 00 00 01 00 00 00 2B 00 00 00 00 00 00 00 01 00 00 00 2B 00 00 00 00 00 02 1E 9E 00 00 00 01 00 00 00 01 00 00 00 2D 00 00 00 00 00 00 00 01 00 00 00 2D 00 00 00 00 00 02 29 A2 00 00 00 01 00 00 00 01 00 00 00 2F 00 00 00 00 00 00 00 01 00 00 00 2F 00 00 00 00 00 02 41 08 00 00 00 01 00 00 00 01 00 00 00 31 00 00 00 00 00 00 00 01 00 00 00 31 00 00 00 00 00 02 CA 56 00 00 00 01 00 00 00 01 00 00 00 33 00 00 00 00 00 00 00 01 00 00 00 33 00 00 00 00 00 02 CA 60 00 00 00 01 00 00 00 01 00 00 00 34 00 00 00 00 00 00 00 01 00 00 00 34 00 00 00 00 00 02 CA 6A 00 00 00 01 00 00 00 01 00 00 00 35 00 00 00 00 00 00 00 01 00 00 00 35 00 00 00 00 00 02 CA 74 00 00 00 01 00 00 00 01 00 00 00 36 00 00 00 00 00 00 00 01 00 00 00 36 00 00 00 00 00 02 D1 2C 00 00 00 01 00 00 00 01 00 00 00 37 00 00 00 00 00 00 00 01 00 00 00 37 00 00 00 00 00 02 D1 36 00 00 00 01 00 00 00 01 00 00 00 38 00 00 00 00 00 00 00 01 00 00 00 38 00 00 00 00 00 02 D1 40 00 00 00 01 00 00 00 01 00 00 00 39 00 00 00 00 00 00 00 01 00 00 00 39 00 00 00 00 00 02 D1 4A 00 00 00 01 00 00 00 01 00 00 00 3A 00 00 00 00 00 00 00 01 00 00 00 3A 00 00 00 00 00 02 E0 54 00 00 00 01 00 00 00 01 00 00 00 3B 00 00 00 00 00 00 00 01 00 00 00 3B 00 00 00 00 00 02 E0 5E 00 00 00 01 00 00 00 01 00 00 00 3C 00 00 00 00 00 00 00 01 00 00 00 3C 00 00 00 00 00 02 E0 68 00 00 00 01 00 00 00 01 00 00 00 3D 00 00 00 00 00 00 00 01 00 00 00 3D 00 00 00 00 00 02 E0 72 00 00 00 01 00 00 00 01 00 00 00 3E 00 00 00 00 00 00 00 01 00 00 00 3E 00 00 00 00 00 02 E0 7C 00 00 00 01 00 00 00 01 00 00 00 3F 00 00 00 00 00 00 00 01 00 00 00 3F 00 00 00 00 00 02 E0 86 00 00 00 01 00 00 00 01 00 00 00 40 00 00 00 00 00 00 00 01 00 00 00 40 00 00 00 00 00 03 4A 76 00 00 00 01 00 00 00 01 00 00 00 41 00 00 00 00 00 00 00 01 00 00 00 41 00 00 00 00 00 03 4A 80 00 00 00 01 00 00 00 01 00 00 00 43 00 00 00 00 00 00 00 01 00 00 00 43 00 00 00 00 00 03 4A 8A 00 00 00 01 00 00 00 01 00 00 00 45 00 00 00 00 00 00 00 01 00 00 00 45 00 00 00 00 00 03 4A 94 00 00 00 01 00 00 00 01 00 00 00 47 00 00 00 00 00 00 00 01 00 00 00 47 00 00 00 00 00 04 89 86 00 00 00 01 00 00 00 01 00 00 00 49 00 00 00 00 00 00 00 01 00 00 00 49 00 00 00 00 00 04 89 90 00 00 00 01 00 00 00 01 00 00 00 4B 00 00 00 00 00 00 00 01 00 00 00 4B 00 00 00 00 00 05 0F 6E 00 00 00 01 00 00 00 01 00 00 00 4D 00 00 00 00 00 00 00 01 00 00 00 4D 00 00 00 00 00 05 0F 78 00 00 00 01 00 00 00 01 00 00 00 4E 00 00 00 00 00 00 00 01 00 00 00 4E 00 00 00 00 00 05 9A 42 00 00 00 01 00 00 00 01 00 00 00 4F 00 00 00 00 00 00 00 01 00 00 00 4F 00 00 00 00 00 06 E2 3A 00 00 00 01 00 00 00 01 00 00 00 51 00 00 00 00 00 00 00 01 00 00 00 51 00 00 00 00 00 08 33 1A 00 00 00 01 00 00 00 01 00 00 00 53 00 00 00 00 00 00 00 01 00 00 00 53 00 00 00 00 00 08 33 24 00 00 00 01 00 00 00 01 00 00 00 54 00 00 00 00 00 00 00 01 00 00 00 54 00 00 00 00 00 09 54 66 00 00 00 01 00 00 00 01 00 00 00 55 00 00 00 00 00 00 00 01 00 00 00 55 00 00 00 00 00 0A 1E 28 00 00 00 01 00 00 00 01 00 00 00 5F 00 00 00 00 00 00 00 01 00 00 00 5F 00 00 00 00 00 0A 1E 32 00 00 00 01 00 00 00 01 00 00 00 60 00 00 00 00 00 00 00 01 00 00 00 60 00 00 00 00 00 0C 55 08 00 00 00 01 00 00 00 01 00 00 00 61 00 00 00 00 00 00 00 01 00 00 00 61 00 00 00 00 00 0C 55 12 00 00 00 01 00 00 00 01 00 00 00 62 00 00 00 00 00 00 00 01 00 00 00 62 00 00 00 00 00 0D 72 94 00 00 00 01 00 00 00 01 00 00 00 63 00 00 00 00 00 00 00 01 00 00 00 63 00 00 00 00 00 0D 72 9E 00 00 00 01 00 00 00 01 00 00 00 64 00 00 00 00 00 00 00 01 00 00 00 64 00 00 00 00 00 0E E9 E4 00 00 00 01 00 00 00 01 00 00 00 65 00 00 00 00 00 00 00 01 00 00 00 65 00 00 00 00 00 0E E9 EE 00 00 00 01 00 00 00 01 00 00 00 66 00 00 00 00 00 00 00 01 00 00 00 66 00 00 00 00 00 0E E9 F8 00 00 00 01 00 00 00 01 00 00 00 67 00 00 00 00 00 00 00 01 00 00 00 67 00 00 00 00 00 0E EA 02 00 00 00 01 00 00 00 01 00 00 00 68 00 00 00 00 00 00 00 01 00 00 00 68 00 00 00 00 00 0E EA 0C 00 00 00 01 00 00 00 01 00 00 00 6B 00 00 00 00 00 00 00 01 00 00 00 6B 00 00 00 00 00 0E EA 16 00 00 00 01 00 00 00 01 00 00 00 6B 00 00 00 00 00 00 00 01 00 00 00 6B 00 00 00 00 00 0F 85 98 00 00 00 01 00 00 00 01 00 00 00 69 00 00 00 00 00 00 00 01 00 00 00 69 00 00 00 00 00 0F 85 A2 00 00 00 01 00 00 00 01 00 00 00 6A 00 00 00 00 00 00 00 01 00 00 00 6A 00 00 00 00 00 10 49 60 00 00 00 01 00 00 00 01 00 00 00 6C 00 00 00 00 00 00 00 01 00 00 00 6C 00 00 00 00 00 10 49 6A 00 00 00 01 00 00 00 01 00 00 00 6D 00 00 00 00 00 00 00 01 00 00 00 6D 00 00 00 00 00 10 6A 3A 00 00 00 01 00 00 00 01 00 00 00 6E 00 00 00 00 00 00 00 01 00 00 00 6E 00 00 00 00 00 10 6A 44 00 00 00 01 00 00 00 01 00 00 00 6F 00 00 00 00 00 00 00 01 00 00 00 6F 00 00 00 00 00 10 A5 18 00 00 00 01 00 00 00 01 00 00 00 70 00 00 00 00 00 00 00 01 00 00 00 70 00 00 00 00 00 10 A5 22 00 00 00 01 00 00 00 01 00 00 00 71 00 00 00 00 00 00 00 01 00 00 00 71 00 00 00 00 00 10 E6 E0 00 00 00 01 00 00 00 01 00 00 00 72 00 00 00 00 00 00 00 01 00 00 00 72 00 00 00 00 00 10 E6 EA 00 00 00 01 00 00 00 01 00 00 00 73 00 00 00 00 00 00 00 01 00 00 00 73 00 00 00 00 00 12 6A A6 00 00 00 01 00 00 00 01 00 00 00 74 00 00 00 00 00 00 00 01 00 00 00 74 00 00 00 00 00 12 6A B0 00 00 00 01 00 00 00 01 00 00 00 75 00 00 00 00 00 00 00 01 00 00 00 75 00 00 00 00 00 12 6A BA 00 00 00 01 00 00 00 01 00 00 00 76 00 00 00 00 00 00 00 01 00 00 00 76 00 00 00 00 00 12 6A C4 00 00 00 01 00 00 00 01 00 00 00 77 00 00 00 00 00 00 00 01 00 00 00 77 00 00 00 00 00 12 6A CE 00 00 00 01 00 00 00 01 00 00 00 78 00 00 00 00 00 00 00 01 00 00 00 78 00 00 00 00 00 12 6A D8 00 00 00 01 00 00 00 01 00 00 00 79 00 00 00 00 00 00 00 01 00 00 00 79 00 00 00 00 00 12 9F 26 00 00 00 01 00 00 00 01 00 00 00 7A 00 00 00 00 00 00 00 01 00 00 00 7A 00 00 00 00 00 12 9F 30 00 00 00 01 00 00 00 01 00 00 00 7B 00 00 00 00 00 00 00 01 00 00 00 7B 00 00 00 00 00 12 9F 3A 00 00 00 01 00 00 00 01 00 00 00 7C 00 00 00 00 00 00 00 01 00 00 00 7C 00 00 00 00 00 12 9F 44 00 00 00 01 00 00 00 01 00 00 00 7D 00 00 00 00 00 00 00 01 00 00 00 7D 00 00 00 00 00 12 9F 4E 00 00 00 01 00 00 00 01 00 00 00 7E 00 00 00 00 00 00 00 01 00 00 00 7E 00 00 00 00 00 13 8C 24 00 00 00 01 00 00 00 01 00 00 00 7F 00 00 00 00 00 00 00 01 00 00 00 7F 00 00 00 00 00 13 8C 2E 00 00 00 01 00 00 00 00 00 00 00 00 00 13 8C 38 00 00 00 01 00 00 00 01 00 00 00 80 00 00 00 00 00 00 00 01 00 00 00 80 00 00 00 00 00 13 8C 42 00 00 00 01 00 00 00 01 00 00 00 85 00 00 00 00 00 00 00 01 00 00 00 85 00 00 00 00 00 13 8C 4C 00 00 00 01 00 00 00 01 00 00 00 81 00 00 00 00 00 00 00 01 00 00 00 81 00 00 00 00 00 13 8C 56 00 00 00 01 00 00 00 01 00 00 00 86 00 00 00 00 00 00 00 01 00 00 00 86 00 00 00 00 00 13 8C 60 00 00 00 01 00 00 00 01 00 00 00 82 00 00 00 00 00 00 00 01 00 00 00 82 00 00 00 00 00 13 8C 6A 00 00 00 01 00 00 00 01 00 00 00 87 00 00 00 00 00 00 00 01 00 00 00 87 00 00 00 00 00 13 8C 74 00 00 00 01 00 00 00 01 00 00 00 83 00 00 00 00 00 00 00 01 00 00 00 83 00 00 00 00 00 13 8C 7E 00 00 00 01 00 00 00 01 00 00 00 88 00 00 00 00 00 00 00 01 00 00 00 88 00 00 00 00 00 13 8C 88 00 00 00 01 00 00 00 01 00 00 00 84 00 00 00 00 00 00 00 01 00 00 00 84 00 00 00 00 00 13 8C 92 00 00 00 01 00 00 00 01 00 00 00 89 00 00 00 00 00 00 00 01 00 00 00 89 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00 00 00 00 00 00 14 00 00 00 00");
                }

                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }*/

        public void OnSetPressState(ClientSession cs, InPacket ip)
        {
            int state = ip.ReadInt();
            
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_PRESS_STATE_NOT))
            {
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(state);

                for (int i = 0; i < 6; i++)
                {
                    if (Slot[i].Active == true)
                    {
                        oPacket.Assemble(Slot[i].cs.CRYPT_KEY, Slot[i].cs.CRYPT_HMAC, Slot[i].cs.CRYPT_PREFIX, Slot[i].cs.CRYPT_COUNT);
                        Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }
    }
}
