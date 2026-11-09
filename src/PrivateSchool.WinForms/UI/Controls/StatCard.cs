using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PrivateSchool.WinForms.UI.Controls;

/// <summary>
/// Картка показника для головної панелі (назва, значення, підпис, піктограма).
/// </summary>
[DesignerCategory("Code")]
public class StatCard : Control
{
    private string _value = "—";
    private string _caption = string.Empty;

    public StatCard(string title, string glyph, Color accent)
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        Title = title;
        Glyph = glyph;
        Accent = accent;
        Size = new Size(220, 110);
        Margin = new Padding(0, 0, 16, 16);
    }

    public string Title { get; }
    public string Glyph { get; }
    public Color Accent { get; }

    public string Value
    {
        get => _value;
        set { _value = value; Invalidate(); }
    }

    public string Caption
    {
        get => _caption;
        set { _caption = value; Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Background);

        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using (var path = Theme.RoundedRect(rect, 10))
        using (var brush = new SolidBrush(Theme.Surface))
        using (var pen = new Pen(Theme.Border))
        {
            g.FillPath(brush, path);
            g.DrawPath(pen, path);
        }

        // Круглий фон під піктограму
        var iconRect = new Rectangle(Width - 50, 14, 34, 34);
        using (var iconBack = new SolidBrush(Theme.Blend(Accent, Color.White, 0.85)))
            g.FillEllipse(iconBack, iconRect);
        if (Theme.IconFontName != null)
        {
            using var iconFont = Theme.IconFont(12f)!;
            TextRenderer.DrawText(g, Glyph, iconFont, iconRect, Accent,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        var textWidth = Width - 72;
        TextRenderer.DrawText(g, Title, Theme.SmallFont, new Rectangle(18, 16, textWidth, 20), Theme.TextMuted,
            TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        TextRenderer.DrawText(g, Value, Theme.CardValueFont, new Rectangle(14, 38, Width - 24, 40), Theme.Text,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        TextRenderer.DrawText(g, Caption, Theme.SmallFont, new Rectangle(18, 78, Width - 30, 20), Accent,
            TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }
}
