using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GameServer.Conexao
{
    public class PlayerInfo
    {
        public int userid,online,ban,moderador,gamePoint,bonusvida,tamanhoinventario;
        public string usuario,senha,nickname;

        public void GetNickname(int userid)
        {
            db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();
            data.Exec(Banco, "SELECT   `nickname`  FROM `nicknames` WHERE `userid` = '" + userid + "'");
            if (Banco.Tables[0].Rows.Count > 0)
            {
                nickname = Banco.Tables[0].Rows[0][0].ToString();
            }
        }

        public void GetGP(int userid)
        {
            db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();
            data.Exec(Banco, "SELECT   `GP`  FROM `GamePoints` WHERE `userid` = '" + userid + "'");
            if (Banco.Tables[0].Rows.Count > 0)
            {
                gamePoint = Ultilize.StrToInt(Banco.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                gamePoint = 0;
            }
        }

        public void GetVidaBonus(int userid)
        {
            db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();
            data.Exec(Banco, "SELECT   `quantidade`  FROM `vidabonus` WHERE `userid` = '" + userid + "'");
            if (Banco.Tables[0].Rows.Count > 0)
            {
                bonusvida = Ultilize.StrToInt(Banco.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                bonusvida = 500;
            }
        }

        public void GetSizeInvetario(int userid)
        {
            db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();
            data.Exec(Banco, "SELECT   `tamanhodoinventario`  FROM `contas` WHERE `userid` = '" + userid + "'");
            if (Banco.Tables[0].Rows.Count == 0)
            {
                tamanhoinventario = Ultilize.StrToInt(Banco.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                tamanhoinventario = 500;
            }
        }

    }
}
