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

namespace GrandChase.Function
{
    public class MsgUsers
    {
        public struct ListFriends
        { 
            public string FriendName;
            public int FriendID;
        }

        public ListFriends[] listfriends = new ListFriends[0];

/*        public void LoadLoginUID(ClientSession msg,string RLogin)
        {
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT  * FROM  `gc`.`account` WHERE Login = '{0}'",RLogin);

            msg.LoginUID = Convert.ToInt32(ds.Tables[0].Rows[0]["LoginUID"].ToString());
        }*/

        public void LoadList(ClientSession msg)
        {            
            DataSet ds = new DataSet();
            Database.Query(ref ds, "SELECT  FriendName,FriendID FROM  `msg`.`friends` WHERE LoginUID = '{0}'", msg.LoginUID);
            Array.Resize(ref listfriends, ds.Tables[0].Rows.Count);
            for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
            {
                listfriends[a].FriendName = Convert.ToString(ds.Tables[0].Rows[a]["FriendName"].ToString());
                listfriends[a].FriendID = Convert.ToInt32(ds.Tables[0].Rows[a]["FriendID"].ToString());             
            }
        }

        public void FriendList(ClientSession msg,InPacket ip)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_INVITE_NOT))
            {                
                msg.LoginUID = ip.ReadInt();
                int LenLogin = ip.ReadInt();
                msg.Login = ip.ReadUnicodeString(LenLogin);

                LogFactory.GetLog("MSG INFOS").LogInfo("LoginUID: "+msg.LoginUID);
                LogFactory.GetLog("MSG INFOS").LogInfo("Length Lgn: " + LenLogin);
                LogFactory.GetLog("MSG INFOS").LogInfo("Login: " + msg.Login);

                LoadList(msg);              
                oPacket.WriteHexString("00 00 00 00 00 00 00 01 00 00 00 0A 00 00 00 0A 00 00 00 0C 46 00 72 00 69 00 65 00 6E 00 64 00");
                oPacket.WriteInt(listfriends.Length);
                LogFactory.GetLog("FriendsList").LogInfo("List Length: " + listfriends.Length);
                for (int z = 0; z < listfriends.Length; z++)
                {
                    LogFactory.GetLog("FriendsList").LogInfo("FriendName: " + listfriends[z].FriendName);
                    oPacket.WriteInt(listfriends[z].FriendID);
                    oPacket.WriteInt(listfriends[z].FriendID);
                    oPacket.WriteInt(listfriends[z].FriendName.Length *2);
                    oPacket.WriteUnicodeString(listfriends[z].FriendName);
                    oPacket.WriteHexString("00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 FF FF FF FF");//00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 FF FF FF FF
                }
                oPacket.WriteHexString("00 00 00 01 00 00 00 18 42 00 6C 00 6F 00 63 00 6B 00 20 00 46 00 72 00 69 00 65 00 6E 00 64 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 01");
                oPacket.Assemble(msg.CRYPT_KEY, msg.CRYPT_HMAC, msg.CRYPT_PREFIX, ++msg.CRYPT_COUNT);
                msg.Send(oPacket);
            }
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_CHANGE_ROOMUSER_INFO_REQ))
            {
                oPacket.WriteHexString("00 00 00 01 00 00 00 0A 00 00 00 00");
                oPacket.CompressAndAssemble(msg.CRYPT_KEY, msg.CRYPT_HMAC, msg.CRYPT_PREFIX, ++msg.CRYPT_COUNT);
                msg.Send(oPacket);
            }
        }

        public void Add(ClientSession msg, InPacket ip)
        {
            using (OutPacket oPacket = new OutPacket(GameOpcodes.EVENT_LEAVE_ROOM_BROAD))
            {
                oPacket.WriteHexString("00 00 00 64 00 00 00 22 4C 00 65 00 74 00 27 00 73 00 20 00 62 00 65 00 20 00 66 00 72 00 69 00 65 00 6E 00 64 00 73 00 7E 00 00 01 60 D9 00 00 00 08 74 00 65 00 73 00 74 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 06 6F 00 66 00 66 00 00 00 00 01 00 FF FF FF FF");
                oPacket.CompressAndAssemble(msg.CRYPT_KEY, msg.CRYPT_HMAC, msg.CRYPT_PREFIX, ++msg.CRYPT_COUNT);
                msg.Send(oPacket);
            }
        }

    }
}
