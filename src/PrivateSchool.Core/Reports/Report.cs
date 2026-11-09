using PrivateSchool.Core.Data;
using PrivateSchool.Core.Services;

namespace PrivateSchool.Core.Reports;

/// <summary>
/// Абстрактний звіт (шаблон Template Method): кожен нащадок визначає власну назву,
/// потрібні параметри та алгоритм побудови таблиці.
/// </summary>
public abstract class Report
{
    protected SchoolDatabase Db { get; }
    protected FinanceService Finance { get; }
    protected AcademicService Academic { get; }
    protected ScheduleService Schedule { get; }

    protected Report(SchoolDatabase db, FinanceService finance, AcademicService academic, ScheduleService schedule)
    {
        Db = db;
        Finance = finance;
        Academic = academic;
        Schedule = schedule;
    }

    public abstract string Name { get; }
    public abstract string Description { get; }

    /// <summary>Чи використовує звіт період (дати "з" та "по").</summary>
    public virtual bool UsesPeriod => false;

    /// <summary>Чи використовує звіт фільтр за групою.</summary>
    public virtual bool UsesGroup => false;

    /// <summary>Чи обов'язково обирати групу.</summary>
    public virtual bool RequiresGroup => false;

    /// <summary>Шаблонний метод: перевіряє параметри та делегує побудову нащадку.</summary>
    public ReportTable Build(ReportParameters parameters)
    {
        if (UsesPeriod && parameters.From.Date > parameters.To.Date)
            throw new ArgumentException("Початкова дата періоду не може бути пізніше кінцевої.");
        if (RequiresGroup && parameters.GroupId == null)
            throw new ArgumentException("Для цього звіту оберіть групу.");

        return BuildTable(parameters);
    }

    protected abstract ReportTable BuildTable(ReportParameters parameters);

    protected string GroupName(int id) => Db.Groups.GetById(id)?.Name ?? "—";

    public override string ToString() => Name;
}
