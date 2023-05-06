namespace dp2billing
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_add = new Button();
            label1 = new Label();
            textBox_account = new TextBox();
            textBox_transactionType = new TextBox();
            label_productType = new Label();
            textBox_amount = new TextBox();
            label2 = new Label();
            textBox_resPath = new TextBox();
            label3 = new Label();
            textBox_reciprocalAccount = new TextBox();
            label4 = new Label();
            textBox_remark = new TextBox();
            label5 = new Label();
            textBox_id = new TextBox();
            label6 = new Label();
            textBox_result = new TextBox();
            button_downloadData = new Button();
            button_initDb = new Button();
            button_buy = new Button();
            button_recharge = new Button();
            button_withdraw = new Button();
            button_getBills = new Button();
            button_getDownloadCount = new Button();
            dateTimePicker_start = new DateTimePicker();
            label7 = new Label();
            dateTimePicker_end = new DateTimePicker();
            SuspendLayout();
            // 
            // button_add
            // 
            button_add.BackColor = SystemColors.ActiveCaption;
            button_add.Location = new Point(168, 424);
            button_add.Name = "button_add";
            button_add.Size = new Size(199, 52);
            button_add.TabIndex = 0;
            button_add.Text = "AddBill";
            button_add.UseVisualStyleBackColor = false;
            button_add.Click += button_add_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(93, 89);
            label1.Name = "label1";
            label1.Size = new Size(69, 35);
            label1.TabIndex = 2;
            label1.Text = "帐户";
            // 
            // textBox_account
            // 
            textBox_account.Location = new Point(168, 82);
            textBox_account.Name = "textBox_account";
            textBox_account.Size = new Size(466, 42);
            textBox_account.TabIndex = 3;
            // 
            // textBox_transactionType
            // 
            textBox_transactionType.Location = new Point(168, 145);
            textBox_transactionType.Name = "textBox_transactionType";
            textBox_transactionType.Size = new Size(466, 42);
            textBox_transactionType.TabIndex = 5;
            // 
            // label_productType
            // 
            label_productType.AutoSize = true;
            label_productType.Location = new Point(39, 145);
            label_productType.Name = "label_productType";
            label_productType.Size = new Size(123, 35);
            label_productType.TabIndex = 4;
            label_productType.Text = "产品类型";
            // 
            // textBox_amount
            // 
            textBox_amount.Location = new Point(168, 268);
            textBox_amount.Name = "textBox_amount";
            textBox_amount.Size = new Size(466, 42);
            textBox_amount.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(39, 268);
            label2.Name = "label2";
            label2.Size = new Size(123, 35);
            label2.TabIndex = 8;
            label2.Text = "交易金额";
            // 
            // textBox_resPath
            // 
            textBox_resPath.Location = new Point(168, 205);
            textBox_resPath.Name = "textBox_resPath";
            textBox_resPath.Size = new Size(466, 42);
            textBox_resPath.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(39, 208);
            label3.Name = "label3";
            label3.Size = new Size(123, 35);
            label3.TabIndex = 6;
            label3.Text = "资源路径";
            // 
            // textBox_reciprocalAccount
            // 
            textBox_reciprocalAccount.Location = new Point(168, 328);
            textBox_reciprocalAccount.Name = "textBox_reciprocalAccount";
            textBox_reciprocalAccount.Size = new Size(466, 42);
            textBox_reciprocalAccount.TabIndex = 11;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(39, 328);
            label4.Name = "label4";
            label4.Size = new Size(123, 35);
            label4.TabIndex = 10;
            label4.Text = "对方帐户";
            // 
            // textBox_remark
            // 
            textBox_remark.Location = new Point(168, 376);
            textBox_remark.Name = "textBox_remark";
            textBox_remark.Size = new Size(466, 42);
            textBox_remark.TabIndex = 13;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(93, 376);
            label5.Name = "label5";
            label5.Size = new Size(69, 35);
            label5.TabIndex = 12;
            label5.Text = "备注";
            // 
            // textBox_id
            // 
            textBox_id.Location = new Point(168, 24);
            textBox_id.Name = "textBox_id";
            textBox_id.Size = new Size(466, 42);
            textBox_id.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(118, 24);
            label6.Name = "label6";
            label6.Size = new Size(44, 35);
            label6.TabIndex = 14;
            label6.Text = "ID";
            // 
            // textBox_result
            // 
            textBox_result.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox_result.BackColor = SystemColors.Info;
            textBox_result.Location = new Point(39, 556);
            textBox_result.Multiline = true;
            textBox_result.Name = "textBox_result";
            textBox_result.ReadOnly = true;
            textBox_result.Size = new Size(1045, 218);
            textBox_result.TabIndex = 17;
            // 
            // button_downloadData
            // 
            button_downloadData.Location = new Point(652, 78);
            button_downloadData.Name = "button_downloadData";
            button_downloadData.Size = new Size(199, 52);
            button_downloadData.TabIndex = 18;
            button_downloadData.Text = "下载数据样例";
            button_downloadData.UseVisualStyleBackColor = true;
            button_downloadData.Click += button_auto_Click;
            // 
            // button_initDb
            // 
            button_initDb.BackColor = SystemColors.ActiveCaption;
            button_initDb.Location = new Point(873, 24);
            button_initDb.Name = "button_initDb";
            button_initDb.Size = new Size(199, 52);
            button_initDb.TabIndex = 19;
            button_initDb.Text = "初始化底层库";
            button_initDb.UseVisualStyleBackColor = false;
            button_initDb.Click += button_initDb_Click;
            // 
            // button_buy
            // 
            button_buy.Location = new Point(652, 132);
            button_buy.Name = "button_buy";
            button_buy.Size = new Size(199, 52);
            button_buy.TabIndex = 20;
            button_buy.Text = "购买产品样例";
            button_buy.UseVisualStyleBackColor = true;
            button_buy.Click += button_buy_Click;
            // 
            // button_recharge
            // 
            button_recharge.Location = new Point(652, 24);
            button_recharge.Name = "button_recharge";
            button_recharge.Size = new Size(199, 52);
            button_recharge.TabIndex = 21;
            button_recharge.Text = "充值样例";
            button_recharge.UseVisualStyleBackColor = true;
            button_recharge.Click += button_recharge_Click;
            // 
            // button_withdraw
            // 
            button_withdraw.Location = new Point(652, 190);
            button_withdraw.Name = "button_withdraw";
            button_withdraw.Size = new Size(199, 52);
            button_withdraw.TabIndex = 22;
            button_withdraw.Text = "提现样例";
            button_withdraw.UseVisualStyleBackColor = true;
            button_withdraw.Click += button_withdraw_Click;
            // 
            // button_getBills
            // 
            button_getBills.BackColor = SystemColors.ActiveCaption;
            button_getBills.Location = new Point(706, 498);
            button_getBills.Name = "button_getBills";
            button_getBills.Size = new Size(176, 52);
            button_getBills.TabIndex = 23;
            button_getBills.Text = "获取帐单";
            button_getBills.UseVisualStyleBackColor = false;
            button_getBills.Click += button_getBills_Click;
            // 
            // button_getDownloadCount
            // 
            button_getDownloadCount.BackColor = SystemColors.ActiveCaption;
            button_getDownloadCount.Location = new Point(888, 498);
            button_getDownloadCount.Name = "button_getDownloadCount";
            button_getDownloadCount.Size = new Size(184, 52);
            button_getDownloadCount.TabIndex = 24;
            button_getDownloadCount.Text = "获取下载量";
            button_getDownloadCount.UseVisualStyleBackColor = false;
            button_getDownloadCount.Click += button_getDownloadCount_Click;
            // 
            // dateTimePicker_start
            // 
            dateTimePicker_start.Location = new Point(43, 503);
            dateTimePicker_start.Name = "dateTimePicker_start";
            dateTimePicker_start.Size = new Size(288, 42);
            dateTimePicker_start.TabIndex = 25;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(337, 507);
            label7.Name = "label7";
            label7.Size = new Size(42, 35);
            label7.TabIndex = 26;
            label7.Text = "至";
            // 
            // dateTimePicker_end
            // 
            dateTimePicker_end.Location = new Point(385, 503);
            dateTimePicker_end.Name = "dateTimePicker_end";
            dateTimePicker_end.Size = new Size(288, 42);
            dateTimePicker_end.TabIndex = 27;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(16F, 35F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1115, 786);
            Controls.Add(dateTimePicker_end);
            Controls.Add(label7);
            Controls.Add(dateTimePicker_start);
            Controls.Add(button_getDownloadCount);
            Controls.Add(button_getBills);
            Controls.Add(button_withdraw);
            Controls.Add(button_recharge);
            Controls.Add(button_buy);
            Controls.Add(button_initDb);
            Controls.Add(button_downloadData);
            Controls.Add(textBox_result);
            Controls.Add(textBox_id);
            Controls.Add(label6);
            Controls.Add(textBox_remark);
            Controls.Add(label5);
            Controls.Add(textBox_reciprocalAccount);
            Controls.Add(label4);
            Controls.Add(textBox_amount);
            Controls.Add(label2);
            Controls.Add(textBox_resPath);
            Controls.Add(label3);
            Controls.Add(textBox_transactionType);
            Controls.Add(label_productType);
            Controls.Add(textBox_account);
            Controls.Add(label1);
            Controls.Add(button_add);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_add;
        private Label label1;
        private TextBox textBox_account;
        private TextBox textBox_transactionType;
        private Label label_productType;
        private TextBox textBox_amount;
        private Label label2;
        private TextBox textBox_resPath;
        private Label label3;
        private TextBox textBox_reciprocalAccount;
        private Label label4;
        private TextBox textBox_remark;
        private Label label5;
        private TextBox textBox_id;
        private Label label6;
        private TextBox textBox_result;
        private Button button_downloadData;
        private Button button_initDb;
        private Button button_buy;
        private Button button_recharge;
        private Button button_withdraw;
        private Button button_getBills;
        private Button button_getDownloadCount;
        private DateTimePicker dateTimePicker_start;
        private Label label7;
        private DateTimePicker dateTimePicker_end;
    }
}