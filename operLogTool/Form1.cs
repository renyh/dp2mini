using DigitalPlatform.IO;
using DigitalPlatform.LibraryServer;
using DigitalPlatform.LibraryServer.Common;
using opeLogTool.core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace opeLogTool
{
    public partial class Form1 : Form
    {
        private readonly object newbyte;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string xml = this.textBox_xml.Text.Trim();

            string fileName = this.textBox_logFile.Text.Trim();
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show(this, "请输入日志文件名");
                return;
            }

            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }



            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;

            string name1 = this.textBox_name1.Text.Trim();
            if (string.IsNullOrEmpty(name1) == false)
            {
                XmlNode node = dom.CreateElement(name1);

                node.InnerText = this.textBox_value1.Text;

                string recPath = this.textBox_path1.Text.Trim();
                if (string.IsNullOrEmpty(recPath) == false)
                    DomUtil.SetAttr(node, "recPath", recPath);

                root.AppendChild(node);
            }

            string name2 = this.textBox_name2.Text.Trim();
            if (string.IsNullOrEmpty(name2) == false)
            {
                XmlNode node = dom.CreateElement(name2);
                node.InnerText = this.textBox_value2.Text;

                string recPath = this.textBox_path2.Text.Trim();
                if (string.IsNullOrEmpty(recPath) == false)
                    DomUtil.SetAttr(node, "recPath", recPath);

                root.AppendChild(node);
            }

            // 组合最新的xml
            xml = dom.OuterXml;



            this.WriteLog(xml, fileName);
        }



        public void WriteLog1(string xml, string fileName)
        {
            FileStream s = null;
            try
            {
                s = new FileStream(fileName, FileMode.Create);


                //s = File.Open(
                //    fileName,
                //    FileMode.OpenOrCreate,
                //    FileAccess.ReadWrite, // Read会造成无法打开 2007/5/22
                //    FileShare.ReadWrite);

                OperLogUtility.WriteEnventLog(s, xml, null);
            }
            catch (Exception ex)
            {
                if (s != null)
                {
                    s.Close();
                    //File.Delete(fileName);
                }
                throw ex;
            }
            finally
            {
                if (s != null)
                    s.Close();
            }
        }

        public void WriteLog(string xml, string fileName)
        {
            try
            {
                this.WriteLog1(xml, fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                return;
            }

            MessageBox.Show(this, "ok");
        }

        #region 根元素有refid

        /*
1.根元素下有refid，无readerRecord和ItemRecord或里面的refid为空，预期mongodb记录里是refid
借书日志-20240201.log
还书日志-20240202.log
         */

        private void button1_Click_1(object sender, EventArgs e)
        {
            string xml = @"<root>
  <libraryCode>
  </libraryCode>
  <operation>borrow</operation>
  <action>borrow</action>
  <readerBarcode>P00001</readerBarcode>
  <readerRefID>3c70aeea-e3e9-4d1b-82de-965b3bd7f65d</readerRefID>
  <itemBarcode>DPB000003</itemBarcode>
  <itemRefID>6487e028-9b79-4b8a-8bad-5e44777bcb63</itemRefID>
  <borrowDate>Sun, 18 Feb 2024 11:25:40 +0800</borrowDate>
  <borrowPeriod>31day</borrowPeriod>
  <returningDate>Wed, 20 Mar 2024 12:00:00 +0800</returningDate>
  <type>普通</type>
  <price>CNY16.80</price>
  <no>0</no>
  <operator>supervisor</operator>
  <operTime>Sun, 18 Feb 2024 11:25:40 +0800</operTime>
  <borrowID>78f9df8a-6019-4452-be44-d2c7741e6c78</borrowID>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
  <time start=""2024-02-18T11:25:39"" end=""2024-02-18T11:25:40"" seconds=""0.070"" />
  <uid>a181295f-c6ea-45e3-be6f-6a5db1e0c6d8</uid>


<readerRecord/>
<itemRecord/>

</root>";

            this.WriteLog(xml, @"c:\1\20240201.log");


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <libraryCode>
  </libraryCode>
  <operation>return</operation>
  <action>return</action>
  <borrowDate>Sun, 18 Feb 2024 14:07:03 +0800</borrowDate>
  <borrowPeriod>31day</borrowPeriod>
  <returningDate>Wed, 20 Mar 2024 12:00:00 +0800</returningDate>
  <borrowOperator>supervisor</borrowOperator>
  <borrowID>23c1402c-2481-427c-b23d-fe322f359667</borrowID>

  <itemBarcode>DPB000003</itemBarcode>
  <itemRefID>6487e028-9b79-4b8a-8bad-5e44777bcb63</itemRefID>

  <readerBarcode>P00001</readerBarcode>
  <readerRefID>3c70aeea-e3e9-4d1b-82de-965b3bd7f65d</readerRefID>

  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 09:17:04 +0800</operTime>

  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
  <time start=""2024-02-19T09:17:04"" end=""2024-02-19T09:17:04"" seconds=""0.072"" />
  <uid>079d77c2-a1b4-41a5-bd85-d459c59efcf9</uid>

<readerRecord/>
<itemRecord/>

</root>";

            this.WriteLog(xml, @"c:\1\20240202.log");

        }
        #endregion


        #region

        /*
2.根元素下无refid，但readerRecord和itemRecord里有refid，预期mongodb记录里是refid。

借书日志-20240203.log
还书日志-20240204.log
         */

        private void button3_Click(object sender, EventArgs e)
        {

            string xml = @"<root>
  <libraryCode>
  </libraryCode>
  <operation>borrow</operation>
  <action>borrow</action>

  <readerBarcode>P00001</readerBarcode>
  <itemBarcode>DPB000003</itemBarcode>

  <borrowDate>Sun, 18 Feb 2024 11:25:40 +0800</borrowDate>
  <borrowPeriod>31day</borrowPeriod>
  <returningDate>Wed, 20 Mar 2024 12:00:00 +0800</returningDate>
  <type>普通</type>
  <price>CNY16.80</price>
  <no>0</no>
  <operator>supervisor</operator>
  <operTime>Sun, 18 Feb 2024 11:25:40 +0800</operTime>
  <borrowID>78f9df8a-6019-4452-be44-d2c7741e6c78</borrowID>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
  <time start=""2024-02-18T11:25:39"" end=""2024-02-18T11:25:40"" seconds=""0.070"" />
  <uid>a181295f-c6ea-45e3-be6f-6a5db1e0c6d8</uid>


</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 读者元素
            XmlNode node = dom.CreateElement("readerRecord");
            node.InnerText = @"<patron>
  <barcode>P00001</barcode>  
  <refID>3c70aeea-e3e9-4d1b-82de-965b3bd7f65d</refID> 
</patron>"; ;
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);


            // 册元素
            node = dom.CreateElement("itemRecord");
            node.InnerText = @"<root>
<refID>6487e028-9b79-4b8a-8bad-5e44777bcb63</refID> 
  <barcode>DPB000003</barcode>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);



            this.WriteLog(dom.OuterXml, @"c:\1\20240203.log");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <libraryCode>
  </libraryCode>
  <operation>return</operation>
  <action>return</action>
  <borrowDate>Sun, 18 Feb 2024 14:07:03 +0800</borrowDate>
  <borrowPeriod>31day</borrowPeriod>
  <returningDate>Wed, 20 Mar 2024 12:00:00 +0800</returningDate>
  <borrowOperator>supervisor</borrowOperator>
  <borrowID>23c1402c-2481-427c-b23d-fe322f359667</borrowID>

  <itemBarcode>DPB000003</itemBarcode>
  <readerBarcode>P00001</readerBarcode>

  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 09:17:04 +0800</operTime>

  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
  <time start=""2024-02-19T09:17:04"" end=""2024-02-19T09:17:04"" seconds=""0.072"" />
  <uid>079d77c2-a1b4-41a5-bd85-d459c59efcf9</uid>

