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
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pji = "123456789012";
            var benchResp = VEPBenchResponse.CreatePJIResponse(pji);

            ushort[] receptionZone = new ushort[32];

            receptionZone[2] = 123;   
            receptionZone[3] = 2;
            receptionZone[6] = 6;     
            receptionZone[7] = 1 << 8;
            receptionZone[8] = 0x01;

            Array.Copy(benchResp.Data, 0, receptionZone, 12, benchResp.Data.Length);

            ReceiveBenchData(receptionZone);
        }

        private void ReceiveBenchData(ushort[] data)
        {
            // 1. Reception Zone Description 값 추출 및 바인딩
            txtAddRZSize.Text = data[2].ToString();
            txtExchStatus.Text = data[3].ToString();
            txtFctCode.Text = (data[6] & 0xFF).ToString();
            txtPCNum.Text = ((data[7] >> 8) & 0xFF).ToString();
            txtProcessCode.Text = (data[8] & 0xFF).ToString();
            txtSubFctCode.Text = ((data[8] >> 8) & 0xFF).ToString();

            // 2. PJI 플래그/길이 추출 
            int flag = data[12] & 0xFF;
            int length = (data[12] >> 8) & 0xFF;

            txtFlag.Text = flag.ToString();
            txtLen.Text = length.ToString();

            // 3. PJI 문자열 추출 (Data[13]부터 PJI 문자)
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                int wordIdx = 13 + (i / 2);
                bool isLow = (i % 2 == 0);
                chars[i] = (char)(isLow ? (data[wordIdx] & 0xFF) : (data[wordIdx] >> 8));
            }

            string pjiValue = new string(chars);

            txtPJI.Text = pjiValue;
        }

        private void btnWriteSynchro_Click(object sender, EventArgs e)
        {
            var sync = new VEPBenchSynchro();
            sync.Angle1 = double.Parse(txtAngle1.Text);
            sync.Angle2 = double.Parse(txtAngle2.Text);
            sync.Angle3 = double.Parse(txtAngle3.Text);

            benchClient.WriteSynchroZone(sync);
        }

        private void btnReadSynchro_Click(object sender, EventArgs e)
        {
            var sync = benchClient.ReadSynchroZone();
            txtAngle1.Text = sync.Angle1.ToString("F2");
            txtAngle2.Text = sync.Angle2.ToString("F2");
            txtAngle3.Text = sync.Angle3.ToString("F2");
        }
    }
}
