using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class leaveRoom
    {
        public void leaveroom(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(34);
            Write.Int(0);
            user.Send(Write.ack);
            //user.obterCanal.PlayersNoLobby.Add(user);
        }
    }
}
