using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GameServer.Buffers
{
    public class CharsInfo
    {
        public struct cEquipamentos
        {
            public int itemid, itemuid;
        }
        public struct cChars
        { 
            public int personagemid,classe,experiencia,nivel,mascote,vitoria,derrota,splef;
            public cEquipamentos[] equipamentos;
        }

        public cChars[] personagems = new cChars[0];

        public void SetLength(int newLength)
        {
            Array.Resize(ref personagems, newLength);
        }

        public void SetLengthEquip(int newLength,int gg)
        {
            Array.Resize(ref personagems[gg].equipamentos, newLength);
        }

        public int getLengthEquip(int gg)
        {
            return personagems[gg].equipamentos.Length;
        }

        public int getLength()
        {
            return personagems.Length;
        }        

        public void GetCharactersFromDB(int userid)
        {
            GameServer.db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();
            DataSet Banco0 = new DataSet();
            DataSet Banco1 = new DataSet();
            data.Exec(Banco, "SELECT   `personagemid`,  `classe`,  `experiencia`,  `nivel`,  `mascote`,  `vitoria`,  `derrota` FROM  `personagems` WHERE `userid` ='"+userid+"'");

            SetLength(Banco.Tables[0].Rows.Count);

            for (Int32 gg = 0; gg < getLength(); gg++)
            {
                personagems[gg].personagemid = Ultilize.StrToInt(Banco.Tables[0].Rows[gg][0].ToString());
                personagems[gg].classe = Ultilize.StrToInt(Banco.Tables[0].Rows[gg][1].ToString());
                personagems[gg].experiencia = Ultilize.StrToInt(Banco.Tables[0].Rows[gg][2].ToString());
                personagems[gg].nivel = Ultilize.StrToInt(Banco.Tables[0].Rows[gg][3].ToString());
                personagems[gg].mascote= Ultilize.StrToInt(Banco.Tables[0].Rows[gg][4].ToString());
                personagems[gg].vitoria = Ultilize.StrToInt(Banco.Tables[0].Rows[gg][5].ToString());
                personagems[gg].derrota = Ultilize.StrToInt(Banco.Tables[0].Rows[gg][6].ToString());

                data.Exec(Banco1, "SELECT   `SP` FROM  `spleft` WHERE `userid` ='" + userid + "' AND `personagemid`='"+personagems[gg].personagemid+"'");
                if (Banco1.Tables[0].Rows.Count > 0)
                {
                    personagems[gg].splef = Ultilize.StrToInt(Banco1.Tables[0].Rows[0][0].ToString());
                }
                else
                {
                    personagems[gg].splef = 0;
                }

                data.Exec(Banco0, "SELECT   `itemid`,  `itemuid` FROM  `equipamentos` WHERE `personagemid` = '" + personagems[gg].personagemid + "' AND `userid` = '"+userid+"'");
                SetLengthEquip(Banco0.Tables[0].Rows.Count,gg);
                for (Int32 gg2 = 0; gg2 < getLengthEquip(gg); gg2++)
                {
                    personagems[gg].equipamentos[gg2].itemid = Ultilize.StrToInt(Banco0.Tables[0].Rows[0][0].ToString());
                    personagems[gg].equipamentos[gg2].itemuid = Ultilize.StrToInt(Banco0.Tables[0].Rows[0][1].ToString());
                }
            }
        }
    }
}
