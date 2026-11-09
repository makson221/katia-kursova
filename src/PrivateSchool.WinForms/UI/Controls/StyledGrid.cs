using System.ComponentModel;
using PrivateSchool.Core.Reports;

namespace PrivateSchool.WinForms.UI.Controls;

/// <summary>
/// Таблиця даних з єдиним сучасним оформленням.
/// </summary>
[DesignerCategory("Code")]
public class StyledGrid : DataGridView
{
    public StyledGrid()
    {
        DoubleBuffered = true;
        ReadOnly = true;
        AllowUserToAddRows = false;
        AllowUserToDeleteRows = false;
        AllowUserToResizeRows = false;
        AllowUserToOrderColumns = true;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        MultiSelect = false;
        RowHeadersVisible = false;
        BorderStyle = BorderStyle.None;
        CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        BackgroundColor = Theme.Surface;
        GridColor = Theme.Border;
        EnableHeadersVisualStyles = false;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        Font = Theme.BaseFont;

        ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        ColumnHeadersHeight = 42;
        ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Theme.SurfaceAlt,
            ForeColor = Theme.TextMuted,
            Font = Theme.LabelFont,
            SelectionBackColor = Theme.SurfaceAlt,
            SelectionForeColor = Theme.TextMuted,
            Padding = new Padding(8, 0, 8, 0),
            WrapMode = DataGridViewTriState.False
        };

        RowTemplate.Height = 36;
        DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Theme.Surface,
            ForeColor = Theme.Text,
            SelectionBackColor = Theme.PrimaryLight,
            SelectionForeColor = Theme.Text,
            Padding = new Padding(8, 0, 8, 0),
            Font = Theme.BaseFont
        };
        AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Theme.SurfaceAlt };
    }

    /// <summary>Додає текстову колонку з вирівнюванням, форматом і відносною шириною.</summary>
    public DataGridViewTextBoxColumn AddColumn(string header, int fillWeight = 100,
        ColumnAlign align = ColumnAlign.Left, string? format = null, int minWidth = 60)
    {
        var column = new DataGridViewTextBoxColumn
        {
            HeaderText = header,
            FillWeight = fillWeight,
            MinimumWidth = minWidth,
            SortMode = DataGridViewColumnSortMode.Automatic,
            DefaultCellStyle =
            {
                Alignment = align switch
                {
                    ColumnAlign.Right => DataGridViewContentAlignment.MiddleRight,
                    ColumnAlign.Center => DataGridViewContentAlignment.MiddleCenter,
                    _ => DataGridViewContentAlignment.MiddleLeft
                }
            }
        };
        if (format != null) column.DefaultCellStyle.Format = format;
        column.HeaderCell.Style.Alignment = column.DefaultCellStyle.Alignment;
        Columns.Add(column);
        return column;
    }

    /// <summary>Ідентифікатор сутності у виділеному рядку (зберігається у Tag).</summary>
    public int? SelectedId => CurrentRow?.Tag as int?;

    public void SelectById(int id)
    {
        foreach (DataGridViewRow row in Rows)
        {
            if (row.Tag is int rowId && rowId == id)
            {
                row.Selected = true;
                CurrentCell = row.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
                if (!row.Displayed) FirstDisplayedScrollingRowIndex = row.Index;
                return;
            }
        }
    }

    /// <summary>Перетворює вміст таблиці у звіт (для експорту/друку того, що бачить користувач).</summary>
    public ReportTable ToReportTable(string title, string subtitle = "")
    {
        var columns = Columns.Cast<DataGridViewColumn>()
            .Where(c => c.Visible)
            .OrderBy(c => c.DisplayIndex)
            .ToList();

        var table = new ReportTable(title, columns.Select(c => new ReportColumn(c.HeaderText, c.DefaultCellStyle.Alignment switch
        {
            DataGridViewContentAlignment.MiddleRight => ColumnAlign.Right,
            DataGridViewContentAlignment.MiddleCenter => ColumnAlign.Center,
            _ => ColumnAlign.Left
        }, c.Width)).ToArray())
        {
            Subtitle = subtitle
        };

        foreach (DataGridViewRow row in Rows)
            table.AddRow(columns.Select(c => (object?)row.Cells[c.Index].FormattedValue?.ToString()).ToArray());

        return table;
    }
}
