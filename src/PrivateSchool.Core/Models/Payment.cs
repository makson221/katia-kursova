namespace PrivateSchool.Core.Models;

/// <summary>
/// Оплата за навчання, внесена за договором студента.
/// </summary>
public class Payment : Entity
{
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public PaymentMethod Method { get; set; } = PaymentMethod.Card;
    public string Purpose { get; set; } = "Оплата за навчання";
    public string ReceiptNumber { get; set; } = string.Empty;

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (StudentId <= 0) yield return "Оберіть студента.";
        if (Amount <= 0) yield return "Сума оплати має бути більшою за нуль.";
        if (Amount > 1_000_000) yield return "Сума оплати перевищує допустиму (1 000 000 грн).";
        if (Date.Date > DateTime.Today) yield return "Дата оплати не може бути в майбутньому.";
        if (string.IsNullOrWhiteSpace(ReceiptNumber)) yield return "Вкажіть номер квитанції.";
    }
}
