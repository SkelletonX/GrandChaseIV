using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class StartGame
    {
        int playersInSala = 1;
        byte Match = 2;
        int Game = 7;
        int Map = 1;
        int FreeSlot = 3;
        public void rungame(User user)
        {
            PacketManager Write = new PacketManager();
            Write.OP(38);
            Write.Int(0);
            Write.Int(1379592610);
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            Write.Int(playersInSala);
            Write.Int(user.pInfo.userid);
            Write.Int(149246);
            Write.Int(playersInSala);
            Write.Int(user.pInfo.userid);
            Write.Hex("00 00 01 04 00 00 00 6A");
            Write.Int(0);
            Write.Int(user.pInfo.userid);
            Write.Hex("00 00 00 00 00 00 00");
            Write.Byte(Match);
            Write.Int(Game);
            Write.Boolean(false);
            Write.Int(Map);
            Write.Hex("00 00 00 00 FF FF FF FF 00 00 00 01 00 00 00");
            Write.Short((short)playersInSala);
            Write.Short((short)FreeSlot);
            Write.Boolean(false);
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            Write.Short((short)playersInSala);
            Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");

            user.Send(Write.ack);
        }
    }
}
