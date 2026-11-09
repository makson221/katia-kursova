namespace PrivateSchool.Forms
{
    partial class GradeForm
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
            this.lblGroup = new System.Windows.Forms.Label();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.lblStudent = new System.Windows.Forms.Label();
            this.cmbStudent = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblValue = new System.Windows.Forms.Label();
            this.numValue = new System.Windows.Forms.NumericUpDown();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblKind = new System.Windows.Forms.Label();
            this.cmbKind = new System.Windows.Forms.ComboBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).BeginInit();
            this.SuspendLayout();
            //
            // lblGroup
            //
            this.lblGroup.AutoSize = true;
            this.lblGroup.Location = new System.Drawing.Point(16, 21);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.TabIndex = 0;
            this.lblGroup.Text = "Група:";
            //
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(160, 18);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(250, 23);
            this.cmbGroup.TabIndex = 1;
            //
            // lblStudent
            //
            this.lblStudent.AutoSize = true;
            this.lblStudent.Location = new System.Drawing.Point(16, 55);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.TabIndex = 2;
            this.lblStudent.Text = "Учень:";
            //
            // cmbStudent
            // 
            this.cmbStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStudent.FormattingEnabled = true;
            this.cmbStudent.Location = new System.Drawing.Point(160, 52);
            this.cmbStudent.Name = "cmbStudent";
            this.cmbStudent.Size = new System.Drawing.Size(250, 23);
            this.cmbStudent.TabIndex = 3;
            //
            // lblDate
            //
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(16, 89);
            this.lblDate.Name = "lblDate";
            this.lblDate.TabIndex = 4;
            this.lblDate.Text = "Дата:";
            //
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(160, 86);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(250, 23);
            this.dtpDate.TabIndex = 5;
            //
            // lblValue
            //
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(16, 123);
            this.lblValue.Name = "lblValue";
            this.lblValue.TabIndex = 6;
            this.lblValue.Text = "Оцінка (1–12):";
            //
            // numValue
            // 
            this.numValue.Location = new System.Drawing.Point(160, 120);
            this.numValue.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.numValue.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numValue.Name = "numValue";
            this.numValue.Size = new System.Drawing.Size(100, 23);
            this.numValue.TabIndex = 7;
            this.numValue.Value = new decimal(new int[] { 10, 0, 0, 0 });
            //
            // lblLevel
            // 
            this.lblLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(153)))));
            this.lblLevel.Location = new System.Drawing.Point(160, 156);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(250, 23);
            this.lblLevel.TabIndex = 8;
            this.lblLevel.Text = "";
            //
            // lblKind
            //
            this.lblKind.AutoSize = true;
            this.lblKind.Location = new System.Drawing.Point(16, 191);
            this.lblKind.Name = "lblKind";
            this.lblKind.TabIndex = 9;
            this.lblKind.Text = "Вид оцінки:";
            //
            // cmbKind
            // 
            this.cmbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKind.FormattingEnabled = true;
            this.cmbKind.Location = new System.Drawing.Point(160, 188);
            this.cmbKind.Name = "cmbKind";
            this.cmbKind.Size = new System.Drawing.Size(250, 23);
            this.cmbKind.TabIndex = 10;
            //
            // lblComment
            //
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(16, 225);
            this.lblComment.Name = "lblComment";
            this.lblComment.TabIndex = 11;
            this.lblComment.Text = "Коментар:";
            //
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(160, 222);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComment.Size = new System.Drawing.Size(250, 50);
            this.txtComment.TabIndex = 12;
            //
            // GradeForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 341);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.cmbKind);
            this.Controls.Add(this.lblKind);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.numValue);
            this.Controls.Add(this.lblValue);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.cmbStudent);
            this.Controls.Add(this.lblStudent);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.lblGroup);
            this.Name = "GradeForm";
            this.Text = "Оцінка";
            ((System.ComponentModel.ISupportInitialize)(this.numValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.Label lblStudent;
        private System.Windows.Forms.ComboBox cmbStudent;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.NumericUpDown numValue;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblKind;
        private System.Windows.Forms.ComboBox cmbKind;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
    }
}
