using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class weeklyreward
    {
        public void list(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(478);
            Write.Int(0);
            user.Send(Write.ack);
        }
    }
}
