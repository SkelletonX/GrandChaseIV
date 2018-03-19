using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class Nasty
    {
        public void getNasty(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(289);
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00 03 00 00 00");
            user.Send(Write.ack);
        }
    }
}
