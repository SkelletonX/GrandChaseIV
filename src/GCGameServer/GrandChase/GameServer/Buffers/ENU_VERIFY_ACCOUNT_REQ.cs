using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameServer.network;
using System.Data;
using GameServer.Conexao;
using GameServer.Buffers;

namespace GameServer.Packets
{
    class Player
    {        
        Log log = new Log();        
        exptable sendexptable = new exptable();
        VerificarInventario verifiqueInventario = new VerificarInventario();        
        ServerTime servertime = new ServerTime();
        ticketlist tickets = new ticketlist();
        PetVested petvesteditem = new PetVested();
        GraduateCharacter graduatecharacterinfo = new GraduateCharacter();
        missiondate missiondatechange = new missiondate();
        jumpingchar jumpcharinfo = new jumpingchar();
        SlotInfo slotinfo = new SlotInfo();
        FullLookInfo fulllookinfo = new FullLookInfo();
        SystemGuideInfo systemguideinfo = new SystemGuideInfo();
        FairyTreeBuff fairytreebuff = new FairyTreeBuff();
        RitasChristimasUserInfo ritaschristimasinfo = new RitasChristimasUserInfo();

        public void logar(User user,PlayerInfo pInfo,CharsInfo charsInfo, Readers Ler)
        {
            db.DBConnect data = new db.DBConnect();
            DataSet Banco = new DataSet();

            string usuario = Ler.String();
            string senha = Ler.String();

            pInfo.usuario = usuario;
            pInfo.senha = senha;

            PacketManager Write = new PacketManager();
            Write.OP(3);
            data.Exec(Banco, "SELECT   `userid`,  `online`,  `ban`,  `moderador`  FROM `contas` WHERE `usuario` = '" + usuario + "' AND `senha` = '" + senha + "'");           


            if (Banco.Tables[0].Rows.Count > 0)
            {                
                pInfo.userid = Convert.ToInt32(Banco.Tables[0].Rows[0][0].ToString());
                pInfo.online = Convert.ToInt32(Banco.Tables[0].Rows[0][1].ToString());
                pInfo.ban = Convert.ToInt32(Banco.Tables[0].Rows[0][2].ToString());
                pInfo.moderador = Convert.ToInt32(Banco.Tables[0].Rows[0][3].ToString());
                
                pInfo.GetNickname(pInfo.userid);
                pInfo.GetGP(pInfo.userid);
                pInfo.GetVidaBonus(pInfo.userid);
                pInfo.GetSizeInvetario(pInfo.userid);

                //tentativa de Login com conta Banida
                if (pInfo.ban > 0)
                {
                    Write.Int(0);
                    Write.UStr(usuario);
                    Write.Hex("00 00 00 00 05 00 11 3E 0F 28 04 1B 40 40 04 1B 77 01 31 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FD 00 00 00 64 01 7C 00 00 00 00 D1 C0 00 03 53 29 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 29 00 00 00 07 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 10 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 11 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 12 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 13 00 00 00 01 07 00 00 01 00 00 00 00 00 00 00 14 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 15 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 16 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 17 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 18 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 19 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 1A 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 1B 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 1E 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 24 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 27 00 00 00 01 03 00 00 00 00 00 00 00 00 00 00 28 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 29 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2A 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 2B 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2C 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2D 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2E 00 00 00 01 03 00 00 00 00 01 00 00 00 00 00 2F 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 30 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 31 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 32 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 33 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 34 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 35 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 36 00 00 00 01 07 00 00 01 00 02 00 00 00 00 00 38 00 00 00 01 06 00 00 00 00 02 00 00 00 00 00 3E 00 00 00 01 01 00 00 01 00 00 00 00 0F 3C 08 8D 00 00 00 00 EC 46 08 8D 40 64 02 52 A2 00 7E E0 00 00 00 80 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 56 AD 8F E4 56 D3 E8 74 00 00 00 00 00 00 00 00 00 00 00 00 01 11 40 7E EE 00 00 00 00 40 64 02 52 A2 3C 7E E0 01 01 00 00 00 01 61 D0 B2 C0 FF 08 FF FF FF BC 02 50 EF C4 08 8D 11 00 00 00 00 00 7E EE A2 00 00 00 00 C0 00 00 00 00 00 00 00 00 00 00 00 04 7E F4 BA 01 00 00 00 00 00 00 00 00 00 00 00 00 11 34 08 8D FD FD 00 59 44 DD 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 01 00 00 00 01 29 00 7C 90 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01");
                    user.Send(Write.ack);
                }
                else
                {
                    sendexptable.xptable(user);
                    verifiqueInventario.GetInventory(user,pInfo.userid);
                    charsInfo.GetCharactersFromDB(pInfo.userid);                    
                    Write.UStr(usuario);
                    Write.UStr(pInfo.nickname);
                    Write.Byte(0);
                    Write.Int(pInfo.gamePoint);
                    Write.Hex("A0 04 8E C0 10 04 8E DD 01 DE 30 31 C8");
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Byte(255);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Short(0);
                    Write.Byte(0);
                    Write.Int(100);
                    Write.Byte(0);
                    Write.Byte(0);
                    Write.Int(charsInfo.getLength());
                    for (int countchar = 0; countchar < charsInfo.getLength(); countchar ++ )
                    {
                        Write.Byte((byte)charsInfo.personagems[countchar].personagemid);
                        Write.Byte((byte)charsInfo.personagems[countchar].personagemid);
                        Write.Int(0);
                        Write.Byte((byte)charsInfo.personagems[countchar].classe);
                        Write.Byte((byte)charsInfo.personagems[countchar].classe);
                        Write.Int(0);
                        Write.Int(charsInfo.personagems[countchar].experiencia);
                        Write.Int(charsInfo.personagems[countchar].vitoria);
                        Write.Int(charsInfo.personagems[countchar].derrota);
                        Write.Int(charsInfo.personagems[countchar].vitoria);
                        Write.Int(charsInfo.personagems[countchar].derrota);
                        Write.Int(0);
                        Write.Int(charsInfo.personagems[countchar].experiencia);
                        Write.Int(charsInfo.personagems[countchar].nivel);
                        Write.Int(charsInfo.personagems[countchar].equipamentos.Length);
                        for (int equipscount = 0; equipscount < charsInfo.personagems[countchar].equipamentos.Length; equipscount++)
                        {
                            Write.Int(charsInfo.personagems[countchar].equipamentos[equipscount].itemid);
                            Write.Int(0);
                            Write.Int(charsInfo.personagems[countchar].equipamentos[equipscount].itemuid);
                            Write.Int(0);
                            Write.Int(0);
                            Write.Int(0);
                            Write.Int(0);
                            Write.Short(0);
                            Write.Byte(0);
                        }
                        Write.Int(0);
                        Write.Int(0);
                        Write.Int(charsInfo.personagems[countchar].splef);
                        Write.Hex("00 00 00 A0 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 07 AF 00 00 00 00 00 00 07 AF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 2C 00 00 01 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 07");
                    }
                    Write.Short((short)configserver.Port);//9401
                    Write.Int(pInfo.userid);
                    Write.UStr(configserver.nome);
                    Write.Int(configserver.checkMensagem);
                    Write.Int(configserver.checkMensagem);
                    if (configserver.mensagem.Length > 0)
                    {
                        Write.UStr(configserver.mensagem);
                    }
                    Write.Hex("00 00 00 4E 00 00 00 07 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 08 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 09 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0A 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0B 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0C 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0D 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 0E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 10 00 00 00 00 00 00 00 00 00 00 00 00 00 00 11 00 00 00 00 00 00 00 00 00 00 00 00 00 00 12 00 00 00 00 00 00 00 00 00 00 00 00 00 00 13 00 00 00 00 00 00 00 00 00 00 00 00 00 00 14 00 00 00 00 00 00 00 00 00 00 00 00 00 00 15 00 00 00 00 00 00 00 00 00 00 00 00 00 00 16 00 00 00 00 00 00 00 00 00 00 00 00 00 00 17 00 00 00 00 00 00 00 00 00 00 00 00 00 00 18 00 00 00 00 00 00 00 00 00 00 00 00 00 00 19 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 1E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 24 00 00 00 00 00 00 00 00 00 00 00 00 00 00 27 00 00 00 00 00 00 00 00 00 00 00 00 00 00 28 00 00 00 00 00 00 00 00 00 00 00 00 00 00 29 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 2F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 30 00 00 00 00 00 00 00 00 00 00 00 00 00 00 31 00 00 00 00 00 00 00 00 00 00 00 00 00 00 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 33 00 00 00 00 00 00 00 00 00 00 00 00 00 00 34 00 00 00 00 00 00 00 00 00 00 00 00 00 00 35 00 00 00 00 00 00 00 00 00 00 00 00 00 00 36 00 00 00 00 00 00 00 00 00 00 00 00 00 00 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 38 00 00 00 00 00 00 00 00 00 00 00 00 00 00 39 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 3F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00 00 00 00 00 00 00 00 43 00 00 00 00 00 00 00 00 00 00 00 00 00 00 44 00 00 00 00 00 00 00 00 00 00 00 00 00 00 45 00 00 00 00 00 00 00 00 00 00 00 00 00 00 46 00 00 00 00 00 00 00 00 00 00 00 00 00 00 47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 48 00 00 00 00 00 00 00 00 00 00 00 00 00 00 49 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 50 00 00 00 00 00 00 00 00 00 00 00 00 00 00 51 00 00 00 00 00 00 00 00 00 00 00 00 00 00 52 00 00 00 00 00 00 00 00 00 00 00 00 00 00 53 00 00 00 00 00 00 00 00 00 00 00 00 00 00 54 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 55 00 00 00 00 00 00 00 00 00 00 00 00 00 00 56 00 00 00 00 00 00 00 00 00 00 00 00 00 00 57 00 00 00 00 00 00 00 00 00 00 00 00 00 00 58 00 00 00 00 00 00 00 00 00 00 00 00 00 00 59 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5A 00 00 00 01 01 00 00 00 00 00 00 00 00 00 00 5B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5D 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 5F 00 00 00 00 00 00 00 00 00 00 00 13 0B 75 8D 00 00 00 1C 00 07 0E 04 00 00 00 01 00 98 96 88 00 00 00 00 58 1F 3B 5C 58 1D DB CC 00 00 00 00 00 07 0E 0E 00 00 00 01 00 98 96 89 00 00 00 00 58 28 D9 00 58 27 87 80 00 00 00 00 00 07 0E 18 00 00 00 01 00 98 96 8A 00 00 00 00 58 28 D9 00 58 27 87 80 00 00 00 00 00 07 19 08 00 00 00 01 00 98 96 81 00 00 00 00 57 A8 09 60 57 A6 B7 E0 00 00 00 00 00 07 19 12 00 00 00 01 00 98 96 82 00 00 00 00 57 A8 09 60 57 A6 B7 E0 00 00 00 00 00 07 24 52 00 00 00 01 00 98 96 81 00 00 00 00 57 A8 09 60 57 A6 B7 E0 00 00 00 00 00 07 24 5C 00 00 00 01 00 98 96 82 00 00 00 00 57 A8 09 60 57 A6 B7 E0 00 00 00 00 00 07 C3 08 00 00 00 01 01 31 2D 03 00 00 00 00 58 2A 7B D4 58 29 2A 54 00 00 00 00 00 0A B0 36 00 00 00 01 00 98 96 83 00 00 00 00 57 AD 34 6C 57 AB E2 EC 00 00 00 00 00 0A B0 40 00 00 00 01 00 98 96 84 00 00 00 00 57 AD 34 6C 57 AB E2 EC 00 00 00 00 00 0A B0 4A 00 00 00 01 00 98 96 85 00 00 00 00 58 27 25 88 58 25 D4 08 00 00 00 00 00 0A B0 54 00 00 00 01 00 98 96 86 00 00 00 00 58 27 25 88 58 25 D4 08 00 00 00 00 00 0A B0 72 00 00 00 01 00 98 96 89 00 00 00 00 58 27 38 84 58 25 E7 04 00 00 00 00 00 0A B0 7C 00 00 00 01 00 98 96 8A 00 00 00 00 58 27 38 84 58 25 E7 04 00 00 00 00 00 0A B0 86 00 00 00 01 00 98 96 8B 00 00 00 00 58 2A 7D 78 58 29 2B F8 00 00 00 00 00 0A B0 90 00 00 00 01 00 98 96 8C 00 00 00 00 58 2A 7D 78 58 29 2B F8 00 00 00 00 00 0A B0 9A 00 00 00 01 00 98 96 8D 00 00 00 00 58 2A 7F 1C 58 29 2D 9C 00 00 00 00 00 0A B0 A4 00 00 00 01 00 98 96 8E 00 00 00 00 58 2A 7F 1C 58 29 2D 9C 00 00 00 00 00 0A B0 AE 00 00 00 01 00 98 96 8F 00 00 00 00 58 2A 80 C0 58 29 2F 40 00 00 00 00 00 0A B0 B8 00 00 00 01 00 98 96 90 00 00 00 00 58 2A 80 C0 58 29 2F 40 00 00 00 00 00 0A B0 C2 00 00 00 01 00 98 96 91 00 00 00 00 58 2A 82 A0 58 29 31 20 00 00 00 00 00 0A B0 CC 00 00 00 01 00 98 96 92 00 00 00 00 58 2A 82 A0 58 29 31 20 00 00 00 00 00 0A E8 58 00 00 00 01 00 98 96 81 00 00 00 00 57 A8 0E 10 57 A6 BC 90 00 00 00 00 00 0A E8 62 00 00 00 01 00 98 96 82 00 00 00 00 57 A8 0E 10 57 A6 BC 90 00 00 00 00 00 0D 48 D2 00 00 00 01 00 98 96 81 00 00 00 00 57 A8 0D D4 57 A6 BC 54 00 00 00 00 00 0D 48 DC 00 00 00 01 00 98 96 82 00 00 00 00 57 A8 0D D4 57 A6 BC 54 00 00 00 00 00 12 9E 04 00 00 00 01 00 98 98 15 00 00 00 00 58 1E B8 58 58 1D 66 D8 00 00 00 00 00 13 8C A6 00 00 00 01 00 B4 0D C2 00 00 00 00 58 2A 8A 98 58 29 39 18 00 00 00 00 00 80 00 18 00 00 00 00 00 5A 00 00 00 00 00 80 00 00 00 00 00 00 00 00");
                    Write.UStr(configserver.MsgNome);
                    Write.Str(configserver.MsgIP);
                    Write.Short((short)configserver.MsgPort);
                    Write.Int(292);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Hex("FF FF FF FF FF FF FF FF");
                    Write.Str(configserver.MsgIP);
                    Write.Int(0);
                    Write.Int(0);
                    Write.Int(56095091);
                    Write.Hex("AC 57 F1 73 AC");
                    Write.Int(0);
                    Write.Int(0);//PETS
                    Write.Byte(0);
                    Write.Int(1);
                    Write.Int(pInfo.tamanhoinventario);
                    Write.Int(0);
                    Write.Int(pInfo.bonusvida);
                    Write.Short(0);
                    Write.Int(1);
                    Write.Int(1);
                    Write.Hex("61 D0 B2 C0 00 64 7E EE E2 C0 07 E7 10 6B 7C 92 A0 00 00 00 00 A4 72 93 E0 57 EF 5E F0");
                    Write.Int(0);
                    Write.Int(20);
                    for (int tChars = 0; tChars < 20; tChars++)
                    {
                        Write.Int(tChars);
                        Write.Int(tChars);
                        Write.Int(0);
                        Write.Int(0);
                        Write.Short(0);
                    }
                    Write.Int(2);
                    Write.Int(30);
                    Write.Int(779510);
                    Write.Int(31);
                    Write.Int(1404170);
                    Write.Int(400);
                    Write.Byte(0);
                    user.Send(Write.ack);
                }
            }
            else
            {
                //falha ao logar
                Write.Int(20);
                Write.UStr(usuario);
                Write.Int(0);
                user.Send(Write.ack);
                user.close();
            }
            servertime.servertime(user);
            tickets.sendlist(user);
            petvesteditem.petvesteditem(user);
            graduatecharacterinfo.GraduateCharacterInfo(user);
            missiondatechange.missiondatechange(user);
            jumpcharinfo.jumpingcharinfo(user);
            slotinfo.slotinfo(user);
            fulllookinfo.fulllookinfo(user);
            systemguideinfo.systemguideinfo(user);
            fairytreebuff.fairytreebuff(user);
            ritaschristimasinfo.ritaschristimasuserInfo(user);
        }
    }
}
