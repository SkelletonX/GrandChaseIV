using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CenterServer
{
    public class Log
    {
        public string LUA_VERSION = "Lua 5.1";
        public string LUA_RELEASE = "Lua 5.1.4";
        public int LUA_VERSION_NUM = 501;
        public string LUA_COPYRIGHT = "Copyright (C) 1994-2008 Lua.org, PUC-Rio";
        public string LUA_AUTHORS = "R. Ierusalimschy, L. H. de Figueiredo & W. Celes";
        string jmp = "\n";
        public void title(string text)
        {
            Console.WindowHeight = 30;
            Console.ForegroundColor = ConsoleColor.Yellow;
            int len = 20;
            string temp = "";
            for (int a = 0; a < len; a++ )
            {
                temp = temp + " ";
            }
            Console.WriteLine(temp+"[ " + text + " ]" + jmp);
            Console.Title = text;
            Console.ResetColor();
        }
        public void Write(string text, params object[] arg0)
        {
            Console.WriteLine(text, arg0);
        }
        public void Error(string text, params object[] arg0)
        {
            Console.WriteLine(jmp);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" "+text,arg0);
            Console.ResetColor();
            Console.WriteLine(jmp);
        }
        public void Warn(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" " + text);
            Console.ResetColor();
        }
        public void Info(string text)
        {            
            Console.WriteLine(" "+text);
            Console.ResetColor();
        }
        public void Status(string text)
        {
            //Console.ForegroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(jmp+"> "+text+jmp);
            Console.ResetColor();
        }

        public void Hex(string text, byte[] data)
        {
            string reader;            
            reader = BitConverter.ToString(data).Replace("-", " ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(jmp + "-" + text);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(reader);
            Console.ResetColor();
        }

    }
}
