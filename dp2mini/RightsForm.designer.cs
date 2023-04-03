namespace dp2mini
{
    partial class RightsForm
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
            this.button_createXml = new System.Windows.Forms.Button();
            this.label28 = new System.Windows.Forms.Label();
            this.textBox_excel = new System.Windows.Forms.TextBox();
            this.button_getExcelFile = new System.Windows.Forms.Button();
            this.button_createMarkDown = new System.Windows.Forms.Button();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.button_getXmlFile = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.textBox_xml = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_createXml
            // 
            this.button_createXml.Location = new System.Drawing.Point(222, 73);
            this.button_createXml.Name = "button_createXml";
            this.button_createXml.Size = new System.Drawing.Size(185, 47);
            this.button_createXml.TabIndex = 59;
            this.button_createXml.Text = "生成Xml";
            this.button_createXml.UseVisualStyleBackColor = true;
            this.button_createXml.Click += new System.EventHandler(this.button_createXml_Click);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(18, 29);
            this.label28.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(190, 27);
            this.label28.TabIndex = 58;
            this.label28.Text = "权限总表Excel";
            // 
            // textBox_excel
            // 
            this.textBox_excel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_excel.Location = new System.Drawing.Point(222, 23);
            this.textBox_excel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_excel.Name = "textBox_excel";
            this.textBox_excel.Size = new System.Drawing.Size(645, 38);
            this.textBox_excel.TabIndex = 57;
            // 
            // button_getExcelFile
            // 
            this.button_getExcelFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getExcelFile.Location = new System.Drawing.Point(875, 21);
            this.button_getExcelFile.Name = "button_getExcelFile";
            this.button_getExcelFile.Size = new System.Drawing.Size(80, 42);
            this.button_getExcelFile.TabIndex = 60;
            this.button_getExcelFile.Text = "...";
            this.button_getExcelFile.UseVisualStyleBackColor = true;
            this.button_getExcelFile.Click += new System.EventHandler(this.button_getExcelFile_Click);
            // 
            // button_createMarkDown
            // 
            this.button_createMarkDown.Location = new System.Drawing.Point(445, 73);
            this.button_createMarkDown.Name = "button_createMarkDown";
            this.button_createMarkDown.Size = new System.Drawing.Size(246, 47);
            this.button_createMarkDown.TabIndex = 61;
            this.button_createMarkDown.Text = "生成MarkDown";
            this.button_createMarkDown.UseVisualStyleBackColor = true;
            this.button_createMarkDown.Click += new System.EventHandler(this.button_createMarkDown_Click);
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_result.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_result.Location = new System.Drawing.Point(23, 193);
            this.textBox_result.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ReadOnly = true;
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(984, 485);
            this.textBox_result.TabIndex = 62;
            // 
            // button_getXmlFile
            // 
            this.button_getXmlFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_getXmlFile.Location = new System.Drawing.Point(875, 136);
            this.button_getXmlFile.Name = "button_getXmlFile";
            this.button_getXmlFile.Size = new System.Drawing.Size(80, 42);
            this.button_getXmlFile.TabIndex = 65;
            this.button_getXmlFile.Text = "...";
            this.button_getXmlFile.UseVisualStyleBackColor = true;
            this.button_getXmlFile.Click += new System.EventHandler(this.button_getXmlFile_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(46, 139);
            this.label27.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(162, 27);
            this.label27.TabIndex = 64;
            this.label27.Text = "目标xml文件";
            // 
            // textBox_xml
            // 
            this.textBox_xml.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_xml.Location = new System.Drawing.Point(222, 136);
            this.textBox_xml.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_xml.Name = "textBox_xml";
            this.textBox_xml.Size = new System.Drawing.Size(645, 38);
            this.textBox_xml.TabIndex = 63;
            // 
            // RightsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1031, 704);
            this.Controls.Add(this.button_getXmlFile);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.textBox_xml);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.button_createMarkDown);
            this.Controls.Add(this.button_getExcelFile);
            this.Controls.Add(this.button_createXml);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.textBox_excel);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "RightsForm";
            this.Text = "权限工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_createXml;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox textBox_excel;
        private System.Windows.Forms.Button button_getExcelFile;
        private System.Windows.Forms.Button button_createMarkDown;
        private System.Windows.Forms.TextBox textBox_result;
        private System.Windows.Forms.Button button_getXmlFile;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox textBox_xml;
    }
}