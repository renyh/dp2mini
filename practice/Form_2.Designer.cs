namespace practice
{
    partial class Form_2
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
            this.label28 = new System.Windows.Forms.Label();
            this.textBox_allRights = new System.Windows.Forms.TextBox();
            this.button_check = new System.Windows.Forms.Button();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.textBox_rights = new System.Windows.Forms.TextBox();
            this.button_getchinese = new System.Windows.Forms.Button();
            this.button_checkalias = new System.Windows.Forms.Button();
            this.button_createUserRights = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.textBox_userrightsdef = new System.Windows.Forms.TextBox();
            this.button_toRfc1123 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(30, 26);
            this.label28.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(162, 27);
            this.label28.TabIndex = 47;
            this.label28.Text = "权限总表txt";
            // 
            // textBox_allRights
            // 
            this.textBox_allRights.Location = new System.Drawing.Point(234, 15);
            this.textBox_allRights.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_allRights.Name = "textBox_allRights";
            this.textBox_allRights.Size = new System.Drawing.Size(502, 38);
            this.textBox_allRights.TabIndex = 46;
            this.textBox_allRights.Text = "D:\\000-权限测试\\rights\\权限总表.txt\r\n\r\n";
            // 
            // button_check
            // 
            this.button_check.Location = new System.Drawing.Point(234, 171);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(405, 47);
            this.button_check.TabIndex = 50;
            this.button_check.Text = "检查xml与excel不一致的地方";
            this.button_check.UseVisualStyleBackColor = true;
            this.button_check.Click += new System.EventHandler(this.button_check_Click);
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_result.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_result.Location = new System.Drawing.Point(14, 298);
            this.textBox_result.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(1359, 543);
            this.textBox_result.TabIndex = 51;
            // 
            // textBox_rights
            // 
            this.textBox_rights.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_rights.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_rights.Location = new System.Drawing.Point(915, 12);
            this.textBox_rights.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_rights.Multiline = true;
            this.textBox_rights.Name = "textBox_rights";
            this.textBox_rights.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_rights.Size = new System.Drawing.Size(439, 76);
            this.textBox_rights.TabIndex = 53;
            // 
            // button_getchinese
            // 
            this.button_getchinese.Location = new System.Drawing.Point(915, 94);
            this.button_getchinese.Name = "button_getchinese";
            this.button_getchinese.Size = new System.Drawing.Size(202, 47);
            this.button_getchinese.TabIndex = 54;
            this.button_getchinese.Text = "获取说明";
            this.button_getchinese.UseVisualStyleBackColor = true;
            this.button_getchinese.Click += new System.EventHandler(this.button_getchinese_Click);
            // 
            // button_checkalias
            // 
            this.button_checkalias.Location = new System.Drawing.Point(234, 224);
            this.button_checkalias.Name = "button_checkalias";
            this.button_checkalias.Size = new System.Drawing.Size(407, 47);
            this.button_checkalias.TabIndex = 55;
            this.button_checkalias.Text = "查找有别名的权限";
            this.button_checkalias.UseVisualStyleBackColor = true;
            this.button_checkalias.Click += new System.EventHandler(this.button_checkalias_Click);
            // 
            // button_createUserRights
            // 
            this.button_createUserRights.Location = new System.Drawing.Point(234, 118);
            this.button_createUserRights.Name = "button_createUserRights";
            this.button_createUserRights.Size = new System.Drawing.Size(405, 47);
            this.button_createUserRights.TabIndex = 56;
            this.button_createUserRights.Text = "生成权限表";
            this.button_createUserRights.UseVisualStyleBackColor = true;
            this.button_createUserRights.Click += new System.EventHandler(this.button_createUserRights_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(744, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 42);
            this.button1.TabIndex = 59;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(30, 76);
            this.label27.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(194, 27);
            this.label27.TabIndex = 58;
            this.label27.Text = "userrightsdef";
            // 
            // textBox_userrightsdef
            // 
            this.textBox_userrightsdef.Location = new System.Drawing.Point(234, 74);
            this.textBox_userrightsdef.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_userrightsdef.Name = "textBox_userrightsdef";
            this.textBox_userrightsdef.Size = new System.Drawing.Size(502, 38);
            this.textBox_userrightsdef.TabIndex = 57;
            this.textBox_userrightsdef.Text = "D:\\000-权限测试\\rights\\userrightsdef.xml";
            // 
            // button_toRfc1123
            // 
            this.button_toRfc1123.Location = new System.Drawing.Point(647, 224);
            this.button_toRfc1123.Name = "button_toRfc1123";
            this.button_toRfc1123.Size = new System.Drawing.Size(284, 47);
            this.button_toRfc1123.TabIndex = 60;
            this.button_toRfc1123.Text = "转换为rfc1123格式";
            this.button_toRfc1123.UseVisualStyleBackColor = true;
            this.button_toRfc1123.Click += new System.EventHandler(this.button_toRfc1123_Click);
            // 
            // Form_2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1406, 887);
            this.Controls.Add(this.button_toRfc1123);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.textBox_userrightsdef);
            this.Controls.Add(this.button_createUserRights);
            this.Controls.Add(this.button_checkalias);
            this.Controls.Add(this.button_getchinese);
            this.Controls.Add(this.textBox_rights);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.button_check);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.textBox_allRights);
            this.Name = "Form_2";
            this.Text = "Form_2";
            this.Load += new System.EventHandler(this.Form_2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox textBox_allRights;
        private System.Windows.Forms.Button button_check;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.TextBox textBox_rights;
        private System.Windows.Forms.Button button_getchinese;
        private System.Windows.Forms.Button button_checkalias;
        private System.Windows.Forms.Button button_createUserRights;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBox_userrightsdef;
        private System.Windows.Forms.Button button_toRfc1123;
    }
}