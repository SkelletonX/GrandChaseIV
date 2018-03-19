using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class userBingo
    {
        public void Bingo(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(655);
            Write.Hex("00 00 00 01 02 37 63 C0 75 46 2F 20 00 00 00 00 09 2E 0B 40 00 00 00 00 00 00 00 00 01 FF 02 1D 7E 97 FF FF FF 91 02 17 AC");
            user.Send(Write.ack);
        }
    }
}
