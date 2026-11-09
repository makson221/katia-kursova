using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PrivateSchool.WinForms.UI.Controls;

public enum ButtonVariant { Primary, Secondary, Danger, Success, Ghost }

/// <summary>
/// Плоска кнопка із заокругленими кутами, піктограмою та станами наведення/натискання.
/// </summary>
[DesignerCategory("Code")]
public class FlatButton : Button
{
    private bool _hover;
    private bool _pressed;
    private ButtonVariant _variant = ButtonVariant.Secondary;
    private string _glyph = string.Empty;

    public FlatButton()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Font = Theme.BoldFont;
        Height = 36;
        Cursor = Cursors.Hand;
        Margin = new Padding(0, 0, 8, 0);
        Padding = new Padding(14, 0, 14, 0);
        BackColor = Color.Transparent;
    }

    public FlatButton(string text, ButtonVariant variant = ButtonVariant.Secondary, string glyph = "") : this()
    {
        Text = text;
        _variant = variant;
        _glyph = glyph;
        AutoSize = true;
    }

    [DefaultValue(ButtonVariant.Secondary)]
    public ButtonVariant Variant
    {
        get => _variant;
        set { _variant = value; Invalidate(); }
    }

    [DefaultValue("")]
    public string Glyph
    {
        get => _glyph;
        set { _glyph = value; Invalidate(); }
    }

    public override Size GetPreferredSize(Size proposedSize)
    {
        var textSize = TextRenderer.MeasureText(Text, Font);
        var iconWidth = HasIcon ? 24 : 0;
        return new Size(textSize.Width + iconWidth + Padding.Horizontal, Math.Max(36, Height));
    }

    private bool HasIcon => _glyph.Length > 0 && Theme.IconFontName != null;

    private (Color Back, Color Fore, Color Border) GetColors()
    {
        var (back, fore, border) = _variant switch
        {
            ButtonVariant.Primary => (Theme.Primary, Color.White, Theme.Primary),
            ButtonVariant.Danger => (Theme.Surface, Theme.Danger, Theme.Blend(Theme.Danger, Color.White, 0.6)),
            ButtonVariant.Success => (Theme.Success, Color.White, Theme.Success),
            ButtonVariant.Ghost => (Color.Transparent, Theme.TextMuted, Color.Transparent),
            _ => (Theme.Surface, Theme.Text, Theme.Border)
        };

        if (!Enabled) return (Theme.Background, Theme.Blend(Theme.TextMuted, Color.White, 0.4), Theme.Border);

        if (_pressed)
            back = _variant is ButtonVariant.Primary or ButtonVariant.Success
                ? Theme.Blend(back, Color.Black, 0.2)
                : Theme.Blend(Theme.Background, Color.Black, 0.05);
        else if (_hover)
            back = _variant switch
            {
                ButtonVariant.Primary => Theme.PrimaryDark,
                ButtonVariant.Success => Theme.Blend(back, Color.Black, 0.1),
                ButtonVariant.Danger => Theme.DangerLight,
                _ => Theme.Background
            };

        return (back, fore, border);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Theme.Background);

        var (back, fore, border) = GetColors();
        var rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using (var path = Theme.RoundedRect(rect, 6))
        {
            if (back != Color.Transparent)
            {
                using var brush = new SolidBrush(back);
                g.FillPath(brush, path);
            }
            if (border != Color.Transparent)
            {
                using var pen = new Pen(Focused && ShowFocusCues ? Theme.Primary : border);
                g.DrawPath(pen, path);
            }
        }

        var textSize = TextRenderer.MeasureText(Text, Font);
        var iconWidth = HasIcon ? 24 : 0;
        var x = (Width - textSize.Width - iconWidth) / 2;

        if (HasIcon)
        {
            using var iconFont = Theme.IconFont(11f)!;
            TextRenderer.DrawText(g, _glyph, iconFont, new Rectangle(x, 0, 20, Height), fore,
                TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        TextRenderer.DrawText(g, Text, Font, new Rectangle(x + iconWidth, 0, textSize.Width + 2, Height), fore,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding);
    }

    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _pressed = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _pressed = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _pressed = false; Invalidate(); base.OnMouseUp(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
    protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Invalidate(); }
}
