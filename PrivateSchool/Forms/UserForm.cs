using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма додавання / редагування користувача системи.
    /// </summary>
    public partial class UserForm : EditFormBase
    {
        private readonly User _user;

        public UserForm(SchoolService service, User user)
            : base(service, user == null, "користувач")
        {
            InitializeComponent();
            _user = user == null ? new User() : user.Clone<User>();

            txtLogin.Text = _user.Login;
            txtFullName.Text = _user.FullName;
            UiHelper.Fill(cmbRole, EnumHelper.GetItems<UserRole>(), _user.Role);
            lblHint.Text = IsNew
                ? "Пароль — щонайменше 6 символів."
                : "Залиште поля пароля порожніми,\nщоб не змінювати пароль.";
        }

        protected override void SaveEntity()
        {
            if (txtPassword.Text != txtConfirm.Text)
                throw new ValidationException("Пароль і підтвердження не збігаються.");

            _user.Login = txtLogin.Text;
            _user.FullName = txtFullName.Text;
            _user.Role = UiHelper.GetValue<UserRole>(cmbRole);

            if (IsNew)
                Service.AddUser(_user, txtPassword.Text);
            else
                Service.UpdateUser(_user, txtPassword.Text);
        }
    }
}
