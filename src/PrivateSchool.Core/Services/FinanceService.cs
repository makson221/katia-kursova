using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Services;

/// <summary>Фінансовий стан студента за договором.</summary>
public sealed record StudentBalance(
    Student Student,
    string GroupName,
    decimal MonthlyFee,
    int BilledMonths,
    decimal Accrued,
    decimal Paid,
    DateTime? LastPaymentDate)
{
    /// <summary>Сальдо: додатне — переплата, від'ємне — заборгованість.</summary>
    public decimal Balance => Paid - Accrued;

    public decimal Debt => Math.Max(0, -Balance);

    public bool HasDebt => Debt > 0;
}

/// <summary>
/// Розрахунок нарахувань, оплат і заборгованостей.
/// Плата нараховується щомісяця з вересня по червень (липень і серпень — канікули).
/// </summary>
public class FinanceService
{
    private readonly SchoolDatabase _db;

    public FinanceService(SchoolDatabase db) => _db = db;

    public static bool IsBillableMonth(int month) => month is not (7 or 8);

    public decimal GetMonthlyFee(Student student)
    {
        var group = _db.Groups.GetById(student.GroupId);
        if (group == null) return 0;
        return Math.Round(group.MonthlyFee * (1 - student.DiscountPercent / 100m), 2);
    }

    /// <summary>Кількість місяців навчання, за які нараховується плата станом на дату.</summary>
    public static int GetBilledMonths(Student student, DateTime asOf)
    {
        var end = student.EndDate.HasValue && student.EndDate.Value < asOf ? student.EndDate.Value : asOf;
        var current = new DateTime(student.EnrollmentDate.Year, student.EnrollmentDate.Month, 1);
        var last = new DateTime(end.Year, end.Month, 1);

        var count = 0;
        while (current <= last)
        {
            if (IsBillableMonth(current.Month)) count++;
            current = current.AddMonths(1);
        }
        return count;
    }

    public decimal GetPaid(int studentId) =>
        _db.Payments.Find(p => p.StudentId == studentId).Sum(p => p.Amount);

    public StudentBalance GetBalance(Student student, DateTime? asOf = null)
    {
        var date = asOf ?? DateTime.Today;
        var fee = GetMonthlyFee(student);
        var months = GetBilledMonths(student, date);
        var payments = _db.Payments.Find(p => p.StudentId == student.Id).ToList();

        return new StudentBalance(
            student,
            _db.Groups.GetById(student.GroupId)?.Name ?? "—",
            fee,
            months,
            fee * months,
            payments.Sum(p => p.Amount),
            payments.Count == 0 ? null : payments.Max(p => p.Date));
    }

    public IReadOnlyList<StudentBalance> GetAllBalances(DateTime? asOf = null) =>
        _db.Students.GetAll().Select(s => GetBalance(s, asOf)).ToList();

    public IReadOnlyList<StudentBalance> GetDebtors(DateTime? asOf = null, int? groupId = null) =>
        GetAllBalances(asOf)
            .Where(b => b.HasDebt && (groupId == null || b.Student.GroupId == groupId))
            .OrderByDescending(b => b.Debt)
            .ToList();

    public decimal GetTotalDebt(DateTime? asOf = null) => GetAllBalances(asOf).Sum(b => b.Debt);

    public decimal GetIncome(DateTime from, DateTime to) =>
        _db.Payments.Find(p => p.Date.Date >= from.Date && p.Date.Date <= to.Date).Sum(p => p.Amount);

    /// <summary>Надходження по місяцях за останні <paramref name="months"/> місяців.</summary>
    public IReadOnlyList<(DateTime Month, decimal Amount)> GetMonthlyIncome(int months, DateTime? asOf = null)
    {
        var date = asOf ?? DateTime.Today;
        var start = new DateTime(date.Year, date.Month, 1).AddMonths(-(months - 1));
        return Enumerable.Range(0, months)
            .Select(i => start.AddMonths(i))
            .Select(m => (m, GetIncome(m, m.AddMonths(1).AddDays(-1))))
            .ToList();
    }

    /// <summary>Генерує наступний номер квитанції у форматі КВ-РРРР-NNNNN.</summary>
    public string NextReceiptNumber(DateTime? date = null)
    {
        var year = (date ?? DateTime.Today).Year;
        var prefix = $"КВ-{year}-";
        var max = _db.Payments
            .Find(p => p.ReceiptNumber.StartsWith(prefix, StringComparison.Ordinal))
            .Select(p => int.TryParse(p.ReceiptNumber[prefix.Length..], out var n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();
        return $"{prefix}{max + 1:D5}";
    }
}
