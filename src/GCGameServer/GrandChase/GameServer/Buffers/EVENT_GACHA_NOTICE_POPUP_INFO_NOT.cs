using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class gachanotice
    {
        public void popup(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(458);
            Write.Int(0);
            user.Send(Write.ack);
        }
    }
}
