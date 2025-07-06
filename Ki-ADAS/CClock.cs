using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Ki_WAT
{
    public class AnalogClock : Control
    {
        private Timer timer;

        // 외형 설정
        public Color BorderColor { get; set; } = Color.Red;
        public int BorderThickness { get; set; } = 4;
        public Font NumberFont { get; set; } = new Font("Arial", 10);
        public Color HourHandColor { get; set; } = Color.Black;
        public Color MinuteHandColor { get; set; } = Color.Black;
        public Color SecondHandColor { get; set; } = Color.Red;

        public AnalogClock()
        {
            // 깜빡임 방지
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = Color.White;

            // 1초마다 갱신
            timer = new Timer { Interval = 1000 };
            timer.Tick += (s, e) => this.Invalidate();
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int w = this.Width;
            int h = this.Height;
            int radius = Math.Min(w, h) / 2 - BorderThickness;
            Point center = new Point(w / 2, h / 2);

            // ▶ 테두리 원
            using (var pen = new Pen(BorderColor, BorderThickness))
            {
                e.Graphics.DrawEllipse(pen,
                    center.X - radius,
                    center.Y - radius,
                    radius * 2,
                    radius * 2);
            }

            // ▶ 숫자 배치 (1~12)
            for (int i = 1; i <= 12; i++)
            {
                double angle = (i * 30 - 90) * Math.PI / 180; // 시계 방향
                float x = center.X + (float)Math.Cos(angle) * (radius - 20);
                float y = center.Y + (float)Math.Sin(angle) * (radius - 20);
                string num = i.ToString();
                SizeF size = e.Graphics.MeasureString(num, NumberFont);
                e.Graphics.DrawString(
                    num,
                    NumberFont,
                    Brushes.Black,
                    x - size.Width / 2,
                    y - size.Height / 2);
            }

            DateTime now = DateTime.Now;

            // ▶ 시침
            DrawHand(e.Graphics, center,
                     angleDegree: (now.Hour % 12 + now.Minute / 60.0) * 30 - 90,
                     length: radius * 0.5f,
                     thickness: 6,
                     color: HourHandColor);

            // ▶ 분침
            DrawHand(e.Graphics, center,
                     angleDegree: (now.Minute + now.Second / 60.0) * 6 - 90,
                     length: radius * 0.7f,
                     thickness: 4,
                     color: MinuteHandColor);

            // ▶ 초침
            DrawHand(e.Graphics, center,
                     angleDegree: now.Second * 6 - 90,
                     length: radius * 0.8f,
                     thickness: 2,
                     color: SecondHandColor);
        }

        private void DrawHand(Graphics g, Point center, double angleDegree, float length, float thickness, Color color)
        {
            double rad = angleDegree * Math.PI / 180;
            PointF end = new PointF(
                center.X + (float)Math.Cos(rad) * length,
                center.Y + (float)Math.Sin(rad) * length
            );
            using (var pen = new Pen(color, thickness) { EndCap = LineCap.Round })
            {
                g.DrawLine(pen, center, end);
            }
        }
    }
}