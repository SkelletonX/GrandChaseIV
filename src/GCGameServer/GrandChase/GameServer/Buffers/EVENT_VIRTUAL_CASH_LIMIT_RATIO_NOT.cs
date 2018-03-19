using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class cashlimit
    {
        public void virtualcashlimit(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(610);
            Write.Hex("00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
