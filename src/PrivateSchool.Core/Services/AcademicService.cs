using PrivateSchool.Core.Data;
using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Services;

/// <summary>Рейтинговий запис студента.</summary>
public sealed record StudentRating(int Rank, Student Student, double Average, int GradesCount, int FailedCount)
{
    public string Ects => GradeScale.ToEcts((int)Math.Round(Average));
}

/// <summary>Узагальнені показники успішності групи.</summary>
public sealed record GroupPerformance(Group Group, int StudentsCount, double? Average, double SuccessRate, int ExcellentCount);

/// <summary>
/// Аналітика успішності: середні бали, рейтинги, якість знань.
/// </summary>
public class AcademicService
{
    private readonly SchoolDatabase _db;

    public AcademicService(SchoolDatabase db) => _db = db;

    public double? GetAverage(int studentId, int? subjectId = null)
    {
        var scores = _db.Grades
            .Find(g => g.StudentId == studentId && (subjectId == null || g.SubjectId == subjectId))
            .Select(g => g.Score)
            .ToList();
        return scores.Count == 0 ? null : Math.Round(scores.Average(), 1);
    }

    public IReadOnlyList<StudentRating> GetGroupRating(int groupId)
    {
        var rows = _db.Students
            .Find(s => s.GroupId == groupId && s.IsStudying)
            .Select(s =>
            {
                var grades = _db.Grades.Find(g => g.StudentId == s.Id).ToList();
                return new
                {
                    Student = s,
                    Average = grades.Count == 0 ? 0 : Math.Round(grades.Average(g => g.Score), 1),
                    Count = grades.Count,
                    Failed = grades.Count(g => !g.IsPassed)
                };
            })
            .OrderByDescending(x => x.Average)
            .ThenBy(x => x.Failed)
            .ThenBy(x => x.Student.FullName)
            .ToList();

        return rows.Select((x, i) => new StudentRating(i + 1, x.Student, x.Average, x.Count, x.Failed)).ToList();
    }

    public IReadOnlyList<GroupPerformance> GetGroupPerformance()
    {
        return _db.Groups.GetAll()
            .Select(group =>
            {
                var studentIds = _db.Students
                    .Find(s => s.GroupId == group.Id && s.IsStudying)
                    .Select(s => s.Id)
                    .ToHashSet();
                var grades = _db.Grades.Find(g => studentIds.Contains(g.StudentId)).ToList();

                var excellent = studentIds.Count(id =>
                {
                    var avg = GetAverage(id);
                    return avg.HasValue && avg.Value >= 90;
                });

                return new GroupPerformance(
                    group,
                    studentIds.Count,
                    grades.Count == 0 ? null : Math.Round(grades.Average(g => g.Score), 1),
                    grades.Count == 0 ? 0 : Math.Round(100.0 * grades.Count(g => g.IsPassed) / grades.Count, 1),
                    excellent);
            })
            .OrderByDescending(p => p.Average ?? 0)
            .ToList();
    }

    /// <summary>Середній бал по всіх оцінках закладу.</summary>
    public double? GetOverallAverage()
    {
        var all = _db.Grades.GetAll();
        return all.Count == 0 ? null : Math.Round(all.Average(g => g.Score), 1);
    }

    /// <summary>Дисципліни, які вивчає група (за розкладом).</summary>
    public IReadOnlyList<Subject> GetGroupSubjects(int groupId)
    {
        var ids = _db.Lessons.Find(l => l.GroupId == groupId).Select(l => l.SubjectId).ToHashSet();
        return _db.Subjects.Find(s => ids.Contains(s.Id)).OrderBy(s => s.Name).ToList();
    }
}
