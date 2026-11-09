using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PrivateSchool.Core.Models;

/// <summary>
/// Абстрактна особа — спільний предок студента та викладача (наслідування).
/// </summary>
public abstract partial class Person : Entity
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-18);
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [JsonIgnore]
    public string FullName => string.Join(' ', new[] { LastName, FirstName, MiddleName }
        .Where(s => !string.IsNullOrWhiteSpace(s)));

    /// <summary>Прізвище та ініціали, напр. "Шевченко Т. Г.".</summary>
    [JsonIgnore]
    public string ShortName
    {
        get
        {
            var result = LastName;
            if (FirstName.Length > 0) result += $" {FirstName[0]}.";
            if (MiddleName.Length > 0) result += $" {MiddleName[0]}.";
            return result;
        }
    }

    [JsonIgnore]
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate.Year;
            if (BirthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    /// <summary>Назва ролі особи у закладі (абстрактна властивість — поліморфізм).</summary>
    [JsonIgnore]
    public abstract string RoleName { get; }

    /// <summary>Мінімальний допустимий вік для конкретного типу особи.</summary>
    protected virtual int MinAge => 5;

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (!IsValidName(LastName)) yield return "Прізвище обов'язкове і може містити лише літери, апостроф та дефіс.";
        if (!IsValidName(FirstName)) yield return "Ім'я обов'язкове і може містити лише літери, апостроф та дефіс.";
        if (MiddleName.Length > 0 && !IsValidName(MiddleName)) yield return "По батькові може містити лише літери, апостроф та дефіс.";

        if (BirthDate > DateTime.Today) yield return "Дата народження не може бути в майбутньому.";
        else if (Age < MinAge) yield return $"Вік має бути не менше {MinAge} років.";
        else if (Age > 100) yield return "Перевірте дату народження (вік понад 100 років).";

        if (Phone.Length > 0 && !IsValidPhone(Phone)) yield return "Телефон має бути у форматі +380XXXXXXXXX.";
        if (Email.Length > 0 && !IsValidEmail(Email)) yield return "Некоректна адреса електронної пошти.";
    }

    public override string ToString() => FullName;

    public static bool IsValidName(string value) =>
        !string.IsNullOrWhiteSpace(value) && NameRegex().IsMatch(value.Trim());

    public static bool IsValidPhone(string value) => PhoneRegex().IsMatch(value.Replace(" ", "").Replace("-", ""));

    public static bool IsValidEmail(string value) => EmailRegex().IsMatch(value.Trim());

    [GeneratedRegex(@"^[\p{L}][\p{L}'ʼ’\- ]{0,49}$")]
    private static partial Regex NameRegex();

    [GeneratedRegex(@"^\+?\d{10,13}$")]
    private static partial Regex PhoneRegex();

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
