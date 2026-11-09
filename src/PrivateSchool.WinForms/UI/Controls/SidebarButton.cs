using System.ComponentModel;

namespace PrivateSchool.WinForms.UI.Controls;

/// <summary>
/// Кнопка навігації бічної панелі з піктограмою та індикатором активного розділу.
/// </summary>
[DesignerCategory("Code")]
public class SidebarButton : Button
{
    private bool _hover;
    private bool _active;

    public SidebarButton(string text, string glyph)
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        Text = text;
        Glyph = glyph;
        Dock = DockStyle.Top;
        Height = 44;
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Font = new Font(Theme.FontName, 10.5f);
        Cursor = Cursors.Hand;
        TabStop = false;
    }

    public string Glyph { get; }

    /// <summary>Пов'язаний розділ (довільний ключ).</summary>
    public object? Key { get; init; }

    public bool Active
    {
        get => _active;
        set { _active = value; Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        var back = _active ? Theme.SidebarHover : _hover ? Theme.Blend(Theme.Sidebar, Theme.SidebarHover, 0.6) : Theme.Sidebar;
        g.Clear(back);

        if (_active)
        {
            using var accent = new SolidBrush(Theme.Primary);
            g.FillRectangle(accent, 0, 6, 4, Height - 12);
        }

        var fore = _active ? Color.White : Theme.SidebarText;
        var textX = 22;
        if (Theme.IconFontName != null)
        {
            using var iconFont = Theme.IconFont(13f)!;
            TextRenderer.DrawText(g, Glyph, iconFont, new Rectangle(20, 0, 26, Height),
                _active ? Color.FromArgb(147, 197, 253) : fore,
                TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            textX = 56;
        }

        using var font = _active ? new Font(Font, FontStyle.Bold) : null;
        TextRenderer.DrawText(g, Text, font ?? Font, new Rectangle(textX, 0, Width - textX - 8, Height), fore,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
}
