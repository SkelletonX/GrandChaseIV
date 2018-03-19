using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class Channellist
    {
        public void ChannelList(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(15);
            Write.Hex("00 00 00 00 00 00 00 7D 01 E2 00 00 00 78 01 6D 8E B1 0E 82 40 14 04 07 11 ED A8 28 28 28 E8 F5 2B C0 D0 51 F1 05 26 5E 0C 09 39 8C E8 FF B3 6B 69 2E 9B AD DE 64 DE 02 27 20 53 AB 81 37 41 69 E9 59 B8 B3 29 D9 15 72 1D 39 A8 4D A7 EB 93 99 A8 04 D1 FF E4 D1 A4 F1 7A 14 F5 50 96 84 AF 30 65 B4 99 F8 F2 FA 99 66 D6 84 CF D3 30 2E 9F F7 7C 92 5F CF A6 8C 96 37 F9 A2 16 06 D9 A2 B7 5F 60 07 08 C8 1B F6");
            user.Send(Write.ack);
        }
    }
}
