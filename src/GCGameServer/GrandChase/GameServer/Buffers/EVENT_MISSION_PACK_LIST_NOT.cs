using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class missionPack
    {
        public void list(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(594);
            Write.Hex("00 00 00 01 00 02 9B EE 00 00 00 02 00 02 9C 2A 00 02 9C 34");
            user.Send(Write.ack);
        }
    }
}
