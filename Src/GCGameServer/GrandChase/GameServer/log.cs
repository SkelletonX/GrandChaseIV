using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GameServer
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

        public static string HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return "<null>";
            int bytesLength = bytes.Length;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            return result.ToString();
        }
        public void Hex(string text, byte[] data, int tipo)
        {
            string reader = HexDump(data);
            if (tipo == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(jmp + "-" + text);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(reader);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(jmp + "-" + text);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(reader);
                Console.ResetColor();
            }
        }

    }
}
