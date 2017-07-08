using GrandChase.IO.Packet;
using GrandChase.Net;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GrandChase.Utilities
{
    public class HandlerStore<T>
    {
        private Dictionary<uint, Action<T, InPacket>> _handlers;

        public int Count
        {
            get
            {
                return _handlers.Count;
            }
        }

        public HandlerStore()
        {
            _handlers = new Dictionary<uint, Action<T, InPacket>>();
        }

        public void Cache()
        {
            _handlers.Clear();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GlobalAssemblyCache)
                    continue;

                foreach (Type type in assembly.GetTypes())
                {
                    MethodInfo[] methods = type.GetMethods();

                    foreach (MethodInfo method in methods)
                    {
                        var attribute = Attribute.GetCustomAttribute(method, typeof(PacketHandlerAttribute), false) as PacketHandlerAttribute;

                        if (attribute == null)
                            continue;

                        var callback = Delegate.CreateDelegate(typeof(Action<T, InPacket>), method, false) as Action<T, InPacket>;

                        if (callback == null)
                            continue;

                        _handlers.Add(attribute.RTTIValue, callback);
                    }
                }
            }
        }

        public Action<T, InPacket> GetHandler(uint operationCode)
        {
            Action<T, InPacket> ret = null;

            _handlers.TryGetValue(operationCode, out ret);

            return ret;
        }
    }
}
