namespace PrivateSchool.Forms
{
    partial class StudentForm
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
            this.lblParentName = new System.Windows.Forms.Label();
            this.txtParentName = new System.Windows.Forms.TextBox();
            this.lblContract = new System.Windows.Forms.Label();
            this.txtContract = new System.Windows.Forms.TextBox();
            this.lblEnrollment = new System.Windows.Forms.Label();
            this.dtpEnrollment = new System.Windows.Forms.DateTimePicker();
            this.lblGroup = new System.Windows.Forms.Label();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblDiscount = new System.Windows.Forms.Label();
            this.numDiscount = new System.Windows.Forms.NumericUpDown();
            this.lblFee = new System.Windows.Forms.Label();
            this.lblParentPhone = new System.Windows.Forms.Label();
            this.txtParentPhone = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).BeginInit();
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
            // lblParentName
            //
            this.lblParentName.AutoSize = true;
            this.lblParentName.Location = new System.Drawing.Point(16, 225);
            this.lblParentName.Name = "lblParentName";
            this.lblParentName.TabIndex = 12;
            this.lblParentName.Text = "ПІБ одного з батьків:";
            //
            // txtParentName
            // 
            this.txtParentName.Location = new System.Drawing.Point(160, 222);
            this.txtParentName.Name = "txtParentName";
            this.txtParentName.Size = new System.Drawing.Size(230, 23);
            this.txtParentName.TabIndex = 13;
            this.txtParentName.MaxLength = 100;
            //
            // lblContract
            //
            this.lblContract.AutoSize = true;
            this.lblContract.Location = new System.Drawing.Point(404, 21);
            this.lblContract.Name = "lblContract";
            this.lblContract.TabIndex = 14;
            this.lblContract.Text = "№ договору:";
            //
            // txtContract
            // 
            this.txtContract.Location = new System.Drawing.Point(548, 18);
            this.txtContract.Name = "txtContract";
            this.txtContract.Size = new System.Drawing.Size(230, 23);
            this.txtContract.TabIndex = 15;
            this.txtContract.MaxLength = 30;
            //
            // lblEnrollment
            //
            this.lblEnrollment.AutoSize = true;
            this.lblEnrollment.Location = new System.Drawing.Point(404, 55);
            this.lblEnrollment.Name = "lblEnrollment";
            this.lblEnrollment.TabIndex = 16;
            this.lblEnrollment.Text = "Дата зарахування:";
            //
            // dtpEnrollment
            // 
            this.dtpEnrollment.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnrollment.Location = new System.Drawing.Point(548, 52);
            this.dtpEnrollment.Name = "dtpEnrollment";
            this.dtpEnrollment.Size = new System.Drawing.Size(230, 23);
            this.dtpEnrollment.TabIndex = 17;
            //
            // lblGroup
            //
            this.lblGroup.AutoSize = true;
            this.lblGroup.Location = new System.Drawing.Point(404, 89);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.TabIndex = 18;
            this.lblGroup.Text = "Група:";
            //
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(548, 86);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(230, 23);
            this.cmbGroup.TabIndex = 19;
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(404, 123);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.TabIndex = 20;
            this.lblStatus.Text = "Статус:";
            //
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(548, 120);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(230, 23);
            this.cmbStatus.TabIndex = 21;
            //
            // lblDiscount
            //
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Location = new System.Drawing.Point(404, 157);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.TabIndex = 22;
            this.lblDiscount.Text = "Знижка, %:";
            //
            // numDiscount
            // 
            this.numDiscount.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            this.numDiscount.Location = new System.Drawing.Point(548, 154);
            this.numDiscount.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numDiscount.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numDiscount.Name = "numDiscount";
            this.numDiscount.Size = new System.Drawing.Size(230, 23);
            this.numDiscount.TabIndex = 23;
            this.numDiscount.Value = new decimal(new int[] { 0, 0, 0, 0 });
            //
            // lblFee
            // 
            this.lblFee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(153)))));
            this.lblFee.Location = new System.Drawing.Point(548, 190);
            this.lblFee.Name = "lblFee";
            this.lblFee.Size = new System.Drawing.Size(230, 23);
            this.lblFee.TabIndex = 24;
            this.lblFee.Text = "";
            //
            // lblParentPhone
            //
            this.lblParentPhone.AutoSize = true;
            this.lblParentPhone.Location = new System.Drawing.Point(404, 225);
            this.lblParentPhone.Name = "lblParentPhone";
            this.lblParentPhone.TabIndex = 25;
            this.lblParentPhone.Text = "Телефон батьків:";
            //
            // txtParentPhone
            // 
            this.txtParentPhone.Location = new System.Drawing.Point(548, 222);
            this.txtParentPhone.Name = "txtParentPhone";
            this.txtParentPhone.Size = new System.Drawing.Size(230, 23);
            this.txtParentPhone.TabIndex = 26;
            this.txtParentPhone.MaxLength = 20;
            //
            // StudentForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 314);
            this.Controls.Add(this.txtParentPhone);
            this.Controls.Add(this.lblParentPhone);
            this.Controls.Add(this.lblFee);
            this.Controls.Add(this.numDiscount);
            this.Controls.Add(this.lblDiscount);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.lblGroup);
            this.Controls.Add(this.dtpEnrollment);
            this.Controls.Add(this.lblEnrollment);
            this.Controls.Add(this.txtContract);
            this.Controls.Add(this.lblContract);
            this.Controls.Add(this.txtParentName);
            this.Controls.Add(this.lblParentName);
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
            this.Name = "StudentForm";
            this.Text = "Учень";
            ((System.ComponentModel.ISupportInitialize)(this.numDiscount)).EndInit();
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
        private System.Windows.Forms.Label lblParentName;
        private System.Windows.Forms.TextBox txtParentName;
        private System.Windows.Forms.Label lblContract;
        private System.Windows.Forms.TextBox txtContract;
        private System.Windows.Forms.Label lblEnrollment;
        private System.Windows.Forms.DateTimePicker dtpEnrollment;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.NumericUpDown numDiscount;
        private System.Windows.Forms.Label lblFee;
        private System.Windows.Forms.Label lblParentPhone;
        private System.Windows.Forms.TextBox txtParentPhone;
    }
}
