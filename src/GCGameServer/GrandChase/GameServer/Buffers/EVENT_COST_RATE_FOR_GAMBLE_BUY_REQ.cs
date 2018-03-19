using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class GamebleBuy
    {
        public void cost_rate(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(872);
            Write.Int(1073741824);
            Write.Hex("00 00 00");
            user.Send(Write.ack);
        }
    }
}
