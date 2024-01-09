﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing.Printing;

using DigitalPlatform.Xml;
using DigitalPlatform.Marc;
using DigitalPlatform.LibraryRestClient;
using System.Collections.Generic;
using DigitalPlatform.IO;
using DigitalPlatform;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using DigitalPlatform.LibraryClient;
using ClosedXML.Excel;
using static System.Windows.Forms.ListView;
using DigitalPlatform.dp2.Statis;
using System.Linq;
using DocumentFormat.OpenXml.InkML;
using DigitalPlatform.Text;

namespace dp2mini
{
    public partial class TransferStatisForm : Form
    {
        // mid父窗口
        MainForm _mainForm = null;

        // 名字以用途命名即可。TokenSource 这种类型名称可以不出现在名字中
        CancellationTokenSource _cancel = new CancellationTokenSource();


        /// <summary>
        ///  构造函数
        /// </summary>
        public TransferStatisForm()
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
        }


        string _libraryCode = "";

        /// <summary>
        /// 检索预约到书记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_search_Click(object sender, EventArgs e)
        {
            //时间范围
            string startDate = this.dateTimePicker_start.Value.ToString("yyyyMMdd");
            string endDate = this.dateTimePicker_end.Value.ToString("yyyyMMdd");

            this._libraryCode = this.textBox_librarycode.Text.Trim();


            // 每次开头都重新 new 一个。这样避免受到上次遗留的 _cancel 对象的状态影响
            this._cancel.Dispose();
            this._cancel = new CancellationTokenSource();

            // 开一个新线程
            Task.Run(() =>
            {
                doSearch(this._cancel.Token,
                    startDate,
                    endDate);
            });
        }

        /// <summary>
        /// 检索做事的函数
        /// </summary>
        /// <param name="token"></param>
        private void doSearch(CancellationToken token,
            string startDate,
            string endDate)
        {
            string strError = "";

            // 记下原来的光标
            Cursor oldCursor = Cursor.Current;

            // 用Invoke线程安全的方式来调
            this.Invoke((Action)(() =>
            {
                // 设置按钮状态
                EnableControls(false);

                //清空界面数据
                this.ClearInfo();

                // 鼠标设为等待状态
                oldCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
            }
            ));

            RestChannel channel = this._mainForm.GetChannel();
            try
            {

                int nRet = OperLogLoader.MakeLogFileNames(startDate,
                    endDate,
                    false,  // 是否包含扩展名 ".log"
                    out List<string> dates,
                    out string strWarning,
                    out strError);
                if (nRet == -1)
                {
                    goto ERROR1;
                }

                {
                    //ProgressEstimate estimate = new ProgressEstimate();

                    OperLogLoader loader = new OperLogLoader
                    {
                        Channel = channel,
                        Stop = null, //  this.Progress;
                                     // loader.owner = this;
                        Estimate = null,// estimate,
                        Dates = dates,
                        Level = 0,  // 2019/7/23 注：2 最简略。不知何故以前用了这个级别。缺点是 oldRecord 元素缺乏 InnerText
                        AutoCache = false,
                        CacheDir = "",
                        LogType = LogType.OperLog,//logType,
                        Filter = "",// setReaderInfo",
                        ServerVersion = ""//serverVersion
                    };

                    //loader.Prompt += Loader_Prompt;
                    try
                    {

                        int nIndex = 0;

                        foreach (OperLogItem item in loader)
                        {
                            token.ThrowIfCancellationRequested();

                            this.Invoke((Action)(() =>
                            {
                                nRet = this.LoadLog(item, out strError);
                                if (nRet == -1)
                                    throw new Exception(strError);

                                    // 通过外面显示一下进度
                                    this._mainForm.SetStatusMessage("正在装载 " + item.Date + "-" + item.Index.ToString());
                            }
                            ));

                            nIndex++;
                        }


                        this.Invoke((Action)(() =>
                        {
                            //if (todoCount > 0)
                            //{
                            //    // 按证条码号排序
                            //    this.SortCol(1);
                            //}
                            // 任务栏显示信息
                            this._mainForm.SetStatusMessage("装载完成，共装入"+nIndex.ToString()+"条日志。");
                        }));
                    }
                    finally
                    {
                        //loader.Prompt -= Loader_Prompt;
                        //channel.Timeout = old_timeout;
                    }
                }

                // 进行取合，放在借书报表中
                this.Invoke((Action)(() =>
                {
                    // 借书统计
                    this.StatisBorrow();

                    //2024/1/9 按证条码统计
                    this.StatisBorrowByBarcode();

                    // 还书统计
                    this.StatisReturn();

                    // 借还统计
                    this.StatisBorrowAndReturn();

                }
));

                return;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                goto ERROR1;
            }
            finally
            {
                // 设置按置状态
                this.Invoke((Action)(() =>
                {
                    EnableControls(true);
                    this.Cursor = oldCursor;

                    this._mainForm.ReturnChannel(channel);
                }
                ));
            }

        ERROR1:
            this.Invoke((Action)(() =>
            {
                MessageBox.Show(strError);
            }
            ));
        }

