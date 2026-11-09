using System.Text.Json.Serialization;

namespace PrivateSchool.Core.Models;

/// <summary>
/// Здобувач освіти (студент/учень) приватного закладу, що навчається за договором.
/// </summary>
public class Student : Person
{
    public int GroupId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; } = DateTime.Today;

    /// <summary>Дата завершення навчання (відрахування/випуску). Null — навчається.</summary>
    public DateTime? EndDate { get; set; }

    public StudentStatus Status { get; set; } = StudentStatus.Active;

    /// <summary>Знижка на навчання у відсотках (0–100).</summary>
    public decimal DiscountPercent { get; set; }

    public string ParentName { get; set; } = string.Empty;
    public string ParentPhone { get; set; } = string.Empty;

    [JsonIgnore]
    public override string RoleName => "Студент";

    [JsonIgnore]
    public bool IsStudying => Status is StudentStatus.Active or StudentStatus.AcademicLeave;

    protected override int MinAge => 6;

    public override IEnumerable<string> Validate()
    {
        foreach (var error in base.Validate()) yield return error;

        if (GroupId <= 0) yield return "Оберіть групу.";
        if (string.IsNullOrWhiteSpace(ContractNumber)) yield return "Вкажіть номер договору.";
        if (DiscountPercent is < 0 or > 100) yield return "Знижка має бути в межах 0–100 %.";
        if (EnrollmentDate.Date > DateTime.Today.AddMonths(6)) yield return "Дата зарахування занадто далеко в майбутньому.";
        if (EnrollmentDate.Date < BirthDate.Date) yield return "Дата зарахування не може бути раніше дати народження.";
        if (EndDate.HasValue && EndDate.Value.Date < EnrollmentDate.Date)
            yield return "Дата завершення навчання не може бути раніше дати зарахування.";
        if (!IsStudying && !EndDate.HasValue)
            yield return "Для відрахованого/випускника вкажіть дату завершення навчання.";
        if (ParentPhone.Length > 0 && !IsValidPhone(ParentPhone))
            yield return "Телефон батьків має бути у форматі +380XXXXXXXXX.";
    }
}
