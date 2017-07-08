using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.Interfaces
{
    public interface ILog
    {
        string Name { get; }

        void LogInfo(string Message, params object[] Args);
        void LogSuccess(string Message, params object[] Args);
        void LogWarning(string Message, params object[] Args);
        void LogHex(string Message, byte[] Array);
        void LogError(string Message, params object[] Args);
        void LogFatal(Exception e);
    }
}