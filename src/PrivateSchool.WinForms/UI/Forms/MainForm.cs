using System.ComponentModel;
using System.Diagnostics;
using PrivateSchool.Core.Models;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;
using PrivateSchool.WinForms.UI.Views;

namespace PrivateSchool.WinForms.UI.Forms;

/// <summary>
/// Головне вікно: бічна навігація, меню, рядок стану та область розділів.
/// </summary>
[DesignerCategory("Code")]
public sealed class MainForm : Form
{
    /// <summary>Опис розділу програми.</summary>
    private sealed record Section(string Title, string Glyph, Permission? Required, Func<ViewBase> Factory);

    private readonly SchoolSystem _app;
    private readonly string _dataPath;
    private readonly Panel _host;
    private readonly List<SidebarButton> _navButtons = new();
    private readonly Dictionary<Section, ViewBase> _views = new();
    private readonly ToolStripStatusLabel _saveStatus;
    private ViewBase? _current;

    public MainForm(SchoolSystem app, string dataPath)
    {
        _app = app;
        _dataPath = dataPath;

        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        Text = $"EduManager — {_app.Db.InstitutionName}";
        Font = Theme.BaseFont;
        BackColor = Theme.Background;
        ClientSize = new Size(1360, 820);
        MinimumSize = new Size(1150, 680);
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;

        var sections = new List<Section>
        {
            new("Головна", Theme.Glyphs.Home, null, () => new DashboardView(_app)),
            new("Студенти", Theme.Glyphs.People, Permission.ViewStudents, () => new StudentsView(_app)),
            new("Викладачі", Theme.Glyphs.Teacher, Permission.ManageTeachers, () => new TeachersView(_app)),
            new("Групи", Theme.Glyphs.Group, Permission.ManageGroups, () => new GroupsView(_app)),
            new("Дисципліни", Theme.Glyphs.Library, Permission.ManageSubjects, () => new SubjectsView(_app)),
            new("Успішність", Theme.Glyphs.Star, Permission.ManageGrades, () => new GradesView(_app)),
            new("Оплати", Theme.Glyphs.Payment, Permission.ViewPayments, () => new PaymentsView(_app)),
            new("Розклад", Theme.Glyphs.Calendar, null, () => new ScheduleView(_app)),
            new("Звіти", Theme.Glyphs.Report, Permission.ViewReports, () => new ReportsView(_app)),
            new("Користувачі", Theme.Glyphs.Users, Permission.ManageUsers, () => new UsersView(_app))
        }.Where(s => s.Required == null || _app.Auth.Can(s.Required.Value)).ToList();

        _host = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background };

        var sidebar = BuildSidebar(sections);
        var menu = BuildMenu(sections);

        var status = new StatusStrip { BackColor = Theme.Surface, SizingGrip = true };
        var user = _app.Auth.CurrentUser!;
        status.Items.Add(new ToolStripStatusLabel($"Користувач: {user.DisplayName} ({user.Role.ToDisplay()})"));
        status.Items.Add(new ToolStripStatusLabel { Spring = true });
        _saveStatus = new ToolStripStatusLabel("Дані завантажено") { ForeColor = Theme.TextMuted };
        status.Items.Add(_saveStatus);
        status.Items.Add(new ToolStripStatusLabel($"Файл даних: {Path.GetFileName(_dataPath)}") { ForeColor = Theme.TextMuted, ToolTipText = _dataPath });

        Controls.Add(_host);
        Controls.Add(sidebar);
        Controls.Add(status);
        Controls.Add(menu);
        MainMenuStrip = menu;

