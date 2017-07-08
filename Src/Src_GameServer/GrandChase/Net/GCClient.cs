using GrandChase.IO;
using GrandChase.IO.Packet;
using System;
using System.Net;
using System.Net.Sockets;

namespace GrandChase.Net
{
    public sealed class GCClient : Session
    {

        public GCClient(Socket socket)
            : base(socket)
        {
        }

        public override void OnDisconnect()
        {
        }

        public override void OnPacket(InPacket inPacket)
        {
        }
    }
}
