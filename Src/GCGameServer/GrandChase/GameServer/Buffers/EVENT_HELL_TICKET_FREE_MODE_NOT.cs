using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class hellticket
    {
        public void freemode(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(600);
            Write.Hex("00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
