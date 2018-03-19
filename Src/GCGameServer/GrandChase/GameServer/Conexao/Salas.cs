using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;

namespace GameServer.Conexao
{
    public class Sala
    {
        public struct slots
        {
            public bool ativo;
            public bool aberto;
            public bool Lider;
            public bool AFK;
            public byte Status;            
            public int Mortes;            
            public User user;
        }
        public short SalaID;
        public string SalaSenha;
        public string SalaNome;
        public int MatchMode;
        public int GameMode;
        public int Map;
        public int ITMode;
        public bool RndMap;        
        public int expulsar;
        public bool jogando;
        public slots[] slotslen = new slots[4];

        public int slotsAbertos()
        {
            Int32 islot = 0;
            for (Int32 a = 0; a < 4; a++)
                if (slotslen[a].aberto == true)
                    islot++;
            return islot;
        }
        

        public int PlayersEmSala()
        {
            Int32 players = 0;
            for (int a = 0; a < 4; a++)
                if (slotslen[a].ativo == true)
                    players++;
            return players;
        }

        public int slotind(User user)
        {
            for (Int32 gg = 0; gg < 4; gg++)
            {
                if (slotslen[gg].user == user&& slotslen[gg].ativo== true)
                    return gg;
            }
            return -1;
        }

        public User ObterSessao()
        {
            for (int a = 0; a < 4; a++)
            {
                if (slotslen[a].Lider== true)
                    return slotslen[a].user;
            }
            return null;
        }

    }
}
