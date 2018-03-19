using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class FullLookInfo
    {
        public void fulllookinfo(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(864);
            Write.Hex("00 00 00 02 11 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 12 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 05 7E 2C 00 00 01 90");
            user.Send(Write.ack);
        }
    }
}
