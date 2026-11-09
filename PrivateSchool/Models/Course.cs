using System;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Навчальний курс (освітня програма), за яким навчаються групи.
    /// </summary>
    [Serializable]
    public class Course : EntityBase
    {
        private string _name = string.Empty;
        private string _description = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = Clean(value); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = Clean(value); }
        }

        public CourseLevel Level { get; set; } = CourseLevel.Beginner;

        /// <summary>Вартість навчання за місяць, грн.</summary>
        public decimal PricePerMonth { get; set; }

        /// <summary>Тривалість курсу в місяцях.</summary>
        public int DurationMonths { get; set; } = 9;

        /// <summary>Кількість занять на тиждень.</summary>
        public int LessonsPerWeek { get; set; } = 2;

        /// <summary>Повна вартість курсу.</summary>
        public decimal TotalPrice
        {
            get { return PricePerMonth * DurationMonths; }
        }

        public override void Validate()
        {
            Require(Name.Length > 0, "Вкажіть назву курсу.");
            Require(PricePerMonth > 0, "Вартість навчання має бути більшою за нуль.");
            Require(DurationMonths >= 1 && DurationMonths <= 48, "Тривалість курсу — від 1 до 48 місяців.");
            Require(LessonsPerWeek >= 1 && LessonsPerWeek <= 7, "Кількість занять на тиждень — від 1 до 7.");
        }

        public override bool Matches(string query)
        {
            return string.IsNullOrWhiteSpace(query) || Contains(Name, query) || Contains(Description, query);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
