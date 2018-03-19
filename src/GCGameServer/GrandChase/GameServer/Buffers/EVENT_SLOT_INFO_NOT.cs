using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class SlotInfo
    {
        public void slotinfo(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(309);
            Write.Hex("00 00 00 02 00 00 00 00 00 00 E5 6A 00 00 00 01 30 A5 3B 92 00 00 00 00 01 00 00 E5 88 00 00 00 01 30 A5 3B 93 00 00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
