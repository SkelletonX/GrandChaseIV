using System;
using GrandChase.Data;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrandChase.IO.Packet;
using GrandChase.Net;
using GrandChase.Net.Client;
using Common;
using GrandChase.IO;
using Manager.Factories;
using Manager;

namespace GrandChase.Function
{
    public class Commands
    {
        public void GMCommands(ClientSession cs,String Chat,InPacket ip)
        {
            if (cs.AuthLevel == 1)
            {
                //Commands!
                if (Chat == "/loginout")
                {
                    using (OutPacket op = new OutPacket(GameOpcodes.EVENT_STAT_LOGINOUT_COUNT))
                    {
                        op.WriteInt(0);
                        op.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                        cs.Send(op);
                    }
                }
                if (Chat == "/Char"+Chat.Substring(5))
                {
                    cs.CurrentChar = Convert.ToInt32(Chat.Substring(5));
                }
                if (Chat == "/addgp"+(string)Chat.Substring(6))
                {
                    DataSet ds = new DataSet();
                    Database.Query(ref ds,"UPDATE   `gc`.`account` SET   `Gamepoint` = '{1}' WHERE `LoginUID` = '{0}'", cs.LoginUID, cs.GamePoint+Convert.ToInt32(Chat.Substring(6)));
                }
                if (Chat == "!!!!!" + Chat.Substring(5))
                {                    
                    using (OutPacket op = new OutPacket(GameOpcodes.EVENT_SIGN_BOARD_NOT))
                    {
                        //LogFactory.GetLog("SIGNBOARD").LogInfo("TEXT: " + Chat.Substring(5));
                        op.WriteHexString("00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
                        op.WriteInt((cs.Nick+" : "+Chat.Substring(5)).Length *2);
                        op.WriteUnicodeString((cs.Nick + " : " + Chat.Substring(5)));
                        foreach (ClientSession u in cs.CurrentChannel.UsersList)
                        {
                            if (u.CurrentRoom == cs.CurrentRoom)
                            {
                                op.Assemble(u.CRYPT_KEY, u.CRYPT_HMAC, u.CRYPT_PREFIX, u.CRYPT_COUNT);
                                u.Send(op);
                                op.CancelAssemble();
                            }
                        }
                    }
                }


            }

        }

        public void DigitOnChat(ClientSession cs,string msg)
        {
            using (OutPacket op = new OutPacket(GameOpcodes.EVENT_CHAT_NOT))
            {
                op.WriteByte(1);
                op.WriteInt(cs.LoginUID);
                op.WriteInt(cs.Nick.Length * 2);
                op.WriteUnicodeString(cs.Nick);
                op.WriteInt(0);
                op.WriteInt(0);
                op.WriteInt(-1);
                op.WriteInt(msg.Length);
                op.WriteUnicodeString(msg);
                op.WriteInt(0);
                op.WriteInt(0);
                op.CompressAndAssemble(cs.CRYPT_KEY, cs.CRYPT_HMAC, cs.CRYPT_PREFIX, ++cs.CRYPT_COUNT);
                cs.Send(op);
            }
        }
        
    }
}
