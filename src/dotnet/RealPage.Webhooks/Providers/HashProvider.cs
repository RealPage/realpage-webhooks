using System.Security.Cryptography;
using System.Text;

namespace RealPage.Webhooks.Providers
{
    public static class HashProvider
    {
        public static string GenerateHash(string secret, string message)
        {
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hash = new HMACSHA256(secretBytes))
            {
                var hashBytes = hash.ComputeHash(messageBytes);
                return ConvertByteToString(hashBytes);
            }
        }

        public static string ConvertByteToString(byte[] hash)
        {
            var result = new StringBuilder();
            foreach (byte t in hash)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString().ToLower();
        }
    }
}
