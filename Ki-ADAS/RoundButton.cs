using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public class RoundButton : Button
    {
        public RoundButton()
        {
            this.Font = new Font("굴림", 24, FontStyle.Bold);
            this.Width = 200;
            this.Height = 200;
            this.BackColor = Color.LightSkyBlue;
            this.ForeColor = Color.Black;
            this.FlatStyle = FlatStyle.Flat;
            this.Size = new Size(250, 250);
            this.FlatAppearance.BorderSize = 0;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, Width, Height);
            this.Region = new System.Drawing.Region(path);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Brush b = new SolidBrush(this.BackColor))
            {
                pevent.Graphics.FillEllipse(b, 0, 0, this.Width - 1, this.Height - 1);
            }

            using (Pen p = new Pen(Color.FromArgb(0, 0, 0), 1))
            {
                pevent.Graphics.DrawEllipse(p, 0, 0, this.Width - 1, this.Height - 1);
            }

            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font,
                this.ClientRectangle, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
