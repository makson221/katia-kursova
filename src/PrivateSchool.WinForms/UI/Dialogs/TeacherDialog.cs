using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Dialogs;

public sealed class TeacherDialog : EntityDialog<Teacher>
{
    private static readonly string[] Positions =
        { "Викладач", "Старший викладач", "Доцент", "Професор", "Завідувач кафедри", "Методист" };

    private static readonly string[] Degrees =
    {
        "", "Кандидат технічних наук", "Кандидат фізико-математичних наук", "Кандидат педагогічних наук",
        "Кандидат економічних наук", "Доктор філософії (PhD)", "Доктор технічних наук", "Доктор економічних наук"
    };

    private readonly TextBox _lastName = NewTextBox(50);
    private readonly TextBox _firstName = NewTextBox(50);
    private readonly TextBox _middleName = NewTextBox(50);
    private readonly DateTimePicker _birthDate = NewDatePicker();
    private readonly TextBox _phone = NewTextBox(16);
    private readonly TextBox _email = NewTextBox(80);
    private readonly ComboBox _position = new() { DropDownStyle = ComboBoxStyle.DropDown };
    private readonly ComboBox _degree = new() { DropDownStyle = ComboBoxStyle.DropDown };
    private readonly TextBox _specialization = NewTextBox(100);
    private readonly DateTimePicker _hireDate = NewDatePicker();
    private readonly NumericUpDown _rate = NewNumeric(0, 10000, 2, 10);

    public TeacherDialog(SchoolSystem app, Teacher entity, bool isNew, Action<Teacher> save)
        : base(app, entity, isNew, save) { }

    protected override string EntityName => "викладача";

    protected override void BuildFields()
    {
        AddSection("Особисті дані");
        AddField("Прізвище *", _lastName);
        AddField("Ім'я *", _firstName);
        AddField("По батькові", _middleName);
        AddField("Дата народження *", _birthDate);
        AddField("Телефон", _phone);
        AddField("Електронна пошта", _email);

        AddSection("Кадрові дані");
        AddField("Посада *", _position);
        AddField("Науковий ступінь", _degree);
        AddField("Спеціалізація", _specialization);
        AddField("Дата прийому на роботу", _hireDate);
        AddField("Ставка за годину, грн", _rate);

        _position.Items.AddRange(Positions);
        _degree.Items.AddRange(Degrees);
    }

    protected override void BindToControls()
    {
        _lastName.Text = Entity.LastName;
        _firstName.Text = Entity.FirstName;
        _middleName.Text = Entity.MiddleName;
        SetDate(_birthDate, Entity.BirthDate);
        _phone.Text = Entity.Phone;
        _phone.PlaceholderText = "+380XXXXXXXXX";
        _email.Text = Entity.Email;
        _position.Text = Entity.Position;
        _degree.Text = Entity.AcademicDegree;
        _specialization.Text = Entity.Specialization;
        SetDate(_hireDate, Entity.HireDate);
        SetNumber(_rate, Entity.HourlyRate);
    }

    protected override void ApplyToEntity()
    {
        Entity.LastName = _lastName.Text.Trim();
        Entity.FirstName = _firstName.Text.Trim();
        Entity.MiddleName = _middleName.Text.Trim();
        Entity.BirthDate = _birthDate.Value.Date;
        Entity.Phone = _phone.Text.Trim();
        Entity.Email = _email.Text.Trim();
        Entity.Position = _position.Text.Trim();
        Entity.AcademicDegree = _degree.Text.Trim();
        Entity.Specialization = _specialization.Text.Trim();
        Entity.HireDate = _hireDate.Value.Date;
        Entity.HourlyRate = _rate.Value;
    }
}
