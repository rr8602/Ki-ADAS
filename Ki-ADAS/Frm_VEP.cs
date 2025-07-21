using Ki_ADAS.VEPBench;
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

namespace Ki_ADAS
{
    public partial class Frm_VEP : Form
    {
        private Frm_Mainfrm m_frmParent = null;
        private VEPBenchClient benchClient = new VEPBenchClient("192.168.10.98", 502);
        private IniFile _iniFile;

        public Frm_VEP()
        {
            InitializeComponent();

            benchClient.Connect();
            benchClient.DebugMode = true;

            benchClient.DescriptionZoneRead += BenchClient_OnDescriptionZoneRead;
            benchClient.StatusZoneChanged += BenchClient_StatusZoneChanged;
            benchClient.SynchroZoneChanged += BenchClient_SynchroZoneChanged;
            benchClient.TransmissionZoneChanged += BenchClient_TransmissionZoneChanged;
            benchClient.ReceptionZoneChanged += BenchClient_ReceptionZoneChanged;

            benchClient.StartMonitoring();
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void BenchClient_OnDescriptionZoneRead(object sender, VEPBenchDescriptionZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, VEPBenchDescriptionZone>(BenchClient_OnDescriptionZoneRead), sender, e);
                return;
            }

            txtDesZone.Text = e.ValidityIndicator.ToString();
            txtStatusZoneAddress.Text = e.StatusZoneAddr.ToString();
            txtStatusZoneSize.Text = e.StatusZoneSize.ToString();
            txtSynchroZoneAddress.Text = e.SynchroZoneAddr.ToString();
            txtSynchroZoneSize.Text = e.SynchroZoneSize.ToString();
            txtTzAddress.Text = e.TransmissionZoneAddr.ToString();
            txtTzSize.Text = e.TransmissionZoneSize.ToString();
            txtReAddress.Text = e.ReceptionZoneAddr.ToString();
            txtReSize.Text = e.ReceptionZoneSize.ToString();
            txtAddTzAddress.Text = e.AdditionalTZAddr.ToString();
            txtAddTzSize.Text = e.AdditionalTZSize.ToString();
            txtAddReAddress.Text = e.AdditionalRZAddr.ToString();
            txtAddReSize.Text = e.AdditionalRZSize.ToString();
        }

        private void BenchClient_StatusZoneChanged(object sender, VEPBenchStatusZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateStatusInfo(
                    e.VepStatus,
                    e.VepCycleEnd,
                    e.BenchCycleEnd,
                    e.StartCycle,
                    e.VepCycleInterruption,
                    e.BenchCycleInterruption)));
            }
            else
            {
                UpdateStatusInfo(
                    e.VepStatus,
                    e.VepCycleEnd,
                    e.BenchCycleEnd,
                    e.StartCycle,
                    e.VepCycleInterruption,
                    e.BenchCycleInterruption);
            }
        }

        private void BenchClient_SynchroZoneChanged(object sender, VEPBenchSynchroZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateSynchroValues(e.Angle1, e.Angle2, e.Angle3)));
            }
            else
            {
                UpdateSynchroValues(e.Angle1, e.Angle2, e.Angle3);
            }
        }

        private void BenchClient_TransmissionZoneChanged(object sender, VEPBenchTransmissionZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateTransmissionInfo(e.AddTzSize, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode)));
            }
            else
            {
                UpdateTransmissionInfo(e.AddTzSize, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode);
            }

            if (e.IsRequest)
            {
                Console.WriteLine($"TransmissionZoneChanged 이벤트: 요청 감지 FctCode={e.FctCode}, PCNum={e.PCNum}");
            }
        }

        private void BenchClient_ReceptionZoneChanged(object sender, VEPBenchReceptionZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateReceptionInfo(e.AddReSize, e.ExchStatus, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode)));
            }
            else
            {
                UpdateReceptionInfo(e.AddReSize, e.ExchStatus, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode);
            }

            string status = e.IsResponseCompleted ? "응답 완료" : "응답 준비";
            Console.WriteLine($"ReceptionZoneChanged 이벤트: {status}, FctCode={e.FctCode}");
        }

        public void UpdateStatusInfo(ushort vepStatus, ushort vepCycleEnd, ushort benchCycleEnd, ushort startCycle, ushort vepCycleInt, ushort benchCycleInt)
        {
            txtStVepStatus.Text = vepStatus.ToString();
            txtStVepCycleEnd.Text = vepCycleEnd.ToString();
            txtStBenchCycleEnd.Text = benchCycleEnd.ToString();
            txtStStartCycle.Text = startCycle.ToString();
            txtStVepCycleInt.Text = vepCycleInt.ToString();
            txtStBenchCycleInt.Text = benchCycleInt.ToString();
        }

        public void UpdateSynchroValues(double angle, double angle2, double angle3)
        {
            txtAngle1.Text = angle.ToString("F2");
            txtAngle2.Text = angle2.ToString("F2");
            txtAngle3.Text = angle3.ToString("F2");
        }

        public void UpdateTransmissionInfo(ushort size, byte fctCode, byte pcNum, byte processCode, byte subFctCode)
        {
            txtAddrTzSize.Text = size.ToString();
            txtTzFctCode.Text = fctCode.ToString();
            txtTzPCNum.Text = pcNum.ToString();
            txtTzProcessCode.Text = processCode.ToString();
            txtTzSubFctCode.Text = subFctCode.ToString();
            txtTzExchStatus.Text = "2"; // 요청 가능 상태로 초깃값 설정
        }

        public void UpdateReceptionInfo(ushort size, ushort exchStatus, byte fctCode, byte pcNum, byte processCode, byte subFctCode)
        {
            txtAddrReSize.Text = size.ToString();
            txtReExchStatus.Text = exchStatus.ToString();
            txtReFctCode.Text = fctCode.ToString();
            txtRePCNum.Text = pcNum.ToString();
            txtReProcessCode.Text = processCode.ToString();
            txtReSubFctCode.Text = subFctCode.ToString();
        }
    }
}
