using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class LoadComplete
    {
        public void LoadCompleteNot(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(841);
            Write.Int(0);
            user.Send(Write.ack);
        }
    }
}
