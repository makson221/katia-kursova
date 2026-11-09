using System.Text.Json.Serialization;

namespace PrivateSchool.Core.Models;

/// <summary>
/// Викладач закладу.
/// </summary>
public class Teacher : Person
{
    public string Position { get; set; } = "Викладач";
    public string AcademicDegree { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public DateTime HireDate { get; set; } = DateTime.Today;

    /// <summary>Ставка оплати за академічну годину, грн.</summary>
    public decimal HourlyRate { get; set; }

    [JsonIgnore]
    public override string RoleName => "Викладач";

    [JsonIgnore]
    public int ExperienceYears => Math.Max(0, (int)((DateTime.Today - HireDate).TotalDays / 365.25));

    protected override int MinAge => 18;

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (string.IsNullOrWhiteSpace(Position)) yield return "Вкажіть посаду.";
        if (HourlyRate < 0) yield return "Ставка не може бути від'ємною.";
        if (HireDate.Date > DateTime.Today) yield return "Дата прийому на роботу не може бути в майбутньому.";
        if (HireDate.Date < BirthDate.AddYears(16).Date) yield return "Дата прийому на роботу некоректна відносно дати народження.";
    }
}
