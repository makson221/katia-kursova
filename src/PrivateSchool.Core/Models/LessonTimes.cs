namespace PrivateSchool.Core.Models;

/// <summary>
/// Розклад дзвінків.
/// </summary>
public static class LessonTimes
{
    public const int MaxLessonsPerDay = 6;

    private static readonly (TimeSpan Start, TimeSpan End)[] Bells =
    {
        (new TimeSpan(8, 30, 0), new TimeSpan(9, 50, 0)),
        (new TimeSpan(10, 5, 0), new TimeSpan(11, 25, 0)),
        (new TimeSpan(11, 55, 0), new TimeSpan(13, 15, 0)),
        (new TimeSpan(13, 30, 0), new TimeSpan(14, 50, 0)),
        (new TimeSpan(15, 5, 0), new TimeSpan(16, 25, 0)),
        (new TimeSpan(16, 40, 0), new TimeSpan(18, 0, 0))
    };

    /// <summary>Навчальні дні тижня (пн–сб).</summary>
    public static readonly DayOfWeek[] StudyDays =
    {
        DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
        DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday
    };

    public static string GetTime(int number) =>
        number is >= 1 and <= MaxLessonsPerDay
            ? $"{Bells[number - 1].Start:hh\\:mm}–{Bells[number - 1].End:hh\\:mm}"
            : "—";

    public static string DayName(DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => "Понеділок",
        DayOfWeek.Tuesday => "Вівторок",
        DayOfWeek.Wednesday => "Середа",
        DayOfWeek.Thursday => "Четвер",
        DayOfWeek.Friday => "П'ятниця",
        DayOfWeek.Saturday => "Субота",
        _ => "Неділя"
    };
}
