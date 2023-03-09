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
            this.button_reader = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_accountType = new System.Windows.Forms.ComboBox();
            this.button_readerLogin_arrived = new System.Windows.Forms.Button();
            this.button_readerLogin_amerce = new System.Windows.Forms.Button();
            this.button_readerLogin_issue = new System.Windows.Forms.Button();
            this.button_readerLogin_comment = new System.Windows.Forms.Button();
            this.button_readerLogin_order = new System.Windows.Forms.Button();
            this.button_item = new System.Windows.Forms.Button();
            this.button_biblio = new System.Windows.Forms.Button();
            this.button_ItemHasBorrow = new System.Windows.Forms.Button();
            this.button_GetItemInfo = new System.Windows.Forms.Button();
            this.button_createEnv = new System.Windows.Forms.Button();
            this.button_delEnv = new System.Windows.Forms.Button();
            this.button_deleteBiblioHHasChildren = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_accessAndObject
            // 
            this.button_accessAndObject.Location = new System.Drawing.Point(755, 27);
            this.button_accessAndObject.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_accessAndObject.Name = "button_accessAndObject";
            this.button_accessAndObject.Size = new System.Drawing.Size(213, 56);
            this.button_accessAndObject.TabIndex = 58;
            this.button_accessAndObject.Tag = "";
            this.button_accessAndObject.Text = "存取定义与对象";
            this.button_accessAndObject.UseVisualStyleBackColor = true;
            this.button_accessAndObject.Click += new System.EventHandler(this.button_accessAndObject_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(19, 169);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1456, 663);
            this.webBrowser1.TabIndex = 62;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 27);
            this.label1.TabIndex = 65;
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
            this.comboBox_TestRight_type.Location = new System.Drawing.Point(35, 37);
            this.comboBox_TestRight_type.Name = "comboBox_TestRight_type";
            this.comboBox_TestRight_type.Size = new System.Drawing.Size(114, 35);
            this.comboBox_TestRight_type.TabIndex = 66;
            this.comboBox_TestRight_type.Text = "读者";
            // 
            // button_testRight
            // 
            this.button_testRight.Location = new System.Drawing.Point(155, 29);
            this.button_testRight.Name = "button_testRight";
            this.button_testRight.Size = new System.Drawing.Size(139, 50);
            this.button_testRight.TabIndex = 67;
            this.button_testRight.Text = "权限测试";
            this.button_testRight.UseVisualStyleBackColor = true;
            this.button_testRight.Click += new System.EventHandler(this.button_testRight_Click);
            // 
            // button_reader
            // 
            this.button_reader.Location = new System.Drawing.Point(361, 94);
            this.button_reader.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader.Name = "button_reader";
            this.button_reader.Size = new System.Drawing.Size(91, 50);
            this.button_reader.TabIndex = 68;
            this.button_reader.Tag = "";
            this.button_reader.Text = "读者";
            this.button_reader.UseVisualStyleBackColor = true;
            this.button_reader.Click += new System.EventHandler(this.button_reader_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 27);
            this.label2.TabIndex = 79;
            this.label2.Text = "选择身份";
            // 
            // comboBox_accountType
            // 
            this.comboBox_accountType.FormattingEnabled = true;
            this.comboBox_accountType.Items.AddRange(new object[] {
            "总馆工作人员",
            "总馆读者",
            "分馆工作人员",
            "分馆读者"});
            this.comboBox_accountType.Location = new System.Drawing.Point(165, 103);
            this.comboBox_accountType.Name = "comboBox_accountType";
            this.comboBox_accountType.Size = new System.Drawing.Size(188, 35);
            this.comboBox_accountType.TabIndex = 78;
            this.comboBox_accountType.Text = "总馆读者";
            // 
            // button_readerLogin_arrived
            // 
            this.button_readerLogin_arrived.BackColor = System.Drawing.SystemColors.Highlight;
            this.button_readerLogin_arrived.Location = new System.Drawing.Point(806, 94);
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
            this.button_readerLogin_amerce.Location = new System.Drawing.Point(973, 94);
            this.button_readerLogin_amerce.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_amerce.Name = "button_readerLogin_amerce";
            this.button_readerLogin_amerce.Size = new System.Drawing.Size(129, 52);
            this.button_readerLogin_amerce.TabIndex = 74;
            this.button_readerLogin_amerce.Tag = "";
            this.button_readerLogin_amerce.Text = "违约金";
            this.button_readerLogin_amerce.UseVisualStyleBackColor = false;
            this.button_readerLogin_amerce.Click += new System.EventHandler(this.button_readerLogin_amerce_Click);
            // 
            // button_readerLogin_issue
            // 
            this.button_readerLogin_issue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_readerLogin_issue.Location = new System.Drawing.Point(1230, 94);
            this.button_readerLogin_issue.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_issue.Name = "button_readerLogin_issue";
            this.button_readerLogin_issue.Size = new System.Drawing.Size(81, 52);
            this.button_readerLogin_issue.TabIndex = 73;
            this.button_readerLogin_issue.Tag = "";
            this.button_readerLogin_issue.Text = "期";
            this.button_readerLogin_issue.UseVisualStyleBackColor = false;
            this.button_readerLogin_issue.Click += new System.EventHandler(this.button_readerLogin_issue_Click);
            // 
            // button_readerLogin_comment
            // 
            this.button_readerLogin_comment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.button_readerLogin_comment.Location = new System.Drawing.Point(682, 94);
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
            this.button_readerLogin_order.Location = new System.Drawing.Point(1112, 94);
            this.button_readerLogin_order.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_readerLogin_order.Name = "button_readerLogin_order";
            this.button_readerLogin_order.Size = new System.Drawing.Size(108, 52);
            this.button_readerLogin_order.TabIndex = 71;
            this.button_readerLogin_order.Tag = "";
            this.button_readerLogin_order.Text = "订购";
            this.button_readerLogin_order.UseVisualStyleBackColor = false;
            this.button_readerLogin_order.Click += new System.EventHandler(this.button_readerLogin_order_Click);
            // 
            // button_item
            // 
            this.button_item.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.button_item.Location = new System.Drawing.Point(572, 94);
            this.button_item.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_item.Name = "button_item";
            this.button_item.Size = new System.Drawing.Size(88, 50);
            this.button_item.TabIndex = 70;
            this.button_item.Tag = "";
            this.button_item.Text = "册";
            this.button_item.UseVisualStyleBackColor = false;
            this.button_item.Click += new System.EventHandler(this.button_item_Click);
            // 
            // button_biblio
            // 
            this.button_biblio.BackColor = System.Drawing.SystemColors.Info;
            this.button_biblio.Location = new System.Drawing.Point(462, 94);
            this.button_biblio.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_biblio.Name = "button_biblio";
            this.button_biblio.Size = new System.Drawing.Size(87, 50);
            this.button_biblio.TabIndex = 69;
            this.button_biblio.Tag = "";
            this.button_biblio.Text = "书目";
            this.button_biblio.UseVisualStyleBackColor = false;
            this.button_biblio.Click += new System.EventHandler(this.button_biblio_Click);
            // 
            // button_ItemHasBorrow
            // 
            this.button_ItemHasBorrow.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.button_ItemHasBorrow.Location = new System.Drawing.Point(302, 29);
            this.button_ItemHasBorrow.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_ItemHasBorrow.Name = "button_ItemHasBorrow";
            this.button_ItemHasBorrow.Size = new System.Drawing.Size(250, 52);
            this.button_ItemHasBorrow.TabIndex = 77;
            this.button_ItemHasBorrow.Tag = "";
            this.button_ItemHasBorrow.Text = "获取有借阅者的册";
            this.button_ItemHasBorrow.UseVisualStyleBackColor = false;
            this.button_ItemHasBorrow.Click += new System.EventHandler(this.button_readerLogin_item3_Click);
            // 
            // button_GetItemInfo
            // 
            this.button_GetItemInfo.Location = new System.Drawing.Point(562, 27);
            this.button_GetItemInfo.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_GetItemInfo.Name = "button_GetItemInfo";
            this.button_GetItemInfo.Size = new System.Drawing.Size(183, 56);
            this.button_GetItemInfo.TabIndex = 78;
            this.button_GetItemInfo.Tag = "";
            this.button_GetItemInfo.Text = "GetItemInfo";
            this.button_GetItemInfo.UseVisualStyleBackColor = true;
            this.button_GetItemInfo.Click += new System.EventHandler(this.button_GetItemInfo_Click);
            // 
            // button_createEnv
            // 
            this.button_createEnv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_createEnv.Location = new System.Drawing.Point(1114, 30);
            this.button_createEnv.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_createEnv.Name = "button_createEnv";
            this.button_createEnv.Size = new System.Drawing.Size(140, 52);
            this.button_createEnv.TabIndex = 70;
            this.button_createEnv.Tag = "";
            this.button_createEnv.Text = "创建环境";
            this.button_createEnv.UseVisualStyleBackColor = true;
            this.button_createEnv.Click += new System.EventHandler(this.button_createZfgEnv_Click);
            // 
            // button_delEnv
            // 
            this.button_delEnv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_delEnv.Location = new System.Drawing.Point(1264, 30);
            this.button_delEnv.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_delEnv.Name = "button_delEnv";
            this.button_delEnv.Size = new System.Drawing.Size(151, 52);
            this.button_delEnv.TabIndex = 79;
            this.button_delEnv.Tag = "";
            this.button_delEnv.Text = "删除环境";
            this.button_delEnv.UseVisualStyleBackColor = true;
            this.button_delEnv.Click += new System.EventHandler(this.button_delEnv_Click);
            // 
            // button_deleteBiblioHHasChildren
            // 
            this.button_deleteBiblioHHasChildren.Location = new System.Drawing.Point(978, 31);
            this.button_deleteBiblioHHasChildren.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_deleteBiblioHHasChildren.Name = "button_deleteBiblioHHasChildren";
            this.button_deleteBiblioHHasChildren.Size = new System.Drawing.Size(258, 56);
            this.button_deleteBiblioHHasChildren.TabIndex = 80;
            this.button_deleteBiblioHHasChildren.Tag = "";
            this.button_deleteBiblioHHasChildren.Text = "删有下级的书目";
            this.button_deleteBiblioHHasChildren.UseVisualStyleBackColor = true;
            this.button_deleteBiblioHHasChildren.Click += new System.EventHandler(this.button_deleteBiblioHHasChildren_Click);
            // 
            // Form_auto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(216F, 216F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1530, 865);
            this.Controls.Add(this.button_deleteBiblioHHasChildren);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_accountType);
            this.Controls.Add(this.button_delEnv);
            this.Controls.Add(this.button_readerLogin_arrived);
            this.Controls.Add(this.button_GetItemInfo);
            this.Controls.Add(this.button_readerLogin_amerce);
            this.Controls.Add(this.button_createEnv);
            this.Controls.Add(this.button_readerLogin_issue);
            this.Controls.Add(this.button_ItemHasBorrow);
            this.Controls.Add(this.button_readerLogin_comment);
            this.Controls.Add(this.button_readerLogin_order);
            this.Controls.Add(this.button_testRight);
            this.Controls.Add(this.button_item);
            this.Controls.Add(this.comboBox_TestRight_type);
            this.Controls.Add(this.button_biblio);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_reader);
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
        private System.Windows.Forms.Button button_reader;
        private System.Windows.Forms.Button button_readerLogin_arrived;
        private System.Windows.Forms.Button button_readerLogin_amerce;
        private System.Windows.Forms.Button button_readerLogin_issue;
        private System.Windows.Forms.Button button_readerLogin_comment;
        private System.Windows.Forms.Button button_readerLogin_order;
        private System.Windows.Forms.Button button_item;
        private System.Windows.Forms.Button button_biblio;
        private System.Windows.Forms.Button button_ItemHasBorrow;
        private System.Windows.Forms.Button button_GetItemInfo;
        private System.Windows.Forms.Button button_createEnv;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_accountType;
        private System.Windows.Forms.Button button_delEnv;
        private System.Windows.Forms.Button button_deleteBiblioHHasChildren;
    }
}