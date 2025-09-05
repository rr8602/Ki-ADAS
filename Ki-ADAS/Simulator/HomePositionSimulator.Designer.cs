using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ki_ADAS
{
    partial class HomePositionSimulator
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
            this.pnlCheckPJI = new System.Windows.Forms.Panel();
            this.lblCheckPJI = new System.Windows.Forms.Label();
            this.pnlVEPStatus = new System.Windows.Forms.Panel();
            this.lblVEPStatus = new System.Windows.Forms.Label();
            this.pnlCameraRadarOption = new System.Windows.Forms.Panel();
            this.lblCameraRadarOption = new System.Windows.Forms.Label();
            this.btnSetTrafficLight = new System.Windows.Forms.Button();
            this.btnScanBarcode = new System.Windows.Forms.Button();
            this.btnRequestPJI = new System.Windows.Forms.Button();
            this.btnCheckVEPStatus = new System.Windows.Forms.Button();
            this.btnSelectOption = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.radioRearLeftRadar = new System.Windows.Forms.RadioButton();
            this.radioRearRightRadar = new System.Windows.Forms.RadioButton();
            this.radioCamera = new System.Windows.Forms.RadioButton();
            this.lblPositioningDevice = new System.Windows.Forms.Label();
            this.lblCameraTarget = new System.Windows.Forms.Label();
            this.lblVehicleDetect = new System.Windows.Forms.Label();
            this.chkVEPWorking = new System.Windows.Forms.CheckBox();
            this.lbl_time = new System.Windows.Forms.Label();
            this.btnMoveCarToBench = new System.Windows.Forms.Button();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.grpTestPosition = new System.Windows.Forms.GroupBox();
            this.btnStartTest = new System.Windows.Forms.Button();
            this.btnSetTestPosition = new System.Windows.Forms.Button();
            this.grpTargetSelection = new System.Windows.Forms.GroupBox();
            this.radioCalFrontCamera = new System.Windows.Forms.RadioButton();
            this.radioCalRearRightRadar = new System.Windows.Forms.RadioButton();
            this.radioCalRearLeftRadar = new System.Windows.Forms.RadioButton();
            this.btnSendToVEP = new System.Windows.Forms.Button();
            this.btnCheckTestFinish = new System.Windows.Forms.Button();
            this.btnReadResults = new System.Windows.Forms.Button();
            this.pnlTestStatus = new System.Windows.Forms.Panel();
            this.lblTestStatusTitle = new System.Windows.Forms.Label();
            this.lblTestStatusValue = new System.Windows.Forms.Label();
            this.lblTargetTitle = new System.Windows.Forms.Label();
            this.lblTargetValue = new System.Windows.Forms.Label();
            this.lblTestResultTitle = new System.Windows.Forms.Label();
            this.lblTestResultValue = new System.Windows.Forms.Label();
            this.btnTestFinish = new System.Windows.Forms.Button();
            this.btnPrintTicket = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnMoveOut = new System.Windows.Forms.Button();
            this.btnExitPosition = new System.Windows.Forms.Button();
            this.btnDisplayResult = new System.Windows.Forms.Button();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.pnlHomePosition.SuspendLayout();
            this.pnlTrafficLight.SuspendLayout();
            this.pnlVehicleEntrance.SuspendLayout();
            this.pnlScanBarcode.SuspendLayout();
            this.pnlStartCycle.SuspendLayout();
            this.pnlCheckPJI.SuspendLayout();
            this.pnlVEPStatus.SuspendLayout();
            this.pnlCameraRadarOption.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpTestPosition.SuspendLayout();
            this.grpTargetSelection.SuspendLayout();
            this.pnlTestStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(16, 12);
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
            // pnlCheckPJI
            // 
            this.pnlCheckPJI.BackColor = System.Drawing.Color.LightGray;
            this.pnlCheckPJI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCheckPJI.Controls.Add(this.lblCheckPJI);
            this.pnlCheckPJI.Location = new System.Drawing.Point(20, 496);
            this.pnlCheckPJI.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCheckPJI.Name = "pnlCheckPJI";
            this.pnlCheckPJI.Size = new System.Drawing.Size(199, 69);
            this.pnlCheckPJI.TabIndex = 7;
            // 
            // lblCheckPJI
            // 
            this.lblCheckPJI.AutoSize = true;
            this.lblCheckPJI.Location = new System.Drawing.Point(53, 23);
            this.lblCheckPJI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCheckPJI.Name = "lblCheckPJI";
            this.lblCheckPJI.Size = new System.Drawing.Size(61, 15);
            this.lblCheckPJI.TabIndex = 0;
            this.lblCheckPJI.Text = "PJI 확인";
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
            // btnRequestPJI
            // 
            this.btnRequestPJI.Location = new System.Drawing.Point(240, 496);
            this.btnRequestPJI.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRequestPJI.Name = "btnRequestPJI";
            this.btnRequestPJI.Size = new System.Drawing.Size(200, 35);
            this.btnRequestPJI.TabIndex = 12;
            this.btnRequestPJI.Text = "PJI 요청";
            this.btnRequestPJI.UseVisualStyleBackColor = true;
            this.btnRequestPJI.Click += new System.EventHandler(this.btnRequestPJI_Click);
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
            this.btnSelectOption.Location = new System.Drawing.Point(452, 460);
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
            this.btnReset.Location = new System.Drawing.Point(453, 576);
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
            this.grpOptions.Controls.Add(this.radioRearLeftRadar);
            this.grpOptions.Controls.Add(this.radioRearRightRadar);
            this.grpOptions.Controls.Add(this.radioCamera);
            this.grpOptions.Location = new System.Drawing.Point(453, 335);
            this.grpOptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpOptions.Size = new System.Drawing.Size(267, 119);
            this.grpOptions.TabIndex = 20;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "옵션 선택";
            this.grpOptions.Visible = false;
            // 
            // radioRearLeftRadar
            // 
            this.radioRearLeftRadar.AutoSize = true;
            this.radioRearLeftRadar.Location = new System.Drawing.Point(20, 90);
            this.radioRearLeftRadar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioRearLeftRadar.Name = "radioRearLeftRadar";
            this.radioRearLeftRadar.Size = new System.Drawing.Size(238, 19);
            this.radioRearLeftRadar.TabIndex = 2;
            this.radioRearLeftRadar.Text = "후방 좌측 레이더 (Syncro S3+)";
            this.radioRearLeftRadar.UseVisualStyleBackColor = true;
            // 
            // radioRearRightRadar
            // 
            this.radioRearRightRadar.AutoSize = true;
            this.radioRearRightRadar.Location = new System.Drawing.Point(20, 58);
            this.radioRearRightRadar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.radioRearRightRadar.Name = "radioRearRightRadar";
            this.radioRearRightRadar.Size = new System.Drawing.Size(238, 19);
            this.radioRearRightRadar.TabIndex = 1;
            this.radioRearRightRadar.Text = "후방 우측 레이더 (Syncro S3+)";
            this.radioRearRightRadar.UseVisualStyleBackColor = true;
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
            this.lblPositioningDevice.Location = new System.Drawing.Point(467, 116);
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
            this.chkVEPWorking.Location = new System.Drawing.Point(239, 628);
            this.chkVEPWorking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkVEPWorking.Name = "chkVEPWorking";
            this.chkVEPWorking.Size = new System.Drawing.Size(111, 19);
            this.chkVEPWorking.TabIndex = 21;
            this.chkVEPWorking.Text = "VEP 작동 중";
            this.chkVEPWorking.UseVisualStyleBackColor = true;
            // 
            // lblTimeElapsed
            // 
            // 
            // lbl_time
            // 
            this.lbl_time.AutoSize = true;
            this.lbl_time.Location = new System.Drawing.Point(467, 204);
            this.lbl_time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_time.Name = "lbl_time";
            this.lbl_time.Size = new System.Drawing.Size(105, 15);
            this.lbl_time.TabIndex = 27;
            this.lbl_time.Text = "0";
            // 
            // btnMoveCarToBench
            // 
            this.btnMoveCarToBench.Location = new System.Drawing.Point(660, 254);
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
            // grpTestPosition
            // 
            this.grpTestPosition.Controls.Add(this.btnStartTest);
            this.grpTestPosition.Controls.Add(this.btnSetTestPosition);
            this.grpTestPosition.Controls.Add(this.grpTargetSelection);
            this.grpTestPosition.Controls.Add(this.btnSendToVEP);
            this.grpTestPosition.Controls.Add(this.btnCheckTestFinish);
            this.grpTestPosition.Controls.Add(this.btnReadResults);
            this.grpTestPosition.Location = new System.Drawing.Point(904, 24);
            this.grpTestPosition.Name = "grpTestPosition";
            this.grpTestPosition.Size = new System.Drawing.Size(300, 380);
            this.grpTestPosition.TabIndex = 10;
            this.grpTestPosition.TabStop = false;
            this.grpTestPosition.Text = "테스트 포지션";
            // 
            // btnStartTest
            // 
            this.btnStartTest.Location = new System.Drawing.Point(20, 30);
            this.btnStartTest.Name = "btnStartTest";
            this.btnStartTest.Size = new System.Drawing.Size(260, 30);
            this.btnStartTest.TabIndex = 0;
            this.btnStartTest.Text = "테스트 사이클 시작";
            this.btnStartTest.UseVisualStyleBackColor = true;
            this.btnStartTest.Click += new System.EventHandler(this.btnStartTest_Click);
            // 
            // btnSetTestPosition
            // 
            this.btnSetTestPosition.Enabled = false;
            this.btnSetTestPosition.Location = new System.Drawing.Point(20, 70);
            this.btnSetTestPosition.Name = "btnSetTestPosition";
            this.btnSetTestPosition.Size = new System.Drawing.Size(260, 30);
            this.btnSetTestPosition.TabIndex = 1;
            this.btnSetTestPosition.Text = "테스트 포지션 설정";
            this.btnSetTestPosition.UseVisualStyleBackColor = true;
            this.btnSetTestPosition.Click += new System.EventHandler(this.btnSetTestPosition_Click);
            // 
            // grpTargetSelection
            // 
            this.grpTargetSelection.Controls.Add(this.radioCalFrontCamera);
            this.grpTargetSelection.Controls.Add(this.radioCalRearRightRadar);
            this.grpTargetSelection.Controls.Add(this.radioCalRearLeftRadar);
            this.grpTargetSelection.Location = new System.Drawing.Point(20, 110);
            this.grpTargetSelection.Name = "grpTargetSelection";
            this.grpTargetSelection.Size = new System.Drawing.Size(260, 120);
            this.grpTargetSelection.TabIndex = 2;
            this.grpTargetSelection.TabStop = false;
            this.grpTargetSelection.Text = "타겟 보정 위치";
            this.grpTargetSelection.Visible = false;
            // 
            // radioCalFrontCamera
            // 
            this.radioCalFrontCamera.AutoSize = true;
            this.radioCalFrontCamera.Enabled = false;
            this.radioCalFrontCamera.Location = new System.Drawing.Point(10, 20);
            this.radioCalFrontCamera.Name = "radioCalFrontCamera";
            this.radioCalFrontCamera.Size = new System.Drawing.Size(108, 19);
            this.radioCalFrontCamera.TabIndex = 0;
            this.radioCalFrontCamera.TabStop = true;
            this.radioCalFrontCamera.Text = "전방 카메라";
            this.radioCalFrontCamera.UseVisualStyleBackColor = true;
            // 
            // radioCalRearRightRadar
            // 
            this.radioCalRearRightRadar.AutoSize = true;
            this.radioCalRearRightRadar.Enabled = false;
            this.radioCalRearRightRadar.Location = new System.Drawing.Point(10, 50);
            this.radioCalRearRightRadar.Name = "radioCalRearRightRadar";
            this.radioCalRearRightRadar.Size = new System.Drawing.Size(143, 19);
            this.radioCalRearRightRadar.TabIndex = 1;
            this.radioCalRearRightRadar.TabStop = true;
            this.radioCalRearRightRadar.Text = "후방 우측 레이더";
            this.radioCalRearRightRadar.UseVisualStyleBackColor = true;
            // 
            // radioCalRearLeftRadar
            // 
            this.radioCalRearLeftRadar.AutoSize = true;
            this.radioCalRearLeftRadar.Enabled = false;
            this.radioCalRearLeftRadar.Location = new System.Drawing.Point(10, 80);
            this.radioCalRearLeftRadar.Name = "radioCalRearLeftRadar";
            this.radioCalRearLeftRadar.Size = new System.Drawing.Size(143, 19);
            this.radioCalRearLeftRadar.TabIndex = 2;
            this.radioCalRearLeftRadar.TabStop = true;
            this.radioCalRearLeftRadar.Text = "후방 좌측 레이더";
            this.radioCalRearLeftRadar.UseVisualStyleBackColor = true;
            // 
            // btnSendToVEP
            // 
            this.btnSendToVEP.Enabled = false;
            this.btnSendToVEP.Location = new System.Drawing.Point(20, 240);
            this.btnSendToVEP.Name = "btnSendToVEP";
            this.btnSendToVEP.Size = new System.Drawing.Size(260, 30);
            this.btnSendToVEP.TabIndex = 3;
            this.btnSendToVEP.Text = "VEP에 전송";
            this.btnSendToVEP.UseVisualStyleBackColor = true;
            this.btnSendToVEP.Click += new System.EventHandler(this.btnSendToVEP_Click);
            // 
            // btnCheckTestFinish
            // 
            this.btnCheckTestFinish.Enabled = false;
            this.btnCheckTestFinish.Location = new System.Drawing.Point(20, 280);
            this.btnCheckTestFinish.Name = "btnCheckTestFinish";
            this.btnCheckTestFinish.Size = new System.Drawing.Size(260, 30);
            this.btnCheckTestFinish.TabIndex = 4;
            this.btnCheckTestFinish.Text = "테스트 완료 확인";
            this.btnCheckTestFinish.UseVisualStyleBackColor = true;
            this.btnCheckTestFinish.Click += new System.EventHandler(this.btnCheckTestFinish_Click);
            // 
            // btnReadResults
            // 
            this.btnReadResults.Enabled = false;
            this.btnReadResults.Location = new System.Drawing.Point(20, 320);
            this.btnReadResults.Name = "btnReadResults";
            this.btnReadResults.Size = new System.Drawing.Size(260, 30);
            this.btnReadResults.TabIndex = 5;
            this.btnReadResults.Text = "테스트 결과 읽기";
            this.btnReadResults.UseVisualStyleBackColor = true;
            this.btnReadResults.Click += new System.EventHandler(this.btnReadResults_Click);
            // 
            // pnlTestStatus
            // 
            this.pnlTestStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTestStatus.Controls.Add(this.lblTestStatusTitle);
            this.pnlTestStatus.Controls.Add(this.lblTestStatusValue);
            this.pnlTestStatus.Controls.Add(this.lblTargetTitle);
            this.pnlTestStatus.Controls.Add(this.lblTargetValue);
            this.pnlTestStatus.Controls.Add(this.lblTestResultTitle);
            this.pnlTestStatus.Controls.Add(this.lblTestResultValue);
            this.pnlTestStatus.Location = new System.Drawing.Point(904, 415);
            this.pnlTestStatus.Name = "pnlTestStatus";
            this.pnlTestStatus.Size = new System.Drawing.Size(300, 291);
            this.pnlTestStatus.TabIndex = 11;
            // 
            // lblTestStatusTitle
            // 
            this.lblTestStatusTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestStatusTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTestStatusTitle.Name = "lblTestStatusTitle";
            this.lblTestStatusTitle.Size = new System.Drawing.Size(280, 20);
            this.lblTestStatusTitle.TabIndex = 0;
            this.lblTestStatusTitle.Text = "테스트 상태:";
            // 
            // lblTestStatusValue
            // 
            this.lblTestStatusValue.Location = new System.Drawing.Point(10, 35);
            this.lblTestStatusValue.Name = "lblTestStatusValue";
            this.lblTestStatusValue.Size = new System.Drawing.Size(289, 20);
            this.lblTestStatusValue.TabIndex = 1;
            this.lblTestStatusValue.Text = "대기 중";
            // 
            // lblTargetTitle
            // 
            this.lblTargetTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetTitle.Location = new System.Drawing.Point(10, 65);
            this.lblTargetTitle.Name = "lblTargetTitle";
            this.lblTargetTitle.Size = new System.Drawing.Size(280, 20);
            this.lblTargetTitle.TabIndex = 2;
            this.lblTargetTitle.Text = "선택된 타겟:";
            // 
            // lblTargetValue
            // 
            this.lblTargetValue.Location = new System.Drawing.Point(10, 90);
            this.lblTargetValue.Name = "lblTargetValue";
            this.lblTargetValue.Size = new System.Drawing.Size(289, 20);
            this.lblTargetValue.TabIndex = 3;
            this.lblTargetValue.Text = "없음";
            // 
            // lblTestResultTitle
            // 
            this.lblTestResultTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestResultTitle.Location = new System.Drawing.Point(10, 120);
            this.lblTestResultTitle.Name = "lblTestResultTitle";
            this.lblTestResultTitle.Size = new System.Drawing.Size(280, 20);
            this.lblTestResultTitle.TabIndex = 4;
            this.lblTestResultTitle.Text = "테스트 결과:";
            // 
            // lblTestResultValue
            // 
            this.lblTestResultValue.Location = new System.Drawing.Point(10, 145);
            this.lblTestResultValue.Name = "lblTestResultValue";
            this.lblTestResultValue.Size = new System.Drawing.Size(289, 135);
            this.lblTestResultValue.TabIndex = 5;
            this.lblTestResultValue.Text = "없음";
            // 
            // btnTestFinish
            // 
            this.btnTestFinish.Location = new System.Drawing.Point(21, 31);
            this.btnTestFinish.Name = "btnTestFinish";
            this.btnTestFinish.Size = new System.Drawing.Size(260, 30);
            this.btnTestFinish.TabIndex = 6;
            this.btnTestFinish.Text = "테스트 종료";
            this.btnTestFinish.UseVisualStyleBackColor = true;
            this.btnTestFinish.Click += new System.EventHandler(this.btnTestFinish_Click);
            // 
            // btnPrintTicket
            // 
            this.btnPrintTicket.Location = new System.Drawing.Point(21, 84);
            this.btnPrintTicket.Name = "btnPrintTicket";
            this.btnPrintTicket.Size = new System.Drawing.Size(260, 30);
            this.btnPrintTicket.TabIndex = 28;
            this.btnPrintTicket.Text = "티켓 출력";
            this.btnPrintTicket.UseVisualStyleBackColor = true;
            this.btnPrintTicket.Click += new System.EventHandler(this.btnPrintTicket_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEnd);
            this.groupBox1.Controls.Add(this.btnMoveOut);
            this.groupBox1.Controls.Add(this.btnExitPosition);
            this.groupBox1.Controls.Add(this.btnDisplayResult);
            this.groupBox1.Controls.Add(this.btnSaveData);
            this.groupBox1.Controls.Add(this.btnTestFinish);
            this.groupBox1.Controls.Add(this.btnPrintTicket);
            this.groupBox1.Location = new System.Drawing.Point(1237, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 380);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "테스트 포지션";
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(21, 320);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(260, 30);
            this.btnEnd.TabIndex = 33;
            this.btnEnd.Text = "프로세스 종료";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // btnMoveOut
            // 
            this.btnMoveOut.Location = new System.Drawing.Point(21, 282);
            this.btnMoveOut.Name = "btnMoveOut";
            this.btnMoveOut.Size = new System.Drawing.Size(260, 30);
            this.btnMoveOut.TabIndex = 32;
            this.btnMoveOut.Text = "차량 Move Out";
            this.btnMoveOut.UseVisualStyleBackColor = true;
            this.btnMoveOut.Click += new System.EventHandler(this.btnMoveOut_Click);
            // 
            // btnExitPosition
            // 
            this.btnExitPosition.Location = new System.Drawing.Point(21, 246);
            this.btnExitPosition.Name = "btnExitPosition";
            this.btnExitPosition.Size = new System.Drawing.Size(260, 30);
            this.btnExitPosition.TabIndex = 31;
            this.btnExitPosition.Text = "Exit Position";
            this.btnExitPosition.UseVisualStyleBackColor = true;
            this.btnExitPosition.Click += new System.EventHandler(this.btnExitPosition_Click);
            // 
            // btnDisplayResult
            // 
            this.btnDisplayResult.Location = new System.Drawing.Point(21, 185);
            this.btnDisplayResult.Name = "btnDisplayResult";
            this.btnDisplayResult.Size = new System.Drawing.Size(260, 30);
            this.btnDisplayResult.TabIndex = 30;
            this.btnDisplayResult.Text = "결과 표시";
            this.btnDisplayResult.UseVisualStyleBackColor = true;
            this.btnDisplayResult.Click += new System.EventHandler(this.btnDisplayResult_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.Location = new System.Drawing.Point(21, 149);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(260, 30);
            this.btnSaveData.TabIndex = 29;
            this.btnSaveData.Text = "데이터 저장";
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // HomePositionSimulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1549, 745);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpTestPosition);
            this.Controls.Add(this.pnlTestStatus);
            this.Controls.Add(this.lbl_time);
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
            this.Controls.Add(this.btnRequestPJI);
            this.Controls.Add(this.btnScanBarcode);
            this.Controls.Add(this.btnSetTrafficLight);
            this.Controls.Add(this.pnlCameraRadarOption);
            this.Controls.Add(this.pnlVEPStatus);
            this.Controls.Add(this.pnlCheckPJI);
            this.Controls.Add(this.pnlStartCycle);
            this.Controls.Add(this.pnlScanBarcode);
            this.Controls.Add(this.pnlVehicleEntrance);
            this.Controls.Add(this.pnlTrafficLight);
            this.Controls.Add(this.pnlHomePosition);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblTitle);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "HomePositionSimulator";
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
            this.pnlCheckPJI.ResumeLayout(false);
            this.pnlCheckPJI.PerformLayout();
            this.pnlVEPStatus.ResumeLayout(false);
            this.pnlVEPStatus.PerformLayout();
            this.pnlCameraRadarOption.ResumeLayout(false);
            this.pnlCameraRadarOption.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpTestPosition.ResumeLayout(false);
            this.grpTargetSelection.ResumeLayout(false);
            this.grpTargetSelection.PerformLayout();
            this.pnlTestStatus.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.Panel pnlCheckPJI;
        private System.Windows.Forms.Label lblCheckPJI;
        private System.Windows.Forms.Panel pnlVEPStatus;
        private System.Windows.Forms.Label lblVEPStatus;
        private System.Windows.Forms.Panel pnlCameraRadarOption;
        private System.Windows.Forms.Label lblCameraRadarOption;
        private System.Windows.Forms.Button btnSetTrafficLight;
        private System.Windows.Forms.Button btnScanBarcode;
        private System.Windows.Forms.Button btnRequestPJI;
        private System.Windows.Forms.Button btnCheckVEPStatus;
        private System.Windows.Forms.Button btnSelectOption;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.RadioButton radioRearRightRadar;
        private System.Windows.Forms.RadioButton radioCamera;
        private System.Windows.Forms.Label lblPositioningDevice;
        private System.Windows.Forms.Label lblCameraTarget;
        private System.Windows.Forms.Label lblVehicleDetect;
        private System.Windows.Forms.CheckBox chkVEPWorking;
        private System.Windows.Forms.Label lbl_time;
        private System.Windows.Forms.Button btnMoveCarToBench;
        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.GroupBox grpTestPosition;
        private System.Windows.Forms.Button btnStartTest;
        private System.Windows.Forms.Button btnSetTestPosition;
        private System.Windows.Forms.GroupBox grpTargetSelection;
        private System.Windows.Forms.RadioButton radioCalFrontCamera;
        private System.Windows.Forms.RadioButton radioCalRearRightRadar;
        private System.Windows.Forms.RadioButton radioCalRearLeftRadar;
        private System.Windows.Forms.Button btnSendToVEP;
        private System.Windows.Forms.Button btnCheckTestFinish;
        private System.Windows.Forms.Button btnReadResults;
        private System.Windows.Forms.Panel pnlTestStatus;
        private System.Windows.Forms.Label lblTestStatusTitle;
        private System.Windows.Forms.Label lblTestStatusValue;
        private System.Windows.Forms.Label lblTargetTitle;
        private System.Windows.Forms.Label lblTargetValue;
        private System.Windows.Forms.Label lblTestResultTitle;
        private System.Windows.Forms.Label lblTestResultValue;
        private RadioButton radioRearLeftRadar;
        private Button btnTestFinish;
        private Button btnPrintTicket;
        private GroupBox groupBox1;
        private Button btnSaveData;
        private Button btnDisplayResult;
        private Button btnExitPosition;
        private Button btnEnd;
        private Button btnMoveOut;
    }
}