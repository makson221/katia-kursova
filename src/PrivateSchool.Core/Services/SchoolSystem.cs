using PrivateSchool.Core.Data;
using PrivateSchool.Core.Interfaces;
using PrivateSchool.Core.Reports;

namespace PrivateSchool.Core.Services;

/// <summary>
/// Фасад системи: створює та зв'язує між собою сховище, контекст даних і сервіси.
/// Інтерфейс користувача працює лише через цей об'єкт.
/// </summary>
public class SchoolSystem
{
    public SchoolDatabase Db { get; }
    public AuthService Auth { get; }
    public FinanceService Finance { get; }
    public AcademicService Academic { get; }
    public ScheduleService Schedule { get; }
    public IntegrityService Integrity { get; }
    public IReadOnlyList<Report> Reports { get; }
    public IReadOnlyList<IReportExporter> Exporters { get; }

    public SchoolSystem(IDataStorage storage, IPasswordHasher? hasher = null)
    {
        var passwordHasher = hasher ?? new Pbkdf2PasswordHasher();

        Db = new SchoolDatabase(storage);
        Auth = new AuthService(Db, passwordHasher);
        Finance = new FinanceService(Db);
        Academic = new AcademicService(Db);
        Schedule = new ScheduleService(Db);
        Integrity = new IntegrityService(Db, Auth);

        Reports = new Report[]
        {
            new DebtorsReport(Db, Finance, Academic, Schedule),
            new PaymentsReport(Db, Finance, Academic, Schedule),
            new GroupRatingReport(Db, Finance, Academic, Schedule),
            new GroupPerformanceReport(Db, Finance, Academic, Schedule),
            new TeacherLoadReport(Db, Finance, Academic, Schedule),
            new StudentListReport(Db, Finance, Academic, Schedule)
        };

        Exporters = new IReportExporter[]
        {
            new CsvReportExporter(),
            new HtmlReportExporter(() => Db.InstitutionName)
        };
    }

    /// <summary>Завантажує дані; при першому запуску створює демонстраційну базу.</summary>
    /// <returns>true, якщо було створено демонстраційні дані.</returns>
    public bool Initialize(bool seedDemoData = true) =>
        Db.Load(seedDemoData ? db => SeedData.Fill(db, Auth) : null);
}
