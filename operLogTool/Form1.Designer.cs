namespace opeLogTool
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            textBox_xml = new TextBox();
            button_wirte = new Button();
            label1 = new Label();
            textBox_logFile = new TextBox();
            textBox_value1 = new TextBox();
            label3 = new Label();
            textBox_value2 = new TextBox();
            textBox_name1 = new TextBox();
            textBox_name2 = new TextBox();
            textBox_path1 = new TextBox();
            textBox_path2 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            splitContainer1 = new SplitContainer();
            listView1 = new ListView();
            columnHeader_file = new ColumnHeader();
            columnHeader_no = new ColumnHeader();
            columnHeader_ope = new ColumnHeader();
            columnHeader_opeTime = new ColumnHeader();
            columnHeader1 = new ColumnHeader();
            contextMenuStrip1 = new ContextMenuStrip(components);
            删除ToolStripMenuItem = new ToolStripMenuItem();
            导出附件ToolStripMenuItem = new ToolStripMenuItem();
            拷贝xml到剪切板ToolStripMenuItem = new ToolStripMenuItem();
            复制到新文件ToolStripMenuItem = new ToolStripMenuItem();
            button_new = new Button();
            button_att = new Button();
            textBox_att = new TextBox();
            label5 = new Label();
            textBox_logXml = new TextBox();
            button_save = new Button();
            button_del = new Button();
            button_append = new Button();
            button_format = new Button();
            button_insert = new Button();
            label4 = new Label();
            textBox_dir = new TextBox();
            button_load = new Button();
            textBox_files = new TextBox();
            label2 = new Label();
            tabPage2 = new TabPage();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // textBox_xml
            // 
            textBox_xml.Location = new Point(18, 91);
            textBox_xml.Multiline = true;
            textBox_xml.Name = "textBox_xml";
            textBox_xml.ScrollBars = ScrollBars.Vertical;
            textBox_xml.Size = new Size(1129, 341);
            textBox_xml.TabIndex = 0;
            // 
            // button_wirte
            // 
            button_wirte.Location = new Point(984, 752);
            button_wirte.Name = "button_wirte";
            button_wirte.Size = new Size(163, 45);
            button_wirte.TabIndex = 1;
            button_wirte.Text = "写日志";
            button_wirte.UseVisualStyleBackColor = true;
            button_wirte.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 758);
            label1.Name = "label1";
            label1.Size = new Size(123, 35);
            label1.TabIndex = 2;
            label1.Text = "文件名：";
            // 
            // textBox_logFile
            // 
            textBox_logFile.Location = new Point(142, 758);
            textBox_logFile.Name = "textBox_logFile";
            textBox_logFile.Size = new Size(836, 42);
            textBox_logFile.TabIndex = 3;
            // 
            // textBox_value1
            // 
            textBox_value1.Location = new Point(278, 459);
            textBox_value1.Multiline = true;
            textBox_value1.Name = "textBox_value1";
            textBox_value1.ScrollBars = ScrollBars.Vertical;
            textBox_value1.Size = new Size(869, 124);
            textBox_value1.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 604);
            label3.Name = "label3";
            label3.Size = new Size(0, 35);
            label3.TabIndex = 8;
            // 
            // textBox_value2
            // 
            textBox_value2.Location = new Point(278, 617);
            textBox_value2.Multiline = true;
            textBox_value2.Name = "textBox_value2";
            textBox_value2.ScrollBars = ScrollBars.Vertical;
            textBox_value2.Size = new Size(869, 119);
            textBox_value2.TabIndex = 7;
            // 
            // textBox_name1
            // 
            textBox_name1.Location = new Point(19, 465);
            textBox_name1.Name = "textBox_name1";
            textBox_name1.Size = new Size(253, 42);
            textBox_name1.TabIndex = 9;
            // 
            // textBox_name2
            // 
            textBox_name2.Location = new Point(19, 620);
            textBox_name2.Name = "textBox_name2";
            textBox_name2.Size = new Size(253, 42);
            textBox_name2.TabIndex = 10;
            // 
            // textBox_path1
            // 
            textBox_path1.BackColor = SystemColors.Info;
            textBox_path1.Location = new Point(20, 509);
            textBox_path1.Multiline = true;
            textBox_path1.Name = "textBox_path1";
            textBox_path1.Size = new Size(252, 74);
            textBox_path1.TabIndex = 11;
            // 
            // textBox_path2
            // 
            textBox_path2.BackColor = SystemColors.Info;
            textBox_path2.Location = new Point(18, 662);
            textBox_path2.Multiline = true;
            textBox_path2.Name = "textBox_path2";
            textBox_path2.Size = new Size(252, 74);
            textBox_path2.TabIndex = 12;
            // 
            // button1
            // 
            button1.Location = new Point(20, 15);
            button1.Name = "button1";
            button1.Size = new Size(54, 47);
            button1.TabIndex = 13;
            button1.Text = "1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // button2
            // 
            button2.Location = new Point(80, 15);
            button2.Name = "button2";
            button2.Size = new Size(54, 47);
            button2.TabIndex = 14;
            button2.Text = "2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(167, 15);
            button3.Name = "button3";
            button3.Size = new Size(54, 47);
            button3.TabIndex = 16;
            button3.Text = "3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(227, 15);
            button4.Name = "button4";
            button4.Size = new Size(54, 47);
            button4.TabIndex = 15;
            button4.Text = "4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(314, 15);
            button5.Name = "button5";
            button5.Size = new Size(54, 47);
            button5.TabIndex = 18;
            button5.Text = "5";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(374, 15);
            button6.Name = "button6";
            button6.Size = new Size(54, 47);
            button6.TabIndex = 17;
            button6.Text = "6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(463, 15);
            button7.Name = "button7";
            button7.Size = new Size(54, 47);
            button7.TabIndex = 20;
            button7.Text = "7";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(523, 15);
            button8.Name = "button8";
            button8.Size = new Size(54, 47);
            button8.TabIndex = 19;
            button8.Text = "8";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new Point(583, 15);
            button9.Name = "button9";
            button9.Size = new Size(54, 47);
            button9.TabIndex = 21;
            button9.Text = "9";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Location = new Point(676, 15);
            button10.Name = "button10";
            button10.Size = new Size(66, 47);
            button10.TabIndex = 24;
            button10.Text = "10";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button11
            // 
            button11.Location = new Point(748, 15);
            button11.Name = "button11";
            button11.Size = new Size(70, 47);
            button11.TabIndex = 23;
            button11.Text = "11";
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click;
            // 
            // button12
            // 
            button12.Location = new Point(824, 15);
            button12.Name = "button12";
            button12.Size = new Size(67, 47);
            button12.TabIndex = 22;
            button12.Text = "12";
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 12);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1486, 895);
            tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(splitContainer1);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(textBox_dir);
            tabPage1.Controls.Add(button_load);
            tabPage1.Controls.Add(textBox_files);
            tabPage1.Controls.Add(label2);
            tabPage1.Location = new Point(10, 53);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1466, 832);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(7, 64);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = SystemColors.Window;
            splitContainer1.Panel1.Controls.Add(listView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = SystemColors.Window;
            splitContainer1.Panel2.Controls.Add(button_new);
            splitContainer1.Panel2.Controls.Add(button_att);
            splitContainer1.Panel2.Controls.Add(textBox_att);
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(textBox_logXml);
            splitContainer1.Panel2.Controls.Add(button_save);
            splitContainer1.Panel2.Controls.Add(button_del);
            splitContainer1.Panel2.Controls.Add(button_append);
            splitContainer1.Panel2.Controls.Add(button_format);
            splitContainer1.Panel2.Controls.Add(button_insert);
            splitContainer1.Size = new Size(1453, 752);
            splitContainer1.SplitterDistance = 592;
            splitContainer1.TabIndex = 12;
            // 
            // listView1
            // 
            listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listView1.BackColor = SystemColors.Window;
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader_file, columnHeader_no, columnHeader_ope, columnHeader_opeTime, columnHeader1 });
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Location = new Point(9, 14);
            listView1.Name = "listView1";
            listView1.Size = new Size(571, 725);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // columnHeader_file
            // 
            columnHeader_file.Text = "文件名";
            columnHeader_file.Width = 200;
            // 
            // columnHeader_no
            // 
            columnHeader_no.Text = "序号";
            columnHeader_no.Width = 70;
            // 
            // columnHeader_ope
            // 
            columnHeader_ope.Text = "操作";
            columnHeader_ope.Width = 250;
            // 
            // columnHeader_opeTime
            // 
            columnHeader_opeTime.Text = "操作时间";
            columnHeader_opeTime.Width = 300;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "附件";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(36, 36);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 删除ToolStripMenuItem, 导出附件ToolStripMenuItem, 拷贝xml到剪切板ToolStripMenuItem, 复制到新文件ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(299, 172);
            // 
            // 删除ToolStripMenuItem
            // 
            删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            删除ToolStripMenuItem.Size = new Size(298, 42);
            删除ToolStripMenuItem.Text = "删除";
            删除ToolStripMenuItem.Click += 删除ToolStripMenuItem_Click;
            // 
            // 导出附件ToolStripMenuItem
            // 
            导出附件ToolStripMenuItem.Name = "导出附件ToolStripMenuItem";
            导出附件ToolStripMenuItem.Size = new Size(298, 42);
            导出附件ToolStripMenuItem.Text = "导出附件";
            导出附件ToolStripMenuItem.Click += 导出附件ToolStripMenuItem_Click;
            // 
            // 拷贝xml到剪切板ToolStripMenuItem
            // 
            拷贝xml到剪切板ToolStripMenuItem.Name = "拷贝xml到剪切板ToolStripMenuItem";
            拷贝xml到剪切板ToolStripMenuItem.Size = new Size(298, 42);
            拷贝xml到剪切板ToolStripMenuItem.Text = "拷贝xml到剪切板";
            拷贝xml到剪切板ToolStripMenuItem.Click += 拷贝xml到剪切板ToolStripMenuItem_Click;
            // 
            // 复制到新文件ToolStripMenuItem
            // 
            复制到新文件ToolStripMenuItem.Name = "复制到新文件ToolStripMenuItem";
            复制到新文件ToolStripMenuItem.Size = new Size(298, 42);
            复制到新文件ToolStripMenuItem.Text = "复制到新文件";
            复制到新文件ToolStripMenuItem.Click += 复制到新文件ToolStripMenuItem_Click;
            // 
            // button_new
            // 
            button_new.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_new.Location = new Point(550, 691);
            button_new.Name = "button_new";
            button_new.Size = new Size(144, 48);
            button_new.TabIndex = 13;
            button_new.Text = "new";
            button_new.UseVisualStyleBackColor = true;
            button_new.Click += button_new_Click;
            // 
            // button_att
            // 
            button_att.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_att.Location = new Point(633, 620);
            button_att.Name = "button_att";
            button_att.Size = new Size(84, 52);
            button_att.TabIndex = 12;
            button_att.Text = "...";
            button_att.UseVisualStyleBackColor = true;
            button_att.Click += button_att_Click;
            // 
            // textBox_att
            // 
            textBox_att.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            textBox_att.Location = new Point(93, 625);
            textBox_att.Name = "textBox_att";
            textBox_att.Size = new Size(534, 42);
            textBox_att.TabIndex = 11;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(17, 629);
            label5.Name = "label5";
            label5.Size = new Size(69, 35);
            label5.TabIndex = 10;
            label5.Text = "附件";
            // 
            // textBox_logXml
            // 
            textBox_logXml.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox_logXml.BackColor = SystemColors.ControlLightLight;
            textBox_logXml.Location = new Point(14, 17);
            textBox_logXml.Multiline = true;
            textBox_logXml.Name = "textBox_logXml";
            textBox_logXml.ScrollBars = ScrollBars.Vertical;
            textBox_logXml.Size = new Size(831, 580);
            textBox_logXml.TabIndex = 0;
            // 
            // button_save
            // 
            button_save.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_save.Location = new Point(14, 691);
            button_save.Name = "button_save";
            button_save.Size = new Size(133, 48);
            button_save.TabIndex = 5;
            button_save.Text = "replace";
            button_save.UseVisualStyleBackColor = true;
            button_save.Click += button_save_Click;
            // 
            // button_del
            // 
            button_del.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_del.Location = new Point(424, 691);
            button_del.Name = "button_del";
            button_del.Size = new Size(120, 48);
            button_del.TabIndex = 6;
            button_del.Text = "delete";
            button_del.UseVisualStyleBackColor = true;
            button_del.Click += button_del_Click;
            // 
            // button_append
            // 
            button_append.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_append.Location = new Point(274, 691);
            button_append.Name = "button_append";
            button_append.Size = new Size(144, 48);
            button_append.TabIndex = 8;
            button_append.Text = "append";
            button_append.UseVisualStyleBackColor = true;
            button_append.Click += button_append_Click;
            // 
            // button_format
            // 
            button_format.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button_format.Location = new Point(727, 691);
            button_format.Name = "button_format";
            button_format.Size = new Size(108, 48);
            button_format.TabIndex = 9;
            button_format.Text = "格式化";
            button_format.UseVisualStyleBackColor = true;
            button_format.Click += button_format_Click;
            // 
            // button_insert
            // 
            button_insert.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button_insert.Location = new Point(153, 691);
            button_insert.Name = "button_insert";
            button_insert.Size = new Size(115, 48);
            button_insert.TabIndex = 7;
            button_insert.Text = "insert";
            button_insert.UseVisualStyleBackColor = true;
            button_insert.Click += button_insert_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 10);
            label4.Name = "label4";
            label4.Size = new Size(69, 35);
            label4.TabIndex = 11;
            label4.Text = "目录";
            // 
            // textBox_dir
            // 
            textBox_dir.Location = new Point(80, 6);
            textBox_dir.Name = "textBox_dir";
            textBox_dir.Size = new Size(494, 42);
            textBox_dir.TabIndex = 10;
            textBox_dir.Text = "C:\\1";
            textBox_dir.KeyDown += textBox_dir_KeyDown;
            // 
            // button_load
            // 
            button_load.Location = new Point(1196, 3);
            button_load.Name = "button_load";
            button_load.Size = new Size(94, 48);
            button_load.TabIndex = 4;
            button_load.Text = "装载";
            button_load.UseVisualStyleBackColor = true;
            button_load.Click += button_load_Click;
            // 
            // textBox_files
            // 
            textBox_files.Location = new Point(656, 6);
            textBox_files.Name = "textBox_files";
            textBox_files.Size = new Size(534, 42);
            textBox_files.TabIndex = 3;
            textBox_files.KeyDown += textBox_files_KeyDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(580, 10);
            label2.Name = "label2";
            label2.Size = new Size(69, 35);
            label2.TabIndex = 2;
            label2.Text = "文件";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(button10);
            tabPage2.Controls.Add(textBox_value2);
            tabPage2.Controls.Add(button5);
            tabPage2.Controls.Add(textBox_xml);
            tabPage2.Controls.Add(button8);
            tabPage2.Controls.Add(textBox_name1);
            tabPage2.Controls.Add(button11);
            tabPage2.Controls.Add(textBox_value1);
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(button6);
            tabPage2.Controls.Add(button_wirte);
            tabPage2.Controls.Add(button7);
            tabPage2.Controls.Add(textBox_path2);
            tabPage2.Controls.Add(textBox_name2);
            tabPage2.Controls.Add(button12);
            tabPage2.Controls.Add(textBox_logFile);
            tabPage2.Controls.Add(button4);
            tabPage2.Controls.Add(button3);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(button9);
            tabPage2.Controls.Add(textBox_path1);
            tabPage2.Location = new Point(10, 53);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1466, 832);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(216F, 216F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1510, 936);
            Controls.Add(tabControl1);
            Controls.Add(label3);
            Name = "Form1";
            Text = "operLogTool";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBox_xml;
        private System.Windows.Forms.Button button_wirte;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_logFile;
        private System.Windows.Forms.TextBox textBox_value1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_value2;
        private System.Windows.Forms.TextBox textBox_name1;
        private System.Windows.Forms.TextBox textBox_name2;
        private System.Windows.Forms.TextBox textBox_path1;
        private System.Windows.Forms.TextBox textBox_path2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader_file;
        private System.Windows.Forms.ColumnHeader columnHeader_no;
        private System.Windows.Forms.TextBox textBox_logXml;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.TextBox textBox_files;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColumnHeader columnHeader_ope;
        private System.Windows.Forms.ColumnHeader columnHeader_opeTime;
        private System.Windows.Forms.Button button_del;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_append;
        private System.Windows.Forms.Button button_insert;
        private System.Windows.Forms.Button button_format;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_dir;
        private SplitContainer splitContainer1;
        private TextBox textBox_att;
        private Label label5;
        private Button button_att;
        private ColumnHeader columnHeader1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 导出附件ToolStripMenuItem;
        private ToolStripMenuItem 删除ToolStripMenuItem;
        private ToolStripMenuItem 拷贝xml到剪切板ToolStripMenuItem;
        private Button button_new;
        private ToolStripMenuItem 复制到新文件ToolStripMenuItem;
    }
}

