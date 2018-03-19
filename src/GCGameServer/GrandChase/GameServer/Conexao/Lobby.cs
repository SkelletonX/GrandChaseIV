using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;
using GameServer.Buffers;

namespace GameServer.Conexao
{
    public class Canal
    {
        public List<User> PlayersNoLobby { get; set; }
        public List<Sala> ListaDeSalas { get; set; }
        public int AtualSala
        {
            get { return ListaDeSalas == null ? 0 : ListaDeSalas.Count; }
        }
        public Dictionary<ushort, Sala> LMapa { get; set; }
        public ushort users
        {
            get { return (ushort)(PlayersNoLobby == null ? 0 : PlayersNoLobby.Count); }
        }

        public object _a = new object();

        public ushort SalaVazia()
        {
            lock (_a)
            {
                for (ushort b = 0; b < ushort.MaxValue; b++)
                {
                    //1185
                    if (LMapa.ContainsKey((ushort)b)) continue;

                    return (ushort)b;
                }
            }
            return ushort.MaxValue;
        }

        public void Salas(User user, Readers ler)
        {
            byte Tipo = ler.Byte();

            int Numero = 0;
            PacketManager Write = new PacketManager();            
            foreach (Sala Sala in ListaDeSalas)
            {
                if (Tipo == 1)
                {
                }
                Numero++;
            }
            Write.OP(17);
            Write.Int(Numero);
            foreach (Sala Sala in ListaDeSalas)
            {
                Write.Byte(0);
                Write.Short(Sala.SalaID);
                Write.UStr(Sala.SalaNome);
                if (Sala.SalaSenha.Length > 0)
                {
                    Write.Byte(0);
                }
                else
                {
                    Write.Byte(1);
                }
                Write.Byte(0);
                Write.UStr(Sala.SalaSenha);
                Write.Short((short)(Sala.PlayersEmSala()+Sala.slotsAbertos()));
                Write.Short((short)Sala.PlayersEmSala());
                Write.Boolean(Sala.jogando);
                Write.Hex("2E 02 1B 25 01 00 00 00 00 01 6B F9 38 77 00 00 00 0C 00 00 00 00 00 00 00 01");
                Write.UStr(Sala.ObterSessao().pInfo.nickname);
                Write.Hex("0B 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 01");
            }
            user.Send(Write.ack);
            
        }

