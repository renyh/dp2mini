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
            this.button_leader = new System.Windows.Forms.Button();
            this.button_getrecord = new System.Windows.Forms.Button();
            this.button_GetBrowseRecords = new System.Windows.Forms.Button();
            this.button_CopyBiblioInfo = new System.Windows.Forms.Button();
            this.button_token = new System.Windows.Forms.Button();
            this.button_resultSet = new System.Windows.Forms.Button();
            this.button_reader_19 = new System.Windows.Forms.Button();
            this.button_reader_fields = new System.Windows.Forms.Button();
            this.button_reader_saved = new System.Windows.Forms.Button();
            this.button_reader_mask = new System.Windows.Forms.Button();
            this.button_reader_write = new System.Windows.Forms.Button();
            this.button_reader_wDr = new System.Windows.Forms.Button();
            this.button_reader_delete = new System.Windows.Forms.Button();
            this.button_importantFields = new System.Windows.Forms.Button();
            this.button_dataFields = new System.Windows.Forms.Button();
            this.button_price = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_accessAndObject
            // 
            this.button_accessAndObject.Location = new System.Drawing.Point(745, 72);
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
            this.webBrowser1.Location = new System.Drawing.Point(19, 262);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(1472, 535);
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
            this.comboBox_TestRight_type.Location = new System.Drawing.Point(19, 84);
            this.comboBox_TestRight_type.Name = "comboBox_TestRight_type";
            this.comboBox_TestRight_type.Size = new System.Drawing.Size(114, 35);
            this.comboBox_TestRight_type.TabIndex = 66;
            this.comboBox_TestRight_type.Text = "读者";
            // 
            // button_testRight
            // 
            this.button_testRight.Location = new System.Drawing.Point(145, 75);
            this.button_testRight.Name = "button_testRight";
            this.button_testRight.Size = new System.Drawing.Size(139, 50);
            this.button_testRight.TabIndex = 67;
            this.button_testRight.Text = "权限测试";
            this.button_testRight.UseVisualStyleBackColor = true;
            this.button_testRight.Click += new System.EventHandler(this.button_testRight_Click);
            // 
            // button_reader
            // 
            this.button_reader.Location = new System.Drawing.Point(345, 132);
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
            this.label2.Location = new System.Drawing.Point(23, 144);
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
            this.comboBox_accountType.Location = new System.Drawing.Point(149, 140);
            this.comboBox_accountType.Name = "comboBox_accountType";
            this.comboBox_accountType.Size = new System.Drawing.Size(188, 35);
            this.comboBox_accountType.TabIndex = 78;
            this.comboBox_accountType.Text = "总馆读者";
            // 
            // button_readerLogin_arrived
            // 
            this.button_readerLogin_arrived.BackColor = System.Drawing.SystemColors.Highlight;
            this.button_readerLogin_arrived.Location = new System.Drawing.Point(790, 131);
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
            this.button_readerLogin_amerce.Location = new System.Drawing.Point(957, 131);
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
            this.button_readerLogin_issue.Location = new System.Drawing.Point(1214, 131);
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
            this.button_readerLogin_comment.Location = new System.Drawing.Point(666, 131);
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
            this.button_readerLogin_order.Location = new System.Drawing.Point(1096, 131);
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
            this.button_item.Location = new System.Drawing.Point(556, 132);
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
            this.button_biblio.Location = new System.Drawing.Point(446, 132);
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
            this.button_ItemHasBorrow.Location = new System.Drawing.Point(292, 74);
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
            this.button_GetItemInfo.Location = new System.Drawing.Point(552, 72);
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
            this.button_createEnv.Location = new System.Drawing.Point(19, 12);
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
            this.button_delEnv.Location = new System.Drawing.Point(169, 12);
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
            this.button_deleteBiblioHHasChildren.Location = new System.Drawing.Point(968, 72);
            this.button_deleteBiblioHHasChildren.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_deleteBiblioHHasChildren.Name = "button_deleteBiblioHHasChildren";
            this.button_deleteBiblioHHasChildren.Size = new System.Drawing.Size(258, 56);
            this.button_deleteBiblioHHasChildren.TabIndex = 80;
            this.button_deleteBiblioHHasChildren.Tag = "";
            this.button_deleteBiblioHHasChildren.Text = "删有下级的书目";
            this.button_deleteBiblioHHasChildren.UseVisualStyleBackColor = true;
            this.button_deleteBiblioHHasChildren.Click += new System.EventHandler(this.button_deleteBiblioHHasChildren_Click);
            // 
            // button_leader
            // 
            this.button_leader.Location = new System.Drawing.Point(345, 12);
            this.button_leader.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_leader.Name = "button_leader";
            this.button_leader.Size = new System.Drawing.Size(151, 52);
            this.button_leader.TabIndex = 81;
            this.button_leader.Tag = "";
            this.button_leader.Text = "头标区";
            this.button_leader.UseVisualStyleBackColor = true;
            this.button_leader.Click += new System.EventHandler(this.button_leader_Click);
            // 
            // button_getrecord
            // 
            this.button_getrecord.Location = new System.Drawing.Point(506, 12);
            this.button_getrecord.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_getrecord.Name = "button_getrecord";
            this.button_getrecord.Size = new System.Drawing.Size(207, 52);
            this.button_getrecord.TabIndex = 82;
            this.button_getrecord.Tag = "";
            this.button_getrecord.Text = "GetRecord权限";
            this.button_getrecord.UseVisualStyleBackColor = true;
            this.button_getrecord.Click += new System.EventHandler(this.button_getrecord_Click);
            // 
            // button_GetBrowseRecords
            // 
            this.button_GetBrowseRecords.Location = new System.Drawing.Point(723, 12);
            this.button_GetBrowseRecords.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_GetBrowseRecords.Name = "button_GetBrowseRecords";
            this.button_GetBrowseRecords.Size = new System.Drawing.Size(251, 52);
            this.button_GetBrowseRecords.TabIndex = 83;
            this.button_GetBrowseRecords.Tag = "";
            this.button_GetBrowseRecords.Text = "GetBrowseRecords";
            this.button_GetBrowseRecords.UseVisualStyleBackColor = true;
            this.button_GetBrowseRecords.Click += new System.EventHandler(this.button_GetBrowseRecords_Click);
            // 
            // button_CopyBiblioInfo
            // 
            this.button_CopyBiblioInfo.Location = new System.Drawing.Point(984, 14);
            this.button_CopyBiblioInfo.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_CopyBiblioInfo.Name = "button_CopyBiblioInfo";
            this.button_CopyBiblioInfo.Size = new System.Drawing.Size(230, 52);
            this.button_CopyBiblioInfo.TabIndex = 84;
            this.button_CopyBiblioInfo.Tag = "";
            this.button_CopyBiblioInfo.Text = "CopyBiblioInfo";
            this.button_CopyBiblioInfo.UseVisualStyleBackColor = true;
            this.button_CopyBiblioInfo.Click += new System.EventHandler(this.button_CopyBiblioInfo_Click);
            // 
            // button_token
            // 
            this.button_token.Location = new System.Drawing.Point(1224, 14);
            this.button_token.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_token.Name = "button_token";
            this.button_token.Size = new System.Drawing.Size(101, 52);
            this.button_token.TabIndex = 85;
            this.button_token.Tag = "";
            this.button_token.Text = "Token";
            this.button_token.UseVisualStyleBackColor = true;
            this.button_token.Click += new System.EventHandler(this.button_token_Click);
            // 
            // button_resultSet
            // 
            this.button_resultSet.Location = new System.Drawing.Point(1335, 12);
            this.button_resultSet.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_resultSet.Name = "button_resultSet";
            this.button_resultSet.Size = new System.Drawing.Size(179, 130);
            this.button_resultSet.TabIndex = 86;
            this.button_resultSet.Tag = "";
            this.button_resultSet.Text = "SearchItem-GetSearchResult内嵌检索与结果集";
            this.button_resultSet.UseVisualStyleBackColor = true;
            this.button_resultSet.Click += new System.EventHandler(this.button_resultSet_Click);
            // 
            // button_reader_19
            // 
            this.button_reader_19.Location = new System.Drawing.Point(19, 206);
            this.button_reader_19.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader_19.Name = "button_reader_19";
            this.button_reader_19.Size = new System.Drawing.Size(95, 50);
            this.button_reader_19.TabIndex = 87;
            this.button_reader_19.Tag = "";
            this.button_reader_19.Text = "1-9级";
            this.button_reader_19.UseVisualStyleBackColor = true;
            this.button_reader_19.Click += new System.EventHandler(this.button_reader_19_Click);
            // 
            // button_reader_fields
            // 
            this.button_reader_fields.Location = new System.Drawing.Point(124, 206);
            this.button_reader_fields.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader_fields.Name = "button_reader_fields";
            this.button_reader_fields.Size = new System.Drawing.Size(141, 50);
            this.button_reader_fields.TabIndex = 88;
            this.button_reader_fields.Tag = "";
            this.button_reader_fields.Text = "字段权限";
            this.button_reader_fields.UseVisualStyleBackColor = true;
            this.button_reader_fields.Click += new System.EventHandler(this.button_reader_fields_Click);
            // 
            // button_reader_saved
            // 
            this.button_reader_saved.Location = new System.Drawing.Point(424, 206);
            this.button_reader_saved.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader_saved.Name = "button_reader_saved";
            this.button_reader_saved.Size = new System.Drawing.Size(142, 50);
            this.button_reader_saved.TabIndex = 89;
            this.button_reader_saved.Tag = "";
            this.button_reader_saved.Text = "保护字段";
            this.button_reader_saved.UseVisualStyleBackColor = true;
            this.button_reader_saved.Click += new System.EventHandler(this.button_reader_saved_Click);
            // 
            // button_reader_mask
            // 
            this.button_reader_mask.Location = new System.Drawing.Point(275, 206);
            this.button_reader_mask.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader_mask.Name = "button_reader_mask";
            this.button_reader_mask.Size = new System.Drawing.Size(139, 50);
            this.button_reader_mask.TabIndex = 90;
            this.button_reader_mask.Tag = "";
            this.button_reader_mask.Text = "字段脱敏";
            this.button_reader_mask.UseVisualStyleBackColor = true;
            this.button_reader_mask.Click += new System.EventHandler(this.button_reader_mask_Click);
            // 
            // button_reader_write
            // 
            this.button_reader_write.Location = new System.Drawing.Point(576, 206);
            this.button_reader_write.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader_write.Name = "button_reader_write";
            this.button_reader_write.Size = new System.Drawing.Size(137, 50);
            this.button_reader_write.TabIndex = 91;
            this.button_reader_write.Tag = "";
            this.button_reader_write.Text = "写字段";
            this.button_reader_write.UseVisualStyleBackColor = true;
            this.button_reader_write.Click += new System.EventHandler(this.button_reader_write_Click);
            // 
            // button_reader_wDr
            // 
            this.button_reader_wDr.Location = new System.Drawing.Point(723, 206);
            this.button_reader_wDr.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader_wDr.Name = "button_reader_wDr";
            this.button_reader_wDr.Size = new System.Drawing.Size(120, 50);
            this.button_reader_wDr.TabIndex = 92;
            this.button_reader_wDr.Tag = "";
            this.button_reader_wDr.Text = "写>读";
            this.button_reader_wDr.UseVisualStyleBackColor = true;
            this.button_reader_wDr.Click += new System.EventHandler(this.button_reader_wDr_Click);
            // 
            // button_reader_delete
            // 
            this.button_reader_delete.Location = new System.Drawing.Point(854, 206);
            this.button_reader_delete.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_reader_delete.Name = "button_reader_delete";
            this.button_reader_delete.Size = new System.Drawing.Size(120, 50);
            this.button_reader_delete.TabIndex = 93;
            this.button_reader_delete.Tag = "";
            this.button_reader_delete.Text = "删除";
            this.button_reader_delete.UseVisualStyleBackColor = true;
            this.button_reader_delete.Click += new System.EventHandler(this.button_reader_delete_Click);
            // 
            // button_importantFields
            // 
            this.button_importantFields.Location = new System.Drawing.Point(984, 206);
            this.button_importantFields.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_importantFields.Name = "button_importantFields";
            this.button_importantFields.Size = new System.Drawing.Size(250, 50);
            this.button_importantFields.TabIndex = 94;
            this.button_importantFields.Tag = "";
            this.button_importantFields.Text = "importantFields";
            this.button_importantFields.UseVisualStyleBackColor = true;
            this.button_importantFields.Click += new System.EventHandler(this.button_importantFields_Click);
            // 
            // button_dataFields
            // 
            this.button_dataFields.Location = new System.Drawing.Point(1244, 206);
            this.button_dataFields.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_dataFields.Name = "button_dataFields";
            this.button_dataFields.Size = new System.Drawing.Size(213, 50);
            this.button_dataFields.TabIndex = 95;
            this.button_dataFields.Tag = "";
            this.button_dataFields.Text = "dataFields";
            this.button_dataFields.UseVisualStyleBackColor = true;
            this.button_dataFields.Click += new System.EventHandler(this.button_dataFields_Click);
            // 
            // button_price
            // 
            this.button_price.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button_price.Location = new System.Drawing.Point(1335, 148);
            this.button_price.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.button_price.Name = "button_price";
            this.button_price.Size = new System.Drawing.Size(156, 52);
            this.button_price.TabIndex = 96;
            this.button_price.Tag = "";
            this.button_price.Text = "创建册价格";
            this.button_price.UseVisualStyleBackColor = false;
            this.button_price.Click += new System.EventHandler(this.button_price_Click);
            // 
            // Form_auto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(216F, 216F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1528, 830);
            this.Controls.Add(this.button_price);
            this.Controls.Add(this.button_dataFields);
            this.Controls.Add(this.button_importantFields);
            this.Controls.Add(this.button_reader_delete);
            this.Controls.Add(this.button_reader_wDr);
            this.Controls.Add(this.button_reader_write);
            this.Controls.Add(this.button_reader_mask);
            this.Controls.Add(this.button_reader_saved);
            this.Controls.Add(this.button_reader_fields);
            this.Controls.Add(this.button_reader_19);
            this.Controls.Add(this.button_resultSet);
            this.Controls.Add(this.button_token);
            this.Controls.Add(this.button_CopyBiblioInfo);
            this.Controls.Add(this.button_GetBrowseRecords);
            this.Controls.Add(this.button_getrecord);
            this.Controls.Add(this.button_leader);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_auto_FormClosing);
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
        private System.Windows.Forms.Button button_leader;
        private System.Windows.Forms.Button button_getrecord;
        private System.Windows.Forms.Button button_GetBrowseRecords;
        private System.Windows.Forms.Button button_CopyBiblioInfo;
        private System.Windows.Forms.Button button_token;
        private System.Windows.Forms.Button button_resultSet;
        private System.Windows.Forms.Button button_reader_19;
        private System.Windows.Forms.Button button_reader_fields;
        private System.Windows.Forms.Button button_reader_saved;
        private System.Windows.Forms.Button button_reader_mask;
        private System.Windows.Forms.Button button_reader_write;
        private System.Windows.Forms.Button button_reader_wDr;
        private System.Windows.Forms.Button button_reader_delete;
        private System.Windows.Forms.Button button_importantFields;
        private System.Windows.Forms.Button button_dataFields;
        private System.Windows.Forms.Button button_price;
    }
}