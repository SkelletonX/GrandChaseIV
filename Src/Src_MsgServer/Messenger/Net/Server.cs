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
using System.Text;
using Common;
using Manager;
using Manager.Factories;

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
           LogFactory.OnWrite += Manager.Log.LogFactory_ConsoleWrite;
            start:
            try
            {
                Settings.Initialize();

                AutoRestartTIme = Settings.GetInt("MsgServer/AutoRestartTime");

                Clients = new GCClients();

                Handlers = new HandlerStore<GCClient>();
                Handlers.Cache();

                Database.Test();
                Database.Analyze();

                RemoteEndPoint = new IPEndPoint(Settings.GetIPAddress("MsgServer/ExternalIP"), Settings.GetInt("MsgServer/Port"));

                Listener = new TcpListener(IPAddress.Any, Server.RemoteEndPoint.Port);
                Listener.Start();
                LogFactory.GetLog("Main").LogInfo("Socket Open : {0}.", Server.Listener.LocalEndpoint);
                
                IsAlive = true;
            }
            catch (Exception e)
            {
                LogFactory.GetLog("Main").LogFatal(e);
            }

            if (Server.IsAlive)
            {
                LogFactory.GetLog("Main").LogSuccess("O servidor foi aberto com sucesso ID : {0}.", Thread.CurrentThread.ManagedThreadId);
            }
            else
            {
                LogFactory.GetLog("Main").LogInfo("Ocorreu um erro e o servidor não está aberto.");
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

            LogFactory.GetLog("Main").LogWarning("O servidor foi parado.");

            if (Server.AutoRestartTIme > 0)
            {
                LogFactory.GetLog("Main").LogInfo("Servidor {0} segundos será reiniciado automaticamente.", Server.AutoRestartTIme);

                Thread.Sleep(Server.AutoRestartTIme * 1000);

                goto start;
            }
            else
            {
                Console.Read();
            }
        }

        private static void OnAcceptSocket(IAsyncResult ar)
        {
            Server.AcceptDone.Set();

            ClientSession pSession = new ClientSession(Listener.EndAcceptSocket(ar));

            LogFactory.GetLog("Main").LogInfo("A conexão de Socket foi aceite. ID: {0}", pSession.Label);
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
