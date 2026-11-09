using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.Core.Reports;

/// <summary>Список студентів із заборгованістю за навчання.</summary>
public sealed class DebtorsReport : Report
{
    public DebtorsReport(SchoolDatabase db, FinanceService f, AcademicService a, ScheduleService s) : base(db, f, a, s) { }

    public override string Name => "Заборгованість за навчання";
    public override string Description => "Студенти, у яких сума оплат менша за нараховану плату станом на сьогодні.";
    public override bool UsesGroup => true;

    protected override ReportTable BuildTable(ReportParameters p)
    {
        var table = new ReportTable(Name,
            new ReportColumn("№", ColumnAlign.Center, 40),
            new ReportColumn("ПІБ студента", Width: 230),
            new ReportColumn("Група", ColumnAlign.Center, 80),
            new ReportColumn("Договір", Width: 110),
            new ReportColumn("Нараховано, грн", ColumnAlign.Right),
            new ReportColumn("Сплачено, грн", ColumnAlign.Right),
            new ReportColumn("Борг, грн", ColumnAlign.Right),
            new ReportColumn("Остання оплата", ColumnAlign.Center),
            new ReportColumn("Телефон батьків", Width: 130))
        {
            Subtitle = $"Станом на {DateTime.Today:dd.MM.yyyy}" + (p.GroupId is int g ? $", група {GroupName(g)}" : "")
        };

        var debtors = Finance.GetDebtors(groupId: p.GroupId);
        var i = 1;
        foreach (var d in debtors)
        {
            table.AddRow(i++, d.Student.FullName, d.GroupName, d.Student.ContractNumber, d.Accrued, d.Paid, d.Debt,
                d.LastPaymentDate?.ToString("dd.MM.yyyy") ?? "немає", d.Student.ParentPhone);
        }

        table.Summary.Add($"Кількість боржників: {debtors.Count}");
        table.Summary.Add($"Загальна сума заборгованості: {debtors.Sum(d => d.Debt):N2} грн");
        return table;
    }
}

/// <summary>Реєстр оплат за обраний період.</summary>
public sealed class PaymentsReport : Report
{
    public PaymentsReport(SchoolDatabase db, FinanceService f, AcademicService a, ScheduleService s) : base(db, f, a, s) { }

    public override string Name => "Реєстр оплат за період";
    public override string Description => "Усі надходження коштів за вказаний період з підсумками за способом оплати.";
    public override bool UsesPeriod => true;
    public override bool UsesGroup => true;

    protected override ReportTable BuildTable(ReportParameters p)
    {
        var table = new ReportTable(Name,
            new ReportColumn("Дата", ColumnAlign.Center, 90),
            new ReportColumn("Квитанція", Width: 120),
            new ReportColumn("ПІБ студента", Width: 230),
            new ReportColumn("Група", ColumnAlign.Center, 80),
            new ReportColumn("Спосіб оплати", Width: 150),
            new ReportColumn("Призначення", Width: 200),
            new ReportColumn("Сума, грн", ColumnAlign.Right))
        {
            Subtitle = $"Період: {p.From:dd.MM.yyyy} — {p.To:dd.MM.yyyy}" + (p.GroupId is int g ? $", група {GroupName(g)}" : "")
        };

        var payments = Db.Payments
            .Find(x => x.Date.Date >= p.From.Date && x.Date.Date <= p.To.Date)
            .Select(x => new { Payment = x, Student = Db.Students.GetById(x.StudentId) })
            .Where(x => p.GroupId == null || x.Student?.GroupId == p.GroupId)
            .OrderBy(x => x.Payment.Date)
            .ThenBy(x => x.Payment.ReceiptNumber)
            .ToList();

        foreach (var x in payments)
        {
            table.AddRow(x.Payment.Date, x.Payment.ReceiptNumber, x.Student?.FullName ?? "—",
                x.Student == null ? "—" : GroupName(x.Student.GroupId),
                x.Payment.Method.ToDisplay(), x.Payment.Purpose, x.Payment.Amount);
        }

        table.Summary.Add($"Кількість оплат: {payments.Count}");
        foreach (var byMethod in payments.GroupBy(x => x.Payment.Method))
            table.Summary.Add($"{byMethod.Key.ToDisplay()}: {byMethod.Sum(x => x.Payment.Amount):N2} грн");
        table.Summary.Add($"Разом надійшло: {payments.Sum(x => x.Payment.Amount):N2} грн");
        return table;
    }
}

