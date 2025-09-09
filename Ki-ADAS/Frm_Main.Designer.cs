namespace Ki_ADAS
{
    partial class Frm_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Main));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label19 = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_pji = new System.Windows.Forms.Label();
            this.lbl_wheelbase = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_model = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.seqList = new System.Windows.Forms.ListView();
            this.label29 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.lb_Message = new System.Windows.Forms.ListBox();
            this.GB_GenInfo = new System.Windows.Forms.GroupBox();
            this.btn_register = new System.Windows.Forms.Button();
            this.txt_barcode = new System.Windows.Forms.TextBox();
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnTestModbus = new System.Windows.Forms.Button();
            this.GB_GenInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader1
            // 
            this.columnHeader1.Tag = "AcceptNo";
            this.columnHeader1.Text = "AcceptNo";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Tag = "PJI";
            this.columnHeader2.Text = "PJI";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Tag = "Model";
            this.columnHeader3.Text = "Model";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 295;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Yellow;
            this.label19.Location = new System.Drawing.Point(-553, -300);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(916, 94);
            this.label19.TabIndex = 162;
            this.label19.Text = "Vehicle List";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_title
            // 
            this.lbl_title.BackColor = System.Drawing.Color.Cornsilk;
            this.lbl_title.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_title.Location = new System.Drawing.Point(364, -300);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(1104, 96);
            this.lbl_title.TabIndex = 159;
            this.lbl_title.Text = "PROCESS INFO";
            this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_pji
            // 
            this.lbl_pji.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_pji.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_pji.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_pji.Location = new System.Drawing.Point(441, 237);
            this.lbl_pji.Name = "lbl_pji";
            this.lbl_pji.Size = new System.Drawing.Size(640, 95);
            this.lbl_pji.TabIndex = 21;
            this.lbl_pji.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_wheelbase
            // 
            this.lbl_wheelbase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_wheelbase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_wheelbase.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_wheelbase.Location = new System.Drawing.Point(441, 131);
            this.lbl_wheelbase.Name = "lbl_wheelbase";
            this.lbl_wheelbase.Size = new System.Drawing.Size(640, 95);
            this.lbl_wheelbase.TabIndex = 18;
            this.lbl_wheelbase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 30F);
            this.label6.Location = new System.Drawing.Point(18, 347);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(336, 95);
            this.label6.TabIndex = 16;
            this.label6.Text = "Barcode";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 30F);
            this.label5.Location = new System.Drawing.Point(18, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(336, 95);
            this.label5.TabIndex = 15;
            this.label5.Text = "PJI";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_model
            // 
            this.lbl_model.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_model.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_model.Font = new System.Drawing.Font("Arial", 30F);
            this.lbl_model.Location = new System.Drawing.Point(441, 21);
            this.lbl_model.Name = "lbl_model";
            this.lbl_model.Size = new System.Drawing.Size(640, 95);
            this.lbl_model.TabIndex = 17;
            this.lbl_model.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 30F);
            this.label2.Location = new System.Drawing.Point(18, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 95);
            this.label2.TabIndex = 12;
            this.label2.Text = "Wheelbase";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Cornsilk;
            this.label1.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(919, -4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1104, 96);
            this.label1.TabIndex = 164;
            this.label1.Text = "PROCESS INFO";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Arial", 30F);
            this.label13.Location = new System.Drawing.Point(18, 34);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(336, 81);
            this.label13.TabIndex = 11;
            this.label13.Text = "Model";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // seqList
            // 
            this.seqList.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.seqList.BackColor = System.Drawing.Color.SeaShell;
            this.seqList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.seqList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.seqList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seqList.FullRowSelect = true;
            this.seqList.GridLines = true;
            this.seqList.HideSelection = false;
            this.seqList.Location = new System.Drawing.Point(2, 95);
            this.seqList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.seqList.MultiSelect = false;
            this.seqList.Name = "seqList";
            this.seqList.OwnerDraw = true;
            this.seqList.Size = new System.Drawing.Size(913, 781);
            this.seqList.TabIndex = 170;
            this.seqList.UseCompatibleStateImageBehavior = false;
            this.seqList.View = System.Windows.Forms.View.Details;
            this.seqList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.seqList_MouseClick);
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.LightGray;
            this.label29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label29.Font = new System.Drawing.Font("Arial", 25F);
            this.label29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label29.Location = new System.Drawing.Point(3, 880);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(911, 54);
            this.label29.TabIndex = 169;
            this.label29.Text = "Log";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(742, 786);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(171, 61);
            this.button1.TabIndex = 168;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Font = new System.Drawing.Font("Arial", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Yellow;
            this.label30.Location = new System.Drawing.Point(2, -4);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(916, 94);
            this.label30.TabIndex = 167;
            this.label30.Text = "Vehicle List";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_Message
            // 
            this.lb_Message.FormattingEnabled = true;
            this.lb_Message.ItemHeight = 15;
            this.lb_Message.Location = new System.Drawing.Point(2, 941);
            this.lb_Message.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lb_Message.Name = "lb_Message";
            this.lb_Message.Size = new System.Drawing.Size(911, 214);
            this.lb_Message.TabIndex = 163;
            // 
            // GB_GenInfo
            // 
            this.GB_GenInfo.Controls.Add(this.btn_register);
            this.GB_GenInfo.Controls.Add(this.txt_barcode);
            this.GB_GenInfo.Controls.Add(this.lbl_pji);
            this.GB_GenInfo.Controls.Add(this.lbl_wheelbase);
            this.GB_GenInfo.Controls.Add(this.lbl_model);
            this.GB_GenInfo.Controls.Add(this.label6);
            this.GB_GenInfo.Controls.Add(this.label5);
            this.GB_GenInfo.Controls.Add(this.label2);
            this.GB_GenInfo.Controls.Add(this.label13);
            this.GB_GenInfo.Font = new System.Drawing.Font("Arial", 15F);
            this.GB_GenInfo.Location = new System.Drawing.Point(920, 91);
            this.GB_GenInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GB_GenInfo.Name = "GB_GenInfo";
            this.GB_GenInfo.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GB_GenInfo.Size = new System.Drawing.Size(1103, 575);
            this.GB_GenInfo.TabIndex = 165;
            this.GB_GenInfo.TabStop = false;
            this.GB_GenInfo.Text = "General Info";
            // 
            // btn_register
            // 
            this.btn_register.BackColor = System.Drawing.Color.Silver;
            this.btn_register.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_register.ForeColor = System.Drawing.Color.Teal;
            this.btn_register.Location = new System.Drawing.Point(954, 347);
            this.btn_register.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_register.Name = "btn_register";
            this.btn_register.Size = new System.Drawing.Size(127, 95);
            this.btn_register.TabIndex = 168;
            this.btn_register.Text = "Register";
            this.btn_register.UseVisualStyleBackColor = false;
            this.btn_register.Click += new System.EventHandler(this.BtnRegister_Click);
            // 
            // txt_barcode
            // 
            this.txt_barcode.BackColor = System.Drawing.SystemColors.Window;
            this.txt_barcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_barcode.Font = new System.Drawing.Font("Arial", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_barcode.Location = new System.Drawing.Point(441, 347);
            this.txt_barcode.Margin = new System.Windows.Forms.Padding(0);
            this.txt_barcode.Multiline = true;
            this.txt_barcode.Name = "txt_barcode";
            this.txt_barcode.Size = new System.Drawing.Size(498, 95);
            this.txt_barcode.TabIndex = 22;
            // 
            // BtnStart
            // 
            this.BtnStart.BackColor = System.Drawing.Color.LightGreen;
            this.BtnStart.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnStart.Location = new System.Drawing.Point(921, 673);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(300, 138);
            this.BtnStart.TabIndex = 172;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = false;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.BackColor = System.Drawing.Color.LightCoral;
            this.BtnStop.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnStop.Location = new System.Drawing.Point(1237, 673);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(300, 138);
            this.BtnStop.TabIndex = 173;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnTestModbus
            // 
            this.BtnTestModbus.BackColor = System.Drawing.Color.Teal;
            this.BtnTestModbus.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnTestModbus.Location = new System.Drawing.Point(921, 1017);
            this.BtnTestModbus.Name = "BtnTestModbus";
            this.BtnTestModbus.Size = new System.Drawing.Size(300, 138);
            this.BtnTestModbus.TabIndex = 174;
            this.BtnTestModbus.Text = "Test";
            this.BtnTestModbus.UseVisualStyleBackColor = false;
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2029, 1179);
            this.Controls.Add(this.BtnTestModbus);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seqList);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.lb_Message);
            this.Controls.Add(this.GB_GenInfo);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.lbl_title);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_Main";
            this.Text = "Frm_Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Main_Load);
            this.GB_GenInfo.ResumeLayout(false);
            this.GB_GenInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_pji;
        private System.Windows.Forms.Label lbl_wheelbase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_model;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListView seqList;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ListBox lb_Message;
        private System.Windows.Forms.GroupBox GB_GenInfo;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnTestModbus;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox txt_barcode;
        private System.Windows.Forms.Button btn_register;
    }
}