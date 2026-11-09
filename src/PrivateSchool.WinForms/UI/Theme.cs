using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace PrivateSchool.WinForms.UI;

/// <summary>
/// Єдина колірна схема, шрифти та допоміжні методи оформлення інтерфейсу.
/// </summary>
public static class Theme
{
    public static readonly Color Primary = ColorTranslator.FromHtml("#2563EB");
    public static readonly Color PrimaryDark = ColorTranslator.FromHtml("#1D4ED8");
    public static readonly Color PrimaryLight = ColorTranslator.FromHtml("#DBEAFE");
    public static readonly Color Sidebar = ColorTranslator.FromHtml("#0F172A");
    public static readonly Color SidebarHover = ColorTranslator.FromHtml("#1E293B");
    public static readonly Color SidebarText = ColorTranslator.FromHtml("#CBD5E1");
    public static readonly Color Background = ColorTranslator.FromHtml("#F1F5F9");
    public static readonly Color Surface = Color.White;
    public static readonly Color SurfaceAlt = ColorTranslator.FromHtml("#F8FAFC");
    public static readonly Color Border = ColorTranslator.FromHtml("#E2E8F0");
    public static readonly Color Text = ColorTranslator.FromHtml("#0F172A");
    public static readonly Color TextMuted = ColorTranslator.FromHtml("#64748B");
    public static readonly Color Success = ColorTranslator.FromHtml("#16A34A");
    public static readonly Color SuccessLight = ColorTranslator.FromHtml("#DCFCE7");
    public static readonly Color Danger = ColorTranslator.FromHtml("#DC2626");
    public static readonly Color DangerLight = ColorTranslator.FromHtml("#FEE2E2");
    public static readonly Color Warning = ColorTranslator.FromHtml("#D97706");
    public static readonly Color WarningLight = ColorTranslator.FromHtml("#FEF3C7");
    public static readonly Color Violet = ColorTranslator.FromHtml("#7C3AED");
    public static readonly Color Teal = ColorTranslator.FromHtml("#0D9488");

    public const string FontName = "Segoe UI";

    public static readonly Font BaseFont = new(FontName, 10f);
    public static readonly Font SmallFont = new(FontName, 9f);
    public static readonly Font BoldFont = new(FontName, 10f, FontStyle.Bold);
    public static readonly Font LabelFont = new(FontName, 9f, FontStyle.Bold);
    public static readonly Font TitleFont = new(FontName, 18f, FontStyle.Bold);
    public static readonly Font SubtitleFont = new(FontName, 10.5f);
    public static readonly Font SectionFont = new(FontName, 12f, FontStyle.Bold);
    public static readonly Font CardValueFont = new(FontName, 18f, FontStyle.Bold);

    /// <summary>Шрифт піктограм Windows 10/11 (Segoe Fluent Icons або Segoe MDL2 Assets), якщо доступний.</summary>
    public static readonly string? IconFontName = DetectIconFont();

    public static Font? IconFont(float size) => IconFontName == null ? null : new Font(IconFontName, size);

    /// <summary>Коди піктограм зі шрифту Segoe MDL2 Assets.</summary>
    public static class Glyphs
    {
        public const string Home = "";
        public const string People = "";
        public const string Teacher = "";
        public const string Group = "";
        public const string Library = "";
        public const string Star = "";
        public const string Payment = "";
        public const string Calendar = "";
        public const string Report = "";
        public const string Users = "";
        public const string Lock = "";
        public const string Power = "";
        public const string Add = "";
        public const string Edit = "";
        public const string Delete = "";
        public const string Refresh = "";
        public const string Export = "";
        public const string Print = "";
        public const string Search = "";
        public const string Card = "";
        public const string Money = "";
        public const string Warning = "";
        public const string Chart = "";
        public const string Info = "";
        public const string Key = "";
        public const string Play = "";
    }

    private static string? DetectIconFont()
    {
        using var fonts = new InstalledFontCollection();
        var names = fonts.Families.Select(f => f.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (names.Contains("Segoe Fluent Icons")) return "Segoe Fluent Icons";
        if (names.Contains("Segoe MDL2 Assets")) return "Segoe MDL2 Assets";
        return null;
    }

    public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        if (radius <= 0)
        {
            path.AddRectangle(bounds);
            return path;
        }

        var d = radius * 2;
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    public static Color Blend(Color color, Color with, double amount) => Color.FromArgb(
        (int)(color.R + (with.R - color.R) * amount),
        (int)(color.G + (with.G - color.G) * amount),
        (int)(color.B + (with.B - color.B) * amount));

    /// <summary>Застосовує стандартне оформлення до полів введення всередині контейнера.</summary>
    public static void StyleInputs(Control root)
    {
        foreach (Control c in root.Controls)
        {
            switch (c)
            {
                case TextBox tb:
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    tb.Font = BaseFont;
                    break;
                case ComboBox cb:
                    cb.FlatStyle = FlatStyle.System;
                    cb.Font = BaseFont;
                    break;
                case NumericUpDown nud:
                    nud.BorderStyle = BorderStyle.FixedSingle;
                    nud.Font = BaseFont;
                    break;
                case DateTimePicker dtp:
                    dtp.Font = BaseFont;
                    dtp.Format = DateTimePickerFormat.Custom;
                    dtp.CustomFormat = "dd.MM.yyyy";
                    break;
            }
            if (c.HasChildren) StyleInputs(c);
        }
    }
}
