using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class Depot
    {
        public void Info(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1341);
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 05 00 0C 3A FA 00 0C 3A F0 00");
            user.Send(Write.ack);
        }
    }
}
