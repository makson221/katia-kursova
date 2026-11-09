using PrivateSchool.Core.Data;
using PrivateSchool.Core.Interfaces;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.Tests;

/// <summary>Швидкий хешер для тестів (без PBKDF2).</summary>
internal sealed class FakeHasher : IPasswordHasher
{
    public string Hash(string password) => "h:" + password;
    public bool Verify(string password, string hash) => hash == "h:" + password;
}

internal static class TestHelpers
{
    public static SchoolSystem EmptySystem(InMemoryDataStorage? storage = null)
    {
        var system = new SchoolSystem(storage ?? new InMemoryDataStorage(), new FakeHasher());
        system.Initialize(seedDemoData: false);
        return system;
    }

    public static Group AddGroup(SchoolDatabase db, decimal fee = 5000, string name = "ПІ-24") =>
        db.Groups.Add(new Group { Name = name, Specialty = "Інженерія ПЗ", StartYear = DateTime.Today.Year - 1, MonthlyFee = fee });

    public static Student AddStudent(SchoolDatabase db, Group group, DateTime? enrolled = null, decimal discount = 0) =>
        db.Students.Add(new Student
        {
            LastName = "Петренко",
            FirstName = "Іван",
            MiddleName = "Петрович",
            BirthDate = new DateTime(2006, 5, 10),
            GroupId = group.Id,
            ContractNumber = "Д-001",
            EnrollmentDate = enrolled ?? new DateTime(2024, 9, 1),
            DiscountPercent = discount
        });

    public static Teacher AddTeacher(SchoolDatabase db, string lastName = "Коваленко") =>
        db.Teachers.Add(new Teacher
        {
            LastName = lastName,
            FirstName = "Олена",
            MiddleName = "Петрівна",
            BirthDate = new DateTime(1985, 1, 1),
            HireDate = new DateTime(2015, 9, 1),
            Position = "Доцент",
            HourlyRate = 400
        });

    public static Subject AddSubject(SchoolDatabase db, string name = "ООП", int? teacherId = null) =>
        db.Subjects.Add(new Subject { Name = name, Hours = 120, TeacherId = teacherId });
}
