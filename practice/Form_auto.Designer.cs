﻿namespace practice
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
            this.button_readerLogin_reader = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_readerLogin_item3 = new System.Windows.Forms.Button();
            this.button_readerLogin_item2 = new System.Windows.Forms.Button();
            this.button_readerLogin_arrived = new System.Windows.Forms.Button();
            this.button_readerLogin_amerce = new System.Windows.Forms.Button();
            this.button_readerLogin_issue = new System.Windows.Forms.Button();
            this.button_readerLogin_comment = new System.Windows.Forms.Button();
            this.button_readerLogin_order = new System.Windows.Forms.Button();
            this.button_readerLogin_item = new System.Windows.Forms.Button();
            this.button_readerLogin_biblio = new System.Windows.Forms.Button();
            this.button_GetItemInfo = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_accessAndObject
            // 
            this.button_accessAndObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_accessAndObject.Location = new System.Drawing.Point(943, 10);
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
            this.webBrowser1.Location = new System.Drawing.Point(31, 279);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1237, 465);
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
            // button_readerLogin_reader
            // 
            this.button_readerLogin_reader.Location = new System.Drawing.Point(23, 37);
            this.button_readerLogin_reader.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_reader.Name = "button_readerLogin_reader";
            this.button_readerLogin_reader.Size = new System.Drawing.Size(114, 52);
            this.button_readerLogin_reader.TabIndex = 68;
            this.button_readerLogin_reader.Tag = "";
            this.button_readerLogin_reader.Text = "读者";
            this.button_readerLogin_reader.UseVisualStyleBackColor = true;
            this.button_readerLogin_reader.Click += new System.EventHandler(this.button_readerLogin_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_readerLogin_item3);
            this.groupBox1.Controls.Add(this.button_readerLogin_item2);
            this.groupBox1.Controls.Add(this.button_readerLogin_arrived);
            this.groupBox1.Controls.Add(this.button_readerLogin_amerce);
            this.groupBox1.Controls.Add(this.button_readerLogin_issue);
            this.groupBox1.Controls.Add(this.button_readerLogin_comment);
            this.groupBox1.Controls.Add(this.button_readerLogin_order);
            this.groupBox1.Controls.Add(this.button_readerLogin_item);
            this.groupBox1.Controls.Add(this.button_readerLogin_biblio);
            this.groupBox1.Controls.Add(this.button_readerLogin_reader);
            this.groupBox1.Location = new System.Drawing.Point(31, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1237, 185);
            this.groupBox1.TabIndex = 69;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "读者身份";
            // 
            // button_readerLogin_item3
            // 
            this.button_readerLogin_item3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_readerLogin_item3.Location = new System.Drawing.Point(879, 37);
            this.button_readerLogin_item3.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_item3.Name = "button_readerLogin_item3";
            this.button_readerLogin_item3.Size = new System.Drawing.Size(241, 52);
            this.button_readerLogin_item3.TabIndex = 77;
            this.button_readerLogin_item3.Tag = "";
            this.button_readerLogin_item3.Text = "册(有借阅信息)";
            this.button_readerLogin_item3.UseVisualStyleBackColor = false;
            this.button_readerLogin_item3.Click += new System.EventHandler(this.button_readerLogin_item3_Click);
            // 
            // button_readerLogin_item2
            // 
            this.button_readerLogin_item2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_readerLogin_item2.Location = new System.Drawing.Point(628, 37);
            this.button_readerLogin_item2.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_item2.Name = "button_readerLogin_item2";
            this.button_readerLogin_item2.Size = new System.Drawing.Size(241, 52);
            this.button_readerLogin_item2.TabIndex = 76;
            this.button_readerLogin_item2.Tag = "";
            this.button_readerLogin_item2.Text = "册(读者有书斋)";
            this.button_readerLogin_item2.UseVisualStyleBackColor = false;
            this.button_readerLogin_item2.Click += new System.EventHandler(this.button_readerLogin_item2_Click);
            // 
            // button_readerLogin_arrived
            // 
            this.button_readerLogin_arrived.BackColor = System.Drawing.SystemColors.Highlight;
            this.button_readerLogin_arrived.Location = new System.Drawing.Point(199, 115);
            this.button_readerLogin_arrived.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_arrived.Name = "button_readerLogin_arrived";
            this.button_readerLogin_arrived.Size = new System.Drawing.Size(157, 52);
            this.button_readerLogin_arrived.TabIndex = 75;
            this.button_readerLogin_arrived.Tag = "";
            this.button_readerLogin_arrived.Text = "预约到书";
            this.button_readerLogin_arrived.UseVisualStyleBackColor = false;
            this.button_readerLogin_arrived.Click += new System.EventHandler(this.button_readerLogin_arrived_Click);
            // 
            // button_readerLogin_amerce
            // 
            this.button_readerLogin_amerce.BackColor = System.Drawing.SystemColors.Highlight;
            this.button_readerLogin_amerce.Location = new System.Drawing.Point(366, 115);
            this.button_readerLogin_amerce.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_amerce.Name = "button_readerLogin_amerce";
            this.button_readerLogin_amerce.Size = new System.Drawing.Size(137, 52);
            this.button_readerLogin_amerce.TabIndex = 74;
            this.button_readerLogin_amerce.Tag = "";
            this.button_readerLogin_amerce.Text = "违约金";
            this.button_readerLogin_amerce.UseVisualStyleBackColor = false;
            this.button_readerLogin_amerce.Click += new System.EventHandler(this.button_readerLogin_amerce_Click);
            // 
            // button_readerLogin_issue
            // 
            this.button_readerLogin_issue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_readerLogin_issue.Location = new System.Drawing.Point(719, 115);
            this.button_readerLogin_issue.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_issue.Name = "button_readerLogin_issue";
            this.button_readerLogin_issue.Size = new System.Drawing.Size(120, 52);
            this.button_readerLogin_issue.TabIndex = 73;
            this.button_readerLogin_issue.Tag = "";
            this.button_readerLogin_issue.Text = "期";
            this.button_readerLogin_issue.UseVisualStyleBackColor = false;
            this.button_readerLogin_issue.Click += new System.EventHandler(this.button_readerLogin_issue_Click);
            // 
            // button_readerLogin_comment
            // 
            this.button_readerLogin_comment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.button_readerLogin_comment.Location = new System.Drawing.Point(23, 115);
            this.button_readerLogin_comment.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_comment.Name = "button_readerLogin_comment";
            this.button_readerLogin_comment.Size = new System.Drawing.Size(114, 52);
            this.button_readerLogin_comment.TabIndex = 72;
            this.button_readerLogin_comment.Tag = "";
            this.button_readerLogin_comment.Text = "评注";
            this.button_readerLogin_comment.UseVisualStyleBackColor = false;
            this.button_readerLogin_comment.Click += new System.EventHandler(this.button_readerLogin_comment_Click);
            // 
            // button_readerLogin_order
            // 
            this.button_readerLogin_order.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_readerLogin_order.Location = new System.Drawing.Point(572, 115);
            this.button_readerLogin_order.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_order.Name = "button_readerLogin_order";
            this.button_readerLogin_order.Size = new System.Drawing.Size(137, 52);
            this.button_readerLogin_order.TabIndex = 71;
            this.button_readerLogin_order.Tag = "";
            this.button_readerLogin_order.Text = "订购";
            this.button_readerLogin_order.UseVisualStyleBackColor = false;
            this.button_readerLogin_order.Click += new System.EventHandler(this.button_readerLogin_order_Click);
            // 
            // button_readerLogin_item
            // 
            this.button_readerLogin_item.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_readerLogin_item.Location = new System.Drawing.Point(376, 37);
            this.button_readerLogin_item.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_item.Name = "button_readerLogin_item";
            this.button_readerLogin_item.Size = new System.Drawing.Size(242, 52);
            this.button_readerLogin_item.TabIndex = 70;
            this.button_readerLogin_item.Tag = "";
            this.button_readerLogin_item.Text = "册(读者无书斋)";
            this.button_readerLogin_item.UseVisualStyleBackColor = false;
            this.button_readerLogin_item.Click += new System.EventHandler(this.button_readerLogin_item_Click);
            // 
            // button_readerLogin_biblio
            // 
            this.button_readerLogin_biblio.BackColor = System.Drawing.SystemColors.Info;
            this.button_readerLogin_biblio.Location = new System.Drawing.Point(199, 37);
            this.button_readerLogin_biblio.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_biblio.Name = "button_readerLogin_biblio";
            this.button_readerLogin_biblio.Size = new System.Drawing.Size(103, 52);
            this.button_readerLogin_biblio.TabIndex = 69;
            this.button_readerLogin_biblio.Tag = "";
            this.button_readerLogin_biblio.Text = "书目";
            this.button_readerLogin_biblio.UseVisualStyleBackColor = false;
            this.button_readerLogin_biblio.Click += new System.EventHandler(this.button_readerLogin_biblio_Click);
            // 
            // button_GetItemInfo
            // 
            this.button_GetItemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_GetItemInfo.Location = new System.Drawing.Point(691, 10);
            this.button_GetItemInfo.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_GetItemInfo.Name = "button_GetItemInfo";
            this.button_GetItemInfo.Size = new System.Drawing.Size(242, 56);
            this.button_GetItemInfo.TabIndex = 78;
            this.button_GetItemInfo.Tag = "";
            this.button_GetItemInfo.Text = "测试GetItemInfo";
            this.button_GetItemInfo.UseVisualStyleBackColor = true;
            this.button_GetItemInfo.Click += new System.EventHandler(this.button_GetItemInfo_Click);
            // 
            // Form_auto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 777);
            this.Controls.Add(this.button_GetItemInfo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_testRight);
            this.Controls.Add(this.comboBox_TestRight_type);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button_accessAndObject);
            this.Name = "Form_auto";
            this.Text = "自动测试";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_accessAndObject;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_TestRight_type;
        private System.Windows.Forms.Button button_testRight;
        private System.Windows.Forms.Button button_readerLogin_reader;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_readerLogin_arrived;
        private System.Windows.Forms.Button button_readerLogin_amerce;
        private System.Windows.Forms.Button button_readerLogin_issue;
        private System.Windows.Forms.Button button_readerLogin_comment;
        private System.Windows.Forms.Button button_readerLogin_order;
        private System.Windows.Forms.Button button_readerLogin_item;
        private System.Windows.Forms.Button button_readerLogin_biblio;
        private System.Windows.Forms.Button button_readerLogin_item2;
        private System.Windows.Forms.Button button_readerLogin_item3;
        private System.Windows.Forms.Button button_GetItemInfo;
    }
}