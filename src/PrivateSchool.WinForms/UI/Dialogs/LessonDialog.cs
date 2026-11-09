using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Dialogs;

public sealed class LessonDialog : EntityDialog<Lesson>
{
    private readonly ComboBox _group = Lookup.DropDownList();
    private readonly ComboBox _subject = Lookup.DropDownList();
    private readonly ComboBox _teacher = Lookup.DropDownList();
    private readonly ComboBox _day = Lookup.DropDownList();
    private readonly ComboBox _number = Lookup.DropDownList();
    private readonly TextBox _room = NewTextBox(10);
    private readonly ComboBox _type = Lookup.DropDownList();
    private readonly Label _conflicts = new() { AutoSize = false, Height = 48 };
    private bool _loading = true;

    public LessonDialog(SchoolSystem app, Lesson entity, bool isNew)
        : base(app, entity, isNew, lesson => app.Schedule.Save(lesson)) { }

    protected override string EntityName => "заняття";

    protected override void BuildFields()
    {
        AddField("Група *", _group);
        AddField("Тип заняття", _type);
        AddField("Дисципліна *", _subject, span: 2);
        AddField("Викладач *", _teacher, span: 2);
        AddField("День тижня", _day);
        AddField("Пара", _number);
        AddField("Аудиторія *", _room);
        AddSpacer();
        AddField("Перевірка розкладу", _conflicts, span: 2);

        _subject.SelectedIndexChanged += (_, _) =>
        {
            if (_loading) return;
            var subject = App.Db.Subjects.GetById(Lookup.SelectedId(_subject) ?? 0);
            if (subject?.TeacherId != null) Lookup.Select(_teacher, subject.TeacherId);
        };

        foreach (var c in new Control[] { _group, _subject, _teacher, _day, _number, _room })
        {
            if (c is ComboBox cb) cb.SelectedIndexChanged += (_, _) => CheckConflicts();
            else c.TextChanged += (_, _) => CheckConflicts();
        }
    }

    protected override void BindToControls()
    {
        Lookup.Fill(_group, Lookup.Groups(App.Db), Entity.GroupId == 0 ? null : Entity.GroupId);
        Lookup.Fill(_subject, Lookup.Subjects(App.Db.Subjects.GetAll()), Entity.SubjectId == 0 ? null : Entity.SubjectId);
        if (Entity.SubjectId == 0) _subject.SelectedIndex = -1;
        Lookup.Fill(_teacher, Lookup.Teachers(App.Db), Entity.TeacherId == 0 ? null : Entity.TeacherId);
        if (Entity.TeacherId == 0) _teacher.SelectedIndex = -1;

        Lookup.Fill(_day, LessonTimes.StudyDays.Select(d => new LookupItem((int)d, LessonTimes.DayName(d))), (int)Entity.Day);
        Lookup.Fill(_number, Enumerable.Range(1, LessonTimes.MaxLessonsPerDay)
            .Select(n => new LookupItem(n, $"{n} пара ({LessonTimes.GetTime(n)})")), Entity.Number);
        _room.Text = Entity.Room;
        Lookup.FillEnum(_type, Entity.Type);

        _loading = false;
        CheckConflicts();
    }

    protected override void ApplyToEntity()
    {
        Entity.GroupId = Lookup.SelectedId(_group) ?? 0;
        Entity.SubjectId = Lookup.SelectedId(_subject) ?? 0;
        Entity.TeacherId = Lookup.SelectedId(_teacher) ?? 0;
        Entity.Day = (DayOfWeek)(Lookup.SelectedId(_day) ?? 1);
        Entity.Number = Lookup.SelectedId(_number) ?? 1;
        Entity.Room = _room.Text.Trim();
        Entity.Type = Lookup.SelectedEnum<LessonType>(_type);
    }

    private void CheckConflicts()
    {
        if (_loading) return;

        var probe = new Lesson
        {
            Id = Entity.Id,
            GroupId = Lookup.SelectedId(_group) ?? 0,
            TeacherId = Lookup.SelectedId(_teacher) ?? 0,
            Day = (DayOfWeek)(Lookup.SelectedId(_day) ?? 1),
            Number = Lookup.SelectedId(_number) ?? 1,
            Room = _room.Text.Trim()
        };

        var conflicts = App.Schedule.FindConflicts(probe);
        if (conflicts.Count == 0)
        {
            _conflicts.Text = "Конфліктів у розкладі немає — час, група, викладач та аудиторія вільні.";
            _conflicts.ForeColor = Theme.Success;
        }
        else
        {
            _conflicts.Text = string.Join(Environment.NewLine, conflicts.Select(c => "• " + c));
            _conflicts.ForeColor = Theme.Danger;
        }
    }
}
