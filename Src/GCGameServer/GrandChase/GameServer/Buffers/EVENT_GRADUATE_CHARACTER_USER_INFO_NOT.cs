using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class GraduateCharacter
    {
        public void GraduateCharacterInfo(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(1580);
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 11 00 00 00 12 00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
