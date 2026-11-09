using System;
using System.Xml.Serialization;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Викладач навчального закладу.
    /// </summary>
    [Serializable]
    public class Teacher : Person
    {
        /// <summary>Середня кількість навчальних тижнів у місяці.</summary>
        public const double WeeksPerMonth = 4.33;

        private string _specialization = string.Empty;
        private string _education = string.Empty;

        public string Specialization
        {
            get { return _specialization; }
            set { _specialization = Clean(value); }
        }

        public string Education
        {
            get { return _education; }
            set { _education = Clean(value); }
        }

        public TeacherCategory Category { get; set; } = TeacherCategory.Specialist;

        public int ExperienceYears { get; set; }

        /// <summary>Погодинна ставка, грн.</summary>
        public decimal HourlyRate { get; set; } = 250m;

        [XmlElement(DataType = "date")]
        public DateTime HireDate { get; set; } = DateTime.Today;

        public override string RoleName
        {
            get { return "Викладач"; }
        }

        protected override int MinAge
        {
            get { return 18; }
        }

        protected override int MaxAge
        {
            get { return 80; }
        }

        /// <summary>Коефіцієнт надбавки до ставки залежно від кваліфікаційної категорії.</summary>
        public decimal CategoryCoefficient
        {
            get
            {
                switch (Category)
                {
                    case TeacherCategory.Second: return 1.10m;
                    case TeacherCategory.First: return 1.20m;
                    case TeacherCategory.Highest: return 1.35m;
                    default: return 1.00m;
                }
            }
        }

        /// <summary>Орієнтовна місячна заробітна плата за тижневим навантаженням.</summary>
        public decimal CalculateMonthlySalary(double weeklyHours)
        {
            decimal hours = (decimal)(weeklyHours * WeeksPerMonth);
            return Math.Round(hours * HourlyRate * CategoryCoefficient, 2);
        }

        public override void Validate()
        {
            base.Validate();
            Require(Specialization.Length > 0, "Вкажіть спеціалізацію (предмет) викладача.");
            Require(ExperienceYears >= 0 && ExperienceYears <= Age - 16,
                "Стаж роботи не може перевищувати вік мінус 16 років.");
            Require(HourlyRate > 0, "Погодинна ставка має бути більшою за нуль.");
            Require(HireDate <= DateTime.Today, "Дата прийняття на роботу не може бути в майбутньому.");
        }

        public override string GetInfo()
        {
            return base.GetInfo() + ", " + Specialization;
        }

        public override bool Matches(string query)
        {
            return base.Matches(query) || Contains(Specialization, query);
        }
    }
}
