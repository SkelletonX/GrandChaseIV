using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class StageLoadComplete
    {
        public void stageLoadComplete(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(928);
            Write.Int(0);
            user.Send(Write.ack);
        }
    }
}
