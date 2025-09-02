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
    public partial class Register : Form
    {
        private XDocument _testResultsDoc;
        private string _xmlPath;
        private Dictionary<string, XDocument> _xmlFiles;

        public Register()
        {
            InitializeComponent();
            LoadBarcodes();
        }

        private void LoadBarcodes()
        {
            try
            {
                string xmlFilePath = Application.StartupPath;
                string[] xmlFiles = Directory.GetFiles(xmlFilePath, "test_result_*.xml");
                _xmlFiles = new Dictionary<string, XDocument>();

                _xmlPath = Path.Combine(Application.StartupPath, "test_result_register.xml");

                if (xmlFiles.Length == 0)
                {
                    _testResultsDoc = new XDocument(
                        new XElement("Root",
                            new XElement("TestResultsList")
                        )
                    );
                    _testResultsDoc.Save(_xmlPath);
                    _xmlFiles.Add(_xmlPath, _testResultsDoc);
                }
                else
                {
                    foreach (string file in xmlFiles)
                    {
                        try
                        {
                            XDocument doc = XDocument.Load(file);
                            _xmlFiles.Add(file, doc);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"파일 로드 오류: {file}\n{ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    _xmlPath = xmlFiles.OrderByDescending(f => new FileInfo(f).CreationTime).FirstOrDefault();
                    _testResultsDoc = _xmlFiles[_xmlPath];
                }

                RefreshBarcodeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"바코드 목록 로드 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshBarcodeList()
        {
            lvBarcodes.Items.Clear();
            var barcodeSet = new HashSet<string>();

            // 모든 XML 파일에서 바코드 추출
            foreach (var xmlFile in _xmlFiles)
            {
                var results = xmlFile.Value.Descendants("TestResults");

                foreach (var result in results)
                {
                    string barcode = result.Element("Barcode")?.Value;
                    string model = result.Element("Model")?.Value;

                    if (!string.IsNullOrEmpty(barcode) && !barcodeSet.Contains(barcode))
                    {
                        ListViewItem item = new ListViewItem(barcode);
                        item.SubItems.Add(model ?? "");
                        item.Tag = xmlFile.Key;
                        lvBarcodes.Items.Add(item);
                        barcodeSet.Add(barcode);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string barcode = txtBarcode.Text.Trim();
                string model = txtModel.Text.Trim();

                if (string.IsNullOrEmpty(barcode))
                {
                    MessageBox.Show("바코드를 입력해주세요.", "입력 확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 중복 검사
                bool isDuplicate = false;

                foreach (ListViewItem item in lvBarcodes.Items)
                {
                    if (item.Text == barcode)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (isDuplicate)
                {
                    MessageBox.Show("이미 등록된 바코드입니다.", "중복 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 새 바코드 추가
                var testResultsList = _testResultsDoc.Descendants("TestResultsList").FirstOrDefault();

                if (testResultsList == null)
                {
                    var root = _testResultsDoc.Root;

                    if (root == null)
                    {
                        root = new XElement("Root");
                        _testResultsDoc.Add(root);
                    }

                    testResultsList = new XElement("TestResultsList");
                    root.Add(testResultsList);
                }

                var testResult = new XElement("TestResults",
                    new XElement("Barcode", barcode),
                    new XElement("Model", model),
                    new XElement("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                );

                testResultsList.Add(testResult);
                _testResultsDoc.Save(_xmlPath);

                // 목록 갱신
                LoadBarcodes();

                txtBarcode.Text = "";
                txtModel.Text = "";
                MessageBox.Show("바코드가 성공적으로 등록되었습니다.", "등록 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"바코드 등록 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvBarcodes.SelectedItems.Count == 0)
                {
                    MessageBox.Show("삭제할 바코드를 선택해주세요.", "선택 확인", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string selectedBarcode = lvBarcodes.SelectedItems[0].Text;
                string filePath = lvBarcodes.SelectedItems[0].Tag?.ToString();

                if (MessageBox.Show($"바코드 '{selectedBarcode}'를 삭제하시겠습니까?", "삭제 확인",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool deleted = false;

                    // 선택된 항목의 파일 경로가 있는 경우
                    if (!string.IsNullOrEmpty(filePath) && _xmlFiles.ContainsKey(filePath))
                    {
                        XDocument doc = _xmlFiles[filePath];
                        var results = doc.Descendants("TestResults").ToList();

                        foreach (var result in results)
                        {
                            if (result.Element("Barcode")?.Value == selectedBarcode)
                            {
                                result.Remove();
                                doc.Save(filePath);
                                deleted = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 모든 XML 파일에서 검색하여 삭제
                        foreach (var xmlFile in _xmlFiles)
                        {
                            var results = xmlFile.Value.Descendants("TestResults").ToList();

                            foreach (var result in results)
                            {
                                if (result.Element("Barcode")?.Value == selectedBarcode)
                                {
                                    result.Remove();
                                    xmlFile.Value.Save(xmlFile.Key);
                                    deleted = true;
                                    break;
                                }
                            }

                            if (deleted) break;
                        }
                    }

                    if (deleted)
                    {
                        LoadBarcodes(); // 목록 갱신
                        MessageBox.Show("바코드가 성공적으로 삭제되었습니다.", "삭제 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("선택한 바코드를 XML 파일에서 찾을 수 없습니다.", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"바코드 삭제 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lvBarcodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvBarcodes.SelectedItems.Count > 0)
            {
                btnDelete.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
            }
        }
    }
}