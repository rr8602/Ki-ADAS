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

            lblStatus.Text = status;
            lblStatus.ForeColor = color;
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
            progressBar.Value = Math.Min(100, Math.Max(0, percentage));
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

            lvProcessLog.Items.Add(item);
            lvProcessLog.Items[lvProcessLog.Items.Count - 1].EnsureVisible();
        }

        // 로그 초기화
        public void ClearLog()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(ClearLog));

                return;
            }

            lvProcessLog.Items.Clear();
            progressBar.Value = 0;
        }
    }
}
