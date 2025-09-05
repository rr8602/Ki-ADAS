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
        private static Frm_Operator _instance;
        private Point mousePoint; // 현재 마우스 포인터의 좌표저장 변수 선언
        public static Frm_Operator Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    _instance = new Frm_Operator();
                }

                return _instance;
            }
        }

        public Frm_Operator()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
            MoveFormToSecondMonitor();
        }
        private void Frm_Operator_Load(object sender, EventArgs e)
        {

        }

        private void MoveFormToSecondMonitor()
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
                MessageBox.Show("두 번째 모니터가 감지되지 않았습니다.");
            }
        }

        private void Frm_Operator_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        // 상태 업데이트
        public void UpdateStatus(string status, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, Color>(UpdateStatus), status, color);
                
                return;
            }

          //  lblStatus.Text = status;
            //lblStatus.ForeColor = color;
        }

        // 진행률 업데이트
        public void UpdateProgress(int currentStep, int totalSteps)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int, int>(UpdateProgress), currentStep, totalSteps);

                return;
            }

            int percentage = (int)((float)currentStep / totalSteps * 100);
            //progressBar.Value = Math.Min(100, Math.Max(0, percentage));
        }

        // 로그 추가
        public void AddLog(DateTime timestamp, string type, string stepId, string description, string syncState, string result)
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

        // 로그 초기화
        public void ClearLog()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(ClearLog));

                return;
            }

            //lvProcessLog.Items.Clear();
           // progressBar.Value = 0;
        }

        private void NavTop_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
            {
                //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }
        }

        private void NavTop_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장
        }

        public void UpdateStepDescription(string description)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStepDescription), description);
                return;
            }

            //roundLabel4.Text = description;
        }
    }
}
