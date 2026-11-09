using System.ComponentModel;
using System.Drawing.Drawing2D;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;

namespace PrivateSchool.WinForms.UI.Forms;

/// <summary>
/// Вікно входу до системи з обмеженням кількості невдалих спроб.
/// </summary>
[DesignerCategory("Code")]
public sealed class LoginForm : Form
{
    private const int MaxAttempts = 3;
    private const int LockSeconds = 15;

    private readonly SchoolSystem _app;
    private readonly TextBox _login;
    private readonly TextBox _password;
    private readonly Label _error;
    private readonly FlatButton _enter;
    private readonly System.Windows.Forms.Timer _lockTimer = new() { Interval = 1000 };
    private int _failedAttempts;
    private int _secondsLeft;

    public LoginForm(SchoolSystem app)
    {
        _app = app;

        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        Text = "EduManager — вхід до системи";
        Font = Theme.BaseFont;
        ClientSize = new Size(860, 500);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Theme.Surface;

        // Ліва декоративна панель
        var hero = new Panel { Dock = DockStyle.Left, Width = 380 };
        hero.Paint += PaintHero;

        // Права панель з формою
        var form = new Panel { Dock = DockStyle.Fill, Padding = new Padding(56, 60, 56, 30), BackColor = Theme.Surface };
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1 };

        var title = new Label { Text = "Вхід до системи", Font = Theme.TitleFont, AutoSize = true, Margin = new Padding(0, 0, 0, 4) };
        var subtitle = new Label
        {
            Text = "Введіть облікові дані, видані адміністратором",
            ForeColor = Theme.TextMuted,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 24)
        };

        _login = new TextBox { Width = 360, Font = new Font(Theme.FontName, 11f), BorderStyle = BorderStyle.FixedSingle, MaxLength = 20 };
        _password = new TextBox
        {
            Width = 360,
            Font = new Font(Theme.FontName, 11f),
            BorderStyle = BorderStyle.FixedSingle,
            UseSystemPasswordChar = true,
            MaxLength = 64
        };
        var show = new CheckBox { Text = "Показати пароль", AutoSize = true, ForeColor = Theme.TextMuted, Margin = new Padding(0, 8, 0, 0) };
        show.CheckedChanged += (_, _) => _password.UseSystemPasswordChar = !show.Checked;

        _error = new Label { ForeColor = Theme.Danger, AutoSize = true, MaximumSize = new Size(360, 0), Margin = new Padding(0, 10, 0, 0) };

        _enter = new FlatButton("Увійти", ButtonVariant.Primary) { AutoSize = false, Width = 360, Height = 42, Margin = new Padding(0, 14, 0, 0) };
        _enter.Click += (_, _) => TryLogin();

        var hint = new Label
        {
            Text = "Демо-доступ:  admin / admin123   ·   manager / manager123   ·   teacher / teacher123",
            ForeColor = Theme.TextMuted,
            Font = Theme.SmallFont,
            AutoSize = true,
            MaximumSize = new Size(360, 0),
            Margin = new Padding(0, 24, 0, 0)
        };

        layout.Controls.Add(title);
        layout.Controls.Add(subtitle);
        layout.Controls.Add(FieldLabel("Логін"));
        layout.Controls.Add(_login);
        layout.Controls.Add(FieldLabel("Пароль"));
        layout.Controls.Add(_password);
        layout.Controls.Add(show);
        layout.Controls.Add(_error);
        layout.Controls.Add(_enter);
        layout.Controls.Add(hint);
        form.Controls.Add(layout);

        Controls.Add(form);
        Controls.Add(hero);

        AcceptButton = _enter;
        _lockTimer.Tick += OnLockTick;
        Shown += (_, _) => _login.Focus();
    }

    private static Label FieldLabel(string text) => new()
    {
        Text = text,
        Font = Theme.LabelFont,
        ForeColor = Theme.TextMuted,
        AutoSize = true,
        Margin = new Padding(0, 10, 0, 4)
    };

    private void TryLogin()
    {
        if (_secondsLeft > 0) return;

        if (string.IsNullOrWhiteSpace(_login.Text) || _password.Text.Length == 0)
        {
            _error.Text = "Введіть логін і пароль.";
            return;
        }

        if (_app.Auth.Login(_login.Text, _password.Text))
        {
            DialogResult = DialogResult.OK;
            Close();
            return;
        }

        _failedAttempts++;
        _password.Clear();
        _password.Focus();

        if (_failedAttempts >= MaxAttempts)
        {
            _failedAttempts = 0;
            _secondsLeft = LockSeconds;
            _enter.Enabled = false;
            _lockTimer.Start();
            OnLockTick(null, EventArgs.Empty);
        }
        else
        {
            _error.Text = $"Неправильний логін або пароль. Залишилось спроб: {MaxAttempts - _failedAttempts}.";
        }
    }

    private void OnLockTick(object? sender, EventArgs e)
    {
        if (_secondsLeft <= 0)
        {
            _lockTimer.Stop();
            _enter.Enabled = true;
            _error.Text = "Спробуйте увійти знову.";
            return;
        }
        _error.Text = $"Забагато невдалих спроб. Вхід заблоковано на {_secondsLeft} с.";
        _secondsLeft--;
    }

    private void PaintHero(object? sender, PaintEventArgs e)
    {
        var panel = (Control)sender!;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var brush = new LinearGradientBrush(panel.ClientRectangle, Theme.PrimaryDark, Theme.Violet, 135f))
            g.FillRectangle(brush, panel.ClientRectangle);

        // Декоративні кола
        using (var circle = new SolidBrush(Color.FromArgb(28, Color.White)))
        {
            g.FillEllipse(circle, -80, panel.Height - 220, 300, 300);
            g.FillEllipse(circle, panel.Width - 160, -60, 240, 240);
        }

        if (Theme.IconFontName != null)
        {
            using var icon = Theme.IconFont(34f)!;
            TextRenderer.DrawText(g, Theme.Glyphs.Teacher, icon, new Point(40, 70), Color.White);
        }

        using var big = new Font(Theme.FontName, 28f, FontStyle.Bold);
        TextRenderer.DrawText(g, "EduManager", big, new Point(36, 140), Color.White);
        TextRenderer.DrawText(g, "Автоматизована система управління\nприватним навчальним закладом",
            Theme.SubtitleFont, new Rectangle(40, 196, panel.Width - 70, 60), Color.FromArgb(224, 231, 255),
            TextFormatFlags.WordBreak);

        var features = new[] { "Облік студентів і викладачів", "Журнал успішності", "Договори та оплати", "Розклад і звіти" };
        var y = 290;
        foreach (var f in features)
        {
            using (var dot = new SolidBrush(Color.FromArgb(165, 180, 252)))
                g.FillEllipse(dot, 42, y + 6, 8, 8);
            TextRenderer.DrawText(g, f, Theme.BaseFont, new Point(60, y), Color.White);
            y += 28;
        }

        TextRenderer.DrawText(g, _app.Db.InstitutionName, Theme.SmallFont, new Point(40, panel.Height - 40),
            Color.FromArgb(199, 210, 254));
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _lockTimer.Dispose();
        base.Dispose(disposing);
    }
}
