using System;
using System.Xml.Serialization;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Учень приватного навчального закладу.
    /// </summary>
    [Serializable]
    public class Student : Person
    {
        private string _contractNumber = string.Empty;
        private string _parentName = string.Empty;
        private string _parentPhone = string.Empty;

        /// <summary>Номер договору про надання освітніх послуг.</summary>
        public string ContractNumber
        {
            get { return _contractNumber; }
            set { _contractNumber = Clean(value); }
        }

        [XmlElement(DataType = "date")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Today;

        /// <summary>Ідентифікатор групи (0 — учня ще не зараховано до групи).</summary>
        public int GroupId { get; set; }

        public StudentStatus Status { get; set; } = StudentStatus.Active;

        /// <summary>Знижка на навчання у відсотках (0–100).</summary>
        public decimal Discount { get; set; }

        public string ParentName
        {
            get { return _parentName; }
            set { _parentName = Clean(value); }
        }

        public string ParentPhone
        {
            get { return _parentPhone; }
            set { _parentPhone = Clean(value); }
        }

        public override string RoleName
        {
            get { return "Учень"; }
        }

        protected override int MinAge
        {
            get { return 5; }
        }

        public bool IsActive
        {
            get { return Status == StudentStatus.Active; }
        }

        /// <summary>Розраховує щомісячну оплату з урахуванням знижки.</summary>
        public decimal CalculateMonthlyFee(Course course)
        {
            if (course == null)
                return 0m;
            return Math.Round(course.PricePerMonth * (100m - Discount) / 100m, 2);
        }

        public override void Validate()
        {
            base.Validate();
            Require(ContractNumber.Length > 0, "Вкажіть номер договору.");
            Require(Discount >= 0 && Discount <= 100, "Знижка має бути в межах від 0 до 100 %.");
            Require(EnrollmentDate > BirthDate, "Дата зарахування не може бути раніше дати народження.");
            Require(EnrollmentDate <= DateTime.Today.AddMonths(3),
                "Дата зарахування не може бути більш ніж на 3 місяці в майбутньому.");
            // Для неповнолітніх обов'язково потрібні контакти батьків
            if (Age < 18)
                Require(ParentName.Length > 0, "Для неповнолітнього учня вкажіть ПІБ одного з батьків.");
        }

        public override string GetInfo()
        {
            return base.GetInfo() + ", договір № " + ContractNumber;
        }

        public override bool Matches(string query)
        {
            return base.Matches(query) || Contains(ContractNumber, query) || Contains(ParentName, query);
        }
    }
}
