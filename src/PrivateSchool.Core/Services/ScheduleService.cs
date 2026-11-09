using PrivateSchool.Core.Data;
using PrivateSchool.Core.Exceptions;
using PrivateSchool.Core.Models;

namespace PrivateSchool.Core.Services;

/// <summary>
/// Робота з розкладом занять та перевірка конфліктів
/// (група, викладач або аудиторія вже зайняті у цей час).
/// </summary>
public class ScheduleService
{
    /// <summary>Тривалість однієї пари в академічних годинах.</summary>
    public const int HoursPerLesson = 2;

    private readonly SchoolDatabase _db;

    public ScheduleService(SchoolDatabase db) => _db = db;

    public IReadOnlyList<string> FindConflicts(Lesson lesson)
    {
        var conflicts = new List<string>();
        var sameTime = _db.Lessons
            .Find(l => l.Id != lesson.Id && l.Day == lesson.Day && l.Number == lesson.Number)
            .ToList();

        var when = $"{LessonTimes.DayName(lesson.Day)}, {lesson.Number} пара";

        if (sameTime.Any(l => l.GroupId == lesson.GroupId))
            conflicts.Add($"Група вже має заняття: {when}.");

        var teacherBusy = sameTime.FirstOrDefault(l => l.TeacherId == lesson.TeacherId);
        if (teacherBusy != null)
            conflicts.Add($"Викладач зайнятий з групою {GroupName(teacherBusy.GroupId)}: {when}.");

        var roomBusy = sameTime.FirstOrDefault(l =>
            string.Equals(l.Room.Trim(), lesson.Room.Trim(), StringComparison.OrdinalIgnoreCase));
        if (roomBusy != null)
            conflicts.Add($"Аудиторія {lesson.Room} зайнята групою {GroupName(roomBusy.GroupId)}: {when}.");

        return conflicts;
    }

    public Lesson Save(Lesson lesson)
    {
        var conflicts = FindConflicts(lesson);
        if (conflicts.Count > 0) throw new DomainException(string.Join(Environment.NewLine, conflicts));

        if (lesson.Id == 0) return _db.Lessons.Add(lesson);
        _db.Lessons.Update(lesson);
        return lesson;
    }

    public IReadOnlyList<Lesson> GetForGroup(int groupId) =>
        Sort(_db.Lessons.Find(l => l.GroupId == groupId));

    public IReadOnlyList<Lesson> GetForTeacher(int teacherId) =>
        Sort(_db.Lessons.Find(l => l.TeacherId == teacherId));

    public IReadOnlyList<Lesson> GetForDay(DayOfWeek day) =>
        Sort(_db.Lessons.Find(l => l.Day == day));

    /// <summary>Тижневе навантаження викладача в академічних годинах.</summary>
    public int GetWeeklyHours(int teacherId) =>
        _db.Lessons.Find(l => l.TeacherId == teacherId).Count() * HoursPerLesson;

    private string GroupName(int groupId) => _db.Groups.GetById(groupId)?.Name ?? "?";

    private static IReadOnlyList<Lesson> Sort(IEnumerable<Lesson> lessons) =>
        lessons.OrderBy(l => ((int)l.Day + 6) % 7).ThenBy(l => l.Number).ToList();
}
