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
            this.label19 = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.seqList = new System.Windows.Forms.ListView();
            this.label29 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label30 = new System.Windows.Forms.Label();
            this.lb_Message = new System.Windows.Forms.ListBox();
            this.GB_GenInfo = new System.Windows.Forms.GroupBox();
            this.BtnStart = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnTestModbus = new System.Windows.Forms.Button();
            this.BtnRegister = new System.Windows.Forms.Button();
            this.GB_GenInfo.SuspendLayout();
            this.SuspendLayout();
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
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label12.Font = new System.Drawing.Font("Arial", 30F);
            this.label12.Location = new System.Drawing.Point(441, 460);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(640, 95);
            this.label12.TabIndex = 22;
            this.label12.Text = "PP 039096538510510";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label11.Font = new System.Drawing.Font("Arial", 30F);
            this.label11.Location = new System.Drawing.Point(441, 350);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(640, 95);
            this.label11.TabIndex = 21;
            this.label11.Text = "0930365";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Font = new System.Drawing.Font("Arial", 30F);
            this.label9.Location = new System.Drawing.Point(441, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(640, 95);
            this.label9.TabIndex = 19;
            this.label9.Text = "385";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("Arial", 30F);
            this.label8.Location = new System.Drawing.Point(441, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(640, 95);
            this.label8.TabIndex = 18;
            this.label8.Text = "2345";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Arial", 30F);
            this.label6.Location = new System.Drawing.Point(18, 460);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(336, 95);
            this.label6.TabIndex = 16;
            this.label6.Text = "Barcode";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 30F);
            this.label5.Location = new System.Drawing.Point(18, 350);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(336, 95);
            this.label5.TabIndex = 15;
            this.label5.Text = "PJI";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Arial", 30F);
            this.label7.Location = new System.Drawing.Point(441, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(640, 95);
            this.label7.TabIndex = 17;
            this.label7.Text = "BBB PH2";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 30F);
            this.label3.Location = new System.Drawing.Point(18, 241);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(336, 95);
            this.label3.TabIndex = 13;
            this.label3.Text = "PARA Cycle";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.label13.Text = "Car description";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // seqList
            // 
            this.seqList.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.seqList.BackColor = System.Drawing.Color.SeaShell;
            this.seqList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.seqList.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seqList.FullRowSelect = true;
            this.seqList.GridLines = true;
            this.seqList.HideSelection = false;
            this.seqList.Location = new System.Drawing.Point(2, 95);
            this.seqList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.seqList.MultiSelect = false;
            this.seqList.Name = "seqList";
            this.seqList.Size = new System.Drawing.Size(913, 781);
            this.seqList.TabIndex = 170;
            this.seqList.UseCompatibleStateImageBehavior = false;
            this.seqList.View = System.Windows.Forms.View.List;
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
            this.GB_GenInfo.Controls.Add(this.label12);
            this.GB_GenInfo.Controls.Add(this.label11);
            this.GB_GenInfo.Controls.Add(this.label9);
            this.GB_GenInfo.Controls.Add(this.label8);
            this.GB_GenInfo.Controls.Add(this.label7);
            this.GB_GenInfo.Controls.Add(this.label6);
            this.GB_GenInfo.Controls.Add(this.label5);
            this.GB_GenInfo.Controls.Add(this.label3);
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
            this.BtnTestModbus.Click += new System.EventHandler(this.BtnTestModbus_Click);
            // 
            // BtnRegister
            // 
            this.BtnRegister.BackColor = System.Drawing.Color.Olive;
            this.BtnRegister.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.BtnRegister.Location = new System.Drawing.Point(921, 831);
            this.BtnRegister.Name = "BtnRegister";
            this.BtnRegister.Size = new System.Drawing.Size(300, 138);
            this.BtnRegister.TabIndex = 175;
            this.BtnRegister.Text = "Register";
            this.BtnRegister.UseVisualStyleBackColor = false;
            this.BtnRegister.Click += new System.EventHandler(this.BtnRegister_Click);
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2029, 1179);
            this.Controls.Add(this.BtnRegister);
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
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_Main";
            this.Text = "Frm_Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Main_Load);
            this.GB_GenInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
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
        private System.Windows.Forms.Button BtnRegister;
    }
}