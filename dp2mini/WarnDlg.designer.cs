﻿namespace dp2mini
{
    partial class WarmDlg
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
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_info
            // 
            this.textBox_info.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_info.Location = new System.Drawing.Point(24, 12);
            this.textBox_info.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ReadOnly = true;
            this.textBox_info.Size = new System.Drawing.Size(396, 165);
            this.textBox_info.TabIndex = 1;
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(278, 192);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(101, 41);
            this.button_cancel.TabIndex = 6;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(61, 192);
            this.button_ok.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(189, 41);
            this.button_ok.TabIndex = 5;
            this.button_ok.Text = "继续";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // WarmDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 266);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.textBox_info);
            this.Name = "WarmDlg";
            this.Text = "通知读者";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_info;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_ok;
    }
}