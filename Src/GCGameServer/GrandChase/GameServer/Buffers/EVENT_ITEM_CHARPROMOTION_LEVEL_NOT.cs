using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class itemcharpromotion
    {
        public void item(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(881);
            Write.Hex("00 00 00 01 00 05 98 58 00 00 00 0A 00 00 00 00 14 01 00 00 00 14 02 00 00 00 14 03 00 00 00 14 04 00 00 00 14 05 00 00 00 14 06 00 00 00 14 07 00 00 00 14 08 00 00 00 14 09 00 00 00 14 00 00 00");
            user.Send(Write.ack);
        }
    }
}
