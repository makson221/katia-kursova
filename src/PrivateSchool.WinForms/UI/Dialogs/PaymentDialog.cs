using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Dialogs;

public sealed class PaymentDialog : EntityDialog<Payment>
{
    private readonly ComboBox _student = Lookup.SearchableCombo();
    private readonly NumericUpDown _amount = NewNumeric(0, 1_000_000, 2, 100);
    private readonly DateTimePicker _date = NewDatePicker();
    private readonly ComboBox _method = Lookup.DropDownList();
    private readonly ComboBox _purpose = new() { DropDownStyle = ComboBoxStyle.DropDown, MaxLength = 200 };
    private readonly TextBox _receipt = NewTextBox(30);
    private readonly Label _info = new() { AutoSize = false, Height = 44, ForeColor = Theme.TextMuted };
    private readonly LinkLabel _fillFee = new() { Text = "Підставити місячну плату", AutoSize = true };
    private readonly LinkLabel _fillDebt = new() { Text = "Підставити суму боргу", AutoSize = true };

    public PaymentDialog(SchoolSystem app, Payment entity, bool isNew, Action<Payment> save)
        : base(app, entity, isNew, save) { }

    protected override string EntityName => "оплати";

    protected override void BuildFields()
    {
        AddField("Студент *", _student, span: 2);
        AddField("Стан розрахунків", _info, span: 2);
        AddField("Сума, грн *", _amount);
        AddField("Дата оплати *", _date);
        AddField("Швидке заповнення", _fillFee);
        AddField(" ", _fillDebt);
        AddField("Спосіб оплати", _method);
        AddField("Номер квитанції *", _receipt);
        AddField("Призначення платежу", _purpose, span: 2);

        _student.SelectedIndexChanged += (_, _) => UpdateInfo();
        _fillFee.LinkClicked += (_, _) =>
        {
            if (SelectedStudent() is { } s) SetNumber(_amount, App.Finance.GetMonthlyFee(s));
        };
        _fillDebt.LinkClicked += (_, _) =>
        {
            if (SelectedStudent() is { } s)
            {
                var debt = App.Finance.GetBalance(s).Debt;
                if (debt > 0) SetNumber(_amount, debt);
                else Messages.Info("У студента немає заборгованості.", this);
            }
        };
    }

    protected override void BindToControls()
    {
        Lookup.Fill(_student, Lookup.Students(App.Db), Entity.StudentId == 0 ? null : Entity.StudentId);
        if (Entity.StudentId == 0) _student.SelectedIndex = -1;

        SetNumber(_amount, Entity.Amount);
        _date.MaxDate = DateTime.Today;
        SetDate(_date, Entity.Date);
        Lookup.FillEnum(_method, Entity.Method);

        var month = DateTime.Today.ToString("MMMM yyyy", new System.Globalization.CultureInfo("uk-UA"));
        _purpose.Items.AddRange(new object[]
        {
            $"Оплата за навчання ({month})", "Оплата за навчання (семестр)", "Оплата за навчання (рік)",
            "Погашення заборгованості", "Додаткові освітні послуги"
        });
        _purpose.Text = IsNew ? (string)_purpose.Items[0]! : Entity.Purpose;
        _receipt.Text = IsNew && Entity.ReceiptNumber.Length == 0 ? App.Finance.NextReceiptNumber() : Entity.ReceiptNumber;
        UpdateInfo();
    }

    protected override void ApplyToEntity()
    {
        Entity.StudentId = Lookup.SelectedId(_student) ?? 0;
        Entity.Amount = _amount.Value;
        Entity.Date = _date.Value.Date;
        Entity.Method = Lookup.SelectedEnum<PaymentMethod>(_method);
        Entity.Purpose = _purpose.Text.Trim();
        Entity.ReceiptNumber = _receipt.Text.Trim();
    }

    protected override IEnumerable<string> ValidateForm()
    {
        if (App.Db.Payments.Exists(p => p.Id != Entity.Id && p.ReceiptNumber == Entity.ReceiptNumber))
            yield return $"Квитанція № {Entity.ReceiptNumber} вже зареєстрована.";
    }

    private Student? SelectedStudent() => App.Db.Students.GetById(Lookup.SelectedId(_student) ?? 0);

    private void UpdateInfo()
    {
        var student = SelectedStudent();
        if (student == null)
        {
            _info.Text = "Оберіть студента (можна почати вводити прізвище).";
            _info.ForeColor = Theme.TextMuted;
            return;
        }

        var b = App.Finance.GetBalance(student);
        _info.Text = $"Плата: {b.MonthlyFee:N2} грн/міс. · Нараховано: {b.Accrued:N2} · Сплачено: {b.Paid:N2}\n" +
                     (b.HasDebt ? $"Заборгованість: {b.Debt:N2} грн" : $"Заборгованості немає (переплата {b.Balance:N2} грн)");
        _info.ForeColor = b.HasDebt ? Theme.Danger : Theme.Success;
    }
}
