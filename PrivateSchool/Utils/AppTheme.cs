using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace PrivateSchool.Utils
{
    /// <summary>Стиль кнопки в темі оформлення.</summary>
    public enum ButtonKind
    {
        Primary,
        Secondary,
        Danger,
        Ghost
    }

    /// <summary>
    /// Єдина тема оформлення програми: палітра кольорів, шрифти, іконки
    /// та методи стилізації стандартних елементів керування.
    /// </summary>
    public static class AppTheme
    {
        // ---------- Палітра ----------
        public static readonly Color Primary = Color.FromArgb(37, 99, 235);
        public static readonly Color PrimaryDark = Color.FromArgb(30, 64, 175);
        public static readonly Color PrimaryHover = Color.FromArgb(29, 78, 216);
        public static readonly Color PrimaryLight = Color.FromArgb(219, 234, 254);
        public static readonly Color PrimarySoft = Color.FromArgb(239, 246, 255);

        public static readonly Color Background = Color.FromArgb(243, 245, 249);
        public static readonly Color Surface = Color.White;
        public static readonly Color Border = Color.FromArgb(226, 232, 240);
        public static readonly Color BorderStrong = Color.FromArgb(203, 213, 225);

        public static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);
        public static readonly Color TextSecondary = Color.FromArgb(100, 116, 139);
        public static readonly Color TextMuted = Color.FromArgb(148, 163, 184);

        public static readonly Color Success = Color.FromArgb(22, 163, 74);
        public static readonly Color Danger = Color.FromArgb(220, 38, 38);
        public static readonly Color DangerLight = Color.FromArgb(254, 242, 242);
        public static readonly Color Warning = Color.FromArgb(217, 119, 6);
        public static readonly Color Violet = Color.FromArgb(124, 58, 237);
        public static readonly Color Teal = Color.FromArgb(13, 148, 136);

        public static readonly Color Sidebar = Color.FromArgb(15, 23, 42);
        public static readonly Color SidebarHover = Color.FromArgb(30, 41, 59);
        public static readonly Color SidebarSelected = Color.FromArgb(37, 52, 80);
        public static readonly Color SidebarText = Color.FromArgb(203, 213, 225);
        public static readonly Color SidebarMuted = Color.FromArgb(100, 116, 139);

        public static readonly Color GridHeader = Color.FromArgb(248, 250, 252);
        public static readonly Color GridAlternate = Color.FromArgb(250, 251, 253);
        public static readonly Color GridSelection = Color.FromArgb(219, 234, 254);

        // ---------- Шрифти ----------
        public const string FontName = "Segoe UI";

        public static readonly Font BaseFont = new Font(FontName, 9.75F);
        public static readonly Font SemiboldFont = new Font(FontName, 9.75F, FontStyle.Bold);
        public static readonly Font SmallFont = new Font(FontName, 8.5F);
        public static readonly Font TitleFont = new Font(FontName, 16F, FontStyle.Bold);
        public static readonly Font SubtitleFont = new Font(FontName, 9.75F);
        public static readonly Font CardTitleFont = new Font(FontName, 11F, FontStyle.Bold);
        public static readonly Font ValueFont = new Font(FontName, 19F, FontStyle.Bold);

        // ---------- Іконки (шрифт Segoe MDL2 Assets / Segoe Fluent Icons, Windows 10/11) ----------
        public const string IconHome = "";
        public const string IconStudents = "";
        public const string IconTeachers = "";
        public const string IconCourses = "";
        public const string IconGroups = "";
        public const string IconSchedule = "";
        public const string IconPayments = "";
        public const string IconGrades = "";
        public const string IconReports = "";
        public const string IconUsers = "";
        public const string IconLogout = "";
        public const string IconAbout = "";
        public const string IconSearch = "";
        public const string IconMoney = "";
        public const string IconWarning = "";
        public const string IconChart = "";

        private static readonly string IconFontName = DetectIconFont();

        /// <summary>Чи доступний у системі шрифт з іконками.</summary>
        public static bool HasIcons
        {
            get { return IconFontName != null; }
        }

        /// <summary>Шрифт іконок заданого розміру або null, якщо шрифт не встановлено.</summary>
        public static Font GetIconFont(float size)
        {
            return IconFontName == null ? null : new Font(IconFontName, size);
        }

        private static string DetectIconFont()
        {
            try
            {
                using (InstalledFontCollection fonts = new InstalledFontCollection())
                {
                    string[] candidates = { "Segoe Fluent Icons", "Segoe MDL2 Assets" };
                    return candidates.FirstOrDefault(c => fonts.Families.Any(f => f.Name == c));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ---------- Малювання ----------

        /// <summary>
        /// Коефіцієнт масштабу екрана (1.0 при 100 %, 1.5 при 150 %).
        /// Використовується, щоб власні елементи керування коректно малювалися на екранах з високою щільністю пікселів.
        /// </summary>
        public static float ScaleOf(Graphics g)
        {
            return g.DpiX / 96f;
        }

        /// <summary>Контур прямокутника із заокругленими кутами.</summary>
        public static GraphicsPath RoundedRect(RectangleF bounds, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float d = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
            if (d <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>Малює білу «картку» із заокругленими кутами та рамкою.</summary>
        public static void PaintCard(Graphics g, Rectangle bounds, Color parentBack, Color fill, int radius)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(parentBack);
            RectangleF rect = new RectangleF(bounds.X + 0.5f, bounds.Y + 0.5f, bounds.Width - 1.5f, bounds.Height - 1.5f);
            using (GraphicsPath path = RoundedRect(rect, radius))
            using (SolidBrush brush = new SolidBrush(fill))
            using (Pen pen = new Pen(Border))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Малює символ логотипу: іконку «академічна шапочка» (якщо є шрифт іконок) або літери «З+».
        /// </summary>
        public static void DrawLogoMark(Graphics g, Rectangle bounds, float size)
        {
            Font iconFont = GetIconFont(size);
            using (Font font = iconFont ?? new Font(FontName, size, FontStyle.Bold))
                TextRenderer.DrawText(g, iconFont != null ? IconTeachers : "З+", font, bounds, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        /// <summary>Змішує колір із білим (amount = 0 — вихідний колір, 1 — білий).</summary>
        public static Color Lighten(Color color, float amount)
        {
            return Color.FromArgb(
                (int)(color.R + (255 - color.R) * amount),
                (int)(color.G + (255 - color.G) * amount),
                (int)(color.B + (255 - color.B) * amount));
        }

        // ---------- Стилізація стандартних елементів ----------

        public static void StyleButton(Button button, ButtonKind kind)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.Cursor = Cursors.Hand;
            button.Font = SemiboldFont;
            button.UseVisualStyleBackColor = false;
            button.FlatAppearance.BorderSize = 1;

            switch (kind)
            {
                case ButtonKind.Primary:
                    button.BackColor = Primary;
                    button.ForeColor = Color.White;
                    button.FlatAppearance.BorderColor = Primary;
                    button.FlatAppearance.MouseOverBackColor = PrimaryHover;
                    button.FlatAppearance.MouseDownBackColor = PrimaryDark;
                    break;
                case ButtonKind.Secondary:
                    button.BackColor = Surface;
                    button.ForeColor = TextPrimary;
                    button.FlatAppearance.BorderColor = BorderStrong;
                    button.FlatAppearance.MouseOverBackColor = Background;
                    button.FlatAppearance.MouseDownBackColor = Border;
                    break;
                case ButtonKind.Danger:
                    button.BackColor = Surface;
                    button.ForeColor = Danger;
                    button.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202);
                    button.FlatAppearance.MouseOverBackColor = DangerLight;
                    button.FlatAppearance.MouseDownBackColor = Color.FromArgb(254, 226, 226);
                    break;
                default:
                    button.BackColor = Color.Transparent;
                    button.ForeColor = TextSecondary;
                    button.FlatAppearance.BorderSize = 0;
                    button.FlatAppearance.MouseOverBackColor = Background;
                    button.FlatAppearance.MouseDownBackColor = Border;
                    break;
            }
        }

        /// <summary>Сучасний вигляд таблиці: світлий заголовок, горизонтальні лінії, виділення акцентним кольором.</summary>
        public static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 40;
            grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeader;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextSecondary;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = GridHeader;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextSecondary;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font(FontName, 9F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            grid.DefaultCellStyle.Font = BaseFont;
            grid.DefaultCellStyle.ForeColor = TextPrimary;
            grid.DefaultCellStyle.BackColor = Surface;
            grid.DefaultCellStyle.SelectionBackColor = GridSelection;
            grid.DefaultCellStyle.SelectionForeColor = TextPrimary;
            grid.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
            grid.AlternatingRowsDefaultCellStyle.BackColor = GridAlternate;

            grid.RowTemplate.Height = 36;
            grid.GridColor = Border;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowHeadersVisible = false;
            grid.AllowUserToResizeRows = false;
            grid.ScrollBars = ScrollBars.Vertical; // колонки розтягуються на всю ширину
        }

        /// <summary>
        /// Рекурсивно оформлює поля введення та підписи на формі редагування.
        /// </summary>
        public static void ApplyToInputs(Control root)
        {
            foreach (Control control in root.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).BorderStyle = BorderStyle.FixedSingle;
                    control.Font = BaseFont;
                }
                else if (control is NumericUpDown)
                {
                    ((NumericUpDown)control).BorderStyle = BorderStyle.FixedSingle;
                    control.Font = BaseFont;
                }
                else if (control is ComboBox || control is DateTimePicker)
                {
                    control.Font = BaseFont;
                }
                else if (control is CheckBox)
                {
                    control.ForeColor = TextSecondary;
                }
                else if (control is Label && control.ForeColor == SystemColors.ControlText)
                {
                    control.ForeColor = TextSecondary;
                }

                if (control.HasChildren && !(control is UserControl))
                    ApplyToInputs(control);
            }
        }
    }
}
