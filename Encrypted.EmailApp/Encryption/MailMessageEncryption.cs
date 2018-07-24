namespace Encrypted.EmailApp.Encryption
{
    public static class MailMessageEncryption
    {
        public static string EncryptText(string text, string passwordPhrase)
        {
            var aesEncrypted = AesEncrypt.EncryptText(text, passwordPhrase);
            var desEncrypted = DesEncrypt.EncryptString(aesEncrypted, passwordPhrase);

            return desEncrypted;
        }

        public static string DecryptText(string text, string passwordPhrase)
        {
            var desDecrypted = DesEncrypt.DecryptString(text, passwordPhrase);

            //Remove pad bytes...
            desDecrypted = desDecrypted.Replace("\0", string.Empty);

            var aesDecrypted = AesEncrypt.DecryptText(desDecrypted, passwordPhrase);

            return aesDecrypted;
        }
    }
}