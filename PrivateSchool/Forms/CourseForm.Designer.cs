namespace PrivateSchool.Forms
{
    partial class CourseForm
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
            this.lblLevel = new System.Windows.Forms.Label();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.lblDuration = new System.Windows.Forms.Label();
            this.numDuration = new System.Windows.Forms.NumericUpDown();
            this.lblLessonsPerWeek = new System.Windows.Forms.Label();
            this.numLessonsPerWeek = new System.Windows.Forms.NumericUpDown();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblTotal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLessonsPerWeek)).BeginInit();
            this.SuspendLayout();
            //
            // lblName
            //
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(16, 21);
            this.lblName.Name = "lblName";
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Назва курсу:";
            //
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(160, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 23);
            this.txtName.TabIndex = 1;
            this.txtName.MaxLength = 100;
            //
            // lblLevel
            //
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(16, 55);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.TabIndex = 2;
            this.lblLevel.Text = "Рівень:";
            //
            // cmbLevel
            // 
            this.cmbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Location = new System.Drawing.Point(160, 52);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(250, 23);
            this.cmbLevel.TabIndex = 3;
            //
            // lblPrice
            //
            this.lblPrice.AutoSize = true;
            this.lblPrice.Location = new System.Drawing.Point(16, 89);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.TabIndex = 4;
            this.lblPrice.Text = "Ціна за місяць, грн:";
            //
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            this.numPrice.Location = new System.Drawing.Point(160, 86);
            this.numPrice.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numPrice.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(250, 23);
            this.numPrice.TabIndex = 5;
            this.numPrice.ThousandsSeparator = true;
            this.numPrice.Value = new decimal(new int[] { 2000, 0, 0, 0 });
            //
            // lblDuration
            //
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(16, 123);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.TabIndex = 6;
            this.lblDuration.Text = "Тривалість, міс.:";
            //
            // numDuration
            // 
            this.numDuration.Location = new System.Drawing.Point(160, 120);
            this.numDuration.Maximum = new decimal(new int[] { 48, 0, 0, 0 });
            this.numDuration.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numDuration.Name = "numDuration";
            this.numDuration.Size = new System.Drawing.Size(250, 23);
            this.numDuration.TabIndex = 7;
            this.numDuration.Value = new decimal(new int[] { 9, 0, 0, 0 });
            //
            // lblLessonsPerWeek
            //
            this.lblLessonsPerWeek.AutoSize = true;
            this.lblLessonsPerWeek.Location = new System.Drawing.Point(16, 157);
            this.lblLessonsPerWeek.Name = "lblLessonsPerWeek";
            this.lblLessonsPerWeek.TabIndex = 8;
            this.lblLessonsPerWeek.Text = "Занять на тиждень:";
            //
            // numLessonsPerWeek
            // 
            this.numLessonsPerWeek.Location = new System.Drawing.Point(160, 154);
            this.numLessonsPerWeek.Maximum = new decimal(new int[] { 7, 0, 0, 0 });
            this.numLessonsPerWeek.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numLessonsPerWeek.Name = "numLessonsPerWeek";
            this.numLessonsPerWeek.Size = new System.Drawing.Size(250, 23);
            this.numLessonsPerWeek.TabIndex = 9;
            this.numLessonsPerWeek.Value = new decimal(new int[] { 2, 0, 0, 0 });
            //
            // lblDescription
            //
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(16, 191);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "Опис:";
            //
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(160, 188);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(250, 80);
            this.txtDescription.TabIndex = 11;
            //
            // lblTotal
            // 
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(153)))));
            this.lblTotal.Location = new System.Drawing.Point(160, 281);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(250, 23);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "";
            //
            // CourseForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 371);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.numLessonsPerWeek);
            this.Controls.Add(this.lblLessonsPerWeek);
            this.Controls.Add(this.numDuration);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.cmbLevel);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Name = "CourseForm";
            this.Text = "Курс";
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLessonsPerWeek)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.NumericUpDown numPrice;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.NumericUpDown numDuration;
        private System.Windows.Forms.Label lblLessonsPerWeek;
        private System.Windows.Forms.NumericUpDown numLessonsPerWeek;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblTotal;
    }
}
