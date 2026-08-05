using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HRMS.Infrastructure
{
    public static class EncryptionHelper
    {
        // Must be exactly 32 bytes for AES-256
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("HRMSNotificationPayloadSecretKey"); 
        // Must be exactly 16 bytes for AES block size
        private static readonly byte[] Iv = Encoding.UTF8.GetBytes("HRMSNotifIV12345"); 


        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = Iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            try
            {
                using var aes = Aes.Create();
                aes.Key = Key;
                aes.IV = Iv;

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch
            {
                // Return plainText in case it was not encrypted
                return cipherText;
            }
        }
    }
}