</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 读者元素
            XmlNode node = dom.CreateElement("readerRecord");
            node.InnerText = @"<patron>
  <barcode>P00001</barcode>  
  <refID>3c70aeea-e3e9-4d1b-82de-965b3bd7f65d</refID> 
</patron>"; ;
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);


            // 册元素
            node = dom.CreateElement("itemRecord");
            node.InnerText = @"<root>
<refID>6487e028-9b79-4b8a-8bad-5e44777bcb63</refID> 
  <barcode>DPB000003</barcode>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);



            this.WriteLog(dom.OuterXml, @"c:\1\20240204.log");
        }

        #endregion

        #region

        private void button5_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <libraryCode>
  </libraryCode>
  <operation>borrow</operation>
  <action>borrow</action>

  <readerBarcode>P00001</readerBarcode>
  <itemBarcode>DPB000003</itemBarcode>

  <borrowDate>Sun, 18 Feb 2024 11:25:40 +0800</borrowDate>
  <borrowPeriod>31day</borrowPeriod>
  <returningDate>Wed, 20 Mar 2024 12:00:00 +0800</returningDate>
  <type>普通</type>
  <price>CNY16.80</price>
  <no>0</no>
  <operator>supervisor</operator>
  <operTime>Sun, 18 Feb 2024 11:25:40 +0800</operTime>
  <borrowID>78f9df8a-6019-4452-be44-d2c7741e6c78</borrowID>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
  <time start=""2024-02-18T11:25:39"" end=""2024-02-18T11:25:40"" seconds=""0.070"" />
  <uid>a181295f-c6ea-45e3-be6f-6a5db1e0c6d8</uid>


