﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DigitalPlatform;
using DigitalPlatform.CirculationClient;
//using DigitalPlatform.Forms;
using DigitalPlatform.LibraryRestClient;
using DigitalPlatform.Text;
using DigitalPlatform.Xml;
using Serilog;
//using Microsoft.EntityFrameworkCore;

namespace dp2mini
{
    public partial class MainForm : Form
    {
        // 主要的通道池，用于当前服务器
        public RestChannelPool _channelPool = new RestChannelPool();

        // 当前连接的服务器的图书馆名
        public string LibraryName = "";

        public SettingInfo Setting = new SettingInfo();

        public string Version = "";



        /// <summary>
        /// 窗体构造函数
        /// </summary>
        public MainForm()
        {
            ClientInfo.ProgramName = "dp2mini";
            ClientInfo.MainForm = this;

            InitializeComponent();

            // 按需登录事件
            this._channelPool.BeforeLogin -= _channelPool_BeforeLogin;
            this._channelPool.BeforeLogin += _channelPool_BeforeLogin;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //
            ClientInfo.Initial("dp2mini");

            // 如果存在原来的配置文件，先删除
            string path = ClientInfo.DataDir + "\\1.0.0.0\\user.config";
            if (File.Exists(path) == true)
            {
                ClientInfo.WriteInfoLog("发现存在user.config文件，自动删除。");
                File.Delete(path);
            }
            ClientInfo.WriteInfoLog(ClientInfo.ClientVersion);

            string dir = ClientInfo.UserDir + "\\log\\";
            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Information()
                .WriteTo.File(Path.Combine(dir, "log_.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information("程序启动了。目录=[" + dir + "]");

            string strClientVersion = ClientInfo.ClientVersion; //Assembly.GetAssembly(typeof(Program)).GetName().Version.ToString();
            this.toolStripStatusLabel_version.Text = strClientVersion;


            this.Setting = this.GetSettings();



            // 先弹出登录对话框
            LoginForm dlg = new LoginForm(this);
            if (dlg.ShowDialog(this) == DialogResult.Cancel)
            {
                this.Close();
                return;
            }
            // 在窗口右下角设置当前登录帐户名称
            this.toolStripStatusLabel_account.Text = "登录账户：" + dlg.Username;
            

            // 获取图书馆名称
            string strError = "";
            int nRet = GetLibraryInfo(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
                this.Close();
                return;
            }

            // 把图书馆名称设到标题上
            this.Text += "[" + this.LibraryName + "]";


            // 初始化预约到库参数
            nRet = InitialArrivedDbProperties(out strError);
            if (nRet == -1)
            {
                MessageBox.Show(this, strError);
                this.Close();
                return;
            }

            
        }



        /// <summary>
        /// 窗体关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this._channelPool.Close();
            }
            catch(Exception ex)
            {
                ClientInfo.WriteErrorLog("关闭通道时出错：" + ex.Message);
                return;
            }
        }



        public void SaveSettings(SettingInfo settingInfo)
        {

            //if (settingInfo.IsSavePassword == false)
            //    settingInfo.Password = "";

            // 登录帐号信息
            ClientInfo.Config?.Set("global", "url", settingInfo.Url);
            ClientInfo.Config?.Set("global", "userName", settingInfo.UserName);
            ClientInfo.Config?.Set("global", "password", MainForm.EncryptPassword(settingInfo.Password));
            //ClientInfo.Config?.Set("global", "isSavePassword", settingInfo.IsSavePassword.ToString());

            // 保存到配置文件
            ClientInfo.Finish();

            // 缓存起来
            this.Setting = this.GetSettings();
        }

        public void SaveNotFoundReason(string reasons)
        {
            // 保存到配置文件
            ClientInfo.Config?.Set("global", "notFoundReasons", reasons);
            ClientInfo.Finish();

            //更新内存
            this.Setting.NotFoundReasons = reasons;

        }

