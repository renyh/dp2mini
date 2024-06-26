﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;

using DigitalPlatform.Xml;
using DigitalPlatform.IO;
using DigitalPlatform;
using DigitalPlatform.ChargingAnalysis;
using DigitalPlatform.CirculationClient;
using xml2html;
using System.Deployment.Application;
using Ionic.Zip;
using DocumentFormat.OpenXml.InkML;
using ClosedXML.Excel;
using DigitalPlatform.dp2.Statis;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Policy;

namespace dp2mini
{
    public partial class chargingAnalysisForm : Form
    {
        #region 窗体装载

        // mid父窗口
        MainForm _mainForm = null;

        /// <summary>
        ///  构造函数
        /// </summary>
        public chargingAnalysisForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 窗体装载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrepForm_Load(object sender, EventArgs e)
        {
            this._mainForm = this.MdiParent as MainForm;
            string strError = "";

            // 阅读分析的数据目录
            string chargingAnalysisDataDir = ClientInfo.UserDir + "\\chargingAnalysis\\";
            if (Directory.Exists(chargingAnalysisDataDir) == false)
            {
                Directory.CreateDirectory(chargingAnalysisDataDir);

                // 先找到安装程序的数据目录
                string dataDir = "";
                // 把几个zip文件拷进来
                if (ApplicationDeployment.IsNetworkDeployed == true)
                {
                    // MessageBox.Show(this, "network");
                    dataDir = Application.LocalUserAppDataPath;
                }
                else
                {
                    // MessageBox.Show(this, "no network");
                    dataDir = Environment.CurrentDirectory;
                }

                string strZipFileName = Path.Combine(dataDir, "chargingAnalysis_data.zip");
                try
                {
                    using (ZipFile zip = ZipFile.Read(strZipFileName))
                    {
                        foreach (ZipEntry one in zip)
                        {
                            one.Extract(chargingAnalysisDataDir, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                }
                catch (Exception ex)
                {
                    strError = ExceptionUtil.GetAutoText(ex);
                    //return -1;
                    MessageBox.Show(this, strError);
                    return;
                }
            }






            // 把登录相关参数传到ChargingAnalysisService服务类
            string serverUrl = this._mainForm.GetPurlUrl(this._mainForm.Setting.Url);
            string userName = this._mainForm.Setting.UserName;
            string password = this._mainForm.Setting.Password;
            string parameters = "type=worker,client=dp2mini|" + ClientInfo.ClientVersion;//Program.ClientVersion;
            int nRet = BorrowAnalysisService.Instance.Init(chargingAnalysisDataDir,
                serverUrl, userName, password, parameters,
                out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
                return;
            }

            // 装载评语模板
            this.LoadCommentTemplate();


            // 把主窗口的状态条清一下。
            this._mainForm.SetStatusMessage("");
        }

        #endregion

        #region 报表目录

        // 报表目录
        public string ReportDir
        {
            get
            {
                return this.textBox_outputDir.Text.Trim();
            }
        }

        // 选择目录
        private void button_selectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = this.textBox_outputDir.Text;
            DialogResult result = dlg.ShowDialog();

            // todo记住上次选择的目录

            if (result != DialogResult.OK || string.IsNullOrEmpty(dlg.SelectedPath) == true)
            {
                //MessageBox.Show(this, "取消");
                return;
            }

            string dir = dlg.SelectedPath;
            this.textBox_outputDir.Text = dir;

            // 把评语输入框清空。
            this.textBox_comment.Text = "";
            this._startTime = DateTime.MinValue; //todo清掉最小时间

            // 重新显示文件
            this.ShowFiles();
        }

        // 手工输入目录，按回车
        private void textBox_outputDir_KeyDown(object sender, KeyEventArgs e)
        {
            //if条件检测按下的是不是Enter键
            if (e.KeyCode == Keys.Enter)
            {
                this.ShowFiles();
            }
        }

        // 显示目录中的文件
        private void ShowFiles()
        {
            try
            {
                string dir = this.textBox_outputDir.Text.Trim();
                if (string.IsNullOrEmpty(dir) == true)
                    return;

                this.listView_files.Items.Clear();

                if (Directory.Exists(dir) == false)
                    return;

                string[] fiels = Directory.GetFiles(dir, "*.xml");
                foreach (string xmlFile in fiels)
                {
                    XmlDocument dom = new XmlDocument();
                    dom.Load(xmlFile);
                    XmlNode root = dom.DocumentElement;

                    //证条码号 patron/barcode 
                    string barcode = DomUtil.GetElementText(root, "patron/barcode");

                    //姓名 patron/name 
                    string name = DomUtil.GetElementText(root, "patron/name");

                    // 班级/部门  patron/department
                    string department = DomUtil.GetElementText(root, "patron/department");

                    //借阅量 borrowInfo/@totalBorrowedCount 属性
                    string totalBorrowedCount = DomUtil.GetAttr(root, "borrowInfo", "totalBorrowedCount");

                    // 排名 borrowInfo/@paiming 属性
                    string paiMing = DomUtil.GetAttr(root, "borrowInfo", "paiMing");

                    // 荣誉称号 borrowInfo/@ title 属性
                    string title = DomUtil.GetAttr(root, "borrowInfo", "title");

                    // 馆长评语
                    string comment = DomUtil.GetElementText(root, "comment");

                    string timeInfo = "";
                    XmlNode commentNode = root.SelectSingleNode("comment");
                    if (commentNode != null)
                    {
                        string startTime = DomUtil.GetAttr(commentNode, "startTime");
                        string endTime = DomUtil.GetAttr(commentNode, "endTime");
                        string usedSeconds = DomUtil.GetAttr(commentNode, "usedSeconds");

                        if (string.IsNullOrEmpty(usedSeconds) == false)
                            timeInfo = usedSeconds;//startTime + "/" + endTime + "/" + usedSeconds;
                    }

                    ListViewItem item = new ListViewItem(barcode);
                    item.SubItems.Add(name);
                    item.SubItems.Add(department);
                    item.SubItems.Add(totalBorrowedCount);
                    item.SubItems.Add(paiMing);
                    item.SubItems.Add(title);
                    item.SubItems.Add(comment);
                    item.SubItems.Add(xmlFile);


                    // 如果对应的html存在，则显示，到时点击第一行时，显示对应
                    string htmlFile = dir + "\\" + barcode + ".html";
                    if (File.Exists(htmlFile) == true)
                    {
                        item.SubItems.Add(htmlFile);
                    }
                    else
                    {
                        item.SubItems.Add("");
                    }

                    // 加一个评语时间
                    item.SubItems.Add(timeInfo);  //9

                    // 加到列表集合中
                    this.listView_files.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                return;
            }
        }

        #endregion

        #region 选行和显示html

        // 选择其中一行
        private void listView_files_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_files.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView_files.SelectedItems[0];

                //从馆员评语显示在输入框
                string comment = item.SubItems[6].Text;
                //this.textBox_comment.Text = comment;
                this.SetCommentForEdit(comment);


                // 如果存在html文件，显示出来
                if (item.SubItems.Count >= 9)
                {
                    string htmlFile = item.SubItems[8].Text;
                    if (string.IsNullOrEmpty(htmlFile) == false || File.Exists(htmlFile) == true)
                    {
                        this.showHtml(htmlFile);
                    }
                    else
                    {
                        SetHtmlString(this.webBrowser1, "<div>读者报表尚未转成html格式，请联系管理员。</div>");
                    }
                }
                else
                {
                    SetHtmlString(this.webBrowser1, "<div>读者报表尚未转成html格式，请联系管理员。</div>");
                }

            }
        }

        // 显示html文件
        public void showHtml(string htmlFile)
        {
            string content = "";
            using (StreamReader reader = new StreamReader(htmlFile))//, Encoding.UTF8))
            {
                content = reader.ReadToEnd().Trim();
            }

            SetHtmlString(this.webBrowser1, content);

            //MessageBox.Show(this, htmlFile);

            ////webBrowser1.Navigate(@".\Documentation\index.html");
            ////this.webBrowser1.Url = new Uri(String.Format("file:///{0}/my_html.html", curDir));
            //this.webBrowser1.Navigate(htmlFile);
            ////this.webBrowser1.
            //this.webBrowser1.Document.Encoding = "utf-8";
        }

        // 给浏览器控件设置html
        public static void SetHtmlString(WebBrowser webBrowser,
            string strHtml)
        {
            webBrowser.DocumentText = strHtml;
        }
        #endregion


        #region 评语相关

        // 装载评语模板
        public void LoadCommentTemplate()
        {
            // 先清空评语列表
            this.comboBox_comment.Items.Clear();
            //把评语模板加到下拉列表
            if (BorrowAnalysisService.Instance.CommentTemplates != null
                && BorrowAnalysisService.Instance.CommentTemplates.Count > 0)
            {
                this.comboBox_comment.Text = C_SelectComment;
                this.comboBox_comment.Items.Add(C_SelectComment);
                foreach (string comment in BorrowAnalysisService.Instance.CommentTemplates)
                {
                    this.comboBox_comment.Items.Add(comment);
                }
            }
        }

        // 评语模板列表第一行
        public const string C_SelectComment = "请选择评语模板";

        // 提交评语
        private void button_setComment_Click(object sender, EventArgs e)
        {
            // 设置评语
            string comment = this.textBox_comment.Text.Trim();

            if (this.listView_files.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请先从列表中选择要修改评语的读者记录。");
                return;
            }

            this.SaveComment(comment);


            MessageBox.Show(this, "评语保存成功。");
        }

        // 提交评语内部函数
        private void SaveComment(string comment1)
        {
            if (this.listView_files.SelectedItems.Count == 0)
                return;


            // 结束时间
            DateTime endTime = DateTime.Now;

            // 结束时间-开始时间=用时
            double usedSeconds = (endTime - this._startTime).TotalSeconds;


            // 支持多条一起编辑评语
            foreach (ListViewItem item in this.listView_files.SelectedItems)
            {
                // 因为里面会替换宏变量，所以先将comment设到一个新变量上。
                string thisComment = comment1;



                string barcode = item.Text;

                string xmlFile = this.ReportDir + "\\" + barcode + ".xml";
                string htmlFile = this.ReportDir + "\\" + barcode + ".html";

                XmlDocument dom = new XmlDocument();
                dom.Load(xmlFile);
                XmlNode root = dom.DocumentElement;


                // 评语中可以带着宏变量
                //<comment>{patronName}，您在校期间读了{borrowCount}书。读万卷书，行万里路。</comment>
                string patronName = item.SubItems[1].Text;
                string borrowCount = item.SubItems[3].Text;
                thisComment = thisComment.Replace("{patronName}", patronName);
                thisComment = thisComment.Replace("{borrowCount}", borrowCount);

                // 设到dom
                DomUtil.SetElementText(root, "comment", thisComment);



                string strStartTime = DateTimeUtil.DateTimeToString(this._startTime);
                string strEndTime = DateTimeUtil.DateTimeToString(endTime);
                string strUsedSeconds = usedSeconds.ToString("#0.00");
                string timeInfo = strUsedSeconds;//strStartTime + "/" + strEndTime + "/" + strUsedSeconds;


                XmlNode commentNode = root.SelectSingleNode("comment");
                DomUtil.SetAttr(commentNode, "startTime", strStartTime);
                DomUtil.SetAttr(commentNode, "endTime", strEndTime);
                DomUtil.SetAttr(commentNode, "usedSeconds", strUsedSeconds);



                // 保存到文件
                dom.Save(xmlFile);

                // 更新界面listview这一行里的评估
                item.SubItems[6].Text = thisComment;

                item.SubItems[9].Text = timeInfo;


                // 重新转出一个html
                try
                {
                    ConvertHelper.Convert(xmlFile, htmlFile);
                }
                catch (Exception ex)
                {
                    //todo 这种情况如何处理
                }

                // 保存到专门的评语文件
                BorrowAnalysisService.Instance.SetComment2file(barcode, thisComment);
            }

            // 设为最小值
            this._startTime = DateTime.MinValue;
        }


        // 选择评语
        private void comboBox_comment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string comment = this.comboBox_comment.Text;
            if (comment == C_SelectComment)
                comment = "";

            // 设到编辑框
            this.SetCommentForEdit(comment);

            // 也同时保存一下评语,感觉直接保存，这样太快太自动了，还是让用户点一下提交评语为好。
            // this.SaveComment(comment);
        }

