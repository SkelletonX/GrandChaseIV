using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Packets
{
    public class HeartBeat
    {
        public void HeartBeatNot(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(0);
            Write.Int(0);
            user.Send(Write.ack);
        }
    }
}