/// <summary>Рейтинг успішності студентів групи.</summary>
public sealed class GroupRatingReport : Report
{
    public GroupRatingReport(SchoolDatabase db, FinanceService f, AcademicService a, ScheduleService s) : base(db, f, a, s) { }

    public override string Name => "Рейтинг студентів групи";
    public override string Description => "Студенти групи, впорядковані за середнім балом, з кількістю незадовільних оцінок.";
    public override bool UsesGroup => true;
    public override bool RequiresGroup => true;

    protected override ReportTable BuildTable(ReportParameters p)
    {
        var groupId = p.GroupId!.Value;
        var table = new ReportTable(Name,
            new ReportColumn("Місце", ColumnAlign.Center, 60),
            new ReportColumn("ПІБ студента", Width: 250),
            new ReportColumn("Середній бал", ColumnAlign.Center, 110),
            new ReportColumn("ЄКТС", ColumnAlign.Center, 60),
            new ReportColumn("Оцінок", ColumnAlign.Center, 80),
            new ReportColumn("Незадовільних", ColumnAlign.Center, 110))
        {
            Subtitle = $"Група {GroupName(groupId)}"
        };

        var rating = Academic.GetGroupRating(groupId);
        foreach (var r in rating)
            table.AddRow(r.Rank, r.Student.FullName, r.GradesCount == 0 ? "—" : r.Average.ToString("0.0"),
                r.GradesCount == 0 ? "—" : r.Ects, r.GradesCount, r.FailedCount);

        var rated = rating.Where(r => r.GradesCount > 0).ToList();
        table.Summary.Add($"Студентів у рейтингу: {rating.Count}");
        if (rated.Count > 0)
        {
            table.Summary.Add($"Середній бал групи: {rated.Average(r => r.Average):0.0}");
            table.Summary.Add($"Мають академічну заборгованість: {rated.Count(r => r.FailedCount > 0)}");
        }
        return table;
    }
}

/// <summary>Порівняльна успішність груп.</summary>
public sealed class GroupPerformanceReport : Report
{
    public GroupPerformanceReport(SchoolDatabase db, FinanceService f, AcademicService a, ScheduleService s) : base(db, f, a, s) { }

    public override string Name => "Успішність за групами";
    public override string Description => "Середній бал, відсоток позитивних оцінок і кількість відмінників у кожній групі.";

    protected override ReportTable BuildTable(ReportParameters p)
    {
        var table = new ReportTable(Name,
            new ReportColumn("Група", ColumnAlign.Center, 90),
            new ReportColumn("Спеціальність", Width: 230),
            new ReportColumn("Курс", ColumnAlign.Center, 60),
            new ReportColumn("Студентів", ColumnAlign.Center, 90),
            new ReportColumn("Середній бал", ColumnAlign.Center, 110),
            new ReportColumn("Успішність, %", ColumnAlign.Center, 110),
            new ReportColumn("Відмінників", ColumnAlign.Center, 100))
        {
            Subtitle = $"Станом на {DateTime.Today:dd.MM.yyyy}"
        };

        var stats = Academic.GetGroupPerformance();
        foreach (var s in stats)
            table.AddRow(s.Group.Name, s.Group.Specialty, s.Group.CourseNumber, s.StudentsCount,
                s.Average?.ToString("0.0") ?? "—", s.SuccessRate, s.ExcellentCount);

        table.Summary.Add($"Середній бал по закладу: {Academic.GetOverallAverage()?.ToString("0.0") ?? "—"}");
        table.Summary.Add($"Відмінників усього: {stats.Sum(s => s.ExcellentCount)}");
        return table;
    }
}

