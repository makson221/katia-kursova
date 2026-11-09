using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Dialogs;

public sealed class SubjectDialog : EntityDialog<Subject>
{
    private readonly TextBox _name = NewTextBox(100);
    private readonly NumericUpDown _hours = NewNumeric(1, 1000, 0, 30);
    private readonly ComboBox _control = Lookup.DropDownList();
    private readonly ComboBox _teacher = Lookup.DropDownList();
    private readonly TextBox _description = new() { Multiline = true, Height = 70, MaxLength = 500, ScrollBars = ScrollBars.Vertical };
    private readonly Label _credits = new() { AutoSize = false, Height = 22, ForeColor = Theme.TextMuted };

    public SubjectDialog(SchoolSystem app, Subject entity, bool isNew, Action<Subject> save)
        : base(app, entity, isNew, save) { }

    protected override string EntityName => "дисципліни";

    protected override void BuildFields()
    {
        AddField("Назва дисципліни *", _name, span: 2);
        AddField("Обсяг, год *", _hours);
        AddField("Форма контролю", _control);
        AddField("Провідний викладач", _teacher, span: 2);
        AddField("Опис", _description, span: 2);
        AddField("Кредити ЄКТС", _credits, span: 2);

        _hours.ValueChanged += (_, _) => _credits.Text = $"{_hours.Value / 30m:0.0} кредиту(ів) ЄКТС (1 кредит = 30 год)";
    }

    protected override void BindToControls()
    {
        _name.Text = Entity.Name;
        SetNumber(_hours, Entity.Hours);
        Lookup.FillEnum(_control, Entity.ControlForm);
        Lookup.Fill(_teacher, Lookup.Teachers(App.Db), Entity.TeacherId, "— не призначено —");
        _description.Text = Entity.Description;
        _credits.Text = $"{Entity.Hours / 30m:0.0} кредиту(ів) ЄКТС (1 кредит = 30 год)";
    }

    protected override void ApplyToEntity()
    {
        Entity.Name = _name.Text.Trim();
        Entity.Hours = (int)_hours.Value;
        Entity.ControlForm = Lookup.SelectedEnum<ControlForm>(_control);
        Entity.TeacherId = Lookup.SelectedId(_teacher);
        Entity.Description = _description.Text.Trim();
    }

    protected override IEnumerable<string> ValidateForm()
    {
        if (App.Db.Subjects.Exists(s => s.Id != Entity.Id && string.Equals(s.Name, Entity.Name, StringComparison.OrdinalIgnoreCase)))
            yield return "Дисципліна з такою назвою вже існує.";
    }
}
