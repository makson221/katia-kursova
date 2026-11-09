namespace PrivateSchool.Core.Models;

/// <summary>
/// Заняття у тижневому розкладі.
/// </summary>
public class Lesson : Entity
{
    public int GroupId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public DayOfWeek Day { get; set; } = DayOfWeek.Monday;

    /// <summary>Номер пари (1–6).</summary>
    public int Number { get; set; } = 1;

    public string Room { get; set; } = string.Empty;
    public LessonType Type { get; set; } = LessonType.Lecture;

    [System.Text.Json.Serialization.JsonIgnore]
    public string Time => LessonTimes.GetTime(Number);

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (GroupId <= 0) yield return "Оберіть групу.";
        if (SubjectId <= 0) yield return "Оберіть дисципліну.";
        if (TeacherId <= 0) yield return "Оберіть викладача.";
        if (Number is < 1 or > LessonTimes.MaxLessonsPerDay)
            yield return $"Номер пари має бути від 1 до {LessonTimes.MaxLessonsPerDay}.";
        if (Day == DayOfWeek.Sunday) yield return "Неділя — вихідний день.";
        if (string.IsNullOrWhiteSpace(Room)) yield return "Вкажіть аудиторію.";
    }
}
