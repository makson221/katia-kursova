using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PrivateSchool.Utils;

namespace PrivateSchool.Controls
{
    /// <summary>
    /// Панель-«картка»: білий фон із заокругленими кутами та тонкою рамкою.
    /// Використовується як контейнер для блоків на головній сторінці та звітах.
    /// </summary>
    public class CardPanel : Panel
    {
        private int _radius = 10;

        public CardPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = AppTheme.Surface;
            Padding = new Padding(16);
        }

        [Category("Оформлення")]
        [DefaultValue(10)]
        public int CornerRadius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                Invalidate();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color parentBack = Parent == null ? AppTheme.Background : Parent.BackColor;
            AppTheme.PaintCard(e.Graphics, ClientRectangle, parentBack, BackColor, _radius);
        }
    }
}
