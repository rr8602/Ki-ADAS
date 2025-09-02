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
            double frontCameraAngle1,
            double frontCameraAngle2,
            double frontCameraAngle3,
            double rearRightRadarAngle,
            double rearLeftRadarAngle)
        {
            InitializeComponent();

            _pji = pji;
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
        }

        private void LoadPjiListFromXml()
        {
            try
            {
                string xmlFilePath = Application.StartupPath;
                string[] xmlFiles = Directory.GetFiles(xmlFilePath, "test_result_*.xml");

                seqList.Items.Clear();
                int totalCount = 0;

                if (xmlFiles.Length > 0)
                {
                    Array.Sort(xmlFiles, (a, b) => string.Compare(b, a)); // 최신 파일이 먼저 오도록 정렬

                    foreach (string file in xmlFiles)
                    {
                        try
                        {
                            XElement root = XElement.Load(file);
                            var results = root.Elements("TestResults");

                            foreach (var result in results)
                            {
                                string barcode = result.Element("Barcode")?.Value;

                                if (!string.IsNullOrEmpty(barcode))
                                {
                                    seqList.Items.Add(barcode);
                                    totalCount++;
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (seqList.Items.Count > 0)
                    {
                        seqList.Items[0].Selected = true;
                        seqList.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("XML 파일이 존재하지 않습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"XML 파일 로드 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void seqList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (seqList.SelectedItems.Count > 0)
            {
                string selectedBarcode = seqList.SelectedItems[0].Text;
                DisplayTestResultDetails(selectedBarcode);
            }
        }

        private void DisplayTestResultDetails(string barcode)
        {
            try
            {
                string xmlFilePath = Application.StartupPath;
                string[] xmlFiles = Directory.GetFiles(xmlFilePath, "test_result_*.xml");

                foreach (string file in xmlFiles)
                {
                    try
                    {
                        XElement root = XElement.Load(file);
                        var results = root.Elements("TestResults");

                        foreach (var result in results)
                        {
                            string currentBarcode = result.Element("Barcode")?.Value;

                            if (currentBarcode == barcode)
                            {
                                lblBarcode.Text = currentBarcode;

                                double frontCameraAngle1 = double.Parse(result.Element("FrontCameraAngle1")?.Value ?? "0");
                                double frontCameraAngle2 = double.Parse(result.Element("FrontCameraAngle2")?.Value ?? "0");
                                double frontCameraAngle3 = double.Parse(result.Element("FrontCameraAngle3")?.Value ?? "0");
                                double rearRightRadarAngle = double.Parse(result.Element("RearRightRadarAngle")?.Value ?? "0");
                                double rearLeftRadarAngle = double.Parse(result.Element("RearLeftRadarAngle")?.Value ?? "0");

                                lblRoll.Text = frontCameraAngle1.ToString("F2");
                                lblAzimuth.Text = frontCameraAngle2.ToString("F2");
                                lblElevation.Text = frontCameraAngle3.ToString("F2");
                                lblRearRightRadarAngle.Text = rearRightRadarAngle.ToString("F2");
                                lblRearLeftRadarAngle.Text = rearLeftRadarAngle.ToString("F2");

                                return;
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                MessageBox.Show($"선택한 바코드({barcode})에 대한 상세 정보를 찾을 수 없습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"테스트 결과 상세정보 표시 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            dateTimePicker1.Value = DateTime.Now;
        }

        private void btnDateSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string xmlFilePath = Application.StartupPath;
                string[] xmlFiles = Directory.GetFiles(xmlFilePath, "test_result_*.xml");

                seqList.Items.Clear();
                int count = 0;

                if (xmlFiles.Length > 0)
                {
                    foreach (string file in xmlFiles)
                    {
                        try
                        {
                            XElement root = XElement.Load(file);
                            var results = root.Elements("TestResults");

                            foreach (var result in results)
                            {
                                string timestamp = result.Element("Timestamp")?.Value;
                                string barcode = result.Element("Barcode")?.Value;

                                if (!string.IsNullOrEmpty(timestamp) && timestamp.StartsWith(searchDate))
                                {
                                    seqList.Items.Add(barcode);
                                    count++;
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (count > 0)
                    {
                        seqList.Items[0].Selected = true;
                        seqList.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"{searchDate} 날짜에 대한 결과가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("XML 파일이 존재하지 않습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"날짜 검색 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPJISearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchPji = txtPji.Text.Trim();

                if (string.IsNullOrEmpty(searchPji))
                {
                    MessageBox.Show("검색할 PJI를 입력하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string xmlFilePath = Application.StartupPath;
                string[] xmlFiles = Directory.GetFiles(xmlFilePath, "test_result_*.xml");

                seqList.Items.Clear();
                int count = 0;

                if (xmlFiles.Length > 0)
                {
                    foreach (string file in xmlFiles)
                    {
                        try
                        {
                            XElement root = XElement.Load(file);
                            var results = root.Elements("TestResults");

                            foreach (var result in results)
                            {
                                string barcode = result.Element("Barcode")?.Value;

                                if (!string.IsNullOrEmpty(barcode) && barcode.Contains(searchPji))
                                {
                                    seqList.Items.Add(barcode);
                                    count++;
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (count > 0)
                    {
                        seqList.Items[0].Selected = true;
                        seqList.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"{searchPji}에 대한 결과가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("XML 파일이 존재하지 않습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PJI 검색 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