        public SettingInfo GetSettings()
        {

            SettingInfo info = new SettingInfo();

            info.Url= ClientInfo.Config.Get("global", "url", "");
            info.UserName = ClientInfo.Config.Get("global", "userName", "");
            info.Password = MainForm.DecryptPasssword( ClientInfo.Config.Get("global", "password", ""));

           //string isSavePassword = ClientInfo.Config.Get("global", "isSavePassword", "");
            //if (string.IsNullOrEmpty(isSavePassword) ==false)
            //     info.IsSavePassword = Convert.ToBoolean(isSavePassword);


            // 未找到原因
            info.NotFoundReasons = ClientInfo.Config.Get("global", "notFoundReasons", "");


            return info;
        }


        /// <summary>
        /// 按需登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _channelPool_BeforeLogin(object sender, BeforeLoginEventArgs e)
        {

            e.LibraryServerUrl = this.GetPurlUrl(this.Setting.Url);
            e.UserName = this.Setting.UserName;
            e.Password = this.Setting.Password;
            e.Parameters = "type=worker,client=dp2mini|" + ClientInfo.ClientVersion;//Program.ClientVersion;
            return;


            /*
            if (e.FirstTry == true)
            {
                e.LibraryServerUrl = this.GetPurlUrl(this.Setting.Url);
                e.UserName = this.Setting.UserName;
                e.Password = this.Setting.Password;
                e.Parameters = "type=worker,client=dp2mini|" + ClientInfo.ClientVersion;//Program.ClientVersion;
                return;
            }



            if (!string.IsNullOrEmpty(e.ErrorInfo))
            {
                MessageBox.Show(this, e.ErrorInfo);

            }


            IWin32Window owner = null;
            if (sender is IWin32Window)
                owner = sender as IWin32Window;
            else
                owner = this;

            LoginForm dlg = null;
            this.Invoke((Action)(() =>
            {
                dlg = SetDefaultAccount(
                    e.LibraryServerUrl,
                    owner);
            }));
            if (dlg == null)
            {
                e.Cancel = true;
                return;
            }

            e.LibraryServerUrl= this.GetPurlUrl(dlg.LibraryUrl);
            e.UserName = dlg.Username;
            e.Password = dlg.Password;
            e.Parameters = "type=worker,client=dp2mini|" + ClientInfo.ClientVersion;//Program.ClientVersion;
        */
        }

        
        /*
        /// <summary>
        /// 设置缺省帐号
        /// </summary>
        /// <param name="strServerUrl"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        LoginForm SetDefaultAccount(string strServerUrl,
            IWin32Window owner)
        {
            if (owner == null)
                owner = this;

            LoginForm loginForm = new LoginForm(this);
            //if (String.IsNullOrEmpty(strServerUrl))
            //    loginForm.LibraryUrl = Properties.Settings.Default.cfg_library_url;
            //else
            //    loginForm.LibraryUrl = strServerUrl;

            //loginForm.Username = Properties.Settings.Default.cfg_library_username;
            //loginForm.IsSavePassword = Properties.Settings.Default.cfg_savePassword;
            //if (loginForm.IsSavePassword)
            //{
            //    loginForm.Password = Properties.Settings.Default.cfg_library_password;
            //}
            //else
            //{
            //    loginForm.Password = "";
            //}

            loginForm.ShowDialog(owner);
            if (loginForm.DialogResult == DialogResult.Cancel)
                return null;

            return loginForm;
        }
        */

        #region 创建和释放通道

        public string GetPurlUrl(string restUrl)
        {
            string url = restUrl;
            if (restUrl.Length >= 5 && restUrl.Substring(0, 5).ToLower() == "rest.")
            {
                url = restUrl.Substring(5);
            }

            return url;
        }

