namespace PrivateSchool.Core.Models;

/// <summary>
/// Навчальна дисципліна.
/// </summary>
public class Subject : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>Обсяг дисципліни в академічних годинах.</summary>
    public int Hours { get; set; } = 90;

    public ControlForm ControlForm { get; set; } = ControlForm.Exam;

    /// <summary>Провідний викладач дисципліни.</summary>
    public int? TeacherId { get; set; }

    /// <summary>Кількість кредитів ЄКТС (1 кредит = 30 годин).</summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public double Credits => Math.Round(Hours / 30.0, 1);

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (string.IsNullOrWhiteSpace(Name)) yield return "Вкажіть назву дисципліни.";
        if (Hours is < 1 or > 1000) yield return "Кількість годин має бути від 1 до 1000.";
    }

    public override string ToString() => Name;
}
