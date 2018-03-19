using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;
using System.Data;
using CenterServer.Conexao;

namespace CenterServer.Packets
{
    class Player
    {
        ListaDeServidores ENU_SERVER_LIST_NOT = new ListaDeServidores();
        NoticiasDoCanal ENU_CHANNEL_NEWS_NOT = new NoticiasDoCanal();
        Client_Contents ENU_NEW_CLIENT_CONTENTS_OPEN_NOT = new Client_Contents();
        SocketTableinf ENU_SOCKET_TABLE_INFO_NOT = new SocketTableinf();
        CashBackRatio ENU_CASHBACK_RATIO_INFO_NOT = new CashBackRatio();
        Log log = new Log();
        PlayerInfo pInfo = new PlayerInfo();

        static string MarksURL = "http://192.95.4.5/GuildMarks/";

        public void logar(User user, Readers Ler)
        {
            db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();

            string usuario = Ler.String();
            string senha = Ler.String();

            pInfo.usuario = usuario;
            pInfo.senha = senha;
            
            PacketManager Write = new PacketManager();
            Write.OP(3);
            Write.Header();
            data.Exec(Banco, "SELECT   `userid`,  `online`,  `ban`,  `moderador`  FROM `contas` WHERE `usuario` = '" + usuario + "' AND `senha` = '" + senha + "'");           


            if (Banco.Tables[0].Rows.Count > 0)
            {
                ENU_SERVER_LIST_NOT.serverlistload(user);//Send ServerList
                ENU_CHANNEL_NEWS_NOT.Noticias(user);//Send Channel News
                ENU_NEW_CLIENT_CONTENTS_OPEN_NOT.ClientContents(user);//Send Client Contents
                ENU_SOCKET_TABLE_INFO_NOT.SocketTable(user);//Send SocketTable
                ENU_CASHBACK_RATIO_INFO_NOT.CashBack(user);//Send CashBack

                pInfo.userid = Convert.ToInt32(Banco.Tables[0].Rows[0][0].ToString());
                pInfo.online = Convert.ToInt32(Banco.Tables[0].Rows[0][1].ToString());
                pInfo.ban = Convert.ToInt32(Banco.Tables[0].Rows[0][2].ToString());
                pInfo.moderador = Convert.ToInt32(Banco.Tables[0].Rows[0][3].ToString());
                
                Write.Int(0);
                Write.UStr(usuario);
                Write.Str(senha);
                Write.Byte(0);
                Write.Hex("00 00 00 14 00 8E A7 C5 01 00 00 00 00 00 00 02 4B 52 00 05 A3 BD 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00");
                Write.UStr(MarksURL);
                Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 03 00 00 00 00 00 00 00 00 64 01 00 00 00 00 00 00 00 64 02 00 00 00 00 00 00 00 64 01 BF 80 00 00 FC 04 97 FF 00 00 00 00 00 00 00 00 00 00 00 00 00");
                user.Send(Write.ack);
            }
            else
            {
                Write.Int(20);
                Write.UStr(usuario);
                Write.Int(0);
                user.Send(Write.ack);
            }
        }
    }
}
