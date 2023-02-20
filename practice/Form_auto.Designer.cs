namespace practice
{
    partial class Form_auto
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
            this.button_accessAndObject = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_TestRight_type = new System.Windows.Forms.ComboBox();
            this.button_testRight = new System.Windows.Forms.Button();
            this.button_readerLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_accessAndObject
            // 
            this.button_accessAndObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_accessAndObject.Location = new System.Drawing.Point(918, 10);
            this.button_accessAndObject.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_accessAndObject.Name = "button_accessAndObject";
            this.button_accessAndObject.Size = new System.Drawing.Size(325, 56);
            this.button_accessAndObject.TabIndex = 58;
            this.button_accessAndObject.Tag = "";
            this.button_accessAndObject.Text = "书目存取定义与对象权限";
            this.button_accessAndObject.UseVisualStyleBackColor = true;
            this.button_accessAndObject.Click += new System.EventHandler(this.button_accessAndObject_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(14, 130);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1207, 614);
            this.webBrowser1.TabIndex = 62;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 27);
            this.label1.TabIndex = 65;
            this.label1.Text = "数据类型";
            // 
            // comboBox_TestRight_type
            // 
            this.comboBox_TestRight_type.FormattingEnabled = true;
            this.comboBox_TestRight_type.Items.AddRange(new object[] {
            "读者",
            "书目",
            "册",
            "订购",
            "评注",
            "期",
            "违约金",
            "预约到书"});
            this.comboBox_TestRight_type.Location = new System.Drawing.Point(152, 24);
            this.comboBox_TestRight_type.Name = "comboBox_TestRight_type";
            this.comboBox_TestRight_type.Size = new System.Drawing.Size(235, 35);
            this.comboBox_TestRight_type.TabIndex = 66;
            this.comboBox_TestRight_type.Text = "读者";
            // 
            // button_testRight
            // 
            this.button_testRight.Location = new System.Drawing.Point(407, 16);
            this.button_testRight.Name = "button_testRight";
            this.button_testRight.Size = new System.Drawing.Size(150, 50);
            this.button_testRight.TabIndex = 67;
            this.button_testRight.Text = "权限测试";
            this.button_testRight.UseVisualStyleBackColor = true;
            this.button_testRight.Click += new System.EventHandler(this.button_testRight_Click);
            // 
            // button_readerLogin
            // 
            this.button_readerLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_readerLogin.Location = new System.Drawing.Point(722, 16);
            this.button_readerLogin.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin.Name = "button_readerLogin";
            this.button_readerLogin.Size = new System.Drawing.Size(186, 52);
            this.button_readerLogin.TabIndex = 68;
            this.button_readerLogin.Tag = "";
            this.button_readerLogin.Text = "读者身份";
            this.button_readerLogin.UseVisualStyleBackColor = true;
            this.button_readerLogin.Click += new System.EventHandler(this.button_readerLogin_Click);
            // 
            // Form_auto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 777);
            this.Controls.Add(this.button_readerLogin);
            this.Controls.Add(this.button_testRight);
            this.Controls.Add(this.comboBox_TestRight_type);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button_accessAndObject);
            this.Name = "Form_auto";
            this.Text = "自动测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_accessAndObject;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_TestRight_type;
        private System.Windows.Forms.Button button_testRight;
        private System.Windows.Forms.Button button_readerLogin;
    }
}