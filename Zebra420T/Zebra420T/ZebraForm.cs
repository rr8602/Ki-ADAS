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
                
                // RawPrinterHelper.SendStringToPrinter(cmbPrinters.SelectedItem.ToString(), zplString);

                lblStatus.Text = "Status: Print job sent successfully.";
                
                MessageBox.Show(zplString, "Generated ZPL");
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Error - See message box for details.";
                MessageBox.Show($"An error occurred: {ex.Message}", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateZpl(string printString)
        {
            var allLines = printString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder zpl = new StringBuilder();
            zpl.AppendLine("^XA");
            zpl.AppendLine("^CI28");

            int yPosition = 30;
            const int lineHeight = 35;
            const int textBlockWidth = 1600;

            foreach (var line in allLines)
            {
                zpl.AppendLine($"^FO20,{yPosition}^A@N,27,27^FB{textBlockWidth},2,0,L,0^FD{line.Trim()}^FS");
                yPosition += lineHeight;
            }

            yPosition += 20;

            zpl.AppendLine("^XZ");

            return zpl.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}