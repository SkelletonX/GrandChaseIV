using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class SystemGuideInfo
    {
        public void systemguideinfo(User user)
        { 
            PacketManager Write = new PacketManager();
            Write.OP(1583);
            Write.Hex("00 00 00 00 00 00 00 01 00 00 00 0F 00 00 00 00 00 00 00 01 00 00 00");
            user.Send(Write.ack);
        }
    }
}
