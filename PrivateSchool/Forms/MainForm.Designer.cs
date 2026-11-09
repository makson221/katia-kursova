namespace PrivateSchool.Forms
{
    partial class MainForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDashboard = new System.Windows.Forms.TabPage();
            this.gridToday = new System.Windows.Forms.DataGridView();
            this.lblToday = new System.Windows.Forms.Label();
            this.flowCards = new System.Windows.Forms.FlowLayoutPanel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.tabStudents = new System.Windows.Forms.TabPage();
            this.listStudents = new PrivateSchool.Controls.EntityListControl();
            this.tabTeachers = new System.Windows.Forms.TabPage();
            this.listTeachers = new PrivateSchool.Controls.EntityListControl();
            this.tabCourses = new System.Windows.Forms.TabPage();
            this.listCourses = new PrivateSchool.Controls.EntityListControl();
            this.tabGroups = new System.Windows.Forms.TabPage();
            this.listGroups = new PrivateSchool.Controls.EntityListControl();
            this.tabSchedule = new System.Windows.Forms.TabPage();
            this.listSchedule = new PrivateSchool.Controls.EntityListControl();
            this.tabPayments = new System.Windows.Forms.TabPage();
            this.listPayments = new PrivateSchool.Controls.EntityListControl();
            this.tabGrades = new System.Windows.Forms.TabPage();
            this.listGrades = new PrivateSchool.Controls.EntityListControl();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.gridReport = new System.Windows.Forms.DataGridView();
            this.lblSummary = new System.Windows.Forms.Label();
            this.panelReportTop = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnBuild = new System.Windows.Forms.Button();
            this.cmbReport = new System.Windows.Forms.ComboBox();
            this.lblReport = new System.Windows.Forms.Label();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.listUsers = new PrivateSchool.Controls.EntityListControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabDashboard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridToday)).BeginInit();
            this.tabStudents.SuspendLayout();
            this.tabTeachers.SuspendLayout();
            this.tabCourses.SuspendLayout();
            this.tabGroups.SuspendLayout();
            this.tabSchedule.SuspendLayout();
            this.tabPayments.SuspendLayout();
            this.tabGrades.SuspendLayout();
            this.tabReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).BeginInit();
            this.panelReportTop.SuspendLayout();
            this.tabUsers.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip
            //
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip.TabIndex = 0;
            //
            // mnuFile
            //
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSave,
            this.mnuExport,
            this.mnuSep1,
            this.mnuLogout,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(48, 20);
            this.mnuFile.Text = "&Файл";
            //
            // mnuSave
            //
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSave.Size = new System.Drawing.Size(290, 22);
            this.mnuSave.Text = "Зберегти дані";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            //
            // mnuExport
            //
            this.mnuExport.Name = "mnuExport";
            this.mnuExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.mnuExport.Size = new System.Drawing.Size(290, 22);
            this.mnuExport.Text = "Експорт поточної таблиці в CSV...";
            this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click);
            //
            // mnuSep1
            //
            this.mnuSep1.Name = "mnuSep1";
            this.mnuSep1.Size = new System.Drawing.Size(287, 6);
            //
            // mnuLogout
            //
            this.mnuLogout.Name = "mnuLogout";
            this.mnuLogout.Size = new System.Drawing.Size(290, 22);
            this.mnuLogout.Text = "Змінити користувача";
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            //
            // mnuExit
            //
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mnuExit.Size = new System.Drawing.Size(290, 22);
            this.mnuExit.Text = "Вихід";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            //
            // mnuHelp
            //
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(65, 20);
            this.mnuHelp.Text = "&Довідка";
            //
            // mnuAbout
            //
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mnuAbout.Size = new System.Drawing.Size(180, 22);
            this.mnuAbout.Text = "Про програму";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.tabDashboard);
            this.tabControl.Controls.Add(this.tabStudents);
            this.tabControl.Controls.Add(this.tabTeachers);
            this.tabControl.Controls.Add(this.tabCourses);
            this.tabControl.Controls.Add(this.tabGroups);
            this.tabControl.Controls.Add(this.tabSchedule);
            this.tabControl.Controls.Add(this.tabPayments);
            this.tabControl.Controls.Add(this.tabGrades);
            this.tabControl.Controls.Add(this.tabReports);
            this.tabControl.Controls.Add(this.tabUsers);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(12, 5);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1184, 615);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            //
            // tabDashboard
            //
            this.tabDashboard.Controls.Add(this.gridToday);
            this.tabDashboard.Controls.Add(this.lblToday);
            this.tabDashboard.Controls.Add(this.flowCards);
            this.tabDashboard.Controls.Add(this.lblWelcome);
            this.tabDashboard.Location = new System.Drawing.Point(4, 28);
            this.tabDashboard.Name = "tabDashboard";
            this.tabDashboard.Padding = new System.Windows.Forms.Padding(10);
            this.tabDashboard.Size = new System.Drawing.Size(1176, 583);
            this.tabDashboard.TabIndex = 0;
            this.tabDashboard.Text = "Головна";
            this.tabDashboard.UseVisualStyleBackColor = true;
            //
            // gridToday
            //
            this.gridToday.AllowUserToAddRows = false;
            this.gridToday.AllowUserToDeleteRows = false;
            this.gridToday.AllowUserToResizeRows = false;
            this.gridToday.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridToday.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridToday.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridToday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridToday.Location = new System.Drawing.Point(10, 288);
            this.gridToday.MultiSelect = false;
            this.gridToday.Name = "gridToday";
            this.gridToday.ReadOnly = true;
            this.gridToday.RowHeadersVisible = false;
            this.gridToday.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridToday.Size = new System.Drawing.Size(1156, 285);
            this.gridToday.TabIndex = 3;
            //
            // lblToday
            //
            this.lblToday.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblToday.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblToday.Location = new System.Drawing.Point(10, 254);
            this.lblToday.Name = "lblToday";
            this.lblToday.Size = new System.Drawing.Size(1156, 34);
            this.lblToday.TabIndex = 2;
            this.lblToday.Text = "Розклад на сьогодні";
            this.lblToday.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // flowCards
            //
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowCards.Location = new System.Drawing.Point(10, 56);
            this.flowCards.Name = "flowCards";
            this.flowCards.Size = new System.Drawing.Size(1156, 198);
            this.flowCards.TabIndex = 1;
            //
            // lblWelcome
            //
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(153)))));
            this.lblWelcome.Location = new System.Drawing.Point(10, 10);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(1156, 46);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Вітаємо!";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tabStudents
            //
            this.tabStudents.Controls.Add(this.listStudents);
            this.tabStudents.Location = new System.Drawing.Point(4, 28);
            this.tabStudents.Name = "tabStudents";
            this.tabStudents.Padding = new System.Windows.Forms.Padding(3);
            this.tabStudents.Size = new System.Drawing.Size(1176, 583);
            this.tabStudents.TabIndex = 1;
            this.tabStudents.Text = "Учні";
            this.tabStudents.UseVisualStyleBackColor = true;
            //
            // listStudents
            //
            this.listStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listStudents.FilterVisible = true;
            this.listStudents.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listStudents.Location = new System.Drawing.Point(3, 3);
            this.listStudents.Name = "listStudents";
            this.listStudents.Size = new System.Drawing.Size(1170, 577);
            this.listStudents.TabIndex = 0;
            //
            // tabTeachers
            //
            this.tabTeachers.Controls.Add(this.listTeachers);
            this.tabTeachers.Location = new System.Drawing.Point(4, 28);
            this.tabTeachers.Name = "tabTeachers";
            this.tabTeachers.Padding = new System.Windows.Forms.Padding(3);
            this.tabTeachers.Size = new System.Drawing.Size(1176, 583);
            this.tabTeachers.TabIndex = 2;
            this.tabTeachers.Text = "Викладачі";
            this.tabTeachers.UseVisualStyleBackColor = true;
            //
            // listTeachers
            //
            this.listTeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTeachers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listTeachers.Location = new System.Drawing.Point(3, 3);
            this.listTeachers.Name = "listTeachers";
            this.listTeachers.Size = new System.Drawing.Size(1170, 577);
            this.listTeachers.TabIndex = 0;
            //
            // tabCourses
            //
            this.tabCourses.Controls.Add(this.listCourses);
            this.tabCourses.Location = new System.Drawing.Point(4, 28);
            this.tabCourses.Name = "tabCourses";
            this.tabCourses.Padding = new System.Windows.Forms.Padding(3);
            this.tabCourses.Size = new System.Drawing.Size(1176, 583);
            this.tabCourses.TabIndex = 3;
            this.tabCourses.Text = "Курси";
            this.tabCourses.UseVisualStyleBackColor = true;
            //
            // listCourses
            //
            this.listCourses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listCourses.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listCourses.Location = new System.Drawing.Point(3, 3);
            this.listCourses.Name = "listCourses";
            this.listCourses.Size = new System.Drawing.Size(1170, 577);
            this.listCourses.TabIndex = 0;
            //
            // tabGroups
            //
            this.tabGroups.Controls.Add(this.listGroups);
            this.tabGroups.Location = new System.Drawing.Point(4, 28);
            this.tabGroups.Name = "tabGroups";
            this.tabGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroups.Size = new System.Drawing.Size(1176, 583);
            this.tabGroups.TabIndex = 4;
            this.tabGroups.Text = "Групи";
            this.tabGroups.UseVisualStyleBackColor = true;
            //
            // listGroups
            //
            this.listGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listGroups.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listGroups.Location = new System.Drawing.Point(3, 3);
            this.listGroups.Name = "listGroups";
            this.listGroups.Size = new System.Drawing.Size(1170, 577);
            this.listGroups.TabIndex = 0;
            //
            // tabSchedule
            //
            this.tabSchedule.Controls.Add(this.listSchedule);
            this.tabSchedule.Location = new System.Drawing.Point(4, 28);
            this.tabSchedule.Name = "tabSchedule";
            this.tabSchedule.Padding = new System.Windows.Forms.Padding(3);
            this.tabSchedule.Size = new System.Drawing.Size(1176, 583);
            this.tabSchedule.TabIndex = 5;
            this.tabSchedule.Text = "Розклад";
            this.tabSchedule.UseVisualStyleBackColor = true;
            //
            // listSchedule
            //
            this.listSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSchedule.FilterVisible = true;
            this.listSchedule.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listSchedule.Location = new System.Drawing.Point(3, 3);
            this.listSchedule.Name = "listSchedule";
            this.listSchedule.Size = new System.Drawing.Size(1170, 577);
            this.listSchedule.TabIndex = 0;
            //
            // tabPayments
            //
            this.tabPayments.Controls.Add(this.listPayments);
            this.tabPayments.Location = new System.Drawing.Point(4, 28);
            this.tabPayments.Name = "tabPayments";
            this.tabPayments.Padding = new System.Windows.Forms.Padding(3);
            this.tabPayments.Size = new System.Drawing.Size(1176, 583);
            this.tabPayments.TabIndex = 6;
            this.tabPayments.Text = "Оплати";
            this.tabPayments.UseVisualStyleBackColor = true;
            //
            // listPayments
            //
            this.listPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPayments.FilterCaption = "Місяць:";
            this.listPayments.FilterVisible = true;
            this.listPayments.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listPayments.Location = new System.Drawing.Point(3, 3);
            this.listPayments.Name = "listPayments";
            this.listPayments.Size = new System.Drawing.Size(1170, 577);
            this.listPayments.TabIndex = 0;
            //
            // tabGrades
            //
            this.tabGrades.Controls.Add(this.listGrades);
            this.tabGrades.Location = new System.Drawing.Point(4, 28);
            this.tabGrades.Name = "tabGrades";
            this.tabGrades.Padding = new System.Windows.Forms.Padding(3);
            this.tabGrades.Size = new System.Drawing.Size(1176, 583);
            this.tabGrades.TabIndex = 7;
            this.tabGrades.Text = "Успішність";
            this.tabGrades.UseVisualStyleBackColor = true;
            //
            // listGrades
            //
            this.listGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listGrades.FilterVisible = true;
            this.listGrades.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listGrades.Location = new System.Drawing.Point(3, 3);
            this.listGrades.Name = "listGrades";
            this.listGrades.Size = new System.Drawing.Size(1170, 577);
            this.listGrades.TabIndex = 0;
            //
            // tabReports
            //
            this.tabReports.Controls.Add(this.gridReport);
            this.tabReports.Controls.Add(this.lblSummary);
            this.tabReports.Controls.Add(this.panelReportTop);
            this.tabReports.Location = new System.Drawing.Point(4, 28);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabReports.Size = new System.Drawing.Size(1176, 583);
            this.tabReports.TabIndex = 8;
            this.tabReports.Text = "Звіти";
            this.tabReports.UseVisualStyleBackColor = true;
            //
            // gridReport
            //
            this.gridReport.AllowUserToAddRows = false;
            this.gridReport.AllowUserToDeleteRows = false;
            this.gridReport.AllowUserToResizeRows = false;
            this.gridReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReport.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReport.Location = new System.Drawing.Point(3, 49);
            this.gridReport.MultiSelect = false;
            this.gridReport.Name = "gridReport";
            this.gridReport.ReadOnly = true;
            this.gridReport.RowHeadersVisible = false;
            this.gridReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReport.Size = new System.Drawing.Size(1170, 499);
            this.gridReport.TabIndex = 1;
            //
            // lblSummary
            //
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblSummary.Location = new System.Drawing.Point(3, 548);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblSummary.Size = new System.Drawing.Size(1170, 32);
            this.lblSummary.TabIndex = 2;
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panelReportTop
            //
            this.panelReportTop.Controls.Add(this.btnExport);
            this.panelReportTop.Controls.Add(this.btnBuild);
            this.panelReportTop.Controls.Add(this.cmbReport);
            this.panelReportTop.Controls.Add(this.lblReport);
            this.panelReportTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelReportTop.Location = new System.Drawing.Point(3, 3);
            this.panelReportTop.Name = "panelReportTop";
            this.panelReportTop.Size = new System.Drawing.Size(1170, 46);
            this.panelReportTop.TabIndex = 0;
            //
            // btnExport
            //
            this.btnExport.Location = new System.Drawing.Point(584, 9);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(130, 28);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Експорт у CSV...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            //
            // btnBuild
            //
            this.btnBuild.Location = new System.Drawing.Point(458, 9);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(120, 28);
            this.btnBuild.TabIndex = 2;
            this.btnBuild.Text = "Сформувати";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            //
            // cmbReport
            //
            this.cmbReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReport.FormattingEnabled = true;
            this.cmbReport.Location = new System.Drawing.Point(62, 12);
            this.cmbReport.Name = "cmbReport";
            this.cmbReport.Size = new System.Drawing.Size(384, 23);
            this.cmbReport.TabIndex = 1;
            this.cmbReport.SelectedIndexChanged += new System.EventHandler(this.btnBuild_Click);
            //
            // lblReport
            //
            this.lblReport.AutoSize = true;
            this.lblReport.Location = new System.Drawing.Point(8, 15);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(35, 15);
            this.lblReport.TabIndex = 0;
            this.lblReport.Text = "Звіт:";
            //
            // tabUsers
            //
            this.tabUsers.Controls.Add(this.listUsers);
            this.tabUsers.Location = new System.Drawing.Point(4, 28);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsers.Size = new System.Drawing.Size(1176, 583);
            this.tabUsers.TabIndex = 9;
            this.tabUsers.Text = "Користувачі";
            this.tabUsers.UseVisualStyleBackColor = true;
            //
            // listUsers
            //
            this.listUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listUsers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listUsers.Location = new System.Drawing.Point(3, 3);
            this.listUsers.Name = "listUsers";
            this.listUsers.Size = new System.Drawing.Size(1170, 577);
            this.listUsers.TabIndex = 0;
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusUser,
            this.lblStatusFile,
            this.lblStatusDate});
            this.statusStrip.Location = new System.Drawing.Point(0, 639);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip.TabIndex = 2;
            //
            // lblStatusUser
            //
            this.lblStatusUser.Name = "lblStatusUser";
            this.lblStatusUser.Size = new System.Drawing.Size(83, 17);
            this.lblStatusUser.Text = "Користувач: —";
            //
            // lblStatusFile
            //
            this.lblStatusFile.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblStatusFile.Name = "lblStatusFile";
            this.lblStatusFile.Size = new System.Drawing.Size(1000, 17);
            this.lblStatusFile.Spring = true;
            this.lblStatusFile.Text = "Дані: —";
            this.lblStatusFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblStatusDate
            //
            this.lblStatusDate.Name = "lblStatusDate";
            this.lblStatusDate.Size = new System.Drawing.Size(65, 17);
            this.lblStatusDate.Text = "01.01.2026";
            //
            // saveFileDialog
            //
            this.saveFileDialog.DefaultExt = "csv";
            this.saveFileDialog.Filter = "CSV-файли (*.csv)|*.csv|Усі файли (*.*)|*.*";
            this.saveFileDialog.Title = "Експорт у CSV";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Автоматизована система приватного навчального закладу";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabDashboard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridToday)).EndInit();
            this.tabStudents.ResumeLayout(false);
            this.tabTeachers.ResumeLayout(false);
            this.tabCourses.ResumeLayout(false);
            this.tabGroups.ResumeLayout(false);
            this.tabSchedule.ResumeLayout(false);
            this.tabPayments.ResumeLayout(false);
            this.tabGrades.ResumeLayout(false);
            this.tabReports.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).EndInit();
            this.panelReportTop.ResumeLayout(false);
            this.panelReportTop.PerformLayout();
            this.tabUsers.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuExport;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabDashboard;
        private System.Windows.Forms.DataGridView gridToday;
        private System.Windows.Forms.Label lblToday;
        private System.Windows.Forms.FlowLayoutPanel flowCards;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TabPage tabStudents;
        private PrivateSchool.Controls.EntityListControl listStudents;
        private System.Windows.Forms.TabPage tabTeachers;
        private PrivateSchool.Controls.EntityListControl listTeachers;
        private System.Windows.Forms.TabPage tabCourses;
        private PrivateSchool.Controls.EntityListControl listCourses;
        private System.Windows.Forms.TabPage tabGroups;
        private PrivateSchool.Controls.EntityListControl listGroups;
        private System.Windows.Forms.TabPage tabSchedule;
        private PrivateSchool.Controls.EntityListControl listSchedule;
        private System.Windows.Forms.TabPage tabPayments;
        private PrivateSchool.Controls.EntityListControl listPayments;
        private System.Windows.Forms.TabPage tabGrades;
        private PrivateSchool.Controls.EntityListControl listGrades;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.DataGridView gridReport;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Panel panelReportTop;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.ComboBox cmbReport;
        private System.Windows.Forms.Label lblReport;
        private System.Windows.Forms.TabPage tabUsers;
        private PrivateSchool.Controls.EntityListControl listUsers;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusUser;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusFile;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusDate;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
