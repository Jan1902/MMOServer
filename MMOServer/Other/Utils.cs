using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MMOServer.Other
{
    class Utils
    {
        public static string GetHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public static byte[] HashString(string text)
        {
            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
        }
    }
}
