using System.Linq;
using PrivateSchool.Models;

namespace PrivateSchool.Services
{
    /// <summary>
    /// Автентифікація користувачів системи.
    /// </summary>
    public class AuthService
    {
        public const int MaxAttempts = 5;

        private readonly SchoolService _school;
        private int _failedAttempts;

        public AuthService(SchoolService school)
        {
            _school = school;
        }

        public int RemainingAttempts
        {
            get { return MaxAttempts - _failedAttempts; }
        }

        /// <summary>Повертає користувача при успішному вході або null.</summary>
        public User Login(string login, string password)
        {
            if (RemainingAttempts <= 0)
                throw new ValidationException("Перевищено кількість спроб входу. Перезапустіть програму.");

            string normalized = (login ?? string.Empty).Trim().ToLowerInvariant();
            User user = _school.Users.Find(u => u.Login == normalized).FirstOrDefault();
            if (user != null && PasswordHasher.Verify(normalized, password, user.PasswordHash))
            {
                _failedAttempts = 0;
                return user;
            }

            _failedAttempts++;
            return null;
        }
    }
}
