using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;

namespace CenterServer.Packets
{
    class SHAFileList
    {
        public void NamesList(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(28);
            Write.Header();
            Write.Hex("00 00 00 00 00 00 00 03 00 00 00 00 00 00");
            user.Send(Write.ack);
        }
    }
}
