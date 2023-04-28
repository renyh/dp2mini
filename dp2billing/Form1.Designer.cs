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
            button_change = new Button();
            label1 = new Label();
            textBox_account = new TextBox();
            textBox_productType = new TextBox();
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
            button_delete = new Button();
            textBox_result = new TextBox();
            button_auto = new Button();
            SuspendLayout();
            // 
            // button_add
            // 
            button_add.Location = new Point(168, 442);
            button_add.Name = "button_add";
            button_add.Size = new Size(143, 52);
            button_add.TabIndex = 0;
            button_add.Text = "add";
            button_add.UseVisualStyleBackColor = true;
            button_add.Click += button_add_Click;
            // 
            // button_change
            // 
            button_change.Location = new Point(317, 442);
            button_change.Name = "button_change";
            button_change.Size = new Size(143, 52);
            button_change.TabIndex = 1;
            button_change.Text = "change";
            button_change.UseVisualStyleBackColor = true;
            button_change.Click += button_change_Click;
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
            // textBox_productType
            // 
            textBox_productType.Location = new Point(168, 145);
            textBox_productType.Name = "textBox_productType";
            textBox_productType.Size = new Size(466, 42);
            textBox_productType.TabIndex = 5;
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
            // button_delete
            // 
            button_delete.Location = new Point(474, 442);
            button_delete.Name = "button_delete";
            button_delete.Size = new Size(143, 52);
            button_delete.TabIndex = 16;
            button_delete.Text = "delete";
            button_delete.UseVisualStyleBackColor = true;
            button_delete.Click += button_delete_Click;
            // 
            // textBox_result
            // 
            textBox_result.BackColor = SystemColors.Info;
            textBox_result.Location = new Point(39, 516);
            textBox_result.Multiline = true;
            textBox_result.Name = "textBox_result";
            textBox_result.ReadOnly = true;
            textBox_result.Size = new Size(1176, 258);
            textBox_result.TabIndex = 17;
            // 
            // button_auto
            // 
            button_auto.Location = new Point(652, 18);
            button_auto.Name = "button_auto";
            button_auto.Size = new Size(199, 52);
            button_auto.TabIndex = 18;
            button_auto.Text = "自动填入信息";
            button_auto.UseVisualStyleBackColor = true;
            button_auto.Click += button_auto_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(16F, 35F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1246, 786);
            Controls.Add(button_auto);
            Controls.Add(textBox_result);
            Controls.Add(button_delete);
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
            Controls.Add(textBox_productType);
            Controls.Add(label_productType);
            Controls.Add(textBox_account);
            Controls.Add(label1);
            Controls.Add(button_change);
            Controls.Add(button_add);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_add;
        private Button button_change;
        private Label label1;
        private TextBox textBox_account;
        private TextBox textBox_productType;
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
        private Button button_delete;
        private TextBox textBox_result;
        private Button button_auto;
    }
}