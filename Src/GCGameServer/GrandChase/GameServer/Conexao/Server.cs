using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;

namespace GameServer.network
{
    public class Server
    {
        public bool active = false;
        public Socket socket = null;        
        public User[] users = null;
        Log log = new Log();
        public byte ServerMaster = 0;

        public Server(byte ServerMaster,string ip)
        {
            try
            {
                IPAddress serverip = null;

                if (IPAddress.TryParse(ip, out serverip))
                {
                    IPEndPoint ipEnd = new IPEndPoint(serverip,configserver.Port);
                    this.active = true;
                    this.ServerMaster = ServerMaster;
                    this.users= new User[configserver.MaxConnection];
                    for (int i = 1; i < users.Length; i++)
                    {
                        users[i] = null;
                    }
                    this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.socket.Bind(ipEnd);
                    this.socket.Listen(0);
                    log.Info("IP do Servidor {" + configserver.hosts[0] + "} Porta {" + configserver.Port + "}");
                    this.socket.BeginAccept(this.new_connection, null);
                }
            }
            catch(Exception e)
            {
                log.Write("{0} \n {1}", e.Message, e.StackTrace);
            }
        }

        private void new_connection(IAsyncResult read)
        {
            try
            {
                if (this.active)
                {
                    Socket newClient = this.socket.EndAccept(read);
                    short getClientID = this.GetID();

                    if (getClientID > 0)
                    {                        
                        this.users[getClientID] = new User(newClient, getClientID,this.ServerMaster);
                    }
                    else
                    {
                        newClient.Close();
                        newClient = null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} \n {1}", e.Message, e.StackTrace);
            }
            finally
            {
                if (this.active)
                {
                    this.socket.BeginAccept(this.new_connection,null);
                }
            }
        }

        private short GetID()
        {
            try
            {
                for (short i = 1; i < this.users.Length; i++)
                {
                    if (users[i] == null)
                    {
                        return i;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} \n {1}", e.Message, e.StackTrace);
            }
            return 0;
        }
    }

}
