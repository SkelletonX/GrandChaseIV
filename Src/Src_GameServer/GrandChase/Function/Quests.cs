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
    public class Quests
    {
        public struct quests
        {
            public int MissionID;
            public int MissionUID;
            public int Progress;
        }
        public quests[] missions= new quests[0];

        public void LoadQuests(ClientSession cs)
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT * FROM `gc`.`missions` WHERE `LoginUID`='{0}'",cs.LoginUID);

            Array.Resize(ref missions, ds.Tables[0].Rows.Count);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                missions[i].MissionID= Convert.ToInt32(ds.Tables[0].Rows[i]["MissionID"].ToString());
                missions[i].MissionUID = Convert.ToInt32(ds.Tables[0].Rows[i]["MissionUID"].ToString());
                missions[i].Progress= Convert.ToInt32(ds.Tables[0].Rows[i]["Progress"].ToString());
            }
        }

        public void RemoveMission(ClientSession cs,InPacket ip)
        {
            int missionUID = ip.ReadInt();
            int missionID = ip.ReadInt();
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_REMOVE_MISSION_ACK))
            {
                oPacket.WriteInt(0);
                oPacket.WriteInt(missionID);
                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
                DataSet ds = new DataSet();
                Database.Query(ref ds, "DELETE FROM  `gc`.`missions` WHERE `LoginUID` = '{0}'   AND `MissionID` = '{1}'", cs.LoginUID,missionID);
            }
        }

        public void RegisterMission(ClientSession cs, InPacket ip)
        {
            int missionID = ip.ReadInt();
            int progress = ip.ReadInt();
            int missionUID = ip.ReadInt();
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_REGIST_MISSION_ACK))
            {
                oPacket.WriteInt(0);
                oPacket.WriteInt(missionID);
                oPacket.WriteInt(progress);
                oPacket.WriteInt(missionUID);
                oPacket.WriteInt(missionID);
                oPacket.WriteHexString("00 00 00 00 59 21 9E A1 59 20 4D 21 00 00 00 00");
                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
                DataSet ds = new DataSet();
                Database.Query(ref ds, "INSERT INTO `gc`.`missions` (  `LoginUID`,  `MissionID`,  `MissionUID`,  `Progress`) VALUES  (    '{0}',    '{1}',    '{2}',    '{3}'  )", cs.LoginUID, missionID, missionUID,progress);

                DataSet ds2 = new DataSet();
                Database.Query(ref ds2, "DELETE FROM  `gc`.`inventory` WHERE `LoginUID` = '{0}'   AND `ItemID` = '{1}'", cs.LoginUID, missionID);
            }
        }

        public void CompleteMission(ClientSession cs, InPacket ip)
        {
            int missionID = ip.ReadInt();
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_COMPLETE_MISSION_ACK))
            {
                oPacket.WriteInt(0);
                oPacket.WriteInt(missionID);
                oPacket.WriteHexString("00 00 01 F8 00 00 00 11 00 00 00 01 00 00 22 B0 00 00 00 01 30 B2 5C A2 00 00 00 09 00 00 00 09 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 57 AD 34 74 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 0D 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 07 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 13 00 00 00 00 00 00 00 00 00 00 00 00 00 00 14 00 00 00 00 00 00 00 00 00 00 00 00 00 00 15 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 00 00 00 00 00 00 00 00 00 00 00 00 00 00 17 00 00 00 00 00 00 00 00 00 00 00 00 00 00 18 00 00 00 00 00 00 00 00 00 00 00 00 00 00 19 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 24 00 00 00 00 00 00 00 00 00 00 00 00 00 00 27 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 28 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 29 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 2A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 30 00 00 00 00 00 00 00 00 00 00 00 00 00 00 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 33 00 00 00 00 00 00 00 00 00 00 00 00 00 00 34 00 00 00 00 00 00 00 00 00 00 00 00 00 00 35 00 00 00 00 00 00 00 00 00 00 00 00 00 00 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 38 00 00 00 00 00 00 00 00 00 00 00 00 00 00 39 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 00 00 00 00 00 00 00 00 00 00 00 00 00 00 46 00 00 00 00 00 00 00 00 00 00 00 00 00 00 47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 00 00 00 00 00 00 00 00 00 00 00 00 00 00 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 53 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 55 00 00 00 00 00 00 00 00 00 00 00 00 00 00 56 00 00 00 00 00 00 00 00 00 00 00 00 00 00 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 00 00 00 00 00 00 00 00 00 59 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5A 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 14 00 00 00 A0");
                oPacket.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(oPacket);
                DataSet ds = new DataSet();
                Database.Query(ref ds, "DELETE FROM  `gc`.`missions` WHERE `LoginUID` = '{0}'   AND `MissionID` = '{1}'", cs.LoginUID, missionID);
            }
        }

    }    
}
