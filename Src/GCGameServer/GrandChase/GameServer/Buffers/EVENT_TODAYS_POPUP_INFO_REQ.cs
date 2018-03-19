using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class todays
    {
        public void popup(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(983);
            Write.Hex("00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
