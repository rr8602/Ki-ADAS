using Ki_ADAS.DB;
using Ki_ADAS.VEPBench;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public partial class Frm_Operator : Form
    {
        private ModelRepository modelRepository;
        private Frm_Main main;
        private static Frm_Operator _instance;
        private VEPBenchDataManager _vepManager = GlobalVal.Instance._VEP;
        private Point mousePoint; // 현재 마우스 포인터의 좌표저장 변수 선언

        private Timer inspectionTimer;
        private int elapsedTimeInSeconds;

        public void StartInspectionTimer()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(StartInspectionTimer));
                    return;
                }

                elapsedTimeInSeconds = 0;
                rlbl_time.Text = "0";
                inspectionTimer.Start();
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStartingInspectionTimer", "Error", ex.Message);
            }
        }

        public void StopInspectionTimer()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(StopInspectionTimer));
                    return;
                }

                inspectionTimer.Stop();
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorStoppingInspectionTimer", "Error", ex.Message);
            }
        }

        private void InspectionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                elapsedTimeInSeconds++;
                rlbl_time.Text = elapsedTimeInSeconds.ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorUpdatingInspectionTimer", "Error", ex.Message);
            }
        }

        public Frm_Operator()
        {
            InitializeComponent();

            inspectionTimer = new Timer();
            inspectionTimer.Interval = 1000;
            inspectionTimer.Tick += InspectionTimer_Tick;

            modelRepository = new ModelRepository(new SettingConfigDb());
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
            MoveFormToSecondMonitor();
        }

        public Frm_Operator(Frm_Main mainForm) : this()
        {
            this.main = mainForm;

            if (this.main != null)
            {
                this.main.SynchroZoneChanged += angle_SynchroZoneChanged;
                ClearLog();
            }
        }

        private void MoveFormToSecondMonitor()
        {
            try
            {
                // 연결된 모든 모니터 가져오기
                Screen[] screens = Screen.AllScreens;

                if (screens.Length > 1)
                {
                    // 두 번째 모니터 가져오기
                    Screen secondScreen = screens[1];

                    // 두 번째 모니터의 작업 영역 중앙에 폼 위치
                    Rectangle workingArea = secondScreen.WorkingArea;
                    this.StartPosition = FormStartPosition.Manual;
                    this.Location = new Point(
                        workingArea.Left + (workingArea.Width - this.Width) / 2,
                        workingArea.Top + (workingArea.Height - this.Height) / 2
                    );
                }
                else
                {
                    MsgBox.Info("SecondMonitorNotDetected");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMovingFormToSecondMonitor", "Error", ex.Message);
            }
        }

        private void Frm_Operator_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (main != null)
                {
                    main.SynchroZoneChanged -= angle_SynchroZoneChanged;
                }

                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorClosingOperatorForm", "Error", ex.Message);
            }
        }

        // 상태 업데이트
        public void UpdateTestStatus(Model selectedModel)
        {
            try
            {
                if (selectedModel == null) return;

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<Model>(UpdateTestStatus));

                    return;
                }

                Color activeColor = Color.Yellow;
                Color defaultColor = Color.Gray;

                lbl_Toe_FL.BackColor = selectedModel.FC_IsTest ? activeColor : defaultColor;
                lbl_Toe_FR.BackColor = selectedModel.F_IsTest ? activeColor : defaultColor;
                lbl_Toe_RR.BackColor = selectedModel.R_IsTest ? activeColor : defaultColor;

                rlbl_modelName.Text = selectedModel.Name;
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorUpdatingTestStatusOperator", "Error", ex.Message);
            }
        }

        // 진행률 업데이트
        public void UpdateProgress(int currentStep, int totalSteps)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<int, int>(UpdateProgress), currentStep, totalSteps);

                    return;
                }

                int percentage = (int)((float)currentStep / totalSteps * 100);
                //progressBar.Value = Math.Min(100, Math.Max(0, percentage));
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorUpdatingProgress", "Error", ex.Message);
            }
        }

        // 로그 추가
        public void AddLog(DateTime timestamp, string type, string stepId, string description, string syncState, string result)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<DateTime, string, string, string, string, string>(AddLog), timestamp, type, stepId, description, syncState, result);

                    return;
                }

                ListViewItem item = new ListViewItem(timestamp.ToString("yyyy-MM-dd HH:mm:ss"));

                switch (type.ToLower())
                {
                    case "성공":
                        item.ForeColor = Color.Green;
                        break;

                    case "실패":
                        item.ForeColor = Color.Red;
                        break;

                    case "경고":
                        item.ForeColor = Color.Orange;
                        break;

                    case "진행 중":
                        item.ForeColor = Color.Blue;
                        break;

                    default:
                        item.ForeColor = Color.Black;
                        break;
                }

                item.SubItems.Add(type);
                item.SubItems.Add(stepId);
                item.SubItems.Add(description);
                item.SubItems.Add(syncState);
                item.SubItems.Add(result);

                //lvProcessLog.Items.Add(item);
                //lvProcessLog.Items[lvProcessLog.Items.Count - 1].EnsureVisible();
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorAddingLog", "Error", ex.Message);
            }
        }

        // 로그 초기화
        public void ClearLog()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(ClearLog));

                    return;
                }

                lbl_message.Text = string.Empty;
                //progressBar.Value = 0;
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorClearingLog", "Error", ex.Message);
            }
        }

        private void NavTop_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
                {
                    //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                    Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorMovingForm", "Error", ex.Message);
            }
        }

        private void NavTop_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCapturingMouseDown", "Error", ex.Message);
            }
        }

        public void UpdateStepDescription(string languageKey)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<string>(UpdateStepDescription), languageKey);
                    return;
                }

                lbl_message.Text = LanguageManager.GetString(languageKey);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorUpdatingStepDescription", "Error", ex.Message);
            }
        }

        private void angle_SynchroZoneChanged(object sender, VEPBenchSynchroZone e)
        {
            UpdateAngleResult();
        }

        public void UpdateAngleResult()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateAngleResult));
                return;
            }

            lbl_roll.Text = _vepManager.SynchroZone.FrontCameraAngle1.ToString("F2");
            lbl_azimuth.Text = _vepManager.SynchroZone.FrontCameraAngle2.ToString("F2");
            lbl_elevation.Text = _vepManager.SynchroZone.FrontCameraAngle3.ToString("F2");
            lbl_FLeft.Text = _vepManager.SynchroZone.FrontLeftRadarAngle.ToString("F2");
            lbl_FRight.Text = _vepManager.SynchroZone.FrontRightRadarAngle.ToString("F2");
            lbl_RLeft.Text = _vepManager.SynchroZone.RearLeftRadarAngle.ToString("F2");
            lbl_RRight.Text = _vepManager.SynchroZone.RearRightRadarAngle.ToString("F2");
        }
    }
}
