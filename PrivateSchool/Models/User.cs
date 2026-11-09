using System;
using System.Text.RegularExpressions;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Користувач системи (працівник адміністрації).
    /// </summary>
    [Serializable]
    public class User : EntityBase
    {
        private static readonly Regex LoginRegex = new Regex(@"^[a-zA-Z0-9_.]{3,30}$");

        private string _login = string.Empty;
        private string _fullName = string.Empty;

        public string Login
        {
            get { return _login; }
            set { _login = Clean(value).ToLowerInvariant(); }
        }

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = Clean(value); }
        }

        /// <summary>Хеш пароля (SHA-256). Сам пароль ніде не зберігається.</summary>
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Manager;

        public bool IsAdministrator
        {
            get { return Role == UserRole.Administrator; }
        }

        public override void Validate()
        {
            Require(LoginRegex.IsMatch(Login),
                "Логін має містити 3–30 символів: латинські літери, цифри, «_» або «.».");
            Require(FullName.Length > 0, "Вкажіть ПІБ користувача.");
            Require(!string.IsNullOrEmpty(PasswordHash), "Пароль не задано.");
        }

        public override bool Matches(string query)
        {
            return string.IsNullOrWhiteSpace(query) || Contains(Login, query) || Contains(FullName, query);
        }

        public override string ToString()
        {
            return FullName + " (" + Login + ")";
        }
    }
}
