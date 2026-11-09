using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Dialogs;

public sealed class UserDialog : EntityDialog<User>
{
    private readonly TextBox _login = NewTextBox(20);
    private readonly TextBox _displayName = NewTextBox(100);
    private readonly ComboBox _role = Lookup.DropDownList();
    private readonly ComboBox _teacher = Lookup.DropDownList();
    private readonly CheckBox _active = new() { Text = "Обліковий запис активний", AutoSize = true };
    private readonly TextBox _password = new() { UseSystemPasswordChar = true, MaxLength = 64 };
    private readonly TextBox _confirm = new() { UseSystemPasswordChar = true, MaxLength = 64 };

    public UserDialog(SchoolSystem app, User entity, bool isNew) : base(app, entity, isNew, null) { }

    protected override string EntityName => "користувача";

    protected override void BuildFields()
    {
        AddField("Логін *", _login);
        AddField("Роль", _role);
        AddField("Ім'я для відображення *", _displayName, span: 2);
        AddField("Викладач (для ролі «Викладач»)", _teacher, span: 2);
        AddSection(IsNew ? "Пароль" : "Новий пароль (залиште порожнім, щоб не змінювати)");
        AddField(IsNew ? "Пароль *" : "Новий пароль", _password);
        AddField("Підтвердження пароля", _confirm);
        AddField("Стан", _active, span: 2);

        _role.SelectedIndexChanged += (_, _) => UpdateTeacherState();
        _teacher.SelectedIndexChanged += (_, _) =>
        {
            if (_displayName.Text.Length == 0 && Lookup.SelectedId(_teacher) is int id)
                _displayName.Text = App.Db.Teachers.GetById(id)?.FullName ?? "";
        };
    }

    protected override void BindToControls()
    {
        _login.Text = Entity.Login;
        _displayName.Text = Entity.DisplayName;
        Lookup.FillEnum(_role, Entity.Role);
        Lookup.Fill(_teacher, Lookup.Teachers(App.Db), Entity.TeacherId, "— не пов'язано —");
        _active.Checked = Entity.IsActive;
        _password.PlaceholderText = $"щонайменше {AuthService.MinPasswordLength} символів, літери й цифри";
        UpdateTeacherState();
    }

    protected override void ApplyToEntity()
    {
        Entity.Login = _login.Text.Trim();
        Entity.DisplayName = _displayName.Text.Trim();
        Entity.Role = Lookup.SelectedEnum<UserRole>(_role);
        Entity.TeacherId = Entity.Role == UserRole.Teacher ? Lookup.SelectedId(_teacher) : null;
        Entity.IsActive = _active.Checked;
    }

    protected override IEnumerable<string> ValidateForm()
    {
        if (IsNew && _password.Text.Length == 0) yield return "Задайте пароль.";
        if (_password.Text != _confirm.Text) yield return "Паролі не збігаються.";
        if (_password.Text.Length > 0)
            foreach (var error in AuthService.ValidatePassword(_password.Text)) yield return error;
    }

    protected override void Save(User entity)
    {
        if (IsNew)
        {
            App.Auth.CreateUser(entity, _password.Text);
            return;
        }

        App.Auth.UpdateUser(entity);
        if (_password.Text.Length > 0) App.Auth.SetPassword(entity.Id, _password.Text);
    }

    private void UpdateTeacherState()
    {
        _teacher.Enabled = Lookup.SelectedEnum<UserRole>(_role) == UserRole.Teacher;
        if (!_teacher.Enabled && _teacher.Items.Count > 0) _teacher.SelectedIndex = 0;
    }
}
