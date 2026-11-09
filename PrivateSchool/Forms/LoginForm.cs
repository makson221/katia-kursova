using System;
using System.Windows.Forms;
using PrivateSchool.Models;
using PrivateSchool.Services;
using PrivateSchool.Utils;

namespace PrivateSchool.Forms
{
    /// <summary>
    /// Форма авторизації користувача.
    /// </summary>
    public partial class LoginForm : Form
    {
        private readonly AuthService _auth;

        public LoginForm(AuthService auth, string schoolName)
        {
            InitializeComponent();
            _auth = auth;
            lblSchool.Text = schoolName;
        }

        /// <summary>Користувач, який успішно увійшов до системи.</summary>
        public User AuthenticatedUser { get; private set; }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text.Trim().Length == 0 || txtPassword.Text.Length == 0)
            {
                UiHelper.ShowWarning(this, "Введіть логін і пароль.");
                return;
            }

            try
            {
                User user = _auth.Login(txtLogin.Text, txtPassword.Text);
                if (user == null)
                {
                    UiHelper.ShowWarning(this, string.Format(
                        "Невірний логін або пароль. Залишилось спроб: {0}.", _auth.RemainingAttempts));
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                AuthenticatedUser = user;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ValidationException ex)
            {
                UiHelper.ShowError(this, ex.Message);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
