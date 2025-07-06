using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KI_Controls
{
	internal class CGuage : Control
	{
		private float currentValue = 0;

		private float sweepAngle = 230f;

		public float maxValue { get; set; } = 160f;
		public float minValue { get; set; } = 0;
		public float TickInterval { get; set; } = 10f;

		public Color BgColor { get; set; } = Color.CornflowerBlue;
		public Color RegionColor { get; set; } = Color.LightGreen;
		public Color NiddleColor { get; set; } = Color.Magenta;

		

		public CGuage()
		{
			this.DoubleBuffered = true;
			this.ResizeRedraw = true;

		}
		public void SetValue(int nSpeed)
		{
			if (nSpeed < 0) return;
			if(nSpeed > maxValue) return;
			this.currentValue = nSpeed;	
			this.Invalidate();
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint(e);
			var g = e.Graphics;
			DrawGauge(g);
		}

		private void DrawGauge(Graphics g)
		{
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			int centerX = this.Width / 2;
			int centerY = (int)(this.Height * 0.65); // 컨트롤 높이에 따라 위치 자동 조정

			int radius = (int)(Math.Min(this.Width, this.Height) * 0.53); // 컨트롤 크기 기준 반지름 설정
			float startAngle = 155f;

			Rectangle rect = new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);

			Pen basePen = new Pen(this.BgColor, radius * 0.08f);
				g.DrawArc(basePen, rect, startAngle, sweepAngle);

			float safeStartVal = Math.Max(minValue, currentValue - 10);
			float safeEndVal = Math.Min(maxValue, currentValue + 10);

			float safeStartAngle = sweepAngle * (safeStartVal - minValue) / (maxValue - minValue);
			float safeSweep = sweepAngle * (safeEndVal - safeStartVal) / (maxValue - minValue);

			using (Pen safePen = new Pen(this.RegionColor, radius * 0.08f))
				g.DrawArc(safePen, rect, startAngle + safeStartAngle, safeSweep);

			int gaugeThickness = (int)(radius * 0.08f);

			float baseLineRadius = radius - basePen.Width / 2;

			for (float i = minValue; i <= maxValue; i += TickInterval)
			{
				float angle = startAngle + sweepAngle * (i - minValue) / (maxValue - minValue);
				double rad = angle * Math.PI / 180;

				// 눈금 길이
				//float tickLength = radius * 0.08f;
				float tickLength = radius * 0.05f;

				float x1 = centerX + (float)(Math.Cos(rad) * (baseLineRadius - 0));
				float y1 = centerY + (float)(Math.Sin(rad) * (baseLineRadius - 0));
				float x2 = centerX + (float)(Math.Cos(rad) * (baseLineRadius + tickLength));
				float y2 = centerY + (float)(Math.Sin(rad) * (baseLineRadius + tickLength));
				g.DrawLine(Pens.Black, x1, y1, x2, y2);

				string label = ((int)i).ToString();
				using (Font tickFont = new Font(this.Font.FontFamily, radius * 0.06f))
				{
					SizeF textSize = g.MeasureString(label, tickFont);
					float xt = centerX + (float)(Math.Cos(rad) * (baseLineRadius - tickLength * 2.5f)) - textSize.Width / 2;
					float yt = centerY + (float)(Math.Sin(rad) * (baseLineRadius - tickLength * 2.5f)) - textSize.Height / 2;
					g.DrawString(label, tickFont, Brushes.Black, xt, yt);
				}
			}

			// Needle
			{
				float needleAngle = startAngle + sweepAngle * (currentValue - minValue) / (maxValue - minValue);
				double rad = needleAngle * Math.PI / 180;

				float needleLength = radius - 50;
				float needleWidth = radius * 0.03f;

				PointF p1 = new PointF(centerX, centerY);
				PointF p2 = new PointF(
					centerX + (float)(Math.Cos(rad) * needleLength),
					centerY + (float)(Math.Sin(rad) * needleLength)
				);

				double leftRad = rad + Math.PI / 2;
				double rightRad = rad - Math.PI / 2;

				PointF p3 = new PointF(
					centerX + (float)(Math.Cos(leftRad) * (needleWidth / 2)),
					centerY + (float)(Math.Sin(leftRad) * (needleWidth / 2))
				);
				PointF p4 = new PointF(
					centerX + (float)(Math.Cos(rightRad) * (needleWidth / 2)),
					centerY + (float)(Math.Sin(rightRad) * (needleWidth / 2))
				);

				PointF[] needleTriangle = new PointF[] { p2, p3, p4 };
				Brush niddleBrush = new SolidBrush(NiddleColor);

				g.FillPolygon(niddleBrush, needleTriangle);
				niddleBrush.Dispose();
			}

			// 값 출력
			string valueStr = ((int)currentValue).ToString();
			using (Font valueFont = new Font(this.Font.FontFamily, radius * 0.2f, FontStyle.Bold))
			{
				SizeF valueSize = g.MeasureString(valueStr, valueFont);
				g.DrawString(valueStr, valueFont, Brushes.Black,
					centerX - valueSize.Width / 2,
					centerY - radius / 2 - valueSize.Height / 2 + (radius * 0.8f));
			}

			basePen.Dispose();
		}
	}
}
