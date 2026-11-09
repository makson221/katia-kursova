namespace PrivateSchool.Core.Models;

/// <summary>
/// Академічна група (клас) — об'єднує студентів однієї спеціальності та року вступу.
/// </summary>
public class Group : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public int StartYear { get; set; } = DateTime.Today.Year;

    /// <summary>Вартість навчання за місяць, грн.</summary>
    public decimal MonthlyFee { get; set; }

    public int Capacity { get; set; } = 25;

    /// <summary>Куратор групи (викладач), необов'язково.</summary>
    public int? CuratorId { get; set; }

    /// <summary>Поточний курс навчання.</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public int CourseNumber
    {
        get
        {
            var today = DateTime.Today;
            var academicYear = today.Month >= 9 ? today.Year : today.Year - 1;
            return Math.Max(1, academicYear - StartYear + 1);
        }
    }

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (string.IsNullOrWhiteSpace(Name)) yield return "Вкажіть назву групи.";
        else if (Name.Length > 20) yield return "Назва групи не повинна перевищувати 20 символів.";
        if (string.IsNullOrWhiteSpace(Specialty)) yield return "Вкажіть спеціальність.";
        if (StartYear < 1990 || StartYear > DateTime.Today.Year + 1) yield return "Некоректний рік набору.";
        if (MonthlyFee < 0) yield return "Вартість навчання не може бути від'ємною.";
        if (Capacity is < 1 or > 100) yield return "Місткість групи має бути від 1 до 100.";
    }

    public override string ToString() => Name;
}