        /// <summary>
        /// 获取通道
        /// </summary>
        /// <param name="strServerUrl"></param>
        /// <param name="strUserName"></param>
        /// <returns></returns>
        public RestChannel GetChannel(string strServerUrl = ".",
            string strUserName = ".")
        {
            if (strServerUrl == ".")
                strServerUrl = this.Setting.Url;//Properties.Settings.Default.cfg_library_url;

            strServerUrl = this.GetPurlUrl(strServerUrl);

            if (strUserName == ".")
                strUserName = this.Setting.UserName;//Properties.Settings.Default.cfg_library_username;

            RestChannel channel = this._channelPool.GetChannel(strServerUrl, strUserName);
            return channel;
        }

        /// <summary>
        /// 释放通道
        /// </summary>
        /// <param name="channel"></param>
        public void ReturnChannel(RestChannel channel)
        {
            this._channelPool.ReturnChannel(channel);
        }
        #endregion


        /// <summary>
        /// 获得图书馆名称
        /// </summary>
        /// <returns>
        /// -1 出错
        /// 0 成功
        /// </returns>
        public int GetLibraryInfo(out string strError)
        {
            strError = "";

            RestChannel channel = this.GetChannel();
            try
            {
                GetSystemParameterResponse response = channel.GetSystemParameter("library", "name");

                long lRet = response.GetSystemParameterResult.Value;
                strError = response.GetSystemParameterResult.ErrorInfo;
                if (lRet == -1)
                {
                    strError = "针对服务器 " + channel.Url + " 获得图书馆名称发生错误：" + strError;
                    return -1;
                }

                this.LibraryName = response.strValue;
            }
            finally
            {
                this.ReturnChannel(channel);
            }

            return 0;
        }

        /// <summary>
        /// 预约数据库名称
        /// </summary>
        public string ArrivedDbName
        {
            get;
            private set;
        }

        public string ReserveTimeSpan { get; set; }

        /// <summary>
        /// 获取预约到书数据库名称
        /// </summary>
        /// <param name="strError"></param>
        /// <returns></returns>
        public int InitialArrivedDbProperties(out string strError)
        {
             strError = "";

            RestChannel channel = this.GetChannel();
            try
            {
                GetSystemParameterResponse response = channel.GetSystemParameter("arrived",
                    "dbname");

                long lRet = response.GetSystemParameterResult.Value;
                strError = response.GetSystemParameterResult.ErrorInfo;
                if (lRet == -1)
                {
                    strError = "针对服务器 " + channel.Url + " 获得预约到书库名过程发生错误：" + strError;
                    return -1;
                }

                this.ArrivedDbName = response.strValue;

                // 2021/2/20 增加
                // 获取预约保留天数
                //    <arrived dbname="预约到书" reserveTimeSpan="1day" outofReservationThreshold="10" 
                //  canReserveOnshelf="true" notifyTypes="dpmail,email,mq" />
                response = channel.GetSystemParameter("system",
                    "arrived");
                lRet = response.GetSystemParameterResult.Value;
                strError = response.GetSystemParameterResult.ErrorInfo;
                if (lRet == -1)
                {
                    strError = "针对服务器 " + channel.Url + " 获得预约参数发生错误：" + strError;
                    return -1;
                }
                string arrivedXml = response.strValue;
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(arrivedXml);
                if (dom.DocumentElement !=null)
                    this.ReserveTimeSpan = DomUtil.GetAttr(dom.DocumentElement, "reserveTimeSpan");


                    //DomUtil.GetElementAttr(dom.DocumentElement, "", "reserveTimeSpan"); 



            }
            finally
            {
                this.ReturnChannel(channel);
            }

            return 0;
        }


        /// <summary>
        /// 设置状态栏参数
        /// </summary>
        /// <param name="text"></param>
        public void SetStatusMessage(string text)
        {
            this.toolStripStatusLabel_message.Text = text;
        }

