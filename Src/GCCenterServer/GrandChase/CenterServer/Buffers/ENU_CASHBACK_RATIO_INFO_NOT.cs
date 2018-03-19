using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;

namespace CenterServer.Packets
{
    class CashBackRatio
    {
        public void CashBack(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(30);
            Write.Header();
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
