using System;

namespace GrandChase.Net
{
    public sealed class PacketSendException : Exception
    {
        public override string Message
        {
            get
            {
                return "Disconnected while sending packet.";
            }
        }
    }
}
