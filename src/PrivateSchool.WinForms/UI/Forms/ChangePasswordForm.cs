using System.ComponentModel;
using PrivateSchool.Core.Exceptions;
using PrivateSchool.Core.Services;
using PrivateSchool.WinForms.UI.Controls;

namespace PrivateSchool.WinForms.UI.Forms;

/// <summary>Зміна пароля поточного користувача.</summary>
[DesignerCategory("Code")]
public sealed class ChangePasswordForm : Form
{
    private readonly SchoolSystem _app;
    private readonly TextBox _old = PasswordBox();
    private readonly TextBox _new = PasswordBox();
    private readonly TextBox _confirm = PasswordBox();
    private readonly Label _error = new() { ForeColor = Theme.Danger, AutoSize = true, MaximumSize = new Size(300, 0) };

    public ChangePasswordForm(SchoolSystem app)
    {
        _app = app;
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        Text = "Зміна пароля";
        Font = Theme.BaseFont;
        BackColor = Theme.Surface;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;

        var layout = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Padding = new Padding(24) };
        layout.Controls.Add(new Label { Text = "Зміна пароля", Font = Theme.SectionFont, AutoSize = true, Margin = new Padding(0, 0, 0, 12) });
        AddField(layout, "Поточний пароль", _old);
        AddField(layout, "Новий пароль", _new);
        AddField(layout, "Підтвердження нового пароля", _confirm);
        layout.Controls.Add(_error);

        var buttons = new FlowLayoutPanel { FlowDirection = FlowDirection.RightToLeft, AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(0, 16, 0, 0) };
        var ok = new FlatButton("Змінити", ButtonVariant.Primary) { Margin = new Padding(8, 0, 0, 0) };
        var cancel = new FlatButton("Скасувати") { DialogResult = DialogResult.Cancel };
        ok.Click += (_, _) => Apply();
        buttons.Controls.Add(ok);
        buttons.Controls.Add(cancel);
        layout.Controls.Add(buttons);
        Controls.Add(layout);

        AcceptButton = ok;
        CancelButton = cancel;
        _new.PlaceholderText = $"мін. {AuthService.MinPasswordLength} символів, літери й цифри";
    }

    private static TextBox PasswordBox() => new() { UseSystemPasswordChar = true, Width = 300, BorderStyle = BorderStyle.FixedSingle };

    private static void AddField(TableLayoutPanel layout, string label, Control control)
    {
        layout.Controls.Add(new Label { Text = label, Font = Theme.LabelFont, ForeColor = Theme.TextMuted, AutoSize = true, Margin = new Padding(0, 8, 0, 4) });
        layout.Controls.Add(control);
    }

    private void Apply()
    {
        if (_new.Text != _confirm.Text)
        {
            _error.Text = "Новий пароль і підтвердження не збігаються.";
            return;
        }
        try
        {
            _app.Auth.ChangeOwnPassword(_old.Text, _new.Text);
            Messages.Info("Пароль успішно змінено.", this);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (DomainException ex)
        {
            _error.Text = ex.Message;
        }
    }
}
