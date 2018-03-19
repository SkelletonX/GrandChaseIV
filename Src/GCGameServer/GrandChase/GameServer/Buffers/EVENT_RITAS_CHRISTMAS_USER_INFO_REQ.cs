using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class RitasChristimasUserInfo
    {
        public void ritaschristimasuserInfo(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1504);
            Write.Hex("00 00 00 00 00 00 00 05 58 84 EF D8 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
