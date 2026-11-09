using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI;

/// <summary>
/// Обмеження даних відповідно до ролі поточного користувача.
/// </summary>
public static class AccessHelper
{
    public static bool IsTeacher(this SchoolSystem app) => app.Auth.CurrentUser?.Role == UserRole.Teacher;

    /// <summary>Дисципліни, з яких поточний користувач може виставляти оцінки.</summary>
    public static IReadOnlyList<Subject> GradableSubjects(this SchoolSystem app)
    {
        var user = app.Auth.CurrentUser;
        if (user?.Role != UserRole.Teacher) return app.Db.Subjects.GetAll().OrderBy(s => s.Name).ToList();

        var teacherId = user.TeacherId ?? -1;
        var fromLessons = app.Db.Lessons.Find(l => l.TeacherId == teacherId).Select(l => l.SubjectId).ToHashSet();
        return app.Db.Subjects.Find(s => s.TeacherId == teacherId || fromLessons.Contains(s.Id))
            .OrderBy(s => s.Name).ToList();
    }

    public static bool CanEditGrade(this SchoolSystem app, Grade grade) =>
        app.Auth.Can(Permission.ManageGrades) &&
        (!app.IsTeacher() || app.GradableSubjects().Any(s => s.Id == grade.SubjectId));
}
