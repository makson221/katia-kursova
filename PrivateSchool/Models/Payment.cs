using System;
using System.Xml.Serialization;

namespace PrivateSchool.Models
{
    /// <summary>
    /// Оплата за навчання.
    /// </summary>
    [Serializable]
    public class Payment : EntityBase
    {
        private string _comment = string.Empty;

        public int StudentId { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; } = DateTime.Today;

        public decimal Amount { get; set; }

        public PaymentMethod Method { get; set; } = PaymentMethod.Cash;

        public string Comment
        {
            get { return _comment; }
            set { _comment = Clean(value); }
        }

        public override void Validate()
        {
            Require(StudentId > 0, "Оберіть учня.");
            Require(Amount > 0, "Сума оплати має бути більшою за нуль.");
            Require(Amount <= 1000000m, "Сума оплати занадто велика.");
            Require(Date <= DateTime.Today, "Дата оплати не може бути в майбутньому.");
        }

        public override bool Matches(string query)
        {
            return string.IsNullOrWhiteSpace(query) || Contains(Comment, query);
        }
    }
}
