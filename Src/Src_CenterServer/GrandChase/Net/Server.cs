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
            Console.Title = "Z Team Studio - CenterServer";
            bool socketss = true;

            if (socketss == true)
            {
                LogFactory.OnWrite += Manager.Log.LogFactory_ConsoleWrite;
                Console.WriteLine("Pressione Enter Para Iniciar o Servidor");
                Console.Clear();
                start:
                    try
                    {
                        Settings.Initialize();

                        AutoRestartTIme = Settings.GetInt("CenterServer/AutoRestartTime");
                        LogFactory.GetLog("Main").LogInfo("Reinicio Automatico do Servidor: {0} Segundos.", Server.AutoRestartTIme);

                        Clients = new GCClients();
                        LogFactory.GetLog("Main").LogInfo("Criando lista de clientes");

                        Handlers = new HandlerStore<GCClient>();
                        Handlers.Cache();

                        Database.Test();
                        Database.Analyze();

                        
                        Shop shop = new Shop();

                        LogFactory.GetLog("Shop").LogWarning("Carregando Items");
                        shop.SendShop();
                        LogFactory.GetLog("Shop").LogWarning("Items Carregados,Total: " + shop.goodsIttems.Length);

                        RemoteEndPoint = new IPEndPoint(Settings.GetIPAddress("CenterServer/ExternalIP"), Settings.GetInt("CenterServer/Port"));

                        Listener = new TcpListener(IPAddress.Any, Server.RemoteEndPoint.Port);
                        Listener.Start();
                        Loading loading = new Loading();

                       // loading.addcheck("main2.exe","A42B77BEEECE321B19257C7EE803548B9DFD7A42");
                        //loading.addcheck("script.exe", "A42B77BEEECE321B19257C7EE803548B9DFD7A42");

                        
                        LogFactory.GetLog("Main").LogInfo("threads Iniciada, Porta: {0}.", Server.Listener.LocalEndpoint);

                        IsAlive = true;
                    }
                    catch (Exception e)
                    {
                        LogFactory.GetLog("Main").LogFatal(e);
                    }

                    if (Server.IsAlive)
                    {
                        LogFactory.GetLog("Main").LogSuccess("O servidor foi aberto com sucesso. threads : {0}.", Thread.CurrentThread.ManagedThreadId);
                    }
                    else
                    {
                        LogFactory.GetLog("Main").LogInfo("Occoreu um Erro Servidor nao foi iniciado.");
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

                    LogFactory.GetLog("Main").LogWarning("Servidor foi Interrompido.");

                    if (Server.AutoRestartTIme > 0)
                    {
                        LogFactory.GetLog("Main").LogInfo("O Servidor Reiniciara daqui a {0} segundos .", Server.AutoRestartTIme);

                        Thread.Sleep(Server.AutoRestartTIme * 1000);

                        goto start;
                    }
                    else
                    {
                        Console.Read();
                    }
            }
        }

        private static void OnAcceptSocket(IAsyncResult ar)
        {
            Server.AcceptDone.Set();

            ClientSession pSession = new ClientSession(Listener.EndAcceptSocket(ar));

            LogFactory.GetLog("Main").LogInfo("Nova Conexao de Socket. ID: {0}", pSession.Label);
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
