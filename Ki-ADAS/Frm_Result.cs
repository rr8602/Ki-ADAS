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
    public partial class Frm_Result : Form
    {
        private Frm_Mainfrm m_frmParent = null;

        private string _barcode;
        private DateTime _meaDate;
        private double _frontCameraAngle1;
        private double _frontCameraAngle2;
        private double _frontCameraAngle3;
        private double _rearRightRadarAngle;
        private double _rearLeftRadarAngle;

        public Frm_Result(
        string barcode,
        DateTime meaDate,
        double frontCameraAngle1,
        double frontCameraAngle2,
        double frontCameraAngle3,
        double rearRightRadarAngle,
        double rearLeftRadarAngle)
        {
            InitializeComponent();

            _barcode = barcode;
            _meaDate = meaDate;
            _frontCameraAngle1 = frontCameraAngle1;
            _frontCameraAngle2 = frontCameraAngle2;
            _frontCameraAngle3 = frontCameraAngle3;
            _rearRightRadarAngle = rearRightRadarAngle;
            _rearLeftRadarAngle = rearLeftRadarAngle;

            lblBarcode.Text = _barcode;
            lblRoll.Text = _frontCameraAngle1.ToString("F2");
            lblAzimuth.Text = _frontCameraAngle2.ToString("F2");
            lblElevation.Text = _frontCameraAngle3.ToString("F2");
            lblRearRightRadarAngle.Text = _rearRightRadarAngle.ToString("F2");
            lblRearLeftRadarAngle.Text = _rearLeftRadarAngle.ToString("F2");
        }


        public Frm_Result()
        {
            InitializeComponent();
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }
    }
}
