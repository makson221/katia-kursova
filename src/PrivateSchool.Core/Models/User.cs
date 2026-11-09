using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PrivateSchool.Core.Models;

/// <summary>
/// Обліковий запис користувача системи.
/// </summary>
public partial class User : Entity
{
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Manager;

    /// <summary>Зв'язок з викладачем (для ролі "Викладач").</summary>
    public int? TeacherId { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime? LastLogin { get; set; }

    [JsonIgnore]
    public string RoleName => Role switch
    {
        UserRole.Administrator => "Адміністратор",
        UserRole.Manager => "Менеджер",
        _ => "Викладач"
    };

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (!LoginRegex().IsMatch(Login)) yield return "Логін: 3–20 символів (латинські літери, цифри, '_', '.').";
        if (string.IsNullOrWhiteSpace(DisplayName)) yield return "Вкажіть ім'я користувача.";
        if (string.IsNullOrEmpty(PasswordHash)) yield return "Пароль не задано.";
        if (Role == UserRole.Teacher && TeacherId is null) yield return "Для ролі \"Викладач\" оберіть викладача.";
    }

    public override string ToString() => $"{DisplayName} ({Login})";

    [GeneratedRegex(@"^[A-Za-z0-9_.]{3,20}$")]
    private static partial Regex LoginRegex();
}
