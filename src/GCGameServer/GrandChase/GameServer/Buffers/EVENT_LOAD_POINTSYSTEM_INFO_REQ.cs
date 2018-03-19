using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class loadpoints
    {
        public void load(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(248);
            Write.Hex("07 E1 04 1E 00 07 E1 05 01 01 07 E1 05 1F 03 00 00 00 00 00 00 00 00 00 00 01 F4 07 E1 05 17 02 00 00 00 05 01 00 00 00 00 02 00 00 00 00 03 00 00 00 00 04 00 00 00 01 07 E1 05 17 02 01 04 05 00 00 00 00 00 00 00 46");
            user.Send(Write.ack);
        }
    }
}
