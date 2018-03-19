using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CenterServer.network;

namespace CenterServer.Packets
{
    public class ListaDeServidores
    {
        public static int MaxLevel;
        struct Lista
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
        Lista[] cLista = new Lista[0];

        private void GetList()
        {
            db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();
            data.Exec(Banco, "SELECT * FROM `servidores`");
            Array.Resize(ref cLista, Banco.Tables[0].Rows.Count);
            for (int a = 0; a < Banco.Tables[0].Rows.Count; a++)
            {
                cLista[a].ServerName = Convert.ToString(Banco.Tables[0].Rows[a]["name"].ToString());
                cLista[a].ServerDesc = Convert.ToString(Banco.Tables[0].Rows[a]["descricao"].ToString());
                cLista[a].ServerIP = Convert.ToString(Banco.Tables[0].Rows[a]["IP"].ToString());
                cLista[a].ServerPort = Convert.ToInt32(Banco.Tables[0].Rows[a]["PORTA"].ToString());
                cLista[a].Users = Convert.ToInt32(Banco.Tables[0].Rows[a]["usuariosOnline"].ToString());
                cLista[a].MaxUsers = Convert.ToInt32(Banco.Tables[0].Rows[a]["MaximoDePlayers"].ToString());
                cLista[a].Flag = Convert.ToInt32(Banco.Tables[0].Rows[a]["Flag"].ToString());
                cLista[a].ServerType = Convert.ToInt32(Banco.Tables[0].Rows[a]["Tipo"].ToString());
            }
        }

        public void serverlistload(User user)
        {
            GetList();
            PacketManager Write = new PacketManager();
            Write.OP(4);
            Write.Header();
            Write.Int(cLista.Length);
            for (int a = 0; a < cLista.Length; a++)
            {
                Write.Int(a + 1);
                Write.Int(a + 1);
                Write.UStr(cLista[a].ServerName);
                Write.Str(cLista[a].ServerIP);
                Write.Short((short)cLista[a].ServerPort);
                Write.Int(cLista[a].Users);
                Write.Int(cLista[a].MaxUsers);
                Write.Int(cLista[a].Flag);
                Write.Int(a + 1);
                Write.Int(a + 1);
                Write.Str(cLista[a].ServerIP);
                Write.UStr(cLista[a].ServerDesc);
                Write.Int(cLista[a].ServerType);                                                
            }
            user.Send(Write.ack);
        }


    }
}
