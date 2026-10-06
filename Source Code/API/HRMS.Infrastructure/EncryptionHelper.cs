using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HRMS.Infrastructure
{
    public static class EncryptionHelper
    {
        // 32-byte key for AES-256
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("HRMSNotificationPayloadSecretKey"); 
        // Legacy 16-byte IV for backward compatibility with pre-existing records
        private static readonly byte[] LegacyIv = Encoding.UTF8.GetBytes("HRMSNotifIV12345"); 

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.GenerateIV(); // Cryptographically secure random 16-byte IV per encryption
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            using var ms = new MemoryStream();
            
            // Prepend IV to ciphertext output
            ms.Write(iv, 0, iv.Length);

            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs, Encoding.UTF8))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            try
            {
                byte[] allBytes = Convert.FromBase64String(cipherText);
                if (allBytes.Length < 16) return cipherText;

                // Try decrypting with prepended dynamic IV
                try
                {
                    byte[] iv = new byte[16];
                    Buffer.BlockCopy(allBytes, 0, iv, 0, 16);

                    using var aes = Aes.Create();
                    aes.Key = Key;
                    aes.IV = iv;

                    using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using var ms = new MemoryStream(allBytes, 16, allBytes.Length - 16);
                    using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                    using var sr = new StreamReader(cs, Encoding.UTF8);
                    return sr.ReadToEnd();
                }
                catch
                {
                    // Fallback to legacy format with static LegacyIv
                    using var aes = Aes.Create();
                    aes.Key = Key;
                    aes.IV = LegacyIv;

                    using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using var ms = new MemoryStream(allBytes);
                    using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                    using var sr = new StreamReader(cs, Encoding.UTF8);
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return cipherText;
            }
        }
    }
}
