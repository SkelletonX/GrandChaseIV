using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using MySql.Data;
using CenterServer.network;
using CenterServer.db;
using System.Collections;
using LuaInterface;

namespace CenterServer
{
    class Program
    {
        public static int[] Ports = { 9500, 9501 };
        static void Main(string[] args)
        {
            Log log = new Log();
            log.title("GCEmu Season 4 -- By Hiro & Skell");
            DBConnect db = new DBConnect();
            log.Warn("Configuraçoes MySQL:");
            db.Initialize();
            configserver.servers = new Server[configserver.hosts.Length + 1];
            db.OpenConnection();            
            if (db.Connected == true)
            {
                log.Info("\n -- Servidor [" + configserver.server + "]");
                log.Info("-- Banco [" + configserver.database + "]");
                log.Info("-- Usuario [" + configserver.uid + "] ");
                log.Info("-- Senha [" + configserver.password + "]\n");
                for (byte a = 0; a < 2; a++)
                {
                    configserver.Port = Ports[a];
                    configserver.servers[a] = new Server(a, configserver.hosts[0]);
                }
            }            
            log.Status(log.LUA_RELEASE + "  " + log.LUA_COPYRIGHT);            
            while (true)
            {
                Console.Write(">>");
                Console.ReadLine();
            }
        }
    }
}