        // 对借书日志进行聚合
        public void StatisBorrow()
        {
            // 让用户选择需要统计的范围。根据批次号、目标位置来进行选择
            var list = this._borrowItems.GroupBy(
                x => new { x.location, x.readerBarcode, x.readerName,x.dept },
                (key, item_list) => new BorrowGroup
                {
                    location = key.location,
                    readerBarcode = key.readerBarcode,
                    readerName = key.readerName,
                    dept = key.dept,
                    Items = new List<BorrowLogItem>(item_list)
                    
                }).OrderByDescending(o=>o.Items.Count).OrderBy(o => o.location).ToList();


            foreach (BorrowGroup group in list)
            {
                ListViewItem viewItem = new ListViewItem(group.location, 0);
                this.listView_borrowStatis.Items.Add(viewItem);

                string temp = "";
                List<string> pList = new List<string>();
                foreach (BorrowLogItem item in group.Items)
                {
                    //temp += item.price + ";";
                    pList.Add(item.price);
                }
                string price=PriceUtil.TotalPrice(pList);

                viewItem.SubItems.Add(group.readerBarcode);
                viewItem.SubItems.Add(group.readerName);
                viewItem.SubItems.Add(group.dept);
                viewItem.SubItems.Add(group.Items.Count.ToString());

                viewItem.SubItems.Add(price);
            }

            //if (this._borrowItems.Count > 0)
            //{
            //    // 按证条码号排序
            //    SortCol(this.listView_borrowStatis, this.SortColumns_borrowStatis, 3);

            //    SortCol(this.listView_borrowStatis, this.SortColumns_borrowStatis, 0);
            //}
        }

        // 对借书日志按证条码号聚合
        public void StatisBorrowByBarcode()
        {
            // 让用户选择需要统计的范围。根据批次号、目标位置来进行选择
            var list = this._borrowItems.GroupBy(
                x => new {  x.readerBarcode },
                (key, item_list) => new BorrowGroup
                {
                    //location = key.location,
                    readerBarcode = key.readerBarcode,
                    //readerName = key.readerName,
                    //dept = key.dept,
                    Items = new List<BorrowLogItem>(item_list)

                }).OrderByDescending(o => o.Items.Count).OrderBy(o => o.location).ToList();


            foreach (BorrowGroup group in list)
            {
                ListViewItem viewItem = new ListViewItem(group.readerBarcode, 0);
                this.listView_borrowByBarcode.Items.Add(viewItem);

                string temp = "";
                List<string> pList = new List<string>();
                foreach (BorrowLogItem item in group.Items)
                {
                    //temp += item.price + ";";
                    pList.Add(item.price);
                }
                string price = PriceUtil.TotalPrice(pList);

                string patronName = "";
                string dept = "";

                if (group.Items != null && group.Items.Count > 0)
                {
                    patronName = group.Items[0].readerName;
                    dept= group.Items[0].dept;
                    string borrowDate = group.Items[0].borrowDate;

                    foreach (BorrowLogItem item in group.Items)
                    {
                        
                        if (borrowDate!=null && item.borrowDate!=null )
                        {
                            DateTime old = DateTimeUtil.ParseFreeTimeString(borrowDate);
                            DateTime new1 = DateTimeUtil.ParseFreeTimeString(item.borrowDate);

                            if (new1.CompareTo(old) > 0)
                            {
                                borrowDate = item.borrowDate;
                                patronName = item.readerName;
                                dept = item.dept;
                            }
                        }
                    }

                }



                //viewItem.SubItems.Add(group.readerBarcode);
                viewItem.SubItems.Add(patronName);
                viewItem.SubItems.Add(dept);
                viewItem.SubItems.Add(group.Items.Count.ToString());

                viewItem.SubItems.Add(price);
            }

            //if (this._borrowItems.Count > 0)
            //{
            //    // 按证条码号排序
            //    SortCol(this.listView_borrowStatis, this.SortColumns_borrowStatis, 3);

            //    SortCol(this.listView_borrowStatis, this.SortColumns_borrowStatis, 0);
            //}
        }

