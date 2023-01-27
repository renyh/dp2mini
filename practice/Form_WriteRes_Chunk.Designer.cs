namespace practice
{
    partial class Form_WriteRes_Chunk
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
            this.label_info = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.textBox_WriteRes_chunkSize = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_info
            // 
            this.label_info.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_info.BackColor = System.Drawing.SystemColors.Info;
            this.label_info.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info.Location = new System.Drawing.Point(33, 32);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(725, 117);
            this.label_info.TabIndex = 40;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.Location = new System.Drawing.Point(633, 379);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(157, 62);
            this.button_cancel.TabIndex = 42;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ok.Location = new System.Drawing.Point(475, 379);
            this.button_ok.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(146, 62);
            this.button_ok.TabIndex = 41;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // textBox_WriteRes_chunkSize
            // 
            this.textBox_WriteRes_chunkSize.Location = new System.Drawing.Point(275, 189);
            this.textBox_WriteRes_chunkSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox_WriteRes_chunkSize.Name = "textBox_WriteRes_chunkSize";
            this.textBox_WriteRes_chunkSize.Size = new System.Drawing.Size(321, 38);
            this.textBox_WriteRes_chunkSize.TabIndex = 43;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(50, 200);
            this.label61.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(204, 27);
            this.label61.TabIndex = 44;
            this.label61.Text = "小包尺寸(Byte)";
            // 
            // Form_WriteRes_Chunk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 461);
            this.Controls.Add(this.textBox_WriteRes_chunkSize);
            this.Controls.Add(this.label61);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.label_info);
            this.Name = "Form_WriteRes_Chunk";
            this.Text = "设置包尺寸";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.TextBox textBox_WriteRes_chunkSize;
        private System.Windows.Forms.Label label61;
    }
}