        /// <summary>
        /// 预约到书查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem_prep_Click(object sender, EventArgs e)
        {
            EnsureChildForm<PrepForm>(true);

            /*
            PrepForm prepForm = new PrepForm()
            {
                MdiParent = this,
                Text = "查询预约到书"
            };
            prepForm.MdiParent = this;
            prepForm.AutoSize = true;
            prepForm.WindowState = FormWindowState.Maximized;
            prepForm.Show();
            */
        }

        // 登录参数设置
        private void toolStripMenuItem_setting_Click(object sender, EventArgs e)
        {
            SettingForm dlg = new SettingForm(this);
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);
            // 不需要判断确认还是取消
        }

        /// <summary>
        /// 预约到书查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_prep_Click(object sender, EventArgs e)
        {
            this.toolStripMenuItem_prep_Click(sender, e);
        }

        /// <summary>
        /// 备书单管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripLabel_noteManager_Click(object sender, EventArgs e)
        {
            EnsureChildForm<NoteForm>(true);

            /*
            NoteForm prepForm = new NoteForm()
            {
                MdiParent = this,
                Text = "备书单管理"
            };
            prepForm.MdiParent = this;
            prepForm.AutoSize = true;
            prepForm.WindowState = FormWindowState.Maximized;
            prepForm.Show();
            */
        }

