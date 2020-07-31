using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoLib
{
    public class Protector
    {
        private static readonly byte[] salt = Encoding.Unicode.GetBytes("salt123456789101112");

        // iterations must be at least 1000, we will use 2000
        private static readonly int iterations = 2000;

        public static string Encrypt(
          string plainText, string password)
        {
            byte[] encryptedBytes;
            byte[] plainBytes = Encoding.Unicode.GetBytes(plainText);
            var aes = Aes.Create();
            var pbkdf2 = new Rfc2898DeriveBytes(
              password, salt, iterations);
            aes.Key = pbkdf2.GetBytes(32); // set a 256-bit key
            aes.IV = pbkdf2.GetBytes(16); // set a 128-bit IV

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(
                  ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                }
                encryptedBytes = ms.ToArray();
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(
          string cryptoText, string password)
        {
            byte[] plainBytes;
            byte[] cryptoBytes = Convert.FromBase64String(cryptoText);
            var aes = Aes.Create();
            var pbkdf2 = new Rfc2898DeriveBytes(
              password, salt, iterations);
            aes.Key = pbkdf2.GetBytes(32);
            aes.IV = pbkdf2.GetBytes(16);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(
                  ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cryptoBytes, 0, cryptoBytes.Length);
                }
                plainBytes = ms.ToArray();
            }

            return Encoding.Unicode.GetString(plainBytes);
        }
    }
}