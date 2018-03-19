using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class jumpingchar
    {
        public void jumpingcharinfo(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1600);
            Write.Hex("00 00 00 00 46 00 00 00 13 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 11 12");
            user.Send(Write.ack);
        }
    }
}
