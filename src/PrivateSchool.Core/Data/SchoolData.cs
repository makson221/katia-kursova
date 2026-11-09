using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Data;

/// <summary>
/// DTO-знімок усіх даних закладу для серіалізації у сховище.
/// </summary>
public class SchoolData
{
    public string InstitutionName { get; set; } = "Приватний коледж «Освіта+»";
    public List<Student> Students { get; set; } = new();
    public List<Teacher> Teachers { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
    public List<Subject> Subjects { get; set; } = new();
    public List<Grade> Grades { get; set; } = new();
    public List<Payment> Payments { get; set; } = new();
    public List<Lesson> Lessons { get; set; } = new();
    public List<User> Users { get; set; } = new();
}
