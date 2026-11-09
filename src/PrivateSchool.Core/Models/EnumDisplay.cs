using System.ComponentModel;
using System.Reflection;

namespace PrivateSchool.Core.Models;

/// <summary>
/// Пара "значення — текст" для відображення переліків у випадаючих списках.
/// </summary>
public sealed record EnumItem<T>(T Value, string Text) where T : struct, Enum
{
    public override string ToString() => Text;
}

/// <summary>
/// Допоміжні методи для локалізованого відображення значень переліків через [Description].
/// </summary>
public static class EnumDisplay
{
    public static string ToDisplay(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }

    public static IReadOnlyList<EnumItem<T>> GetItems<T>() where T : struct, Enum =>
        Enum.GetValues<T>().Select(v => new EnumItem<T>(v, v.ToDisplay())).ToList();
}
