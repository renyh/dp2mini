namespace dp2Mini
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip_main = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem_file = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_prep = new System.Windows.Forms.ToolStripMenuItem();
            this.备书单管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_help = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_setting = new System.Windows.Forms.ToolStripMenuItem();
            this.UToolStripMenuItem_openUserFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_openDataFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_openProgramFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip_main = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_message = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_loginName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip_main = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_prep = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_note = new System.Windows.Forms.ToolStripButton();
            this.menuStrip_main.SuspendLayout();
            this.statusStrip_main.SuspendLayout();
            this.toolStrip_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip_main
            // 
            this.menuStrip_main.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip_main.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_file,
            this.toolStripMenuItem_help});
            this.menuStrip_main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_main.Name = "menuStrip_main";
            this.menuStrip_main.Padding = new System.Windows.Forms.Padding(11, 3, 0, 3);
            this.menuStrip_main.Size = new System.Drawing.Size(1121, 34);
            this.menuStrip_main.TabIndex = 1;
            this.menuStrip_main.Text = "menuStrip_main";
            // 
            // toolStripMenuItem_file
            // 
            this.toolStripMenuItem_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_prep,
            this.备书单管理ToolStripMenuItem});
            this.toolStripMenuItem_file.Name = "toolStripMenuItem_file";
            this.toolStripMenuItem_file.Size = new System.Drawing.Size(84, 30);
            this.toolStripMenuItem_file.Text = "文件(&F)";
            // 
            // toolStripMenuItem_prep
            // 
            this.toolStripMenuItem_prep.Name = "toolStripMenuItem_prep";
            this.toolStripMenuItem_prep.Size = new System.Drawing.Size(240, 34);
            this.toolStripMenuItem_prep.Text = "预约到书查询(&S)";
            this.toolStripMenuItem_prep.Click += new System.EventHandler(this.toolStripMenuItem_prep_Click);
            // 
            // 备书单管理ToolStripMenuItem
            // 
            this.备书单管理ToolStripMenuItem.Name = "备书单管理ToolStripMenuItem";
            this.备书单管理ToolStripMenuItem.Size = new System.Drawing.Size(240, 34);
            this.备书单管理ToolStripMenuItem.Text = "备书单管理(&N)";
            this.备书单管理ToolStripMenuItem.Click += new System.EventHandler(this.备书单管理ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem_help
            // 
            this.toolStripMenuItem_help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_setting,
            this.UToolStripMenuItem_openUserFolder,
            this.ToolStripMenuItem_openDataFolder,
            this.ToolStripMenuItem_openProgramFolder});
            this.toolStripMenuItem_help.Name = "toolStripMenuItem_help";
            this.toolStripMenuItem_help.Size = new System.Drawing.Size(88, 30);
            this.toolStripMenuItem_help.Text = "帮助(&H)";
            // 
            // toolStripMenuItem_setting
            // 
            this.toolStripMenuItem_setting.Name = "toolStripMenuItem_setting";
            this.toolStripMenuItem_setting.Size = new System.Drawing.Size(262, 34);
            this.toolStripMenuItem_setting.Text = "参数设置(&S)";
            this.toolStripMenuItem_setting.Click += new System.EventHandler(this.toolStripMenuItem_setting_Click);
            // 
            // UToolStripMenuItem_openUserFolder
            // 
            this.UToolStripMenuItem_openUserFolder.Name = "UToolStripMenuItem_openUserFolder";
            this.UToolStripMenuItem_openUserFolder.Size = new System.Drawing.Size(262, 34);
            this.UToolStripMenuItem_openUserFolder.Text = "打开用户文件夹(&U)";
            this.UToolStripMenuItem_openUserFolder.Click += new System.EventHandler(this.UToolStripMenuItem_openUserFolder_Click);
            // 
            // ToolStripMenuItem_openDataFolder
            // 
            this.ToolStripMenuItem_openDataFolder.Name = "ToolStripMenuItem_openDataFolder";
            this.ToolStripMenuItem_openDataFolder.Size = new System.Drawing.Size(262, 34);
            this.ToolStripMenuItem_openDataFolder.Text = "打开数据文件夹(&D)";
            this.ToolStripMenuItem_openDataFolder.Click += new System.EventHandler(this.ToolStripMenuItem_openDataFolder_Click);
            // 
            // ToolStripMenuItem_openProgramFolder
            // 
            this.ToolStripMenuItem_openProgramFolder.Name = "ToolStripMenuItem_openProgramFolder";
            this.ToolStripMenuItem_openProgramFolder.Size = new System.Drawing.Size(262, 34);
            this.ToolStripMenuItem_openProgramFolder.Text = "打开程序文件夹(&P)";
            this.ToolStripMenuItem_openProgramFolder.Click += new System.EventHandler(this.ToolStripMenuItem_openProgramFolder_Click);
            // 
            // statusStrip_main
            // 
            this.statusStrip_main.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_message,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel_loginName});
            this.statusStrip_main.Location = new System.Drawing.Point(0, 533);
            this.statusStrip_main.Name = "statusStrip_main";
            this.statusStrip_main.Padding = new System.Windows.Forms.Padding(2, 0, 26, 0);
            this.statusStrip_main.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip_main.Size = new System.Drawing.Size(1121, 35);
            this.statusStrip_main.TabIndex = 3;
            this.statusStrip_main.Text = "statusStrip_main";
            // 
            // toolStripStatusLabel_message
            // 
            this.toolStripStatusLabel_message.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel_message.Name = "toolStripStatusLabel_message";
            this.toolStripStatusLabel_message.Size = new System.Drawing.Size(953, 28);
            this.toolStripStatusLabel_message.Spring = true;
            this.toolStripStatusLabel_message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(140, 28);
            this.toolStripStatusLabel1.Text = "当前登录账户：";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabel_loginName
            // 
            this.toolStripStatusLabel_loginName.Name = "toolStripStatusLabel_loginName";
            this.toolStripStatusLabel_loginName.Size = new System.Drawing.Size(0, 28);
            this.toolStripStatusLabel_loginName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStrip_main
            // 
            this.toolStrip_main.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_prep,
            this.toolStripButton_note});
            this.toolStrip_main.Location = new System.Drawing.Point(0, 34);
            this.toolStrip_main.Name = "toolStrip_main";
            this.toolStrip_main.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStrip_main.Size = new System.Drawing.Size(1121, 33);
            this.toolStrip_main.TabIndex = 5;
            this.toolStrip_main.Text = "toolStrip1";
            // 
            // toolStripButton_prep
            // 
            this.toolStripButton_prep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_prep.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_prep.Image")));
            this.toolStripButton_prep.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_prep.Name = "toolStripButton_prep";
            this.toolStripButton_prep.Size = new System.Drawing.Size(122, 28);
            this.toolStripButton_prep.Text = "预约到书查询";
            this.toolStripButton_prep.Click += new System.EventHandler(this.toolStripButton_prep_Click);
            // 
            // toolStripButton_note
            // 
            this.toolStripButton_note.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_note.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_note.Image")));
            this.toolStripButton_note.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_note.Name = "toolStripButton_note";
            this.toolStripButton_note.Size = new System.Drawing.Size(104, 33);
            this.toolStripButton_note.Text = "备书单管理";
            this.toolStripButton_note.Click += new System.EventHandler(this.toolStripButton_note_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 568);
            this.Controls.Add(this.toolStrip_main);
            this.Controls.Add(this.statusStrip_main);
            this.Controls.Add(this.menuStrip_main);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip_main;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "馆员备书";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip_main.ResumeLayout(false);
            this.menuStrip_main.PerformLayout();
            this.statusStrip_main.ResumeLayout(false);
            this.statusStrip_main.PerformLayout();
            this.toolStrip_main.ResumeLayout(false);
            this.toolStrip_main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip_main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_file;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_prep;
        private System.Windows.Forms.StatusStrip statusStrip_main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_help;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_setting;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_message;
        private System.Windows.Forms.ToolStrip toolStrip_main;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_loginName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButton_prep;
        private System.Windows.Forms.ToolStripMenuItem UToolStripMenuItem_openUserFolder;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_openDataFolder;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_openProgramFolder;
        private System.Windows.Forms.ToolStripMenuItem 备书单管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton_note;
    }
}

