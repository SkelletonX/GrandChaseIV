using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;

namespace CenterServer.Packets
{
    class NoticiasDoCanal
    {

        public void Noticias(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(9);
            Write.Header();
            Write.Hex("00 00 00 00 00 00 00 00 01 00 00");
            user.Send(Write.ack);
        }

    }
}
