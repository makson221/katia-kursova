namespace PrivateSchool.Core.Models;

/// <summary>
/// Перерахунок 100-бальної оцінки у шкалу ЄКТС та національну шкалу.
/// </summary>
public static class GradeScale
{
    public const int MinScore = 0;
    public const int MaxScore = 100;
    public const int PassScore = 60;

    public static string ToEcts(int score) => score switch
    {
        >= 90 => "A",
        >= 82 => "B",
        >= 74 => "C",
        >= 64 => "D",
        >= 60 => "E",
        >= 35 => "FX",
        _ => "F"
    };

    public static int ToNational(int score) => score switch
    {
        >= 90 => 5,
        >= 74 => 4,
        >= 60 => 3,
        _ => 2
    };

    public static string ToNationalText(int score) => ToNational(score) switch
    {
        5 => "відмінно",
        4 => "добре",
        3 => "задовільно",
        _ => "незадовільно"
    };

    public static bool IsPassed(int score) => score >= PassScore;
}
