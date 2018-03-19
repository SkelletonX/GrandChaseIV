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
using CenterServer.Packets;

namespace CenterServer.network
{
    public class Packets
    {
        Readers Ler = new Readers();
        HeartBeat heartbeat = new HeartBeat();
        Load loadImages = new Load();
        Log log = new Log();
        Player player = new Player();
        Ping ping = new Ping();
        SocketTableinf SocketTableinfo = new SocketTableinf();
        SHAFileList SHAFile = new SHAFileList();
        GuideBook Guidebook = new GuideBook();

        public Packets(short opcode, Readers Ler,User here,byte[] buffer)
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
                        player.logar(here,Ler);
                        break;
                    }

                case 17:
                    {
                        Guidebook.GuideBookList(here);
                        break;
                    }

                case 23:
                    {
                        loadImages.LoadList(here);
                        break;
                    }

                case 25:
                    {
                        ping.ping(here);
                        break;
                    }

                case 27:
                    {
                        SHAFile.NamesList(here);
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
