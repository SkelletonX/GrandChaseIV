using Common;
using GrandChase.Data;
using GrandChase.Net.Client;
using System.Collections.Generic;
using System;
using System.Data;

namespace GrandChase.Net
{
    public class ClientHolder : TSingleton<ClientHolder>
    {
        private object _lock = new object();

        private Dictionary<string, ClientSession> _clients { get; set; }

        public ClientHolder()
        {
            _clients = new Dictionary<string, ClientSession>();
        }

        public bool IsLoggedIn( string username )
        {
            lock ( _lock )
            {
                if( _clients.ContainsKey( username.ToLower() ) )
                {
                    return true;
                }
            }

            return false;
        }

        public bool VerifyAccount( string login, string passwd, ClientSession session, ref VerifyAccountResult result )
        {
           
            return true;
        }

        public void DestoryAccount( ClientSession session )
        {
            
        }
    }
}
