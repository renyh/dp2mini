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
            this.button_reader = new System.Windows.Forms.Button();
            this.button_biblio = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_accessAndObject
            // 
            this.button_accessAndObject.Location = new System.Drawing.Point(14, 12);
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
            this.webBrowser1.Location = new System.Drawing.Point(14, 92);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1624, 658);
            this.webBrowser1.TabIndex = 62;
            // 
            // button_reader
            // 
            this.button_reader.Location = new System.Drawing.Point(349, 12);
            this.button_reader.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader.Name = "button_reader";
            this.button_reader.Size = new System.Drawing.Size(298, 56);
            this.button_reader.TabIndex = 63;
            this.button_reader.Tag = "";
            this.button_reader.Text = "读者及对象所需权限";
            this.button_reader.UseVisualStyleBackColor = true;
            this.button_reader.Click += new System.EventHandler(this.button_reader_Click);
            // 
            // button_biblio
            // 
            this.button_biblio.Location = new System.Drawing.Point(657, 12);
            this.button_biblio.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_biblio.Name = "button_biblio";
            this.button_biblio.Size = new System.Drawing.Size(298, 56);
            this.button_biblio.TabIndex = 64;
            this.button_biblio.Tag = "";
            this.button_biblio.Text = "书目及对象所需权限";
            this.button_biblio.UseVisualStyleBackColor = true;
            this.button_biblio.Click += new System.EventHandler(this.button_biblio_Click);
            // 
            // Form_auto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1668, 783);
            this.Controls.Add(this.button_biblio);
            this.Controls.Add(this.button_reader);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button_accessAndObject);
            this.Name = "Form_auto";
            this.Text = "自动测试";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_accessAndObject;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button button_reader;
        private System.Windows.Forms.Button button_biblio;
    }
}