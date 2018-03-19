using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class exppotion
    {
        public void list(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1339);
            Write.Hex("00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
