using System.Drawing;
using System.Windows.Forms;

namespace Ki_ADAS
{
    public class ThickGroupBox : GroupBox
    {
        private Color _borderColor = Color.RoyalBlue;
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; this.Invalidate(); }
        }

        private int _borderWidth = 2;
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; this.Invalidate(); }
        }

        public ThickGroupBox()
        {
            this.DoubleBuffered = true;
            this.TitleBackColor = Color.Transparent;
            this.TitleForeColor = Color.Empty;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SizeF stringSize = e.Graphics.MeasureString(this.Text, this.Font);
            Rectangle textRect = new Rectangle(this.Padding.Left + 8, 0, (int)stringSize.Width, (int)stringSize.Height);

            using (var pen = new Pen(BorderColor, BorderWidth))
            {
                int halfTextHeight = textRect.Height / 2;

                Rectangle borderRect = new Rectangle(
                    ClientRectangle.X,
                    ClientRectangle.Y + halfTextHeight,
                    ClientRectangle.Width - 1,
                    ClientRectangle.Height - halfTextHeight - 1);

                // Top-Left
                e.Graphics.DrawLine(pen, borderRect.X, borderRect.Y, borderRect.X + textRect.X - 4, borderRect.Y);
                // Top-Right
                e.Graphics.DrawLine(pen, borderRect.X + textRect.Right, borderRect.Y, borderRect.Right, borderRect.Y);
                // Right
                e.Graphics.DrawLine(pen, borderRect.Right, borderRect.Y, borderRect.Right, borderRect.Bottom);
                // Bottom
                e.Graphics.DrawLine(pen, borderRect.Right, borderRect.Bottom, borderRect.Left, borderRect.Bottom);
                // Left
                e.Graphics.DrawLine(pen, borderRect.Left, borderRect.Bottom, borderRect.Left, borderRect.Y);
            }

            using (var brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(brush, textRect);
            }
            using (var brush = new SolidBrush(this.ForeColor))
            {
                e.Graphics.DrawString(this.Text, this.Font, brush, textRect.Location);
            }
        }

        [System.ComponentModel.Browsable(false)]
        public Color TitleBackColor { get; set; }

        [System.ComponentModel.Browsable(false)]
        public Color TitleForeColor { get; set; }
    }
}
