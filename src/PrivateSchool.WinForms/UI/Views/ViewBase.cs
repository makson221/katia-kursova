using System.ComponentModel;
using PrivateSchool.Core.Services;

namespace PrivateSchool.WinForms.UI.Views;

/// <summary>
/// Базовий клас розділу головного вікна: заголовок, підзаголовок і область вмісту.
/// </summary>
[DesignerCategory("Code")]
public abstract class ViewBase : UserControl
{
    private readonly Label _subtitle;

    protected ViewBase(SchoolSystem app)
    {
        App = app;
        Dock = DockStyle.Fill;
        BackColor = Theme.Background;
        Font = Theme.BaseFont;
        Padding = new Padding(28, 20, 28, 20);
        DoubleBuffered = true;

        Content = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Background };

        var header = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Theme.Background };
        var title = new Label
        {
            Text = Title,
            Font = Theme.TitleFont,
            ForeColor = Theme.Text,
            AutoSize = true,
            Location = new Point(0, 0)
        };
        _subtitle = new Label
        {
            Text = Subtitle,
            Font = Theme.SubtitleFont,
            ForeColor = Theme.TextMuted,
            AutoSize = true,
            Location = new Point(2, 38)
        };
        header.Controls.Add(title);
        header.Controls.Add(_subtitle);

        Controls.Add(Content);
        Controls.Add(header);
    }

    protected SchoolSystem App { get; }

    /// <summary>Контейнер для вмісту розділу.</summary>
    protected Panel Content { get; }

    public abstract string Title { get; }
    public virtual string Subtitle => string.Empty;

    protected void SetSubtitle(string text) => _subtitle.Text = text;

    /// <summary>Оновлює відображувані дані з бази.</summary>
    public abstract void RefreshData();

    /// <summary>Викликається щоразу, коли розділ стає активним.</summary>
    public virtual void OnActivated() => RefreshData();

    /// <summary>Безпечне виконання дії з показом повідомлення про помилку.</summary>
    protected void Try(Action action)
    {
        try { action(); }
        catch (Exception ex) { Messages.Exception(ex, FindForm()); }
    }
}