</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 读者元素
            XmlNode node = dom.CreateElement("readerRecord");
            node.InnerText = @"<patron>
  <barcode>P00001</barcode>  
</patron>"; ;
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);


            // 册元素
            node = dom.CreateElement("itemRecord");
            node.InnerText = @"<root>
  <barcode>DPB000003</barcode>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);



            this.WriteLog(dom.OuterXml, @"c:\1\20240205.log");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <libraryCode>
  </libraryCode>
  <operation>return</operation>
  <action>return</action>
  <borrowDate>Sun, 18 Feb 2024 14:07:03 +0800</borrowDate>
  <borrowPeriod>31day</borrowPeriod>
  <returningDate>Wed, 20 Mar 2024 12:00:00 +0800</returningDate>
  <borrowOperator>supervisor</borrowOperator>
  <borrowID>23c1402c-2481-427c-b23d-fe322f359667</borrowID>

  <itemBarcode>DPB000003</itemBarcode>
  <readerBarcode>P00001</readerBarcode>

  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 09:17:04 +0800</operTime>

  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
  <time start=""2024-02-19T09:17:04"" end=""2024-02-19T09:17:04"" seconds=""0.072"" />
  <uid>079d77c2-a1b4-41a5-bd85-d459c59efcf9</uid>

</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 读者元素
            XmlNode node = dom.CreateElement("readerRecord");
            node.InnerText = @"<patron>
  <barcode>P00001</barcode>  
</patron>"; ;
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);


            // 册元素
            node = dom.CreateElement("itemRecord");
            node.InnerText = @"<root>
  <barcode>DPB000003</barcode>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);



            this.WriteLog(dom.OuterXml, @"c:\1\20240206.log");

        }

        #endregion

        #region

        /*

1. 修改证条码的日志，预期会自动更新mongodb的证条码。
20240217.log

2. 更新了refid，预期会自动更新mongodb的证条码为refid。
20240218.log

3. 同时更新了证条码与refid，预期会自动更新mongodb的证条码为refid。
20240219.log
         */

        private void button7_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <operation>setReaderInfo</operation>
  <libraryCode>
  </libraryCode>
  <action>change</action>
  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 09:42:55 +0800</operTime>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 最新记录
            XmlNode node = dom.CreateElement("record");
            node.InnerText = @"<patron>
  <barcode>P000019</barcode>  
</patron>";
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);



            // 旧记录
            node = dom.CreateElement("oldRecord");
            node.InnerText = @"<patron>
  <barcode>P00001</barcode>  
</patron>";
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);


            this.WriteLog(dom.OuterXml, @"c:\1\20240207.log");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <operation>setReaderInfo</operation>
  <libraryCode>
  </libraryCode>
  <action>change</action>
  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 09:42:55 +0800</operTime>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 最新记录
            XmlNode node = dom.CreateElement("record");
            node.InnerText = @"<patron>
  <barcode>P000019</barcode> 
  <refID>3c70aeea-e3e9-4d1b-82de-965b3bd7f123</refID> 
