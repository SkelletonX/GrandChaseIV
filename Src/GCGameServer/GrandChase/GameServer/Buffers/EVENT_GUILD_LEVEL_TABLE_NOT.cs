using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class GuildLevel
    {
        public void level(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1007);
            Write.Hex("00 00 00 0B 00 00 00 00 00 00 00 01 00 00 03 E8 00 02 00 00 0F A0 00 03 00 00 27 10 00 04 00 00 59 D8 00 05 00 00 A4 10 00 06 00 01 09 A0 00 07 00 01 AD B0 00 08 00 02 71 00 00 09 00 03 D0 90 00 0A 00 06 41 90 00 00 00");
            user.Send(Write.ack);
        }
    }
}
