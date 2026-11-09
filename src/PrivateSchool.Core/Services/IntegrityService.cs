using PrivateSchool.Core.Data;
using PrivateSchool.Core.Exceptions;
using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Services;

/// <summary>
/// Забезпечує посилальну цілісність даних під час видалення:
/// забороняє видалення, якщо є залежні записи, або каскадно видаляє їх.
/// </summary>
public class IntegrityService
{
    private readonly SchoolDatabase _db;
    private readonly AuthService _auth;

    public IntegrityService(SchoolDatabase db, AuthService auth)
    {
        _db = db;
        _auth = auth;
    }

    /// <summary>Причина, з якої запис не можна видалити, або null.</summary>
    public string? GetDeleteBlocker(Entity entity) => entity switch
    {
        Group g when _db.Students.Exists(s => s.GroupId == g.Id) =>
            $"У групі {g.Name} є студенти ({_db.Students.Find(s => s.GroupId == g.Id).Count()}). Спершу переведіть або видаліть їх.",
        Teacher t when _auth.CurrentUser?.TeacherId == t.Id =>
            "Не можна видалити викладача, під обліковим записом якого ви працюєте.",
        Teacher t when _db.Lessons.Exists(l => l.TeacherId == t.Id) =>
            "Викладач має заняття у розкладі. Спершу змініть розклад.",
        Subject s when _db.Grades.Exists(g => g.SubjectId == s.Id) =>
            "З цієї дисципліни вже виставлено оцінки.",
        User u when u.Id == _auth.CurrentUser?.Id =>
            "Не можна видалити власний обліковий запис.",
        User { Role: UserRole.Administrator, IsActive: true } u when _auth.CountActiveAdmins(exceptId: u.Id) == 0 =>
            "Не можна видалити останнього адміністратора.",
        _ => null
    };

    /// <summary>Опис записів, які буде видалено разом з основним (для підтвердження).</summary>
    public string? DescribeCascade(Entity entity)
    {
        switch (entity)
        {
            case Student s:
                var grades = _db.Grades.Find(g => g.StudentId == s.Id).Count();
                var payments = _db.Payments.Find(p => p.StudentId == s.Id).Count();
                return grades + payments == 0 ? null : $"Буде також видалено оцінок: {grades}, оплат: {payments}.";
            case Teacher t:
                var accounts = _db.Users.Find(u => u.TeacherId == t.Id).Count();
                return accounts == 0 ? null : $"Буде також видалено облікових записів викладача: {accounts}.";
            case Group g:
                var lessons = _db.Lessons.Find(l => l.GroupId == g.Id).Count();
                return lessons == 0 ? null : $"Буде також видалено занять з розкладу: {lessons}.";
            case Subject subj:
                var subjLessons = _db.Lessons.Find(l => l.SubjectId == subj.Id).Count();
                return subjLessons == 0 ? null : $"Буде також видалено занять з розкладу: {subjLessons}.";
            default:
                return null;
        }
    }

    public void Delete(Entity entity)
    {
        var blocker = GetDeleteBlocker(entity);
        if (blocker != null) throw new DomainException(blocker);

        _db.RunInBatch(() =>
        {
            switch (entity)
            {
                case Student s:
                    _db.Grades.RemoveWhere(g => g.StudentId == s.Id);
                    _db.Payments.RemoveWhere(p => p.StudentId == s.Id);
                    _db.Students.Remove(s.Id);
                    break;
                case Teacher t:
                    foreach (var subject in _db.Subjects.Find(x => x.TeacherId == t.Id).ToList())
                    {
                        var copy = (Subject)subject.Clone();
                        copy.TeacherId = null;
                        _db.Subjects.Update(copy);
                    }
                    foreach (var group in _db.Groups.Find(x => x.CuratorId == t.Id).ToList())
                    {
                        var copy = (Group)group.Clone();
                        copy.CuratorId = null;
                        _db.Groups.Update(copy);
                    }
                    // Обліковий запис з роллю "Викладач" не може існувати без прив'язки до викладача.
                    _db.Users.RemoveWhere(u => u.TeacherId == t.Id);
                    _db.Teachers.Remove(t.Id);
                    break;
                case Group g:
                    _db.Lessons.RemoveWhere(l => l.GroupId == g.Id);
                    _db.Groups.Remove(g.Id);
                    break;
                case Subject subj:
                    _db.Lessons.RemoveWhere(l => l.SubjectId == subj.Id);
                    _db.Subjects.Remove(subj.Id);
                    break;
                case Grade grade:
                    _db.Grades.Remove(grade.Id);
                    break;
                case Payment payment:
                    _db.Payments.Remove(payment.Id);
                    break;
                case Lesson lesson:
                    _db.Lessons.Remove(lesson.Id);
                    break;
                case User user:
                    _db.Users.Remove(user.Id);
                    break;
                default:
                    throw new NotSupportedException($"Видалення {entity.GetType().Name} не підтримується.");
            }
        });
    }
}