</patron>";
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);



            // 旧记录
            node = dom.CreateElement("oldRecord");
            node.InnerText = @"<patron>
  <barcode>P000019</barcode>  
</patron>";
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);


            this.WriteLog(dom.OuterXml, @"c:\1\20240208.log");
        }

        private void button9_Click(object sender, EventArgs e)
        {

            string xml = @"<root>
  <operation>setReaderInfo</operation>
  <libraryCode>
  </libraryCode>
  <action>change</action>
  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 09:42:55 +0800</operTime>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 最新记录
            XmlNode node = dom.CreateElement("record");
            node.InnerText = @"<patron>
  <barcode>P000011</barcode> 
  <refID>3c70aeea-e3e9-4d1b-82de-965b3bd7f456</refID> 
</patron>";
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);



            // 旧记录
            node = dom.CreateElement("oldRecord");
            node.InnerText = @"<patron>
  <barcode>P000019</barcode>  
</patron>";
            DomUtil.SetAttr(node, "recPath", "读者/1");
            root.AppendChild(node);


            this.WriteLog(dom.OuterXml, @"c:\1\20240209.log");

        }

        #endregion

        #region 修改册日志

        /*
1. 修改册条码的日志，预期会自动更新mongodb的证条码。
20240210.log
符合预期

2. 更新了refid，预期会自动更新mongodb的证条码为refid。
20240211.log
符合预期

3. 同时更新了册条码与refid，预期会自动更新mongodb的证条码为refid。
20240212.log
符合预期
         */

        private void button10_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <operation>setEntity</operation>
  <libraryCode>,</libraryCode>
  <action>change</action>
  <style>outofrangeAsError</style>
  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 11:52:48 +0800</operTime>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 最新记录
            XmlNode node = dom.CreateElement("record");
            node.InnerText = @"<root>
<barcode>DPB0000039</barcode>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);



            // 旧记录
            node = dom.CreateElement("oldRecord");
            node.InnerText = @"<root>
<barcode>DPB000003</barcode>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);


            this.WriteLog(dom.OuterXml, @"c:\1\20240210.log");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <operation>setEntity</operation>
  <libraryCode>,</libraryCode>
  <action>change</action>
  <style>outofrangeAsError</style>
  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 11:52:48 +0800</operTime>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 最新记录
            XmlNode node = dom.CreateElement("record");
            node.InnerText = @"<root>
<barcode>DPB0000039</barcode>
<refID>6487e028-9b79-4b8a-8bad-5e44777bc123i</refID>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);



            // 旧记录
            node = dom.CreateElement("oldRecord");
            node.InnerText = @"<root>
<barcode>DPB0000039</barcode>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);


            this.WriteLog(dom.OuterXml, @"c:\1\20240211.log");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string xml = @"<root>
  <operation>setEntity</operation>
  <libraryCode>,</libraryCode>
  <action>change</action>
  <style>outofrangeAsError</style>
  <operator>supervisor</operator>
  <operTime>Mon, 19 Feb 2024 11:52:48 +0800</operTime>
  <clientAddress via=""net.pipe://localhost/dp2library/xe"">localhost</clientAddress>
  <version>1.10</version>
</root>";

            // 把xml装载到dom
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;


            // 最新记录
            XmlNode node = dom.CreateElement("record");
            node.InnerText = @"<root>
<barcode>DPB0000031</barcode>
<refID>6487e028-9b79-4b8a-8bad-5e44777bc456i</refID>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);



            // 旧记录
            node = dom.CreateElement("oldRecord");
            node.InnerText = @"<root>
