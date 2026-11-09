namespace PrivateSchool.Forms
{
    partial class TeacherForm
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
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblMiddleName = new System.Windows.Forms.Label();
            this.txtMiddleName = new System.Windows.Forms.TextBox();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.dtpBirthDate = new System.Windows.Forms.DateTimePicker();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblSpecialization = new System.Windows.Forms.Label();
            this.txtSpecialization = new System.Windows.Forms.TextBox();
            this.lblEducation = new System.Windows.Forms.Label();
            this.txtEducation = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblExperience = new System.Windows.Forms.Label();
            this.numExperience = new System.Windows.Forms.NumericUpDown();
            this.lblHourlyRate = new System.Windows.Forms.Label();
            this.numHourlyRate = new System.Windows.Forms.NumericUpDown();
            this.lblHireDate = new System.Windows.Forms.Label();
            this.dtpHireDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.numExperience)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHourlyRate)).BeginInit();
            this.SuspendLayout();
            //
            // lblLastName
            //
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(16, 21);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.TabIndex = 0;
            this.lblLastName.Text = "Прізвище:";
            //
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(160, 18);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(230, 23);
            this.txtLastName.TabIndex = 1;
            this.txtLastName.MaxLength = 50;
            //
            // lblFirstName
            //
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(16, 55);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.TabIndex = 2;
            this.lblFirstName.Text = "Ім'я:";
            //
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(160, 52);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(230, 23);
            this.txtFirstName.TabIndex = 3;
            this.txtFirstName.MaxLength = 50;
            //
            // lblMiddleName
            //
            this.lblMiddleName.AutoSize = true;
            this.lblMiddleName.Location = new System.Drawing.Point(16, 89);
            this.lblMiddleName.Name = "lblMiddleName";
            this.lblMiddleName.TabIndex = 4;
            this.lblMiddleName.Text = "По батькові:";
            //
            // txtMiddleName
            // 
            this.txtMiddleName.Location = new System.Drawing.Point(160, 86);
            this.txtMiddleName.Name = "txtMiddleName";
            this.txtMiddleName.Size = new System.Drawing.Size(230, 23);
            this.txtMiddleName.TabIndex = 5;
            this.txtMiddleName.MaxLength = 50;
            //
            // lblBirthDate
            //
            this.lblBirthDate.AutoSize = true;
            this.lblBirthDate.Location = new System.Drawing.Point(16, 123);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.TabIndex = 6;
            this.lblBirthDate.Text = "Дата народження:";
            //
            // dtpBirthDate
            // 
            this.dtpBirthDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBirthDate.Location = new System.Drawing.Point(160, 120);
            this.dtpBirthDate.Name = "dtpBirthDate";
            this.dtpBirthDate.Size = new System.Drawing.Size(230, 23);
            this.dtpBirthDate.TabIndex = 7;
            //
            // lblPhone
            //
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(16, 157);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.TabIndex = 8;
            this.lblPhone.Text = "Телефон:";
            //
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(160, 154);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(230, 23);
            this.txtPhone.TabIndex = 9;
            this.txtPhone.MaxLength = 20;
            //
            // lblEmail
            //
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(16, 191);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "E-mail:";
            //
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(160, 188);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(230, 23);
            this.txtEmail.TabIndex = 11;
            this.txtEmail.MaxLength = 60;
            //
            // lblSpecialization
            //
            this.lblSpecialization.AutoSize = true;
            this.lblSpecialization.Location = new System.Drawing.Point(404, 21);
            this.lblSpecialization.Name = "lblSpecialization";
            this.lblSpecialization.TabIndex = 12;
            this.lblSpecialization.Text = "Спеціалізація:";
            //
            // txtSpecialization
            // 
            this.txtSpecialization.Location = new System.Drawing.Point(548, 18);
            this.txtSpecialization.Name = "txtSpecialization";
            this.txtSpecialization.Size = new System.Drawing.Size(230, 23);
            this.txtSpecialization.TabIndex = 13;
            this.txtSpecialization.MaxLength = 80;
            //
            // lblEducation
            //
            this.lblEducation.AutoSize = true;
            this.lblEducation.Location = new System.Drawing.Point(404, 55);
            this.lblEducation.Name = "lblEducation";
            this.lblEducation.TabIndex = 14;
            this.lblEducation.Text = "Освіта:";
            //
            // txtEducation
            // 
            this.txtEducation.Location = new System.Drawing.Point(548, 52);
            this.txtEducation.Name = "txtEducation";
            this.txtEducation.Size = new System.Drawing.Size(230, 23);
            this.txtEducation.TabIndex = 15;
            this.txtEducation.MaxLength = 150;
            //
            // lblCategory
            //
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(404, 89);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.TabIndex = 16;
            this.lblCategory.Text = "Категорія:";
            //
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(548, 86);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(230, 23);
            this.cmbCategory.TabIndex = 17;
            //
            // lblExperience
            //
            this.lblExperience.AutoSize = true;
            this.lblExperience.Location = new System.Drawing.Point(404, 123);
            this.lblExperience.Name = "lblExperience";
            this.lblExperience.TabIndex = 18;
            this.lblExperience.Text = "Стаж, років:";
            //
            // numExperience
            // 
            this.numExperience.Location = new System.Drawing.Point(548, 120);
            this.numExperience.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            this.numExperience.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numExperience.Name = "numExperience";
            this.numExperience.Size = new System.Drawing.Size(230, 23);
            this.numExperience.TabIndex = 19;
            this.numExperience.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // lblHourlyRate
            //
            this.lblHourlyRate.AutoSize = true;
            this.lblHourlyRate.Location = new System.Drawing.Point(404, 157);
            this.lblHourlyRate.Name = "lblHourlyRate";
            this.lblHourlyRate.TabIndex = 20;
            this.lblHourlyRate.Text = "Ставка, грн/год:";
            //
            // numHourlyRate
            // 
            this.numHourlyRate.DecimalPlaces = 2;
            this.numHourlyRate.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            this.numHourlyRate.Location = new System.Drawing.Point(548, 154);
            this.numHourlyRate.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.numHourlyRate.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numHourlyRate.Name = "numHourlyRate";
            this.numHourlyRate.Size = new System.Drawing.Size(230, 23);
            this.numHourlyRate.TabIndex = 21;
            this.numHourlyRate.ThousandsSeparator = true;
            this.numHourlyRate.Value = new decimal(new int[] { 250, 0, 0, 0 });
            //
            // lblHireDate
            //
            this.lblHireDate.AutoSize = true;
            this.lblHireDate.Location = new System.Drawing.Point(404, 191);
            this.lblHireDate.Name = "lblHireDate";
            this.lblHireDate.TabIndex = 22;
            this.lblHireDate.Text = "Дата прийняття:";
            //
            // dtpHireDate
            // 
            this.dtpHireDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHireDate.Location = new System.Drawing.Point(548, 188);
            this.dtpHireDate.Name = "dtpHireDate";
            this.dtpHireDate.Size = new System.Drawing.Size(230, 23);
            this.dtpHireDate.TabIndex = 23;
            //
            // TeacherForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 280);
            this.Controls.Add(this.dtpHireDate);
            this.Controls.Add(this.lblHireDate);
            this.Controls.Add(this.numHourlyRate);
            this.Controls.Add(this.lblHourlyRate);
            this.Controls.Add(this.numExperience);
            this.Controls.Add(this.lblExperience);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtEducation);
            this.Controls.Add(this.lblEducation);
            this.Controls.Add(this.txtSpecialization);
            this.Controls.Add(this.lblSpecialization);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.dtpBirthDate);
            this.Controls.Add(this.lblBirthDate);
            this.Controls.Add(this.txtMiddleName);
            this.Controls.Add(this.lblMiddleName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.lblLastName);
            this.Name = "TeacherForm";
            this.Text = "Викладач";
            ((System.ComponentModel.ISupportInitialize)(this.numExperience)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHourlyRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblMiddleName;
        private System.Windows.Forms.TextBox txtMiddleName;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblSpecialization;
        private System.Windows.Forms.TextBox txtSpecialization;
        private System.Windows.Forms.Label lblEducation;
        private System.Windows.Forms.TextBox txtEducation;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblExperience;
        private System.Windows.Forms.NumericUpDown numExperience;
        private System.Windows.Forms.Label lblHourlyRate;
        private System.Windows.Forms.NumericUpDown numHourlyRate;
        private System.Windows.Forms.Label lblHireDate;
        private System.Windows.Forms.DateTimePicker dtpHireDate;
    }
}
