namespace PrivateSchool.Forms
{
    partial class UserForm
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
            this.lblLogin = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // lblLogin
            //
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(16, 21);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.TabIndex = 0;
            this.lblLogin.Text = "Логін:";
            //
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(160, 18);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(250, 23);
            this.txtLogin.TabIndex = 1;
            this.txtLogin.MaxLength = 30;
            //
            // lblFullName
            //
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(16, 55);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.TabIndex = 2;
            this.lblFullName.Text = "ПІБ:";
            //
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(160, 52);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(250, 23);
            this.txtFullName.TabIndex = 3;
            this.txtFullName.MaxLength = 100;
            //
            // lblRole
            //
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(16, 89);
            this.lblRole.Name = "lblRole";
            this.lblRole.TabIndex = 4;
            this.lblRole.Text = "Роль:";
            //
            // cmbRole
            // 
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(160, 86);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(250, 23);
            this.cmbRole.TabIndex = 5;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(16, 123);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Пароль:";
            //
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(160, 120);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(250, 23);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            //
            // lblConfirm
            //
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new System.Drawing.Point(16, 157);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.TabIndex = 8;
            this.lblConfirm.Text = "Підтвердження:";
            //
            // txtConfirm
            // 
            this.txtConfirm.Location = new System.Drawing.Point(160, 154);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(250, 23);
            this.txtConfirm.TabIndex = 9;
            this.txtConfirm.UseSystemPasswordChar = true;
            //
            // lblHint
            // 
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(84)))), ((int)(((byte)(153)))));
            this.lblHint.Location = new System.Drawing.Point(160, 190);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(250, 36);
            this.lblHint.TabIndex = 10;
            this.lblHint.Text = "";
            //
            // UserForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 293);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.txtConfirm);
            this.Controls.Add(this.lblConfirm);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.lblLogin);
            this.Name = "UserForm";
            this.Text = "Користувач";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Label lblHint;
    }
}
