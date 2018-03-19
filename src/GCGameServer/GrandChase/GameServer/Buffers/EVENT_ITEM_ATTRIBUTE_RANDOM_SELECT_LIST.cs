using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class itemattrandom
    {
        public void list(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(955);
            Write.Hex("00 00 00 02 00 00 00 02 00 00 00 01 00 06 F9 64 00 00 00 03 00 00 00 01 00 06 F9 64 00 00 00");
            user.Send(Write.ack);
        }
    }
}