        // 对还书日志进行聚合
        public void StatisReturn()
        {
            // 让用户选择需要统计的范围。根据批次号、目标位置来进行选择
            var list = this._returnItems.GroupBy(
                x => new { x.location, x.readerBarcode, x.readerName,x.dept },
                (key, item_list) => new BorrowGroup
                {
                    location = key.location,
                    readerBarcode = key.readerBarcode,
                    readerName = key.readerName,
                    dept=key.dept,
                    Items = new List<BorrowLogItem>(item_list)
                }).OrderByDescending(o => o.Items.Count).OrderBy(o => o.location).ToList();


            foreach (BorrowGroup group in list)
            {
                ListViewItem viewItem = new ListViewItem(group.location, 0);
                this.listView_returnStatis.Items.Add(viewItem);

                viewItem.SubItems.Add(group.readerBarcode);
                viewItem.SubItems.Add(group.readerName);
                viewItem.SubItems.Add(group.dept);
                viewItem.SubItems.Add(group.Items.Count.ToString());
            }
        }


        // 对借书和还书日志进行聚合
        public void StatisBorrowAndReturn()
        {
            // 让用户选择需要统计的范围。根据批次号、目标位置来进行选择
            var list = this._borrowAndReturnItems.GroupBy(
                x => new {  x.readerBarcode, x.readerName, x.dept },
                (key, item_list) => new BorrowGroup
                {
                    //location = key.location,

                    readerBarcode = key.readerBarcode,
                    readerName = key.readerName,
                    dept = key.dept,
                    Items = new List<BorrowLogItem>(item_list)
                }).OrderByDescending(o => o.Items.Count).OrderBy(o => o.location).ToList();


            foreach (BorrowGroup group in list)
            {
                ListViewItem viewItem = new ListViewItem(group.readerBarcode, 0);
                this.listView_borrowAndReurn_statis.Items.Add(viewItem);

                //viewItem.SubItems.Add(group.readerBarcode);
                viewItem.SubItems.Add(group.readerName);
                viewItem.SubItems.Add(group.dept);
                viewItem.SubItems.Add(group.Items.Count.ToString());
            }
        }



        // 日志记录hastable,方便点一条，在右侧看到详细信息
        public Hashtable _logItems = new Hashtable();

        // 用于做借书聚合的类
        List<BorrowLogItem> _borrowItems = new List<BorrowLogItem>();

        // 用于做还书聚合的类
        List<BorrowLogItem> _returnItems = new List<BorrowLogItem>();

        // 用于做借还聚合的类
        List<BorrowLogItem> _borrowAndReturnItems = new List<BorrowLogItem>();

