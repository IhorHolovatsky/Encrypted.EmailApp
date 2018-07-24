using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Encrypted.EmailApp.Utils;

namespace Encrypted.EmailApp.Encryption
{
    public static class DesEncrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly string INIT_VECTOR = "uniquevector1234";
        private static readonly byte[] SALT = Encoding.UTF8.GetBytes(Configuration.EncryptionKeySalt);
        // This constant is used to determine the keysize of the encryption algorithm
        private const int KEY_SIZE = 256;

        public static string EncryptString(string plainText, string passPhrase)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var keyBytes = new Rfc2898DeriveBytes(passPhrase, SALT)
                .GetBytes(KEY_SIZE / 8);

            return EncryptString(plainTextBytes, keyBytes);
        }

        public static string EncryptString(byte[] plainTextBytes, string passPhrase)
        {
            var keyBytes = new Rfc2898DeriveBytes(passPhrase, SALT)
                .GetBytes(KEY_SIZE / 8);

            return EncryptString(plainTextBytes, keyBytes);
        }


        public static string EncryptString(byte[] plainTextBytes, byte[] keyBytes)
        {
            var initVectorBytes = Encoding.UTF8.GetBytes(INIT_VECTOR);

            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                var cipherTextBytes = memoryStream.ToArray();

                return Convert.ToBase64String(cipherTextBytes);
            }
        }


        public static byte[] DecryptStringReturnBytes(string cipherText, string passPhrase)
        {
            var cipherTextBytes = Convert.FromBase64String(cipherText);

            var initVectorBytes = Encoding.UTF8.GetBytes(INIT_VECTOR);

            var keyBytes = new Rfc2898DeriveBytes(passPhrase, SALT)
                .GetBytes(KEY_SIZE / 8);

            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            using (var memoryStream = new MemoryStream(cipherTextBytes))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                var plainTextBytes = new byte[cipherTextBytes.Length];
                cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                return plainTextBytes;
            }
        }

        public static string DecryptString(string cipherText, string passPhrase)
        {
            return Encoding.UTF8.GetString(DecryptStringReturnBytes(cipherText, passPhrase));
        }
    }
}