using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class correio
    {
        public void Correio(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1281);
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
