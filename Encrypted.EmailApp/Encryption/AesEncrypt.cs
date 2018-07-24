using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Encrypted.EmailApp.Utils;

namespace Encrypted.EmailApp.Encryption
{
    public static class AesEncrypt
    {
        private static readonly byte[] SALT = Encoding.UTF8.GetBytes(Configuration.EncryptionKeySalt);
        // This constant is used to determine the keysize of the encryption algorithm
        private const int KEY_SIZE = 256;

        public static string DecryptText(string input, string password)
        {
            // Get the bytes of the string
            var bytesToBeDecrypted = Encoding.UTF8.GetBytes(input);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = DecryptBytes(bytesToBeDecrypted, passwordBytes);

            var result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        public static string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = EncryptBytes(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        private static byte[] DecryptBytes(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes;

            using (var ms = new MemoryStream())
            using (var aes = new RijndaelManaged())
            {
                aes.KeySize = KEY_SIZE;
                aes.BlockSize = KEY_SIZE / 2;

                var key = new Rfc2898DeriveBytes(passwordBytes, SALT, 1000);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                aes.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                }

                decryptedBytes = ms.ToArray();
            }

            return decryptedBytes;
        }

        private static byte[] EncryptBytes(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes;

            using (var ms = new MemoryStream())
            using (var aes = new RijndaelManaged())
            {
                aes.KeySize = KEY_SIZE;
                aes.BlockSize = KEY_SIZE / 2;

                var key = new Rfc2898DeriveBytes(passwordBytes, SALT, 1000);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                aes.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }
                encryptedBytes = ms.ToArray();
            }


            return encryptedBytes;
        }
    }
}