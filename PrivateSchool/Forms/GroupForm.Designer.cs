namespace PrivateSchool.Forms
{
    partial class GroupForm
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblCourse = new System.Windows.Forms.Label();
            this.cmbCourse = new System.Windows.Forms.ComboBox();
            this.lblTeacher = new System.Windows.Forms.Label();
            this.cmbTeacher = new System.Windows.Forms.ComboBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblMaxStudents = new System.Windows.Forms.Label();
            this.numMaxStudents = new System.Windows.Forms.NumericUpDown();
            this.lblRoom = new System.Windows.Forms.Label();
            this.txtRoom = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxStudents)).BeginInit();
            this.SuspendLayout();
            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(16, 21);
            this.lblName.Name = "lblName";
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Назва групи:";
            //
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(160, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 23);
            this.txtName.TabIndex = 1;
            this.txtName.MaxLength = 40;
            //
            // lblCourse
            //
            this.lblCourse.AutoSize = true;
            this.lblCourse.Location = new System.Drawing.Point(16, 55);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.TabIndex = 2;
            this.lblCourse.Text = "Курс:";
            //
            // cmbCourse
            // 
            this.cmbCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCourse.FormattingEnabled = true;
            this.cmbCourse.Location = new System.Drawing.Point(160, 52);
            this.cmbCourse.Name = "cmbCourse";
            this.cmbCourse.Size = new System.Drawing.Size(250, 23);
            this.cmbCourse.TabIndex = 3;
            //
            // lblTeacher
            //
            this.lblTeacher.AutoSize = true;
            this.lblTeacher.Location = new System.Drawing.Point(16, 89);
            this.lblTeacher.Name = "lblTeacher";
            this.lblTeacher.TabIndex = 4;
            this.lblTeacher.Text = "Викладач:";
            //
            // cmbTeacher
            // 
            this.cmbTeacher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTeacher.FormattingEnabled = true;
            this.cmbTeacher.Location = new System.Drawing.Point(160, 86);
            this.cmbTeacher.Name = "cmbTeacher";
            this.cmbTeacher.Size = new System.Drawing.Size(250, 23);
            this.cmbTeacher.TabIndex = 5;
            //
            // lblStartDate
            //
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(16, 123);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.TabIndex = 6;
            this.lblStartDate.Text = "Дата старту:";
            //
            // dtpStartDate
            // 
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(160, 120);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(250, 23);
            this.dtpStartDate.TabIndex = 7;
            //
            // lblMaxStudents
            //
            this.lblMaxStudents.AutoSize = true;
            this.lblMaxStudents.Location = new System.Drawing.Point(16, 157);
            this.lblMaxStudents.Name = "lblMaxStudents";
            this.lblMaxStudents.TabIndex = 8;
            this.lblMaxStudents.Text = "Макс. кількість учнів:";
            //
            // numMaxStudents
            // 
            this.numMaxStudents.Location = new System.Drawing.Point(160, 154);
            this.numMaxStudents.Maximum = new decimal(new int[] { 40, 0, 0, 0 });
            this.numMaxStudents.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numMaxStudents.Name = "numMaxStudents";
            this.numMaxStudents.Size = new System.Drawing.Size(250, 23);
            this.numMaxStudents.TabIndex = 9;
            this.numMaxStudents.Value = new decimal(new int[] { 12, 0, 0, 0 });
            //
            // lblRoom
            //
            this.lblRoom.AutoSize = true;
            this.lblRoom.Location = new System.Drawing.Point(16, 191);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.TabIndex = 10;
            this.lblRoom.Text = "Аудиторія:";
            //
            // txtRoom
            // 
            this.txtRoom.Location = new System.Drawing.Point(160, 188);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(250, 23);
            this.txtRoom.TabIndex = 11;
            this.txtRoom.MaxLength = 40;
            //
            // GroupForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 280);
            this.Controls.Add(this.txtRoom);
            this.Controls.Add(this.lblRoom);
            this.Controls.Add(this.numMaxStudents);
            this.Controls.Add(this.lblMaxStudents);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.cmbTeacher);
            this.Controls.Add(this.lblTeacher);
            this.Controls.Add(this.cmbCourse);
            this.Controls.Add(this.lblCourse);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Name = "GroupForm";
            this.Text = "Група";
            ((System.ComponentModel.ISupportInitialize)(this.numMaxStudents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.Label lblTeacher;
        private System.Windows.Forms.ComboBox cmbTeacher;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label lblMaxStudents;
        private System.Windows.Forms.NumericUpDown numMaxStudents;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.TextBox txtRoom;
    }
}
