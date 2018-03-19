using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class itemtrade
    {
        public void list(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(607);
            Write.Hex("00 00 00 03 00 02 CB AA 00 00 00 06 00 01 86 46 00 00 00 01 FF FF FF FF 00 FF 00 00 00 0F BF 5E 00 00 00 46 00 0F C0 94 00 00 00 01 FF FF FF FF 00 FF 00 00 00 0F BF 86 00 00 00 04 00 0F BF 90 00 00 00 01 FF FF FF FF 00 FF 00 00 00 00 00 01 00 03 2D F2 00 02 AE B8 00 00 00 01 FF FF FF FF 00 FF 00 00 00 00 00");
            user.Send(Write.ack);
        }
         
    }
}
