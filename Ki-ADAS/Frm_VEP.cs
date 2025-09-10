using Ki_ADAS.VEPBench;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public partial class Frm_VEP : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;

        private Frm_Mainfrm m_frmParent = null;
        private VEPBenchClient benchClient;

        private Dictionary<string, Action<int>> _propertySetters;
        private Dictionary<string, Func<int>> _propertyGetters;

        public Frm_VEP(VEPBenchClient client)
        {
            InitializeComponent();
            benchClient = client;

            InitializeMappings();

            benchClient.DescriptionZoneRead += BenchClient_OnDescriptionZoneRead;
            benchClient.StatusZoneChanged += BenchClient_StatusZoneChanged;
            benchClient.SynchroZoneChanged += BenchClient_SynchroZoneChanged;
            benchClient.TransmissionZoneChanged += BenchClient_TransmissionZoneChanged;
            benchClient.ReceptionZoneChanged += BenchClient_ReceptionZoneChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SendMessage(txtEditValue.Handle, EM_SETCUEBANNER, 0, "Enter value");
        }

        private void InitializeMappings()
        {
            var synchroZone = GlobalVal.Instance._VEP.SynchroZone;

            _propertySetters = new Dictionary<string, Action<int>>
            {
                { "S_00", value => synchroZone.FrontCameraAngle1 = value },
                { "S_01", value => synchroZone.FrontCameraAngle2 = value },
                { "S_02", value => synchroZone.FrontCameraAngle3 = value },
                { "S_03", value => synchroZone.RearRightRadarAngle = value },
                { "S_04", value => synchroZone.RearLeftRadarAngle = value },
                { "S_05", value => synchroZone.FrontRightRadarAngle = value },
                { "S_06", value => synchroZone.FrontLeftRadarAngle = value }
            };

            _propertyGetters = new Dictionary<string, Func<int>>
            {
                { "S_00", () => synchroZone.FrontCameraAngle1 },
                { "S_01", () => synchroZone.FrontCameraAngle2 },
                { "S_02", () => synchroZone.FrontCameraAngle3 },
                { "S_03", () => synchroZone.RearRightRadarAngle },
                { "S_04", () => synchroZone.RearLeftRadarAngle },
                { "S_05", () => synchroZone.FrontRightRadarAngle },
                { "S_06", () => synchroZone.FrontLeftRadarAngle }
            };
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
                BeginInvoke(new Action(() => UpdateSynchroValues(
                    e.FrontCameraAngle1,
                    e.FrontCameraAngle2,
                    e.FrontCameraAngle3,
                    e.RearRightRadarAngle,
                    e.RearLeftRadarAngle,
                    e.FrontRightRadarAngle,
                    e.FrontLeftRadarAngle)));
            }
            else
            {
                UpdateSynchroValues(
                    e.FrontCameraAngle1,
                    e.FrontCameraAngle2,
                    e.FrontCameraAngle3,
                    e.RearRightRadarAngle,
                    e.RearLeftRadarAngle,
                    e.FrontRightRadarAngle,
                    e.FrontLeftRadarAngle);
            }
        }

        private void BenchClient_TransmissionZoneChanged(object sender, VEPBenchTransmissionZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateTransmissionInfo(e.AddTzSize, e.ExchStatus, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode)));
            }
            else
            {
                UpdateTransmissionInfo(e.AddTzSize, e.ExchStatus, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode);
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

        public void UpdateSynchroValues(int frontCameraAngle1, int frontCameraAngle2, int frontCameraAngle3, int rearRightRadarAngle, int rearLeftRadarAngle, int frontRightRadarAngle, int frontLeftRadarAngle)
        {
            txtFrontCameraAngle1.Text = frontCameraAngle1.ToString();
            txtFrontCameraAngle2.Text = frontCameraAngle2.ToString();
            txtFrontCameraAngle3.Text = frontCameraAngle3.ToString();
            txtRearRightRadarAngle.Text = rearRightRadarAngle.ToString();
            txtRearLeftRadarAngle.Text = rearLeftRadarAngle.ToString();
            txtFrontRightRadarAngle.Text = frontRightRadarAngle.ToString();
            txtFrontLeftRadarAngle.Text = frontLeftRadarAngle.ToString();
        }

        public void UpdateTransmissionInfo(ushort size, ushort exchStatus, byte fctCode, byte pcNum, byte processCode, byte subFctCode)
        {
            txtAddrTzSize.Text = size.ToString();
            txtTzExchStatus.Text = exchStatus.ToString();
            txtTzFctCode.Text = fctCode.ToString();
            txtTzPCNum.Text = pcNum.ToString();
            txtTzProcessCode.Text = processCode.ToString();
            txtTzSubFctCode.Text = subFctCode.ToString();
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

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = cmbValueList.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(selectedItem))
                {
                    MessageBox.Show("Select the item you want to modify.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtEditValue.Text, out int valueToSet))
                {
                    MessageBox.Show("Enter a valid integer.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_propertySetters.TryGetValue(selectedItem, out var propertySetter) &&
                    _propertyGetters.TryGetValue(selectedItem, out var propertyGetter))
                {
                    var originalValue = propertyGetter();

                    try
                    {
                        propertySetter(valueToSet);
                        benchClient.WriteSynchroZone();
                    }
                    finally
                    {
                        propertySetter(originalValue);
                    }

                    MessageBox.Show("Value modified successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Unknown item.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error modifying value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditStartCycle_Click(object sender, EventArgs e)
        {
            try
            {
                var statusZone = GlobalVal.Instance._VEP.StatusZone;
                var originalValue = statusZone.StartCycle;

                try
                {
                    statusZone.StartCycle = 1;
                    benchClient.WriteStatusZone();
                }
                finally
                {
                    statusZone.StartCycle = originalValue;
                }

                MessageBox.Show("set the Start Cycle value to 1 successfully .", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting Start Cycle value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditTExchStatus_Click(object sender, EventArgs e)
        {
            try
            {
                var transmissionZone = GlobalVal.Instance._VEP.TransmissionZone;
                var originalValue = transmissionZone.ExchStatus;

                try
                {
                    transmissionZone.ExchStatus = VEPBenchTransmissionZone.ExchStatus_Response;
                    benchClient.WriteTransmissionZone();
                }
                finally
                {
                    transmissionZone.ExchStatus = originalValue;
                }

                MessageBox.Show("Transmission Zone set ExchStatus value to 1.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting Transmission ZoneExchStatus value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditRExchStatus_Click(object sender, EventArgs e)
        {
            try
            {
                var receptionZone = GlobalVal.Instance._VEP.ReceptionZone;
                var originalValue = receptionZone.ExchStatus;

                try
                {
                    receptionZone.ExchStatus = VEPBenchReceptionZone.ExchStatus_Response;
                    benchClient.WriteReceptionZone();
                }
                finally
                {
                    receptionZone.ExchStatus = originalValue;
                }

                MessageBox.Show("Reception Zone set ExchStatus value to 1.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting ReceiptZoneExchStatus value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
