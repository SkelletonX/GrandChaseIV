using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class RKTornado
    {
        public void list(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1045);
            Write.Hex("00 00 00 00 00 00 00 00 00 5D 01 37 46 00 00 00");
            user.Send(Write.ack);
        }
    }
}