        private void 备书单管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnsureChildForm<NoteForm>(true);

        }


        /// <summary>
        /// 测试打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_test_Click(object sender, EventArgs e)
        {
            string strError = "";

            string line = null;
            StringBuilder sb = new StringBuilder();

            string printFile = Application.StartupPath + "//print.txt";
            if (File.Exists(printFile) == false)
            {
                MessageBox.Show(this, "打印不存在");
                return;
            }

            using (StreamReader reader = new StreamReader("print.txt", Encoding.UTF8))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    sb.Append("<p>").Append(line).Append("</p>").AppendLine();
                }
            }

            using (StreamWriter writer = new StreamWriter("print.xml", false, Encoding.UTF8))
            {
                writer.Write(WrapString(sb.ToString()));
            }

            CardPrintForm form = new CardPrintForm();
            form.PrinterInfo = new PrinterInfo();
            form.CardFilename = "print.xml";  // 卡片文件名

            form.WindowState = FormWindowState.Minimized;
            form.Show();
            int nRet = form.PrintFromCardFile(false,out strError);
            if (nRet == -1)
            {
                form.WindowState = FormWindowState.Normal;
                strError = strError + "\r\n\r\n以下内容未能成功打印:\r\n" + sb.ToString();
                goto ERROR1;
            }
            form.Close();
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }

        #region 静态函数

        static string WrapString(string strText)
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
        #endregion

        #region 只打开一个MDI子窗口

        /// <summary>
        /// 获得一个已经打开的 MDI 子窗口，如果没有，则新打开一个
        /// </summary>
        /// <typeparam name="T">子窗口类型</typeparam>
        /// <returns>子窗口对象</returns>
        public T EnsureChildForm<T>(bool bActivate = false)
        {
            T form = GetTopChildWindow<T>();
            if (form == null)
            {
                form = Activator.CreateInstance<T>();
                dynamic o = form;
                o.MdiParent = this;
                if (form.GetType() == typeof(chargingAnalysisForm))
                {
                    o.WindowState = FormWindowState.Maximized;
                }

                {
                    try
                    {
                        // 2017/4/25
                        if (o.MainForm == null)
                            o.MainForm = this;
                    }
                    catch
                    {
                        // 等将来所有窗口类型的 MainForm 都是只读的以后，再修改这里
                    }
                }
                o.Show();
            }
            else
            {
                if (bActivate == true)
                {
                    try
                    {
                        dynamic o = form;
                        o.Activate();

                        if (o.WindowState == FormWindowState.Minimized)
                            o.WindowState = FormWindowState.Normal;
                    }
                    catch
                    {
                    }
                }
            }
            return form;
        }


        // 
        /// <summary>
        /// 得到特定类型的顶层 MDI 子窗口
        /// 注：不算 Fixed 窗口
        /// </summary>
        /// <typeparam name="T">子窗口类型</typeparam>
        /// <returns>子窗口对象</returns>
        public T GetTopChildWindow<T>()
        {
            if (ActiveMdiChild == null)
                return default(T);

            // 得到顶层的MDI Child
            IntPtr hwnd = this.ActiveMdiChild.Handle;

            if (hwnd == IntPtr.Zero)
                return default(T);

            while (hwnd != IntPtr.Zero)
            {
                Form child = null;
                // 判断一个窗口句柄，是否为 MDI 子窗口？
                // return:
                //      null    不是 MDI 子窗口o
                //      其他      返回这个句柄对应的 Form 对象
                child = IsChildHwnd(hwnd);
                if (child != null )//&& IsFixedMyForm(child) == false)  // 2016/12/16 跳过固定于左侧的 MyForm
                {
                    // if (child is T)
                    if (child.GetType().Equals(typeof(T)) == true)
                    {
                        try
                        {
                            return (T)Convert.ChangeType(child, typeof(T));
                        }
                        catch (InvalidCastException ex)
                        {
                            throw new InvalidCastException("在将类型 '" + child.GetType().ToString() + "' 转换为类型 '" + typeof(T).ToString() + "' 的过程中出现异常: " + ex.Message, ex);
                        }
                    }
                }

                hwnd = API.GetWindow(hwnd, API.GW_HWNDNEXT);
            }

            return default(T);
        }

        // 判断一个窗口句柄，是否为 MDI 子窗口？
        // 注：不处理 Visible == false 的窗口。因为取 Handle 会导致 Visible 变成 true
        // return:
        //      null    不是 MDI 子窗口o
        //      其他      返回这个句柄对应的 Form 对象
        Form IsChildHwnd(IntPtr hwnd)
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child.Visible == true && hwnd == child.Handle)
                    return child;
            }

            return null;
        }

        #endregion

        #region 打开各种文件夹

        private void UToolStripMenuItem_openUserFolder_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ClientInfo.UserDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ExceptionUtil.GetAutoText(ex));
            }
        }

        private void ToolStripMenuItem_openDataFolder_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ClientInfo.DataDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ExceptionUtil.GetAutoText(ex));
            }
        }

        private void ToolStripMenuItem_openProgramFolder_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ExceptionUtil.GetAutoText(ex));
            }
        }

        #endregion


        #region 密码加密
        static string EncryptKey = "dp2mini_key";
        internal static string DecryptPasssword(string strEncryptedText)
        {
            if (String.IsNullOrEmpty(strEncryptedText) == false)
            {
                try
                {
                    string strPassword = Cryptography.Decrypt(
        strEncryptedText,
        EncryptKey);
                    return strPassword;
                }
                catch
                {
                    return "errorpassword";
                }
            }

            return "";
        }
        internal static string EncryptPassword(string strPlainText)
        {
            return Cryptography.Encrypt(strPlainText, EncryptKey);
        }

        #endregion


        private void toolStripButton_note_Click(object sender, EventArgs e)
        {
            EnsureChildForm<NoteForm>(true);
        }

        private void toolStripLabel_transferStatis_Click(object sender, EventArgs e)
        {
            EnsureChildForm<TransferStatisForm>(true);
        }

        private void toolStripLabel_marc_Click(object sender, EventArgs e)
        {
            EnsureChildForm<MarcForm>(true);
        }

        private void 巡检ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnsureChildForm<toolForm>(true);
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton_chargingAnalysis_Click(object sender, EventArgs e)
        {
            EnsureChildForm<chargingAnalysisForm>(true);
        }


        // 权限工具
        private void 生成权限工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RightsForm dlg = new RightsForm(this);
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog(this);
        }
    }

}
