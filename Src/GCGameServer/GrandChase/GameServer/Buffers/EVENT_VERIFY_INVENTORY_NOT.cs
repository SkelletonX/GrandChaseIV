using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GameServer.network;

namespace GameServer.Buffers
{
    class VerificarInventario
    {
        public struct Inventario
        {
            public int itemuid,itemtype,itemid,quantidade;
        }

        private Inventario[] lenInvetario = new Inventario[0];

        //alterar o tamanho
        public void SetLength(int newLength)
        {
            Array.Resize(ref lenInvetario, newLength);
        }

        //obter o tamanho
        public int getLength()
        {
            return lenInvetario.Length;
        }

        private void GetInventoryFromDB(int userid)
        {
            GameServer.db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();
            data.Exec(Banco, "SELECT   `itemuid`,  `itemid`,  `itemtipo`,  `quantidade`  FROM `inventario` WHERE `userid` = '" + userid + "'");

            SetLength(Banco.Tables[0].Rows.Count);

            for (Int32 a = 0; a < lenInvetario.Length; a++)
            {
                SetLength(Banco.Tables[0].Rows.Count + 1);
                lenInvetario[a].itemuid = Ultilize.StrToInt(Banco.Tables[0].Rows[a][0].ToString());
                lenInvetario[a].itemid = Ultilize.StrToInt(Banco.Tables[0].Rows[a][1].ToString());
                lenInvetario[a].itemtype = Ultilize.StrToInt(Banco.Tables[0].Rows[a][2].ToString());
                lenInvetario[a].quantidade = Ultilize.StrToInt(Banco.Tables[0].Rows[a][3].ToString());
            }
        }

        public void GetInventory(User user,int userid)
        {
            GetInventoryFromDB(userid);
            PacketManager Write = new PacketManager();
            Write.OP(224);
            Write.Int(1);
            Write.Int(1);
            Write.Int(getLength());
            for (Int32 a = 0; a < getLength(); a++)
            {
                Write.Int(lenInvetario[a].itemid);
                Write.Int(1);
                Write.Int(lenInvetario[a].itemuid);
                Write.Int(lenInvetario[a].quantidade);
                Write.Int(-1);
                Write.Int(0);
                Write.Short(0);
                Write.Int(-1);
                Write.Int(0);
                Write.Int(1450577322);
                Write.Int(0);
                Write.Int(0);
                Write.Int(0);
                Write.Int(0);
                Write.Int(0);
                Write.Int(0);
                Write.Int(0);
                Write.Int(0);
                Write.Byte(0);
            }
            user.Send(Write.ack);
        }
    }
}
