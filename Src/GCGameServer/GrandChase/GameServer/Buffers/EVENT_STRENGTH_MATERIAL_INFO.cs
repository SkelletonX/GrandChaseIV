using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class stregthMaterial
    {
        public void materialinfo(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1085);
            Write.Hex("00 00 00 02 00 00 00 02 00 00 00 14 00 00 00 32 00 00 00 06 00 00 00 00 00 00 00 01 00 00 00 02 00 09 92 5A 00 09 92 64 00 00 00 00 00 00 00 02 00 00 00 03 00 09 92 6E 00 0B A3 9C 00 0C 33 98 00 00 00 00 00 00 00 03 00 00 00 0B 00 09 92 A0 00 09 92 AA 00 09 92 B4 00 09 92 BE 00 09 EC 46 00 09 EC 50 00 09 EC 5A 00 09 EC 64 00 0B 10 94 00 0C 32 B2 00 0C 3B 04 00 00 00 00 00 00 00 05 00 00 00 01 00 0C 32 76 00 00 00 01 00 00 00 01 00 00 00 03 00 09 92 78 00 09 92 82 00 0B A3 A6 00 00 00 02 00 00 00 01 00 00 00 02 00 09 92 8C 00 09 92 96 00 00 00 08 00 09 CE 82 00 09 EC AA 00 09 EC B4 00 09 EC BE 00 09 EC C8 00 0C 33 A2 00 0C 33 AC 00 0C 33 B6 00 00 00 11 00 00 00 07 00 0B 3B F0 00 0B 3B FA 00 0B 3C 04 00 0B 3C 0E 00 0B 3C 18 00 0B 3C 22 00 0B 3C 2C 00 00 00 04 00 09 92 6E 00 00 00 00 00 00 00 0F 00 0B A3 9C 00 00 00 00 00 00 00 0F 00 0C 32 76 00 00 00 00 00 00 00 11 00 0C 33 98 00 00 00 00 00 00 00 11 00");
            user.Send(Write.ack);
        }
    }
}
