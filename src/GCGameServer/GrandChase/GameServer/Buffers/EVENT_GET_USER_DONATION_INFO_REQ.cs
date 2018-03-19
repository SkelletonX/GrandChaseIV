using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class donation
    {
        public void info(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(523);
            Write.Hex("00 00 00 01 00 00 00 00 00 00 00 00 00 02 1F DE 00 00 00 01");
            user.Send(Write.ack);
        }
    }
}
