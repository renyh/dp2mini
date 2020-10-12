using System;
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
using DigitalPlatform.Forms;
using DigitalPlatform.LibraryRestClient;
using System.Collections.Generic;
using DigitalPlatform.IO;
using DigitalPlatform;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using DigitalPlatform.LibraryClient;

namespace dp2mini
{
    public partial class TransferStatisForm : Form
    {
        // mid父窗口
        MainForm _mainForm = null;

        // 名字以用途命名即可。TokenSource 这种类型名称可以不出现在名字中
        CancellationTokenSource _cancel = new CancellationTokenSource();

        public const string C_State_outof = "outof";
        public const string C_State_arrived = "arrived";

        public Hashtable ItemHt = new Hashtable();

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

            // 批次号
            string batchNo = this.textBox_batchNo.Text;



            // 每次开头都重新 new 一个。这样避免受到上次遗留的 _cancel 对象的状态影响
            this._cancel.Dispose();
            this._cancel = new CancellationTokenSource();

            // 开一个新线程
            Task.Run(() =>
            {
                doSearch(this._cancel.Token,
                    startDate,
                    endDate,
                    batchNo);
            });
        }

        /// <summary>
        /// 检索做事的函数
        /// </summary>
        /// <param name="token"></param>
        private void doSearch(CancellationToken token,
            string startDate,
            string endDate,
            string batchNo)
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
                    true,  // 是否包含扩展名 ".log"
                    out List<string> dates,
                    out string strWarning,
                    out  strError);
                if (nRet == -1)
                {
                    goto ERROR1;
                }

                for (int i = 0; i < 5; i++)
                {
                    this.Invoke((Action)(() =>
                {

                    this.textBox1.Text += i.ToString() + "\r\n";

                //// 任务栏显示信息
                //this._mainForm.SetStatusMessage("命中总数'" + lTotalCount + "'条，"
                //    + "其中预约到书'" + todoCount + "'条，"
                //    + "超过保留期或读者自己取消'" + outofCount + "'条，"
                //    + "已创建到备书单'" + inNoteCount + "'条。");
            }));

                }



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
                    //this._mainForm.ReturnDp2Channel(channel);
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

        /// <summary>
        /// 清空界面数据
        /// </summary>
        public void ClearInfo()
        {
            this.textBox1.Text = "";
            // 清空listview
            //this.listView_results.Items.Clear();
            //this.listView_outof.Items.Clear();

            ////设置父窗口状态栏参数
            //this._mainForm.SetStatusMessage("");
        }

        /// <summary>
        /// 设置控件是否可用
        /// </summary>
        /// <param name="bEnable"></param>
        void EnableControls(bool bEnable)
        {
            this.button_search.Enabled = bEnable;
            this.button_stop.Enabled = !(bEnable);
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




    }



}
