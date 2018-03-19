using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class Rainbow
    {
        public void rainbow(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(597);
            Write.Hex("00 00 00 00 01 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
