using System.ComponentModel;
using PrivateSchool.WinForms.UI.Controls;

namespace PrivateSchool.WinForms.UI.Forms;

/// <summary>Відомості про програму.</summary>
[DesignerCategory("Code")]
public sealed class AboutForm : Form
{
    // Відредагуйте ці рядки під свої дані для титульної інформації курсового проєкту.
    private const string Author = "студентка групи ______  ПІБ ______________";
    private const string Supervisor = "______________________";

    public AboutForm()
    {
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96F, 96F);
        Text = "Про програму";
        Font = Theme.BaseFont;
        BackColor = Theme.Surface;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;

        var version = typeof(AboutForm).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";
        var layout = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Padding = new Padding(28) };
        layout.Controls.Add(new Label { Text = "EduManager", Font = Theme.TitleFont, ForeColor = Theme.Primary, AutoSize = true });
        layout.Controls.Add(new Label { Text = $"Версія {version}", ForeColor = Theme.TextMuted, AutoSize = true, Margin = new Padding(2, 0, 0, 12) });
        layout.Controls.Add(new Label
        {
            Text = "Автоматизована система приватного навчального закладу.\n\n" +
                   "Курсовий проєкт з дисципліни «Об'єктно-орієнтоване програмування».\n" +
                   "Мова: C# (.NET 8), інтерфейс: Windows Forms, зберігання даних: JSON.\n\n" +
                   $"Виконав(ла): {Author}\nКерівник: {Supervisor}\n\n" +
                   "Гарячі клавіші: Ctrl+N — додати, F2 — редагувати, Delete — видалити,\n" +
                   "Ctrl+F — пошук, F5 — оновити, F1 — довідка.",
            AutoSize = true,
            MaximumSize = new Size(480, 0)
        });
        var ok = new FlatButton("Закрити", ButtonVariant.Primary) { DialogResult = DialogResult.OK, Anchor = AnchorStyles.Right, Margin = new Padding(0, 18, 0, 0) };
        layout.Controls.Add(ok);
        Controls.Add(layout);
        AcceptButton = ok;
        CancelButton = ok;
    }
}
