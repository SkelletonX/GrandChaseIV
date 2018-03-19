using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GCNet.CoreLib;
using GCNet.PacketLib;
using GameServer.Packets;
using GameServer.Buffers;
using GameServer.Conexao;

namespace GameServer.network
{
    public class Packets
    {
        Readers Ler = new Readers();
        HeartBeat heartbeat = new HeartBeat();
        Log log = new Log();
        Player player = new Player();
        equipitem equipamentos = new equipitem();
        CharsInfo charsInfo = new CharsInfo();
        getfullsp spInfo = new getfullsp();
        PetCustom petcustom = new PetCustom();
        VerificarInventario invetario = new VerificarInventario();
        ClientInfo ClientInfo = new ClientInfo();
        GamebleBuy cost_rate = new GamebleBuy();
        charselect joinchar = new charselect();
        choicebox Choicebox = new choicebox();
        exppotion expPotion = new exppotion();
        noneInventory InventoryBuff = new noneInventory();
        AgitStore agitstorematerial = new AgitStore();
        AgitMap agitmap = new AgitMap();
        FairyTree fairytreelvtable = new FairyTree();
        Channellist channellist = new Channellist();
        EnterChannel enterChannel = new EnterChannel();
        Depot depotInfo = new Depot();
        RegisterNick regNick = new RegisterNick();
        Canal canal = new Canal();
        leaveRoom SairdaSala = new leaveRoom();
        StartGame playgame = new StartGame();
        LoadComplete Loading = new LoadComplete();
        StageLoadComplete stageLoading = new StageLoadComplete();

        public Packets(short opcode, Readers Ler,User here,byte[] buffer,PlayerInfo pInfo)
        {
            switch (opcode)
            {                
                case 0:
                    {
                        heartbeat.HeartBeatNot(here);
                        break;
                    }

                case 2:
                    {
                        player.logar(here,pInfo,charsInfo,Ler);
                        break;
                    }

                case 24:
                    {
                        canal.CriarSala(here,pInfo, Ler, charsInfo);
                        break;
                    }

                case 423:
                    {
                        spInfo.fullspInfo(here);
                        break;
                    }

                case 1555:
                    {
                        joinchar.CharSelectJoin(here);
                        break;
                    }

                case 871:
                    {
                        cost_rate.cost_rate(here);
                        break;
                    }

                case 1106:
                    {
                        agitmap.map(here);
                        break;
                    }

                case 1184:
                    {
                        fairytreelvtable.lvtable(here);
                        break;
                    }

                case 1340:
                    {
                        depotInfo.Info(here);
                        break;
                    }

                case 1012:
                    {
                        Choicebox.list(here);
                        channellist.ChannelList(here);
                        enterChannel.enterchannel(here,Ler);
                        break;
                    }
                case 12:
                    {
                        enterChannel.enterchannel(here, Ler);
                        break;
                    }

                case 14:
                    {
                        channellist.ChannelList(here);
                        enterChannel.enterchannel(here, Ler);                        
                        break;
                    }

               case 33:
                    {
                        SairdaSala.leaveroom(here);
                        break;
                    }

               case 36:
                    {
                        playgame.rungame(here);
                        break;
                    }
               case 39:
                    {
                        Loading.LoadCompleteNot(here);
                        break;
                    }

               case 927:
                    {
                        stageLoading.stageLoadComplete(here);
                        break;
                    }

                default:
                    {
                        log.Hex("Recebido, OpCode {" + opcode + "} Payload: ", buffer, 1);
                        break;
                    }
            }
        }
     }
}
