using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KI_Controls
{
	internal class LineProgress : Control
	{
		public int Minimum { get; set; } = 0;
		public int Maximum { get; set; } = 10;
		public int Value { get; set; } = 0;
		public Color BarColor { get; set; } = Color.SeaGreen;
		public Color TickColor { get; set; } = Color.DarkGray;
		public Color TextColor { get; set; } = Color.Black;
		public Font TickFont { get; set; } = new Font("Arial", 8);


		public LineProgress()
		{
			this.DoubleBuffered = true;
			this.ResizeRedraw = true;
			this.Height = 40;  // 높이 확보
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			var g = e.Graphics;
			g.Clear(this.BackColor);

			int range = Maximum - Minimum;
			float percent = (float)(Value - Minimum) / range;
			int fillWidth = (int)(this.Width * percent);

			// 바 채우기
			using (var brush = new SolidBrush(BarColor))
			{
				g.FillRectangle(brush, 0, 0, fillWidth, 20);
			}

			// 테두리
			using (var pen = new Pen(Color.Black))
			{
				g.DrawRectangle(pen, 0, 0, this.Width - 1, 20);
			}

			// 눈금 및 숫자
			using (var tickPen = new Pen(TickColor))
			using (var textBrush = new SolidBrush(TextColor))
			{
				for (int i = 0; i <= range; i++)
				{
					float t = (float)i / range;
					int x = (int)(t * this.Width);

					g.DrawLine(tickPen, x, 0, x, 20); // 눈금
					//string label = (Minimum + i).ToString(); // 카운트 숫자
					//SizeF textSize = g.MeasureString(label, TickFont);
					//g.DrawString(label, TickFont, textBrush, x - textSize.Width / 2, 22);
				}
			}
		}
	}
}
