namespace Simulator
{
    partial class Frm_CameraSimulator
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
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.grpAngles = new System.Windows.Forms.GroupBox();
            this.lblRollThreshold = new System.Windows.Forms.Label();
            this.lblAzimuthThreshold = new System.Windows.Forms.Label();
            this.lblElevationThreshold = new System.Windows.Forms.Label();
            this.lblRollAngle = new System.Windows.Forms.Label();
            this.lblAzimuthAngle = new System.Windows.Forms.Label();
            this.lblElevationAngle = new System.Windows.Forms.Label();
            this.tbRollAngle = new System.Windows.Forms.TextBox();
            this.tbAzimuthAngle = new System.Windows.Forms.TextBox();
            this.tbElevationAngle = new System.Windows.Forms.TextBox();
            this.btnApplyAngles = new System.Windows.Forms.Button();
            this.grpSimulation = new System.Windows.Forms.GroupBox();
            this.btnSimulateSuccess = new System.Windows.Forms.Button();
            this.btnSimulateFail = new System.Windows.Forms.Button();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpSynchro = new System.Windows.Forms.GroupBox();
            this.btnStartCalibration = new System.Windows.Forms.Button();
            this.lblSynchro3Title = new System.Windows.Forms.Label();
            this.lblSynchro4Title = new System.Windows.Forms.Label();
            this.lblSynchro89Title = new System.Windows.Forms.Label();
            this.lblSynchro110Title = new System.Windows.Forms.Label();
            this.lblSynchro111Title = new System.Windows.Forms.Label();
            this.lblSynchro112Title = new System.Windows.Forms.Label();
            this.lblSynchro3 = new System.Windows.Forms.Label();
            this.lblSynchro4 = new System.Windows.Forms.Label();
            this.lblSynchro89 = new System.Windows.Forms.Label();
            this.lblSynchro110 = new System.Windows.Forms.Label();
            this.lblSynchro111 = new System.Windows.Forms.Label();
            this.lblSynchro112 = new System.Windows.Forms.Label();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.grpConnection.SuspendLayout();
            this.grpAngles.SuspendLayout();
            this.grpSimulation.SuspendLayout();
            this.grpStatus.SuspendLayout();
            this.grpSynchro.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.btnDisconnect);
            this.grpConnection.Controls.Add(this.btnConnect);
            this.grpConnection.Location = new System.Drawing.Point(12, 12);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(300, 80);
            this.grpConnection.TabIndex = 0;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "VEP 연결";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(160, 25);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(120, 35);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "연결 해제";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.BtnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(20, 25);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(120, 35);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "연결";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // grpAngles
            // 
            this.grpAngles.Controls.Add(this.lblRollThreshold);
            this.grpAngles.Controls.Add(this.lblAzimuthThreshold);
            this.grpAngles.Controls.Add(this.lblElevationThreshold);
            this.grpAngles.Controls.Add(this.lblRollAngle);
            this.grpAngles.Controls.Add(this.lblAzimuthAngle);
            this.grpAngles.Controls.Add(this.lblElevationAngle);
            this.grpAngles.Controls.Add(this.tbRollAngle);
            this.grpAngles.Controls.Add(this.tbAzimuthAngle);
            this.grpAngles.Controls.Add(this.tbElevationAngle);
            this.grpAngles.Controls.Add(this.btnApplyAngles);
            this.grpAngles.Location = new System.Drawing.Point(12, 100);
            this.grpAngles.Name = "grpAngles";
            this.grpAngles.Size = new System.Drawing.Size(300, 200);
            this.grpAngles.TabIndex = 1;
            this.grpAngles.TabStop = false;
            this.grpAngles.Text = "카메라 각도 설정";
            // 
            // lblRollThreshold
            // 
            this.lblRollThreshold.AutoSize = true;
            this.lblRollThreshold.Font = new System.Drawing.Font("굴림", 8F);
            this.lblRollThreshold.Location = new System.Drawing.Point(160, 55);
            this.lblRollThreshold.Name = "lblRollThreshold";
            this.lblRollThreshold.Size = new System.Drawing.Size(118, 14);
            this.lblRollThreshold.TabIndex = 2;
            this.lblRollThreshold.Text = "(허용범위: ±1.0)";
            // 
            // lblAzimuthThreshold
            // 
            this.lblAzimuthThreshold.AutoSize = true;
            this.lblAzimuthThreshold.Font = new System.Drawing.Font("굴림", 8F);
            this.lblAzimuthThreshold.Location = new System.Drawing.Point(160, 100);
            this.lblAzimuthThreshold.Name = "lblAzimuthThreshold";
            this.lblAzimuthThreshold.Size = new System.Drawing.Size(118, 14);
            this.lblAzimuthThreshold.TabIndex = 5;
            this.lblAzimuthThreshold.Text = "(허용범위: ±1.0)";
            // 
            // lblElevationThreshold
            // 
            this.lblElevationThreshold.AutoSize = true;
            this.lblElevationThreshold.Font = new System.Drawing.Font("굴림", 8F);
            this.lblElevationThreshold.Location = new System.Drawing.Point(160, 145);
            this.lblElevationThreshold.Name = "lblElevationThreshold";
            this.lblElevationThreshold.Size = new System.Drawing.Size(118, 14);
            this.lblElevationThreshold.TabIndex = 8;
            this.lblElevationThreshold.Text = "(허용범위: ±1.0)";
            // 
            // lblRollAngle
            // 
            this.lblRollAngle.AutoSize = true;
            this.lblRollAngle.Location = new System.Drawing.Point(20, 30);
            this.lblRollAngle.Name = "lblRollAngle";
            this.lblRollAngle.Size = new System.Drawing.Size(67, 15);
            this.lblRollAngle.TabIndex = 0;
            this.lblRollAngle.Text = "Roll 각도";
            // 
            // lblAzimuthAngle
            // 
            this.lblAzimuthAngle.AutoSize = true;
            this.lblAzimuthAngle.Location = new System.Drawing.Point(20, 75);
            this.lblAzimuthAngle.Name = "lblAzimuthAngle";
            this.lblAzimuthAngle.Size = new System.Drawing.Size(93, 15);
            this.lblAzimuthAngle.TabIndex = 3;
            this.lblAzimuthAngle.Text = "Azimuth 각도";
            // 
            // lblElevationAngle
            // 
            this.lblElevationAngle.AutoSize = true;
            this.lblElevationAngle.Location = new System.Drawing.Point(20, 120);
            this.lblElevationAngle.Name = "lblElevationAngle";
            this.lblElevationAngle.Size = new System.Drawing.Size(102, 15);
            this.lblElevationAngle.TabIndex = 6;
            this.lblElevationAngle.Text = "Elevation 각도";
            // 
            // tbRollAngle
            // 
            this.tbRollAngle.Location = new System.Drawing.Point(160, 30);
            this.tbRollAngle.Name = "tbRollAngle";
            this.tbRollAngle.Size = new System.Drawing.Size(120, 25);
            this.tbRollAngle.TabIndex = 1;
            // 
            // tbAzimuthAngle
            // 
            this.tbAzimuthAngle.Location = new System.Drawing.Point(160, 75);
            this.tbAzimuthAngle.Name = "tbAzimuthAngle";
            this.tbAzimuthAngle.Size = new System.Drawing.Size(120, 25);
            this.tbAzimuthAngle.TabIndex = 4;
            // 
            // tbElevationAngle
            // 
            this.tbElevationAngle.Location = new System.Drawing.Point(160, 120);
            this.tbElevationAngle.Name = "tbElevationAngle";
            this.tbElevationAngle.Size = new System.Drawing.Size(120, 25);
            this.tbElevationAngle.TabIndex = 7;
            // 
            // btnApplyAngles
            // 
            this.btnApplyAngles.Location = new System.Drawing.Point(100, 160);
            this.btnApplyAngles.Name = "btnApplyAngles";
            this.btnApplyAngles.Size = new System.Drawing.Size(120, 30);
            this.btnApplyAngles.TabIndex = 9;
            this.btnApplyAngles.Text = "각도 적용";
            this.btnApplyAngles.UseVisualStyleBackColor = true;
            this.btnApplyAngles.Click += new System.EventHandler(this.BtnApplyAngles_Click);
            // 
            // grpSimulation
            // 
            this.grpSimulation.Controls.Add(this.btnSimulateSuccess);
            this.grpSimulation.Controls.Add(this.btnSimulateFail);
            this.grpSimulation.Location = new System.Drawing.Point(12, 310);
            this.grpSimulation.Name = "grpSimulation";
            this.grpSimulation.Size = new System.Drawing.Size(300, 100);
            this.grpSimulation.TabIndex = 2;
            this.grpSimulation.TabStop = false;
            this.grpSimulation.Text = "시뮬레이션";
            // 
            // btnSimulateSuccess
            // 
            this.btnSimulateSuccess.BackColor = System.Drawing.Color.LightGreen;
            this.btnSimulateSuccess.Location = new System.Drawing.Point(20, 30);
            this.btnSimulateSuccess.Name = "btnSimulateSuccess";
            this.btnSimulateSuccess.Size = new System.Drawing.Size(120, 50);
            this.btnSimulateSuccess.TabIndex = 0;
            this.btnSimulateSuccess.Text = "성공 시뮬레이션";
            this.btnSimulateSuccess.UseVisualStyleBackColor = false;
            this.btnSimulateSuccess.Click += new System.EventHandler(this.BtnSimulateSuccess_Click);
            // 
            // btnSimulateFail
            // 
            this.btnSimulateFail.BackColor = System.Drawing.Color.LightPink;
            this.btnSimulateFail.Location = new System.Drawing.Point(160, 30);
            this.btnSimulateFail.Name = "btnSimulateFail";
            this.btnSimulateFail.Size = new System.Drawing.Size(120, 50);
            this.btnSimulateFail.TabIndex = 1;
            this.btnSimulateFail.Text = "실패 시뮬레이션";
            this.btnSimulateFail.UseVisualStyleBackColor = false;
            this.btnSimulateFail.Click += new System.EventHandler(this.BtnSimulateFail_Click);
            // 
            // grpStatus
            // 
            this.grpStatus.Controls.Add(this.lblStatusTitle);
            this.grpStatus.Controls.Add(this.lblStatus);
            this.grpStatus.Location = new System.Drawing.Point(330, 12);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Size = new System.Drawing.Size(374, 80);
            this.grpStatus.TabIndex = 3;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "캘리브레이션 상태";
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.AutoSize = true;
            this.lblStatusTitle.Location = new System.Drawing.Point(20, 35);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(77, 15);
            this.lblStatusTitle.TabIndex = 0;
            this.lblStatusTitle.Text = "현재 상태:";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.LightGray;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(110, 25);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(258, 40);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "준비됨";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpSynchro
            // 
            this.grpSynchro.Controls.Add(this.btnStartCalibration);
            this.grpSynchro.Controls.Add(this.lblSynchro3Title);
            this.grpSynchro.Controls.Add(this.lblSynchro4Title);
            this.grpSynchro.Controls.Add(this.lblSynchro89Title);
            this.grpSynchro.Controls.Add(this.lblSynchro110Title);
            this.grpSynchro.Controls.Add(this.lblSynchro111Title);
            this.grpSynchro.Controls.Add(this.lblSynchro112Title);
            this.grpSynchro.Controls.Add(this.lblSynchro3);
            this.grpSynchro.Controls.Add(this.lblSynchro4);
            this.grpSynchro.Controls.Add(this.lblSynchro89);
            this.grpSynchro.Controls.Add(this.lblSynchro110);
            this.grpSynchro.Controls.Add(this.lblSynchro111);
            this.grpSynchro.Controls.Add(this.lblSynchro112);
            this.grpSynchro.Location = new System.Drawing.Point(330, 100);
            this.grpSynchro.Name = "grpSynchro";
            this.grpSynchro.Size = new System.Drawing.Size(380, 310);
            this.grpSynchro.TabIndex = 4;
            this.grpSynchro.TabStop = false;
            this.grpSynchro.Text = "Synchro 값 모니터링";
            // 
            // btnStartCalibration
            // 
            this.btnStartCalibration.Location = new System.Drawing.Point(23, 258);
            this.btnStartCalibration.Name = "btnStartCalibration";
            this.btnStartCalibration.Size = new System.Drawing.Size(351, 46);
            this.btnStartCalibration.TabIndex = 6;
            this.btnStartCalibration.Text = "시작";
            this.btnStartCalibration.UseVisualStyleBackColor = true;
            this.btnStartCalibration.Click += new System.EventHandler(this.btnStartCalibration_Click);
            // 
            // lblSynchro3Title
            // 
            this.lblSynchro3Title.AutoSize = true;
            this.lblSynchro3Title.Location = new System.Drawing.Point(20, 30);
            this.lblSynchro3Title.Name = "lblSynchro3Title";
            this.lblSynchro3Title.Size = new System.Drawing.Size(80, 15);
            this.lblSynchro3Title.TabIndex = 0;
            this.lblSynchro3Title.Text = "Synchro 3:";
            // 
            // lblSynchro4Title
            // 
            this.lblSynchro4Title.AutoSize = true;
            this.lblSynchro4Title.Location = new System.Drawing.Point(20, 70);
            this.lblSynchro4Title.Name = "lblSynchro4Title";
            this.lblSynchro4Title.Size = new System.Drawing.Size(80, 15);
            this.lblSynchro4Title.TabIndex = 2;
            this.lblSynchro4Title.Text = "Synchro 4:";
            // 
            // lblSynchro89Title
            // 
            this.lblSynchro89Title.AutoSize = true;
            this.lblSynchro89Title.Location = new System.Drawing.Point(20, 110);
            this.lblSynchro89Title.Name = "lblSynchro89Title";
            this.lblSynchro89Title.Size = new System.Drawing.Size(88, 15);
            this.lblSynchro89Title.TabIndex = 4;
            this.lblSynchro89Title.Text = "Synchro 89:";
            // 
            // lblSynchro110Title
            // 
            this.lblSynchro110Title.AutoSize = true;
            this.lblSynchro110Title.Location = new System.Drawing.Point(20, 150);
            this.lblSynchro110Title.Name = "lblSynchro110Title";
            this.lblSynchro110Title.Size = new System.Drawing.Size(96, 15);
            this.lblSynchro110Title.TabIndex = 6;
            this.lblSynchro110Title.Text = "Synchro 110:";
            // 
            // lblSynchro111Title
            // 
            this.lblSynchro111Title.AutoSize = true;
            this.lblSynchro111Title.Location = new System.Drawing.Point(20, 190);
            this.lblSynchro111Title.Name = "lblSynchro111Title";
            this.lblSynchro111Title.Size = new System.Drawing.Size(96, 15);
            this.lblSynchro111Title.TabIndex = 8;
            this.lblSynchro111Title.Text = "Synchro 111:";
            // 
            // lblSynchro112Title
            // 
            this.lblSynchro112Title.AutoSize = true;
            this.lblSynchro112Title.Location = new System.Drawing.Point(20, 230);
            this.lblSynchro112Title.Name = "lblSynchro112Title";
            this.lblSynchro112Title.Size = new System.Drawing.Size(96, 15);
            this.lblSynchro112Title.TabIndex = 10;
            this.lblSynchro112Title.Text = "Synchro 112:";
            // 
            // lblSynchro3
            // 
            this.lblSynchro3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSynchro3.Location = new System.Drawing.Point(150, 25);
            this.lblSynchro3.Name = "lblSynchro3";
            this.lblSynchro3.Size = new System.Drawing.Size(224, 30);
            this.lblSynchro3.TabIndex = 1;
            this.lblSynchro3.Text = "-";
            this.lblSynchro3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSynchro4
            // 
            this.lblSynchro4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSynchro4.Location = new System.Drawing.Point(150, 65);
            this.lblSynchro4.Name = "lblSynchro4";
            this.lblSynchro4.Size = new System.Drawing.Size(224, 30);
            this.lblSynchro4.TabIndex = 3;
            this.lblSynchro4.Text = "-";
            this.lblSynchro4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSynchro89
            // 
            this.lblSynchro89.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSynchro89.Location = new System.Drawing.Point(150, 105);
            this.lblSynchro89.Name = "lblSynchro89";
            this.lblSynchro89.Size = new System.Drawing.Size(224, 30);
            this.lblSynchro89.TabIndex = 5;
            this.lblSynchro89.Text = "-";
            this.lblSynchro89.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSynchro110
            // 
            this.lblSynchro110.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSynchro110.Location = new System.Drawing.Point(150, 145);
            this.lblSynchro110.Name = "lblSynchro110";
            this.lblSynchro110.Size = new System.Drawing.Size(224, 30);
            this.lblSynchro110.TabIndex = 7;
            this.lblSynchro110.Text = "-";
            this.lblSynchro110.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSynchro111
            // 
            this.lblSynchro111.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSynchro111.Location = new System.Drawing.Point(150, 185);
            this.lblSynchro111.Name = "lblSynchro111";
            this.lblSynchro111.Size = new System.Drawing.Size(224, 30);
            this.lblSynchro111.TabIndex = 9;
            this.lblSynchro111.Text = "-";
            this.lblSynchro111.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSynchro112
            // 
            this.lblSynchro112.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSynchro112.Location = new System.Drawing.Point(150, 225);
            this.lblSynchro112.Name = "lblSynchro112";
            this.lblSynchro112.Size = new System.Drawing.Size(224, 30);
            this.lblSynchro112.TabIndex = 11;
            this.lblSynchro112.Text = "-";
            this.lblSynchro112.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.lstLog);
            this.grpLog.Location = new System.Drawing.Point(12, 420);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(698, 200);
            this.grpLog.TabIndex = 5;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "로그";
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.ItemHeight = 15;
            this.lstLog.Location = new System.Drawing.Point(10, 25);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(682, 154);
            this.lstLog.TabIndex = 0;
            // 
            // Frm_CameraSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 632);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.grpSynchro);
            this.Controls.Add(this.grpStatus);
            this.Controls.Add(this.grpSimulation);
            this.Controls.Add(this.grpAngles);
            this.Controls.Add(this.grpConnection);
            this.Name = "Frm_CameraSimulator";
            this.Text = "전면 카메라 캘리브레이션 시뮬레이터";
            this.grpConnection.ResumeLayout(false);
            this.grpAngles.ResumeLayout(false);
            this.grpAngles.PerformLayout();
            this.grpSimulation.ResumeLayout(false);
            this.grpStatus.ResumeLayout(false);
            this.grpStatus.PerformLayout();
            this.grpSynchro.ResumeLayout(false);
            this.grpSynchro.PerformLayout();
            this.grpLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;

        private System.Windows.Forms.GroupBox grpAngles;
        private System.Windows.Forms.Label lblRollAngle;
        private System.Windows.Forms.Label lblAzimuthAngle;
        private System.Windows.Forms.Label lblElevationAngle;
        private System.Windows.Forms.TextBox tbRollAngle;
        private System.Windows.Forms.TextBox tbAzimuthAngle;
        private System.Windows.Forms.TextBox tbElevationAngle;
        private System.Windows.Forms.Button btnApplyAngles;
        private System.Windows.Forms.Label lblRollThreshold;
        private System.Windows.Forms.Label lblAzimuthThreshold;
        private System.Windows.Forms.Label lblElevationThreshold;

        private System.Windows.Forms.GroupBox grpSimulation;
        private System.Windows.Forms.Button btnSimulateSuccess;
        private System.Windows.Forms.Button btnSimulateFail;

        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.GroupBox grpSynchro;
        private System.Windows.Forms.Label lblSynchro3Title;
        private System.Windows.Forms.Label lblSynchro4Title;
        private System.Windows.Forms.Label lblSynchro89Title;
        private System.Windows.Forms.Label lblSynchro110Title;
        private System.Windows.Forms.Label lblSynchro111Title;
        private System.Windows.Forms.Label lblSynchro112Title;
        private System.Windows.Forms.Label lblSynchro3;
        private System.Windows.Forms.Label lblSynchro4;
        private System.Windows.Forms.Label lblSynchro89;
        private System.Windows.Forms.Label lblSynchro110;
        private System.Windows.Forms.Label lblSynchro111;
        private System.Windows.Forms.Label lblSynchro112;

        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.Button btnStartCalibration;
    }
}