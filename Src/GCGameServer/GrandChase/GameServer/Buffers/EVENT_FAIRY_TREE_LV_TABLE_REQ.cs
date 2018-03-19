using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class FairyTree
    {
        public void lvtable(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1185);
            Write.Hex("00 00 00 07 00 00 00 01 00 00 00 00 00 00 00 02 00 00 00 0A 00 00 00 03 00 00 00 1E 00 00 00 04 00 00 00 3C 00 00 00 05 00 00 00 64 00 00 00 06 00 00 01 2C 00 00 00 07 00 00 03 84 00 00 00");
            user.Send(Write.ack);            
        }
    }
}
