namespace bookshelf
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_serverUrl = new System.Windows.Forms.TextBox();
            this.textBox_area = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_jia = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_open = new System.Windows.Forms.Button();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.button_test = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "密集架服务器地址：";
            // 
            // textBox_serverUrl
            // 
            this.textBox_serverUrl.Location = new System.Drawing.Point(46, 72);
            this.textBox_serverUrl.Name = "textBox_serverUrl";
            this.textBox_serverUrl.Size = new System.Drawing.Size(736, 38);
            this.textBox_serverUrl.TabIndex = 1;
            this.textBox_serverUrl.Text = "http://xxx:8733/DFServer/RestFullWcf";
            // 
            // textBox_area
            // 
            this.textBox_area.Location = new System.Drawing.Point(98, 151);
            this.textBox_area.Name = "textBox_area";
            this.textBox_area.Size = new System.Drawing.Size(276, 38);
            this.textBox_area.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "区：";
            // 
            // textBox_jia
            // 
            this.textBox_jia.Location = new System.Drawing.Point(98, 207);
            this.textBox_jia.Name = "textBox_jia";
            this.textBox_jia.Size = new System.Drawing.Size(276, 38);
            this.textBox_jia.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 27);
            this.label3.TabIndex = 4;
            this.label3.Text = "架：";
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(98, 268);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(115, 49);
            this.button_open.TabIndex = 6;
            this.button_open.Text = "开架";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.BackColor = System.Drawing.SystemColors.Info;
            this.textBox_result.Location = new System.Drawing.Point(48, 346);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ReadOnly = true;
            this.textBox_result.Size = new System.Drawing.Size(734, 149);
            this.textBox_result.TabIndex = 7;
            // 
            // button_test
            // 
            this.button_test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_test.Location = new System.Drawing.Point(718, 268);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(64, 49);
            this.button_test.TabIndex = 8;
            this.button_test.Text = "...";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(228, 268);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(107, 49);
            this.button_close.TabIndex = 9;
            this.button_close.Text = "关闭";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 507);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.button_open);
            this.Controls.Add(this.textBox_jia);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_area);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_serverUrl);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_serverUrl;
        private System.Windows.Forms.TextBox textBox_area;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_jia;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.Button button_close;
    }
}

