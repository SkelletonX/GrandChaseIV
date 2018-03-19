using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class baduserinfo
    {
        public void userInfo(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(362);
            Write.Hex("00 00 00 59 24 CE 28 00 00 00");
            user.Send(Write.ack);
        }
    }
}
