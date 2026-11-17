using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PrivateSchool.Utils;

namespace PrivateSchool.Controls
{
    /// <summary>
    /// Картка ключового показника для головної сторінки: назва, значення, підпис та іконка.
    /// </summary>
    public class StatCard : Control
    {
        private string _title = "Показник";
        private string _value = "0";
        private string _caption = string.Empty;
        private string _icon = string.Empty;
        private Color _accent = AppTheme.Primary;

        public StatCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Selectable, false);
            BackColor = AppTheme.Surface;
            Size = new Size(260, 104);
        }

        [Category("Оформлення")]
        public string Title
        {
            get { return _title; }
            set { _title = value; Invalidate(); }
        }

        [Category("Оформлення")]
        public string Value
        {
            get { return _value; }
            set { _value = value; Invalidate(); }
        }

        [Category("Оформлення")]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; Invalidate(); }
        }

        [Category("Оформлення")]
        [DefaultValue("")]
        public string IconGlyph
        {
            get { return _icon; }
            set { _icon = value ?? string.Empty; Invalidate(); }
        }

        [Category("Оформлення")]
        public Color AccentColor
        {
            get { return _accent; }
            set { _accent = value; Invalidate(); }
        }

        /// <summary>Оновлює всі тексти картки одним викликом.</summary>
        public void SetData(string value, string caption, Color accent)
        {
            _value = value;
            _caption = caption;
            _accent = accent;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            float k = AppTheme.ScaleOf(g);
            Func<int, int> px = v => (int)Math.Round(v * k);

            Color parentBack = Parent == null ? AppTheme.Background : Parent.BackColor;
            AppTheme.PaintCard(g, ClientRectangle, parentBack, BackColor, px(10));

            // Кольорова «плашка» з іконкою праворуч угорі
            Rectangle badge = new Rectangle(Width - px(54), px(16), px(38), px(38));
            using (GraphicsPath path = AppTheme.RoundedRect(badge, px(9)))
            using (SolidBrush brush = new SolidBrush(AppTheme.Lighten(_accent, 0.86f)))
                g.FillPath(brush, path);

            Font iconFont = AppTheme.GetIconFont(14F);
            if (iconFont != null && _icon.Length > 0)
            {
                using (iconFont)
                    TextRenderer.DrawText(g, _icon, iconFont, badge, _accent,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            else
            {
                using (SolidBrush dot = new SolidBrush(_accent))
                    g.FillEllipse(dot, badge.X + px(13), badge.Y + px(13), px(12), px(12));
            }

            TextRenderer.DrawText(g, _title, AppTheme.SemiboldFont, new Rectangle(px(18), px(16), Width - px(82), px(20)),
                AppTheme.TextSecondary, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            TextRenderer.DrawText(g, _value, AppTheme.ValueFont, new Rectangle(px(14), px(38), Width - px(28), px(38)),
                AppTheme.TextPrimary, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            if (!string.IsNullOrEmpty(_caption))
                TextRenderer.DrawText(g, _caption, AppTheme.SmallFont, new Rectangle(px(18), Height - px(28), Width - px(36), px(18)),
                    _accent, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
    }
}
