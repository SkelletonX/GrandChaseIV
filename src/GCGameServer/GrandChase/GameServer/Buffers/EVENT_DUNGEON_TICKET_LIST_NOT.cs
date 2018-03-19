using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class ticketlist
    {
        public void sendlist(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1249);
            Write.Hex("00 00 00 07 00 00 00 43 00 00 00 43 00 00 00 01 00 0A 1E 3C 00 00 00 01 00 00 00 01 00 0A 1E 46 00 00 00 01 00 00 00 44 00 00 00 44 00 00 00 01 00 0B 62 4C 00 00 00 01 00 00 00 00 00 00 00 45 00 00 00 45 00 00 00 01 00 0A 1E 3C 00 00 00 01 00 00 00 01 00 0A 1E 46 00 00 00 01 00 00 00 47 00 00 00 47 00 00 00 01 00 0C 52 D8 00 00 00 01 00 00 00 01 00 0C 55 1C 00 00 00 01 00 00 00 48 00 00 00 48 00 00 00 01 00 0D 72 A8 00 00 00 01 00 00 00 00 00 00 00 4B 00 00 00 4B 00 00 00 01 00 0F 89 E4 00 00 00 01 00 00 00 00 00 00 00 53 00 00 00 53 00 00 00 01 00 11 A4 9A 00 00 00 01 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
