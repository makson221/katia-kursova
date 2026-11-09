using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Абстрактний клас «Особа» — спільний предок для учнів та викладачів.
    /// Інкапсулює персональні дані та правила їх перевірки.
    /// </summary>
    [Serializable]
    public abstract class Person : EntityBase
    {
        private static readonly Regex PhoneRegex = new Regex(@"^\+?[0-9\-\s\(\)]{10,18}$");
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        private string _lastName = string.Empty;
        private string _firstName = string.Empty;
        private string _middleName = string.Empty;
        private string _phone = string.Empty;
        private string _email = string.Empty;

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = Clean(value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = Clean(value); }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = Clean(value); }
        }

        [XmlElement(DataType = "date")]
        public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-10);

        public string Phone
        {
            get { return _phone; }
            set { _phone = Clean(value); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = Clean(value); }
        }

        /// <summary>Повне ім'я (прізвище, ім'я, по батькові).</summary>
        public string FullName
        {
            get { return (LastName + " " + FirstName + " " + MiddleName).Trim(); }
        }

        /// <summary>Скорочене ім'я: «Петренко І. В.».</summary>
        public string ShortName
        {
            get { return (LastName + " " + Initial(FirstName) + " " + Initial(MiddleName)).Trim(); }
        }

        /// <summary>Повна кількість років на поточну дату.</summary>
        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age))
                    age--;
                return age;
            }
        }

        /// <summary>Назва ролі особи в навчальному закладі (визначається у нащадках).</summary>
        public abstract string RoleName { get; }

        /// <summary>Мінімально допустимий вік для конкретного типу особи.</summary>
        protected abstract int MinAge { get; }

        protected virtual int MaxAge
        {
            get { return 100; }
        }

        public override void Validate()
        {
            Require(LastName.Length > 0, "Вкажіть прізвище.");
            Require(FirstName.Length > 0, "Вкажіть ім'я.");
            Require(BirthDate < DateTime.Today, "Дата народження має бути в минулому.");
            Require(Age >= MinAge && Age <= MaxAge,
                string.Format("{0}: вік має бути в межах від {1} до {2} років.", RoleName, MinAge, MaxAge));
            Require(Phone.Length == 0 || PhoneRegex.IsMatch(Phone),
                "Номер телефону має некоректний формат (приклад: +380501234567).");
            Require(Email.Length == 0 || EmailRegex.IsMatch(Email), "Електронна пошта має некоректний формат.");
        }

        /// <summary>Коротка інформація про особу; нащадки доповнюють її власними даними.</summary>
        public virtual string GetInfo()
        {
            return string.Format("{0}: {1}, {2} р.", RoleName, FullName, Age);
        }

        public override bool Matches(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return true;
            return Contains(FullName, query) || Contains(Phone, query) || Contains(Email, query);
        }

        public override string ToString()
        {
            return FullName;
        }

        private static string Initial(string name)
        {
            return string.IsNullOrEmpty(name) ? string.Empty : name.Substring(0, 1) + ".";
        }
    }
}