        public int LoadLog(OperLogItem logItem,out string error)
        {
            error = "";

            //"<operation>setEntity</operation><libraryCode></libraryCode><action>new</action>
            //<style>outofrangeAsError</style><record recPath=\"中文图书实体/1\">&lt;root&gt;&lt;parent&gt;1&lt;/parent&gt;&lt;location&gt;流通库&lt;/location&gt;&lt;price&gt;CNY28.80&lt;/price&gt;&lt;bookType&gt;普通&lt;/bookType&gt;&lt;accessNo&gt;I563.85/H022&lt;/accessNo&gt;&lt;barcode&gt;B001&lt;/barcode&gt;&lt;refID&gt;b0067871-f39d-4c7e-aacf-750855ebfde0&lt;/refID&gt;&lt;operations&gt;&lt;operation name=\"create\" time=\"Wed, 07 Apr 2021 12:26:49 +0800\" operator=\"supervisor\" /&gt;&lt;/operations&gt;&lt;/root&gt;</record>
            // <operator>supervisor</operator><operTime>Wed, 07 Apr 2021 12:26:49 +0800</operTime><clientAddress via=\"net.pipe://localhost/dp2library/xe\">localhost</clientAddress>
            //<version>1.08</version>"
            string xml = logItem.Xml;
            if (string.IsNullOrEmpty(xml) == true)
                return 0;
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode root = dom.DocumentElement;

            // 取出馆代码
            string libraryCode = DomUtil.GetElementInnerText(root, "libraryCode");

            // 如果输入了馆代码，则进行过滤
            if (string.IsNullOrEmpty(this._libraryCode) == false)
            {
                // 总馆的过滤掉
                if (string.IsNullOrEmpty(libraryCode) == true)
                    return 0;

                // 不匹配的过滤掉
                if (libraryCode.IndexOf(this._libraryCode) == -1)
                    return 0;
            }

            //// 如果输入了馆代码，则进行过滤
            //if (string.IsNullOrEmpty(this._libraryCode) == false &&   libraryCode.IndexOf(this._libraryCode) ==-1)
            //    return 0;


            // 操作类型 operation/action
            string operation = DomUtil.GetElementText(root, "operation");
            string action = DomUtil.GetElementText(root, "action");
            string operType= operation+"/"+ action;

            // 操作者
            string operator1 = DomUtil.GetElementInnerText(root, "operator");

            // 操作时间
            string operTime = DomUtil.GetElementInnerText(root, "operTime");
            string strOperTime = GetRfc1123DisplayString(operTime);


            // 给总表中加记录
            {
                /*
                文件名
                序号
                馆代码
                操作类型
                操作者
                操作时间
                耗时
                */
                ListViewItem viewItem = new ListViewItem(logItem.Date, 0);
                this.listView_results.Items.Add(viewItem);
                viewItem.SubItems.Add(logItem.Index.ToString());
                viewItem.SubItems.Add(libraryCode);
                viewItem.SubItems.Add(operType);
                viewItem.SubItems.Add(operator1);
                viewItem.SubItems.Add(strOperTime);
            }

            // 加到hashtable里，方便点击单独看详细信息
            string key = logItem.Date + "-" + logItem.Index;
            _logItems[key] = logItem;


            // 如果是borrow加到借书表中，发现borrow和return的格式一致
            if (operation == "borrow" || operation=="return")
            {
                ListViewItem viewItem = new ListViewItem(logItem.Date, 0);

                // 根据操作类型，加到不同的表格
                if (operation == "borrow")
                    this.listView_log_borrow.Items.Add(viewItem);
                else
                    this.listView_return.Items.Add(viewItem);
                
                
                viewItem.SubItems.Add(logItem.Index.ToString());
                viewItem.SubItems.Add(libraryCode);

                int nRet = GetBorrowInfo(dom, out BorrowLogItem borrowLog, out string strError);
                if (nRet == -1)
                {
                    string info = "获取借书详细信息出错" + strError;
                    viewItem.SubItems.Add(info);
                    viewItem.SubItems.Add("");
                    viewItem.SubItems.Add("");
                    viewItem.SubItems.Add("");
                    viewItem.SubItems.Add("");
                    viewItem.SubItems.Add("");
                }
                else
                {
                    // 馆藏地
                    // 证条码号
                    // 姓名
                    // 册条码号
                    viewItem.SubItems.Add(borrowLog.location);
                    viewItem.SubItems.Add(borrowLog.readerBarcode);
                    viewItem.SubItems.Add(borrowLog.readerName);
                    viewItem.SubItems.Add(borrowLog.dept);
                    viewItem.SubItems.Add(borrowLog.itemBarcode);

                    viewItem.SubItems.Add(borrowLog.price);

                    //borrowLog.pri

                    // 根据类型判断，加到对应的内存集中
                    if (operation == "borrow")
                        this._borrowItems.Add(borrowLog);
                    else
                        this._returnItems.Add(borrowLog);

                    // 借还都算
                    _borrowAndReturnItems.Add(borrowLog);
                }


                viewItem.SubItems.Add(operator1);
                viewItem.SubItems.Add(strOperTime);

            }

            return 0;
        }

        /// <summary>
        /// 获得 RFC 1123 时间字符串的显示格式字符串
        /// </summary>
        /// <param name="strRfc1123TimeString">时间字符串。RFC1123 格式</param>
        /// <returns>显示格式</returns>
        public static string GetRfc1123DisplayString(string strRfc1123TimeString)
        {
            if (string.IsNullOrEmpty(strRfc1123TimeString) == true)
                return "";

            try
            {
                return DateTimeUtil.Rfc1123DateTimeStringToLocal(strRfc1123TimeString, "G");// + " (" + strRfc1123TimeString + ")";
            }
            catch (Exception ex)
            {
                return "解析 RFC1123 时间字符串 '" + strRfc1123TimeString + "' 时出错: " + ex.Message;
            }
        }

