using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Dialogs;

public sealed class GroupDialog : EntityDialog<Group>
{
    private readonly TextBox _name = NewTextBox(20);
    private readonly ComboBox _specialty = new() { DropDownStyle = ComboBoxStyle.DropDown };
    private readonly NumericUpDown _year = NewNumeric(1990, DateTime.Today.Year + 1);
    private readonly NumericUpDown _capacity = NewNumeric(1, 100);
    private readonly NumericUpDown _fee = NewNumeric(0, 1_000_000, 2, 100);
    private readonly ComboBox _curator = Lookup.DropDownList();

    public GroupDialog(SchoolSystem app, Group entity, bool isNew, Action<Group> save)
        : base(app, entity, isNew, save) { }

    protected override string EntityName => "групи";

    protected override void BuildFields()
    {
        AddField("Назва групи *", _name);
        AddField("Рік набору *", _year);
        AddField("Спеціальність *", _specialty, span: 2);
        AddField("Вартість навчання, грн/міс.", _fee);
        AddField("Місткість (місць)", _capacity);
        AddField("Куратор", _curator, span: 2);

        _year.ThousandsSeparator = false;
        _name.PlaceholderText = "напр. ПІ-25";
        _specialty.Items.AddRange(App.Db.Groups.GetAll().Select(g => g.Specialty).Distinct().OrderBy(s => s).ToArray<object>());
    }

    protected override void BindToControls()
    {
        _name.Text = Entity.Name;
        _specialty.Text = Entity.Specialty;
        SetNumber(_year, Entity.StartYear);
        SetNumber(_capacity, Entity.Capacity);
        SetNumber(_fee, Entity.MonthlyFee);
        Lookup.Fill(_curator, Lookup.Teachers(App.Db), Entity.CuratorId, "— не призначено —");
    }

    protected override void ApplyToEntity()
    {
        Entity.Name = _name.Text.Trim().ToUpperInvariant();
        Entity.Specialty = _specialty.Text.Trim();
        Entity.StartYear = (int)_year.Value;
        Entity.Capacity = (int)_capacity.Value;
        Entity.MonthlyFee = _fee.Value;
        Entity.CuratorId = Lookup.SelectedId(_curator);
    }

    protected override IEnumerable<string> ValidateForm()
    {
        if (App.Db.Groups.Exists(g => g.Id != Entity.Id && string.Equals(g.Name, Entity.Name, StringComparison.OrdinalIgnoreCase)))
            yield return $"Група з назвою {Entity.Name} вже існує.";

        var students = App.Db.Students.Find(s => s.GroupId == Entity.Id && s.IsStudying).Count();
        if (!IsNew && students > Entity.Capacity)
            yield return $"У групі вже навчається {students} студентів — місткість не може бути меншою.";
    }
}
