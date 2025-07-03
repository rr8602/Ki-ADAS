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
    public class ArrowButton : Button
    {
        public enum ArrowDirection
        {
            Up,
            Down,
            Left,
            Right
        }

        public ArrowDirection Direction { get; set; } = ArrowDirection.Up;

        public ArrowButton()
        {
            this.Font = new Font("굴림", 24, FontStyle.Bold);
            this.ForeColor = Color.Black;
            this.BackColor = Color.LightSkyBlue;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetArrowShape();
        }

        private void SetArrowShape()
        {
            GraphicsPath path = new GraphicsPath();

            int w = this.Width;
            int h = this.Height;
            int arrowPart;

            if (Direction == ArrowDirection.Left || Direction == ArrowDirection.Right)
            {
                arrowPart = w / 3; // 가로 방향 화살표
            }
            else
            {
                arrowPart = h / 3; // 세로 방향 화살표
            }

            if (Direction == ArrowDirection.Up)
            {
                path.AddPolygon(new Point[]
                {
                    new Point(w / 2, 0),             // 위 꼭짓점
                    new Point(0, arrowPart),   // 좌측
                    new Point(w / 3, arrowPart),
                    new Point(w / 3, h),
                    new Point(2 * w / 3, h),
                    new Point(2 * w / 3, arrowPart),
                    new Point(w, arrowPart)    // 우측
                });
            }
            else if (Direction == ArrowDirection.Down)
            {
                path.AddPolygon(new Point[]
                {
                    new Point(w / 3, 0),
                    new Point(w / 3, h - arrowPart),
                    new Point(0, h - arrowPart),
                    new Point(w / 2, h),             // 아래 꼭짓점
                    new Point(w, h - arrowPart),
                    new Point(2 * w / 3, h - arrowPart),
                    new Point(2 * w / 3, 0)
                });
            }
            else if (Direction == ArrowDirection.Left)
            {
                path.AddPolygon(new Point[]
                {
                    new Point(0, h / 2),
                    new Point(arrowPart, 0),
                    new Point(arrowPart, h / 3),
                    new Point(w, h / 3),
                    new Point(w, 2 * h / 3),
                    new Point(arrowPart, 2 * h / 3),
                    new Point(arrowPart, h)
                });
            }
            else if (Direction == ArrowDirection.Right)
            {
                path.AddPolygon(new Point[]
                {
                    new Point(w, h / 2),
                    new Point(w - arrowPart, 0),
                    new Point(w - arrowPart, h / 3),
                    new Point(0, h / 3),
                    new Point(0, 2 * h / 3),
                    new Point(w - arrowPart, 2 * h / 3),
                    new Point(w - arrowPart, h)
                });
            }

            this.Region = new Region(path);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Brush b = new SolidBrush(this.BackColor))
            {
                pevent.Graphics.FillRegion(b, this.Region);
            }

            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, this.ClientRectangle,
                this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
