using System.Security.Cryptography;
using System.Text;

namespace Encrypted.EmailApp.Utils
{
    public static class Extensions
    {
        public static string Truncate(this string value, int maxChars)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxChars
                ? value
                : value.Substring(0, maxChars) + "...";
        }

        public static string GetUniqueKey(int maxSize)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];

            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }

            var result = new StringBuilder(maxSize);
            foreach (var b in data)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }
    }
}