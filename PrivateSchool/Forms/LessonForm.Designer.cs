namespace PrivateSchool.Forms
{
    partial class LessonForm
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
            this.lblTeacher = new System.Windows.Forms.Label();
            this.lblDay = new System.Windows.Forms.Label();
            this.cmbDay = new System.Windows.Forms.ComboBox();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.lblDuration = new System.Windows.Forms.Label();
            this.numDuration = new System.Windows.Forms.NumericUpDown();
            this.lblRoom = new System.Windows.Forms.Label();
            this.txtRoom = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numDuration)).BeginInit();
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
            // lblTeacher
            // 
            this.lblTeacher.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(153)))));
            this.lblTeacher.Location = new System.Drawing.Point(160, 54);
            this.lblTeacher.Name = "lblTeacher";
            this.lblTeacher.Size = new System.Drawing.Size(250, 23);
            this.lblTeacher.TabIndex = 2;
            this.lblTeacher.Text = "";
            //
            // lblDay
            //
            this.lblDay.AutoSize = true;
            this.lblDay.Location = new System.Drawing.Point(16, 89);
            this.lblDay.Name = "lblDay";
            this.lblDay.TabIndex = 3;
            this.lblDay.Text = "День тижня:";
            //
            // cmbDay
            // 
            this.cmbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.Location = new System.Drawing.Point(160, 86);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(250, 23);
            this.cmbDay.TabIndex = 4;
            //
            // lblStartTime
            //
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(16, 123);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.TabIndex = 5;
            this.lblStartTime.Text = "Початок:";
            //
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "HH:mm";
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(160, 120);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(100, 23);
            this.dtpStartTime.TabIndex = 6;
            //
            // lblDuration
            //
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(16, 157);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.TabIndex = 7;
            this.lblDuration.Text = "Тривалість, хв:";
            //
            // numDuration
            // 
            this.numDuration.Increment = new decimal(new int[] { 15, 0, 0, 0 });
            this.numDuration.Location = new System.Drawing.Point(160, 154);
            this.numDuration.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
            this.numDuration.Minimum = new decimal(new int[] { 30, 0, 0, 0 });
            this.numDuration.Name = "numDuration";
            this.numDuration.Size = new System.Drawing.Size(100, 23);
            this.numDuration.TabIndex = 8;
            this.numDuration.Value = new decimal(new int[] { 90, 0, 0, 0 });
            //
            // lblRoom
            //
            this.lblRoom.AutoSize = true;
            this.lblRoom.Location = new System.Drawing.Point(16, 191);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.TabIndex = 9;
            this.lblRoom.Text = "Аудиторія:";
            //
            // txtRoom
            // 
            this.txtRoom.Location = new System.Drawing.Point(160, 188);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(250, 23);
            this.txtRoom.TabIndex = 10;
            this.txtRoom.MaxLength = 40;
            //
            // LessonForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 280);
            this.Controls.Add(this.txtRoom);
            this.Controls.Add(this.lblRoom);
            this.Controls.Add(this.numDuration);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.dtpStartTime);
            this.Controls.Add(this.lblStartTime);
            this.Controls.Add(this.cmbDay);
            this.Controls.Add(this.lblDay);
            this.Controls.Add(this.lblTeacher);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.lblGroup);
            this.Name = "LessonForm";
            this.Text = "Заняття";
            ((System.ComponentModel.ISupportInitialize)(this.numDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.Label lblTeacher;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.ComboBox cmbDay;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.NumericUpDown numDuration;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.TextBox txtRoom;
    }
}
