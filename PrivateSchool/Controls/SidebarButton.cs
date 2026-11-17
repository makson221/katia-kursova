using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PrivateSchool.Utils;

namespace PrivateSchool.Controls
{
    /// <summary>
    /// Кнопка бічної панелі навігації з іконкою, станом наведення та виділення.
    /// Повністю малюється власним кодом (перевизначення OnPaint).
    /// </summary>
    public class SidebarButton : Control
    {
        private bool _selected;
        private bool _hover;
        private string _icon = string.Empty;

        public SidebarButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, false);
            BackColor = AppTheme.Sidebar;
            ForeColor = AppTheme.SidebarText;
            Font = new Font(AppTheme.FontName, 10F);
            Cursor = Cursors.Hand;
            Size = new Size(216, 42);
        }

        /// <summary>Символ іконки зі шрифту Segoe MDL2 Assets.</summary>
        [Category("Оформлення")]
        [DefaultValue("")]
        public string IconGlyph
        {
            get { return _icon; }
            set
            {
                _icon = value ?? string.Empty;
                Invalidate();
            }
        }

        /// <summary>Чи є кнопка поточним (активним) розділом.</summary>
        [Category("Поведінка")]
        [DefaultValue(false)]
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent == null ? BackColor : Parent.BackColor);
            float k = AppTheme.ScaleOf(g);

            Color fill = _selected ? AppTheme.SidebarSelected : (_hover ? AppTheme.SidebarHover : Color.Empty);
            if (fill != Color.Empty)
            {
                using (GraphicsPath path = AppTheme.RoundedRect(new RectangleF(8 * k, 2 * k, Width - 16 * k, Height - 4 * k), 8 * k))
                using (SolidBrush brush = new SolidBrush(fill))
                    g.FillPath(brush, path);
            }

            if (_selected)
            {
                using (GraphicsPath bar = AppTheme.RoundedRect(new RectangleF(8 * k, 10 * k, 4 * k, Height - 20 * k), 2 * k))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(96, 165, 250)))
                    g.FillPath(brush, bar);
            }

            Color textColor = _selected || _hover ? Color.White : ForeColor;
            int textLeft = (int)(26 * k);

            Font iconFont = AppTheme.GetIconFont(12F);
            if (iconFont != null && _icon.Length > 0)
            {
                using (iconFont)
                    TextRenderer.DrawText(g, _icon, iconFont, new Rectangle((int)(24 * k), 0, (int)(24 * k), Height), textColor,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                textLeft = (int)(58 * k);
            }

            using (Font font = new Font(Font, _selected ? FontStyle.Bold : FontStyle.Regular))
                TextRenderer.DrawText(g, Text, font, new Rectangle(textLeft, 0, Width - textLeft - (int)(8 * k), Height), textColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }
}
