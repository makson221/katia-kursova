using PrivateSchool.Core.Data;
using PrivateSchool.Core.Exceptions;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.Tests;

public class FinanceTests
{
    [Fact]
    public void BilledMonths_SkipSummerHolidays()
    {
        var s = new Student { EnrollmentDate = new DateTime(2024, 9, 1) };
        // вер 2024 – вер 2025 = 13 календарних місяців, мінус липень і серпень
        Assert.Equal(11, FinanceService.GetBilledMonths(s, new DateTime(2025, 9, 15)));
    }

    [Fact]
    public void BilledMonths_StopAtEndDate()
    {
        var s = new Student { EnrollmentDate = new DateTime(2024, 9, 1), EndDate = new DateTime(2024, 11, 20) };
        Assert.Equal(3, FinanceService.GetBilledMonths(s, new DateTime(2025, 5, 1)));
    }

    [Fact]
    public void Balance_AccountsForDiscountAndPayments()
    {
        var sys = TestHelpers.EmptySystem();
        var group = TestHelpers.AddGroup(sys.Db, fee: 4000);
        var student = TestHelpers.AddStudent(sys.Db, group, new DateTime(2024, 9, 1), discount: 25);
        sys.Db.Payments.Add(new Payment { StudentId = student.Id, Amount = 6000, Date = new DateTime(2024, 9, 5), ReceiptNumber = "1" });

        var balance = sys.Finance.GetBalance(student, new DateTime(2024, 11, 10));

        Assert.Equal(3000, balance.MonthlyFee);
        Assert.Equal(9000, balance.Accrued);
        Assert.Equal(3000, balance.Debt);
        Assert.Single(sys.Finance.GetDebtors(new DateTime(2024, 11, 10)));
    }

    [Fact]
    public void NextReceiptNumber_Increments()
    {
        var sys = TestHelpers.EmptySystem();
        var student = TestHelpers.AddStudent(sys.Db, TestHelpers.AddGroup(sys.Db));
        var year = DateTime.Today.Year;
        Assert.Equal($"КВ-{year}-00001", sys.Finance.NextReceiptNumber());

        sys.Db.Payments.Add(new Payment { StudentId = student.Id, Amount = 1, ReceiptNumber = $"КВ-{year}-00041" });
        Assert.Equal($"КВ-{year}-00042", sys.Finance.NextReceiptNumber());
    }
}

public class ScheduleTests
{
    [Fact]
    public void FindConflicts_DetectsGroupTeacherAndRoom()
    {
        var sys = TestHelpers.EmptySystem();
        var g1 = TestHelpers.AddGroup(sys.Db, name: "A-1");
        var g2 = TestHelpers.AddGroup(sys.Db, name: "A-2");
        var t = TestHelpers.AddTeacher(sys.Db);
        var subj = TestHelpers.AddSubject(sys.Db, teacherId: t.Id);

        sys.Schedule.Save(new Lesson { GroupId = g1.Id, SubjectId = subj.Id, TeacherId = t.Id, Day = DayOfWeek.Monday, Number = 1, Room = "101" });

        var clash = new Lesson { GroupId = g2.Id, SubjectId = subj.Id, TeacherId = t.Id, Day = DayOfWeek.Monday, Number = 1, Room = "101" };
        var conflicts = sys.Schedule.FindConflicts(clash);

        Assert.Equal(2, conflicts.Count); // викладач і аудиторія
        Assert.Throws<DomainException>(() => sys.Schedule.Save(clash));

        clash.Number = 2;
        Assert.Empty(sys.Schedule.FindConflicts(clash));
        sys.Schedule.Save(clash);
        Assert.Equal(4, sys.Schedule.GetWeeklyHours(t.Id));
    }
}

public class AcademicTests
{
    [Fact]
    public void GroupRating_OrdersByAverage()
    {
        var sys = TestHelpers.EmptySystem();
        var g = TestHelpers.AddGroup(sys.Db);
        var subj = TestHelpers.AddSubject(sys.Db);
        var weak = TestHelpers.AddStudent(sys.Db, g);
        var strong = sys.Db.Students.Add(new Student
        {
            LastName = "Бойко", FirstName = "Анна", BirthDate = new DateTime(2006, 1, 1),
            GroupId = g.Id, ContractNumber = "Д-2", EnrollmentDate = new DateTime(2024, 9, 1)
        });

        void Grade(Student s, int score) =>
            sys.Db.Grades.Add(new Grade { StudentId = s.Id, SubjectId = subj.Id, Score = score });

        Grade(weak, 55); Grade(weak, 70);
        Grade(strong, 95); Grade(strong, 91);

        var rating = sys.Academic.GetGroupRating(g.Id);
        Assert.Equal(strong.Id, rating[0].Student.Id);
        Assert.Equal(93, rating[0].Average);
        Assert.Equal(1, rating[1].FailedCount);

        var perf = sys.Academic.GetGroupPerformance().Single();
        Assert.Equal(75, perf.SuccessRate);
        Assert.Equal(1, perf.ExcellentCount);
    }
}

public class AuthTests
{
    [Fact]
    public void Login_ChecksPasswordAndPermissions()
    {
        var sys = TestHelpers.EmptySystem();
        sys.Auth.CreateUser(new User { Login = "boss", DisplayName = "Бос", Role = UserRole.Manager }, "secret1");

        Assert.False(sys.Auth.Login("boss", "wrong1"));
        Assert.True(sys.Auth.Login("BOSS", "secret1"));
        Assert.True(sys.Auth.Can(Permission.ManagePayments));
        Assert.False(sys.Auth.Can(Permission.ManageUsers));
        Assert.NotNull(sys.Auth.CurrentUser!.LastLogin);
    }

