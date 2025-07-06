namespace Ki_ADAS
{
    partial class Frm_Operator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NavTop = new System.Windows.Forms.Panel();
            this.roundLabel3 = new KI_Controls.RoundLabel();
            this.roundLabel2 = new KI_Controls.RoundLabel();
            this.roundLabel1 = new KI_Controls.RoundLabel();
            this.analogClock1 = new Ki_WAT.AnalogClock();
            this.NavBottom = new System.Windows.Forms.Panel();
            this.roundLabel4 = new KI_Controls.RoundLabel();
            this.NavTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // NavTop
            // 
            this.NavTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NavTop.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavTop.Controls.Add(this.roundLabel3);
            this.NavTop.Controls.Add(this.roundLabel2);
            this.NavTop.Controls.Add(this.roundLabel1);
            this.NavTop.Controls.Add(this.analogClock1);
            this.NavTop.Location = new System.Drawing.Point(1, 2);
            this.NavTop.Margin = new System.Windows.Forms.Padding(4);
            this.NavTop.Name = "NavTop";
            this.NavTop.Size = new System.Drawing.Size(1920, 173);
            this.NavTop.TabIndex = 9;
            this.NavTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NavTop_MouseDown);
            this.NavTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NavTop_MouseMove);
            // 
            // roundLabel3
            // 
            this.roundLabel3.BackColor = System.Drawing.Color.Black;
            this.roundLabel3.BackgroundColor = System.Drawing.Color.Black;
            this.roundLabel3.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.roundLabel3.BorderRadius = 20;
            this.roundLabel3.BorderSize = 0;
            this.roundLabel3.FlatAppearance.BorderSize = 0;
            this.roundLabel3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundLabel3.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundLabel3.ForeColor = System.Drawing.Color.White;
            this.roundLabel3.Location = new System.Drawing.Point(1663, 28);
            this.roundLabel3.Name = "roundLabel3";
            this.roundLabel3.Size = new System.Drawing.Size(215, 114);
            this.roundLabel3.TabIndex = 8;
            this.roundLabel3.Text = "9999";
            this.roundLabel3.TextColor = System.Drawing.Color.White;
            this.roundLabel3.UseVisualStyleBackColor = false;
            // 
            // roundLabel2
            // 
            this.roundLabel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.roundLabel2.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.roundLabel2.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.roundLabel2.BorderRadius = 20;
            this.roundLabel2.BorderSize = 0;
            this.roundLabel2.FlatAppearance.BorderSize = 0;
            this.roundLabel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundLabel2.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.roundLabel2.ForeColor = System.Drawing.Color.White;
            this.roundLabel2.Location = new System.Drawing.Point(243, 88);
            this.roundLabel2.Name = "roundLabel2";
            this.roundLabel2.Size = new System.Drawing.Size(1396, 54);
            this.roundLabel2.TabIndex = 7;
            this.roundLabel2.Text = "-";
            this.roundLabel2.TextColor = System.Drawing.Color.White;
            this.roundLabel2.UseVisualStyleBackColor = false;
            // 
            // roundLabel1
            // 
            this.roundLabel1.BackColor = System.Drawing.Color.MidnightBlue;
            this.roundLabel1.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.roundLabel1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.roundLabel1.BorderRadius = 20;
            this.roundLabel1.BorderSize = 0;
            this.roundLabel1.FlatAppearance.BorderSize = 0;
            this.roundLabel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundLabel1.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.roundLabel1.ForeColor = System.Drawing.Color.White;
            this.roundLabel1.Location = new System.Drawing.Point(243, 28);
            this.roundLabel1.Name = "roundLabel1";
            this.roundLabel1.Size = new System.Drawing.Size(1396, 54);
            this.roundLabel1.TabIndex = 6;
            this.roundLabel1.Text = "-";
            this.roundLabel1.TextColor = System.Drawing.Color.White;
            this.roundLabel1.UseVisualStyleBackColor = false;
            // 
            // analogClock1
            // 
            this.analogClock1.BackColor = System.Drawing.Color.White;
            this.analogClock1.BorderColor = System.Drawing.Color.LightCoral;
            this.analogClock1.BorderThickness = 3;
            this.analogClock1.HourHandColor = System.Drawing.Color.Black;
            this.analogClock1.Location = new System.Drawing.Point(23, 19);
            this.analogClock1.MinuteHandColor = System.Drawing.Color.Black;
            this.analogClock1.Name = "analogClock1";
            this.analogClock1.NumberFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analogClock1.SecondHandColor = System.Drawing.Color.Red;
            this.analogClock1.Size = new System.Drawing.Size(187, 114);
            this.analogClock1.TabIndex = 5;
            this.analogClock1.Text = "analogClock1";
            // 
            // NavBottom
            // 
            this.NavBottom.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NavBottom.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavBottom.Location = new System.Drawing.Point(-347, 1627);
            this.NavBottom.Margin = new System.Windows.Forms.Padding(4);
            this.NavBottom.Name = "NavBottom";
            this.NavBottom.Size = new System.Drawing.Size(1920, 8);
            this.NavBottom.TabIndex = 10;
            // 
            // roundLabel4
            // 
            this.roundLabel4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.roundLabel4.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.roundLabel4.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.roundLabel4.BorderRadius = 20;
            this.roundLabel4.BorderSize = 0;
            this.roundLabel4.FlatAppearance.BorderSize = 0;
            this.roundLabel4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundLabel4.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.roundLabel4.ForeColor = System.Drawing.Color.White;
            this.roundLabel4.Location = new System.Drawing.Point(143, 204);
            this.roundLabel4.Name = "roundLabel4";
            this.roundLabel4.Size = new System.Drawing.Size(1626, 659);
            this.roundLabel4.TabIndex = 11;
            this.roundLabel4.Text = "-";
            this.roundLabel4.TextColor = System.Drawing.Color.White;
            this.roundLabel4.UseVisualStyleBackColor = false;
            // 
            // Frm_Operator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.roundLabel4);
            this.Controls.Add(this.NavTop);
            this.Controls.Add(this.NavBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Frm_Operator";
            this.Text = "Frm_Operator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Operator_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Operator_Load);
            this.NavTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel NavTop;
        private System.Windows.Forms.Panel NavBottom;
        private KI_Controls.RoundLabel roundLabel3;
        private KI_Controls.RoundLabel roundLabel2;
        private KI_Controls.RoundLabel roundLabel1;
        private Ki_WAT.AnalogClock analogClock1;
        private KI_Controls.RoundLabel roundLabel4;
    }
}