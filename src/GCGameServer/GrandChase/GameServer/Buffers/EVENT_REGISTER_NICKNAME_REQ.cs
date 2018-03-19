using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;
using System.Data;

namespace GameServer.Buffers
{
    class RegisterNick
    {
        public void Nick(User user,Readers ler,int userid)
        {
            string nickname = ler.UString();
            
            db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();
            DataSet Banco1 = new DataSet();

            Console.WriteLine("NICK: "+nickname);
            PacketManager Write = new PacketManager();
            Write.OP(135);
            data.Exec(Banco, "SELECT   `userid`  FROM `nicknames` WHERE `nickname` = '" + nickname+ "'");
            if (Banco.Tables[0].Rows.Count > 0)
            {
                Write.Int(1);
            }
            else
            {
                Write.Int(0);
                data.Exec(Banco, "UPDATE   `nicknames` SET  `nickname` = 'nickname' WHERE `userid` = '"+userid+"'");
            }            
            Write.UStr(nickname);
            user.Send(Write.ack);
        }
    }
}
