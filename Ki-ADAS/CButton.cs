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
	internal class CButton : Button
	{
		private Color originalBackColor;
		private bool isPressed = false;
		private bool isToggled;
		public bool IsToggled
		{
			get => isToggled;
			set
			{
				isToggled = value;
				UpdateAppearance();
			}
		}

		public bool bNormalButton { get; set; } = false;
		public Color ToggleBackColor { get; set; } = Color.CornflowerBlue;
		public Color NormalBackColor { get; set; } = Color.LightGray;
		public Color ToggleForeColor { get; set; } = Color.Black;
		public Color NormalForeColor { get; set; } = Color.Black;



		public uint BorderSize { get; set; } = 1;
		public Color BorderColor { get; set; } = Color.Black;
		public bool GetCheck() { return isToggled;  }
		public void SetCheck(bool bToggle) 
		{ 
			isToggled = bToggle; 
			if (bToggle) ToggleForeColor = Color.White;
			else ToggleForeColor = Color.Black;

			UpdateAppearance();
		}


		public CButton()
		{
			this.FlatStyle = FlatStyle.Flat;
			this.FlatAppearance.BorderSize = (int)BorderSize;
			this.FlatAppearance.BorderColor = BorderColor;

			this.BackColor = NormalBackColor;
			this.ForeColor = NormalForeColor;
			this.Click += (s, e) => Toggle();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint |
			  ControlStyles.UserPaint |
			  ControlStyles.OptimizedDoubleBuffer |
			  ControlStyles.ResizeRedraw |
			  ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
		}

		public void Toggle()
		{
			IsToggled = !IsToggled;
		}

		private void UpdateAppearance()
		{
			if (isToggled)
			{
				this.BackColor = ToggleBackColor;
				this.ForeColor = ToggleForeColor;

			}
			else
			{
				if (bNormalButton)
				{
					this.BackColor = ToggleBackColor;
					this.ForeColor = ToggleForeColor;

				}
				else
				{
					this.BackColor = NormalBackColor;
					this.ForeColor = NormalForeColor;

				}
			}
			originalBackColor = this.BackColor;
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			//base.OnPaint(pevent);
			// 배경, 테두리 등은 그대로 그리되
			base.OnPaintBackground(pevent);

			using (Pen pen = new Pen(this.FlatAppearance.BorderColor, this.FlatAppearance.BorderSize))
			{
				pevent.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
			}

			Rectangle textRect = this.ClientRectangle;
			if (isPressed)
				textRect.Offset(0, 2);

			TextRenderer.DrawText(
				pevent.Graphics,
				this.Text,
				this.Font,
				textRect,
				this.ForeColor,
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
			);
		}
		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			base.OnMouseDown(mevent);
			isPressed = true;
			this.Invalidate();  // 다시 그리기 요청
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
			isPressed = false;
			this.Invalidate();
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			
			if (bNormalButton)
			{
				originalBackColor = this.BackColor;
				this.BackColor = LightenColor(this.BackColor, 0.2f); // 20% 밝게
			}
			else
			{
				originalBackColor = this.BackColor;
				this.BackColor = LightenColor(this.ToggleBackColor, 0.2f); // 20% 밝게

			}
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			
			if (bNormalButton)
			{
				this.BackColor = originalBackColor;
			}
			else
			{
				this.BackColor = originalBackColor;
			}

			base.OnMouseLeave(e);
		}

		/// <summary>
		/// 지정된 색을 밝게 만듭니다.
		/// </summary>
		private Color LightenColor(Color color, float amount)
		{
			int r = (int)(color.R + (255 - color.R) * amount);
			int g = (int)(color.G + (255 - color.G) * amount);
			int b = (int)(color.B + (255 - color.B) * amount);
			return Color.FromArgb(r, g, b);
		}



		//protected override void OnResize(EventArgs e)
		//{
		//	base.OnResize(e);
		//	SetRoundedRegion();
		//}

		//private void SetRoundedRegion()
		//{
		//	int radius = 15; // 원하는 둥근 정도
		//	GraphicsPath path = new GraphicsPath();
		//	path.AddArc(0, 0, radius, radius, 180, 90);
		//	path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
		//	path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
		//	path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
		//	path.CloseFigure();
		//	this.Region = new Region(path);
		//}
		//protected override CreateParams CreateParams
		//{
		//	get
		//	{
		//		CreateParams cp = base.CreateParams;
		//		cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
		//		return cp;
		//	}
		//}

		//protected override void OnPaintBackground(PaintEventArgs pevent)
		//{
		//	// 완전히 무시하면 깜빡임이 생길 수 있으므로, 부모를 그려줌
		//	if (this.Parent != null)
		//	{
		//		Graphics g = pevent.Graphics;
		//		Rectangle rect = this.Bounds;
		//		g.TranslateTransform(-this.Left, -this.Top);
		//		PaintEventArgs pea = new PaintEventArgs(g, rect);
		//		InvokePaintBackground(this.Parent, pea);
		//		InvokePaint(this.Parent, pea);
		//		g.TranslateTransform(this.Left, this.Top);
		//	}
		//}
	}
}