        static string GetTimeString(XmlDocument dom)
        {
            XmlElement time = dom.DocumentElement.SelectSingleNode("time") as XmlElement;
            if (time == null)
                return "";
            return time.GetAttribute("seconds") + " 秒; " + time.GetAttribute("start") + " - " + time.GetAttribute("end");
        }

        /// <summary>
        /// 清空界面数据
        /// </summary>
        public void ClearInfo()
        {
            // 清空listview
            this.listView_results.Items.Clear();

            // 清空借书列表
            this.listView_log_borrow.Items.Clear();
            this.listView_borrowStatis.Items.Clear();
            this.listView_borrowByBarcode.Items.Clear();
            // 清空借书记录内存表
            _borrowItems.Clear();

            // 清空借书列表
            this.listView_return.Items.Clear();
            this.listView_returnStatis.Items.Clear();
            this._returnItems.Clear();

            // 借还统计信息
            this.listView_borrowAndReurn_statis.Items.Clear();
            this._borrowAndReturnItems.Clear();

            //设置父窗口状态栏参数
            this._mainForm.SetStatusMessage("");
        }

        /// <summary>
        /// 设置控件是否可用
        /// </summary>
        /// <param name="bEnable"></param>
        void EnableControls(bool bEnable)
        {
            this.button_search.Enabled = bEnable;
            this.button_stop.Enabled = !(bEnable);

            this.button_toExcel.Enabled = bEnable;
        }

        /// <summary>
        /// 停止检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_stop_Click(object sender, EventArgs e)
        {
            // 停止
            this._cancel.Cancel();
        }



        delegate void Delegate_QuickSetFilenames(Control control);

        void QuickSetFilenames(Control control)
        {
            string strStartDate = "";
            string strEndDate = "";

            string strName = control.Text.Replace(" ", "").Trim();

            if (strName == "今天")
            {
                DateTime now = DateTime.Now;

                strStartDate = DateTimeUtil.DateTimeToString8(now);
                strEndDate = DateTimeUtil.DateTimeToString8(now);
            }
            else if (strName == "本周")
            {
                DateTime now = DateTime.Now;
                int nDelta = (int)now.DayOfWeek; // 0-6 sunday - saturday
                DateTime start = now - new TimeSpan(nDelta, 0, 0, 0);

                strStartDate = DateTimeUtil.DateTimeToString8(start);
                // strEndDate = DateTimeUtil.DateTimeToString8(start + new TimeSpan(7, 0,0,0));
                strEndDate = DateTimeUtil.DateTimeToString8(now);
            }
            else if (strName == "本月")
            {
                DateTime now = DateTime.Now;
                strEndDate = DateTimeUtil.DateTimeToString8(now);
                strStartDate = strEndDate.Substring(0, 6) + "01";
            }
            else if (strName == "本年")
            {
                DateTime now = DateTime.Now;
                strEndDate = DateTimeUtil.DateTimeToString8(now);
                strStartDate = strEndDate.Substring(0, 4) + "0101";
            }
            else if (strName == "最近七天" || strName == "最近7天")
            {
                DateTime now = DateTime.Now;
                DateTime start = now - new TimeSpan(7 - 1, 0, 0, 0);

                strStartDate = DateTimeUtil.DateTimeToString8(start);
                strEndDate = DateTimeUtil.DateTimeToString8(now);
            }
            else if (strName == "最近三十天" || strName == "最近30天")
            {
                DateTime now = DateTime.Now;
                DateTime start = now - new TimeSpan(30 - 1, 0, 0, 0);
                strStartDate = DateTimeUtil.DateTimeToString8(start);
                strEndDate = DateTimeUtil.DateTimeToString8(now);
            }
            else if (strName == "最近三十一天" || strName == "最近31天")
            {
                DateTime now = DateTime.Now;
                DateTime start = now - new TimeSpan(31 - 1, 0, 0, 0);
                strStartDate = DateTimeUtil.DateTimeToString8(start);
                strEndDate = DateTimeUtil.DateTimeToString8(now);
            }
            else if (strName == "最近三百六十五天" || strName == "最近365天")
            {
                DateTime now = DateTime.Now;
                DateTime start = now - new TimeSpan(365 - 1, 0, 0, 0);
                strStartDate = DateTimeUtil.DateTimeToString8(start);
                strEndDate = DateTimeUtil.DateTimeToString8(now);
            }
            else if (strName == "最近十年" || strName == "最近10年")
            {
                DateTime now = DateTime.Now;
                DateTime start = now - new TimeSpan(10 * 365 - 1, 0, 0, 0);
                strStartDate = DateTimeUtil.DateTimeToString8(start);
                strEndDate = DateTimeUtil.DateTimeToString8(now);
            }
            else
            {
                MessageBox.Show(this, "无法识别的周期 '" + strName + "'");
                return;
            }

            this.dateTimePicker_start.Value = DateTimeUtil.Long8ToDateTime(strStartDate);
            this.dateTimePicker_end.Value = DateTimeUtil.Long8ToDateTime(strEndDate);
        }