    [Fact]
    public void CreateUser_RejectsWeakPasswordAndDuplicateLogin()
    {
        var sys = TestHelpers.EmptySystem();
        Assert.Throws<ValidationException>(() =>
            sys.Auth.CreateUser(new User { Login = "u1", DisplayName = "U" }, "123"));

        sys.Auth.CreateUser(new User { Login = "user1", DisplayName = "U" }, "pass12");
        Assert.Throws<DomainException>(() =>
            sys.Auth.CreateUser(new User { Login = "USER1", DisplayName = "U2" }, "pass12"));
    }

    [Fact]
    public void LastAdmin_CannotBeDeletedOrDemoted()
    {
        var sys = TestHelpers.EmptySystem();
        var admin = sys.Auth.CreateUser(new User { Login = "admin", DisplayName = "A", Role = UserRole.Administrator }, "admin123");
        var other = sys.Auth.CreateUser(new User { Login = "mgr", DisplayName = "M", Role = UserRole.Manager }, "manager1");
        sys.Auth.Login("mgr", "manager1");

        Assert.NotNull(sys.Integrity.GetDeleteBlocker(admin));

        var demoted = (User)admin.Clone();
        demoted.Role = UserRole.Manager;
        Assert.Throws<DomainException>(() => sys.Auth.UpdateUser(demoted));
        Assert.NotNull(sys.Integrity.GetDeleteBlocker(other)); // себе видалити не можна
    }
}

public class IntegrityTests
{
    [Fact]
    public void GroupWithStudents_CannotBeDeleted()
    {
        var sys = TestHelpers.EmptySystem();
        var g = TestHelpers.AddGroup(sys.Db);
        TestHelpers.AddStudent(sys.Db, g);

        Assert.Throws<DomainException>(() => sys.Integrity.Delete(g));
    }

    [Fact]
    public void DeletingStudent_CascadesGradesAndPayments()
    {
        var sys = TestHelpers.EmptySystem();
        var g = TestHelpers.AddGroup(sys.Db);
        var s = TestHelpers.AddStudent(sys.Db, g);
        var subj = TestHelpers.AddSubject(sys.Db);
        sys.Db.Grades.Add(new Grade { StudentId = s.Id, SubjectId = subj.Id, Score = 80 });
        sys.Db.Payments.Add(new Payment { StudentId = s.Id, Amount = 100, ReceiptNumber = "1" });

        Assert.NotNull(sys.Integrity.DescribeCascade(s));
        sys.Integrity.Delete(s);

        Assert.Equal(0, sys.Db.Students.Count);
        Assert.Equal(0, sys.Db.Grades.Count);
        Assert.Equal(0, sys.Db.Payments.Count);
    }

    [Fact]
    public void DeletingTeacher_ClearsReferences()
    {
        var sys = TestHelpers.EmptySystem();
        var t = TestHelpers.AddTeacher(sys.Db);
        var subj = TestHelpers.AddSubject(sys.Db, teacherId: t.Id);

        sys.Integrity.Delete(t);

        Assert.Null(sys.Db.Subjects.GetById(subj.Id)!.TeacherId);
    }
}

public class SeedAndReportTests
{
    [Fact]
    public void DemoData_IsGeneratedAndValid()
    {
        var storage = new InMemoryDataStorage();
        var sys = new SchoolSystem(storage, new FakeHasher());

        Assert.True(sys.Initialize());
        Assert.True(sys.Db.Students.Count > 50);
        Assert.Equal(10, sys.Db.Teachers.Count);
        Assert.True(sys.Db.Lessons.Count > 40);
        Assert.True(sys.Db.Payments.Count > 100);
        Assert.Equal(3, sys.Db.Users.Count);
        Assert.Equal(1, storage.SaveCount);

        // Усі заняття без конфліктів, усі сутності валідні.
        Assert.All(sys.Db.Lessons.GetAll(), l => Assert.Empty(sys.Schedule.FindConflicts(l)));
        Assert.All(sys.Db.Students.GetAll(), s => Assert.Empty(s.Validate()));
        Assert.True(sys.Auth.Login("admin", "admin123"));

        // Є і боржники, і ті, хто сплатив.
        var debtors = sys.Finance.GetDebtors();
        Assert.NotEmpty(debtors);
        Assert.True(debtors.Count < sys.Db.Students.Count / 2);
    }

    [Fact]
    public void AllReports_BuildAndExport()
    {
        var sys = new SchoolSystem(new InMemoryDataStorage(), new FakeHasher());
        sys.Initialize();
        var parameters = new Core.Reports.ReportParameters
        {
            From = DateTime.Today.AddMonths(-3),
            To = DateTime.Today,
            GroupId = sys.Db.Groups.GetAll()[0].Id
        };

        var dir = Path.Combine(Path.GetTempPath(), $"reports-{Guid.NewGuid():N}");
        Directory.CreateDirectory(dir);
        try
        {
            foreach (var report in sys.Reports)
            {
                var table = report.Build(parameters);
                Assert.NotEmpty(table.Rows);
                Assert.All(table.Rows, r => Assert.Equal(table.Columns.Count, r.Length));

                foreach (var exporter in sys.Exporters)
                {
                    var file = Path.Combine(dir, $"{report.GetType().Name}.{exporter.FileFilter.Split('.').Last()}");
                    exporter.Export(table, file);
                    Assert.True(new FileInfo(file).Length > 0);
                }
            }
        }
        finally
        {
            Directory.Delete(dir, recursive: true);
        }
    }
}
