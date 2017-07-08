using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manager.Helpers;

namespace Manager.Factories
{
    using Interfaces;
    using Abstracts;
    using EventArgs;
    using Enums;
    using System.IO;
    public class LogFactory : ASingleton<LogFactory>
    {
        private static string logDirectory = "logs";
        private Dictionary<string, ILog> Logs;
        public static event EventHandler<LogWriteEventArgs> OnWrite;

        public static string logPath => Path.Combine(logDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".log");

        public override void Initalize()
        {
            Logs = new Dictionary<string, ILog>();

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            int Width = (Console.LargestWindowWidth * 80) / 100; 
            int Height = (Console.LargestWindowHeight * 80) / 100;
            //As variaveis Width & Height, podem mudar se quiser
            if (Width > 0 && Height > 0)
                Console.SetWindowSize(Width, Height);
        }

        public override void Destroy()
        {
        }

        public static ILog GetLog(string Name)
        {
            if(!Instance.Logs.ContainsKey(Name))
            {
                ILog Log = new Log(Name);
                Instance.Logs.Add(Name, Log);
            }
            return Instance.Logs[Name];
        }

        public static ILog GetLog(Type LogType)
        {
            return GetLog(LogType.Name);
        }

        public static ILog GetLog(object Instance)
        {
            return GetLog(Instance.GetType());
        }

        public static ILog GetLog<T>()
        {
            return GetLog(typeof(T));
        }

        private static void CallOnWrite(ILog Log, string Message, LogType Type)
        {
            if (OnWrite != null)
            {
                LogWriteEventArgs Args = new LogWriteEventArgs(Log, Message, Type);
                OnWrite(Log, Args);
            }
        }

        private class Log : ILog
        {
            public string Name { get; }

            public Log(string Name)
            {
                this.Name = Name;
            }

            public void LogHex(string Message, byte[] Array)
            {
                Message = Message + Environment.NewLine + Utils.HexDump(Array);

                CallOnWrite(this, Message, LogType.Hex);

                using (var fileStream = new FileStream(logPath, FileMode.Append))
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(DateTime.Now.ToString() + " " + Message);
            }

            public void LogInfo(string Message, params object[] Args)
            {
                Message = string.Format(Message, Args);
                CallOnWrite(this, Message, LogType.Information);

                using (var fileStream = new FileStream(logPath, FileMode.Append))
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(DateTime.Now.ToString() + " " + Message);
            }

            public void LogSuccess(string Message, params object[] Args)
            {
                Message = string.Format(Message, Args);
                CallOnWrite(this, Message, LogType.Success);

                using (var fileStream = new FileStream(logPath, FileMode.Append))
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(DateTime.Now.ToString() + " " + Message);
            }

            public void LogWarning(string Message, params object[] Args)
            {
                Message = string.Format(Message, Args);
                CallOnWrite(this, Message, LogType.Warning);

                using (var fileStream = new FileStream(logPath, FileMode.Append))
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(DateTime.Now.ToString() + " " + Message);
            }

            public void LogError(string Message, params object[] Args)
            {
                Message = string.Format(Message, Args);
                CallOnWrite(this, Message, LogType.Error);

                using (var fileStream = new FileStream(logPath, FileMode.Append))
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(DateTime.Now.ToString() + " " + Message);
            }

            public void LogFatal(Exception e)
            {
                string Message = string.Format("Name: {1}{0}Message: {2}{0}Stack trace: {3}", Environment.NewLine, e.GetType().Name, e.Message, e.StackTrace);
                CallOnWrite(this, Message, LogType.Fatal);

                using (var fileStream = new FileStream(logPath, FileMode.Append))
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(DateTime.Now.ToString() + " " + e);
            }

        }
    }
}