namespace passwordTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_source = new System.Windows.Forms.TextBox();
            this.textBox_key = new System.Windows.Forms.TextBox();
            this.textBox_target = new System.Windows.Forms.TextBox();
            this.button_encrypt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_decrypt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_source
            // 
            this.textBox_source.Location = new System.Drawing.Point(120, 42);
            this.textBox_source.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox_source.Name = "textBox_source";
            this.textBox_source.Size = new System.Drawing.Size(672, 38);
            this.textBox_source.TabIndex = 0;
            // 
            // textBox_key
            // 
            this.textBox_key.Location = new System.Drawing.Point(120, 114);
            this.textBox_key.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox_key.Name = "textBox_key";
            this.textBox_key.Size = new System.Drawing.Size(672, 38);
            this.textBox_key.TabIndex = 1;
            // 
            // textBox_target
            // 
            this.textBox_target.Location = new System.Drawing.Point(120, 304);
            this.textBox_target.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox_target.Multiline = true;
            this.textBox_target.Name = "textBox_target";
            this.textBox_target.Size = new System.Drawing.Size(672, 140);
            this.textBox_target.TabIndex = 2;
            // 
            // button_encrypt
            // 
            this.button_encrypt.Location = new System.Drawing.Point(120, 209);
            this.button_encrypt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button_encrypt.Name = "button_encrypt";
            this.button_encrypt.Size = new System.Drawing.Size(173, 48);
            this.button_encrypt.TabIndex = 3;
            this.button_encrypt.Text = "明文->密文";
            this.button_encrypt.UseVisualStyleBackColor = true;
            this.button_encrypt.Click += new System.EventHandler(this.button_encrypt_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 27);
            this.label1.TabIndex = 4;
            this.label1.Text = "明文";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 27);
            this.label2.TabIndex = 5;
            this.label2.Text = "密钥";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 304);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 27);
            this.label3.TabIndex = 6;
            this.label3.Text = "密文";
            // 
            // button_decrypt
            // 
            this.button_decrypt.Location = new System.Drawing.Point(339, 209);
            this.button_decrypt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button_decrypt.Name = "button_decrypt";
            this.button_decrypt.Size = new System.Drawing.Size(173, 48);
            this.button_decrypt.TabIndex = 7;
            this.button_decrypt.Text = "密文->明文";
            this.button_decrypt.UseVisualStyleBackColor = true;
            this.button_decrypt.Click += new System.EventHandler(this.button_decrypt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 567);
            this.Controls.Add(this.button_decrypt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_encrypt);
            this.Controls.Add(this.textBox_target);
            this.Controls.Add(this.textBox_key);
            this.Controls.Add(this.textBox_source);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_source;
        private System.Windows.Forms.TextBox textBox_key;
        private System.Windows.Forms.TextBox textBox_target;
        private System.Windows.Forms.Button button_encrypt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_decrypt;
    }
}

