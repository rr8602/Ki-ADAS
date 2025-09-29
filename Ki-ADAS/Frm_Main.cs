using Ki_ADAS;
using Ki_ADAS.DB;
using Ki_ADAS.ThreadADAS;
using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace Ki_ADAS
{
    public partial class Frm_Main : Form
    {
        public Frm_Mainfrm m_frmParent = null;
        private SettingConfigDb _db;
        private ModelRepository _modelRepository;
        private InfoRepository _infoRepository;
        private VEPBenchClient _vepBenchClient;
        private IniFile _iniFile;
        private Thread_Main _mainThread;
        public static string ipAddress;
        public static int port;
        public static string barcodeIp;
        private const string CONFIG_SECTION = "Network";
        private const string VEP_IP_KEY = "VepIp";
        private const string VEP_PORT = "VepPort";
        private const string BARCODE_IP_KEY = "BarcodeIp";

        public event EventHandler<VEPBenchSynchroZone> SynchroZoneChanged
        {
            add { _vepBenchClient.SynchroZoneChanged += value; }
            remove { _vepBenchClient.SynchroZoneChanged -= value; }
        }

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
            barcodeIp = _iniFile.ReadValue(CONFIG_SECTION, BARCODE_IP_KEY);

            _modelRepository = new ModelRepository(_db);
            _infoRepository = new InfoRepository(_db);

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
                MsgBox.ErrorWithFormat("ErrorLoadingRegisteredVehicleList", "Error", ex.Message);
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (seqList.SelectedItems.Count == 0)
                {
                    MsgBox.Info("NoRegisteredVehicles", "Notification");
                    return;
                }

                _vepBenchClient.StartMonitoring();
                _mainThread.StartThread();

                AddLogMessage("ADAS process started.");
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStartingADASProcess", "Error", ex.Message);

                AddLogMessage($"Error occurred: {ex.Message}");

                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                CreateBarcodeData(txt_barcode.Text);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDuringVehicleRegistration", "Error", ex.Message);

                AddLogMessage($"Vehicle registration error: {ex.Message}");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            _mainThread.StopThread();
        }

        public void SetBarcodeData(string barcode)
        {
            try
            {
                CreateBarcodeData(barcode);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDuringVehicleRegistration", "Error", ex.Message);
                AddLogMessage($"Error processing barcode: {ex.Message}");
            }
        }

        public void CreateBarcodeData(string barcode)
        {
            string trimmedBarcode = barcode.Trim();
            string pji = trimmedBarcode.Substring(2, 7);
            string modelCode = trimmedBarcode.Substring(trimmedBarcode.Length - 3);
            string modelName = _modelRepository.GetModelNameByBarcode(modelCode);

            if (string.IsNullOrEmpty(modelName))
            {
                MsgBox.ErrorWithFormat("CouldNotFindModelCode", "Error", modelCode);
                AddLogMessage($"Could not find model for barcode: {trimmedBarcode}");
                return;
            }

            if (!_infoRepository.PjiExists(pji))
            {
                var newVehicle = new Info
                {
                    AcceptNo = _infoRepository.GetNextAcceptNo(),
                    PJI = pji,
                    Model = "Sonata"
                };

                if (string.IsNullOrEmpty(newVehicle.AcceptNo))
                {
                    MsgBox.Error("FailedToGenerateAcceptNo");
                    AddLogMessage("Failed to generate a new AcceptNo.");
                    return;
                }

                bool isSaved = _infoRepository.SaveVehicleInfo(newVehicle);

                if (!isSaved)
                {
                    MsgBox.Error("FailedToSaveVehicleInformation", "DBError");
                    AddLogMessage($"Failed to save vehicle info for PJI: {pji}");
                    return;
                }

                var item = new ListViewItem(newVehicle.AcceptNo);
                item.SubItems.Add(newVehicle.PJI);
                item.SubItems.Add(newVehicle.Model);
                seqList.Items.Add(item);

                seqList.EnsureVisible(seqList.Items.Count - 1);

                AddLogMessage($"Vehicle automatically registered: {newVehicle.PJI} / {newVehicle.Model}");
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
            try
            {
                base.OnClosing(e);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorClosingMainForm", "Error", ex.Message);
            }
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            try
            {
                this.seqList.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.seqList_DrawColumnHeader);
                this.seqList.DrawSubItem += new DrawListViewSubItemEventHandler(this.seqList_DrawSubItem);
                this.seqList.SelectedIndexChanged += new EventHandler(this.seqList_SelectedIndexChanged);

                LoadRegisteredVehicles();
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorLoadingMainForm", "Error", ex.Message);
            }
        }

        private void seqList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_frmParent != null && m_frmParent.User_Monitor != null)
                {
                    if (seqList.SelectedItems.Count > 0)
                    {
                        m_frmParent.User_Monitor.UpdateTestStatus(this.SelectedModelInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorUpdatingTestStatus", "Error", ex.Message);
            }
        }

        private void seqList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            try
            {
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    e.DrawBackground();
                    e.Graphics.DrawString(e.Header.Text, e.Font, new SolidBrush(this.seqList.ForeColor), e.Bounds, sf);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDrawingColumnHeader", "Error", ex.Message);
            }
        }

        private void seqList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            try
            {
                TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

                if (e.Item.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, SystemColors.HighlightText, flags);
                }
                else
                {
                    Color backColor = (e.ItemIndex % 2 == 0)
                        ? Color.White
                        : Color.FromArgb(255, 240, 240, 240);

                    using (Brush b = new SolidBrush(backColor))
                    {
                        e.Graphics.FillRectangle(b, e.Bounds);
                    }

                    TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, e.SubItem.ForeColor, flags);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDrawingSubItem", "Error", ex.Message);
            }
        }

        private void seqList_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                var selectedVehicle = SelectedVehicleInfo;

                lbl_model.Text = selectedVehicle?.Model ?? "-";
                lbl_pji.Text = selectedVehicle?.PJI ?? "-";
                lbl_wheelbase.Text = SelectedModelInfo?.Wheelbase.ToString() ?? "-";
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorUpdatingVehicleInfo", "Error", ex.Message);
            }
        }
    }
}