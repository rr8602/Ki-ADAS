using Ki_ADAS.DB;
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
using System.Xml.Linq;

namespace Ki_ADAS
{
    public partial class Frm_Result : Form
    {
        private Frm_Mainfrm m_frmParent = null;
        private ResultRepository _resultRepository;
        private VEPBenchDataManager _vepManager = GlobalVal.Instance._VEP;

        public Frm_Result(SettingConfigDb db)
        {
            InitializeComponent();

            _resultRepository = new ResultRepository(db);

            SetAngleInfo();
        }

        private void LoadInfoList()
        {
            try
            {
                seqList.Items.Clear();

                var results = _resultRepository.GetResultInfo();

                if (results == null || results.Count == 0)
                {
                    return;
                }

                foreach (var result in results)
                {
                    var item = new ListViewItem(result.AcceptNo);
                    item.SubItems.Add(result.PJI);
                    seqList.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the XML file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void seqList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (seqList.SelectedItems.Count > 0)
                {
                    SetAngleInfo();
                }
                else
                {
                    MessageBox.Show("Select a test result to view details.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while displaying test result details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void SetAngleInfo()
        {
            lblRoll.Text = _vepManager.SynchroZone.FrontCameraAngle1.ToString();
            lblAzimuth.Text = _vepManager.SynchroZone.FrontCameraAngle2.ToString();
            lblElevation.Text = _vepManager.SynchroZone.FrontCameraAngle3.ToString();
            lblRearRightRadarAngle.Text = _vepManager.SynchroZone.RearRightRadarAngle.ToString();
            lblRearLeftRadarAngle.Text = _vepManager.SynchroZone.RearLeftRadarAngle.ToString();
            lblFrontRightRadarAngle.Text = _vepManager.SynchroZone.FrontRightRadarAngle.ToString();
            lblFrontLeftRadarAngle.Text = _vepManager.SynchroZone.FrontLeftRadarAngle.ToString();
        }

        private void Frm_Result_Load(object sender, EventArgs e)
        {
            this.seqList.OwnerDraw = true;
            this.seqList.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.seqList_DrawColumnHeader);
            this.seqList.DrawSubItem += new DrawListViewSubItemEventHandler(this.seqList_DrawSubItem);

            LoadInfoList();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void btnDateSearch_Click(object sender, EventArgs e)
        {
            try
            {
                seqList.Items.Clear();

                var resultsByDate = _resultRepository.GetResultInfoByDate(dateTimePicker1.Value.ToString("yyyyMMdd"));
                int count = 0;

                if (resultsByDate == null || resultsByDate.Count == 0)
                {
                    MessageBox.Show($"No results found for {dateTimePicker1.Value:yyyy-MM-dd}.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var result in resultsByDate)
                {
                    var item = new ListViewItem(result.AcceptNo);
                    item.SubItems.Add(result.PJI);
                    seqList.Items.Add(item);
                    count++;
                }

                if (count > 0)
                {
                    seqList.Items[0].Selected = true;
                    seqList.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during date search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPJISearch_Click(object sender, EventArgs e)
        {
            try
            {
                seqList.Items.Clear();

                var resultsByPji = _resultRepository.GetResultInfoByPji(txtPji.Text.Trim());
                int count = 0;

                if (resultsByPji == null || resultsByPji.Count == 0)
                {
                    MessageBox.Show("No results found for the specified PJI.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (var result in resultsByPji)
                {
                    var item = new ListViewItem(result.AcceptNo);
                    item.SubItems.Add(result.PJI);
                    seqList.Items.Add(item);
                    count++;
                }

                if (count > 0)
                {
                    seqList.Items[0].Selected = true;
                    seqList.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during PJI search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
