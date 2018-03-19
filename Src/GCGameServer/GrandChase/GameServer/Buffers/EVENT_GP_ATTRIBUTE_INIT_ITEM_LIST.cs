using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class GpAttribute
    {
        public void list(User user)
        { 
           PacketManager Write = new PacketManager();
            Write.OP(1448);
            Write.Hex("00 00 00 03 00 00 00 01 00 00 00 01 00 0F 85 B6 00 00 00 02 00 00 00 01 00 0F 85 B6 00 00 00 03 00 00 00 01 00 0F 85 B6 00 00 00");
            user.Send(Write.ack);
        }
    }
}
