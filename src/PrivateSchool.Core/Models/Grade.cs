using System.Text.Json.Serialization;

namespace PrivateSchool.Core.Models;

/// <summary>
/// Оцінка студента з дисципліни (100-бальна шкала).
/// </summary>
public class Grade : Entity
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int Score { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public GradeType Type { get; set; } = GradeType.Module;
    public string Comment { get; set; } = string.Empty;

    [JsonIgnore] public string Ects => GradeScale.ToEcts(Score);
    [JsonIgnore] public int National => GradeScale.ToNational(Score);
    [JsonIgnore] public bool IsPassed => GradeScale.IsPassed(Score);

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (StudentId <= 0) yield return "Оберіть студента.";
        if (SubjectId <= 0) yield return "Оберіть дисципліну.";
        if (Score is < GradeScale.MinScore or > GradeScale.MaxScore)
            yield return $"Оцінка має бути в межах {GradeScale.MinScore}–{GradeScale.MaxScore} балів.";
        if (Date.Date > DateTime.Today) yield return "Дата оцінювання не може бути в майбутньому.";
    }
}
