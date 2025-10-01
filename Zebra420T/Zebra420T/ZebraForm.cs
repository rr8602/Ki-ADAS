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
            const int labelWidthDots = 862;
            const int labelHeightDots = 406;
            const int margin = 20;

            var allLines = printString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder zpl = new StringBuilder();
            zpl.AppendLine("^XA");
            zpl.AppendLine("^CI28");

            const int textBlockWidth = labelWidthDots - (margin * 2);

            const int fontHeight = 20;
            const int fontWidth = 18;
            const int lineHeight = 25;

            int yPosition = margin;

            foreach (var line in allLines)
            {
                if (yPosition + fontHeight > labelHeightDots - margin)
                {
                    break;
                }

                zpl.AppendLine($"^FO{margin},{yPosition}^A@N,{fontHeight},{fontWidth},E:KFONT3.FNT^FB{textBlockWidth},2,0,L,0^FD{line.Trim()}^FS");
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