        // 评语开始时间
        DateTime _startTime = DateTime.MinValue;
        private void textBox_comment_Enter(object sender, EventArgs e)
        {
            //MessageBox.Show(this, "h");

            // 记下时间
            if (this._startTime == DateTime.MinValue)
            {
                this._startTime = DateTime.Now;
            }
        }

        // 编辑评语
        public void SetCommentForEdit(string comment)
        {
            // 设到编辑框
            this.textBox_comment.Text = comment;

            // 设一下开始时间
            this._startTime = DateTime.Now;
        }

        // 保存评语为模板
        private void button_saveCommentTemplate_Click(object sender, EventArgs e)
        {
            string comment = this.textBox_comment.Text.Trim();
            if (comment == "")
            {
                MessageBox.Show(this, "尚未输入评语。");
                return;
            }

            // 保存评语为模板
            bool bRet = BorrowAnalysisService.Instance.SaveCommentTemplate(comment, out string error);
            if (bRet == false)
            {
                MessageBox.Show(this, "保存失败：" + error);
                return;
            }

            // 装载到下拉列表
            this.LoadCommentTemplate();

            this.comboBox_comment.Text = comment;

            MessageBox.Show(this, "保存评语模板成功。");
        }

        #endregion

        #region 点击表头排序

        // 排序
        SortColumns SortColumns_report = new SortColumns();
        private void listView_files_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            chargingAnalysisForm.SortCol(this.listView_files, SortColumns_report, nClickColumn);
        }

