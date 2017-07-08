using GrandChase.Data;
using GrandChase.IO;
using GrandChase.IO.Packet;
using GrandChase.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GrandChase.Net.Client;
using GrandChase.Function;
using System.Text;
using Common;
using Manager.Factories;
using Manager;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;

namespace GrandChase.Net
{
    internal static class Server
    {
        private static bool isAlive;
        private static ManualResetEvent AcceptDone = new ManualResetEvent(false);

        public static TcpListener Listener { get; private set; }
        public static IPEndPoint RemoteEndPoint { get; private set; }
        
        public static GCClients Clients { get; private set; }
        public static HandlerStore<GCClient> Handlers { get; private set; }
        public static Dictionary<int, IPEndPoint> UdpBackup { get; private set; }

        public static int AutoRestartTIme { get; private set; }

        public static string MsgServerIP;
        public static short MsgServerPort;
        public static string UDPRelayIP;
        public static short UDPRelayPort;
        public static string TCPRelayIP;
        public static short TCPRelayPort;

        public static string DataPath { get; private set; }

        public static object mLock = new object();

        public static bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                isAlive = value;

                if (!value)
                {
                    Server.AcceptDone.Set();
                }
            }
        }

        [STAThread]
        private static void Main()
        {
            Console.Title = "Zteam Studio GameServer";
            LogFactory.OnWrite += Log.LogFactory_ConsoleWrite;
                start:
                    try
                    {
                        Settings.Initialize();

                        AutoRestartTIme = Settings.GetInt("GameServer/AutoRestartTime");
                        LogFactory.GetLog("Main").LogInfo("reinicializaçao automatica do servidor é definido como {0} segundos.", Server.AutoRestartTIme);

                        UDPRelayIP = Settings.GetString("GameServer/UDPRelayIP");
                        UDPRelayPort = Settings.GetShort("GameServer/UDPRelayPort");
                        TCPRelayIP = Settings.GetString("GameServer/TCPRelayIP");
                        TCPRelayPort = Settings.GetShort("GameServer/TCPRelayPort");
                        MsgServerIP = Settings.GetString("GameServer/MsgServerIP");
                        MsgServerPort = Settings.GetShort("GameServer/MsgServerPort");

                        TSingleton<ChannelManager>.Instance.AddChannel("대전", 2000, 0);
                        TSingleton<ChannelManager>.Instance.AddChannel("던전", 2000, 0);

                        Clients = new GCClients();
                        LogFactory.GetLog("Main").LogInfo("Criando lista de clientes objetos");

                        Handlers = new HandlerStore<GCClient>();
                        Handlers.Cache();

                        Database.Test();
                        Database.Analyze();

                        LogFactory.GetLog("Shop").LogInfo("Carregando Items da DB");
                        Shop shop = new Shop();

                        shop.LoadShop();
                        LogFactory.GetLog("Shop").LogInfo("Items Carregads, Total: " + shop.goodsIttems.Length);

                        LogFactory.GetLog("Hero Dungeon").LogInfo("Carregando Lista De Hero!");

                        GCCommon MyCommon = new GCCommon();

                        MyCommon.LoadHero();
                        LogFactory.GetLog("Hero Dungeon").LogInfo("Hero Dungeon Carreda!,Total: " + MyCommon.herodungeon.Length);

                        RemoteEndPoint = new IPEndPoint(Settings.GetIPAddress("GameServer/ExternalIP"), Settings.GetInt("GameServer/Port"));

                        Listener = new TcpListener(IPAddress.Any, Server.RemoteEndPoint.Port);
                        Listener.Start();
                        LogFactory.GetLog("Main").LogInfo("conexao socket iniciada. porta: {0}.", Server.Listener.LocalEndpoint);

                        User.StatusOnline(null, null, 0, 2);

                        IsAlive = true;
                    }
                    catch (Exception e)
                    {
                        LogFactory.GetLog("MAIN").LogFatal(e);
                    }

                    if (Server.IsAlive)
                    {
                        LogFactory.GetLog("Main").LogInfo("o servidor foi aberto com sucesso. a identificaçao do segmento : {0}.", Thread.CurrentThread.ManagedThreadId);
                    }
                    else
                    {
                        LogFactory.GetLog("Main").LogInfo("o servidor nao foi iniciado com sucesso.");
                    }

                    while (Server.IsAlive)
                    {
                        Server.AcceptDone.Reset();

                        Server.Listener.BeginAcceptSocket(new AsyncCallback(Server.OnAcceptSocket), null);

                        Server.AcceptDone.WaitOne();
                    }

                    GCClient[] remainingClients = Server.Clients.ToArray();

                    foreach (GCClient client in remainingClients)
                    {
                        client.Close();
                    }

                    Server.Dispose();

                    LogFactory.GetLog("Main").LogWarning("Error ");

                    if (Server.AutoRestartTIme > 0)
                    {
                        Thread.Sleep(Server.AutoRestartTIme * 1000);

                        goto start;
                    }
                    else
                    {
                        Console.Read();
                    }
                {
                    return;
                }
        }

        private static void OnAcceptSocket(IAsyncResult ar)
        {
            Server.AcceptDone.Set();

            ClientSession pSession = new ClientSession(Listener.EndAcceptSocket(ar));

        }

        private static void OnUDPPacketReceived(InPacket inPacket, IPEndPoint endpoint)
        {
        }

        private static void Dispose()
        {
            if (Server.Listener != null)
            {
                Server.Listener.Stop();
            }
        }
    }
}
