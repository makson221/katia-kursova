using System;
using System.Xml.Serialization;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Навчальна група: курс + викладач + перелік учнів.
    /// </summary>
    [Serializable]
    public class Group : EntityBase
    {
        private string _name = string.Empty;
        private string _room = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = Clean(value); }
        }

        public int CourseId { get; set; }

        public int TeacherId { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        public int MaxStudents { get; set; } = 12;

        /// <summary>Аудиторія за замовчуванням.</summary>
        public string Room
        {
            get { return _room; }
            set { _room = Clean(value); }
        }

        public override void Validate()
        {
            Require(Name.Length > 0, "Вкажіть назву групи.");
            Require(CourseId > 0, "Оберіть курс для групи.");
            Require(TeacherId > 0, "Оберіть викладача для групи.");
            Require(MaxStudents >= 1 && MaxStudents <= 40, "Кількість місць у групі — від 1 до 40.");
        }

        public override bool Matches(string query)
        {
            return string.IsNullOrWhiteSpace(query) || Contains(Name, query) || Contains(Room, query);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