        public void CriarSala(User user,PlayerInfo pInfo,Readers ler, CharsInfo charsInfo)
        {
            short blank = ler.Short();
            string Nome = ler.UString();
            int id = ler.Byte();            
            byte Guild = ler.Byte();
            string senha = ler.UString();
            short playersemSala = ler.Short();
            short MaxPlayers = ler.Short();
            byte jogando = ler.Byte();
            byte unk2 = ler.Byte();
            byte matchmode = ler.Byte();
            int gamemode = ler.Int();
            int readMatch = ler.Int();
            int RandomMap = ler.Byte();
            int Map = ler.Int();
            ler.Int();
            ler.Bytes(97);
            string iLogin = ler.String();
            int iUserid = ler.Int();
            string iNick = ler.String();
            int indexcharacter = ler.Int();
            byte iPersonagemID = ler.Byte();
            Sala sala = new Sala();
            sala.SalaID = (short)id;
            sala.ITMode = 2;
            sala.SalaNome = Nome;
            sala.SalaSenha = senha;
            sala.MatchMode = (int)matchmode;
            sala.GameMode = (int)gamemode;
            sala.RndMap = false;
            sala.Map = (int)Map;
            sala.expulsar = 3;
            sala.jogando = false;
            
            user.PersonagemAtual = iPersonagemID;

            for (int m = 0; m < MaxPlayers; m++)
            {
                if (m == 0)
                {
                    sala.slotslen[0].ativo = true;
                    sala.slotslen[0].user = user;
                    sala.slotslen[0].Lider = true;
                    sala.slotslen[0].aberto = false;
                }
                else
                {
                    sala.slotslen[m].ativo = false;
                    sala.slotslen[m].aberto = true;
                    sala.slotslen[m].user = null;
                    sala.slotslen[m].Status = 0;
                    sala.slotslen[m].AFK = false;
                    sala.slotslen[m].Lider = false;
                }
            }
            user.AtualSala = sala;
            
            PacketManager Write = new PacketManager();
            Write.OP(25);
            Write.Int(0);
            Write.UStr(user.pInfo.usuario);
            Write.Int(user.pInfo.userid);
            Write.UStr(user.pInfo.nickname);
            Write.Int(indexcharacter);
            Write.Byte((byte)iPersonagemID);
            Write.Hex("00 FF 00 FF 00 FF 00 00 00 00 00 01 00 00 00 64 00 00");
            Write.Int(user.pInfo.gamePoint);
            Write.Byte(0);
            Write.Byte(0);
            Write.Hex("00 00 00 4E 00 00 00 07 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 13 00 00 00 00 00 00 00 00 00 00 00 00 00 00 14 00 00 00 00 00 00 00 00 00 00 00 00 00 00 15 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 00 00 00 00 00 00 00 00 00 00 00 00 00 00 17 00 00 00 00 00 00 00 00 00 00 00 00 00 00 18 00 00 00 00 00 00 00 00 00 00 00 00 00 00 19 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 24 00 00 00 00 00 00 00 00 00 00 00 00 00 00 27 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 28 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 29 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 2A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 30 00 00 00 00 00 00 00 00 00 00 00 00 00 00 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 33 00 00 00 00 00 00 00 00 00 00 00 00 00 00 34 00 00 00 00 00 00 00 00 00 00 00 00 00 00 35 00 00 00 00 00 00 00 00 00 00 00 00 00 00 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 38 00 00 00 00 00 00 00 00 00 00 00 00 00 00 39 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 00 00 00 00 00 00 00 00 00 00 00 00 00 00 46 00 00 00 00 00 00 00 00 00 00 00 00 00 00 47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 00 00 00 00 00 00 00 00 00 00 00 00 00 00 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 53 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 55 00 00 00 00 00 00 00 00 00 00 00 00 00 00 56 00 00 00 00 00 00 00 00 00 00 00 00 00 00 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 00 00 00 00 00 00 00 00 00 59 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5A 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 00 00");
            charsInfo.GetCharactersFromDB(pInfo.userid); ;
            Write.Int(charsInfo.getLength());
            for (int countchar = 0; countchar < charsInfo.getLength(); countchar++)
            {                
                Write.Byte((byte)charsInfo.personagems[countchar].personagemid);
                Write.Int(0);
                Write.Byte((byte)charsInfo.personagems[countchar].classe);
                Write.Int(0);
                Write.Byte(0);
                Write.Int(charsInfo.personagems[countchar].experiencia);
                Write.Int(charsInfo.personagems[countchar].nivel);
                Write.Int(charsInfo.personagems[countchar].vitoria);
                Write.Int(charsInfo.personagems[countchar].derrota);

                Write.Int(charsInfo.personagems[countchar].equipamentos.Length);                
                for (int equipscount = 0; equipscount < charsInfo.personagems[countchar].equipamentos.Length; equipscount++)
                {
                    Write.Int(charsInfo.personagems[countchar].equipamentos[equipscount].itemid);
                    Write.Int(1);
                    Write.Int(charsInfo.personagems[countchar].equipamentos[equipscount].itemuid);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.UShort(0);
                }
                Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 00 00 00 01 00");
                Write.Int(0);
                Write.Int(255);
                Write.Int(160);
                Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 FF 00 00 00 00 00 00");
                Write.Int(0);
                Write.Hex("00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07");
            }
            Write.Hex("00 00 00 03 01 58 A8 C0 01 8E A8 C0 01 00 00 7F 00 00 00 01 7E FE 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 E5 6A 00 00 00 01 31 7F 24 36 00 00 00 00 01 00 00 E5 88 00 00 00 01 31 7F 24 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 01 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
            Write.Int(user.AtualSala.SalaID);
            Write.UStr(user.AtualSala.SalaNome);
            Write.Hex("01 00");
            Write.UStr(user.AtualSala.SalaSenha);
            Write.Short((short)sala.PlayersEmSala());
            Write.Short((short)(sala.PlayersEmSala() + sala.slotsAbertos()));
            Write.Short(6);
            Write.Byte((byte)sala.MatchMode);
            Write.Int(sala.GameMode);
            Write.Int(sala.ITMode);
            Write.Boolean(sala.RndMap);
            Write.Int(sala.Map);
            Write.Int(12);
            Write.Hex("00 01 01 01 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 01 AA DC 63 C0 25 80 AA DC 63 C0 25 E4 01 00 01 00 00 01 2C 00 00 00 14 00 00 1A 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 04 01 00 00 00 01");
            
            user.Send(Write.ack);
        }

    }
}
