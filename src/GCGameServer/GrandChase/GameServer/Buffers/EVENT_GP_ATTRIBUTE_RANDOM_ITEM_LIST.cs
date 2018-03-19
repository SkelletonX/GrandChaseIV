using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class GpAttributeRandom
    {
        public void list(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1449);
            Write.Hex("00 00 00 03 00 00 00 01 00 00 00 01 00 0F 85 AC 00 00 00 02 00 00 00 01 00 0F 85 AC 00 00 00 03 00 00 00 01 00 0F 85 AC 00 00 00");
            user.Send(Write.ack);
        }
    }
}
