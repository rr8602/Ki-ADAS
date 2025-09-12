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
    public partial class Frm_Manual : Form
    {
        private Frm_Mainfrm m_frmParent = null;

        public Frm_Manual()
        {
            InitializeComponent();
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void Frm_Manual_Load(object sender, EventArgs e)
        {
            try
            {
                int btnWidth = 250;
                int btnHeiight = 250;

                ArrowButton btnUp = new ArrowButton
                {
                    Direction = ArrowButton.ArrowDirection.Up,
                    Text = "UP",
                    Width = btnWidth,
                    Height = btnHeiight,
                    Location = new Point(200, 150)
                };

                ArrowButton btnDown = new ArrowButton
                {
                    Direction = ArrowButton.ArrowDirection.Down,
                    Text = "DO\nWN",
                    Width = btnWidth,
                    Height = btnHeiight,
                    Location = new Point(200, 420)
                };

                ArrowButton btnRearLeftFw = new ArrowButton
                {
                    Direction = ArrowButton.ArrowDirection.Left,
                    Text = "FW",
                    Width = btnWidth,
                    Height = btnHeiight,
                    Location = new Point(670, 150)
                };

                ArrowButton btnRearLeftRw = new ArrowButton
                {
                    Direction = ArrowButton.ArrowDirection.Right,
                    Text = "RW",
                    Width = btnWidth,
                    Height = btnHeiight,
                    Location = new Point(930, 150)
                };

                ArrowButton btnRearRightFw = new ArrowButton
                {
                    Direction = ArrowButton.ArrowDirection.Left,
                    Text = "FW",
                    Width = btnWidth,
                    Height = btnHeiight,
                    Location = new Point(670, 660)
                };

                ArrowButton btnRearRightRw = new ArrowButton
                {
                    Direction = ArrowButton.ArrowDirection.Right,
                    Text = "RW",
                    Width = btnWidth,
                    Height = btnHeiight,
                    Location = new Point(930, 660)
                };

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
                    Location = new Point(1260, 660)
                };

                RoundButton btnRearReturn = new RoundButton
                {
                    Text = "Return",
                    Location = new Point(1530, 660)
                };

                this.Controls.Add(btnUp);
                this.Controls.Add(btnDown);

                this.Controls.Add(btnRearLeftFw);
                this.Controls.Add(btnRearLeftRw);

                this.Controls.Add(btnRearRightFw);
                this.Controls.Add(btnRearRightRw);

                this.Controls.Add(btnFrontAdvance);
                this.Controls.Add(btnFrontReturn);

                this.Controls.Add(btnRearAdvance);
                this.Controls.Add(btnRearReturn);
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorLoadingManualForm", "Error", ex.Message);
            }
        }
    }
}
