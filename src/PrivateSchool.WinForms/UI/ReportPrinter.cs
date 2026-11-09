using System.Drawing.Printing;
using PrivateSchool.Core.Reports;

namespace PrivateSchool.WinForms.UI;

/// <summary>
/// Друк звіту з розбиттям на сторінки (через PrintDocument / PrintPreviewDialog).
/// </summary>
public sealed class ReportPrinter : IDisposable
{
    private readonly ReportTable _table;
    private readonly string _institution;
    private readonly PrintDocument _document = new();
    private readonly Font _titleFont = new(Theme.FontName, 14f, FontStyle.Bold);
    private readonly Font _headerFont = new(Theme.FontName, 8.5f, FontStyle.Bold);
    private readonly Font _cellFont = new(Theme.FontName, 8.5f);
    private int _rowIndex;
    private int _page;

    public ReportPrinter(ReportTable table, string institution)
    {
        _table = table;
        _institution = institution;
        _document.DocumentName = table.Title;
        _document.DefaultPageSettings.Landscape = table.Columns.Count > 6;
        _document.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
        _document.BeginPrint += (_, _) => { _rowIndex = 0; _page = 0; };
        _document.PrintPage += OnPrintPage;
    }

    public void ShowPreview(IWin32Window? owner)
    {
        using var preview = new PrintPreviewDialog
        {
            Document = _document,
            Width = 1000,
            Height = 750,
            UseAntiAlias = true,
            Text = "Попередній перегляд — " + _table.Title
        };
        preview.ShowDialog(owner);
    }

    private void OnPrintPage(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        var area = e.MarginBounds;
        var y = (float)area.Top;
        _page++;

        if (_page == 1)
        {
            g.DrawString(_institution, _cellFont, Brushes.Gray, area.Left, y);
            y += 18;
            g.DrawString(_table.Title, _titleFont, Brushes.Black, area.Left, y);
            y += 28;
            if (_table.Subtitle.Length > 0)
            {
                g.DrawString(_table.Subtitle, _cellFont, Brushes.DimGray, area.Left, y);
                y += 20;
            }
            y += 6;
        }

        // Ширини колонок пропорційні заданим у звіті
        var totalWeight = _table.Columns.Sum(c => c.Width);
        var widths = _table.Columns.Select(c => area.Width * c.Width / (float)totalWeight).ToArray();
        var rowHeight = _cellFont.GetHeight(g) + 8;

        // Заголовок таблиці
        var x = (float)area.Left;
        using (var headerBrush = new SolidBrush(Color.FromArgb(226, 232, 240)))
            g.FillRectangle(headerBrush, area.Left, y, area.Width, rowHeight + 4);
        for (var i = 0; i < _table.Columns.Count; i++)
        {
            DrawCell(g, _table.Columns[i].Header, _headerFont, new RectangleF(x, y, widths[i], rowHeight + 4), _table.Columns[i].Align);
            x += widths[i];
        }
        y += rowHeight + 4;

        using var linePen = new Pen(Color.FromArgb(203, 213, 225));
        while (_rowIndex < _table.Rows.Count)
        {
            if (y + rowHeight > area.Bottom - 20)
            {
                DrawFooter(g, area);
                e.HasMorePages = true;
                return;
            }

            var row = _table.Rows[_rowIndex++];
            x = area.Left;
            for (var i = 0; i < row.Length; i++)
            {
                DrawCell(g, row[i], _cellFont, new RectangleF(x, y, widths[i], rowHeight), _table.Columns[i].Align);
                x += widths[i];
            }
            y += rowHeight;
            g.DrawLine(linePen, area.Left, y, area.Right, y);
        }

        y += 12;
        foreach (var line in _table.Summary)
        {
            if (y + rowHeight > area.Bottom - 20) break;
            g.DrawString(line, _headerFont, Brushes.Black, area.Left, y);
            y += rowHeight;
        }

        DrawFooter(g, area);
        e.HasMorePages = false;
    }

    private void DrawCell(Graphics g, string text, Font font, RectangleF rect, ColumnAlign align)
    {
        using var format = new StringFormat
        {
            Alignment = align switch
            {
                ColumnAlign.Right => StringAlignment.Far,
                ColumnAlign.Center => StringAlignment.Center,
                _ => StringAlignment.Near
            },
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };
        var inner = RectangleF.Inflate(rect, -3, 0);
        g.DrawString(text, font, Brushes.Black, inner, format);
    }

    private void DrawFooter(Graphics g, Rectangle area)
    {
        var text = $"Сторінка {_page}   ·   Сформовано {_table.CreatedAt:dd.MM.yyyy HH:mm}";
        g.DrawString(text, _cellFont, Brushes.Gray, area.Left, area.Bottom - 12);
    }

    public void Dispose()
    {
        _document.Dispose();
        _titleFont.Dispose();
        _headerFont.Dispose();
        _cellFont.Dispose();
    }
}