        private void comboBox_quickSetFilenames_SelectedIndexChanged(object sender, EventArgs e)
        {
            Delegate_QuickSetFilenames d = new Delegate_QuickSetFilenames(QuickSetFilenames);
            this.BeginInvoke(d, new object[] { sender });
        }

 

        // 把列表中的全部记录导出excel
        private void button_toExcel_Click(object sender, EventArgs e)
        {

            // 先选择导出哪些表
            SelectSheetForm dlg = new SelectSheetForm(this);
            dlg.StartPosition = FormStartPosition.CenterScreen;
            DialogResult result= dlg.ShowDialog(this);
            if (result == DialogResult.Cancel)
            {
                MessageBox.Show(this, "用户放弃导出");
                return;
            }

            if (result == DialogResult.OK)
            {
                if (dlg.OutputTables == null || dlg.OutputTables.Count == 0)
                {
                    MessageBox.Show(this, "用户未选择要导出的表。");
                    return;
                }

                string strError = "";
                List<SheetItem> sheets = new List<SheetItem>();  // 准备要导出表的集合

                bool bHasData = false;

                foreach (string tabName in dlg.OutputTables)
                {
                    ListView temp = null;
                    if (tabName == "日志总表")
                        temp = this.listView_results;
                    else if (tabName == "借书日志")
                        temp = this.listView_log_borrow;
                    else if (tabName == "借书统计")
                        temp = this.listView_borrowStatis;
                    else if (tabName == "还书日志")
                        temp = this.listView_return;
                    else if (tabName == "还书统计")
                        temp = this.listView_returnStatis;
                    else if (tabName == "借还统计")
                        temp = this.listView_borrowAndReurn_statis;
                    else if (tabName == "借书统计(按证条码）")
                        temp = this.listView_borrowByBarcode;
                    else
                        continue;  // 不认识的表

                    SheetItem sheet = new SheetItem();
                    sheet.sheetName = tabName;
                    sheet.items = new List<ListViewItem>();
                    foreach (ListViewItem item in temp.Items)
                    {
                        sheet.items.Add(item);

                        if (bHasData == false)
                            bHasData = true;
                    }
                    sheets.Add(sheet);  // 加到集合里
                }


                // 如果所有表中都没有数据，提示用户说没有数据，请先查询
                if (bHasData == false)
                {
                    MessageBox.Show(this, "您要导出的列表中均没有数据，请先查询，在下方列表看到数据后，再导出。");
                    return;
                }



                // return:
                //      -1  出错
                //      0   放弃或中断
                //      1   成功
                int nRet = ClosedXmlUtil.ExportToExcel(null, sheets, out strError);
                if (nRet == -1)
                {
                    MessageBox.Show(this, "导出excel出错:" + strError);
                    return;
                }
            }
        }

        private void listView_results_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 先把xml置空
            this.textBox_xml.Text = "";

            if (this.listView_results.SelectedItems.Count == 0)
                return;

            ListViewItem item = this.listView_results.SelectedItems[0];

            string key = item.SubItems[0].Text + "-" + item.SubItems[1].Text;

            OperLogItem logItem = (OperLogItem)this._logItems[key];
            if (logItem == null)
                return;

            //this.textBox_xml.Text = logItem.Xml;

