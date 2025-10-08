using System;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace Zebra420T
{
    public partial class ZebraForm : Form
    {
        private readonly IZebraPrintData _printData;

        public ZebraForm()
        {
            InitializeComponent();
        }

        public ZebraForm(IZebraPrintData printData) : this()
        {
            _printData = printData;
        }

        private void ZebraForm_Load(object sender, EventArgs e)
        {
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinters.Items.Add(printer);
            }

            PrinterSettings settings = new PrinterSettings();

            if (cmbPrinters.Items.Contains(settings.PrinterName))
            {
                cmbPrinters.SelectedItem = settings.PrinterName;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (cmbPrinters.SelectedItem == null)
            {
                lblStatus.Text = "Status: Error - No printer selected.";
                MessageBox.Show("Please select a printer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_printData == null)
            {
                lblStatus.Text = "Status: Error - No data to print.";
                MessageBox.Show("No data available to print.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                lblStatus.Text = "Status: Processing...";
                this.Update();

                string printableString = _printData.GeneratePrintString();
                string zplString = GenerateZpl(printableString);

                MessageBox.Show(zplString, "Generated ZPL");

                RawPrinterHelper.SendStringToPrinter(cmbPrinters.SelectedItem.ToString(), zplString);

                lblStatus.Text = "Status: Print job sent successfully.";
                MessageBox.Show("Print job sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Status: Error - {ex.Message}";
                MessageBox.Show($"An error occurred while printing:\n\n{ex.Message}", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateZpl(string printString)
        {
            const int labelHeightDots = 610;
            const int margin = 80;

            var allLines = printString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder zpl = new StringBuilder();
            zpl.AppendLine("^XA");
            zpl.AppendLine("^CI28");

            const int fontHeight = 40;
            const int fontWidth = 40;
            const int lineHeight = 45;

            int yPosition = margin;

            foreach (var line in allLines)
            {
                if (yPosition + fontHeight > labelHeightDots - margin)
                {
                    break;
                }

                // AON : 기본 폰트 출력
                // A@N : 다운로드 폰트 출력 (ex: 한글)
                zpl.AppendLine($"^FO{margin},{yPosition}^A0N,{fontHeight},{fontWidth}^FD{line.Trim()}^FS");
                yPosition += lineHeight;
            }

            zpl.AppendLine("^XZ");

            return zpl.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
