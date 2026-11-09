using System;
using System.Xml.Serialization;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Оцінка учня за 12-бальною шкалою.
    /// </summary>
    [Serializable]
    public class Grade : EntityBase
    {
        public const int MinValue = 1;
        public const int MaxValue = 12;

        private string _comment = string.Empty;

        public int StudentId { get; set; }

        public int GroupId { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; } = DateTime.Today;

        public int Value { get; set; } = 10;

        public GradeKind Kind { get; set; } = GradeKind.Current;

        public string Comment
        {
            get { return _comment; }
            set { _comment = Clean(value); }
        }

        /// <summary>Рівень навчальних досягнень за шкалою МОН України.</summary>
        public string LevelName
        {
            get { return GetLevelName(Value); }
        }

        public static string GetLevelName(double value)
        {
            if (value >= 10) return "Високий";
            if (value >= 7) return "Достатній";
            if (value >= 4) return "Середній";
            return "Початковий";
        }

        public override void Validate()
        {
            Require(StudentId > 0, "Оберіть учня.");
            Require(GroupId > 0, "Оберіть групу.");
            Require(Value >= MinValue && Value <= MaxValue,
                string.Format("Оцінка має бути в межах від {0} до {1}.", MinValue, MaxValue));
            Require(Date <= DateTime.Today, "Дата оцінки не може бути в майбутньому.");
        }

        public override bool Matches(string query)
        {
            return string.IsNullOrWhiteSpace(query) || Contains(Comment, query);
        }
    }
}
