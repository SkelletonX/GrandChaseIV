using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class PetVested
    {
        public void petvesteditem(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1550);
            Write.Hex("00 00 00 08 00 11 1D 90 00 13 AF EC 00 12 C8 FC 00 15 6D BE 00 15 6D DC 00 15 6D FA 00 15 6D 0A 00 13 AF A6 00 00 00");
            user.Send(Write.ack);
        }
    }
}