        public static void SortCol(ListView myListView, SortColumns sortCol, int nClickColumn)
        {
            ColumnSortStyle sortStyle = ColumnSortStyle.LeftAlign;

            // 第3列借阅量，第4列排名，这两名是数值排序
            if (nClickColumn == 3 || nClickColumn == 4)
                sortStyle = ColumnSortStyle.RightAlign;

            sortCol.SetFirstColumn(nClickColumn,
                sortStyle,
                myListView.Columns,
                true);

            // 排序
            myListView.ListViewItemSorter = new SortColumnsComparer(sortCol);

            myListView.ListViewItemSorter = null;
        }


        #endregion

        #region 按钮事件

        // 点创建报表按钮
        private void button_createReport_Click(object sender, EventArgs e)
        {

            // 输出目录
            string dir = this.textBox_outputDir.Text.Trim();

            // 2022/12/7 没必要检查，因为进到里面，可以是做一些处理的事情，不一定是一键生成。
            //// 当设置的目录时，才进行目录是否合格，用户可能在此界面不选择，在创建报表界面才选择。
            //if (string.IsNullOrEmpty(dir) == false)
            //{

            //    // 如果目录不存在，则创建一个新目录
            //    if (Directory.Exists(dir) == false)
            //        Directory.CreateDirectory(dir);

            //    // 如果目录不是空目录，提醒。
            //    DirectoryInfo dirInfo = new DirectoryInfo(dir);
            //    if (dirInfo.GetFiles("*.xml").Length > 0
            //        || dirInfo.GetFiles("*.html").Length > 0)  // todo，后面里面可能会放一个cfg目录，里面是配置文件
            //    {
            //        MessageBox.Show(this, "报表目录[" + dir + "]里，存在已生成好的报表(xml/html文件)，创建报表时需要选择一个空目录。");
            //        return;
            //    }
            //}


            createReport dlg = new createReport();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.OutputDir = dir;
            //dlg.ShowDialog(this);
            DialogResult result = dlg.ShowDialog(this);


            // 需要重新显示报表，另外有可能目录都变了
            this.textBox_outputDir.Text = dlg.OutputDir;
            // 重新显示一下文件列表
            this.ShowFiles();
        }




