using System.Configuration;

namespace Encrypted.EmailApp.Utils
{
    public static class Configuration
    {
        public static string EncryptionKeySalt => ConfigurationManager.AppSettings[nameof(EncryptionKeySalt)];
    }
}