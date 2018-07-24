namespace Encrypted.EmailApp.Encryption
{
    public static class MailMessageEncryption
    {
        public static string EncryptText(string text, string passwordPhrase)
        {
            var aesEncrypted = AesEncrypt.EncryptText(text, passwordPhrase);
            //var desEncrypted = DesEncrypt.EncryptString(text, passwordPhrase);

            return aesEncrypted;
        }

        public static string DecryptText(string text, string passwordPhrase)
        {
            //var desDecrypted = DesEncrypt.DecryptString(text, passwordPhrase);
            var aesDecrypted = AesEncrypt.DecryptText(text, passwordPhrase);

            return aesDecrypted;
        }
    }
}