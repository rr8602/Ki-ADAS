using System;
using System.Drawing;
using System.Windows.Forms;

namespace HomePositionSimulator
{
    partial class Form1
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlHomePosition = new System.Windows.Forms.Panel();
            this.lblHomePosition = new System.Windows.Forms.Label();
            this.pnlTrafficLight = new System.Windows.Forms.Panel();
            this.lblTrafficLight = new System.Windows.Forms.Label();
            this.pnlVehicleEntrance = new System.Windows.Forms.Panel();
            this.lblVehicleEntrance = new System.Windows.Forms.Label();
            this.pnlScanBarcode = new System.Windows.Forms.Panel();
            this.lblScanBarcode = new System.Windows.Forms.Label();
            this.pnlStartCycle = new System.Windows.Forms.Panel();
            this.lblStartCycle = new System.Windows.Forms.Label();
            this.pnlCheckPII = new System.Windows.Forms.Panel();
            this.lblCheckPII = new System.Windows.Forms.Label();
            this.pnlVEPStatus = new System.Windows.Forms.Panel();
            this.lblVEPStatus = new System.Windows.Forms.Label();
            this.pnlCameraRadarOption = new System.Windows.Forms.Panel();
            this.lblCameraRadarOption = new System.Windows.Forms.Label();
            this.btnSetTrafficLight = new System.Windows.Forms.Button();
            this.btnScanBarcode = new System.Windows.Forms.Button();
            this.btnRequestPII = new System.Windows.Forms.Button();
            this.btnCheckVEPStatus = new System.Windows.Forms.Button();
            this.btnSelectOption = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.radioRadar = new System.Windows.Forms.RadioButton();
            this.radioCamera = new System.Windows.Forms.RadioButton();
            this.lblPositioningDevice = new System.Windows.Forms.Label();
            this.lblCameraTarget = new System.Windows.Forms.Label();
            this.lblVehicleDetect = new System.Windows.Forms.Label();
            this.chkVEPWorking = new System.Windows.Forms.CheckBox();
            this.lblTimeElapsed = new System.Windows.Forms.Label();
            this.btnMoveCarToBench = new System.Windows.Forms.Button();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.pnlHomePosition.SuspendLayout();
            this.pnlTrafficLight.SuspendLayout();
            this.pnlVehicleEntrance.SuspendLayout();
            this.pnlScanBarcode.SuspendLayout();
            this.pnlStartCycle.SuspendLayout();
            this.pnlCheckPII.SuspendLayout();
            this.pnlVEPStatus.SuspendLayout();
            this.pnlCameraRadarOption.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(16, 10);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(334, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Home Position Simulator";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(17, 52);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(118, 25);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "상태: 홈 포지션";
            // 
            // pnlHomePosition
            // 
            this.pnlHomePosition.BackColor = System.Drawing.Color.LightGreen;
            this.pnlHomePosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHomePosition.Controls.Add(this.lblHomePosition);
            this.pnlHomePosition.Location = new System.Drawing.Point(20, 92);
            this.pnlHomePosition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlHomePosition.Name = "pnlHomePosition";
            this.pnlHomePosition.Size = new System.Drawing.Size(199, 69);
            this.pnlHomePosition.TabIndex = 2;
            // 
            // lblHomePosition
            // 
            this.lblHomePosition.AutoSize = true;
            this.lblHomePosition.Location = new System.Drawing.Point(53, 23);
            this.lblHomePosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHomePosition.Name = "lblHomePosition";
            this.lblHomePosition.Size = new System.Drawing.Size(72, 15);
            this.lblHomePosition.TabIndex = 0;
            this.lblHomePosition.Text = "홈 포지션";
            // 
            // pnlTrafficLight
            // 
            this.pnlTrafficLight.BackColor = System.Drawing.Color.LightGray;
            this.pnlTrafficLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTrafficLight.Controls.Add(this.lblTrafficLight);
            this.pnlTrafficLight.Location = new System.Drawing.Point(20, 173);
            this.pnlTrafficLight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlTrafficLight.Name = "pnlTrafficLight";
            this.pnlTrafficLight.Size = new System.Drawing.Size(199, 69);
            this.pnlTrafficLight.TabIndex = 3;
            // 
            // lblTrafficLight
            // 
            this.lblTrafficLight.AutoSize = true;
            this.lblTrafficLight.Location = new System.Drawing.Point(53, 23);
            this.lblTrafficLight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrafficLight.Name = "lblTrafficLight";
            this.lblTrafficLight.Size = new System.Drawing.Size(87, 15);
            this.lblTrafficLight.TabIndex = 0;
            this.lblTrafficLight.Text = "신호등 상태";
            // 
            // pnlVehicleEntrance
            // 
            this.pnlVehicleEntrance.BackColor = System.Drawing.Color.LightGray;
            this.pnlVehicleEntrance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVehicleEntrance.Controls.Add(this.lblVehicleEntrance);
            this.pnlVehicleEntrance.Location = new System.Drawing.Point(20, 254);
            this.pnlVehicleEntrance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlVehicleEntrance.Name = "pnlVehicleEntrance";
            this.pnlVehicleEntrance.Size = new System.Drawing.Size(199, 69);
            this.pnlVehicleEntrance.TabIndex = 4;
            // 
            // lblVehicleEntrance
            // 
            this.lblVehicleEntrance.AutoSize = true;
            this.lblVehicleEntrance.Location = new System.Drawing.Point(53, 23);
            this.lblVehicleEntrance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVehicleEntrance.Name = "lblVehicleEntrance";
            this.lblVehicleEntrance.Size = new System.Drawing.Size(72, 15);
            this.lblVehicleEntrance.TabIndex = 0;
            this.lblVehicleEntrance.Text = "차량 진입";
            // 
            // pnlScanBarcode
            // 
            this.pnlScanBarcode.BackColor = System.Drawing.Color.LightGray;
            this.pnlScanBarcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlScanBarcode.Controls.Add(this.lblScanBarcode);
            this.pnlScanBarcode.Location = new System.Drawing.Point(20, 335);
            this.pnlScanBarcode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlScanBarcode.Name = "pnlScanBarcode";
            this.pnlScanBarcode.Size = new System.Drawing.Size(199, 69);
            this.pnlScanBarcode.TabIndex = 5;
            // 
            // lblScanBarcode
            // 
            this.lblScanBarcode.AutoSize = true;
            this.lblScanBarcode.Location = new System.Drawing.Point(53, 23);
            this.lblScanBarcode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScanBarcode.Name = "lblScanBarcode";
            this.lblScanBarcode.Size = new System.Drawing.Size(87, 15);
            this.lblScanBarcode.TabIndex = 0;
            this.lblScanBarcode.Text = "바코드 스캔";
            // 
            // pnlStartCycle
            // 
            this.pnlStartCycle.BackColor = System.Drawing.Color.LightGray;
            this.pnlStartCycle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStartCycle.Controls.Add(this.lblStartCycle);
            this.pnlStartCycle.Location = new System.Drawing.Point(20, 415);
            this.pnlStartCycle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlStartCycle.Name = "pnlStartCycle";
            this.pnlStartCycle.Size = new System.Drawing.Size(199, 69);
            this.pnlStartCycle.TabIndex = 6;
            // 
            // lblStartCycle
            // 
            this.lblStartCycle.AutoSize = true;
            this.lblStartCycle.Location = new System.Drawing.Point(53, 23);
            this.lblStartCycle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStartCycle.Name = "lblStartCycle";
            this.lblStartCycle.Size = new System.Drawing.Size(87, 15);
            this.lblStartCycle.TabIndex = 0;
            this.lblStartCycle.Text = "사이클 시작";
            // 
            // pnlCheckPII
            // 
            this.pnlCheckPII.BackColor = System.Drawing.Color.LightGray;
            this.pnlCheckPII.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCheckPII.Controls.Add(this.lblCheckPII);
            this.pnlCheckPII.Location = new System.Drawing.Point(20, 496);
            this.pnlCheckPII.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCheckPII.Name = "pnlCheckPII";
            this.pnlCheckPII.Size = new System.Drawing.Size(199, 69);
            this.pnlCheckPII.TabIndex = 7;
            // 
            // lblCheckPII
            // 
            this.lblCheckPII.AutoSize = true;
            this.lblCheckPII.Location = new System.Drawing.Point(53, 23);
            this.lblCheckPII.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCheckPII.Name = "lblCheckPII";
            this.lblCheckPII.Size = new System.Drawing.Size(58, 15);
            this.lblCheckPII.TabIndex = 0;
            this.lblCheckPII.Text = "PII 확인";
            // 
            // pnlVEPStatus
            // 
            this.pnlVEPStatus.BackColor = System.Drawing.Color.LightGray;
            this.pnlVEPStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVEPStatus.Controls.Add(this.lblVEPStatus);
            this.pnlVEPStatus.Location = new System.Drawing.Point(20, 577);
            this.pnlVEPStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlVEPStatus.Name = "pnlVEPStatus";
            this.pnlVEPStatus.Size = new System.Drawing.Size(199, 69);
            this.pnlVEPStatus.TabIndex = 8;
            // 
            // lblVEPStatus
            // 
            this.lblVEPStatus.AutoSize = true;
            this.lblVEPStatus.Location = new System.Drawing.Point(53, 23);
            this.lblVEPStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVEPStatus.Name = "lblVEPStatus";
            this.lblVEPStatus.Size = new System.Drawing.Size(69, 15);
            this.lblVEPStatus.TabIndex = 0;
            this.lblVEPStatus.Text = "VEP 상태";
            // 
            // pnlCameraRadarOption
            // 
            this.pnlCameraRadarOption.BackColor = System.Drawing.Color.LightGray;
            this.pnlCameraRadarOption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCameraRadarOption.Controls.Add(this.lblCameraRadarOption);
            this.pnlCameraRadarOption.Location = new System.Drawing.Point(453, 254);
            this.pnlCameraRadarOption.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCameraRadarOption.Name = "pnlCameraRadarOption";
            this.pnlCameraRadarOption.Size = new System.Drawing.Size(199, 69);
            this.pnlCameraRadarOption.TabIndex = 9;
            // 
            // lblCameraRadarOption
            // 
            this.lblCameraRadarOption.AutoSize = true;
            this.lblCameraRadarOption.Location = new System.Drawing.Point(40, 23);
            this.lblCameraRadarOption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCameraRadarOption.Name = "lblCameraRadarOption";
            this.lblCameraRadarOption.Size = new System.Drawing.Size(107, 15);
            this.lblCameraRadarOption.TabIndex = 0;
            this.lblCameraRadarOption.Text = "장치 옵션 선택";
            // 
            // btnSetTrafficLight
            // 
            this.btnSetTrafficLight.Location = new System.Drawing.Point(240, 173);
            this.btnSetTrafficLight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSetTrafficLight.Name = "btnSetTrafficLight";
            this.btnSetTrafficLight.Size = new System.Drawing.Size(200, 35);
            this.btnSetTrafficLight.TabIndex = 10;
            this.btnSetTrafficLight.Text = "신호등 초록색 설정";
            this.btnSetTrafficLight.UseVisualStyleBackColor = true;
            this.btnSetTrafficLight.Click += new System.EventHandler(this.btnSetTrafficLight_Click);
            // 
            // btnScanBarcode
            // 
            this.btnScanBarcode.Location = new System.Drawing.Point(240, 335);
            this.btnScanBarcode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnScanBarcode.Name = "btnScanBarcode";
            this.btnScanBarcode.Size = new System.Drawing.Size(200, 35);
            this.btnScanBarcode.TabIndex = 11;
            this.btnScanBarcode.Text = "바코드 스캔";
            this.btnScanBarcode.UseVisualStyleBackColor = true;
            this.btnScanBarcode.Click += new System.EventHandler(this.btnScanBarcode_Click);
            // 
            // btnRequestPII
            // 
            this.btnRequestPII.Location = new System.Drawing.Point(240, 496);
            this.btnRequestPII.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRequestPII.Name = "btnRequestPII";
            this.btnRequestPII.Size = new System.Drawing.Size(200, 35);
            this.btnRequestPII.TabIndex = 12;
            this.btnRequestPII.Text = "PII 요청";
            this.btnRequestPII.UseVisualStyleBackColor = true;
            this.btnRequestPII.Click += new System.EventHandler(this.btnRequestPII_Click);
            // 
            // btnCheckVEPStatus
            // 
            this.btnCheckVEPStatus.Location = new System.Drawing.Point(240, 577);
            this.btnCheckVEPStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCheckVEPStatus.Name = "btnCheckVEPStatus";
            this.btnCheckVEPStatus.Size = new System.Drawing.Size(200, 35);
            this.btnCheckVEPStatus.TabIndex = 13;
            this.btnCheckVEPStatus.Text = "VEP 상태 확인";
            this.btnCheckVEPStatus.UseVisualStyleBackColor = true;
            this.btnCheckVEPStatus.Click += new System.EventHandler(this.btnCheckVEPStatus_Click);
            // 
            // btnSelectOption
            // 
            this.btnSelectOption.Location = new System.Drawing.Point(453, 438);
            this.btnSelectOption.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSelectOption.Name = "btnSelectOption";
            this.btnSelectOption.Size = new System.Drawing.Size(200, 35);
            this.btnSelectOption.TabIndex = 14;
            this.btnSelectOption.Text = "옵션 선택";
            this.btnSelectOption.UseVisualStyleBackColor = true;
            this.btnSelectOption.Click += new System.EventHandler(this.btnSelectOption_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(680, 577);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(200, 35);
            this.btnReset.TabIndex = 15;
            this.btnReset.Text = "초기화";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.radioRadar);
            this.grpOptions.Controls.Add(this.radioCamera);
            this.grpOptions.Location = new System.Drawing.Point(453, 335);
            this.grpOptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpOptions.Size = new System.Drawing.Size(267, 92);
            this.grpOptions.TabIndex = 20;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "옵션 선택";
            // 
            // radioRadar
            // 
            this.radioRadar.AutoSize = true;
            this.radioRadar.Location = new System.Drawing.Point(20, 58);
            this.radioRadar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioRadar.Name = "radioRadar";
            this.radioRadar.Size = new System.Drawing.Size(168, 19);
            this.radioRadar.TabIndex = 1;
            this.radioRadar.Text = "레이더 (Syncro S3+)";
            this.radioRadar.UseVisualStyleBackColor = true;
            // 
            // radioCamera
            // 
            this.radioCamera.AutoSize = true;
            this.radioCamera.Location = new System.Drawing.Point(20, 29);
            this.radioCamera.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioCamera.Name = "radioCamera";
            this.radioCamera.Size = new System.Drawing.Size(168, 19);
            this.radioCamera.TabIndex = 0;
            this.radioCamera.Text = "카메라 (Syncro S1+)";
            this.radioCamera.UseVisualStyleBackColor = true;
            // 
            // lblPositioningDevice
            // 
            this.lblPositioningDevice.AutoSize = true;
            this.lblPositioningDevice.Location = new System.Drawing.Point(467, 115);
            this.lblPositioningDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPositioningDevice.Name = "lblPositioningDevice";
            this.lblPositioningDevice.Size = new System.Drawing.Size(127, 15);
            this.lblPositioningDevice.TabIndex = 24;
            this.lblPositioningDevice.Text = "포지셔닝 장치: 홈";
            // 
            // lblCameraTarget
            // 
            this.lblCameraTarget.AutoSize = true;
            this.lblCameraTarget.Location = new System.Drawing.Point(467, 144);
            this.lblCameraTarget.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCameraTarget.Name = "lblCameraTarget";
            this.lblCameraTarget.Size = new System.Drawing.Size(112, 15);
            this.lblCameraTarget.TabIndex = 25;
            this.lblCameraTarget.Text = "카메라 타겟: 홈";
            // 
            // lblVehicleDetect
            // 
            this.lblVehicleDetect.AutoSize = true;
            this.lblVehicleDetect.Location = new System.Drawing.Point(467, 173);
            this.lblVehicleDetect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVehicleDetect.Name = "lblVehicleDetect";
            this.lblVehicleDetect.Size = new System.Drawing.Size(147, 15);
            this.lblVehicleDetect.TabIndex = 26;
            this.lblVehicleDetect.Text = "차량 감지 센서: 꺼짐";
            // 
            // chkVEPWorking
            // 
            this.chkVEPWorking.AutoSize = true;
            this.chkVEPWorking.Location = new System.Drawing.Point(453, 496);
            this.chkVEPWorking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkVEPWorking.Name = "chkVEPWorking";
            this.chkVEPWorking.Size = new System.Drawing.Size(111, 19);
            this.chkVEPWorking.TabIndex = 21;
            this.chkVEPWorking.Text = "VEP 작동 중";
            this.chkVEPWorking.UseVisualStyleBackColor = true;
            // 
            // lblTimeElapsed
            // 
            this.lblTimeElapsed.AutoSize = true;
            this.lblTimeElapsed.Location = new System.Drawing.Point(467, 202);
            this.lblTimeElapsed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimeElapsed.Name = "lblTimeElapsed";
            this.lblTimeElapsed.Size = new System.Drawing.Size(105, 15);
            this.lblTimeElapsed.TabIndex = 27;
            this.lblTimeElapsed.Text = "경과 시간: 0초";
            // 
            // btnMoveCarToBench
            // 
            this.btnMoveCarToBench.Location = new System.Drawing.Point(680, 254);
            this.btnMoveCarToBench.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMoveCarToBench.Name = "btnMoveCarToBench";
            this.btnMoveCarToBench.Size = new System.Drawing.Size(200, 40);
            this.btnMoveCarToBench.TabIndex = 22;
            this.btnMoveCarToBench.Text = "차량을 벤치로 이동";
            this.btnMoveCarToBench.UseVisualStyleBackColor = true;
            this.btnMoveCarToBench.Click += new System.EventHandler(this.btnMoveCarToBench_Click);
            // 
            // grpStatus
            // 
            this.grpStatus.Location = new System.Drawing.Point(453, 92);
            this.grpStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpStatus.Size = new System.Drawing.Size(333, 150);
            this.grpStatus.TabIndex = 23;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "시스템 상태";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(912, 670);
            this.Controls.Add(this.lblTimeElapsed);
            this.Controls.Add(this.lblVehicleDetect);
            this.Controls.Add(this.lblCameraTarget);
            this.Controls.Add(this.lblPositioningDevice);
            this.Controls.Add(this.grpStatus);
            this.Controls.Add(this.btnMoveCarToBench);
            this.Controls.Add(this.chkVEPWorking);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSelectOption);
            this.Controls.Add(this.btnCheckVEPStatus);
            this.Controls.Add(this.btnRequestPII);
            this.Controls.Add(this.btnScanBarcode);
            this.Controls.Add(this.btnSetTrafficLight);
            this.Controls.Add(this.pnlCameraRadarOption);
            this.Controls.Add(this.pnlVEPStatus);
            this.Controls.Add(this.pnlCheckPII);
            this.Controls.Add(this.pnlStartCycle);
            this.Controls.Add(this.pnlScanBarcode);
            this.Controls.Add(this.pnlVehicleEntrance);
            this.Controls.Add(this.pnlTrafficLight);
            this.Controls.Add(this.pnlHomePosition);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTitle);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "Home Position Simulator";
            this.pnlHomePosition.ResumeLayout(false);
            this.pnlHomePosition.PerformLayout();
            this.pnlTrafficLight.ResumeLayout(false);
            this.pnlTrafficLight.PerformLayout();
            this.pnlVehicleEntrance.ResumeLayout(false);
            this.pnlVehicleEntrance.PerformLayout();
            this.pnlScanBarcode.ResumeLayout(false);
            this.pnlScanBarcode.PerformLayout();
            this.pnlStartCycle.ResumeLayout(false);
            this.pnlStartCycle.PerformLayout();
            this.pnlCheckPII.ResumeLayout(false);
            this.pnlCheckPII.PerformLayout();
            this.pnlVEPStatus.ResumeLayout(false);
            this.pnlVEPStatus.PerformLayout();
            this.pnlCameraRadarOption.ResumeLayout(false);
            this.pnlCameraRadarOption.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel pnlHomePosition;
        private System.Windows.Forms.Label lblHomePosition;
        private System.Windows.Forms.Panel pnlTrafficLight;
        private System.Windows.Forms.Label lblTrafficLight;
        private System.Windows.Forms.Panel pnlVehicleEntrance;
        private System.Windows.Forms.Label lblVehicleEntrance;
        private System.Windows.Forms.Panel pnlScanBarcode;
        private System.Windows.Forms.Label lblScanBarcode;
        private System.Windows.Forms.Panel pnlStartCycle;
        private System.Windows.Forms.Label lblStartCycle;
        private System.Windows.Forms.Panel pnlCheckPII;
        private System.Windows.Forms.Label lblCheckPII;
        private System.Windows.Forms.Panel pnlVEPStatus;
        private System.Windows.Forms.Label lblVEPStatus;
        private System.Windows.Forms.Panel pnlCameraRadarOption;
        private System.Windows.Forms.Label lblCameraRadarOption;
        private System.Windows.Forms.Button btnSetTrafficLight;
        private System.Windows.Forms.Button btnScanBarcode;
        private System.Windows.Forms.Button btnRequestPII;
        private System.Windows.Forms.Button btnCheckVEPStatus;
        private System.Windows.Forms.Button btnSelectOption;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.RadioButton radioRadar;
        private System.Windows.Forms.RadioButton radioCamera;
        private System.Windows.Forms.Label lblPositioningDevice;
        private System.Windows.Forms.Label lblCameraTarget;
        private System.Windows.Forms.Label lblVehicleDetect;
        private System.Windows.Forms.CheckBox chkVEPWorking;
        private System.Windows.Forms.Label lblTimeElapsed;
        private System.Windows.Forms.Button btnMoveCarToBench;
        private System.Windows.Forms.GroupBox grpStatus;
    }
}