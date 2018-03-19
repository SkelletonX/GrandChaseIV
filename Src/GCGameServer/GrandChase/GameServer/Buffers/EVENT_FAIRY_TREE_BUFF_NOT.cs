using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class FairyTreeBuff
    {
        public void fairytreebuff(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1178);
            Write.Hex("00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 58 85 33 11");
            user.Send(Write.ack);
        }
    }
}
