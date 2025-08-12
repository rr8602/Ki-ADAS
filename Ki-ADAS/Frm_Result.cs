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
    public partial class Frm_Result : Form
    {
        private Frm_Mainfrm m_frmParent = null;

        private string _pji;
        private DateTime _meaDate;
        private double _frontCameraAngle1;
        private double _frontCameraAngle2;
        private double _frontCameraAngle3;
        private double _rearRightRadarAngle;
        private double _rearLeftRadarAngle;

        public Frm_Result(
            string pji,
            DateTime meaDate,
            double frontCameraAngle1,
            double frontCameraAngle2,
            double frontCameraAngle3,
            double rearRightRadarAngle,
            double rearLeftRadarAngle)
        {
            InitializeComponent();

            _pji = pji;
            _meaDate = meaDate;
            _frontCameraAngle1 = frontCameraAngle1;
            _frontCameraAngle2 = frontCameraAngle2;
            _frontCameraAngle3 = frontCameraAngle3;
            _rearRightRadarAngle = rearRightRadarAngle;
            _rearLeftRadarAngle = rearLeftRadarAngle;

            lblBarcode.Text = _pji;
            lblRoll.Text = _frontCameraAngle1.ToString("F2");
            lblAzimuth.Text = _frontCameraAngle2.ToString("F2");
            lblElevation.Text = _frontCameraAngle3.ToString("F2");
            lblRearRightRadarAngle.Text = _rearRightRadarAngle.ToString("F2");
            lblRearLeftRadarAngle.Text = _rearLeftRadarAngle.ToString("F2");

            dateTimePicker1.Value = DateTime.Now;
        }

        private void LoadPjiListFromXml()
        {
            try
            {
                string dateString = DateTime.Now.ToString("yyyyMMdd");
                string xmlFileName = $"test_result_{dateString}.xml";
                string xmlFilePath = Path.Combine(Application.StartupPath, xmlFileName);

                seqList.Items.Clear();

                if (File.Exists(xmlFileName))
                {
                    XElement root = XElement.Load(xmlFilePath);
                    var results = root.Elements("TestResults");

                    foreach (var result in results)
                    {
                        string barcode = result.Element("Barcode")?.Value;
                        
                        if (!string.IsNullOrEmpty(barcode))
                        {
                            seqList.Items.Add(barcode);
                        }
                    }

                    if (seqList.Items.Count > 0)
                    {
                        seqList.Items[0].Selected = true;
                        seqList.Focus();
                        MessageBox.Show($"총 {seqList.Items.Count}개의 테스트 결과를 불러왔습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"{xmlFileName} 파일이 존재하지 않습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"XML 파일을 불러오는 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Frm_Result()
        {
            InitializeComponent();
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void Frm_Result_Load(object sender, EventArgs e)
        {
            LoadPjiListFromXml();
        }
    }
}
