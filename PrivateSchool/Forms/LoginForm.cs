using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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

            AppTheme.StyleButton(btnLogin, ButtonKind.Primary);
            btnLogin.Font = new Font(AppTheme.FontName, 11F, FontStyle.Bold);
            AppTheme.StyleButton(btnCancel, ButtonKind.Ghost);
#if NET5_0_OR_GREATER
            txtLogin.PlaceholderText = "Введіть логін";
            txtPassword.PlaceholderText = "Введіть пароль";
#endif
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

        /// <summary>Фірмова панель: градієнт, декоративні кола та логотип.</summary>
        private void panelBrand_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle area = panelBrand.ClientRectangle;
            float k = AppTheme.ScaleOf(g);

            using (LinearGradientBrush back = new LinearGradientBrush(area,
                       Color.FromArgb(37, 99, 235), Color.FromArgb(30, 41, 99), 60F))
                g.FillRectangle(back, area);

            using (SolidBrush circle = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
            {
                g.FillEllipse(circle, area.Width - 150 * k, -90 * k, 260 * k, 260 * k);
                g.FillEllipse(circle, -80 * k, area.Height - 140 * k, 220 * k, 220 * k);
            }

            Rectangle logo = new Rectangle((int)(40 * k), (int)(70 * k), (int)(60 * k), (int)(60 * k));
            using (GraphicsPath path = AppTheme.RoundedRect(logo, 14 * k))
            using (SolidBrush fill = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
            using (Pen border = new Pen(Color.FromArgb(90, 255, 255, 255)))
            {
                g.FillPath(fill, path);
                g.DrawPath(border, path);
            }
            AppTheme.DrawLogoMark(g, logo, 22F);
        }
    }
}
