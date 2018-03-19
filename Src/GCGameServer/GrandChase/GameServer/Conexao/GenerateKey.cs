using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GCNet.CoreLib;
using System.Security.Cryptography;

namespace GameServer.network
{
    public class Initialize
    {
        public string[] RandomStrings = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "z", "y", "w", "x", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "I", "Y", "W", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        public string[] GenerateIV = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "B", "C", "D", "E", "F" };

        public void Vetor(User user)
        {
            CryptoHandler Crypto = new CryptoHandler();
            AuthHandler Auth = new AuthHandler();

            user.IV = Auth.HmacKey;
            user.Hmac = Crypto.Key;
            Random getKey = new Random();
            string vetor = "", hmac = "";
            for (int i = 0; i < 8; i++)
            {
                vetor = vetor + RandomStrings[getKey.Next(RandomStrings.Length)];                
                hmac = hmac + RandomStrings[getKey.Next(RandomStrings.Length)];
            }
            using (PacketManager Write = new PacketManager())
            {
                Write.OP(1);
                Write.Hex("D7 25");
                Write.Str(vetor);                
                Write.Str(hmac);
                Write.Hex("00 00 00 01 00 00 00 00 00 00 00 00");
                user.Send(Write.ack);
            }
            user.IV = Encoding.GetEncoding(949).GetBytes(vetor);
            user.Hmac = Encoding.GetEncoding(949).GetBytes(hmac);            
        }

        public byte[] decrypt(byte[] data,byte[]Vetor,byte[] hmac)
        {       
            byte[] buffer = new byte[0];            
            Array.Resize(ref buffer, data.Length - 10);
            Array.Copy(data, 0, buffer, 0, data.Length - 10);

            byte[] packet = DESEncryption.DecryptData(buffer, Vetor, hmac);
            Array.Resize(ref buffer,packet.Length-16);
            Array.Copy(packet, 16, buffer, 0, buffer.Length);

            return buffer;
        }

    }
}
