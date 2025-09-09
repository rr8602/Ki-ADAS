using Ki_ADAS.VEPBench;
using Ki_ADAS.DB;
using Ki_ADAS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace Ki_ADAS
{
    public partial class Frm_Main : Form
    {
        public Frm_Mainfrm m_frmParent = null;
        private Frm_VEP _vep;
        private SettingConfigDb _db;
        private ModelRepository _modelRepository;
        private InfoRepository _infoRepository;
        private Frm_Config _frmConfig;
        private VEPBenchClient _vepBenchClient;
        private IniFile _iniFile;
        private Thread_Main _mainThread;
        public static string ipAddress;
        public static int port;
        private const string CONFIG_SECTION = "Network";
        private const string VEP_IP_KEY = "VepIp";
        private const string VEP_PORT = "VepPort";

        public Frm_Main(SettingConfigDb dbInstance, VEPBenchClient client)
        {
            _db = dbInstance;
            _vepBenchClient = client;
            _mainThread = new Thread_Main(this, _vepBenchClient, _db);
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            string iniPath = Path.Combine(Application.StartupPath, "config.ini");
            _iniFile = new IniFile(iniPath);

            ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
            port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

            _modelRepository = new ModelRepository(_db);
            _infoRepository = new InfoRepository(_db);
            _frmConfig = new Frm_Config(_db);

            BtnStop.Enabled = false;
        }

        public Info SelectedVehicleInfo
        {
            get
            {
                ListViewItem itemToUse = null;

                if (seqList.SelectedItems.Count > 0)
                {
                    itemToUse = seqList.SelectedItems[0];
                }
                else if (seqList.Items.Count > 0)
                {
                    itemToUse = seqList.Items[0];
                }

                if (itemToUse != null)
                {
                    return new Info
                    {
                        AcceptNo = itemToUse.SubItems[0].Text,
                        PJI = itemToUse.SubItems[1].Text,
                        Model = itemToUse.SubItems[2].Text
                    };
                }

                return null;
            }
        }

        public Model SelectedModelInfo
        {
            get
            {
                var vehicleInfo = SelectedVehicleInfo;

                if (vehicleInfo == null)
                {
                    return new Model();
                }

                return _modelRepository.GetModelDetails(vehicleInfo.Model);
            }
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void LoadRegisteredVehicles()
        {
            try
            {
                seqList.Items.Clear();
                var vehicles = _infoRepository.GetRegisteredVehicles();

                foreach (var vehicle in vehicles)
                {
                    var item = new ListViewItem(vehicle.AcceptNo);
                    item.SubItems.Add(vehicle.PJI);
                    item.SubItems.Add(vehicle.Model);
                    seqList.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading registered vehicle list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (seqList.SelectedItems.Count == 0)
                {
                    MessageBox.Show("No registered vehicles. Please register a vehicle first.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _vepBenchClient.StartMonitoring();
                _mainThread.StartThread();

                AddLogMessage("ADAS process started.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while starting the ADAS process: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLogMessage($"Error occurred: {ex.Message}");

                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string barcode = txt_barcode.Text.Trim();

                if (string.IsNullOrEmpty(barcode))
                {
                    MessageBox.Show("Please enter the barcode.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string pji = barcode.Substring(2, 7);
                string modelCode = barcode.Substring(barcode.Length - 3);
                string modelName = _modelRepository.GetModelNameByBarcode(modelCode);

                if (string.IsNullOrEmpty(modelName))
                {
                    MessageBox.Show($"Could not find model code: {modelCode}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var newVehicle = new Info
                {
                    AcceptNo = _infoRepository.GetNextAcceptNo(),
                    PJI = pji,
                    Model = modelName
                };

                if (string.IsNullOrEmpty(newVehicle.AcceptNo))
                {
                    MessageBox.Show("Failed to generate AcceptNo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool isSaved = _infoRepository.SaveVehicleInfo(newVehicle);
                if (!isSaved)
                {
                    MessageBox.Show("Failed to save vehicle information.", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var item = new ListViewItem(newVehicle.AcceptNo);
                item.SubItems.Add(newVehicle.PJI);
                item.SubItems.Add(newVehicle.Model);
                seqList.Items.Add(item);

                txt_barcode.Clear();
                AddLogMessage($"Vehicle registration complete: {newVehicle.PJI} / {newVehicle.Model}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during vehicle registration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLogMessage($"Vehicle registration error: {ex.Message}");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mainThread == null || !_vepBenchClient.IsConnected)
                {
                    MessageBox.Show("ADAS process is not running.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _mainThread.StopThread();
                _vepBenchClient.StopMonitoring();
                _vepBenchClient.DisConnect();

                BtnStart.Enabled = true;
                BtnStop.Enabled = false;

                AddLogMessage("ADAS process stopped.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while stopping the ADAS process: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLogMessage($"Error occurred: {ex.Message}");
            }
        }

        public void AddLogMessage(string message)
        {
            if (lb_Message.InvokeRequired)
            {
                lb_Message.Invoke(new Action<string>(AddLogMessage), new object[] { message });
                return;
            }

            lb_Message.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            lb_Message.SelectedIndex = lb_Message.Items.Count - 1;
        }

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            base.OnClosing(e);
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            this.seqList.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.seqList_DrawColumnHeader);
            this.seqList.DrawSubItem += new DrawListViewSubItemEventHandler(this.seqList_DrawSubItem);
            this.seqList.SelectedIndexChanged += new System.EventHandler(this.seqList_SelectedIndexChanged);

            LoadRegisteredVehicles();
        }

        private void seqList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_frmParent != null && m_frmParent.User_Monitor != null)
            {
                if (seqList.SelectedItems.Count > 0)
                {
                    m_frmParent.User_Monitor.UpdateTestStatus(this.SelectedModelInfo);
                }
            }
        }

        private void seqList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                e.DrawBackground();
                e.Graphics.DrawString(e.Header.Text, e.Font, new SolidBrush(this.seqList.ForeColor), e.Bounds, sf);
            }
        }

        private void seqList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, SystemColors.HighlightText, flags);
            }
            else
            {
                e.DrawBackground();
                TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, e.SubItem.ForeColor, flags);
            }
        }

        private void seqList_MouseClick(object sender, MouseEventArgs e)
        {
            var selectedVehicle = SelectedVehicleInfo;

            lbl_model.Text = selectedVehicle?.Model ?? "-";
            lbl_pji.Text = selectedVehicle?.PJI ?? "-";
            lbl_wheelbase.Text = SelectedModelInfo?.Wheelbase.ToString() ?? "-";
        }
    }
}