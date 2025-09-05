namespace Ki_ADAS
{
    partial class Frm_Mainfrm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Mainfrm));
            this.panelNavBar = new System.Windows.Forms.Panel();
            this.BtnVEP = new System.Windows.Forms.Button();
            this.BtnResult = new System.Windows.Forms.Button();
            this.BtnManual = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.BtnParameter = new System.Windows.Forms.Button();
            this.BtnIo = new System.Windows.Forms.Button();
            this.BtnConfig = new System.Windows.Forms.Button();
            this.BtnCalibration = new System.Windows.Forms.Button();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.BtnMain = new System.Windows.Forms.Button();
            this.panelNavBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelNavBar
            // 
            this.panelNavBar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelNavBar.Controls.Add(this.BtnVEP);
            this.panelNavBar.Controls.Add(this.BtnResult);
            this.panelNavBar.Controls.Add(this.BtnManual);
            this.panelNavBar.Controls.Add(this.pictureBox1);
            this.panelNavBar.Controls.Add(this.picLogo);
            this.panelNavBar.Controls.Add(this.BtnParameter);
            this.panelNavBar.Controls.Add(this.BtnIo);
            this.panelNavBar.Controls.Add(this.BtnConfig);
            this.panelNavBar.Controls.Add(this.BtnCalibration);
            this.panelNavBar.Controls.Add(this.pbLogo);
            this.panelNavBar.Controls.Add(this.BtnMain);
            this.panelNavBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelNavBar.Location = new System.Drawing.Point(0, 0);
            this.panelNavBar.Margin = new System.Windows.Forms.Padding(5);
            this.panelNavBar.Name = "panelNavBar";
            this.panelNavBar.Size = new System.Drawing.Size(104, 1264);
            this.panelNavBar.TabIndex = 8;
            // 
            // BtnVEP
            // 
            this.BtnVEP.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnVEP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnVEP.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnVEP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnVEP.Location = new System.Drawing.Point(4, 544);
            this.BtnVEP.Margin = new System.Windows.Forms.Padding(5);
            this.BtnVEP.Name = "BtnVEP";
            this.BtnVEP.Size = new System.Drawing.Size(95, 75);
            this.BtnVEP.TabIndex = 553;
            this.BtnVEP.Tag = "frmSetting";
            this.BtnVEP.Text = "VEP";
            this.BtnVEP.UseVisualStyleBackColor = false;
            this.BtnVEP.Click += new System.EventHandler(this.BtnVEP_Click);
            // 
            // BtnResult
            // 
            this.BtnResult.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnResult.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnResult.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnResult.Location = new System.Drawing.Point(5, 459);
            this.BtnResult.Margin = new System.Windows.Forms.Padding(5);
            this.BtnResult.Name = "BtnResult";
            this.BtnResult.Size = new System.Drawing.Size(95, 75);
            this.BtnResult.TabIndex = 552;
            this.BtnResult.Tag = "frmSetting";
            this.BtnResult.Text = "Result";
            this.BtnResult.UseVisualStyleBackColor = false;
            this.BtnResult.Click += new System.EventHandler(this.BtnResult_Click);
            // 
            // BtnManual
            // 
            this.BtnManual.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnManual.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnManual.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnManual.Location = new System.Drawing.Point(6, 374);
            this.BtnManual.Margin = new System.Windows.Forms.Padding(5);
            this.BtnManual.Name = "BtnManual";
            this.BtnManual.Size = new System.Drawing.Size(95, 75);
            this.BtnManual.TabIndex = 551;
            this.BtnManual.Tag = "frmManual";
            this.BtnManual.Text = "Manual";
            this.BtnManual.UseVisualStyleBackColor = false;
            this.BtnManual.Click += new System.EventHandler(this.BtnManual_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(14, 1146);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(75, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 550;
            this.pictureBox1.TabStop = false;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("picLogo.InitialImage")));
            this.picLogo.Location = new System.Drawing.Point(14, 15);
            this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(75, 80);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLogo.TabIndex = 549;
            this.picLogo.TabStop = false;
            // 
            // BtnParameter
            // 
            this.BtnParameter.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnParameter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnParameter.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnParameter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnParameter.Location = new System.Drawing.Point(6, 204);
            this.BtnParameter.Margin = new System.Windows.Forms.Padding(5);
            this.BtnParameter.Name = "BtnParameter";
            this.BtnParameter.Size = new System.Drawing.Size(95, 75);
            this.BtnParameter.TabIndex = 95;
            this.BtnParameter.Tag = "frmParameter";
            this.BtnParameter.Text = "Param";
            this.BtnParameter.UseVisualStyleBackColor = false;
            // 
            // BtnIo
            // 
            this.BtnIo.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnIo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnIo.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnIo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnIo.Location = new System.Drawing.Point(5, 629);
            this.BtnIo.Margin = new System.Windows.Forms.Padding(5);
            this.BtnIo.Name = "BtnIo";
            this.BtnIo.Size = new System.Drawing.Size(95, 75);
            this.BtnIo.TabIndex = 94;
            this.BtnIo.Tag = "frmIo";
            this.BtnIo.Text = "Digital I/O";
            this.BtnIo.UseVisualStyleBackColor = false;
            // 
            // BtnConfig
            // 
            this.BtnConfig.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnConfig.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnConfig.Location = new System.Drawing.Point(5, 714);
            this.BtnConfig.Margin = new System.Windows.Forms.Padding(5);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(95, 75);
            this.BtnConfig.TabIndex = 7;
            this.BtnConfig.Tag = "frmSetting";
            this.BtnConfig.Text = "Config";
            this.BtnConfig.UseVisualStyleBackColor = false;
            this.BtnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // BtnCalibration
            // 
            this.BtnCalibration.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnCalibration.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnCalibration.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnCalibration.Location = new System.Drawing.Point(5, 289);
            this.BtnCalibration.Margin = new System.Windows.Forms.Padding(5);
            this.BtnCalibration.Name = "BtnCalibration";
            this.BtnCalibration.Size = new System.Drawing.Size(95, 75);
            this.BtnCalibration.TabIndex = 4;
            this.BtnCalibration.Tag = "frmCalibration";
            this.BtnCalibration.Text = "Calibration";
            this.BtnCalibration.UseVisualStyleBackColor = false;
            this.BtnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // pbLogo
            // 
            this.pbLogo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbLogo.Location = new System.Drawing.Point(0, 1218);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(104, 46);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLogo.TabIndex = 24;
            this.pbLogo.TabStop = false;
            // 
            // BtnMain
            // 
            this.BtnMain.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnMain.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnMain.Font = new System.Drawing.Font("Verdana", 12F);
            this.BtnMain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnMain.Location = new System.Drawing.Point(6, 119);
            this.BtnMain.Margin = new System.Windows.Forms.Padding(5);
            this.BtnMain.Name = "BtnMain";
            this.BtnMain.Size = new System.Drawing.Size(95, 75);
            this.BtnMain.TabIndex = 2;
            this.BtnMain.Tag = "frmMain";
            this.BtnMain.Text = "Main";
            this.BtnMain.UseVisualStyleBackColor = false;
            this.BtnMain.Click += new System.EventHandler(this.BtnMain_Click);
            // 
            // Frm_Mainfrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2176, 1264);
            this.Controls.Add(this.panelNavBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_Mainfrm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Frm_Mainfrm_Load);
            this.panelNavBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelNavBar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button BtnParameter;
        private System.Windows.Forms.Button BtnIo;
        private System.Windows.Forms.Button BtnConfig;
        private System.Windows.Forms.Button BtnCalibration;
        public System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Button BtnMain;
        private System.Windows.Forms.Button BtnManual;
        private System.Windows.Forms.Button BtnResult;
        private System.Windows.Forms.Button BtnVEP;
    }
}

