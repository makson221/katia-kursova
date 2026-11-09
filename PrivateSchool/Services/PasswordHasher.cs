using System.Security.Cryptography;
using System.Text;

namespace PrivateSchool.Services
{
    /// <summary>
    /// Хешування паролів алгоритмом SHA-256 із «сіллю».
    /// </summary>
    public static class PasswordHasher
    {
        private const string Salt = "PrivateSchool#2026";

        public static string Hash(string login, string password)
        {
            string input = Salt + ":" + (login ?? string.Empty).Trim().ToLowerInvariant() + ":" + (password ?? string.Empty);
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static bool Verify(string login, string password, string hash)
        {
            return Hash(login, password) == hash;
        }
    }
}
