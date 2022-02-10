using System;
using System.Security.Cryptography;

namespace WiseCat
{

    public class Security
    {
        private static string secret = "4kJp3rhBTdeFVozf+R00RhGI6UEuQREKGL02glRgnJrUB7F1zUVxwyG4yU+79fBG";

        public static string ComputeHash(params string[] messages)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            byte[][] byteMessages = new byte[messages.Length][];
            for (int i = 0; i < messages.Length; i++)
            {
                byteMessages[i] = encoding.GetBytes(messages[i]);
            }

            return ComputeHash(byteMessages);
        }

        public static string ComputeHash(params byte[][] messages)
        {
            // Create the hash
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] key = encoding.GetBytes(secret);
            HMACSHA1 hmacsha1 = new HMACSHA1(key);

            if (messages.Length > 0)
            {
                int i;
                for (i = 0; i < messages.Length - 1; i++)
                {
                    hmacsha1.TransformBlock(messages[i], 0, messages[i].Length, messages[i], 0);
                }
                hmacsha1.TransformFinalBlock(messages[i], 0, messages[i].Length);
            }

            return Convert.ToBase64String(hmacsha1.Hash);
        }
    }
}