        // 另存文件
        private void ToolStripMenuItem_download_Click(object sender, EventArgs e)
        {

            // 先做到另存一个文件

            if (this.listView_files.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请先选择另存的读者行");
                return;
            }



            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = this.textBox_outputDir.Text;
            DialogResult result = dlg.ShowDialog();

            // todo记住上次选择的目录

            if (result != DialogResult.OK || string.IsNullOrEmpty(dlg.SelectedPath) == true)
            {
                //MessageBox.Show(this, "取消");
                return;
            }

            string dir = dlg.SelectedPath;


            //if (this.listView_files.SelectedItems.Count > 1)
            //{
            //    MessageBox.Show(this, "您选择了多条读者，请选择一个读者另存。");
            //    return;
            //}

            foreach (ListViewItem viewItem in this.listView_files.SelectedItems)
            {

                //ListViewItem viewItem = this.listView_files.SelectedItems[0];

                // 用于拼文件名
                string barcode = viewItem.SubItems[0].Text;
                string name = viewItem.SubItems[1].Text;
                string tempFileName = barcode + "_" + name + ".html";

                // 源文件
                string htmlFile = viewItem.SubItems[8].Text;
                if (htmlFile == "")
                {
                    MessageBox.Show(this, "读者name还未转成html报表，无法导出");
                    return;
                }


                FileInfo fileInfo = new FileInfo(htmlFile);

                string targetFileName = dir + "\\" + fileInfo.Name;//  htmlFile;

                string html = "";
                using (StreamReader reader = new StreamReader(htmlFile))//, Encoding.UTF8))
                {
                    html = reader.ReadToEnd().Trim();
                }

                // StreamWriter当文件不存在时，会自动创建一个新文件。
                using (StreamWriter writer = new StreamWriter(targetFileName, false, Encoding.UTF8))
                {
                    // 写到打印文件
                    writer.Write(html);
                }

            }

            MessageBox.Show(this, "导出完成");

            // 打开文件
            //Process.Start(targetFileName);
        }

