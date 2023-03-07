using DigitalPlatform.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace practice
{
    public partial class Form_2 : Form
    {
        Form_main mainForm = null;

        public Form_2(Form_main main)
        {
            InitializeComponent();
        }

        private void Form_2_Load(object sender, EventArgs e)
        {

        }

        private void button_check_Click(object sender, EventArgs e)
        {

            this.textBox_result.Text = "";

            //userrightsdef.xml
            string userrightsdef_file = this.textBox_userrightsdef.Text.Trim();
            string allRights_file= this.textBox_allRights.Text.Trim();

            if (string.IsNullOrEmpty(userrightsdef_file) == true || string.IsNullOrEmpty(allRights_file) == true)
            {
                MessageBox.Show(this, "缺文件");
                return;
            }



            /*
<?xml version='1.0' encoding='utf-8'?>
<root>
	<property name='borrow'>
		<comment lang='zh'>借书</comment>
		<comment lang='en'>borrow</comment>
	</property>
             */
            List<string> defList = new List<string>();
            XmlDocument dom = new XmlDocument();
            
            dom.Load(userrightsdef_file);
            XmlNodeList nodeList = dom.DocumentElement.SelectNodes("property");
            foreach (XmlNode node in nodeList)
            { 
                defList.Add(DomUtil.GetAttr(node, "name"));
            }

            List<string> rightList=new List<string> ();
            using (StreamReader sr = new StreamReader(allRights_file))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    line =line.Trim();

                    if (line=="")
                        continue;

                    string[] r=line.Split('\t');

                    
                    rightList.Add(r[0].Trim());
                }
            }

            // 先检查一下哪些权限没有在userrightsdef.xml中定义
            string nodef = "";
            foreach (string right in rightList)
            {

                if (defList.Contains(right) == false)
                    nodef += right + "\r\n";
            }
            this.textBox_result.Text = "以下权限未在userrightsdef.xml中定义：\r\n"
                + nodef;


            // 再检查一下userrightsdef.xml中那些权限是写错的。
            string errorRight = "";
            foreach (string right in defList)
            {

                if (rightList.Contains(right) == false)
                    errorRight += right + "\r\n";
            }
            this.textBox_result.Text += "\r\n以下在userrightsdef.xml中定义的权限不存在：\r\n"
                + errorRight;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "请指定一个上传的文件";
                dlg.FileName = "";
                dlg.Filter = "All files (*.*)|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                this.textBox_userrightsdef.Text = dlg.FileName;


                // 在主界面做吧
                //// 获取minitype
                //string minitype = PathUtil.MimeTypeFrom(localPath);

                //// 组成metadata xml字符串
                //this.textBox_WriteRes_strMetadata.Text = BuildMetadata(minitype, localPath);

            }
        }

        private void button_getchinese_Click(object sender, EventArgs e)
        {
            string userrightsdef_file = this.textBox_userrightsdef.Text.Trim();
            /*
<?xml version='1.0' encoding='utf-8'?>
<root>
	<property name='borrow'>
		<comment lang='zh'>借书</comment>
		<comment lang='en'>borrow</comment>
	</property>
	<property name='return'>
		<comment lang='zh'>还书</comment>
		<comment lang='en'>return</comment>
	</property>
             */
            List<string> defList = new List<string>();
            XmlDocument dom = new XmlDocument();
            dom.Load(userrightsdef_file);
            XmlNode root = dom.DocumentElement;
            //XmlNodeList nodeList = dom.DocumentElement.SelectNodes("property[@name='']");

            string chinese = "";
            string english = "";

            this.textBox_result.Text = "";
            string rights = this.textBox_rights.Text.Trim();
            rights = rights.Replace("\r\n", "\n");
            string[] rList=rights.Split('\n');
            foreach (string r in rList)
            { 
                XmlNode node = root.SelectSingleNode("property[@name='"+r+"']");
                if (node != null)
                {
                    chinese += DomUtil.GetElementText(node, "comment[@lang='zh']")+"\r\n";
                    english += DomUtil.GetElementText(node, "comment[@lang='en']") + "\r\n";
                }
                else
                {
                    chinese += "未找到\r\n";
                    english += "未找到\r\n";
                }
            }

            this.textBox_result.Text += chinese + "\r\n" + english;
        }

        private void button_checkalias_Click(object sender, EventArgs e)
        {
            this.textBox_result.Text = "";
            string userrightsdef_file = this.textBox_userrightsdef.Text.Trim();
            List<string> defList = new List<string>();
            XmlDocument dom = new XmlDocument();
            dom.Load(userrightsdef_file);
            XmlNode root = dom.DocumentElement;
            
            XmlNodeList nodeList = dom.DocumentElement.SelectNodes("property[@alias]");
            foreach (XmlNode node in nodeList)
            {
                string name = DomUtil.GetAttr(node, "name");
                string alias=DomUtil.GetAttr(node, "alias");

                this.textBox_result.Text+=name + "\t" + alias+"\r\n";

            }

            
        }

        private void button_createUserRights_Click(object sender, EventArgs e)
        {




            this.textBox_result.Text = "";

            //userrightsdef.xml
            string userrightsdef_file = this.textBox_userrightsdef.Text.Trim();
            string allRights_file = this.textBox_allRights.Text.Trim();

            if (string.IsNullOrEmpty(userrightsdef_file) == true || string.IsNullOrEmpty(allRights_file) == true)
            {
                MessageBox.Show(this, "缺文件");
                return;
            }

            string xml = "";


            List<string> rightList = new List<string>();
            using (StreamReader sr = new StreamReader(allRights_file))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    // 注意不要提交删除空白
                    //line = line.Trim();   

                    if (line == "")
                        continue;

                    string[] r = line.Split('\t');

                    /*
<root>
<property name='borrow' risklevel>
<comment lang='zh'>借书</comment>
<comment lang='en'>borrow</comment>
</property>
 */

                    // 作废/不出现的权限不处理
                    /*
writeres	写入资源	write res	2	废弃
writeobject	写入对象资源	write object	2	废弃
                     */
                    if (r.Length >= 5)
                    {
                        string state=r[4].Trim();
                        if (string.IsNullOrEmpty(state) == false)
                            continue;
                    }

                    string name=r[0].Trim();

                    // 表示 是一个空行
                    if (string.IsNullOrEmpty(name) == true)
                        continue;

                    string zh=r[1].Trim();
                    string en=r[2].Trim();
                    string level=r[3].Trim();

                    xml += @"<property name='"+name+"' risklevel='"+level+"'>"
                                + "<comment lang='zh'>"+zh+"</comment>"
                                + "<comment lang='en'>"+en+"</comment>"
                                + "</property>";
                }
            }


            xml = "<root>"+xml+"</root>";


            try
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(xml);
                DomUtil.GetIndentXml(dom);
                dom.Save(userrightsdef_file);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            MessageBox.Show(this, "生成成功");
            return;
        }
    }
}
