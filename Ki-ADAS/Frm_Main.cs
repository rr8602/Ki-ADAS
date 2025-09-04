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
        private ADASProcess _adasProcess;
        private IniFile _iniFile;
        public static string ipAddress;
        public static int port;
        private const string CONFIG_SECTION = "Network";
        private const string VEP_IP_KEY = "VepIp";
        private const string VEP_PORT = "VepPort";

        public Frm_Main(SettingConfigDb dbInstance, VEPBenchClient client)
        {
            _db = dbInstance;
            _vepBenchClient = client;
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
            _adasProcess = new ADASProcess(_vepBenchClient, _modelRepository, this);

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
                    MessageBox.Show("테스트 목록에서 항목을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string selectedBarcode = seqList.SelectedItems[0].Text;

                /*
                _adasProcess.OnProcessStepChanged += ADASProcess_OnProcessStepChanged;
                _adasProcess.InitializeProcessSteps();

                bool success = _adasProcess.Start(ipAddress, port);

                // VEP 클라이언트가 없거나 연결이 끊어진 경우 새로 생성
                if (_vepBenchClient == null || !_vepBenchClient.IsConnected)
                {
                    if (_vepBenchClient != null)
                    {
                        _vepBenchClient.DisConnect();
                    }

                    _vepBenchClient = new VEPBenchClient(ipAddress, port);
                    _vepBenchClient.Connect();

                    // ADAS 프로세스 재초기화
                    _adasProcess = new ADASProcess(_vepBenchClient);
                    _adasProcess.OnProcessStepChanged += ADASProcess_OnProcessStepChanged;
                }*/
                // 임시 HomePositionSimulator 실행 부분 (나중에 삭제)
                bool connected = _vepBenchClient.TestConnection();

                if (connected)
                {
                    BtnStart.Enabled = false;
                    BtnStop.Enabled = true;

                    AddLogMessage("VEP 연결 성공 - 시뮬레이터를 통해 ADAS 프로세스 시작 가능");
                    AddLogMessage("'홈 포지션 시뮬레이터' 버튼을 클릭하여 테스트를 시작하세요.");

                    _vepBenchClient.DebugMode = false;
                    _vepBenchClient.StartMonitoring();

                    if (MessageBox.Show("홈 포지션 시뮬레이터를 지금 실행하시겠습니까?",
                        "시뮬레이터 실행", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        HomePositionSimulator simulator = new HomePositionSimulator(_vepBenchClient, _modelRepository, this);
                        simulator.Show();
                    }
                }
                else
                {
                    AddLogMessage("VEP 연결 실패 - IP 주소와 포트를 확인해주세요");
                    MessageBox.Show($"VEP 연결에 실패했습니다. IP 주소({ipAddress})와 포트({port})를 확인하세요.",
                        "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"오류 발생: {ex.Message}");

                MessageBox.Show($"ADAS 프로세스 시작 중 오류가 발생했습니다: {ex.Message}",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
            }

            /*try
            {
                string ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
                int port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

                bool started = _adasProcess.Start(ipAddress, port);

                if (started)
                {
                    BtnStart.Enabled = false;
                    BtnStop.Enabled = true;

                    AddLogMessage("ADAS  프로세스 시작");
                }
                else
                {
                    AddLogMessage("ADAS 프로세스 시작 실패");
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"오류 발생: {ex.Message}");
            }*/
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string barcode = txt_barcode.Text.Trim();

                if (string.IsNullOrEmpty(barcode))
                {
                    MessageBox.Show("바코드를 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string pji = barcode.Substring(2, 7);
                string modelCode = barcode.Substring(barcode.Length - 3);
                string modelName = _modelRepository.GetModelNameByBarcode(modelCode);

                if (string.IsNullOrEmpty(modelName))
                {
                    MessageBox.Show($"모델 코드를 찾을 수 없습니다: {modelCode}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("AcceptNo 생성에 실패했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool isSaved = _infoRepository.SaveVehicleInfo(newVehicle);
                if (!isSaved)
                {
                    MessageBox.Show("차량 정보 저장에 실패했습니다.", "DB 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var item = new ListViewItem(newVehicle.AcceptNo);
                item.SubItems.Add(newVehicle.PJI);
                item.SubItems.Add(newVehicle.Model);
                seqList.Items.Add(item);

                txt_barcode.Clear();
                AddLogMessage($"차량 등록 완료: {newVehicle.PJI} / {newVehicle.Model}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"차량 등록 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddLogMessage($"차량 등록 오류: {ex.Message}");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _adasProcess.Stop();
                _vepBenchClient.StopMonitoring();
                _vepBenchClient.DisConnect();

                BtnStart.Enabled = true;
                BtnStop.Enabled = false;

                AddLogMessage("ADAS 프로세스 중지");
            }
            catch (Exception ex)
            {
                AddLogMessage($"오류 발생: {ex.Message}");
            }
        }

        private void ADASProcess_OnProcessStepChanged(object sender, ADASProcess.ADASProcessEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ADASProcess_OnProcessStepChanged(sender, e)));

                return;
            }
            
            if (m_frmParent != null && m_frmParent.User_Monitor != null)
            {
                if (e.Step != null)
                {
                    m_frmParent.User_Monitor.UpdateStepDescription(e.Step.Description);
                }

                if (e.StateType == ADASProcess.ProcessStateType.Success && e.Message == "프로세스 완료")
                {
                    m_frmParent.User_Monitor.UpdateStepDescription("테스트 완료");
                }
            }

            string logMessage = $"[{e.StateType}] {e.Message}";

            AddLogMessage(logMessage);

            if (e.StateType == ADASProcess.ProcessStateType.Success && e.Message == "프로세스 완료")
            {
                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
                AddLogMessage("ADAS 프로세스가 성공적으로 완료되었습니다.");
            }
            else if (e.StateType == ADASProcess.ProcessStateType.Error)
            {
                BtnStart.Enabled = true;
                BtnStop.Enabled = false;
                AddLogMessage("ADAS 프로세스가 오류로 인해 중지되었습니다.");
            }
        }

        private void AddLogMessage(string message)
        {
            lb_Message.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            lb_Message.SelectedIndex = lb_Message.Items.Count - 1;
        }

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_adasProcess != null)
            {
                _adasProcess.Stop();
            }

            base.OnClosing(e);
        }

        private void BtnTestModbus_Click(object sender, EventArgs e)
        {
            try
            {
                string ipAddress = _iniFile.ReadValue(CONFIG_SECTION, VEP_IP_KEY);
                int port = _iniFile.ReadInteger(CONFIG_SECTION, VEP_PORT);

                if (_vepBenchClient != null)
                {
                    _vepBenchClient.DisConnect();
                }

                _vepBenchClient = new VEPBenchClient(ipAddress, port);
                _adasProcess = new ADASProcess(_vepBenchClient, _modelRepository, this);
                _adasProcess.OnProcessStepChanged += ADASProcess_OnProcessStepChanged;

                bool success = _adasProcess.Start(ipAddress, port);

                if (success)
                {
                    BtnTestModbus.Enabled = false;
                }
                else
                {
                    MessageBox.Show("ADAS 프로세스 시작에 실패했습니다.", "시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}", "예외 발생", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            this.seqList.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.seqList_DrawColumnHeader);
            this.seqList.DrawSubItem += new DrawListViewSubItemEventHandler(this.seqList_DrawSubItem);

            LoadRegisteredVehicles();
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
    }
}