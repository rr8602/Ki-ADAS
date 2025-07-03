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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Operator));
            this.NavTop = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.NavBottom = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lvProcessLog = new System.Windows.Forms.ListView();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NavTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NavTop
            // 
            this.NavTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NavTop.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavTop.Controls.Add(this.label9);
            this.NavTop.Controls.Add(this.picLogo);
            this.NavTop.Location = new System.Drawing.Point(1, 2);
            this.NavTop.Margin = new System.Windows.Forms.Padding(5);
            this.NavTop.Name = "NavTop";
            this.NavTop.Size = new System.Drawing.Size(2996, 389);
            this.NavTop.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.MidnightBlue;
            this.label9.Font = new System.Drawing.Font("Arial", 30F);
            this.label9.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(128, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(1554, 66);
            this.label9.TabIndex = 550;
            this.label9.Text = "-";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // NavBottom
            // 
            this.NavBottom.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.NavBottom.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NavBottom.Location = new System.Drawing.Point(-643, 1568);
            this.NavBottom.Margin = new System.Windows.Forms.Padding(5);
            this.NavBottom.Name = "NavBottom";
            this.NavBottom.Size = new System.Drawing.Size(2194, 10);
            this.NavBottom.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lvProcessLog);
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Location = new System.Drawing.Point(27, 239);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1656, 634);
            this.panel1.TabIndex = 11;
            // 
            // lvProcessLog
            // 
            this.lvProcessLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvProcessLog.FullRowSelect = true;
            this.lvProcessLog.GridLines = true;
            this.lvProcessLog.HideSelection = false;
            this.lvProcessLog.Location = new System.Drawing.Point(-2, 463);
            this.lvProcessLog.Name = "lvProcessLog";
            this.lvProcessLog.Size = new System.Drawing.Size(1656, 167);
            this.lvProcessLog.TabIndex = 2;
            this.lvProcessLog.UseCompatibleStateImageBehavior = false;
            this.lvProcessLog.View = System.Windows.Forms.View.Details;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(-2, -2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1656, 130);
            this.progressBar.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(0, -2);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1654, 632);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "ADAS 프로세스 대기 중";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "시간";
            this.columnHeader1.Width = 270;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "유형";
            this.columnHeader2.Width = 270;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "단계 ID";
            this.columnHeader3.Width = 270;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "설명";
            this.columnHeader4.Width = 270;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "동기화 상태";
            this.columnHeader5.Width = 270;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "결과";
            this.columnHeader6.Width = 270;
            // 
            // Frm_Operator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1700, 884);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.NavTop);
            this.Controls.Add(this.NavBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_Operator";
            this.Text = "Frm_Operator";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Operator_FormClosing);
            this.NavTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel NavTop;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Panel NavBottom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListView lvProcessLog;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}