using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;
using GameServer.Conexao;

namespace GameServer.Buffers
{
    class equipitem
    {
        public void EquipItem(User user,CharsInfo charsInfo,string usuario,int userid)
        {
            PacketManager Write = new PacketManager();
            Write.OP(64);
            Write.UStr(user.pInfo.usuario);
            Write.Byte(0);
            Write.Int(charsInfo.personagems.Length);
            for (int a = 0; a < charsInfo.personagems.Length; a ++ )
            {
                Write.Byte((byte)charsInfo.personagems[a].personagemid);
                Write.Int(charsInfo.personagems[a].equipamentos.Length);
                for (int b = 0; b < charsInfo.personagems[a].equipamentos.Length; b++ )
                {
                    Write.Int(charsInfo.personagems[a].equipamentos[b].itemid);
                    Write.Int(1);
                    Write.Int(charsInfo.personagems[a].equipamentos[b].itemuid);
                    Write.Int(65536);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(256);
                    Write.Byte(0);
                    Write.Byte(0);
                    Write.Byte(0);
                }
                Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 76 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            }
            Write.Int(user.pInfo.userid);
            Write.Short(0);
            user.Send(Write.ack);
        }
    }
}
