namespace dp2mini
{
    partial class LoginForm
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_login = new System.Windows.Forms.Button();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_libraryUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(431, 252);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(174, 51);
            this.button_cancel.TabIndex = 8;
            this.button_cancel.Text = "取消(&C)";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(229, 252);
            this.button_login.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(174, 51);
            this.button_login.TabIndex = 7;
            this.button_login.Text = "登录";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(229, 183);
            this.textBox_password.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(443, 38);
            this.textBox_password.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 189);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 27);
            this.label3.TabIndex = 4;
            this.label3.Text = "密码：";
            // 
            // textBox_username
            // 
            this.textBox_username.Location = new System.Drawing.Point(229, 122);
            this.textBox_username.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(443, 38);
            this.textBox_username.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 122);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "用户名：";
            // 
            // textBox_libraryUrl
            // 
            this.textBox_libraryUrl.Location = new System.Drawing.Point(229, 60);
            this.textBox_libraryUrl.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox_libraryUrl.Name = "textBox_libraryUrl";
            this.textBox_libraryUrl.Size = new System.Drawing.Size(844, 38);
            this.textBox_libraryUrl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器地址：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 428);
            this.button1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(263, 51);
            this.button1.TabIndex = 9;
            this.button1.Text = "打开数据文件夹";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1115, 498);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_libraryUrl);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_libraryUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}