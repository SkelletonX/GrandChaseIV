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
    public class Character
    {
        public struct sEquip
        {
            public int ItemUID;
            public int ItemID;
        }

        public struct sSkill
        {
            public int SkillGroup;
            public int SkillID;
        }
        public struct SkillInfo
        {
            public byte charindex;
            public byte SkillGroup;
            public int SkilliD;
        }

        public struct sChar
        {
            public int CharType;
            public int Win;
            public int Loss;
            public int Promotion;
            public int Exp;
            public int Level;
            public bool WeaponChange;
            public int WeaponChangeID;
            public int Pet;
            public int TotalEquips;
            public sEquip[] Equip;
            public sSkill[] MySkill;
            public sSkill[] EquipSkill;
            public SkillInfo[] skillinfo;
        }

        public sChar[] MyChar = new sChar[0];

        public bool isHaveChar(int findCharType)
        {
            for (int i = 0; i < MyChar.Length; i++)
            {
                if (MyChar[i].CharType == findCharType)
                    return true;
            }

            return false;
        }

        public int findCharIndex(int findCharType)
        {
            for (int i = 0; i < MyChar.Length; i++)
            {
                if (MyChar[i].CharType == findCharType)
                    return i;
            }

            return -1;
        }

        public void LoadCharacter(ClientSession cs)
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT * FROM `character` WHERE LoginUID = '{0}' ORDER BY CharType ASC", cs.LoginUID);


            Array.Resize(ref MyChar, ds.Tables[0].Rows.Count);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                MyChar[i].CharType = Convert.ToInt32(ds.Tables[0].Rows[i]["CharType"].ToString());
                MyChar[i].Promotion = Convert.ToInt32(ds.Tables[0].Rows[i]["Promotion"].ToString());
                MyChar[i].Exp = Convert.ToInt32(ds.Tables[0].Rows[i]["Exp"].ToString());
                MyChar[i].Level = Convert.ToInt32(ds.Tables[0].Rows[i]["Level"].ToString());
                MyChar[i].WeaponChange = Convert.ToInt32(ds.Tables[0].Rows[i]["WeaponChange"].ToString()) == 1 ? true : false;
                MyChar[i].WeaponChangeID = Convert.ToInt32(ds.Tables[0].Rows[i]["WeaponChangeID"].ToString());
                MyChar[i].Pet = Convert.ToInt32(ds.Tables[0].Rows[i]["Pet"].ToString());
                MyChar[i].Win= Convert.ToInt32(ds.Tables[0].Rows[i]["Win"].ToString());
                MyChar[i].Loss= Convert.ToInt32(ds.Tables[0].Rows[i]["Loss"].ToString());

                
                DataSet ds2 = new DataSet();
                Database.Query(ref ds2, "SELECT * FROM `equipment` WHERE LoginUID = '{0}' AND CharType = '{1}' ORDER BY ItemUID ASC", cs.LoginUID, MyChar[i].CharType);

                
                Array.Resize(ref MyChar[i].Equip, ds2.Tables[0].Rows.Count);
                for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                {
                    MyChar[i].Equip[j].ItemUID = Convert.ToInt32(ds2.Tables[0].Rows[j]["ItemUID"].ToString());
                    MyChar[i].Equip[j].ItemID = Convert.ToInt32(ds2.Tables[0].Rows[j]["ItemID"].ToString());
                }

                DataSet ds3 = new DataSet();
                Database.Query(ref ds3, "SELECT * FROM `skilltree` WHERE `LoginUI` = '{0}'", cs.LoginUID);
                Array.Resize(ref MyChar[i].skillinfo, ds3.Tables[0].Rows.Count);
                for (int k = 0; k < MyChar[i].skillinfo.Length; k++)
                {
                    MyChar[i].skillinfo[k].charindex= Convert.ToByte(ds3.Tables[0].Rows[k]["charindex"].ToString());
                    MyChar[i].skillinfo[k].SkillGroup = Convert.ToByte(ds3.Tables[0].Rows[k]["SkillGroup"].ToString());
                    MyChar[i].skillinfo[k].SkilliD = Convert.ToInt32(ds3.Tables[0].Rows[k]["SkillID"].ToString());
                }

               
                Array.Resize(ref MyChar[i].EquipSkill, 0);
                Array.Resize(ref MyChar[i].MySkill, 0);
            }
        }

        public void OnEquipItem(ClientSession cs, InPacket ip)
        {
            int LoginIDLen = ip.ReadInt();
            string LoginID = ip.ReadUnicodeString(LoginIDLen);
            ip.ReadInt(); // 00 00 00 00
            byte CharNum = ip.ReadByte();
            for(byte i = 0; i < CharNum; i++)
            {
                byte TargetChar = ip.ReadByte();
                int EquipCount = ip.ReadInt();

                int MyCharPos = -1;
                for (int t = 0; t < cs.MyCharacter.MyChar.Length; t++)
                    if (cs.MyCharacter.MyChar[t].CharType == TargetChar)
                        MyCharPos = t;

                // 내가 가진 캐릭터 목록에 없다
                if (MyCharPos == -1)
                    continue;

                //Array.Resize(ref cs.MyCharacter.MyChar[MyCharPos].Equip, EquipCount);
                int len = cs.MyCharacter.MyChar[MyCharPos].Equip.Length;
                for (int j = 0; j < EquipCount; j++)
                {
                    int kind = 0;
                    int ItemID = ip.ReadInt();
                    ip.ReadInt(); // 00 00 00 01
                    int ItemUID = ip.ReadInt();
                    ip.ReadInt(); // 00 00 00 00
                    ip.ReadInt(); // 00 00 00 00
                    ip.ReadInt(); // 00 00 00 00
                    ip.ReadInt(); // 00 00 00 00
                    ip.ReadByte(); // 00 00 00
                    ip.ReadByte(); //
                    ip.ReadByte(); //
                    /*        
                     * helm 0        
                     * upper 1        
                     * lower 2        
                     * Weapon 3        
                     * gloves 8        
                     * shoes 9        
                     * Circlet 10        
                     * Wings 12        
                     * Mask 11        
                     * cloak 13        
                     * Stompers 14        
                     * Shields 15        
                     */
                    DataSet ds3 = new DataSet();
                    Database.Query(ref ds3, "SELECT   `Kind` FROM  `gc`.`goodsinfolist` WHERE `GoodsID` = '{0}'", ItemID);
                    if (ds3.Tables[0].Rows.Count == 0)
                    {
                        LogFactory.GetLog("EQUIPS").LogWarning("ITEM NAO EXISTE!");
                        kind =0;
                    }
                    else
                    {
                        kind = Convert.ToInt32(ds3.Tables[0].Rows[0]["Kind"].ToString());
                    }
                    int type = 0;
                    if (kind == 0)
                    {
                        type = 0;
                    }
                    if (kind == 1)
                    {
                        type = 1;
                    }
                    if (kind == 2)
                    {
                        type = 2;
                    }
                    if (kind == 3)
                    {
                        type = 3;
                    }
                    if (kind == 8)
                    {
                        type = 4;
                    }
                    if (kind == 9)
                    {
                        type = 5;
                    }
                    if (kind == 13)
                    {
                        type = 6;
                    }
                    //cs.MyCharacter.MyChar[MyCharPos].Equip[j].ItemID = ItemID;  
                    DataSet ds2 = new DataSet();
                    Database.Query(ref ds2, "SELECT   `itemID` FROM  `gc`.`equipment` WHERE `LoginUID` = '{0}' AND `CharType` = '{1}' AND `ItemUID` = '{2}'", cs.LoginUID, MyChar[MyCharPos].CharType, type);
                    if (ds2.Tables[0].Rows.Count == 0)
                    {
                        DataSet ds = new DataSet();
                        Database.Query(ref ds, "INSERT INTO `gc`.`equipment` (  `LoginUID`,  `CharType`,  `ItemType`,  `ItemID`,`ItemUID`) VALUES  (    '{0}',    '{1}',    '{2}',    '{3}'  ,'{4}')", cs.LoginUID, MyChar[MyCharPos].CharType, type, ItemID, ItemID);
                    }
                    else
                    {
                        DataSet ds = new DataSet();
                        Database.Query(ref ds, "UPDATE   `gc`.`equipment` SET  `ItemID` = '{0}' WHERE `LoginUID` = '{1}'   AND `CharType` = '{2}'   AND `ItemType` = '{3}'", ItemID, cs.LoginUID, MyChar[MyCharPos].CharType, type);
                    }                    
                    if (EquipCount > len)
                    {
                        for (int k = 0; k < EquipCount; k++)
                        {
                            len++;
                        }
                    }
                    LogFactory.GetLog("EQUIPAMENTOS").LogInfo("ATUAL TYPE: "+type);
                    if (type > len)
                    {
                        for (int h = 0; h < type; h++)
                        {
                            type--;
                            LogFactory.GetLog("EQUIPAMENTOS").LogInfo("ATUAL TYPE: " + type);
                        }
                    }                    
                    Array.Resize(ref cs.MyCharacter.MyChar[MyCharPos].Equip, len);
                    cs.MyCharacter.MyChar[MyCharPos].TotalEquips++;
                    LogFactory.GetLog("EQUIPS").LogInfo("TOTAL: " + cs.MyCharacter.MyChar[MyCharPos].TotalEquips);
                    cs.MyCharacter.MyChar[MyCharPos].Equip[type].ItemID = ItemID;
                    cs.MyCharacter.MyChar[MyCharPos].Equip[type].ItemUID = ItemUID;
                    
                    //Array.Resize(ref cs.MyCharacter.MyChar[MyCharPos].Equip, EquipCount++);
                    /*cs.MyCharacter.MyChar[MyCharPos].Equip[type].ItemID = ItemID;
                    cs.MyCharacter.MyChar[MyCharPos].Equip[type].ItemUID = ItemUID;*/
                     
                }

                // 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
                ip.Skip(99);
            }

            // 패킷 구조 똑같이 보내면 된다.
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_EQUIP_ITEM_BROAD))
            {
                oPacket.WriteInt(cs.Login.Length * 2);
                oPacket.WriteUnicodeString(cs.Login);
                oPacket.WriteInt(0);
                oPacket.WriteByte((byte)cs.MyCharacter.MyChar.Length);

                for (int i = 0; i < cs.MyCharacter.MyChar.Length; i++)
                {
                    oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].CharType);
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip.Length);
                    for (int j = 0; j < cs.MyCharacter.MyChar[i].Equip.Length; j++)
                    {
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip[j].ItemID);
                        oPacket.WriteInt(1);
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip[j].ItemUID);
                        oPacket.WriteInt(); // 00 00 00 00
                        oPacket.WriteInt(); // 00 00 00 00
                        oPacket.WriteInt(); // 00 00 00 00
                        oPacket.WriteInt(); // 00 00 00 00
                        oPacket.WriteByte(); // 00 00 00
                        oPacket.WriteByte(); //
                        oPacket.WriteByte(); // 
                    }

                    // 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
                    oPacket.Skip(99);
                }

                oPacket.WriteInt(0); // 그냥

                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }

            if (cs.CurrentRoom == null)
                return;

            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_EQUIP_ITEM_BROAD))
            {
                oPacket.WriteInt(cs.Login.Length);
                oPacket.WriteUnicodeString(cs.Login);

                oPacket.WriteByte(2); // ???

                oPacket.WriteInt(cs.MyCharacter.MyChar.Length);
                for (int i = 0; i < cs.MyCharacter.MyChar.Length; i++)
                {
                    oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].CharType);
                    oPacket.WriteInt(0); // 00 00 00 00
                    oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip.Length);
                    for (int j = 0; j < cs.MyCharacter.MyChar[i].Equip.Length; j++)
                    {
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip[j].ItemID);
                        oPacket.WriteInt(1);
                        oPacket.WriteInt(cs.MyCharacter.MyChar[i].Equip[j].ItemUID);
                        oPacket.WriteInt(); // 00 00 00 00
                        oPacket.WriteInt(); // 00 00 00 00
                        oPacket.WriteInt(); // 00 00 00 00
                        oPacket.WriteInt(); // 00 00 00 00
                        oPacket.WriteByte(); // 00 00 00
                        oPacket.WriteByte(); //
                        oPacket.WriteByte(); // 
                    }

                    oPacket.Skip(61);
                    oPacket.WriteHexString("FF FF");
                    oPacket.Skip(32);
                    oPacket.WriteInt(cs.LoginUID);
                }

                for (int i = 0; i < 6; i++)
                {
                    if (cs.CurrentRoom.Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(cs.CurrentRoom.Slot[i].cs.CRYPT_KEY, cs.CurrentRoom.Slot[i].cs.CRYPT_HMAC, cs.CurrentRoom.Slot[i].cs.CRYPT_PREFIX, cs.CurrentRoom.Slot[i].cs.CRYPT_COUNT);
                        cs.CurrentRoom.Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }

        public void OnChangeEquipInRoom(ClientSession cs, InPacket ip)
        {
           
            if (cs.CurrentRoom == null)
                return;

            
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_CHANGE_LOOK_EQUIP_NOT))
            {
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteInt(cs.MyCharacter.MyChar.Length);

                for (int i = 0; i < cs.MyCharacter.MyChar.Length; i++)
                {
                    oPacket.WriteByte((byte)cs.MyCharacter.MyChar[i].CharType);
                    oPacket.WriteInt(0); // 00 00 00 00
                }

                
                for (int i = 0; i < 6; i++)
                {
                    if (cs.CurrentRoom.Slot[i].Active == true)
                    {
                        oPacket.CompressAndAssemble(cs.CurrentRoom.Slot[i].cs.CRYPT_KEY, cs.CurrentRoom.Slot[i].cs.CRYPT_HMAC, cs.CurrentRoom.Slot[i].cs.CRYPT_PREFIX, cs.CurrentRoom.Slot[i].cs.CRYPT_COUNT);
                        cs.CurrentRoom.Slot[i].cs.Send(oPacket); 
                        oPacket.CancelAssemble(); 
                    }
                }
            }
        }

        public void OnTrainSkill(ClientSession cs, InPacket ip)
        {
          
            int SkillID = ip.ReadInt();

            
            for(int i = 0; i < MyChar.Length; i++)
            {
                Array.Resize(ref MyChar[i].MySkill, MyChar[i].MySkill.Length + 1);
                MyChar[i].MySkill[MyChar[i].MySkill.Length - 1].SkillID = SkillID;
            }

            int MyCharPos = -1;
            for (int t = 0; t < cs.MyCharacter.MyChar.Length; t++)
                if (cs.MyCharacter.MyChar[t].CharType == cs.CurrentChar)
                    MyCharPos = t;

            DataSet ds2 = new DataSet();
            Database.Query(ref ds2, "SELECT   `CharID` FROM  `skilltreeid` WHERE `SkillID` = '{0}'", SkillID);
            if (ds2.Tables[0].Rows.Count == 0)
            {
                LogFactory.GetLog("ADD SKILL").LogWarning("ESSA SKILL NAO ESTA NA DB: " + SkillID);
            }
            else
            {
                MyCharPos = Convert.ToInt32(ds2.Tables[0].Rows[0]["CharID"].ToString());
                cs.CurrentChar = MyCharPos;
            }

            LogFactory.GetLog("ADD SKILL").LogSuccess("SKILLID: "+SkillID+" CHARID: "+MyCharPos);

            /*DataSet ds = new DataSet();
            Database.Query(ref ds, "INSERT INTO `skilltreeid` (`SkillID`, `CharID`) VALUES  ('{0}', '{1}')",SkillID,cs.CurrentChar);*/
            DataSet ds = new DataSet();
            Database.Query(ref ds, "INSERT INTO `gc`.`skilltree` (  `loginui`,  `charindex`,  `SkillGroup`,  `SkillID`) VALUES  (    '{0}',    '{1}',    '0',    '{2}'  )", cs.LoginUID, MyCharPos, SkillID);
            /*DataSet ds = new DataSet();
            Database.Query(ref ds, "INSERT INTO `gc`.`skilltree` (  `loginui`,  `charindex`,  `SkillGroup`,  `SkillID`) VALUES  (    '{0}',    '{1}',    '0',    '{2}'  )", cs.LoginUID,MyCharPos,SkillID);*/
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_SKILL_TRAINING_ACK))
            {
                oPacket.WriteInt(0); // 성공 여부인가봄
                oPacket.WriteInt(SkillID);

                oPacket.Assemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }        

        /*public void LoadSkilltree(ClientSession cs)
        {
            DataSet ds3 = new DataSet();
            Database.Query(ref ds3, "SELECT * FROM `skilltree` WHERE `LoginUI` = '{0}'", cs.LoginUID);
            Array.Resize(ref skillinfo, ds3.Tables[0].Rows.Count);
            for (int i = 0; i < skillinfo.Length; i++)
            {
                skillinfo[i].charindex =Convert.ToByte(ds3.Tables[0].Rows[i]["charindex"].ToString());
                LogFactory.GetLog("SKILL").LogInfo("CHARID: " + skillinfo[i].charindex);
                skillinfo[i].ui1 = Convert.ToByte(ds3.Tables[0].Rows[i]["ui1"].ToString());
                LogFactory.GetLog("SKILL").LogInfo("CHARID: " + skillinfo[i].ui1);
                skillinfo[i].ui2 = Convert.ToInt32(ds3.Tables[0].Rows[i]["ui2"].ToString());
                LogFactory.GetLog("SKILL").LogInfo("CHARID: " + skillinfo[i].ui2);
            }
        }*/

        public void OnSetSkill(ClientSession cs, InPacket ip)
        {
            int unknown1 = ip.ReadInt();
            int LoginUID = ip.ReadInt();
            int UnknownCharNum = ip.ReadInt();
            /*
            for(int i = 0; i < UnknownCharNum; i++)
            {                
                int c = ip.ReadByte();
                byte u1= ip.ReadByte(); // ???
                int u2 = ip.ReadInt(); // ???
                
                LogFactory.GetLog("MAIN").LogInfo("    {0} / {1} / {2}", c, u1, u2);
            }
            */
            int CharNum = ip.ReadInt();
            for (int i = 0; i < CharNum; i++)
            {
                int CharType = ip.ReadByte();
                ip.ReadByte(); // ?
                ip.ReadInt(); // ? 00 00 00 02
                ip.ReadInt(); // ? 00 00 00 00

                int SkillCount = ip.ReadInt();

                LogFactory.GetLog("MAIN").LogInfo("    CHARID :{0} / SKILLID: {1}", CharType, SkillCount);

                int CharPos = findCharIndex(CharType); 
               
                if(CharPos != -1)
                {
                    Array.Resize(ref MyChar[CharPos].EquipSkill, SkillCount);

                    
                    for (int j = 0; j < SkillCount; j++)
                    {
                        ip.ReadInt(); // ? 00 00 00 00
                        int SkillGruop = ip.ReadInt(); 
                        int SkillID = ip.ReadInt();

                        MyChar[CharPos].EquipSkill[j].SkillGroup = SkillGruop;
                        MyChar[CharPos].EquipSkill[j].SkillID = SkillID;
                    }
                }

                ip.ReadInt(); // 00 00 00 01
                ip.ReadInt(); // 00 00 00 00
            }

            
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_SET_SKILL_BROAD))
            {
                oPacket.WriteInt(0);

                byte[] data = ip.ToArray();
                data = BytesUtil.ReadBytes(data, 4, data.Length - 4);

                oPacket.WriteBytes(data);
                /*
                oPacket.WriteInt(0);
                oPacket.WriteInt(cs.LoginUID);
                oPacket.WriteHexString("00 00 00 0B 00 00 00 00 00 00 01 00 00 00 00 00 02 00 00 00 00 00 05 00 00 00 00 00 0A FF 00 00 00 00 0A 00 00 00 00 00 0D 00 00 00 00 00 0E 00 00 00 00 00 0F 00 00 00 00 00 11 00 00 00 00 00 12 00 00 00 00 00");
                oPacket.WriteInt(MyChar.Length);
                for (int i = 0; i < MyChar.Length; i++)
                {
                    oPacket.WriteByte((byte)MyChar[i].CharType);
                    oPacket.WriteByte(0);
                    oPacket.WriteInt(2);
                    oPacket.WriteInt(0);

                    oPacket.WriteInt(MyChar[i].skillinfo.Length);
                    for (int j = 0; j < MyChar[i].skillinfo.Length; j++)
                    {
                        oPacket.WriteInt(0);
                        oPacket.WriteInt(MyChar[i].skillinfo[j].SkillGroup);
                        oPacket.WriteInt(MyChar[i].skillinfo[j].SkilliD);
                    }

                    oPacket.WriteInt(1);
                    oPacket.WriteInt(0);
                }
                    /*
                    oPacket.WriteInt(0); // ??
                    oPacket.WriteInt(LoginUID);

                    oPacket.WriteInt(MyChar.Length);
                    for (int i = 0; i < MyChar.Length; i++)
                    {
                        oPacket.WriteByte((byte)i); // 캐릭터 번호
                        oPacket.WriteByte(0); // ???
                        oPacket.WriteInt(); // ???
                    }

                    oPacket.WriteInt(MyChar.Length);
                    for (int i = 0; i < MyChar.Length; i++)
                    {
                        oPacket.WriteByte((byte)MyChar[i].CharType);
                        oPacket.WriteByte(0);
                        oPacket.WriteInt(2);
                        oPacket.WriteInt(0);

                        oPacket.WriteInt(MyChar[i].EquipSkill.Length);
                        for (int j = 0; j < MyChar[i].EquipSkill.Length; j++)
                        {
                            oPacket.WriteInt(0);
                            oPacket.WriteInt(MyChar[i].EquipSkill[j].SkillGroup);
                            oPacket.WriteInt(MyChar[i].EquipSkill[j].SkillID);
                        }

                        oPacket.WriteInt(1);
                        oPacket.WriteInt(0);
                    }
                    */

                    oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, cs.CRYPT_COUNT);
                cs.Send(oPacket);
            }
        }
    }
}
 