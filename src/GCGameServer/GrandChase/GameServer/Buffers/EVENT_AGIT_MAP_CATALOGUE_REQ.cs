using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class AgitMap
    {
        public void map(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1107);
            Write.Hex("00 09 3C 56 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 3C 00 02 00 02 00 00 00 0F 4D 79 46 69 72 73 74 41 7A 69 74 2E 6C 75 61 00 00 00 01 00 00 00 01 00 00 03 84 00 02 00 02 00 00 00 12 41 7A 69 74 32 30 31 5F 39 30 62 79 36 30 2E 6C 75 61 00 00 00 02 00 00 00 02 00 00 0A F0 00 03 00 03 00 00 00 12 41 7A 69 74 33 30 31 5F 39 30 62 79 36 30 2E 6C 75 61 00 00 00 03 00 00 00 03 00 00 0C E4 00 03 00 03 00 00 00 12 41 7A 69 74 33 30 32 5F 39 30 62 79 36 30 2E 6C 75 61 00 00 00");
            user.Send(Write.ack);
        }
    }
}
