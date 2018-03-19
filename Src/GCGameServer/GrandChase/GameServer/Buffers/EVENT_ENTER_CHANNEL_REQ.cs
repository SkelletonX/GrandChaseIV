using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class EnterChannel
    {
        public void enterchannel(User user, Readers ler)
        {
            int serverid = ler.Int();
            PacketManager Write = new PacketManager();
            Write.OP(13);
            Write.Hex("00 00 00 00 00 59 23 DD F0 59 25 2F 6F");
            user.Send(Write.ack);
        }
    }
}
