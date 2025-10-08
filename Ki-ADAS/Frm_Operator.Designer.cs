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
            this.rlbl_time = new KI_Controls.RoundLabel();
            this.rlbl_modelName = new KI_Controls.RoundLabel();
            this.analogClock1 = new Ki_WAT.AnalogClock();
            this.NavBottom = new System.Windows.Forms.Panel();
            this.lbl_Toe_FL = new System.Windows.Forms.Label();
            this.lbl_Toe_FR = new System.Windows.Forms.Label();
            this.lbl_Toe_RR = new System.Windows.Forms.Label();
            this.lbl_frcam = new System.Windows.Forms.Label();
            this.lbl_frada = new System.Windows.Forms.Label();
            this.lbl_rrada = new System.Windows.Forms.Label();
            this.lbl_message = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_FRight = new System.Windows.Forms.Label();
            this.lbl_FLeft = new System.Windows.Forms.Label();
            this.lbl_RRight = new System.Windows.Forms.Label();
            this.lbl_RLeft = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lbl_elevation = new System.Windows.Forms.Label();
            this.lbl_azimuth = new System.Windows.Forms.Label();
            this.lbl_roll = new System.Windows.Forms.Label();
            this.NavTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // NavTop
            // 
            this.NavTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NavTop.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavTop.Controls.Add(this.rlbl_time);
            this.NavTop.Controls.Add(this.rlbl_modelName);
            this.NavTop.Controls.Add(this.analogClock1);
            this.NavTop.Location = new System.Drawing.Point(1, 2);
            this.NavTop.Margin = new System.Windows.Forms.Padding(5);
            this.NavTop.Name = "NavTop";
            this.NavTop.Size = new System.Drawing.Size(2714, 247);
            this.NavTop.TabIndex = 9;
            this.NavTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NavTop_MouseDown);
            this.NavTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NavTop_MouseMove);
            // 
            // rlbl_time
            // 
            this.rlbl_time.BackColor = System.Drawing.Color.Black;
            this.rlbl_time.BackgroundColor = System.Drawing.Color.Black;
            this.rlbl_time.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rlbl_time.BorderRadius = 20;
            this.rlbl_time.BorderSize = 0;
            this.rlbl_time.FlatAppearance.BorderSize = 0;
            this.rlbl_time.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rlbl_time.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_time.ForeColor = System.Drawing.Color.White;
            this.rlbl_time.Location = new System.Drawing.Point(1901, 35);
            this.rlbl_time.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rlbl_time.Name = "rlbl_time";
            this.rlbl_time.Size = new System.Drawing.Size(246, 142);
            this.rlbl_time.TabIndex = 8;
            this.rlbl_time.Text = "9999";
            this.rlbl_time.TextColor = System.Drawing.Color.White;
            this.rlbl_time.UseVisualStyleBackColor = false;
            // 
            // rlbl_modelName
            // 
            this.rlbl_modelName.BackColor = System.Drawing.Color.MidnightBlue;
            this.rlbl_modelName.BackgroundColor = System.Drawing.Color.MidnightBlue;
            this.rlbl_modelName.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rlbl_modelName.BorderRadius = 20;
            this.rlbl_modelName.BorderSize = 0;
            this.rlbl_modelName.FlatAppearance.BorderSize = 0;
            this.rlbl_modelName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rlbl_modelName.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rlbl_modelName.ForeColor = System.Drawing.Color.White;
            this.rlbl_modelName.Location = new System.Drawing.Point(277, 35);
            this.rlbl_modelName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rlbl_modelName.Name = "rlbl_modelName";
            this.rlbl_modelName.Size = new System.Drawing.Size(1595, 142);
            this.rlbl_modelName.TabIndex = 6;
            this.rlbl_modelName.Text = "Model Name";
            this.rlbl_modelName.TextColor = System.Drawing.Color.White;
            this.rlbl_modelName.UseVisualStyleBackColor = false;
            // 
            // analogClock1
            // 
            this.analogClock1.BackColor = System.Drawing.Color.White;
            this.analogClock1.BorderColor = System.Drawing.Color.LightCoral;
            this.analogClock1.BorderThickness = 3;
            this.analogClock1.HourHandColor = System.Drawing.Color.Black;
            this.analogClock1.Location = new System.Drawing.Point(26, 24);
            this.analogClock1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.analogClock1.MinuteHandColor = System.Drawing.Color.Black;
            this.analogClock1.Name = "analogClock1";
            this.analogClock1.NumberFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analogClock1.SecondHandColor = System.Drawing.Color.Red;
            this.analogClock1.Size = new System.Drawing.Size(214, 142);
            this.analogClock1.TabIndex = 5;
            this.analogClock1.Text = "analogClock1";
            // 
            // NavBottom
            // 
            this.NavBottom.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NavBottom.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavBottom.Location = new System.Drawing.Point(-839, 2225);
            this.NavBottom.Margin = new System.Windows.Forms.Padding(5);
            this.NavBottom.Name = "NavBottom";
            this.NavBottom.Size = new System.Drawing.Size(2194, 10);
            this.NavBottom.TabIndex = 10;
            // 
            // lbl_Toe_FL
            // 
            this.lbl_Toe_FL.BackColor = System.Drawing.Color.Silver;
            this.lbl_Toe_FL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Toe_FL.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Toe_FL.Location = new System.Drawing.Point(27, 254);
            this.lbl_Toe_FL.Name = "lbl_Toe_FL";
            this.lbl_Toe_FL.Size = new System.Drawing.Size(590, 171);
            this.lbl_Toe_FL.TabIndex = 93;
            this.lbl_Toe_FL.Text = "FRCAM";
            this.lbl_Toe_FL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Toe_FR
            // 
            this.lbl_Toe_FR.BackColor = System.Drawing.Color.Silver;
            this.lbl_Toe_FR.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Toe_FR.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Toe_FR.Location = new System.Drawing.Point(27, 425);
            this.lbl_Toe_FR.Name = "lbl_Toe_FR";
            this.lbl_Toe_FR.Size = new System.Drawing.Size(590, 171);
            this.lbl_Toe_FR.TabIndex = 94;
            this.lbl_Toe_FR.Text = "F-RADAR";
            this.lbl_Toe_FR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Toe_RR
            // 
            this.lbl_Toe_RR.BackColor = System.Drawing.Color.Silver;
            this.lbl_Toe_RR.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_Toe_RR.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Toe_RR.Location = new System.Drawing.Point(27, 596);
            this.lbl_Toe_RR.Name = "lbl_Toe_RR";
            this.lbl_Toe_RR.Size = new System.Drawing.Size(590, 171);
            this.lbl_Toe_RR.TabIndex = 95;
            this.lbl_Toe_RR.Text = "R-RADAR";
            this.lbl_Toe_RR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_frcam
            // 
            this.lbl_frcam.BackColor = System.Drawing.Color.White;
            this.lbl_frcam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_frcam.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_frcam.Location = new System.Drawing.Point(617, 254);
            this.lbl_frcam.Name = "lbl_frcam";
            this.lbl_frcam.Size = new System.Drawing.Size(750, 171);
            this.lbl_frcam.TabIndex = 96;
            this.lbl_frcam.Text = "-";
            this.lbl_frcam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_frada
            // 
            this.lbl_frada.BackColor = System.Drawing.Color.White;
            this.lbl_frada.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_frada.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_frada.Location = new System.Drawing.Point(617, 425);
            this.lbl_frada.Name = "lbl_frada";
            this.lbl_frada.Size = new System.Drawing.Size(750, 171);
            this.lbl_frada.TabIndex = 97;
            this.lbl_frada.Text = "-";
            this.lbl_frada.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_rrada
            // 
            this.lbl_rrada.BackColor = System.Drawing.Color.White;
            this.lbl_rrada.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_rrada.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_rrada.Location = new System.Drawing.Point(617, 596);
            this.lbl_rrada.Name = "lbl_rrada";
            this.lbl_rrada.Size = new System.Drawing.Size(750, 171);
            this.lbl_rrada.TabIndex = 98;
            this.lbl_rrada.Text = "-";
            this.lbl_rrada.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_message
            // 
            this.lbl_message.BackColor = System.Drawing.Color.White;
            this.lbl_message.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_message.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_message.Location = new System.Drawing.Point(19, 851);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(2153, 237);
            this.lbl_message.TabIndex = 99;
            this.lbl_message.Text = "-";
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(29, 779);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(341, 72);
            this.label7.TabIndex = 100;
            this.label7.Text = "Message";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1370, 426);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(399, 84);
            this.label8.TabIndex = 101;
            this.label8.Text = "LEFT";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(1773, 426);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(399, 84);
            this.label10.TabIndex = 103;
            this.label10.Text = "RIGHT";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FRight
            // 
            this.lbl_FRight.BackColor = System.Drawing.Color.Transparent;
            this.lbl_FRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_FRight.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FRight.Location = new System.Drawing.Point(1773, 512);
            this.lbl_FRight.Name = "lbl_FRight";
            this.lbl_FRight.Size = new System.Drawing.Size(399, 84);
            this.lbl_FRight.TabIndex = 105;
            this.lbl_FRight.Text = "0.0";
            this.lbl_FRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_FLeft
            // 
            this.lbl_FLeft.BackColor = System.Drawing.Color.Transparent;
            this.lbl_FLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_FLeft.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FLeft.Location = new System.Drawing.Point(1370, 512);
            this.lbl_FLeft.Name = "lbl_FLeft";
            this.lbl_FLeft.Size = new System.Drawing.Size(399, 84);
            this.lbl_FLeft.TabIndex = 104;
            this.lbl_FLeft.Text = "0.0";
            this.lbl_FLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RRight
            // 
            this.lbl_RRight.BackColor = System.Drawing.Color.Transparent;
            this.lbl_RRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_RRight.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RRight.Location = new System.Drawing.Point(1773, 682);
            this.lbl_RRight.Name = "lbl_RRight";
            this.lbl_RRight.Size = new System.Drawing.Size(399, 84);
            this.lbl_RRight.TabIndex = 109;
            this.lbl_RRight.Text = "0.0";
            this.lbl_RRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_RLeft
            // 
            this.lbl_RLeft.BackColor = System.Drawing.Color.Transparent;
            this.lbl_RLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_RLeft.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RLeft.Location = new System.Drawing.Point(1370, 682);
            this.lbl_RLeft.Name = "lbl_RLeft";
            this.lbl_RLeft.Size = new System.Drawing.Size(397, 85);
            this.lbl_RLeft.TabIndex = 108;
            this.lbl_RLeft.Text = "0.0";
            this.lbl_RLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label14.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(1773, 596);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(399, 84);
            this.label14.TabIndex = 107;
            this.label14.Text = "RIGHT";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label15.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(1370, 596);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(399, 84);
            this.label15.TabIndex = 106;
            this.label15.Text = "LEFT";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label16.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(1370, 254);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(265, 85);
            this.label16.TabIndex = 110;
            this.label16.Text = "Roll";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label17.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(1638, 254);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(265, 85);
            this.label17.TabIndex = 111;
            this.label17.Text = "Azimute";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label18.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(1905, 254);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(265, 85);
            this.label18.TabIndex = 112;
            this.label18.Text = "Elevation";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_elevation
            // 
            this.lbl_elevation.BackColor = System.Drawing.Color.Transparent;
            this.lbl_elevation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_elevation.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_elevation.Location = new System.Drawing.Point(1905, 339);
            this.lbl_elevation.Name = "lbl_elevation";
            this.lbl_elevation.Size = new System.Drawing.Size(265, 85);
            this.lbl_elevation.TabIndex = 115;
            this.lbl_elevation.Text = "0.0";
            this.lbl_elevation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_azimuth
            // 
            this.lbl_azimuth.BackColor = System.Drawing.Color.Transparent;
            this.lbl_azimuth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_azimuth.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_azimuth.Location = new System.Drawing.Point(1638, 339);
            this.lbl_azimuth.Name = "lbl_azimuth";
            this.lbl_azimuth.Size = new System.Drawing.Size(265, 85);
            this.lbl_azimuth.TabIndex = 114;
            this.lbl_azimuth.Text = "0.0";
            this.lbl_azimuth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_roll
            // 
            this.lbl_roll.BackColor = System.Drawing.Color.Transparent;
            this.lbl_roll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_roll.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_roll.Location = new System.Drawing.Point(1370, 339);
            this.lbl_roll.Name = "lbl_roll";
            this.lbl_roll.Size = new System.Drawing.Size(265, 85);
            this.lbl_roll.TabIndex = 113;
            this.lbl_roll.Text = "0.0";
            this.lbl_roll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Frm_Operator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2194, 1102);
            this.Controls.Add(this.lbl_elevation);
            this.Controls.Add(this.lbl_azimuth);
            this.Controls.Add(this.lbl_roll);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.lbl_RRight);
            this.Controls.Add(this.lbl_RLeft);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lbl_FRight);
            this.Controls.Add(this.lbl_FLeft);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbl_message);
            this.Controls.Add(this.lbl_rrada);
            this.Controls.Add(this.lbl_frada);
            this.Controls.Add(this.lbl_frcam);
            this.Controls.Add(this.lbl_Toe_RR);
            this.Controls.Add(this.lbl_Toe_FR);
            this.Controls.Add(this.lbl_Toe_FL);
            this.Controls.Add(this.NavTop);
            this.Controls.Add(this.NavBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Frm_Operator";
            this.Text = "Frm_Operator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Operator_FormClosing);
            this.NavTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel NavTop;
        private System.Windows.Forms.Panel NavBottom;
        private KI_Controls.RoundLabel rlbl_time;
        private KI_Controls.RoundLabel rlbl_modelName;
        private Ki_WAT.AnalogClock analogClock1;
        private System.Windows.Forms.Label lbl_Toe_FL;
        private System.Windows.Forms.Label lbl_Toe_FR;
        private System.Windows.Forms.Label lbl_Toe_RR;
        private System.Windows.Forms.Label lbl_frcam;
        private System.Windows.Forms.Label lbl_frada;
        private System.Windows.Forms.Label lbl_rrada;
        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbl_FRight;
        private System.Windows.Forms.Label lbl_FLeft;
        private System.Windows.Forms.Label lbl_RRight;
        private System.Windows.Forms.Label lbl_RLeft;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lbl_elevation;
        private System.Windows.Forms.Label lbl_azimuth;
        private System.Windows.Forms.Label lbl_roll;
    }
}