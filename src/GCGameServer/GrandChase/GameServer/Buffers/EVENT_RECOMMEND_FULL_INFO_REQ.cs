using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class Recommended
    {
        public void full(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(568);
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 56 7C F7 80 56 7D 72 66 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
