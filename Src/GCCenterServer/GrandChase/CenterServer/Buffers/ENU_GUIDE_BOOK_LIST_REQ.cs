using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;

namespace CenterServer.Packets
{
    class GuideBook
    {
        public void GuideBookList(User usr)
        {
            PacketManager Write = new PacketManager();
            Write.OP(18);
            Write.Header();
            Write.Hex("00 00 00 01 00 00 00 00 00 00 00");
            usr.Send(Write.ack);
        }
    }
}