/// <summary>Тижневе навантаження викладачів та орієнтовна оплата.</summary>
public sealed class TeacherLoadReport : Report
{
    /// <summary>Кількість навчальних тижнів у місяці для розрахунку.</summary>
    private const decimal WeeksPerMonth = 4m;

    public TeacherLoadReport(SchoolDatabase db, FinanceService f, AcademicService a, ScheduleService s) : base(db, f, a, s) { }

    public override string Name => "Навантаження викладачів";
    public override string Description => "Кількість пар і годин на тиждень за розкладом та орієнтовна місячна оплата праці.";

    protected override ReportTable BuildTable(ReportParameters p)
    {
        var table = new ReportTable(Name,
            new ReportColumn("ПІБ викладача", Width: 230),
            new ReportColumn("Посада", Width: 150),
            new ReportColumn("Дисципліни", Width: 260),
            new ReportColumn("Пар/тиждень", ColumnAlign.Center, 100),
            new ReportColumn("Годин/тиждень", ColumnAlign.Center, 110),
            new ReportColumn("Ставка, грн/год", ColumnAlign.Right),
            new ReportColumn("Оплата/місяць, грн", ColumnAlign.Right, 140))
        {
            Subtitle = $"Розрахунок: {WeeksPerMonth} навчальні тижні на місяць"
        };

        decimal total = 0;
        foreach (var t in Db.Teachers.GetAll().OrderBy(t => t.FullName))
        {
            var lessons = Schedule.GetForTeacher(t.Id);
            var hours = Schedule.GetWeeklyHours(t.Id);
            var subjects = lessons.Select(l => l.SubjectId).Distinct()
                .Select(id => Db.Subjects.GetById(id)?.Name).Where(n => n != null);
            var salary = hours * t.HourlyRate * WeeksPerMonth;
            total += salary;
            table.AddRow(t.FullName, t.Position, string.Join(", ", subjects), lessons.Count, hours, t.HourlyRate, salary);
        }

        table.Summary.Add($"Викладачів: {Db.Teachers.Count}");
        table.Summary.Add($"Фонд оплати праці (орієнтовно): {total:N2} грн/місяць");
        return table;
    }
}

/// <summary>Контингент студентів (список за групами).</summary>
public sealed class StudentListReport : Report
{
    public StudentListReport(SchoolDatabase db, FinanceService f, AcademicService a, ScheduleService s) : base(db, f, a, s) { }

    public override string Name => "Контингент студентів";
    public override string Description => "Список студентів з контактними даними, статусом та умовами договору.";
    public override bool UsesGroup => true;

    protected override ReportTable BuildTable(ReportParameters p)
    {
        var table = new ReportTable(Name,
            new ReportColumn("№", ColumnAlign.Center, 40),
            new ReportColumn("ПІБ студента", Width: 230),
            new ReportColumn("Група", ColumnAlign.Center, 80),
            new ReportColumn("Дата народження", ColumnAlign.Center, 120),
            new ReportColumn("Телефон", Width: 130),
            new ReportColumn("Статус", Width: 130),
            new ReportColumn("Договір", Width: 110),
            new ReportColumn("Знижка, %", ColumnAlign.Center, 90))
        {
            Subtitle = p.GroupId is int g ? $"Група {GroupName(g)}" : "Усі групи"
        };

        var students = Db.Students.Find(s => p.GroupId == null || s.GroupId == p.GroupId)
            .OrderBy(s => GroupName(s.GroupId)).ThenBy(s => s.FullName).ToList();

        var i = 1;
        foreach (var s in students)
            table.AddRow(i++, s.FullName, GroupName(s.GroupId), s.BirthDate, s.Phone, s.Status.ToDisplay(),
                s.ContractNumber, s.DiscountPercent.ToString("0.#"));

        table.Summary.Add($"Усього: {students.Count}");
        foreach (var byStatus in students.GroupBy(s => s.Status).OrderBy(x => x.Key))
            table.Summary.Add($"{byStatus.Key.ToDisplay()}: {byStatus.Count()}");
        return table;
    }
}
