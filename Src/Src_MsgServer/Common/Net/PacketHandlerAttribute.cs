using System;

namespace GrandChase.Net
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PacketHandlerAttribute : Attribute
    {
        public uint RTTIValue { get; private set; }
    }
}
