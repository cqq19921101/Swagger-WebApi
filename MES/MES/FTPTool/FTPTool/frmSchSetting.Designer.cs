namespace FTPTool
{
    partial class frmSchSetting
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSchSetting));
            this.label1 = new System.Windows.Forms.Label();
            this.cboSite = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cboRepeat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dgvStepDetail = new System.Windows.Forms.DataGridView();
            this.Step = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemoteFileFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemoteIsFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalFileFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalIsFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cms_NewAction = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSchName = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtHour = new System.Windows.Forms.TextBox();
            this.txtMinute = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lbHour = new System.Windows.Forms.ListBox();
            this.lbTime = new System.Windows.Forms.ListBox();
            this.lkTips = new System.Windows.Forms.LinkLabel();
            this.label10 = new System.Windows.Forms.Label();
            this.chkSuccessfulMail = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboReConnectTimes = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cboReconnectInterval = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.rbRename = new System.Windows.Forms.RadioButton();
            this.rbOverride = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStepDetail)).BeginInit();
            this.cms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(16, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Step2-->Choose a site profile";
            // 
            // cboSite
            // 
            this.cboSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSite.FormattingEnabled = true;
            this.cboSite.Location = new System.Drawing.Point(16, 76);
            this.cboSite.Name = "cboSite";
            this.cboSite.Size = new System.Drawing.Size(190, 22);
            this.cboSite.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(16, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Step3-->Set the start time";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(16, 127);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(93, 22);
            this.dtpDate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(16, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "Step4-->Set the repeat minutes";
            // 
            // cboRepeat
            // 
            this.cboRepeat.FormattingEnabled = true;
            this.cboRepeat.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "1Hour",
            "2Hour",
            "3Hour",
            "12Hour",
            "1Day",
            "2Day"});
            this.cboRepeat.Location = new System.Drawing.Point(16, 178);
            this.cboRepeat.Name = "cboRepeat";
            this.cboRepeat.Size = new System.Drawing.Size(121, 22);
            this.cboRepeat.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(254, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(264, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "Step6-->Set status(Schedule is enabled)";
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(257, 73);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(77, 18);
            this.chkEnabled.TabIndex = 9;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(254, 186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(269, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "Step9-->Set mail loop (Use ; to separate)";
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(257, 207);
            this.txtMail.Multiline = true;
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(337, 43);
            this.txtMail.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(16, 236);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(194, 14);
            this.label6.TabIndex = 11;
            this.label6.Text = "Step10-->Set schedule action";
            // 
            // dgvStepDetail
            // 
            this.dgvStepDetail.AllowUserToAddRows = false;
            this.dgvStepDetail.AllowUserToDeleteRows = false;
            this.dgvStepDetail.AllowUserToResizeRows = false;
            this.dgvStepDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvStepDetail.BackgroundColor = System.Drawing.Color.White;
            this.dgvStepDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStepDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Step,
            this.Action,
            this.RemoteFileFolder,
            this.RemoteIsFolder,
            this.LocalFileFolder,
            this.LocalIsFolder});
            this.dgvStepDetail.ContextMenuStrip = this.cms;
            this.dgvStepDetail.Location = new System.Drawing.Point(16, 256);
            this.dgvStepDetail.Name = "dgvStepDetail";
            this.dgvStepDetail.ReadOnly = true;
            this.dgvStepDetail.RowHeadersVisible = false;
            this.dgvStepDetail.RowTemplate.Height = 23;
            this.dgvStepDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStepDetail.Size = new System.Drawing.Size(578, 153);
            this.dgvStepDetail.TabIndex = 14;
            this.dgvStepDetail.DoubleClick += new System.EventHandler(this.dgvStepDetail_DoubleClick);
            // 
            // Step
            // 
            this.Step.HeaderText = "Step";
            this.Step.Name = "Step";
            this.Step.ReadOnly = true;
            this.Step.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Step.Width = 42;
            // 
            // Action
            // 
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            this.Action.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Action.Width = 51;
            // 
            // RemoteFileFolder
            // 
            this.RemoteFileFolder.HeaderText = "RemoteFileFolder";
            this.RemoteFileFolder.Name = "RemoteFileFolder";
            this.RemoteFileFolder.ReadOnly = true;
            this.RemoteFileFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RemoteFileFolder.Width = 121;
            // 
            // RemoteIsFolder
            // 
            this.RemoteIsFolder.HeaderText = "RemoteIsFolder";
            this.RemoteIsFolder.Name = "RemoteIsFolder";
            this.RemoteIsFolder.ReadOnly = true;
            this.RemoteIsFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RemoteIsFolder.Width = 112;
            // 
            // LocalFileFolder
            // 
            this.LocalFileFolder.HeaderText = "LocalFileFolder";
            this.LocalFileFolder.Name = "LocalFileFolder";
            this.LocalFileFolder.ReadOnly = true;
            this.LocalFileFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LocalFileFolder.Width = 105;
            // 
            // LocalIsFolder
            // 
            this.LocalIsFolder.HeaderText = "LocalIsFolder";
            this.LocalIsFolder.Name = "LocalIsFolder";
            this.LocalIsFolder.ReadOnly = true;
            this.LocalIsFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LocalIsFolder.Width = 96;
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cms_NewAction});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(143, 26);
            // 
            // cms_NewAction
            // 
            this.cms_NewAction.Name = "cms_NewAction";
            this.cms_NewAction.Size = new System.Drawing.Size(142, 22);
            this.cms_NewAction.Text = "New Action";
            this.cms_NewAction.Click += new System.EventHandler(this.cms_NewAction_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(519, 415);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(606, 276);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 23);
            this.btnUp.TabIndex = 15;
            this.btnUp.Text = "↑";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(606, 305);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 23);
            this.btnDown.TabIndex = 16;
            this.btnDown.Text = "↓";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(606, 334);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(23, 23);
            this.btnAdd.TabIndex = 17;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(606, 363);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(23, 23);
            this.btnDel.TabIndex = 18;
            this.btnDel.Text = "╳";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(16, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(201, 14);
            this.label7.TabIndex = 19;
            this.label7.Text = "Step1-->Give a schedule name";
            // 
            // txtSchName
            // 
            this.txtSchName.Location = new System.Drawing.Point(16, 27);
            this.txtSchName.Name = "txtSchName";
            this.txtSchName.Size = new System.Drawing.Size(190, 22);
            this.txtSchName.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(521, 49);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 68);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 8F);
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(16, 415);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Double click to modfiy";
            // 
            // txtHour
            // 
            this.txtHour.BackColor = System.Drawing.Color.White;
            this.txtHour.Location = new System.Drawing.Point(115, 127);
            this.txtHour.Name = "txtHour";
            this.txtHour.ReadOnly = true;
            this.txtHour.Size = new System.Drawing.Size(25, 22);
            this.txtHour.TabIndex = 23;
            this.txtHour.Click += new System.EventHandler(this.txtHour_Click);
            // 
            // txtMinute
            // 
            this.txtMinute.BackColor = System.Drawing.Color.White;
            this.txtMinute.Location = new System.Drawing.Point(146, 127);
            this.txtMinute.Name = "txtMinute";
            this.txtMinute.ReadOnly = true;
            this.txtMinute.Size = new System.Drawing.Size(25, 22);
            this.txtMinute.TabIndex = 24;
            this.txtMinute.Click += new System.EventHandler(this.txtMinute_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(137, 130);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 14);
            this.label9.TabIndex = 25;
            this.label9.Text = ":";
            // 
            // lbHour
            // 
            this.lbHour.FormattingEnabled = true;
            this.lbHour.ItemHeight = 14;
            this.lbHour.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.lbHour.Location = new System.Drawing.Point(115, 148);
            this.lbHour.Name = "lbHour";
            this.lbHour.Size = new System.Drawing.Size(45, 130);
            this.lbHour.TabIndex = 26;
            this.lbHour.Visible = false;
            this.lbHour.Click += new System.EventHandler(this.lbHour_Click);
            // 
            // lbTime
            // 
            this.lbTime.FormattingEnabled = true;
            this.lbTime.ItemHeight = 14;
            this.lbTime.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.lbTime.Location = new System.Drawing.Point(146, 148);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(45, 130);
            this.lbTime.TabIndex = 27;
            this.lbTime.Visible = false;
            this.lbTime.Click += new System.EventHandler(this.lbTime_Click);
            // 
            // lkTips
            // 
            this.lkTips.AutoSize = true;
            this.lkTips.Location = new System.Drawing.Point(145, 181);
            this.lkTips.Name = "lkTips";
            this.lkTips.Size = new System.Drawing.Size(32, 14);
            this.lkTips.TabIndex = 28;
            this.lkTips.TabStop = true;
            this.lkTips.Text = "Tips";
            this.lkTips.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkTips_LinkClicked);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(254, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(229, 14);
            this.label10.TabIndex = 29;
            this.label10.Text = "Step7-->Send mail while successful";
            // 
            // chkSuccessfulMail
            // 
            this.chkSuccessfulMail.AutoSize = true;
            this.chkSuccessfulMail.Location = new System.Drawing.Point(257, 117);
            this.chkSuccessfulMail.Name = "chkSuccessfulMail";
            this.chkSuccessfulMail.Size = new System.Drawing.Size(155, 18);
            this.chkSuccessfulMail.TabIndex = 10;
            this.chkSuccessfulMail.Text = "Send successful mail";
            this.chkSuccessfulMail.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(254, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(247, 14);
            this.label11.TabIndex = 30;
            this.label11.Text = "Step5-->Reconnect times and inverval";
            // 
            // cboReConnectTimes
            // 
            this.cboReConnectTimes.FormattingEnabled = true;
            this.cboReConnectTimes.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cboReConnectTimes.Location = new System.Drawing.Point(257, 28);
            this.cboReConnectTimes.Name = "cboReConnectTimes";
            this.cboReConnectTimes.Size = new System.Drawing.Size(39, 22);
            this.cboReConnectTimes.TabIndex = 7;
            this.cboReConnectTimes.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(302, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 14);
            this.label12.TabIndex = 32;
            this.label12.Text = "Times,";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(356, 32);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 14);
            this.label13.TabIndex = 33;
            this.label13.Text = "Interval";
            // 
            // cboReconnectInterval
            // 
            this.cboReconnectInterval.FormattingEnabled = true;
            this.cboReconnectInterval.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cboReconnectInterval.Location = new System.Drawing.Point(418, 29);
            this.cboReconnectInterval.Name = "cboReconnectInterval";
            this.cboReconnectInterval.Size = new System.Drawing.Size(51, 22);
            this.cboReconnectInterval.TabIndex = 8;
            this.cboReconnectInterval.Text = "1";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(475, 32);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 14);
            this.label14.TabIndex = 35;
            this.label14.Text = "Minutes";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Blue;
            this.label15.Location = new System.Drawing.Point(254, 142);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(324, 14);
            this.label15.TabIndex = 36;
            this.label15.Text = "Step8-->What do you want to do while file is exist";
            // 
            // rbRename
            // 
            this.rbRename.AutoSize = true;
            this.rbRename.Location = new System.Drawing.Point(387, 159);
            this.rbRename.Name = "rbRename";
            this.rbRename.Size = new System.Drawing.Size(194, 18);
            this.rbRename.TabIndex = 12;
            this.rbRename.Text = "Rename current upload file";
            this.rbRename.UseVisualStyleBackColor = true;
            // 
            // rbOverride
            // 
            this.rbOverride.AutoSize = true;
            this.rbOverride.Checked = true;
            this.rbOverride.Location = new System.Drawing.Point(257, 159);
            this.rbOverride.Name = "rbOverride";
            this.rbOverride.Size = new System.Drawing.Size(124, 18);
            this.rbOverride.TabIndex = 11;
            this.rbOverride.TabStop = true;
            this.rbOverride.Text = "Override old file";
            this.rbOverride.UseVisualStyleBackColor = true;
            // 
            // frmSchSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 455);
            this.Controls.Add(this.rbRename);
            this.Controls.Add(this.rbOverride);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cboReconnectInterval);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cboReConnectTimes);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lbHour);
            this.Controls.Add(this.chkSuccessfulMail);
            this.Controls.Add(this.lbTime);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtMinute);
            this.Controls.Add(this.lkTips);
            this.Controls.Add(this.txtHour);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSchName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.dgvStepDetail);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtMail);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboRepeat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSite);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSchSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Schedule Setting";
            this.Load += new System.EventHandler(this.frmSchSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStepDetail)).EndInit();
            this.cms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboRepeat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgvStepDetail;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ToolStripMenuItem cms_NewAction;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Step;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemoteFileFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemoteIsFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalFileFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalIsFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSchName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtHour;
        private System.Windows.Forms.TextBox txtMinute;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox lbHour;
        private System.Windows.Forms.ListBox lbTime;
        private System.Windows.Forms.LinkLabel lkTips;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkSuccessfulMail;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboReConnectTimes;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cboReconnectInterval;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.RadioButton rbRename;
        private System.Windows.Forms.RadioButton rbOverride;
    }
}