        #endregion

        private void button_toExcel_Click(object sender, EventArgs e)
        {
            string strDir = this.textBox_outputDir.Text.Trim();
            if (string.IsNullOrEmpty(strDir))
            {
                MessageBox.Show(this, "请选择报表所在的目录");
                return;
            }

            //if (this.listView_files.SelectedItems.Count ==0) {
            //    MessageBox.Show(this, "请选择要合并导出的行");
            //    return;
            //}


            DirectoryInfo dir = new DirectoryInfo(strDir);

            string excelFile = dir + "\\total.xlsx";


            // 创建excel对象
            XLWorkbook doc = null;
            try
            {
                doc = new XLWorkbook();// XLEventTracking.Disabled);
                // 如果原来文件存在，则先删除
                File.Delete(excelFile);
            }
            catch (Exception ex)
            {
                string error = ExceptionUtil.GetAutoText(ex);
                MessageBox.Show(this, "导出excel出错" + error);
                return;
            }


            //创建一个sheet
            IXLWorksheet sheet = null;
            sheet = doc.Worksheets.Add("借阅汇总");//"表格");
                                               // 设置第一行的列头信息

            sheet.Cell(1, 1).SetValue("借阅者编号");
            sheet.Cell(1, 2).SetValue("借阅者姓名");
            sheet.Cell(1, 3).SetValue("读者类型");
            sheet.Cell(1, 4).SetValue("借阅人员类别码");
            sheet.Cell(1, 5).SetValue("班级");

            sheet.Cell(1, 6).SetValue("图书编号");
            sheet.Cell(1, 7).SetValue("借阅日期");
            sheet.Cell(1, 8).SetValue("借期");
            sheet.Cell(1, 9).SetValue("预计归还日期");
            sheet.Cell(1, 10).SetValue("实际归还日期");

            int nRowIndex = 2;




            // 找到所有的xml文件
            FileInfo[] files = dir.GetFiles("*.xml");
            MessageBox.Show("找到" + files.Length + "个xml文件");


            foreach (FileInfo file in files)
            // foreach(ListViewItem item in this.listView_files.SelectedItems)
            {

                //string fileName = strDir+"\\"+item.Text + ".xml";

                string fileName = file.FullName;

                XmlDocument dom = new XmlDocument();
                dom.Load(fileName);
                XmlNode root = dom.DocumentElement;

                /*
  <borrows count="1">
    <borrowItem itemBarcode="B002" title="杜诗详注 [专著] / (唐)杜甫撰 ; (清)仇兆鳌注" borrowTime="2023-08-27 17:22:16" returnTime="" period="31day" accessNo="I222.742/D818" location="流通库" action="" />
  </borrows>
  <borrowHistory count="2">
    <borrowItem itemBarcode="B003" title="杜诗详注 [专著] / (唐)杜甫撰 ; (清)仇兆鳌注" borrowTime="2023-08-27 16:22:19" returnTime="2023-08-27 16:22:26" period="31day" accessNo="I222.742/D818" location="流通库" action="return" />
    <borrowItem itemBarcode="B002" title="杜诗详注 [专著] / (唐)杜甫撰 ; (清)仇兆鳌注" borrowTime="2023-08-27 16:22:17" returnTime="2023-08-27 16:22:25" period="31day" accessNo="I222.742/D818" location="流通库" action="return" />
  </borrowHistory>
                 */

                XmlNodeList list = root.SelectNodes("//borrowItem");
                if (list == null || list.Count == 0)
                    continue;

                // 先把读者基本信息取出来
                /*
    <barcode>P101</barcode>
    <readerType>本科生</readerType>
    <name>姓名101</name>
    <gender>男</gender>
    <department>2022级1班</department>
                 */

                string patronBarcode = DomUtil.GetElementText(root, "patron/barcode");
                string readerType = DomUtil.GetElementText(root, "patron/readerType");

                string readerTypeCode = "2";
                if (readerType != "学生")
                    readerTypeCode = "1";
                string name = DomUtil.GetElementText(root, "patron/name");
                string gender = DomUtil.GetElementText(root, "patron/gender");
                string department = DomUtil.GetElementText(root, "patron/department");

                foreach (XmlNode node in list)
                {
                    string itemBarcode = DomUtil.GetAttr(node, "itemBarcode");
                    //borrowTime
                    string borrowTime = DomUtil.GetAttr(node, "borrowTime");


                    //period
                    string period = DomUtil.GetAttr(node, "period");

                    // 根据借期算出应还日期
                    string strday = period;
                    string endDate = "";
                    int nIndex = period.IndexOf("day");
                    if (nIndex != -1)
                    {
                        strday = period.Substring(0, nIndex);
                    }
                    int day = 0;
                    try
                    {
                        day = Convert.ToInt32(strday);
                    }
                    catch (Exception ex) { }

                    DateTime startDate = DateTime.Parse(borrowTime);
                    endDate = startDate.AddDays(day).ToString("yyyy-MM-dd HH:mm:ss");


                    //returnTime
                    string returnTime = DomUtil.GetAttr(node, "returnTime");


                    sheet.Cell(nRowIndex, 1).SetValue(patronBarcode);
                    sheet.Cell(nRowIndex, 2).SetValue(name);
                    sheet.Cell(nRowIndex, 3).SetValue(readerType);
                    sheet.Cell(nRowIndex, 4).SetValue(readerTypeCode);
                    sheet.Cell(nRowIndex, 5).SetValue(department);

                    sheet.Cell(nRowIndex, 6).SetValue(itemBarcode);
                    sheet.Cell(nRowIndex, 7).SetValue(borrowTime);
                    sheet.Cell(nRowIndex, 8).SetValue(period);
                    sheet.Cell(nRowIndex, 9).SetValue(endDate);
                    sheet.Cell(nRowIndex, 10).SetValue(returnTime);

                    nRowIndex++;

                }

            }


            // 保存excel文件
            doc.SaveAs(excelFile);
            doc.Dispose();

            // 自动打开excel文件
            try
            {
                System.Diagnostics.Process.Start(excelFile);
            }
            catch
            {

            }
        }

