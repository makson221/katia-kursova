using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PrivateSchool.WinForms.UI.Controls;

/// <summary>
/// Проста стовпчикова діаграма, намальована засобами GDI+.
/// </summary>
[DesignerCategory("Code")]
public class BarChart : Control
{
    private IReadOnlyList<(string Label, double Value)> _items = Array.Empty<(string, double)>();

    public BarChart()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        BackColor = Theme.Surface;
        Font = Theme.SmallFont;
    }

    public string Title { get; set; } = string.Empty;
    public Color BarColor { get; set; } = Theme.Primary;
    public Func<double, string> ValueFormatter { get; set; } = v => v.ToString("N0");

    /// <summary>Значення, для якого малюється горизонтальна лінія (напр. прохідний бал).</summary>
    public double? Threshold { get; set; }

    /// <summary>Виділяти останній стовпчик (зручно для часових рядів — поточний період).</summary>
    public bool HighlightLast { get; set; } = true;

    public IReadOnlyList<(string Label, double Value)> Items
    {
        get => _items;
        set { _items = value; Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(BackColor);

        TextRenderer.DrawText(g, Title, Theme.SectionFont, new Point(16, 12), Theme.Text);

        var plot = new Rectangle(70, 52, Width - 94, Height - 52 - 40);
        if (plot.Width < 40 || plot.Height < 40) return;

        if (_items.Count == 0)
        {
            TextRenderer.DrawText(g, "Немає даних для відображення", Font, plot, Theme.TextMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        var max = Math.Max(_items.Max(i => i.Value), Threshold ?? 0);
        if (max <= 0) max = 1;
        max = NiceCeiling(max);

        // Сітка та шкала
        using var gridPen = new Pen(Theme.Border) { DashStyle = DashStyle.Dash };
        for (var i = 0; i <= 4; i++)
        {
            var y = plot.Bottom - plot.Height * i / 4;
            g.DrawLine(gridPen, plot.Left, y, plot.Right, y);
            var label = CompactNumber(max * i / 4);
            TextRenderer.DrawText(g, label, Font, new Rectangle(0, y - 9, plot.Left - 6, 18), Theme.TextMuted,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        }

        var slot = plot.Width / (float)_items.Count;
        var barWidth = Math.Min(56f, slot * 0.6f);
        for (var i = 0; i < _items.Count; i++)
        {
            var (label, value) = _items[i];
            var h = (float)(plot.Height * value / max);
            var x = plot.Left + slot * i + (slot - barWidth) / 2;
            var bar = new RectangleF(x, plot.Bottom - h, barWidth, h);

            if (h > 0)
            {
                using var path = Theme.RoundedRect(Rectangle.Round(bar), Math.Min(6, (int)(h / 2)));
                var isLast = HighlightLast && i == _items.Count - 1;
                var color = isLast || !HighlightLast ? BarColor : Theme.Blend(BarColor, Color.White, 0.35);
                if (Threshold is double limit && value < limit) color = Theme.Danger;
                using var brush = new SolidBrush(color);
                g.FillPath(brush, path);
            }

            TextRenderer.DrawText(g, ValueFormatter(value), Font,
                new Rectangle((int)(plot.Left + slot * i), (int)bar.Top - 20, (int)slot, 18), Theme.Text,
                TextFormatFlags.HorizontalCenter);
            TextRenderer.DrawText(g, label, Font,
                new Rectangle((int)(plot.Left + slot * i), plot.Bottom + 8, (int)slot, 20), Theme.TextMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis);
        }

        if (Threshold is double t)
        {
            var y = plot.Bottom - (float)(plot.Height * t / max);
            using var pen = new Pen(Theme.Danger, 1.5f) { DashStyle = DashStyle.Dash };
            g.DrawLine(pen, plot.Left, y, plot.Right, y);
        }
    }

    private static double NiceCeiling(double value)
    {
        var magnitude = Math.Pow(10, Math.Floor(Math.Log10(value)));
        var normalized = value / magnitude;
        var nice = normalized <= 1 ? 1 : normalized <= 2 ? 2 : normalized <= 2.5 ? 2.5 : normalized <= 5 ? 5 : 10;
        return nice * magnitude;
    }

    private static string CompactNumber(double v) =>
        v >= 1_000_000 ? $"{v / 1_000_000:0.#} млн" : v >= 1000 ? $"{v / 1000:0.#} тис" : v.ToString("0.#");
}
