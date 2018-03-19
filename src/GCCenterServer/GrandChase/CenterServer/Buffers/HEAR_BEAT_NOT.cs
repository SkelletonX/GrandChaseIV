using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;

namespace CenterServer.Packets
{
    public class HeartBeat
    {
        public void HeartBeatNot(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(0);
            Write.Header();
            Write.Hex("00 00 00 00"); 
            user.Send(Write.ack);
        }
    }
}
