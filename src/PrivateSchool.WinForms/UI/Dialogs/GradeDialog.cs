using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Dialogs;

public sealed class GradeDialog : EntityDialog<Grade>
{
    private readonly ComboBox _student = Lookup.SearchableCombo();
    private readonly ComboBox _subject = Lookup.DropDownList();
    private readonly ComboBox _type = Lookup.DropDownList();
    private readonly NumericUpDown _score = NewNumeric(GradeScale.MinScore, GradeScale.MaxScore);
    private readonly DateTimePicker _date = NewDatePicker();
    private readonly TextBox _comment = NewTextBox(200);
    private readonly Label _scale = new() { AutoSize = false, Height = 26, Font = Theme.BoldFont };

    public GradeDialog(SchoolSystem app, Grade entity, bool isNew, Action<Grade> save)
        : base(app, entity, isNew, save) { }

    protected override string EntityName => "оцінки";

    protected override void BuildFields()
    {
        AddField("Студент *", _student, span: 2);
        AddField("Дисципліна *", _subject, span: 2);
        AddField("Вид контролю", _type);
        AddField("Дата *", _date);
        AddField("Бали (0–100) *", _score);
        AddField("Шкала ЄКТС / національна", _scale);
        AddField("Коментар", _comment, span: 2);

        _score.ValueChanged += (_, _) => UpdateScale();
    }

    protected override void BindToControls()
    {
        Lookup.Fill(_student, Lookup.Students(App.Db, s => s.IsStudying || s.Id == Entity.StudentId),
            Entity.StudentId == 0 ? null : Entity.StudentId);
        if (Entity.StudentId == 0) _student.SelectedIndex = -1;

        var subjects = App.GradableSubjects();
        Lookup.Fill(_subject, Lookup.Subjects(subjects), Entity.SubjectId == 0 ? null : Entity.SubjectId);
        if (Entity.SubjectId == 0 && subjects.Count != 1) _subject.SelectedIndex = -1;

        Lookup.FillEnum(_type, Entity.Type);
        SetNumber(_score, Entity.Score);
        _date.MaxDate = DateTime.Today;
        SetDate(_date, Entity.Date);
        _comment.Text = Entity.Comment;
        UpdateScale();
    }

    protected override void ApplyToEntity()
    {
        Entity.StudentId = Lookup.SelectedId(_student) ?? 0;
        Entity.SubjectId = Lookup.SelectedId(_subject) ?? 0;
        Entity.Type = Lookup.SelectedEnum<GradeType>(_type);
        Entity.Score = (int)_score.Value;
        Entity.Date = _date.Value.Date;
        Entity.Comment = _comment.Text.Trim();
    }

    protected override IEnumerable<string> ValidateForm()
    {
        if (!App.CanEditGrade(Entity))
            yield return "Ви можете виставляти оцінки лише з дисциплін, які викладаєте.";
    }

    private void UpdateScale()
    {
        var score = (int)_score.Value;
        _scale.Text = $"{GradeScale.ToEcts(score)}  ·  {GradeScale.ToNational(score)} ({GradeScale.ToNationalText(score)})";
        _scale.ForeColor = GradeScale.IsPassed(score) ? Theme.Success : Theme.Danger;
    }
}
