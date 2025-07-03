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
    public partial class Frm_Calibration : Form
    {
        private Frm_Mainfrm m_frmParent = null;

        RoundButton btnFrontAdvance = new RoundButton
        {
            Text = "Advance",
            Location = new Point(1260, 180)
        };

        RoundButton btnFrontReturn = new RoundButton
        {
            Text = "Return",
            Location = new Point(1530, 180)
        };

        RoundButton btnRearAdvance = new RoundButton
        {
            Text = "Advance",
            Location = new Point(1260, 650)
        };

        RoundButton btnRearReturn = new RoundButton
        {
            Text = "Return",
            Location = new Point(1530, 650)
        };

        public Frm_Calibration()
        {
            InitializeComponent();

            this.Controls.Add(btnFrontAdvance);
            this.Controls.Add(btnFrontReturn);
            this.Controls.Add(btnRearAdvance);
            this.Controls.Add(btnRearReturn);
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }
    }
}