            string xml = logItem.Xml;
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);

            // 把这几个节点处理为容易看的格式，原来是作为node的text的
            ToViewString(dom, "record");
            ToViewString(dom, "oldRecord");
            ToViewString(dom, "readerRecord");
            ToViewString(dom, "itemRecord");

            string strError = "";
            string strXml = "";
            int nRet = DomUtil.GetIndentXml(dom.OuterXml,
                true,
                out strXml,
                out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
                return;
            }

            this.textBox_xml.Text = strXml;
        }


        public void ToViewString(XmlDocument dom, string nodeName)
        {
            string partXml = DomUtil.GetElementInnerText(dom.DocumentElement, nodeName);
            if (string.IsNullOrEmpty(partXml) == false)
            {
                XmlDocument partDom = new XmlDocument();
                partDom.LoadXml(partXml);
                DomUtil.SetElementInnerXml(dom.DocumentElement, nodeName, partDom.OuterXml);
            }
        }




        // Borrow
        int GetBorrowInfo(XmlDocument dom,
            out BorrowLogItem borrowLog,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            // 返回的借书日志对象
             borrowLog = new BorrowLogItem();

            XmlNode root = dom.DocumentElement;

            borrowLog.readerBarcode = DomUtil.GetElementText(root, "readerBarcode");
            borrowLog.itemBarcode = DomUtil.GetElementText(root, "itemBarcode");

            //string strBorrowDate = GetRfc1123DisplayString(DomUtil.GetElementText(root, "borrowDate"));
            //string strBorrowPeriod = DomUtil.GetElementText(root, "borrowPeriod");

            borrowLog.borrowDate = GetRfc1123DisplayString(DomUtil.GetElementText(root, "borrowDate"));
            borrowLog.borrowPeriod = DomUtil.GetElementText(root, "borrowPeriod");

            XmlNode node = null;

            // 获取读者信息
            string strReaderXml= DomUtil.GetElementText(root, "readerRecord", out node);
            if (node != null)
            {
                XmlDocument reader_dom = new XmlDocument();
                try
                {
                    reader_dom.LoadXml(strReaderXml);
                }
                catch (Exception ex)
                {
                    strError = "读者记录XML装入DOM时出错: " + ex.Message;
                    return -1;
                }

                //string strBarcode = DomUtil.GetElementInnerText(reader_dom.DocumentElement, "barcode");
                //string strState = DomUtil.GetElementInnerText(reader_dom.DocumentElement, "state");
                borrowLog.readerType = DomUtil.GetElementInnerText(reader_dom.DocumentElement, "readerType");
                //string strCardNumber = DomUtil.GetElementInnerText(reader_dom.DocumentElement, "cardNumber");
               // string strComment = DomUtil.GetElementInnerText(reader_dom.DocumentElement, "comment");
                //string strCreateDate = GetRfc1123DisplayString(DomUtil.GetElementInnerText(reader_dom.DocumentElement, "createDate"));
                //string strExpireDate = GetRfc1123DisplayString(DomUtil.GetElementInnerText(reader_dom.DocumentElement, "expireDate"));
                borrowLog.readerName = DomUtil.GetElementInnerText(reader_dom.DocumentElement, "name");

                //><department>单位</department>
                borrowLog.dept = DomUtil.GetElementInnerText(reader_dom.DocumentElement, "department");

            }

            // 获取图书信息
            string strItemRecord = DomUtil.GetElementText(dom.DocumentElement, "itemRecord", out node);
            if (node != null)
            {

                XmlDocument item_dom = new XmlDocument();
                try
                {
                    item_dom.LoadXml(strItemRecord);
                }
                catch (Exception ex)
                {
                    strError = "读者记录XML装入DOM时出错: " + ex.Message;
                    return -1;
                }

                //string strBarcode = DomUtil.GetElementInnerText(item_dom.DocumentElement, "barcode");
               // string strState = DomUtil.GetElementInnerText(item_dom.DocumentElement, "state");
                borrowLog.bookType = DomUtil.GetElementInnerText(item_dom.DocumentElement, "bookType");
               // string strPublishTime = DomUtil.GetElementInnerText(item_dom.DocumentElement, "publishTime");
                borrowLog.location = DomUtil.GetElementInnerText(item_dom.DocumentElement, "location");
                //string strShelfNo = DomUtil.GetElementInnerText(item_dom.DocumentElement, "shelftNo");

                //2024/1/3 取出price
                borrowLog.price = DomUtil.GetElementInnerText(item_dom.DocumentElement, "price");


            }



            return 0;
        }

        #region 关于点列头排序

        // 借书日志 点列头排序
        private void listView_log_borrow_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            SortCol(this.listView_log_borrow, SortColumns_borrow, nClickColumn);
        }

        // 借书统计 点列头排序
        private void listView_borrowStatis_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            SortCol(this.listView_borrowStatis, SortColumns_borrowStatis, nClickColumn);
        }

        // 还书日志 点列头排序
        private void listView_return_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            SortCol(this.listView_return, SortColumns_return, nClickColumn);
        }

        // 还书统计 点列头排序
        private void listView_returnStatis_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            SortCol(this.listView_returnStatis, SortColumns_borrowStatis, nClickColumn);
        }

        // 点击列头排序
        private void listView_results_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            SortCol(this.listView_results, SortColumns_all, nClickColumn);
        }

        private void listView_borrowAndReurn_statis_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            SortCol(this.listView_borrowAndReurn_statis, SortColumns_borrowAndReturnStatis, nClickColumn);
        }

        SortColumns SortColumns_all = new SortColumns();
        SortColumns SortColumns_borrow = new SortColumns();
        SortColumns SortColumns_borrowStatis = new SortColumns();
        SortColumns SortColumns_return = new SortColumns();
        SortColumns SortColumns_returnStatis = new SortColumns();
        SortColumns SortColumns_borrowStatisByBarcode = new SortColumns();

        SortColumns SortColumns_borrowAndReturnStatis = new SortColumns();

        public static void SortCol(ListView myListView, SortColumns sortCol, int nClickColumn)
        {
            ColumnSortStyle sortStyle = ColumnSortStyle.LeftAlign;

            // 第一列为记录路径，排序风格特殊
            if (nClickColumn == 0)
                sortStyle = ColumnSortStyle.RecPath;

            sortCol.SetFirstColumn(nClickColumn,
                sortStyle,
                myListView.Columns,
                true);

            // 排序
            myListView.ListViewItemSorter = new SortColumnsComparer(sortCol);

            myListView.ListViewItemSorter = null;
        }

        #endregion

        private void ToolStripMenuItem_huizong_Click(object sender, EventArgs e)
        {
            string totalCountText = "";
            ListView mylistview = null;
            if (this.tabControl_table.SelectedTab.Text == "借书统计")
            {
                mylistview = listView_borrowStatis;
                totalCountText = "借书";
            }
            else if (this.tabControl_table.SelectedTab.Text == "还书统计")
            {
                mylistview = this.listView_returnStatis;
                totalCountText = "还书";
            }

            if (mylistview == null)
                return;

            if (mylistview.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "请先选中要汇总的行。");
                return;
            }

            List<BorrowGroup> list = new List<BorrowGroup>();
            foreach (ListViewItem item in mylistview.SelectedItems)
            {
                BorrowGroup group = new BorrowGroup();
                group.location = item.SubItems[0].Text;
                group.readerBarcode = item.SubItems[1].Text;
                group.readerName = item.SubItems[2].Text;
                group.count = Convert.ToInt32(item.SubItems[4].Text);
                list.Add(group);
            }

            string text = "";

            var groups = list.GroupBy(p => p.location);
            foreach (var group in groups)
            {
                //Console.WriteLine(group.Key);

                string location = group.Key;

                int readerCount = 0;
                int totalCount = 0;
                foreach (var one in group)
                {
                    readerCount++;

                    totalCount += one.count;

                    //Console.WriteLine($"\t{person.Name},{person.Age}");
                }

                string line = location + "  读者数量:" + readerCount + "  "+totalCountText+"总量:" + totalCount;

                if (text != "")
                    text += "\r\n";
                text += line;
            }

            MessageBox.Show(this, text);
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControl_table.SelectedTab.Text == "借书统计")
            {
                foreach (ListViewItem item in this.listView_borrowStatis.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListViewItem item in this.listView_returnStatis.Items)
                {
                    item.Selected = true;
                }

            }

        }

        private void listView_borrowByBarcode_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int nClickColumn = e.Column;
            SortCol(this.listView_borrowByBarcode, SortColumns_borrowStatisByBarcode, nClickColumn);
        }
    }

    public class BorrowLogItem
    {
        // 读者条码
        public string readerBarcode { get; set; }
        
        // 册条码
        public string itemBarcode { get; set; }

        // 借书日期
        public string borrowDate { get; set; }

        // 借书期限
        public string borrowPeriod { get; set; }

        // 读者类型
        public string readerType { get; set; }

        // 读者姓名
        public string readerName { get; set; }

        // 读者单位
        public string dept { get; set; }

        // 图书类型
        public string bookType { get; set; }

        // 馆藏地
        public string location { get; set; }

        // 2024/1/3 取出价格
        public string price { get; set; }


    }


    // 借书统计
    public class BorrowGroup
    {
        public string location { get; set; }
        public string readerBarcode { get; set; }

        public string readerName { get; set; }

        public string dept { get; set; }

        public List<BorrowLogItem> Items { get; set; }


        public int count{ get; set; }
    }

    public class LocationGroup
    {
        public string location { get; set; }
        public int readerCount { get; set; }
        public int totalCount { get; set; }
    }

}
