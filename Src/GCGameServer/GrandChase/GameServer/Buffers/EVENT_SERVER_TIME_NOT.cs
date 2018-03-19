using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    public class ServerTime
    {
        public void servertime(User user)
        {
            DateTime getdata = DateTime.Now;
            PacketManager Write = new PacketManager();
            Write.OP(406);
            Write.Int(0);
            Write.Int(getdata.Year);//Ano
            Write.Int(getdata.Month);//Mês
            Write.Int(getdata.Day);//Dia
            Write.Int(getdata.Hour);//Hora
            Write.Int(getdata.Minute);//Minuto
            Write.Int(getdata.Second);//Segundo
            user.Send(Write.ack);
        }
    }
}
