namespace practice
{
    partial class Form_marcField
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
            this.textBox_biblio = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.button_setField = new System.Windows.Forms.Button();
            this.button_getField = new System.Windows.Forms.Button();
            this.textBox_map = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_result = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_biblio
            // 
            this.textBox_biblio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_biblio.Location = new System.Drawing.Point(216, 58);
            this.textBox_biblio.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_biblio.Multiline = true;
            this.textBox_biblio.Name = "textBox_biblio";
            this.textBox_biblio.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_biblio.Size = new System.Drawing.Size(938, 308);
            this.textBox_biblio.TabIndex = 49;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(98, 61);
            this.label29.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(110, 27);
            this.label29.TabIndex = 50;
            this.label29.Text = "MarcXml";
            // 
            // button_setField
            // 
            this.button_setField.Location = new System.Drawing.Point(401, 573);
            this.button_setField.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_setField.Name = "button_setField";
            this.button_setField.Size = new System.Drawing.Size(174, 56);
            this.button_setField.TabIndex = 59;
            this.button_setField.Tag = "";
            this.button_setField.Text = "setField";
            this.button_setField.UseVisualStyleBackColor = true;
            this.button_setField.Click += new System.EventHandler(this.button_setField_Click);
            // 
            // button_getField
            // 
            this.button_getField.Location = new System.Drawing.Point(217, 573);
            this.button_getField.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_getField.Name = "button_getField";
            this.button_getField.Size = new System.Drawing.Size(174, 56);
            this.button_getField.TabIndex = 58;
            this.button_getField.Tag = "";
            this.button_getField.Text = "getField";
            this.button_getField.UseVisualStyleBackColor = true;
            this.button_getField.Click += new System.EventHandler(this.button_getField_Click);
            // 
            // textBox_map
            // 
            this.textBox_map.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_map.Location = new System.Drawing.Point(216, 405);
            this.textBox_map.Multiline = true;
            this.textBox_map.Name = "textBox_map";
            this.textBox_map.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_map.Size = new System.Drawing.Size(930, 152);
            this.textBox_map.TabIndex = 57;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 409);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 27);
            this.label1.TabIndex = 60;
            this.label1.Text = "字段抽取规则";
            // 
            // textBox_result
            // 
            this.textBox_result.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_result.BackColor = System.Drawing.SystemColors.Info;
            this.textBox_result.Location = new System.Drawing.Point(39, 698);
            this.textBox_result.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.textBox_result.Multiline = true;
            this.textBox_result.Name = "textBox_result";
            this.textBox_result.ReadOnly = true;
            this.textBox_result.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_result.Size = new System.Drawing.Size(1115, 308);
            this.textBox_result.TabIndex = 61;
            // 
            // Form_marcField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1198, 1047);
            this.Controls.Add(this.textBox_result);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_setField);
            this.Controls.Add(this.button_getField);
            this.Controls.Add(this.textBox_map);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.textBox_biblio);
            this.Name = "Form_marcField";
            this.Text = "Form_marcField";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_biblio;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button button_setField;
        private System.Windows.Forms.Button button_getField;
        private System.Windows.Forms.TextBox textBox_map;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_result;
    }
}