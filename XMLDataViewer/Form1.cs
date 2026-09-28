using System.Xml;
using XMLDataViewer.Core.Models;
using XMLDataViewer.Core.Reader;
using XMLDataViewer.Core.Utils;

namespace XMLDataViewer
{
    public partial class Form1 : Form
    {
        private readonly ICarSalesReader _salesReader = new CarSalesReader();
        private bool _isLoading = false;

        private CancellationTokenSource? _cts;

        public Form1()
        {
            InitializeComponent();
            SetupGrid();
        }

        private void SetupGrid()
        {
            salesView.AutoGenerateColumns = false;
            salesView.AllowUserToResizeColumns = false;
            salesView.AllowUserToResizeRows = false;
            salesView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            salesView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            salesView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            salesView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            salesView.DefaultCellStyle.Padding = new Padding(5, 2, 0, 0);

            salesView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            salesView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            salesView.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 2, 0, 0);

            salesView.CellPainting += (sender, e) =>
            {
                if (e.RowIndex != -1 || e.ColumnIndex < 0) return;

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                using var brush = new SolidBrush(Color.White);
                e.Graphics?.FillRectangle(brush, e.CellBounds.Right - 1, e.CellBounds.Top + 1, 1, e.CellBounds.Height - 2);
                e.Handled = true;
            };

            salesView.Columns.Clear();

            salesView.ColumnHeadersHeight = 40;
            salesView.RowTemplate.Height = 40;

            salesView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColModelAndCost",
                HeaderText = $"{Properties.Resources.TypeHeader}{Environment.NewLine}{Properties.Resources.TypeCostHeader}",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Resizable = DataGridViewTriState.False,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            salesView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ColCostWithDph",
                HeaderText = $"{Environment.NewLine}{Properties.Resources.DPHCostHeader}",
                Width = 100,
                Resizable = DataGridViewTriState.False,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });
        }


        private void SalesView_Paint(object? sender, PaintEventArgs e)
        {
            if (!_isLoading && salesView.Rows.Count > 0)
                return;

            var text = _isLoading
                ? Properties.Resources.DataLoading
                : Properties.Resources.DataEmpty;

            using var font = new Font(salesView.Font.FontFamily, 10, FontStyle.Regular);
            using var brush = new SolidBrush(Color.Black);

            using var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            e.Graphics.DrawString(text, font, brush, salesView.ClientRectangle, stringFormat);
        }

        private void salesView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || salesView.Rows[e.RowIndex].DataBoundItem is not WeekendSalesSummary summary)
                return;

            var columnName = salesView.Columns[e.ColumnIndex].Name;

            switch (columnName)
            {
                case "ColModelAndCost":
                    e.Value = $"{summary.ModelName}{Environment.NewLine}{summary.TotalCostWithoutDph:N2}";
                    e.FormattingApplied = true;
                    break;
                case "ColCostWithDph":
                    e.Value = $"{Environment.NewLine}{summary.TotalCostWithDph:N2}";
                    e.FormattingApplied = true;
                    break;
            }
        }

        private async void loadButton_Click(object sender, EventArgs e)
        {

            try
            {
                using var openFileDialog = new OpenFileDialog
                {
                    Filter = "XML soubory (*.xml)|*.xml|Všechny soubory (*.*)|*.*",
                    Title = "Vyberte soubor",
                    Multiselect = false
                };

                if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                _cts = new CancellationTokenSource();
                loadButton.Enabled = false;
                cancelButton.Enabled = true;
                Cursor = Cursors.WaitCursor;

                _isLoading = true;
                salesView.DataSource = null;
                salesView.Invalidate();

                await using var stream = File.OpenRead(openFileDialog.FileName);
                var sales = await _salesReader.ReadXmlAsync(stream, _cts.Token);

                var summaries = await Task.Run(
                    () => CalcUtils.CalculateWeekendSalesSummaries(sales),
                    _cts.Token);

                salesView.DataSource = summaries;

            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(
                    this,
                    Properties.Resources.Dialog_Cancel_Text,
                    Properties.Resources.Dialog_Cancel_Title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex) when (ex is FormatException or XmlException or IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    Properties.Resources.Dialog_Parse_Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    this,
                    Properties.Resources.Dialog_Error_Text,
                    Properties.Resources.Dialog_Error_Title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
                salesView.Invalidate();
                loadButton.Enabled = true;
                cancelButton.Enabled = false;
                Cursor = Cursors.Default;
                _cts?.Dispose();
                _cts = null;

            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
        }
    }
}
