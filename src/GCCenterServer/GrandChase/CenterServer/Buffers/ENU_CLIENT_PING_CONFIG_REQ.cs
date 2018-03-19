using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;

namespace CenterServer.Packets
{
    public class Ping
    {
        public void ping(User user)
         {
             PacketManager Write = new PacketManager();
             Write.OP(26);
             Write.Header();
             Write.Hex("00 00 0F A0 00 00 0F A0 00 00 0F A0 00 00 00 01 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00");
             user.Send(Write.ack);
        }
    }
}