        private void 复制证条码号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = "";
            foreach (ListViewItem item in this.listView_files.SelectedItems)
            {
                text += item.Text + "\r\n";
            }

            Clipboard.SetDataObject(text);//复制内容到粘贴板
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (this.listView_files.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "尚未选中需要导出的事项。");
                return;
            }

            string strDir = this.textBox_outputDir.Text.Trim();
            if (string.IsNullOrEmpty(strDir))
            {
                MessageBox.Show(this, "请选择报表所在的目录");
                return;
            }

            //if (this.listView_files.SelectedItems.Count ==0) {
            //    MessageBox.Show(this, "请选择要合并导出的行");
            //    return;
            //}


            DirectoryInfo dir = new DirectoryInfo(strDir);

            string excelFile = dir + "\\列表.xlsx";


            // 创建excel对象
            XLWorkbook doc = null;
            try
            {
                doc = new XLWorkbook();// XLEventTracking.Disabled);
                // 如果原来文件存在，则先删除
                File.Delete(excelFile);
            }
            catch (Exception ex)
            {
                string error = ExceptionUtil.GetAutoText(ex);
                MessageBox.Show(this, "导出列表出错" + error);
                return;
            }


            //创建一个sheet
            IXLWorksheet sheet = null;
            sheet = doc.Worksheets.Add("借阅量排名");//"表格");




            // 设置第一行的列头信息

            sheet.Cell(1, 1).SetValue("证条码号");
            sheet.Cell(1, 2).SetValue("姓名");
            sheet.Cell(1, 3).SetValue("班级");
            sheet.Cell(1, 4).SetValue("借书数量");
            sheet.Cell(1, 5).SetValue("排名");

            sheet.Cell(1, 6).SetValue("称号");
            sheet.Cell(1, 7).SetValue("备注");

            int nRowIndex = 2;


            foreach (ListViewItem item in this.listView_files.SelectedItems)
            {

                sheet.Cell(nRowIndex, 1).SetValue(item.SubItems[0].Text);
                sheet.Cell(nRowIndex, 2).SetValue(item.SubItems[1].Text);
                sheet.Cell(nRowIndex, 3).SetValue(item.SubItems[2].Text);
                sheet.Cell(nRowIndex, 4).SetValue(item.SubItems[3].Text);
                sheet.Cell(nRowIndex, 5).SetValue(item.SubItems[4].Text);

                sheet.Cell(nRowIndex, 6).SetValue(item.SubItems[5].Text);
                sheet.Cell(nRowIndex, 7).SetValue(item.SubItems[6].Text);


                nRowIndex++;

            }

            // 保存excel文件
            doc.SaveAs(excelFile);
            doc.Dispose();

            // 自动打开excel文件
            try
            {
                System.Diagnostics.Process.Start(excelFile);
            }
            catch
            {

            }
        }


    }
}
