using System;
using System.Xml.Serialization;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Заняття в розкладі (повторюється щотижня).
    /// </summary>
    [Serializable]
    public class Lesson : EntityBase
    {
        private string _room = string.Empty;

        public int GroupId { get; set; }

        public DayOfWeek Day { get; set; } = DayOfWeek.Monday;

        /// <summary>Час початку заняття. TimeSpan не серіалізується XmlSerializer-ом,
        /// тому у файл записується рядкове представлення (властивість StartTimeText).</summary>
        [XmlIgnore]
        public TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0);

        [XmlElement("StartTime")]
        public string StartTimeText
        {
            get { return StartTime.ToString(@"hh\:mm"); }
            set { StartTime = TimeSpan.Parse(value); }
        }

        public int DurationMinutes { get; set; } = 90;

        public string Room
        {
            get { return _room; }
            set { _room = Clean(value); }
        }

        public TimeSpan EndTime
        {
            get { return StartTime.Add(TimeSpan.FromMinutes(DurationMinutes)); }
        }

        public string TimeRange
        {
            get { return StartTime.ToString(@"hh\:mm") + "–" + EndTime.ToString(@"hh\:mm"); }
        }

        /// <summary>Чи перетинається заняття в часі з іншим заняттям.</summary>
        public bool OverlapsWith(Lesson other)
        {
            if (other == null || other.Day != Day)
                return false;
            return StartTime < other.EndTime && other.StartTime < EndTime;
        }

        public override void Validate()
        {
            Require(GroupId > 0, "Оберіть групу.");
            Require(DurationMinutes >= 30 && DurationMinutes <= 240, "Тривалість заняття — від 30 до 240 хвилин.");
            Require(StartTime >= new TimeSpan(7, 0, 0) && EndTime <= new TimeSpan(22, 0, 0),
                "Заняття мають проходити з 07:00 до 22:00.");
            Require(Room.Length > 0, "Вкажіть аудиторію.");
        }
    }
}
