using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using PrivateSchool.Utils;

namespace PrivateSchool.Controls
{
    /// <summary>Один стовпчик діаграми.</summary>
    public class ChartItem
    {
        public ChartItem(string label, decimal value)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; private set; }
        public decimal Value { get; private set; }
    }

    /// <summary>
    /// Проста стовпчикова діаграма, що малюється засобами GDI+ (без сторонніх бібліотек).
    /// Останній стовпчик (поточний період) виділяється насиченим кольором.
    /// </summary>
    public class BarChart : Control
    {
        private readonly List<ChartItem> _items = new List<ChartItem>();
        private Color _barColor = AppTheme.Primary;

        public BarChart()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Selectable, false);
            BackColor = AppTheme.Surface;
            Size = new Size(400, 240);
        }

        [Category("Оформлення")]
        public Color BarColor
        {
            get { return _barColor; }
            set { _barColor = value; Invalidate(); }
        }

        public void SetData(IEnumerable<ChartItem> items)
        {
            _items.Clear();
            _items.AddRange(items);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);

            if (_items.Count == 0)
            {
                TextRenderer.DrawText(g, "Немає даних", AppTheme.BaseFont, ClientRectangle, AppTheme.TextMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            float k = AppTheme.ScaleOf(g);
            int left = (int)(52 * k), top = (int)(22 * k), bottom = (int)(30 * k), right = (int)(8 * k);
            Rectangle plot = new Rectangle(left, top, Math.Max(10, Width - left - right), Math.Max(10, Height - top - bottom));
            decimal max = NiceMaximum(_items.Max(i => i.Value));

            // Горизонтальні лінії сітки та підписи осі Y
            using (Pen gridPen = new Pen(AppTheme.Border) { DashStyle = DashStyle.Dash })
            {
                for (int i = 0; i <= 4; i++)
                {
                    int y = plot.Bottom - (int)(plot.Height * i / 4.0);
                    g.DrawLine(gridPen, plot.Left, y, plot.Right, y);
                    TextRenderer.DrawText(g, Compact(max * i / 4), AppTheme.SmallFont,
                        new Rectangle(0, y - (int)(9 * k), left - (int)(8 * k), (int)(18 * k)), AppTheme.TextMuted,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                }
            }

            float slot = plot.Width / (float)_items.Count;
            float barWidth = Math.Min(48f * k, slot * 0.56f);
            for (int i = 0; i < _items.Count; i++)
            {
                ChartItem item = _items[i];
                bool current = i == _items.Count - 1;
                float h = max == 0 ? 0 : (float)(item.Value / max) * plot.Height;
                float x = plot.Left + slot * i + (slot - barWidth) / 2;
                RectangleF bar = new RectangleF(x, plot.Bottom - h, barWidth, h);

                if (h > 0.5f)
                {
                    using (GraphicsPath path = TopRounded(bar, 6 * k))
                    using (SolidBrush brush = new SolidBrush(current ? _barColor : AppTheme.Lighten(_barColor, 0.55f)))
                        g.FillPath(brush, path);
                }

                Rectangle valueRect = new Rectangle((int)(plot.Left + slot * i), (int)(bar.Top - 20 * k), (int)slot, (int)(18 * k));
                TextRenderer.DrawText(g, Compact(item.Value), AppTheme.SmallFont, valueRect,
                    current ? AppTheme.TextPrimary : AppTheme.TextSecondary, TextFormatFlags.HorizontalCenter);

                Rectangle labelRect = new Rectangle((int)(plot.Left + slot * i), plot.Bottom + (int)(6 * k), (int)slot, (int)(20 * k));
                TextRenderer.DrawText(g, item.Label, AppTheme.SmallFont, labelRect,
                    current ? AppTheme.TextPrimary : AppTheme.TextSecondary,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        /// <summary>«Кругле» значення верхньої межі осі, щоб підписи були зручними для читання.</summary>
        private static decimal NiceMaximum(decimal value)
        {
            if (value <= 0)
                return 1;
            double magnitude = Math.Pow(10, Math.Floor(Math.Log10((double)value)));
            double[] steps = { 1, 1.5, 2, 2.5, 3, 4, 5, 6, 8, 10 };
            double nice = steps.Select(s => s * magnitude).First(v => v >= (double)value);
            return (decimal)nice;
        }

        /// <summary>Скорочений запис числа: 38 200 → «38,2 тис».</summary>
        private static string Compact(decimal value)
        {
            if (value >= 1000000)
                return (value / 1000000m).ToString("0.#") + " млн";
            if (value >= 1000)
                return (value / 1000m).ToString("0.#") + " тис";
            return value.ToString("0");
        }

        private static GraphicsPath TopRounded(RectangleF r, float radius)
        {
            float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height * 2));
            GraphicsPath path = new GraphicsPath();
            if (d < 1)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddLine(r.Right, r.Y + d / 2, r.Right, r.Bottom);
            path.AddLine(r.Right, r.Bottom, r.X, r.Bottom);
            path.CloseFigure();
            return path;
        }
    }
}
