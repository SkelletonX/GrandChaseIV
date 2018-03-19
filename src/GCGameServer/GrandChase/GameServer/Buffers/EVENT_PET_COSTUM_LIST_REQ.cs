using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class PetCustom
    {
        public void list(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(518);
            Write.Hex("00 00 00 0C 00 00 97 9A 00 00 00 00 01 00 01 9A 96 00 00 97 A4 00 00 00 00 01 00 02 18 90 00 00 97 AE 00 00 00 00 01 00 02 18 86 00 00 97 B8 00 00 00 00 01 00 01 9A A0 00 00 A5 6E 00 00 00 00 01 00 01 A5 CC 00 01 34 D4 00 00 00 00 01 00 01 B9 04 00 01 77 F0 00 00 00 00 01 00 01 9A AA 00 02 FC F6 00 00 00 00 03 00 0E 20 EA 00 11 12 50 00 11 12 5A 00 05 BB 44 00 00 00 00 01 00 05 BB A8 00 09 1A E6 00 00 00 00 01 00 09 1B 04 00 0B 7F 52 00 00 00 00 01 00 10 9E 56 00 0D DF 18 00 00 00 00 01 00 10 06 BC 00 00 00");
            user.Send(Write.ack);
        }
    }
}