<barcode>DPB0000039</barcode>
</root>";
            DomUtil.SetAttr(node, "recPath", "中文图书实体/12");
            root.AppendChild(node);


            this.WriteLog(dom.OuterXml, @"c:\1\20240212.log");
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.splitContainer1.SplitterDistance = 150;
        }

        public string GetDir()
        {
            string dir = this.textBox_dir.Text.Trim();
            if (string.IsNullOrEmpty(dir) == true)
            {
                MessageBox.Show(this, "请输入目录");
                return "";
            }
            return dir;
        }

        public string GetFiles()
        {
            string dir = this.GetDir();
            if (string.IsNullOrEmpty(dir) == true)
                return "";

            string files = this.textBox_files.Text.Trim();
            if (string.IsNullOrEmpty(files) == true)
            {
                //获取目录下的所有文件
                DirectoryInfo d = new DirectoryInfo(dir);
                FileInfo[] fs = d.GetFiles("*.log");
                if (fs != null && fs.Length > 0)
                {
                    foreach (FileInfo f in fs)
                    {
                        if (string.IsNullOrEmpty(files) == false)
                            files += ",";
                        files += f.Name;
                    }
                    //files = fs[0].Name;
                }
            }

            if (string.IsNullOrEmpty(files) == true)
            {
                MessageBox.Show(this, "未输入文件名，且目录中没有文件。");
                return "";
            }
            return files;
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            // 清空listview
            this.listView1.Items.Clear();


            string files = this.GetFiles();
            if (files == "")
                return;

            string strDir = this.GetDir();

            string[] fileList = files.Split(new char[] { ',' });
            foreach (string strFileName in fileList)
            {

                string strError = "";
                int nIndex = 0;

                while (true)
                {

                    using (MemoryStream s = new MemoryStream())
                    {
                        // return:
                        //      -1  error
                        //      0   file not found
                        //      1   succeed
                        //      2   超过范围
                        // 然后尝试读出刚写入的日志记录内容，并加以比对验证
                        int nRet = OperLogUtility.GetOperLog(
                            null,
                            strDir,
                            strFileName,
                            nIndex,
                            -1,
                            "",
                            "",
                            null,
                            out long lHintNext,
                            out string xml,
                            s, //null,
                            out strError);
                        if (nRet != 1)
                        {
                            break;
                        }

                        // 取出xml

                        ListViewItem item = new ListViewItem(strFileName, 0);  //文件名

                        item.SubItems.Add(nIndex.ToString());  //序号


                        /*
        <root>
          <libraryCode></libraryCode>
          <operation>borrow</operation>
          <action>borrow</action>
        <operTime>Wed, 21 Feb 2024 09:38:48 +0800</operTime>
                         */
                        XmlDocument dom = new XmlDocument();
                        dom.LoadXml(xml);
                        string operation = DomUtil.GetElementText(dom.DocumentElement, "operation");
                        string action = DomUtil.GetElementText(dom.DocumentElement, "action");
                        if (string.IsNullOrEmpty(action) == false)
                            operation += "/" + action;
                        item.SubItems.Add(operation); // 操作

                        string operTime = DomUtil.GetElementText(dom.DocumentElement, "operTime");
                        item.SubItems.Add(DateTimeUtil.ToLocalTime(operTime, "yyyy-MM-dd HH:mm:ss")); // 日期


                        // 附件
                        if (s != null && s.Length > 0)
                        {
                            item.SubItems.Add(s.Length.ToString());  //序号
                        }

                        // 格式化一下
                        item.Tag = DomUtil.GetIndentXml(xml);

                        this.listView1.Items.Add(item);

                        nIndex++;
                    }

                }//end while

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox_logXml.Text = "";
            if (this.listView1.SelectedItems.Count > 0)
            {


                this.textBox_logXml.Text = this.listView1.SelectedItems[0].Tag.ToString();






            }
        }


        // 将附件装入到内存流中
        public void getAttrStream(MemoryStream ms)
        {
            // 如果指定的附件，优先用附件
            string attFile = this.textBox_att.Text.Trim();
            if (string.IsNullOrEmpty(attFile) == false)
            {
                using (FileStream s = File.Open(attFile, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    byte[] bytes = new byte[s.Length];
                    s.Read(bytes, 0, (int)s.Length);
                    ms.Write(bytes, 0, (int)s.Length);

                    // 指针调到开始
                    ms.Seek(0, SeekOrigin.Begin);
                }
            }
            else
            {

                int nRet = 0;

                string strDir = this.GetDir();
                if (strDir == "")
                    return;

                if (this.listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show(this, "请在左侧列表先选中要保存的记录");
                    return;
                }
                ListViewItem item = this.listView1.SelectedItems[0];
                string strFileName = item.Text;
                long nIndex = Convert.ToInt32(item.SubItems[1].Text);

                string strError = "";

                // 如果左侧的事项上有附件，把附件读出来一起保存
                nRet = OperLogUtility.GetOperLog(null,
                   strDir,
                   strFileName,
                   nIndex,
                   -1,
                   "",
                   "",
                   null,
                   out long lHintNext,
                   out string tempxml,
                   ms, //null,
                   out strError);
                if (nRet != 1)
                {
                    throw new Exception("获取附件出错：" + strError);
                }

                ms.Seek(0, SeekOrigin.Begin); //一定要调到开始位置

            }
        }


        private void button_save_Click(object sender, EventArgs e)
        {
            int nRet = 0;

            string strDir = this.GetDir();
            if (strDir == "")
                return;

            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请在左侧列表先选中要保存的记录");
                return;
            }
            ListViewItem item = this.listView1.SelectedItems[0];
            string strFileName = item.Text;
            long nIndex = Convert.ToInt32(item.SubItems[1].Text);
            string xml = this.textBox_logXml.Text.Trim();

            string strError = "";

            using (MemoryStream ms = new MemoryStream())
            {
                getAttrStream(ms);

                // return:
                //      -1  error
                //      0   file not found
                //      1   succeed
                //      2   超过范围
                nRet = OperLogUtility.ReplaceOperLog(
                                   null,
                                   strDir,
                                   strFileName,
                                  nIndex,
                                   -1,
                                   "",
                                   xml,
                                   ms,  //null
                                   out long tail,
                                   out strError);
                if (nRet == 1)
                {
                    MessageBox.Show(this, "成功");
                }
                else
                {
                    MessageBox.Show(this, "失败：" + strError);
                    return;
                }

                // 重新装载一下
                button_load_Click(null, null);
            }
        }


        private void button_insert_Click(object sender, EventArgs e)
        {
            string strDir = this.GetDir();
            if (strDir == "")
                return;

            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请在左侧列表选择插入位置的记录");
                return;
            }
            ListViewItem item = this.listView1.SelectedItems[0];
            string strFileName = item.Text;
            long lIndex = Convert.ToInt32(item.SubItems[1].Text);

            string xml = this.textBox_logXml.Text.Trim();

            // 附件
            using (MemoryStream ms = new MemoryStream())
            {
                getAttrStream(ms);


                // return:
                //      -1  error
                //      0   file not found
                //      1   succeed
                //      2   超过范围
                int nRet = OperLogUtility.InsertOperLog(
                                null,
                                strDir,
                                 strFileName,
                                 lIndex,
                                -1,
                                xml,
                                ms,
                                out long lHintNext,
                                out string strError);
                if (nRet == 1)
                {
                    MessageBox.Show(this, "成功");
                }
                else
                {
                    MessageBox.Show(this, "失败：" + strError);
                    return;
                }

                // 重新装载一下
                button_load_Click(null, null);
            }

        }

        private void button_append_Click(object sender, EventArgs e)
        {
            string strDir = this.GetDir();
            if (strDir == "")
                return;

            // 优先认左侧选中项的文件名，如果未选中，按textbox输入的文件名，如文件不存在，会自动创建
            string strFileName = "";
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView1.SelectedItems[0];
                strFileName = item.Text;
            }
            if (strFileName == "")
            {
                strFileName = this.textBox_files.Text.Trim();
                string[] fileList = strFileName.Split(new char[] { ',' });
                if (strFileName == "" || fileList.Length != 1)
                {
                    MessageBox.Show(this, "文件名为空 或 格式不合法(中间不能有,号)。");
                    return;
                }
            }

            string xml = this.textBox_logXml.Text.Trim();

            using (MemoryStream ms = new MemoryStream())
            {
                getAttrStream(ms);

                // return:
                //      -1  error
                //      0   file not found
                //      1   succeed
                //      2   超过范围
                int nRet = OperLogUtility.AppendOperLog(
                                    null,
                                    strDir,
                                     strFileName,
                                    xml,
                                    ms,
                                    out string strError);
                if (nRet == 1)
                {
                    MessageBox.Show(this, "成功");
                }
                else
                {
                    MessageBox.Show(this, "失败：" + strError);
                    return;
                }

                // 重新装载一下
                button_load_Click(null, null);
            }

        }


        private void button_del_Click(object sender, EventArgs e)
        {
            string strDir = this.GetDir();
            if (strDir == "")
                return;

            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请在左侧列表先选中要删除的记录");
                return;
            }


            ListViewItem item = this.listView1.SelectedItems[0];
            string strFileName = item.Text;
            long lIndex = Convert.ToInt32(item.SubItems[1].Text);

            // return:
            //      -1  error
            //      0   file not found
            //      1   succeed
            //      2   超过范围
            int nRet = OperLogUtility.DeleteOperLog(
                null,
                strDir,
                 strFileName,
                 lIndex,
                -1,
                out long lHintNext,
                out string strError);
            if (nRet != 1)
            {
                MessageBox.Show(this, "失败：" + strError);
                return;
            }


            //MessageBox.Show(this, "删除完成");

            // 重新装载一下
            button_load_Click(null, null);
        }

        private void button_format_Click(object sender, EventArgs e)
        {
            string xml = this.textBox_logXml.Text.Trim();
            if (string.IsNullOrEmpty(xml) == false)
                this.textBox_logXml.Text = DomUtil.GetIndentXml(xml);
        }

        private void button_att_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "请指定附件";
                dlg.FileName = this.textBox_att.Text;
                dlg.Filter = "All files (*.*)|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.textBox_att.Text = dlg.FileName;
                }
                else
                {
                    this.textBox_att.Text = "";
                }
            }
        }

        private void 导出附件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strDir = this.GetDir();
            if (strDir == "")
                return;

            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请在左侧列表选择插入位置的记录");
                return;
            }
            ListViewItem item = this.listView1.SelectedItems[0];
            string strFileName = item.Text;
            long lIndex = Convert.ToInt32(item.SubItems[1].Text);



            // 询问文件名
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Title = "请指定要创建的附件文件名";
            dlg.CreatePrompt = false;
            dlg.OverwritePrompt = true;
            dlg.FileName = "";
            dlg.Filter = "附件文件 (*.bin)|*.bin|All files (*.*)|*.*";

            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string strOutputFileName = dlg.FileName;
            long length = 0;

            FileStream s = null;
            try
            {
                s = new FileStream(strOutputFileName, FileMode.Create);

                int nRet = OperLogUtility.GetOperLog(null,
                    strDir,
                    strFileName,
                    lIndex,
                    -1,
                    "",
                    "",
                    null,
                    out long lHintNext,
                    out string xml,
                    s, //null,
                    out string strError);
                if (nRet != 1)
                {
                    MessageBox.Show(this, "获取附件出错：" + strError);
                    return;
                }

                // 
                length = s.Length;
            }
            finally
            {
                if (s != null)
                    s.Close();
            }

            if (length == 0)
            {
                File.Delete(strOutputFileName);
                MessageBox.Show(this, "本条日志没有附件");
                return;
            }


            MessageBox.Show(this, "导出成功");

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button_del_Click(null, null);
        }

        private void 拷贝xml到剪切板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strDir = this.GetDir();
            if (strDir == "")
                return;

            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请先选择事项");
                return;
            }

            string xml = "";
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                if (string.IsNullOrEmpty(xml) == false)
                {
                    xml += "\r\n\r\n===\r\n\r\n";
                }

                string strFileName = item.Text;
                long lIndex = Convert.ToInt32(item.SubItems[1].Text);

                xml += item.Tag.ToString();
            }

            if (xml != "")
                Clipboard.SetDataObject(xml, true);


        }

        private void button_new_Click(object sender, EventArgs e)
        {
            string strDir = this.GetDir();
            if (strDir == "")
                return;

            string strFileName = this.textBox_files.Text.Trim();
            string[] fileList = strFileName.Split(new char[] { ',' });
            if (strFileName == "" || fileList.Length != 1)
            {
                MessageBox.Show(this, "文件名为空 或 格式不合法(中间不能有,号)。");
                return;
            }



            string xml = this.textBox_logXml.Text.Trim();
            using (MemoryStream ms = new MemoryStream())
            {
                getAttrStream(ms);

                // 对于new按钮的功能，因为明确是创建到一个新文件中，所以如果这个文件已经存在，则先删除，避免产生追加的情况。
                if (strFileName != "")
                {
                    string f = strDir + "\\" + strFileName;
                    if (File.Exists(f) == true)
                        File.Delete(f);
                }

                // return:
                //      -1  error
                //      0   file not found
                //      1   succeed
                //      2   超过范围
                int nRet = OperLogUtility.AppendOperLog(
                                    null,
                                    strDir,
                                     strFileName,
                                    xml,
                                    ms,
                                    out string strError);
                if (nRet == 1)
                {
                    MessageBox.Show(this, "成功");
                }
                else
                {
                    MessageBox.Show(this, "失败：" + strError);
                    return;
                }


            }

            // 重新装载一下
            button_load_Click(null, null);

        }

        private void textBox_dir_KeyDown(object sender, KeyEventArgs e)
        {
            //if条件检测按下的是不是Enter键
            if (e.KeyCode == Keys.Enter)
            {
                this.button_load_Click(null, null);
            }
        }

        private void textBox_files_KeyDown(object sender, KeyEventArgs e)
        {
            //if条件检测按下的是不是Enter键
            if (e.KeyCode == Keys.Enter)
            {
                this.button_load_Click(null, null);
            }
        }

        private void 复制到新文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strDir = this.GetDir();
            if (strDir == "")
                return;

            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请先选择事项");
                return;
            }

            string newFile = this.textBox_files.Text.Trim();
            string[] fileList = newFile.Split(new char[] { ',' });
            if (newFile == "" || fileList.Length != 1)
            {
                MessageBox.Show(this, "文件名为空 或 格式不合法(中间不能有,号)。");
                return;
            }
            // 对于new按钮的功能，因为明确是创建到一个新文件中，所以如果这个文件已经存在，则先删除，避免产生追加的情况。
            if (newFile != "")
            {
                string f = strDir + "\\" + newFile;
                if (File.Exists(f) == true)
                    File.Delete(f);
            }


            int nRet = 0;
            string strError = "";

            string xml = "";
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                string strFileName = item.Text;
                long lIndex = Convert.ToInt32(item.SubItems[1].Text);
                xml = item.Tag.ToString();

                using (MemoryStream ms = new MemoryStream())
                {
                    // 如果有附件，把附件取出来一起复制
                    string attLength = "";
                    if (item.SubItems.Count > 4)
                        attLength = item.SubItems[4].Text.Trim();
                    if (attLength != "")
                    {
                        // 如果左侧的事项上有附件，把附件读出来一起保存
                        nRet = OperLogUtility.GetOperLog(null,
                           strDir,
                           strFileName,
                           lIndex,
                           -1,
                           "",
                           "",
                           null,
                           out long lHintNext,
                           out string tempxml,
                           ms, //null,
                           out strError);
                        if (nRet != 1)
                        {
                            throw new Exception("获取附件出错：" + strError);
                        }

                        ms.Seek(0, SeekOrigin.Begin); //一定要调到开始位置
                    }




                    // return:
                    //      -1  error
                    //      0   file not found
                    //      1   succeed
                    //      2   超过范围
                    nRet = OperLogUtility.AppendOperLog(
                                       null,
                                       strDir,
                                        newFile,  // 追加到新文件
                                       xml,
                                       ms,
                                       out strError);
                    if (nRet != 1)
                    {
                        MessageBox.Show(this, "失败：" + strError);
                        return;
                    }


                }
            }




            MessageBox.Show(this, "成功");

            this.button_load_Click(null, null);





        }

        private void button_search_Click(object sender, EventArgs e)
        {
            string word = this.textBox_word.Text.Trim();

            foreach (ListViewItem item in this.listView1.Items)
            {
               string temp= item.Tag.ToString();

                if (temp.Contains(word) ==true)
                {
                    item.Selected = true;
                    //MessageBox.Show(this, item.Index.ToString());
                }
            }

            MessageBox.Show(this, "搜索完成");


                
        }
    }
}
