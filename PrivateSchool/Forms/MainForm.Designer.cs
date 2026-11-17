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
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.panelBrand = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblAppSub = new System.Windows.Forms.Label();
            this.panelNav = new System.Windows.Forms.Panel();
            this.lblMenuCaption = new System.Windows.Forms.Label();
            this.navDashboard = new PrivateSchool.Controls.SidebarButton();
            this.navStudents = new PrivateSchool.Controls.SidebarButton();
            this.navTeachers = new PrivateSchool.Controls.SidebarButton();
            this.navCourses = new PrivateSchool.Controls.SidebarButton();
            this.navGroups = new PrivateSchool.Controls.SidebarButton();
            this.navSchedule = new PrivateSchool.Controls.SidebarButton();
            this.navPayments = new PrivateSchool.Controls.SidebarButton();
            this.navGrades = new PrivateSchool.Controls.SidebarButton();
            this.navReports = new PrivateSchool.Controls.SidebarButton();
            this.navUsers = new PrivateSchool.Controls.SidebarButton();
            this.panelUser = new System.Windows.Forms.Panel();
            this.lblUserLine = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.navLogout = new PrivateSchool.Controls.SidebarButton();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblPageSubtitle = new System.Windows.Forms.Label();
            this.btnHeaderExport = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.pageDashboard = new System.Windows.Forms.Panel();
            this.tableDashboard = new System.Windows.Forms.TableLayoutPanel();
            this.tableCards = new System.Windows.Forms.TableLayoutPanel();
            this.card1 = new PrivateSchool.Controls.StatCard();
            this.card2 = new PrivateSchool.Controls.StatCard();
            this.card3 = new PrivateSchool.Controls.StatCard();
            this.card4 = new PrivateSchool.Controls.StatCard();
            this.card5 = new PrivateSchool.Controls.StatCard();
            this.card6 = new PrivateSchool.Controls.StatCard();
            this.card7 = new PrivateSchool.Controls.StatCard();
            this.card8 = new PrivateSchool.Controls.StatCard();
            this.tableBottom = new System.Windows.Forms.TableLayoutPanel();
            this.cardChart = new PrivateSchool.Controls.CardPanel();
            this.chartIncome = new PrivateSchool.Controls.BarChart();
            this.lblChartSubtitle = new System.Windows.Forms.Label();
            this.lblChartTitle = new System.Windows.Forms.Label();
            this.cardToday = new PrivateSchool.Controls.CardPanel();
            this.gridToday = new System.Windows.Forms.DataGridView();
            this.lblTodaySubtitle = new System.Windows.Forms.Label();
            this.lblToday = new System.Windows.Forms.Label();
            this.pageStudents = new System.Windows.Forms.Panel();
            this.listStudents = new PrivateSchool.Controls.EntityListControl();
            this.pageTeachers = new System.Windows.Forms.Panel();
            this.listTeachers = new PrivateSchool.Controls.EntityListControl();
            this.pageCourses = new System.Windows.Forms.Panel();
            this.listCourses = new PrivateSchool.Controls.EntityListControl();
            this.pageGroups = new System.Windows.Forms.Panel();
            this.listGroups = new PrivateSchool.Controls.EntityListControl();
            this.pageSchedule = new System.Windows.Forms.Panel();
            this.listSchedule = new PrivateSchool.Controls.EntityListControl();
            this.pagePayments = new System.Windows.Forms.Panel();
            this.listPayments = new PrivateSchool.Controls.EntityListControl();
            this.pageGrades = new System.Windows.Forms.Panel();
            this.listGrades = new PrivateSchool.Controls.EntityListControl();
            this.pageUsers = new System.Windows.Forms.Panel();
            this.listUsers = new PrivateSchool.Controls.EntityListControl();
            this.pageReports = new System.Windows.Forms.Panel();
            this.cardReport = new PrivateSchool.Controls.CardPanel();
            this.gridReport = new System.Windows.Forms.DataGridView();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.lblSummary = new System.Windows.Forms.Label();
            this.panelReportTop = new System.Windows.Forms.Panel();
            this.lblReport = new System.Windows.Forms.Label();
            this.cmbReport = new System.Windows.Forms.ComboBox();
            this.btnBuild = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panelSidebar.SuspendLayout();
            this.panelBrand.SuspendLayout();
            this.panelNav.SuspendLayout();
            this.panelUser.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.pageDashboard.SuspendLayout();
            this.tableDashboard.SuspendLayout();
            this.tableCards.SuspendLayout();
            this.tableBottom.SuspendLayout();
            this.cardChart.SuspendLayout();
            this.cardToday.SuspendLayout();
            this.pageStudents.SuspendLayout();
            this.pageTeachers.SuspendLayout();
            this.pageCourses.SuspendLayout();
            this.pageGroups.SuspendLayout();
            this.pageSchedule.SuspendLayout();
            this.pagePayments.SuspendLayout();
            this.pageGrades.SuspendLayout();
            this.pageUsers.SuspendLayout();
            this.pageReports.SuspendLayout();
            this.cardReport.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.panelReportTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridToday)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.panelSidebar.Controls.Add(this.panelNav);
            this.panelSidebar.Controls.Add(this.panelUser);
            this.panelSidebar.Controls.Add(this.panelBrand);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(232, 780);
            this.panelSidebar.TabIndex = 2;
            // 
            // panelBrand
            // 
            this.panelBrand.Controls.Add(this.lblAppSub);
            this.panelBrand.Controls.Add(this.lblAppName);
            this.panelBrand.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBrand.Location = new System.Drawing.Point(0, 0);
            this.panelBrand.Name = "panelBrand";
            this.panelBrand.Size = new System.Drawing.Size(232, 84);
            this.panelBrand.TabIndex = 0;
            this.panelBrand.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBrand_Paint);
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.Location = new System.Drawing.Point(70, 20);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(84, 25);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "Знання+";
            // 
            // lblAppSub
            // 
            this.lblAppSub.AutoSize = true;
            this.lblAppSub.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblAppSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblAppSub.Location = new System.Drawing.Point(72, 46);
            this.lblAppSub.Name = "lblAppSub";
            this.lblAppSub.Size = new System.Drawing.Size(130, 13);
            this.lblAppSub.TabIndex = 1;
            this.lblAppSub.Text = "Система управління";
            // 
            // panelNav
            // 
            this.panelNav.Controls.Add(this.navUsers);
            this.panelNav.Controls.Add(this.navReports);
            this.panelNav.Controls.Add(this.navGrades);
            this.panelNav.Controls.Add(this.navPayments);
            this.panelNav.Controls.Add(this.navSchedule);
            this.panelNav.Controls.Add(this.navGroups);
            this.panelNav.Controls.Add(this.navCourses);
            this.panelNav.Controls.Add(this.navTeachers);
            this.panelNav.Controls.Add(this.navStudents);
            this.panelNav.Controls.Add(this.navDashboard);
            this.panelNav.Controls.Add(this.lblMenuCaption);
            this.panelNav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNav.Location = new System.Drawing.Point(0, 84);
            this.panelNav.Name = "panelNav";
            this.panelNav.Size = new System.Drawing.Size(232, 588);
            this.panelNav.TabIndex = 1;
            // 
            // lblMenuCaption
            // 
            this.lblMenuCaption.AutoSize = true;
            this.lblMenuCaption.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblMenuCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblMenuCaption.Location = new System.Drawing.Point(22, 6);
            this.lblMenuCaption.Name = "lblMenuCaption";
            this.lblMenuCaption.Size = new System.Drawing.Size(40, 13);
            this.lblMenuCaption.TabIndex = 0;
            this.lblMenuCaption.Text = "МЕНЮ";
            // 
            // navDashboard
            // 
            this.navDashboard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navDashboard.IconGlyph = "\uE80F";
            this.navDashboard.Location = new System.Drawing.Point(0, 26);
            this.navDashboard.Name = "navDashboard";
            this.navDashboard.Size = new System.Drawing.Size(232, 42);
            this.navDashboard.TabIndex = 1;
            this.navDashboard.Text = "Головна";
            this.navDashboard.Click += new System.EventHandler(this.nav_Click);
            // 
            // navStudents
            // 
            this.navStudents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navStudents.IconGlyph = "\uE77B";
            this.navStudents.Location = new System.Drawing.Point(0, 70);
            this.navStudents.Name = "navStudents";
            this.navStudents.Size = new System.Drawing.Size(232, 42);
            this.navStudents.TabIndex = 2;
            this.navStudents.Text = "Учні";
            this.navStudents.Click += new System.EventHandler(this.nav_Click);
            // 
            // navTeachers
            // 
            this.navTeachers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navTeachers.IconGlyph = "\uE7BE";
            this.navTeachers.Location = new System.Drawing.Point(0, 114);
            this.navTeachers.Name = "navTeachers";
            this.navTeachers.Size = new System.Drawing.Size(232, 42);
            this.navTeachers.TabIndex = 3;
            this.navTeachers.Text = "Викладачі";
            this.navTeachers.Click += new System.EventHandler(this.nav_Click);
            // 
            // navCourses
            // 
            this.navCourses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navCourses.IconGlyph = "\uE8F1";
            this.navCourses.Location = new System.Drawing.Point(0, 158);
            this.navCourses.Name = "navCourses";
            this.navCourses.Size = new System.Drawing.Size(232, 42);
            this.navCourses.TabIndex = 4;
            this.navCourses.Text = "Курси";
            this.navCourses.Click += new System.EventHandler(this.nav_Click);
            // 
            // navGroups
            // 
            this.navGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navGroups.IconGlyph = "\uE716";
            this.navGroups.Location = new System.Drawing.Point(0, 202);
            this.navGroups.Name = "navGroups";
            this.navGroups.Size = new System.Drawing.Size(232, 42);
            this.navGroups.TabIndex = 5;
            this.navGroups.Text = "Групи";
            this.navGroups.Click += new System.EventHandler(this.nav_Click);
            // 
            // navSchedule
            // 
            this.navSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navSchedule.IconGlyph = "\uE787";
            this.navSchedule.Location = new System.Drawing.Point(0, 246);
            this.navSchedule.Name = "navSchedule";
            this.navSchedule.Size = new System.Drawing.Size(232, 42);
            this.navSchedule.TabIndex = 6;
            this.navSchedule.Text = "Розклад";
            this.navSchedule.Click += new System.EventHandler(this.nav_Click);
            // 
            // navPayments
            // 
            this.navPayments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navPayments.IconGlyph = "\uE8C7";
            this.navPayments.Location = new System.Drawing.Point(0, 290);
            this.navPayments.Name = "navPayments";
            this.navPayments.Size = new System.Drawing.Size(232, 42);
            this.navPayments.TabIndex = 7;
            this.navPayments.Text = "Оплати";
            this.navPayments.Click += new System.EventHandler(this.nav_Click);
            // 
            // navGrades
            // 
            this.navGrades.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navGrades.IconGlyph = "\uE734";
            this.navGrades.Location = new System.Drawing.Point(0, 334);
            this.navGrades.Name = "navGrades";
            this.navGrades.Size = new System.Drawing.Size(232, 42);
            this.navGrades.TabIndex = 8;
            this.navGrades.Text = "Успішність";
            this.navGrades.Click += new System.EventHandler(this.nav_Click);
            // 
            // navReports
            // 
            this.navReports.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navReports.IconGlyph = "\uE9F9";
            this.navReports.Location = new System.Drawing.Point(0, 378);
            this.navReports.Name = "navReports";
            this.navReports.Size = new System.Drawing.Size(232, 42);
            this.navReports.TabIndex = 9;
            this.navReports.Text = "Звіти";
            this.navReports.Click += new System.EventHandler(this.nav_Click);
            // 
            // navUsers
            // 
            this.navUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navUsers.IconGlyph = "\uE8D7";
            this.navUsers.Location = new System.Drawing.Point(0, 422);
            this.navUsers.Name = "navUsers";
            this.navUsers.Size = new System.Drawing.Size(232, 42);
            this.navUsers.TabIndex = 10;
            this.navUsers.Text = "Користувачі";
            this.navUsers.Click += new System.EventHandler(this.nav_Click);
            // 
            // panelUser
            // 
            this.panelUser.Controls.Add(this.navLogout);
            this.panelUser.Controls.Add(this.lblUserRole);
            this.panelUser.Controls.Add(this.lblUserName);
            this.panelUser.Controls.Add(this.lblUserLine);
            this.panelUser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelUser.Location = new System.Drawing.Point(0, 672);
            this.panelUser.Name = "panelUser";
            this.panelUser.Size = new System.Drawing.Size(232, 108);
            this.panelUser.TabIndex = 2;
            // 
            // lblUserLine
            // 
            this.lblUserLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblUserLine.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUserLine.Location = new System.Drawing.Point(0, 0);
            this.lblUserLine.Name = "lblUserLine";
            this.lblUserLine.Size = new System.Drawing.Size(232, 1);
            this.lblUserLine.TabIndex = 0;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoEllipsis = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(22, 14);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(196, 20);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "Користувач";
            // 
            // lblUserRole
            // 
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblUserRole.Location = new System.Drawing.Point(22, 34);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(196, 18);
            this.lblUserRole.TabIndex = 2;
            this.lblUserRole.Text = "Роль";
            // 
            // navLogout
            // 
            this.navLogout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.navLogout.IconGlyph = "\uE748";
            this.navLogout.Location = new System.Drawing.Point(0, 58);
            this.navLogout.Name = "navLogout";
            this.navLogout.Size = new System.Drawing.Size(232, 42);
            this.navLogout.TabIndex = 3;
            this.navLogout.Text = "Змінити користувача";
            this.navLogout.Click += new System.EventHandler(this.navLogout_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnAbout);
            this.panelHeader.Controls.Add(this.btnHeaderExport);
            this.panelHeader.Controls.Add(this.lblPageSubtitle);
            this.panelHeader.Controls.Add(this.lblPageTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(232, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1048, 84);
            this.panelHeader.TabIndex = 1;
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.AutoSize = true;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblPageTitle.Location = new System.Drawing.Point(22, 16);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(103, 31);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "Головна";
            // 
            // lblPageSubtitle
            // 
            this.lblPageSubtitle.AutoSize = true;
            this.lblPageSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblPageSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblPageSubtitle.Location = new System.Drawing.Point(25, 50);
            this.lblPageSubtitle.Name = "lblPageSubtitle";
            this.lblPageSubtitle.Size = new System.Drawing.Size(60, 17);
            this.lblPageSubtitle.TabIndex = 1;
            this.lblPageSubtitle.Text = "Підзаголовок";
            // 
            // btnHeaderExport
            // 
            this.btnHeaderExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHeaderExport.Location = new System.Drawing.Point(726, 24);
            this.btnHeaderExport.Name = "btnHeaderExport";
            this.btnHeaderExport.Size = new System.Drawing.Size(156, 36);
            this.btnHeaderExport.TabIndex = 2;
            this.btnHeaderExport.Text = "Експорт у CSV";
            this.btnHeaderExport.Click += new System.EventHandler(this.btnHeaderExport_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbout.Location = new System.Drawing.Point(892, 24);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(132, 36);
            this.btnAbout.TabIndex = 3;
            this.btnAbout.Text = "Про програму";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.pageReports);
            this.panelContent.Controls.Add(this.pageUsers);
            this.panelContent.Controls.Add(this.pageGrades);
            this.panelContent.Controls.Add(this.pagePayments);
            this.panelContent.Controls.Add(this.pageSchedule);
            this.panelContent.Controls.Add(this.pageGroups);
            this.panelContent.Controls.Add(this.pageCourses);
            this.panelContent.Controls.Add(this.pageTeachers);
            this.panelContent.Controls.Add(this.pageStudents);
            this.panelContent.Controls.Add(this.pageDashboard);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(232, 84);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(24, 0, 24, 24);
            this.panelContent.Size = new System.Drawing.Size(1048, 696);
            this.panelContent.TabIndex = 0;
            // 
            // pageDashboard
            // 
            this.pageDashboard.Controls.Add(this.tableDashboard);
            this.pageDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageDashboard.Location = new System.Drawing.Point(24, 0);
            this.pageDashboard.Name = "pageDashboard";
            this.pageDashboard.Size = new System.Drawing.Size(1000, 672);
            this.pageDashboard.TabIndex = 0;
            // 
            // tableDashboard
            // 
            this.tableDashboard.ColumnCount = 1;
            this.tableDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDashboard.Controls.Add(this.tableCards, 0, 0);
            this.tableDashboard.Controls.Add(this.tableBottom, 0, 1);
            this.tableDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDashboard.Location = new System.Drawing.Point(0, 0);
            this.tableDashboard.Margin = new System.Windows.Forms.Padding(0);
            this.tableDashboard.Name = "tableDashboard";
            this.tableDashboard.RowCount = 2;
            this.tableDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 248F));
            this.tableDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDashboard.Size = new System.Drawing.Size(1000, 672);
            this.tableDashboard.TabIndex = 0;
            // 
            // tableCards
            // 
            this.tableCards.ColumnCount = 4;
            this.tableCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableCards.Controls.Add(this.card1, 0, 0);
            this.tableCards.Controls.Add(this.card2, 1, 0);
            this.tableCards.Controls.Add(this.card3, 2, 0);
            this.tableCards.Controls.Add(this.card4, 3, 0);
            this.tableCards.Controls.Add(this.card5, 0, 1);
            this.tableCards.Controls.Add(this.card6, 1, 1);
            this.tableCards.Controls.Add(this.card7, 2, 1);
            this.tableCards.Controls.Add(this.card8, 3, 1);
            this.tableCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCards.Location = new System.Drawing.Point(0, 0);
            this.tableCards.Margin = new System.Windows.Forms.Padding(0);
            this.tableCards.Name = "tableCards";
            this.tableCards.RowCount = 2;
            this.tableCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCards.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCards.Size = new System.Drawing.Size(1000, 248);
            this.tableCards.TabIndex = 0;
            // 
            // card1
            // 
            this.card1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card1.Location = new System.Drawing.Point(0, 0);
            this.card1.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.card1.Name = "card1";
            this.card1.Size = new System.Drawing.Size(234, 108);
            this.card1.TabIndex = 0;
            // 
            // card2
            // 
            this.card2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card2.Location = new System.Drawing.Point(250, 0);
            this.card2.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.card2.Name = "card2";
            this.card2.Size = new System.Drawing.Size(234, 108);
            this.card2.TabIndex = 1;
            // 
            // card3
            // 
            this.card3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card3.Location = new System.Drawing.Point(500, 0);
            this.card3.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.card3.Name = "card3";
            this.card3.Size = new System.Drawing.Size(234, 108);
            this.card3.TabIndex = 2;
            // 
            // card4
            // 
            this.card4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card4.Location = new System.Drawing.Point(750, 0);
            this.card4.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.card4.Name = "card4";
            this.card4.Size = new System.Drawing.Size(250, 108);
            this.card4.TabIndex = 3;
            // 
            // card5
            // 
            this.card5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card5.Location = new System.Drawing.Point(0, 124);
            this.card5.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.card5.Name = "card5";
            this.card5.Size = new System.Drawing.Size(234, 108);
            this.card5.TabIndex = 4;
            // 
            // card6
            // 
            this.card6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card6.Location = new System.Drawing.Point(250, 124);
            this.card6.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.card6.Name = "card6";
            this.card6.Size = new System.Drawing.Size(234, 108);
            this.card6.TabIndex = 5;
            // 
            // card7
            // 
            this.card7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card7.Location = new System.Drawing.Point(500, 124);
            this.card7.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.card7.Name = "card7";
            this.card7.Size = new System.Drawing.Size(234, 108);
            this.card7.TabIndex = 6;
            // 
            // card8
            // 
            this.card8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.card8.Location = new System.Drawing.Point(750, 124);
            this.card8.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.card8.Name = "card8";
            this.card8.Size = new System.Drawing.Size(250, 108);
            this.card8.TabIndex = 7;
            // 
            // tableBottom
            // 
            this.tableBottom.ColumnCount = 2;
            this.tableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.tableBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.tableBottom.Controls.Add(this.cardChart, 0, 0);
            this.tableBottom.Controls.Add(this.cardToday, 1, 0);
            this.tableBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBottom.Location = new System.Drawing.Point(0, 248);
            this.tableBottom.Margin = new System.Windows.Forms.Padding(0);
            this.tableBottom.Name = "tableBottom";
            this.tableBottom.RowCount = 1;
            this.tableBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBottom.Size = new System.Drawing.Size(1000, 424);
            this.tableBottom.TabIndex = 1;
            // 
            // cardChart
            // 
            this.cardChart.Controls.Add(this.chartIncome);
            this.cardChart.Controls.Add(this.lblChartSubtitle);
            this.cardChart.Controls.Add(this.lblChartTitle);
            this.cardChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardChart.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.cardChart.Name = "cardChart";
            this.cardChart.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.cardChart.Size = new System.Drawing.Size(480, 424);
            this.cardChart.TabIndex = 0;
            // 
            // lblChartTitle
            // 
            this.lblChartTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblChartTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblChartTitle.Name = "lblChartTitle";
            this.lblChartTitle.Size = new System.Drawing.Size(440, 24);
            this.lblChartTitle.TabIndex = 0;
            this.lblChartTitle.Text = "Надходження коштів";
            // 
            // lblChartSubtitle
            // 
            this.lblChartSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblChartSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblChartSubtitle.Name = "lblChartSubtitle";
            this.lblChartSubtitle.Size = new System.Drawing.Size(440, 30);
            this.lblChartSubtitle.TabIndex = 1;
            this.lblChartSubtitle.Text = "Оплати за останні 6 місяців";
            // 
            // chartIncome
            // 
            this.chartIncome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartIncome.Name = "chartIncome";
            this.chartIncome.Size = new System.Drawing.Size(440, 338);
            this.chartIncome.TabIndex = 2;
            // 
            // cardToday
            // 
            this.cardToday.Controls.Add(this.gridToday);
            this.cardToday.Controls.Add(this.lblTodaySubtitle);
            this.cardToday.Controls.Add(this.lblToday);
            this.cardToday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardToday.Margin = new System.Windows.Forms.Padding(0);
            this.cardToday.Name = "cardToday";
            this.cardToday.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.cardToday.Size = new System.Drawing.Size(480, 424);
            this.cardToday.TabIndex = 1;
            // 
            // lblToday
            // 
            this.lblToday.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblToday.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblToday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblToday.Name = "lblToday";
            this.lblToday.Size = new System.Drawing.Size(440, 24);
            this.lblToday.TabIndex = 0;
            this.lblToday.Text = "Розклад на сьогодні";
            // 
            // lblTodaySubtitle
            // 
            this.lblTodaySubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTodaySubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTodaySubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblTodaySubtitle.Name = "lblTodaySubtitle";
            this.lblTodaySubtitle.Size = new System.Drawing.Size(440, 30);
            this.lblTodaySubtitle.TabIndex = 1;
            this.lblTodaySubtitle.Text = "—";
            // 
            // gridToday
            // 
            this.gridToday.AllowUserToAddRows = false;
            this.gridToday.AllowUserToDeleteRows = false;
            this.gridToday.AllowUserToResizeRows = false;
            this.gridToday.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridToday.BackgroundColor = System.Drawing.Color.White;
            this.gridToday.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridToday.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridToday.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridToday.MultiSelect = false;
            this.gridToday.Name = "gridToday";
            this.gridToday.ReadOnly = true;
            this.gridToday.RowHeadersVisible = false;
            this.gridToday.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridToday.Size = new System.Drawing.Size(440, 338);
            this.gridToday.TabIndex = 2;
            // 
            // pageStudents
            // 
            this.pageStudents.Controls.Add(this.listStudents);
            this.pageStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageStudents.Location = new System.Drawing.Point(24, 0);
            this.pageStudents.Name = "pageStudents";
            this.pageStudents.Size = new System.Drawing.Size(1000, 672);
            this.pageStudents.TabIndex = 1;
            this.pageStudents.Visible = false;
            // 
            // listStudents
            // 
            this.listStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listStudents.FilterVisible = true;
            this.listStudents.Location = new System.Drawing.Point(0, 0);
            this.listStudents.Name = "listStudents";
            this.listStudents.Size = new System.Drawing.Size(1000, 672);
            this.listStudents.TabIndex = 0;
            // 
            // pageTeachers
            // 
            this.pageTeachers.Controls.Add(this.listTeachers);
            this.pageTeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageTeachers.Location = new System.Drawing.Point(24, 0);
            this.pageTeachers.Name = "pageTeachers";
            this.pageTeachers.Size = new System.Drawing.Size(1000, 672);
            this.pageTeachers.TabIndex = 2;
            this.pageTeachers.Visible = false;
            // 
            // listTeachers
            // 
            this.listTeachers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTeachers.Location = new System.Drawing.Point(0, 0);
            this.listTeachers.Name = "listTeachers";
            this.listTeachers.Size = new System.Drawing.Size(1000, 672);
            this.listTeachers.TabIndex = 0;
            // 
            // pageCourses
            // 
            this.pageCourses.Controls.Add(this.listCourses);
            this.pageCourses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageCourses.Location = new System.Drawing.Point(24, 0);
            this.pageCourses.Name = "pageCourses";
            this.pageCourses.Size = new System.Drawing.Size(1000, 672);
            this.pageCourses.TabIndex = 3;
            this.pageCourses.Visible = false;
            // 
            // listCourses
            // 
            this.listCourses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listCourses.Location = new System.Drawing.Point(0, 0);
            this.listCourses.Name = "listCourses";
            this.listCourses.Size = new System.Drawing.Size(1000, 672);
            this.listCourses.TabIndex = 0;
            // 
            // pageGroups
            // 
            this.pageGroups.Controls.Add(this.listGroups);
            this.pageGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageGroups.Location = new System.Drawing.Point(24, 0);
            this.pageGroups.Name = "pageGroups";
            this.pageGroups.Size = new System.Drawing.Size(1000, 672);
            this.pageGroups.TabIndex = 4;
            this.pageGroups.Visible = false;
            // 
            // listGroups
            // 
            this.listGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listGroups.Location = new System.Drawing.Point(0, 0);
            this.listGroups.Name = "listGroups";
            this.listGroups.Size = new System.Drawing.Size(1000, 672);
            this.listGroups.TabIndex = 0;
            // 
            // pageSchedule
            // 
            this.pageSchedule.Controls.Add(this.listSchedule);
            this.pageSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageSchedule.Location = new System.Drawing.Point(24, 0);
            this.pageSchedule.Name = "pageSchedule";
            this.pageSchedule.Size = new System.Drawing.Size(1000, 672);
            this.pageSchedule.TabIndex = 5;
            this.pageSchedule.Visible = false;
            // 
            // listSchedule
            // 
            this.listSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSchedule.FilterVisible = true;
            this.listSchedule.Location = new System.Drawing.Point(0, 0);
            this.listSchedule.Name = "listSchedule";
            this.listSchedule.Size = new System.Drawing.Size(1000, 672);
            this.listSchedule.TabIndex = 0;
            // 
            // pagePayments
            // 
            this.pagePayments.Controls.Add(this.listPayments);
            this.pagePayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagePayments.Location = new System.Drawing.Point(24, 0);
            this.pagePayments.Name = "pagePayments";
            this.pagePayments.Size = new System.Drawing.Size(1000, 672);
            this.pagePayments.TabIndex = 6;
            this.pagePayments.Visible = false;
            // 
            // listPayments
            // 
            this.listPayments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPayments.FilterCaption = "Місяць:";
            this.listPayments.FilterVisible = true;
            this.listPayments.Location = new System.Drawing.Point(0, 0);
            this.listPayments.Name = "listPayments";
            this.listPayments.Size = new System.Drawing.Size(1000, 672);
            this.listPayments.TabIndex = 0;
            // 
            // pageGrades
            // 
            this.pageGrades.Controls.Add(this.listGrades);
            this.pageGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageGrades.Location = new System.Drawing.Point(24, 0);
            this.pageGrades.Name = "pageGrades";
            this.pageGrades.Size = new System.Drawing.Size(1000, 672);
            this.pageGrades.TabIndex = 7;
            this.pageGrades.Visible = false;
            // 
            // listGrades
            // 
            this.listGrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listGrades.FilterVisible = true;
            this.listGrades.Location = new System.Drawing.Point(0, 0);
            this.listGrades.Name = "listGrades";
            this.listGrades.Size = new System.Drawing.Size(1000, 672);
            this.listGrades.TabIndex = 0;
            // 
            // pageUsers
            // 
            this.pageUsers.Controls.Add(this.listUsers);
            this.pageUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageUsers.Location = new System.Drawing.Point(24, 0);
            this.pageUsers.Name = "pageUsers";
            this.pageUsers.Size = new System.Drawing.Size(1000, 672);
            this.pageUsers.TabIndex = 8;
            this.pageUsers.Visible = false;
            // 
            // listUsers
            // 
            this.listUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listUsers.Location = new System.Drawing.Point(0, 0);
            this.listUsers.Name = "listUsers";
            this.listUsers.Size = new System.Drawing.Size(1000, 672);
            this.listUsers.TabIndex = 0;
            // 
            // pageReports
            // 
            this.pageReports.Controls.Add(this.cardReport);
            this.pageReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageReports.Location = new System.Drawing.Point(24, 0);
            this.pageReports.Name = "pageReports";
            this.pageReports.Size = new System.Drawing.Size(1000, 672);
            this.pageReports.TabIndex = 9;
            this.pageReports.Visible = false;
            // 
            // cardReport
            // 
            this.cardReport.Controls.Add(this.gridReport);
            this.cardReport.Controls.Add(this.panelSummary);
            this.cardReport.Controls.Add(this.panelReportTop);
            this.cardReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardReport.Name = "cardReport";
            this.cardReport.Padding = new System.Windows.Forms.Padding(16, 10, 16, 16);
            this.cardReport.Size = new System.Drawing.Size(1000, 672);
            this.cardReport.TabIndex = 0;
            // 
            // gridReport
            // 
            this.gridReport.AllowUserToAddRows = false;
            this.gridReport.AllowUserToDeleteRows = false;
            this.gridReport.AllowUserToResizeRows = false;
            this.gridReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReport.BackgroundColor = System.Drawing.Color.White;
            this.gridReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReport.MultiSelect = false;
            this.gridReport.Name = "gridReport";
            this.gridReport.ReadOnly = true;
            this.gridReport.RowHeadersVisible = false;
            this.gridReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReport.Size = new System.Drawing.Size(440, 338);
            this.gridReport.TabIndex = 1;
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.panelSummary.Controls.Add(this.lblSummary);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.panelSummary.Size = new System.Drawing.Size(968, 44);
            this.panelSummary.TabIndex = 2;
            // 
            // lblSummary
            // 
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(940, 44);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelReportTop
            // 
            this.panelReportTop.Controls.Add(this.btnExport);
            this.panelReportTop.Controls.Add(this.btnBuild);
            this.panelReportTop.Controls.Add(this.cmbReport);
            this.panelReportTop.Controls.Add(this.lblReport);
            this.panelReportTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelReportTop.Name = "panelReportTop";
            this.panelReportTop.Size = new System.Drawing.Size(968, 58);
            this.panelReportTop.TabIndex = 0;
            // 
            // lblReport
            // 
            this.lblReport.AutoSize = true;
            this.lblReport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblReport.Location = new System.Drawing.Point(0, 19);
            this.lblReport.Name = "lblReport";
            this.lblReport.Size = new System.Drawing.Size(31, 15);
            this.lblReport.TabIndex = 0;
            this.lblReport.Text = "Звіт:";
            // 
            // cmbReport
            // 
            this.cmbReport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReport.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbReport.FormattingEnabled = true;
            this.cmbReport.Location = new System.Drawing.Point(44, 15);
            this.cmbReport.Name = "cmbReport";
            this.cmbReport.Size = new System.Drawing.Size(400, 25);
            this.cmbReport.TabIndex = 1;
            this.cmbReport.SelectedIndexChanged += new System.EventHandler(this.btnBuild_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(456, 11);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(130, 34);
            this.btnBuild.TabIndex = 2;
            this.btnBuild.Text = "Сформувати";
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(594, 11);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(150, 34);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Експорт у CSV";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.ClientSize = new System.Drawing.Size(1280, 780);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1120, 720);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Автоматизована система приватного навчального закладу";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panelSidebar.ResumeLayout(false);
            this.panelBrand.ResumeLayout(false);
            this.panelNav.ResumeLayout(false);
            this.panelUser.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.pageDashboard.ResumeLayout(false);
            this.tableDashboard.ResumeLayout(false);
            this.tableCards.ResumeLayout(false);
            this.tableBottom.ResumeLayout(false);
            this.cardChart.ResumeLayout(false);
            this.cardToday.ResumeLayout(false);
            this.pageStudents.ResumeLayout(false);
            this.pageTeachers.ResumeLayout(false);
            this.pageCourses.ResumeLayout(false);
            this.pageGroups.ResumeLayout(false);
            this.pageSchedule.ResumeLayout(false);
            this.pagePayments.ResumeLayout(false);
            this.pageGrades.ResumeLayout(false);
            this.pageUsers.ResumeLayout(false);
            this.pageReports.ResumeLayout(false);
            this.cardReport.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.panelReportTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridToday)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelBrand;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblAppSub;
        private System.Windows.Forms.Panel panelNav;
        private System.Windows.Forms.Label lblMenuCaption;
        private PrivateSchool.Controls.SidebarButton navDashboard;
        private PrivateSchool.Controls.SidebarButton navStudents;
        private PrivateSchool.Controls.SidebarButton navTeachers;
        private PrivateSchool.Controls.SidebarButton navCourses;
        private PrivateSchool.Controls.SidebarButton navGroups;
        private PrivateSchool.Controls.SidebarButton navSchedule;
        private PrivateSchool.Controls.SidebarButton navPayments;
        private PrivateSchool.Controls.SidebarButton navGrades;
        private PrivateSchool.Controls.SidebarButton navReports;
        private PrivateSchool.Controls.SidebarButton navUsers;
        private System.Windows.Forms.Panel panelUser;
        private System.Windows.Forms.Label lblUserLine;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserRole;
        private PrivateSchool.Controls.SidebarButton navLogout;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblPageSubtitle;
        private System.Windows.Forms.Button btnHeaderExport;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel pageDashboard;
        private System.Windows.Forms.TableLayoutPanel tableDashboard;
        private System.Windows.Forms.TableLayoutPanel tableCards;
        private PrivateSchool.Controls.StatCard card1;
        private PrivateSchool.Controls.StatCard card2;
        private PrivateSchool.Controls.StatCard card3;
        private PrivateSchool.Controls.StatCard card4;
        private PrivateSchool.Controls.StatCard card5;
        private PrivateSchool.Controls.StatCard card6;
        private PrivateSchool.Controls.StatCard card7;
        private PrivateSchool.Controls.StatCard card8;
        private System.Windows.Forms.TableLayoutPanel tableBottom;
        private PrivateSchool.Controls.CardPanel cardChart;
        private PrivateSchool.Controls.BarChart chartIncome;
        private System.Windows.Forms.Label lblChartSubtitle;
        private System.Windows.Forms.Label lblChartTitle;
        private PrivateSchool.Controls.CardPanel cardToday;
        private System.Windows.Forms.DataGridView gridToday;
        private System.Windows.Forms.Label lblTodaySubtitle;
        private System.Windows.Forms.Label lblToday;
        private System.Windows.Forms.Panel pageStudents;
        private PrivateSchool.Controls.EntityListControl listStudents;
        private System.Windows.Forms.Panel pageTeachers;
        private PrivateSchool.Controls.EntityListControl listTeachers;
        private System.Windows.Forms.Panel pageCourses;
        private PrivateSchool.Controls.EntityListControl listCourses;
        private System.Windows.Forms.Panel pageGroups;
        private PrivateSchool.Controls.EntityListControl listGroups;
        private System.Windows.Forms.Panel pageSchedule;
        private PrivateSchool.Controls.EntityListControl listSchedule;
        private System.Windows.Forms.Panel pagePayments;
        private PrivateSchool.Controls.EntityListControl listPayments;
        private System.Windows.Forms.Panel pageGrades;
        private PrivateSchool.Controls.EntityListControl listGrades;
        private System.Windows.Forms.Panel pageUsers;
        private PrivateSchool.Controls.EntityListControl listUsers;
        private System.Windows.Forms.Panel pageReports;
        private PrivateSchool.Controls.CardPanel cardReport;
        private System.Windows.Forms.DataGridView gridReport;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Panel panelReportTop;
        private System.Windows.Forms.Label lblReport;
        private System.Windows.Forms.ComboBox cmbReport;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
