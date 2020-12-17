using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread thread = new Thread(funcHesh);
            Thread threadTwo = new Thread(funcHesh);
            Thread threadThree = new Thread(funcHesh);
            Thread threadFour = new Thread(funcHesh);
            thread.Start();
        }
        static string cHash(string rawData)
        {
            using (SHA256 shHash = SHA256.Create())
            {
                byte[] bytes = shHash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        static void funcHesh()
        {
            int count = 0;
            string Chars = "abcdefghijklmnopqrstuvwxyz";
            foreach (var Password in getComb(Chars.ToArray(), 5))
            {
                count++;
                Console.WriteLine("Номер провереной комбинации:" + count + "; \tпароль:" + Password);
                if (cHash(Convert.ToString(Password)) == "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b")
                {
                    Console.WriteLine("Пароль расшифрован номер: " + count + " пароль: " + Password);
                    break;
                }
            }
        }
        private static IEnumerable<string> getComb(char[] Chars, int mLen)
        {
            if (mLen < 1) yield break;
            foreach (var a in Chars)
            {
                yield return a.ToString();
                foreach (var b in getComb(Chars, mLen - 1)) yield return a + b;
            }
        }
    }
}