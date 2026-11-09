using PrivateSchool.Core.Models;

namespace PrivateSchool.Tests;

public class ModelTests
{
    [Theory]
    [InlineData(100, "A", 5)]
    [InlineData(90, "A", 5)]
    [InlineData(89, "B", 4)]
    [InlineData(82, "B", 4)]
    [InlineData(75, "C", 4)]
    [InlineData(64, "D", 3)]
    [InlineData(60, "E", 3)]
    [InlineData(59, "FX", 2)]
    [InlineData(34, "F", 2)]
    public void GradeScale_ConvertsScores(int score, string ects, int national)
    {
        Assert.Equal(ects, GradeScale.ToEcts(score));
        Assert.Equal(national, GradeScale.ToNational(score));
    }

    [Fact]
    public void Person_ShortName_And_FullName()
    {
        var s = new Student { LastName = "Шевченко", FirstName = "Тарас", MiddleName = "Григорович" };
        Assert.Equal("Шевченко Тарас Григорович", s.FullName);
        Assert.Equal("Шевченко Т. Г.", s.ShortName);
        Assert.Equal("Студент", s.RoleName);
        Assert.Equal("Викладач", new Teacher().RoleName);
    }

    [Fact]
    public void Student_Validation_ReportsErrors()
    {
        var s = new Student
        {
            LastName = "",
            FirstName = "123",
            BirthDate = DateTime.Today.AddYears(1),
            DiscountPercent = 150,
            Email = "wrong",
            Phone = "12"
        };
        var errors = s.Validate().ToList();

        Assert.Contains(errors, e => e.Contains("Прізвище"));
        Assert.Contains(errors, e => e.Contains("Ім'я"));
        Assert.Contains(errors, e => e.Contains("майбутньому"));
        Assert.Contains(errors, e => e.Contains("Знижка"));
        Assert.Contains(errors, e => e.Contains("пошти"));
        Assert.Contains(errors, e => e.Contains("Телефон"));
        Assert.Contains(errors, e => e.Contains("групу"));
    }

    [Fact]
    public void Student_Expelled_RequiresEndDate()
    {
        var s = new Student
        {
            LastName = "Бойко", FirstName = "Анна", BirthDate = new DateTime(2005, 1, 1),
            GroupId = 1, ContractNumber = "Д-1", EnrollmentDate = new DateTime(2023, 9, 1),
            Status = StudentStatus.Expelled
        };
        Assert.Contains(s.Validate(), e => e.Contains("дату завершення"));

        s.EndDate = new DateTime(2024, 1, 1);
        Assert.True(s.IsValid);
    }

    [Fact]
    public void Clone_CreatesIndependentCopy()
    {
        var original = new Group { Id = 3, Name = "КН-24" };
        var copy = (Group)original.Clone();
        copy.Name = "КН-25";

        Assert.Equal("КН-24", original.Name);
        Assert.Equal(original, copy); // рівність за типом та Id
    }

    [Fact]
    public void EnumDisplay_UsesDescription()
    {
        Assert.Equal("Іспит", GradeType.Exam.ToDisplay());
        Assert.Equal(4, EnumDisplay.GetItems<StudentStatus>().Count);
    }

    [Fact]
    public void LessonTimes_ReturnsBellSchedule()
    {
        Assert.Equal("08:30–09:50", LessonTimes.GetTime(1));
        Assert.Equal("—", LessonTimes.GetTime(9));
    }
}
