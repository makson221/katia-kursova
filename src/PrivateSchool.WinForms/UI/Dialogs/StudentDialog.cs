using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Dialogs;

public sealed class StudentDialog : EntityDialog<Student>
{
    private readonly TextBox _lastName = NewTextBox(50);
    private readonly TextBox _firstName = NewTextBox(50);
    private readonly TextBox _middleName = NewTextBox(50);
    private readonly DateTimePicker _birthDate = NewDatePicker();
    private readonly TextBox _phone = NewTextBox(16);
    private readonly TextBox _email = NewTextBox(80);
    private readonly ComboBox _group = Lookup.DropDownList();
    private readonly TextBox _contract = NewTextBox(30);
    private readonly DateTimePicker _enrollment = NewDatePicker();
    private readonly ComboBox _status = Lookup.DropDownList();
    private readonly DateTimePicker _endDate = NewDatePicker(optional: true);
    private readonly NumericUpDown _discount = NewNumeric(0, 100, 1, 5);
    private readonly TextBox _parentName = NewTextBox(100);
    private readonly TextBox _parentPhone = NewTextBox(16);
    private readonly Label _feeInfo = new() { AutoSize = false, Height = 22, ForeColor = Theme.TextMuted };

    public StudentDialog(SchoolSystem app, Student entity, bool isNew, Action<Student> save)
        : base(app, entity, isNew, save) { }

    protected override string EntityName => "студента";

    protected override void BuildFields()
    {
        AddSection("Особисті дані");
        AddField("Прізвище *", _lastName);
        AddField("Ім'я *", _firstName);
        AddField("По батькові", _middleName);
        AddField("Дата народження *", _birthDate);
        AddField("Телефон", _phone);
        AddField("Електронна пошта", _email);

        AddSection("Навчання та договір");
        AddField("Група *", _group);
        AddField("Номер договору *", _contract);
        AddField("Дата зарахування *", _enrollment);
        AddField("Статус", _status);
        AddField("Дата завершення навчання", _endDate);
        AddField("Знижка, %", _discount);
        AddField("Вартість навчання", _feeInfo, span: 2);

        AddSection("Батьки / законні представники");
        AddField("ПІБ одного з батьків", _parentName);
        AddField("Телефон батьків", _parentPhone);

        _group.SelectedIndexChanged += (_, _) => UpdateFeeInfo();
        _discount.ValueChanged += (_, _) => UpdateFeeInfo();
        _status.SelectedIndexChanged += (_, _) =>
        {
            var studying = Lookup.SelectedEnum<StudentStatus>(_status) is StudentStatus.Active or StudentStatus.AcademicLeave;
            if (!studying && !_endDate.Checked) { _endDate.Checked = true; _endDate.Value = DateTime.Today; }
        };
    }

    protected override void BindToControls()
    {
        _lastName.Text = Entity.LastName;
        _firstName.Text = Entity.FirstName;
        _middleName.Text = Entity.MiddleName;
        SetDate(_birthDate, Entity.BirthDate);
        _phone.Text = Entity.Phone;
        _email.Text = Entity.Email;
        _phone.PlaceholderText = "+380XXXXXXXXX";
        _parentPhone.PlaceholderText = "+380XXXXXXXXX";

        Lookup.Fill(_group, Lookup.Groups(App.Db), Entity.GroupId == 0 ? null : Entity.GroupId);
        if (IsNew && Entity.GroupId == 0) _group.SelectedIndex = -1;

        _contract.Text = IsNew && Entity.ContractNumber.Length == 0 ? SuggestContractNumber() : Entity.ContractNumber;
        SetDate(_enrollment, Entity.EnrollmentDate);
        Lookup.FillEnum(_status, Entity.Status);
        _endDate.Checked = Entity.EndDate.HasValue;
        if (Entity.EndDate.HasValue) SetDate(_endDate, Entity.EndDate.Value);
        SetNumber(_discount, Entity.DiscountPercent);
        _parentName.Text = Entity.ParentName;
        _parentPhone.Text = Entity.ParentPhone;
        UpdateFeeInfo();
    }

    protected override void ApplyToEntity()
    {
        Entity.LastName = _lastName.Text.Trim();
        Entity.FirstName = _firstName.Text.Trim();
        Entity.MiddleName = _middleName.Text.Trim();
        Entity.BirthDate = _birthDate.Value.Date;
        Entity.Phone = _phone.Text.Trim();
        Entity.Email = _email.Text.Trim();
        Entity.GroupId = Lookup.SelectedId(_group) ?? 0;
        Entity.ContractNumber = _contract.Text.Trim();
        Entity.EnrollmentDate = _enrollment.Value.Date;
        Entity.Status = Lookup.SelectedEnum<StudentStatus>(_status);
        Entity.EndDate = _endDate.Checked ? _endDate.Value.Date : null;
        Entity.DiscountPercent = _discount.Value;
        Entity.ParentName = _parentName.Text.Trim();
        Entity.ParentPhone = _parentPhone.Text.Trim();
    }

    protected override IEnumerable<string> ValidateForm()
    {
        if (App.Db.Students.Exists(s => s.Id != Entity.Id &&
                                        string.Equals(s.ContractNumber, Entity.ContractNumber, StringComparison.OrdinalIgnoreCase)))
            yield return $"Договір № {Entity.ContractNumber} вже зареєстровано за іншим студентом.";

        var group = App.Db.Groups.GetById(Entity.GroupId);
        if (group != null && Entity.IsStudying)
        {
            var occupied = App.Db.Students.Find(s => s.GroupId == group.Id && s.Id != Entity.Id && s.IsStudying).Count();
            if (occupied >= group.Capacity)
                yield return $"Група {group.Name} заповнена ({occupied} з {group.Capacity} місць).";
        }
    }

    private void UpdateFeeInfo()
    {
        var group = App.Db.Groups.GetById(Lookup.SelectedId(_group) ?? 0);
        if (group == null)
        {
            _feeInfo.Text = "Оберіть групу, щоб побачити вартість навчання.";
            return;
        }
        var fee = group.MonthlyFee * (1 - _discount.Value / 100m);
        _feeInfo.Text = $"Базова вартість: {group.MonthlyFee:N2} грн/міс.  →  З урахуванням знижки: {fee:N2} грн/міс.";
    }

    private string SuggestContractNumber()
    {
        var year = DateTime.Today.Year;
        var prefix = $"Д-{year}-";
        var next = App.Db.Students.Find(s => s.ContractNumber.StartsWith(prefix))
            .Select(s => int.TryParse(s.ContractNumber[prefix.Length..], out var n) ? n : 0)
            .DefaultIfEmpty(0).Max() + 1;
        return $"{prefix}{next:D3}";
    }
}
