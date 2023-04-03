using ClosedXML.Excel;
using DigitalPlatform.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace dp2mini
{
    public partial class RightsForm : Form
    {
        MainForm _mainFrom = null;
        public RightsForm(MainForm mainForm)
        {
            this._mainFrom = mainForm;
            
            InitializeComponent();
        }




        private void button_getXmlFile_Click(object sender, EventArgs e)
        {
            //询问文件名
            SaveFileDialog dlg = new SaveFileDialog
            {
                Title = "请指定读者报表文件名",
                CreatePrompt = false,
                OverwritePrompt = true,
                FileName = this.textBox_xml.Text.Trim(),

                //InitialDirectory = Environment.CurrentDirectory,
                Filter = "xml文档 (*.xml)|*.xml|All files (*.*)|*.*",

                RestoreDirectory = true
            };

            // 如果在询问文件名对话框，点了取消，退不处理，返回0，
            if (dlg.ShowDialog() == DialogResult.OK)
                this.textBox_xml.Text = dlg.FileName;
            else
                this.textBox_xml.Text = "";
        }

        private void button_getExcelFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "请指定权限总表Excel文件";
                dlg.FileName = this.textBox_excel.Text;
                dlg.Filter = "All files (*.*)|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.textBox_excel.Text = dlg.FileName;
                }
                else
                {
                    this.textBox_excel.Text = "";
                }
            }
        }

        public const string C_Type_xml = "xml";
        public const string C_Type_markdown = "markdown";
        public void output(string type)
        {
            this.textBox_result.Text = "";

            string excelfile = this.textBox_excel.Text.Trim();
            if (string.IsNullOrEmpty(excelfile) == true)
            {
                MessageBox.Show(this, "尚未提交权限总表Excel文件!");
                return;
            }
            XLWorkbook workbook = null;
            try
            {
                workbook = new XLWorkbook(excelfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "打开excel异常:" + ex.Message);
                return;
            }
            IXLWorksheet sheet = workbook.Worksheets.FirstOrDefault();

            var rows = sheet.Rows();
            int nCount = rows.Count();

            string output = "";
            if (type== C_Type_xml)
                output = "<root>";

            int rowIndex = 0;
            foreach (var row in rows)
            {
                Application.DoEvents();

                // 第一行是列标题，过掉
                if (rowIndex++ == 0)
                {
                    continue;
                }

                int cellCount = row.CellCount();

                // 有状态的值不出现
                string state = "";
                if (cellCount >= 5)
                {
                     state = row.Cell(5).GetString().Trim();
                    if (string.IsNullOrEmpty(state) == false && type == C_Type_xml)
                        continue;
                }

                string name = row.Cell(1).GetString().Trim();

                // 表示是一个空行
                if (string.IsNullOrEmpty(name) == true)
                    continue;

                // 第2列为空表示是一个分类行
                if (row.Cell(2).GetString() == "")
                {
                    if (type == "xml")
                        output += "<!--" + name + "-->";
                    else
                        output += "\r\n"
                            +"# "+name +"\r\n";

                    continue;
                }

                string zh = row.Cell(2).GetString().Trim();
                string en = row.Cell(3).GetString().Trim();
                string level = row.Cell(4).GetString().Trim();

                string desc= row.Cell(6).GetString().Trim();
                string remark= row.Cell(7).GetString().Trim();
                /*
                <root>
                <property name='borrow' risklevel>
                <comment lang='zh'>借书</comment>
                <comment lang='en'>borrow</comment>
                </property>
                */
                if (type == C_Type_xml)
                {
                    output += @"<property name='" + name + "' risklevel='" + level + "'>"
                            + "<comment lang='zh'>" + zh + "</comment>"
                            + "<comment lang='en'>" + en + "</comment>"
                            + "</property>";
                }
                else
                {
                    output += @"## " + name + "\r\n"
                        + "### 中文说明" + "\r\n"
                        + zh + "\r\n"
                        + "### 英文说明" + "\r\n"
                        + en + "\r\n"
                        + "### 危险等级" + "\r\n"
                        + level + "\r\n"
                        + "### 描述信息" + "\r\n"
                        + desc + "\r\n"
                        + "### 应用地方" + "\r\n"
                        + remark + "\r\n";

                    if (string.IsNullOrEmpty(state) == false)
                    {
                        output += "### 状态" + "\r\n"
                            + state + "\r\n";
                    }
                }

                //rowIndex++;
            }
            if (type == C_Type_xml)
            {
                output += "</root>";
                output = DomUtil.GetIndentXml(output);
            }

            // 显示结果
            this.textBox_result.Text = output;


            //userrightsdef.xml
            string xmlFile = this.textBox_xml.Text.Trim();
            if (string.IsNullOrEmpty(xmlFile) == false)
            {
                try
                {
                    XmlDocument dom = new XmlDocument();
                    dom.LoadXml(output);
                    dom.Save(xmlFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            MessageBox.Show(this, "生成完成");
            return;
        }

        private void button_createXml_Click(object sender, EventArgs e)
        {
            this.output(C_Type_xml);
        }

        private void button_createMarkDown_Click(object sender, EventArgs e)
        {
            this.output(C_Type_markdown);
        }
    }
}