        _app.Db.DataChanged += OnDataChanged;
        Load += (_, _) => Navigate(sections[0]);
    }

    /// <summary>true — користувач обрав "Вийти з облікового запису" (потрібно знову показати вікно входу).</summary>
    public bool LogoutRequested { get; private set; }

    private Panel BuildSidebar(IReadOnlyList<Section> sections)
    {
        var sidebar = new Panel { Dock = DockStyle.Left, Width = 240, BackColor = Theme.Sidebar };

        var logo = new Panel { Dock = DockStyle.Top, Height = 96, BackColor = Theme.Sidebar };
        logo.Paint += (_, e) =>
        {
            using var big = new Font(Theme.FontName, 17f, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, "EduManager", big, new Point(22, 22), Color.White);
            TextRenderer.DrawText(e.Graphics, _app.Db.InstitutionName, Theme.SmallFont,
                new Rectangle(24, 58, logo.Width - 32, 20), Color.FromArgb(148, 163, 184), TextFormatFlags.EndEllipsis);
        };

        var nav = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Sidebar, Padding = new Padding(0, 8, 0, 0) };
        foreach (var section in sections.Reverse())
        {
            var button = new SidebarButton(section.Title, section.Glyph) { Key = section };
            button.Click += (_, _) => Navigate(section);
            nav.Controls.Add(button);
            _navButtons.Insert(0, button);
        }

        var user = _app.Auth.CurrentUser!;
        var userPanel = new Panel { Dock = DockStyle.Bottom, Height = 150, BackColor = Theme.Sidebar, Padding = new Padding(0, 8, 0, 8) };
        var userInfo = new Panel { Dock = DockStyle.Top, Height = 56 };
        userInfo.Paint += (_, e) =>
        {
            using var line = new Pen(Theme.SidebarHover);
            e.Graphics.DrawLine(line, 16, 0, userInfo.Width - 16, 0);
            var initials = string.Concat(user.DisplayName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => w[0]));
            using var circle = new SolidBrush(Theme.Primary);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(circle, 20, 12, 36, 36);
            TextRenderer.DrawText(e.Graphics, initials, Theme.BoldFont, new Rectangle(20, 12, 36, 36), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            TextRenderer.DrawText(e.Graphics, user.DisplayName, Theme.BoldFont, new Rectangle(64, 12, userInfo.Width - 72, 20),
                Color.White, TextFormatFlags.EndEllipsis);
            TextRenderer.DrawText(e.Graphics, user.Role.ToDisplay(), Theme.SmallFont, new Rectangle(64, 31, userInfo.Width - 72, 18),
                Color.FromArgb(148, 163, 184), TextFormatFlags.EndEllipsis);
        };
        var logout = new SidebarButton("Вийти", Theme.Glyphs.Power);
        logout.Click += (_, _) => Logout();
        var password = new SidebarButton("Змінити пароль", Theme.Glyphs.Key);
        password.Click += (_, _) => ChangePassword();
        userPanel.Controls.Add(logout);
        userPanel.Controls.Add(password);
        userPanel.Controls.Add(userInfo);

        sidebar.Controls.Add(nav);
        sidebar.Controls.Add(userPanel);
        sidebar.Controls.Add(logo);
        return sidebar;
    }

    private MenuStrip BuildMenu(IReadOnlyList<Section> sections)
    {
        var menu = new MenuStrip { BackColor = Theme.Surface, Font = Theme.BaseFont, Padding = new Padding(6, 3, 0, 3) };

        var file = new ToolStripMenuItem("&Файл");
        file.DropDownItems.Add(new ToolStripMenuItem("Створити резервну копію…", null, (_, _) => Backup()) { ShortcutKeys = Keys.Control | Keys.B });
        file.DropDownItems.Add(new ToolStripMenuItem("Відкрити теку з даними", null, (_, _) => OpenDataFolder()));
        file.DropDownItems.Add(new ToolStripSeparator());
        file.DropDownItems.Add(new ToolStripMenuItem("Змінити пароль…", null, (_, _) => ChangePassword()));
        file.DropDownItems.Add(new ToolStripMenuItem("Вийти з облікового запису", null, (_, _) => Logout()));
        file.DropDownItems.Add(new ToolStripSeparator());
        file.DropDownItems.Add(new ToolStripMenuItem("Вихід", null, (_, _) => Close()) { ShortcutKeys = Keys.Alt | Keys.F4 });

        var view = new ToolStripMenuItem("&Розділи");
        var index = 1;
        foreach (var section in sections)
        {
            var item = new ToolStripMenuItem(section.Title, null, (_, _) => Navigate(section));
            if (index <= 9) item.ShortcutKeys = Keys.Control | (Keys)((int)Keys.D0 + index);
            view.DropDownItems.Add(item);
            index++;
        }
        view.DropDownItems.Add(new ToolStripSeparator());
        view.DropDownItems.Add(new ToolStripMenuItem("Оновити дані", null, (_, _) => _current?.RefreshData()) { ShortcutKeys = Keys.F5 });

        var help = new ToolStripMenuItem("&Довідка");
        help.DropDownItems.Add(new ToolStripMenuItem("Про програму", null, (_, _) => ShowAbout()) { ShortcutKeys = Keys.F1 });

        menu.Items.AddRange(new ToolStripItem[] { file, view, help });
        return menu;
    }

    private void Navigate(Section section)
    {
        try
        {
            if (!_views.TryGetValue(section, out var view))
            {
                view = section.Factory();
                _views[section] = view;
            }

            if (_current != view)
            {
                _host.SuspendLayout();
                _host.Controls.Clear();
                _host.Controls.Add(view);
                _host.ResumeLayout();
                _current = view;
            }

            foreach (var button in _navButtons) button.Active = Equals(button.Key, section);
            view.OnActivated();
        }
        catch (Exception ex)
        {
            Messages.Exception(ex, this);
        }
    }

    private void OnDataChanged(object? sender, EventArgs e) =>
        _saveStatus.Text = $"Зміни збережено о {DateTime.Now:HH:mm:ss}";

    private void Backup()
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Резервна копія бази даних",
            Filter = "Файл даних JSON (*.json)|*.json",
            FileName = $"EduManager-backup-{DateTime.Now:yyyy-MM-dd-HHmm}.json"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            _app.Db.Save();
            File.Copy(_dataPath, dialog.FileName, overwrite: true);
            Messages.Info($"Резервну копію створено:\n{dialog.FileName}", this);
        }
        catch (Exception ex)
        {
            Messages.Exception(ex, this);
        }
    }

    private void OpenDataFolder()
    {
        var folder = Path.GetDirectoryName(Path.GetFullPath(_dataPath));
        if (folder != null && Directory.Exists(folder))
            Process.Start(new ProcessStartInfo(folder) { UseShellExecute = true });
    }

    private void ChangePassword()
    {
        using var form = new ChangePasswordForm(_app);
        form.ShowDialog(this);
    }

    private void ShowAbout()
    {
        using var form = new AboutForm();
        form.ShowDialog(this);
    }

    private void Logout()
    {
        if (!Messages.Confirm("Вийти з облікового запису?", this)) return;
        LogoutRequested = true;
        Close();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _app.Db.DataChanged -= OnDataChanged;
        base.OnFormClosed(e);
    }
}
