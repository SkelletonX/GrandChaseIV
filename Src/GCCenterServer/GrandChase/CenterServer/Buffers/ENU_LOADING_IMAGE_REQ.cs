using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenterServer.network;

namespace CenterServer.Packets
{
    public class Load
    {
        string[] TtLoad = new string[4];
        string[] Match = new string[3];
        string[] Square = new string[3];

        void ChangeFiles()
        {
            TtLoad[0] = "Load1_1.dds";
            TtLoad[1] = "Load1_2.dds";
            TtLoad[2] = "Load1_3.dds";
            TtLoad[3] = "LoadGauge.dds";
            Square[0] = "Square.lua";
            Square[1] = "SquareObject.lua";
            Square[2] = "Square3DObject.lua";
            Match[0] = "ui_match_load1.dds";
            Match[1] = "ui_match_load2.dds";
            Match[2] = "ui_match_load3.dds";
        }

        public void LoadList(User user)
        {
            ChangeFiles();
            PacketManager Write = new PacketManager();            
            Write.OP(24);
            Write.Header();
            Write.Hex("00 00 00 00 00 00 00 05 00 00 00 00 00 00 00 01 00 00 00");
            Write.Hex("02 00 00 00 03 00 00 00 04 00 00 00 01 00 00 00 00");

            Write.Int(TtLoad.Length);
            Write.UStr(TtLoad[0]);
            Write.UStr(TtLoad[1]);
            Write.UStr(TtLoad[2]);
            Write.UStr(TtLoad[3]);
            Write.Hex("00 00 00 02 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00");

            Write.Int(Match.Length);
            Write.UStr(Match[0]);
            Write.UStr(Match[1]);
            Write.UStr(Match[2]);
            Write.Int(0);

            Write.Int(Square.Length);
            Write.Int(0);
            Write.UStr(Square[0]);
            Write.Int(1);
            Write.UStr(Square[1]);
            Write.Int(2);
            Write.UStr(Square[2]);
            Write.Hex("00 00 00 03 00 00 00 00 00 00 00 01 00 00 00 02 00 00 00 00");
            Write.Int(0);
            user.Send(Write.ack);
        }
    }
}
