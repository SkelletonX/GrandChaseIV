using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Buffers
{
    class ClientInfo
    {
        public void PC_Info(User user,Readers ler)
        {
            ler.Int();
            string WindowsVersion = ler.UString();
            string PlacaDeRede = ler.UString();
            int unk = ler.Int();
            int[] unk2 = { 0 };
            for (int i = 0; i < unk; i++)
            {
                Array.Resize(ref unk2,unk);
                unk2[i] = ler.Int();
            }
            string PlacaDeVideo = ler.UString();
            ler.Int();
            ler.Byte();
            string DirectxVersion = ler.UString();

            PacketManager Write = new PacketManager();
            Write.OP(226);
            Write.Int(0);
            Write.UStr(WindowsVersion);
            Write.UStr(PlacaDeRede);
            Write.Int(unk);
            for (int y = 0; y < unk; y++)
            {
                Write.Int(unk2[y]);
            }
            Write.UStr(PlacaDeVideo);
            Write.Int(0);
            Write.Byte(0);
            Write.UStr(DirectxVersion);
            Write.Hex("00 00 00");
            user.Send(Write.ack);
        }
    }
}
