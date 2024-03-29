﻿using DigitalPlatform.CirculationClient;
using DigitalPlatform.IO;
using DigitalPlatform.LibraryRestClient;
using DigitalPlatform.Xml;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace dp2mini
{
    public partial class NoteForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NoteForm()
        {
            InitializeComponent();

            // 接管增加Note事件
            DbManager.Instance.AddNoteHandler -= new AddNoteDelegate(this.AddNoteToListView);
            DbManager.Instance.AddNoteHandler += new AddNoteDelegate(this.AddNoteToListView);
        }

        // 名字以用途命名即可。TokenSource 这种类型名称可以不出现在名字中
        CancellationTokenSource _cancel = new CancellationTokenSource();

        // mid父窗口
        MainForm _mainForm = null;
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteForm_Load(object sender, EventArgs e)
        {
            this._mainForm = this.MdiParent as MainForm;

            // 每次开头都重新 new 一个。这样避免受到上次遗留的 _cancel 对象的状态影响
            this._cancel.Dispose();
            this._cancel = new CancellationTokenSource();
            // 开一个新线程
            Task.Run(() =>
            {
                LoadNotes(this._cancel.Token);
            });
        }

        private void NoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 取消Note变化事件
            DbManager.Instance.AddNoteHandler -= new AddNoteDelegate(this.AddNoteToListView);

        }

        /// <summary>
        /// 处理外部增加备书单的事件
        /// </summary>
        /// <param name="note"></param>
        public void AddNoteToListView(Note note)
        {
            string noteId = DbManager.NumToString(note.Id);
            ListViewItem viewItem = new ListViewItem(noteId, 0);

            this.listView_note.Items.Insert(0, viewItem);
            this.LoadOneNote(note, viewItem);
        }

        /// <summary>
        /// 装载备书单
        /// </summary>
        /// <param name="token"></param>
        public void LoadNotes(CancellationToken token)
        {
            //把本地库的备书单显示在列表中
            List<Note> notes = DbManager.Instance.GetNotes();
            foreach (Note note in notes)
            {
                if (token.IsCancellationRequested == true)
                    break;

                string noteId = DbManager.NumToString(note.Id);
                ListViewItem viewItem = new ListViewItem(noteId, 0);
                this.Invoke((Action)(() =>
                {
                    this.listView_note.Items.Add(viewItem);
                    this.LoadOneNote(note, viewItem);
                }
                ));
            }
        }

        public void LoadOneNote(Note note,ListViewItem viewItem)
        {
            viewItem.SubItems.Clear();
            /*
                    单号
                    当前进度
                    读者
                    包含的预约记录
                    创建日期

                    */
            string noteId = DbManager.NumToString(note.Id);
            viewItem.Text = noteId;
            viewItem.SubItems.Add(Note.GetStepCaption( note.Step));
            viewItem.SubItems.Add(note.PatronName);
            viewItem.SubItems.Add(note.PatronTel);
            viewItem.SubItems.Add(note.Items);
            viewItem.SubItems.Add(note.CreateTime);

            /*
            打印
            备书
            通知
            取书
             */
            viewItem.SubItems.Add(GetStepStateText(note.PrintState)); //note.PrintState);
            viewItem.SubItems.Add(note.PrintTime);
            viewItem.SubItems.Add(GetStepStateText(note.CheckResult));// note.CheckResult);
            viewItem.SubItems.Add(note.CheckedTime);
            viewItem.SubItems.Add(GetStepStateText(note.NoticeState));//note.NoticeState);
            viewItem.SubItems.Add(note.NoticeTime);
            viewItem.SubItems.Add(GetStepStateText(note.TakeoffState));//note.TakeoffState);
            viewItem.SubItems.Add(note.TakeoffTime);
        }




        private void listView_note_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 先清空下方界面上次备书单的信息
            this.SetOperateButton("");
            this.listView_items.Items.Clear();

            if (this.listView_note.SelectedItems.Count == 0)
                return;


            // 选择一行，下方进度按钮变化，并且显示详细信息
            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;
            Note note = DbManager.Instance.GetNote(noteId);

            // 设置这个备书单的操作按钮
            SetOperateButton(note.Step);

            // 显示详细信息
            this.ShowDetail(noteId);
        }

        public void ShowDetail(string noteId)
        {
            this.listView_items.Items.Clear();
            List<ReservationItem> items = DbManager.Instance.GetItemsByNoteId(noteId);
            foreach (ReservationItem item in items)
            {
                this.AppendNewLine(this.listView_items, item);
            }
        }

        /// <summary>
        /// 在 ListView 最后追加一行
        /// </summary>
        /// <param name="list">ListView 对象</param>
        /// <param name="strID">左边第一列内容</param>
        /// <param name="others">其余列内容</param>
        /// <returns>新创建的 ListViewItem 对象</returns>
        public  ListViewItem AppendNewLine(ListView list,
            ReservationItem resItem)
        {
            ListViewItem viewItem = new ListViewItem(resItem.RecPath, 0);
            list.Items.Add(viewItem);

            /*
            路径
            备书结果
            读者证条码
            读者姓名
            */
            viewItem.SubItems.Add(this.GetCheckResultText(resItem.CheckResult));
            viewItem.SubItems.Add(resItem.NotFoundReason);
            viewItem.SubItems.Add(resItem.PatronBarcode);
            viewItem.SubItems.Add(resItem.PatronName);
            /*
            册条码
            书名
            索取号
            馆藏地点
            ISBN
            作者
            */
            viewItem.SubItems.Add(resItem.ItemBarcode);
            viewItem.SubItems.Add(resItem.Title);
            viewItem.SubItems.Add(resItem.AccessNo);
            viewItem.SubItems.Add(resItem.Location);
            viewItem.SubItems.Add(resItem.ISBN);
            viewItem.SubItems.Add(resItem.Author);
            /*
            读者电话
            读者部门
            预约时间
            到书时间
            预约状态
             */
            viewItem.SubItems.Add(resItem.PatronTel);
            viewItem.SubItems.Add(resItem.Department);
            viewItem.SubItems.Add(resItem.RequestTime);
            viewItem.SubItems.Add(resItem.ArrivedTime);
            viewItem.SubItems.Add(resItem.State);



            return viewItem;
        }

        #region 操作按钮状态

        private void FinishButton(Button btn)
        {
            btn.Enabled = false;
            btn.BackColor = Color.Green;
        }

        private void TodoButton(Button btn)
        {
            btn.Enabled = true;
            btn.BackColor = Color.Yellow;
        }

        private void DisableButton(Button btn)
        {
            btn.Enabled = false;
            btn.BackColor = Button.DefaultBackColor;
        }

        public void SetOperateButton(string step)
        {
            //create/print/check/notice/takeoff
            this.DisableButton(this.button_create);
            this.DisableButton(this.button_print);
            this.DisableButton(this.button_check);
            this.DisableButton(this.button_notice);
            this.DisableButton(this.button_takeoff);


            if (step == Note.C_Step_Create)
            {
                this.FinishButton(this.button_create);
                this.TodoButton(this.button_print);
            }
            else if (step == Note.C_Step_Print)
            {
                this.FinishButton(this.button_create);
                this.FinishButton(this.button_print);
                this.TodoButton(this.button_check);
            }
            else if (step == Note.C_Step_Check)
            {
                this.FinishButton(this.button_create);
                this.FinishButton(this.button_print);
                this.FinishButton(this.button_check);
                this.TodoButton(this.button_notice);
            }
            else if (step == Note.C_Step_Notice)
            {
                this.FinishButton(this.button_create);
                this.FinishButton(this.button_print);
                this.FinishButton(this.button_check);
                this.FinishButton(this.button_notice);
                this.TodoButton(this.button_takeoff);
            }
            else if (step == Note.C_Step_Takeoff)
            {
                this.FinishButton(this.button_create);
                this.FinishButton(this.button_print);
                this.FinishButton(this.button_check);
                this.FinishButton(this.button_notice);
                this.FinishButton(this.button_takeoff);
            }

            // 打印小票永远可以点击打印
            if (this.listView_note.SelectedItems.Count != 0)  // 2021/3/2改，如果一行也没有选中，打印按钮不要为可用状态。
            {
                this.button_print.Enabled = true;
            }
        }

        #endregion

        private void button_print_Click(object sender, EventArgs e)
        {
            if (this.listView_note.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "尚未选择备书单");
                return;
            }

            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;
            Note note = DbManager.Instance.GetNote(noteId);
            if (note == null)
            { 
                MessageBox.Show(this,"未找到单号为'" + noteId + "'的记录。");
                return;
            }

            // 2021/3/2加，检查处理过程中有没有变化
            // 检查备书单中册的到书状态在服务器端有无变成outof的？如果变为outof可能是读者放弃取书或者超过的保留期
            bool boutof = this.GetRecordState(noteId, out string info);
            if (boutof == true)
            {
                MessageBox.Show(this, info);
                return;
            }




            // 因为打印小票可以反复打印，所以只有当步骤是创建备书票时，才需要修改订票的状态
            if (note.Step == Note.C_Step_Create)
            {
                string printTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                note.PrintTime = printTime;
            }

            // 实际打印
            string strError = "";
            int nRet = this.Print(note,out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
                return;
            }
            if (nRet == 0)
            {
                MessageBox.Show(this, "您取消了打印");
                return;
            }

            // 因为打印小票可以反复打印，所以只有当步骤是创建备书票时，才需要修改订票的状态
            if (note.Step == Note.C_Step_Create )
            {
                note.PrintState = "Y";
                note.Step = Note.C_Step_Print;

                // 更新本地数据库备书库打印状态和时间
                DbManager.Instance.UpdateNote(note);

                // 更新备书行的显示
                this.LoadOneNote(note, viewItem);
                this.SetOperateButton(note.Step);

            }
        }



        private string GetStepStateText(string stepState)
        {
            if (stepState == "Y")
                return "完成";

            return stepState;
        }

        private void 打印小票预览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView_note.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "尚未选择备书单");
                return;
            }

            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;
            Note note = DbManager.Instance.GetNote(noteId);
            if (note == null)
            {
                MessageBox.Show(this, "未找到单号为'" + noteId + "'的记录。");
                return;
            }

            string printTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            note.PrintTime = printTime;
            PrintPreview(note);
        }

        private void 输出小票信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView_note.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "尚未选择备书单");
                return;
            }

            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;
            Note note = DbManager.Instance.GetNote(noteId);

            string[] reasons = this._mainForm.GetSettings().ReasonArray;
            string reasonText = "";
            foreach (string r in reasons)
            {
                if (reasonText != "")
                    reasonText += "\r\n";

                reasonText += "□ " + r;
            }
            if (reasonText != "")
            {
                reasonText = "未找到原因：\r\n" + reasonText;
            }

            StringBuilder sb = new StringBuilder();
            // 备书单整体信息
            sb.AppendLine("备书单号：" + noteId ); //备书单id
            sb.AppendLine(note.PatronName ); //读者姓名
            sb.AppendLine(note.PatronTel ); //读者电话
            sb.AppendLine( note.PrintTime); //打印时间
            sb.AppendLine("================="); //打印时间

            // 预约记录详细信息
            List<ReservationItem> items = DbManager.Instance.GetItemsByNoteId(noteId);
            foreach (ReservationItem item in items)
            {
                sb.AppendLine( item.ItemBarcode);
                sb.AppendLine(item.Title);
                sb.AppendLine(item.Location);
                sb.AppendLine(item.AccessNo);
                sb.AppendLine( item.ISBN );
                sb.AppendLine(item.Author );
                sb.AppendLine( item.RequestTime);
                sb.AppendLine(reasonText);
                sb.AppendLine("--------------------------");
            }

            textForm dlg = new textForm();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.Info = sb.ToString();
            dlg.ShowDialog(this);
        }

        private void 查看备书结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;
            Note note = DbManager.Instance.GetNote(noteId);

            if (note.Step != Note.C_Step_Check
                && note.Step != Note.C_Step_Notice
                && note.Step != Note.C_Step_Takeoff)
            {
                MessageBox.Show(this, "本单尚未备书完成，备书完成才能查看备书结果信息");
                return;
            }

            string info = this.GetResultInfo(noteId);

            textForm dlg = new textForm();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.Info = info;
            dlg.ShowDialog(this);

        }

        public string GetResultInfo(string noteId)
        {
            Note note = DbManager.Instance.GetNote(noteId);
            StringBuilder sb = new StringBuilder();
            // 备书单整体信息
            sb.AppendLine("备书单号：" + noteId); //备书单id
            sb.AppendLine(note.PatronName); //读者姓名
            sb.AppendLine(note.PatronTel); //读者电话
            sb.AppendLine("备书完成时间：" + note.CheckedTime);
            sb.AppendLine("================="); //打印时间

            List<ReservationItem> items = DbManager.Instance.GetItemsByNoteId(noteId);
            foreach (ReservationItem item in items)
            {
                sb.AppendLine(item.ItemBarcode + " " + item.Title);
                sb.AppendLine(GetCheckResultText(item.CheckResult));
                if (item.CheckResult=="N")
                    sb.AppendLine("原因："+item.NotFoundReason);
                sb.AppendLine("----------------------------"); //打印时间
            }
            return sb.ToString();
        }

        public string GetCheckResultText(string checkResult)
        {
            if (checkResult.ToUpper() == "Y")
                return "满足";
            else if (checkResult.ToUpper() == "N")
                return "不满足";
            return checkResult;
        }

        #region 打印功能

        // 打印文件
        private string _printFilename = "print.xml";

        /// <summary>
        /// 输出打印文件
        /// </summary>
        /// <param name="noteId"></param>
        void OutputPrintFile(Note note)
        {
            using (StreamWriter writer = new StreamWriter(this._printFilename,
                false, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder(1024);// 256);

                string[] reasons = this._mainForm.GetSettings().ReasonArray;
                string reasonText = "";
                foreach (string r in reasons)
                {
                    if (reasonText != "")
                        reasonText += "<br/>";

                    reasonText += "<font size='10'>□</font> " + r;
                }
                if (reasonText !="")
                {
                    reasonText = "<p>图书未找到原因：<br/>" + reasonText+"</p>";
                }


                // 备书单整体信息
                string noteId = DbManager.NumToString(note.Id);
                sb.AppendLine("<p>备书单号："+noteId+"</p>"); //备书单id
                sb.AppendLine("<p><b><font size='10'>"+note.PatronName+"</font></b></p>"); //读者姓名
                sb.AppendLine("<p><font size='10'>"+note.PatronTel+"</font></p>"); //读者电话
                sb.AppendLine("<p>"+note.PrintTime+"</p>"); //打印时间
                sb.AppendLine("<p>=================</p>"); //打印时间

                // 预约记录详细信息
                List<ReservationItem> items = DbManager.Instance.GetItemsByNoteId(noteId);
                foreach (ReservationItem item in items)
                {
                    sb.AppendLine("<p><font size='10'>"+item.ItemBarcode+"</font></p>");
                    sb.AppendLine("<p>"+item.Title+"</p>");
                    sb.AppendLine("<p><font size='10'>"+item.Location+"</font></p>");
                    sb.AppendLine("<p><font size='10'>"+item.AccessNo+"</font></p>");
                    sb.AppendLine("<p>"+item.ISBN+"</p>");
                    sb.AppendLine("<p>"+item.Author+"</p>");
                    sb.AppendLine("<p>预约时间：" + item.RequestTime + "</p>");
                    sb.AppendLine(reasonText);
                    sb.AppendLine("<p>--------------------------</p>");
                }

                // 加打印内容加上格式
                string wrapText = NoteForm.WrapString(sb.ToString());

                // 写到打印文件
                writer.Write(wrapText);
            }
        }

        /// <summary>
        /// 包装打印字符串
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string WrapString(string strText)
        {
            string strPrefix = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n"
                + "<root>\r\n"
                + "<pageSetting width='190'>\r\n"
                + "  <font name=\"微软雅黑\" size=\"8\" style=\"\" />\r\n"
                + "  <p align=\"left\" indent='-60'/>\r\n"
                + "</pageSetting>\\\r\n"
                + "<document padding=\"0,0,0,0\">\r\n"
                + "  <column width=\"auto\" padding='60,0,0,0'>\r\n";

            string strPostfix = "</column></document></root>";

            return strPrefix + strText + strPostfix;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="noteId"></param>
        /// -1 出错
        /// 0 取消打印
        /// 1 成功
        private int Print(Note note,out string strError)
        {
            strError = "";

            // 鼠标设为等待状态
            Cursor oldCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            // 输出打印文件
            this.OutputPrintFile(note);

            CardPrintForm form = new CardPrintForm();
            form.PrinterInfo = new PrinterInfo();
            form.CardFilename = this._printFilename;  // 卡片文件名
            form.ShowInTaskbar = false;
            form.WindowState = FormWindowState.Minimized;
            form.Show();  // 必须这样写 2020/2/21 增加备注
            try
            {
                return form.PrintFromCardFile(false,
                    out strError);
            }
            finally
            {
                form.Close();
                this.Cursor = oldCursor;
            }
        }

        /// <summary>
        /// 打印预约
        /// </summary>
        /// <param name="noteId"></param>
        private void PrintPreview(Note note)
        {
            Cursor oldCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            string printTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.OutputPrintFile(note);
            CardPrintForm dlg = new CardPrintForm();
            dlg.CardFilename = this._printFilename;  // 卡片文件名
            dlg.PrintPreviewFromCardFile();

            this.Cursor = oldCursor;
        }




        #endregion

        private void contextMenuStrip_note_Opening(object sender, CancelEventArgs e)
        {
            if (this.listView_note.SelectedItems.Count <= 0)
            {
                this.打印小票预览ToolStripMenuItem.Enabled = false;
                this.输出小票信息ToolStripMenuItem.Enabled = false;
                this.查看备书结果ToolStripMenuItem.Enabled = false;
                this.撤消备书单ToolStripMenuItem.Enabled = false;
            }
            else
            {
                this.打印小票预览ToolStripMenuItem.Enabled = true;
                this.输出小票信息ToolStripMenuItem.Enabled = true;
                this.查看备书结果ToolStripMenuItem.Enabled = true;

                ListViewItem viewItem = this.listView_note.SelectedItems[0];
                Note note = DbManager.Instance.GetNote(viewItem.SubItems[0].Text);
                if (note != null && note.Step != Note.C_Step_Takeoff)
                {
                    this.撤消备书单ToolStripMenuItem.Enabled = true;
                }
                else
                {
                    this.撤消备书单ToolStripMenuItem.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 准备图书完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_check_Click(object sender, EventArgs e)
        {
            if (this.listView_note.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "尚未选择备书单");
                return;
            }

            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;

            // 2021/3/2加，检查处理过程中有没有变化
            // 检查备书单中册的到书状态在服务器端有无变成outof的？如果变为outof可能是读者放弃取书或者超过的保留期
            bool boutof = this.GetRecordState(noteId, out string info);
            if (boutof == true)
            {
                MessageBox.Show(this, info);
                return;
            }


            checkForm dlg = new checkForm(this._mainForm);
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.NoteId = noteId;
            DialogResult ret = dlg.ShowDialog(this);
            if (ret == DialogResult.Cancel)
            {
                // 用户取消操作，则不做什么事情
                return;
            }

            if (ret == DialogResult.OK)
            {
                // 找到的图书
                string foundItems = dlg.FoundItems;
                if (string.IsNullOrEmpty(foundItems) == false)
                {
                    string[] paths = foundItems.Split(new char[] {','});
                    foreach (string path in paths)
                    {
                        // 更新数据库预约记录的备书结果
                        ReservationItem item = DbManager.Instance.GetItem(path);
                        item.CheckResult = "Y";
                        DbManager.Instance.UpdateItem(item);
                    }
                }

                // 未找到的图书
                string notfoundItems = dlg.NotFoundItems;
                if (string.IsNullOrEmpty(notfoundItems) == false)
                {
                    string[] paths = notfoundItems.Split(new char[] { ',' });
                    foreach (string path in paths)
                    {
                        // 更新数据库预约记录的备书结果
                        ReservationItem item = DbManager.Instance.GetItem(path);
                        item.CheckResult = "N";

                        string strReason = (string)dlg.NotFoundReasonHt[path];
                        item.NotFoundReason = strReason;

                        DbManager.Instance.UpdateItem(item);
                    }
                }

                string checkTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Note note = DbManager.Instance.GetNote(noteId);
                note.CheckedTime = checkTime;
                note.CheckResult = "Y";
                note.Step = Note.C_Step_Check;

                // 更新本地数据库备书库打印状态和时间
                DbManager.Instance.UpdateNote(note);

                // 更新备书行的显示
                this.LoadOneNote(note, viewItem);
                this.SetOperateButton(note.Step);

                // 显示详细信息
                this.ShowDetail(noteId);


                //// 从服务器上取消预约，预约记录的状态会从arrived变为outof
                //// 开一个新线程
                //Task.Run(() =>
                //{
                //    DeleteReservation(noteId);
                //});
            }

        }

        public void DeleteReservation(string noteId)
        {

            RestChannel channel = this._mainForm.GetChannel();
            try
            {
                string strError = "";

                List<ReservationItem> items = DbManager.Instance.GetItemsByNoteId(noteId);
                foreach (ReservationItem item in items)
                {
                    //this.AppendNewLine(this.listView_items, item);

                    ReservationResponse response = channel.Reservation("delete",
                        item.PatronBarcode,
                        item.ItemBarcode);
                    if (response.ReservationResult.ErrorCode != ErrorCode.NoError)
                    {
                        strError += response.ReservationResult.ErrorInfo + "\r\n";
                    }

                    // 更新一下本地库的预约记录状态，与服务器保持一致，
                    // 但界面还不会立即反应出来，需要点一下上方的备书单，再显示出来的详细信息就为outof状态了
                    item.State = "outof";
                    DbManager.Instance.UpdateItem(item);
                }


                if (strError != "")
                {
                    // 用Invoke线程安全的方式来调
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show(this, "调服务器取消预约出错:" + strError);
                        return;
                    }
                    ));
                }
            }
            finally
            {

                this._mainForm.ReturnChannel(channel);
            }            
        }

        private void button_notice_Click(object sender, EventArgs e)
        {
            if (this.listView_note.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "尚未选择备书单");
                return;
            }

            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;

            // 2021/3/2加，检查处理过程中有没有变化
            // 检查备书单中册的到书状态在服务器端有无变成outof的？如果变为outof可能是读者放弃取书或者超过的保留期
            bool boutof = this.GetRecordState(noteId, out string checkinfo);
            if (boutof == true)
            {
                MessageBox.Show(this, checkinfo);
                return;
            }

            // 显示电话通知内容
            string info = this.GetResultInfo(noteId);
            noticeForm dlg = new noticeForm();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.Info = info;
            DialogResult ret= dlg.ShowDialog(this);
            if (ret == DialogResult.Cancel)
            {
                // 用户取消操作，则不做什么事情
                return;
            }

            

            // 发送微信通知
            /*
<root>
<type>预约到书通知</type>
<itemBarcode>0000001</itemBarcode>
<onShelf>false</onShelf>
<accessNo>123</accessNo>
<requestDate>Wed, 23 Dec 2020 14:24:10 GMT</requestDate>
<today>2016/5/17 10:10:59</today>
<reserveTime>2天</reserveTime>
<location>星洲/书柜</location>
<summary>数学小报. -- ISBN 7</summary>
<patronName>张三</patronName>
<patronRecord>
  <barcode>13862157150</barcode>
  <name>张三</name>
  <libraryCode></libraryCode>
</patronRecord>
</root>

// http://123.57.163.11/dp2library/demo/rest/SetMessage

{
"strAction":"send",
"strStyle":"",
"messages":[
{
"strRecipient":"!mq:13862157150",
"strMime": "xml",
"strBody": "<root><type>预约到书通知</type><itemBarcode>0000001</itemBarcode>
            <onShelf>false</onShelf><accessNo>I247.5/5</accessNo><requestDate>Wed, 23 Dec 2020 14:24:10 GMT</requestDate><today>2016/5/17 10:10:59</today><reserveTime>2天</reserveTime><location>星洲/书柜</location><summary>数学小报. -- ISBN 7-...</summary><patronName>张三</patronName><patronRecord><barcode>13862157150</barcode><name>张三</name><libraryCode></libraryCode></patronRecord></root>"
}]
}
             */

            // 用于写日志和报错
            string errorPatronName = "";
            string errorItemBarcodes = "";

 

            RestChannel channel = this._mainForm.GetChannel();
            try
            {
                string strError = "";

                int foundCount = 0;
                int notfoundCount = 0;
                string foundInfo = "";
                string notfoundInfo = "";


                List<ReservationItem> items = DbManager.Instance.GetItemsByNoteId(noteId);
                if (items.Count == 0)
                {
                    MessageBox.Show(this, "该备书单没有详细信息");
                    return;
                }

                foreach (ReservationItem item in items)
                { 

                    string requestTime = item.RequestTime;
                    DateTime dt = DateTime.Parse(requestTime);
                    string rfcDate = DateTimeUtil.Rfc1123DateTimeString(dt);  //.Date8toRfc1123(requestTime.Substring(0, 8),out rfcDate,out strError);

                    // 如果是找到的图书，给读者发送预约到书通知
                    if (item.CheckResult == "Y")
                    {
                        // 找到的数量和名称
                        foundCount++;
                        if (foundInfo != "")
                            foundInfo += ",";
                        foundInfo += item.Title + "(" + item.ItemBarcode + ")";



                        MessageData message = new MessageData();
                        message.strRecipient = "!mq:" + item.PatronBarcode;
                        message.strMime = "xml";
                        string strBody = "<root>"
    + "<type>预约到书通知</type>"
    + "<source>dp2mini</source>"
    + "<itemBarcode>" + item.ItemBarcode + "</itemBarcode>"
    + "<onShelf>" + item.OnShelf + "</onShelf>"
    + "<accessNo>" + item.AccessNo + "</accessNo>"
    + "<requestDate>" + rfcDate + "</requestDate>"
    + "<today>" + item.ArrivedTime + "</today>"  //2016/5/17 10:10:59
    + "<reserveTime>" + this._mainForm.ReserveTimeSpan + "</reserveTime>"   //2天
    + "<location>" + item.Location + "</location>"
    + "<summary>" + item.Title + "</summary>"
    + "<patronName>" + item.PatronName + "</patronName>"
    + "<patronRecord>"
    + "<barcode>" + item.PatronBarcode + "</barcode>"
    + "<name>" + item.PatronName + "</name>"
    + "<libraryCode>" + item.LibraryCode + "</libraryCode>"
    + "</patronRecord>"
    + "</root>";
                        XmlDocument dom = new XmlDocument();
                        dom.LoadXml(strBody);
                        message.strBody = dom.DocumentElement.OuterXml;

                        SetMessageResponse response = channel.SetMessage("send",
                            "",
                            message);
                        if (response.SetMessageResult.ErrorCode != ErrorCode.NoError)
                        {
                            // 同一个备书单的读者是同一位
                            errorPatronName = item.PatronName + "(" + item.PatronBarcode + ")";

                            Log.Error("给'" + errorPatronName + "'发送册'" + item.ItemBarcode + "'到书的微信通知出错:" + response.SetMessageResult.ErrorInfo);

                            if (errorItemBarcodes != "")
                                errorItemBarcodes += ",";
                            errorItemBarcodes += item.ItemBarcode;
                        }
                    }
                    else
                    {
                        // 未找到的数量和名称
                        notfoundCount++;
                        if (notfoundInfo != "")
                            notfoundInfo += ",";
                        notfoundInfo += item.Title + "(" + item.ItemBarcode + ")";
                    }
                }

                if (errorItemBarcodes != "")
                {
                    // 用Invoke线程安全的方式来调
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show(this, "给'" + errorPatronName + "'发送册'" + errorItemBarcodes + "'到书的微信通知出错,详细请查看日志");
                    }));
                }


                // 发送找到和未找到总结信息的通知
                {
                    ReservationItem item = items[0];

                    string strcontent = "";
                    if (foundCount > 0)
                        strcontent = "备好" + foundCount.ToString() + "本图书:" + foundInfo+"。";
                    if (notfoundCount > 0)
                        strcontent += "未找到" + notfoundCount.ToString() + "本: " + notfoundInfo + "。";

                    MessageData message = new MessageData();
                    message.strRecipient = "!mq:" +item.PatronBarcode;
                    message.strMime = "xml";
                    string strBody = "<root>"
    + "<type>预约备书结果</type>"
    + "<content>" + strcontent+ "</content>"
    + "<patronName>" + item.PatronName + "</patronName>"
    + "<patronRecord>"
    + "<barcode>" + item.PatronBarcode + "</barcode>"
    + "<name>" + item.PatronName + "</name>"
    + "<libraryCode>" + item.LibraryCode + "</libraryCode>"
    + "</patronRecord>"
    + "</root>";
                    XmlDocument dom = new XmlDocument();
                    dom.LoadXml(strBody);
                    message.strBody = dom.DocumentElement.OuterXml;

                    SetMessageResponse response = channel.SetMessage("send",
                        "",
                        message);
                    if (response.SetMessageResult.ErrorCode != ErrorCode.NoError)
                    {
                        string error = "给'" + item.PatronName + "(" + item.PatronBarcode + ")" + "'发送备书结果微信通知出错:" + response.SetMessageResult.ErrorInfo;
                        //写日志
                        Log.Error(error);

                        // 用Invoke线程安全的方式来调
                        this.Invoke((Action)(() =>
                        {
                            MessageBox.Show(this, error);
                        }));

                    }
                }

            }
            finally
            {
                this._mainForm.ReturnChannel(channel);
            }


            Note note = DbManager.Instance.GetNote(noteId);
            string noticeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            note.NoticeTime = noticeTime;
            note.NoticeState = "Y";
            note.Step = Note.C_Step_Notice;

            // 更新本地数据库备书库打印状态和时间
            DbManager.Instance.UpdateNote(note);

            // 更新备书行的显示
            this.LoadOneNote(note, viewItem);
            this.SetOperateButton(note.Step);
        }

        /// <summary>
        /// 检查备书单中记录当前的预约到书状态，有没有变为outof。
        /// 如果变为outof，有可能是读者放弃取书或者已经超过了保留期限。需撤消备书单，重新处理。
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool GetRecordState(string noteId, out string info)
        {
            info = "";
            bool bOutof = false;
            bool bBorrowed = false;

            string outofBarcodes = "";
            string borrowedBarcodes = "";

            RestChannel channel = this._mainForm.GetChannel();
            try
            {
                List<ReservationItem> items = DbManager.Instance.GetItemsByNoteId(noteId);
                foreach (ReservationItem item in items)
                {

                    GetRecordResponse response = channel.GetRecord(item.RecPath);

                    //记录 '0000000078' 在库中不存在
                    if (response.GetRecordResult.Value == -1)
                    {
                        bBorrowed = true;
                        if (borrowedBarcodes != "")
                            borrowedBarcodes += ",";
                        borrowedBarcodes += "'" + item.ItemBarcode + "'";
                        continue;
                    }


                    string xml = response.strXml;

                    XmlDocument dom = new XmlDocument();
                    dom.LoadXml(xml);
                    string state = DomUtil.GetElementText(dom.DocumentElement, "state");
                    if (state == PrepForm.C_State_outof)
                    {
                        bOutof = true;

                        if (outofBarcodes != "")
                            outofBarcodes += ",";
                        outofBarcodes += "'"+item.ItemBarcode+"'";
                        continue;
                    }
                }

                if (bOutof == true)
                {
                    if (info != "")
                        info += "\r\n";

                    info += "册条码为" + outofBarcodes + "的图书预约到书状态在服务器端已变为outof，原因可能是读者放弃取书或者已经超过了保留期限。";
                }

                if (bBorrowed == true)
                {
                    if (info != "")
                        info += "\r\n";

                    info += "册条码为" + borrowedBarcodes + "的预约到书记录在服务器端不存在，原因是读者已经办理了借书。";
                }

                if (string.IsNullOrEmpty(info) == false)
                    info += "\r\n"+"请撤消备书单，重新处理。";

            }
            finally
            {
                this._mainForm.ReturnChannel(channel);
            }

            if (bOutof == true || bBorrowed == true)
                return true;
            else
                return false;
        }


        private void button_takeoff_Click(object sender, EventArgs e)
        {
            if (this.listView_note.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "尚未选择备书单");
                return;
            }

            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;

            // 2021/3/2加，检查处理过程中有没有变化
            // 检查备书单中册的到书状态在服务器端有无变成outof的？如果变为outof可能是读者放弃取书或者超过的保留期
            bool boutof = this.GetRecordState(noteId, out string checkinfo);
            if (boutof == true)
            {
                MessageBox.Show(this, checkinfo);
                return;
            }

            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            DialogResult dlg = MessageBox.Show(this,"确认关闭备书单吗?", 
                "dp2mini",
                buttons);
            if (dlg == DialogResult.OK)
            {
                Note note = DbManager.Instance.GetNote(noteId);
                string takeoffTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                note.TakeoffTime = takeoffTime;
                note.TakeoffState = "Y";
                note.Step = Note.C_Step_Takeoff;

                // 更新本地数据库备书库打印状态和时间
                DbManager.Instance.UpdateNote(note);

                // 更新备书行的显示
                this.LoadOneNote(note, viewItem);
                this.SetOperateButton(note.Step);

                // 2020/2/22 改在最后一步从服务器取消预约，因为做了这一步会修改服务上预约到书记录的状态为outof，
                // 从服务器上取消预约，预约记录的状态会从arrived变为outof
                // 开一个新线程
                Task.Run(() =>
                {
                    DeleteReservation(noteId);
                });

                //
                //viewItem.BackColor = Color.LightGray;
            }
        }

        private void 撤消备书单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView_note.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "尚未选择备书单");
                return;
            }

            ListViewItem viewItem = this.listView_note.SelectedItems[0];
            string noteId = viewItem.SubItems[0].Text;
            Note note = DbManager.Instance.GetNote(noteId);
            if (note.Step == Note.C_Step_Takeoff)
            {
                MessageBox.Show(this, "此备书单已结束，不能撤消。");
                return;
            }

            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            DialogResult dlg = MessageBox.Show(this, "您确定要撤消备书单吗？",
                "dp2mini",
                buttons,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button2);
            if (dlg == DialogResult.OK)
            {
                // 将下级item的删除
                List<ReservationItem> items = DbManager.Instance.GetItemsByNoteId(noteId);
                foreach (ReservationItem item in items)
                {
                    DbManager.Instance.RemoveItem(item);
                }

                // 从本地备书表中删除
                DbManager.Instance.RemoveNote(noteId);

                // 从界面删除
                this.listView_note.Items.Remove(viewItem);


            }

            
                
        }
    }
}
