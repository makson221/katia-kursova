namespace PrivateSchool.Controls
{
    partial class EntityListControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.flowFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.flowActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.lblCount = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelToolbar.SuspendLayout();
            this.flowFilters.SuspendLayout();
            this.flowActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panelToolbar
            // 
            this.panelToolbar.BackColor = System.Drawing.Color.White;
            this.panelToolbar.Controls.Add(this.flowFilters);
            this.panelToolbar.Controls.Add(this.flowActions);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(16, 10);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(868, 54);
            this.panelToolbar.TabIndex = 0;
            // 
            // flowFilters
            // 
            this.flowFilters.AutoSize = true;
            this.flowFilters.BackColor = System.Drawing.Color.White;
            this.flowFilters.Controls.Add(this.lblFilter);
            this.flowFilters.Controls.Add(this.cmbFilter);
            this.flowFilters.Controls.Add(this.lblSearch);
            this.flowFilters.Controls.Add(this.txtSearch);
            this.flowFilters.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowFilters.Location = new System.Drawing.Point(420, 0);
            this.flowFilters.Name = "flowFilters";
            this.flowFilters.Size = new System.Drawing.Size(448, 54);
            this.flowFilters.TabIndex = 1;
            this.flowFilters.WrapContents = false;
            // 
            // lblFilter
            // 
            this.lblFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblFilter.Location = new System.Drawing.Point(0, 8);
            this.lblFilter.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(52, 34);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "Група:";
            this.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblFilter.Visible = false;
            // 
            // cmbFilter
            // 
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.DropDownWidth = 260;
            this.cmbFilter.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(56, 13);
            this.cmbFilter.Margin = new System.Windows.Forms.Padding(0, 13, 16, 0);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(170, 25);
            this.cmbFilter.TabIndex = 1;
            this.cmbFilter.Visible = false;
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblSearch.Location = new System.Drawing.Point(242, 8);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(0, 8, 4, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(52, 34);
            this.lblSearch.TabIndex = 2;
            this.lblSearch.Text = "Пошук:";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSearch
            // 
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.txtSearch.Location = new System.Drawing.Point(298, 13);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 13, 0, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(240, 25);
            this.txtSearch.TabIndex = 3;
            this.toolTip.SetToolTip(this.txtSearch, "Введіть частину ПІБ, назви, телефону тощо. Esc — очистити.");
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // flowActions
            // 
            this.flowActions.AutoSize = true;
            this.flowActions.BackColor = System.Drawing.Color.White;
            this.flowActions.Controls.Add(this.btnAdd);
            this.flowActions.Controls.Add(this.btnEdit);
            this.flowActions.Controls.Add(this.btnDelete);
            this.flowActions.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowActions.Location = new System.Drawing.Point(0, 0);
            this.flowActions.Name = "flowActions";
            this.flowActions.Size = new System.Drawing.Size(372, 54);
            this.flowActions.TabIndex = 0;
            this.flowActions.WrapContents = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(0, 10);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0, 10, 8, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(116, 34);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "+  Додати";
            this.toolTip.SetToolTip(this.btnAdd, "Додати новий запис (Insert)");
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(124, 10);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(0, 10, 8, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(116, 34);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "Редагувати";
            this.toolTip.SetToolTip(this.btnEdit, "Редагувати вибраний запис (Enter або подвійний клік)");
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(248, 10);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 10, 8, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(116, 34);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Видалити";
            this.toolTip.SetToolTip(this.btnDelete, "Видалити вибраний запис (Delete)");
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.BackgroundColor = System.Drawing.Color.White;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(16, 64);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(868, 396);
            this.grid.TabIndex = 1;
            this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_CellFormatting);
            this.grid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grid_DataBindingComplete);
            this.grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_KeyDown);
            // 
            // lblCount
            // 
            this.lblCount.BackColor = System.Drawing.Color.White;
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblCount.Location = new System.Drawing.Point(16, 460);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(868, 32);
            this.lblCount.TabIndex = 2;
            this.lblCount.Text = "Записів: 0";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EntityListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.panelToolbar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "EntityListControl";
            this.Padding = new System.Windows.Forms.Padding(16, 10, 16, 6);
            this.Size = new System.Drawing.Size(900, 500);
            this.panelToolbar.ResumeLayout(false);
            this.panelToolbar.PerformLayout();
            this.flowFilters.ResumeLayout(false);
            this.flowFilters.PerformLayout();
            this.flowActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.FlowLayoutPanel flowFilters;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.FlowLayoutPanel flowActions;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
