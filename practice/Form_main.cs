﻿using common;
using DigitalPlatform;
using DigitalPlatform.LibraryRestClient;
using DigitalPlatform.Marc;
using practice.test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace practice
{
    public partial class Form_main : Form
    {
        // 通道池
        RestChannelPool _channelPool = new RestChannelPool();

        public Form_main()
        {
            InitializeComponent();
        }

        public string ServerUrl
        {
            get
            {
                return this.Server_textBox_url.Text;
            }
        }

        public string UserName
        {
            get
            {
                return this.Login_textBox_userName.Text;
            }
        }

        public string Password
        {
            get
            {
                return this.Login_textBox_password.Text;
            }
        }

        public string Parameters
        {
            get
            {
                return this.Login_textBox_parameters.Text;
            }
        }

        public RestChannel GetChannel()
        {
            if (this.ServerUrl == "" || this.UserName == "")
            {
                throw new Exception("尚未设置dp2library url或用户名");
            }

            RestChannel channel= this._channelPool.GetChannel(this.ServerUrl, this.UserName);

            return channel;
        }

        void channelPool_BeforeLogin(object sender, BeforeLoginEventArgs e)
        {
            if (string.IsNullOrEmpty(this.ServerUrl))
            {
                e.Cancel = true;
                e.ErrorInfo = "dp2library URL为空";
            }

            e.LibraryServerUrl = this.ServerUrl;
            e.UserName = this.UserName;
            e.Password = this.Password;
            e.Parameters = this.Parameters;//"type=worker,client=dp2analysis|0.01";
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this._channelPool.BeforeLogin -= new BeforeLoginEventHandle(channelPool_BeforeLogin);
            this._channelPool.BeforeLogin += new BeforeLoginEventHandle(channelPool_BeforeLogin);


            this.Server_textBox_url.Text = Properties.Settings.Default.global_url;
            this.Login_textBox_userName.Text = Properties.Settings.Default.login_userName;
            this.Login_textBox_password.Text = Properties.Settings.Default.login_password;
            this.Login_textBox_parameters.Text = Properties.Settings.Default.login_parameters;

            this.SearchBiblio_textBox_BiblioDbNames.Text = Properties.Settings.Default.searchBiblio_biblioDbNames;
            this.SearchBiblio_textBox_QueryWord.Text = Properties.Settings.Default.searchBiblio_queryWord;
            this.SearchBiblio_textBox_PerMax.Text = Properties.Settings.Default.searchBiblio_perMax;
            this.SearchBiblio_textBox_FromStyle.Text = Properties.Settings.Default.searchBiblio_fromStyle;
            this.SearchBiblio_comboBox_MatchStyle.Text = Properties.Settings.Default.searchBiblio_matchStyle;
            this.SearchBiblio_textBox_ResultSetName.Text = Properties.Settings.Default.searchBiblio_resultSetName;
            this.SearchBiblio_textBox_SearchStyle.Text = Properties.Settings.Default.searchBiblio_searchStyle;

            this.GetSearchResult_textBox_ResultSetName.Text = Properties.Settings.Default.getSearchResult_resultsetName;
            this.GetSearchResult_textBox_Start.Text = Properties.Settings.Default.getSearchResult_start;
            this.GetSearchResult_textBox_Count.Text = Properties.Settings.Default.getSearchResult_count;
            this.GetSearchResult_textBox_BrowseInfoStyle.Text = Properties.Settings.Default.getSearchResult_browseInfoStyle;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._channelPool.CleanChannel();

            Properties.Settings.Default.global_url = this.Server_textBox_url.Text;

            Properties.Settings.Default.login_userName = this.Login_textBox_userName.Text;
            Properties.Settings.Default.login_password = this.Login_textBox_password.Text;
            Properties.Settings.Default.login_parameters = this.Login_textBox_parameters.Text;


            Properties.Settings.Default.searchBiblio_biblioDbNames=this.SearchBiblio_textBox_BiblioDbNames.Text;
            Properties.Settings.Default.searchBiblio_queryWord=this.SearchBiblio_textBox_QueryWord.Text;
            Properties.Settings.Default.searchBiblio_perMax=this.SearchBiblio_textBox_PerMax.Text;
            Properties.Settings.Default.searchBiblio_fromStyle=this.SearchBiblio_textBox_FromStyle.Text;
            Properties.Settings.Default.searchBiblio_matchStyle= this.SearchBiblio_comboBox_MatchStyle.Text;
            Properties.Settings.Default.searchBiblio_resultSetName=this.SearchBiblio_textBox_ResultSetName.Text;
            Properties.Settings.Default.searchBiblio_searchStyle=this.SearchBiblio_textBox_SearchStyle.Text;

            Properties.Settings.Default.getSearchResult_resultsetName=this.GetSearchResult_textBox_ResultSetName.Text;
            Properties.Settings.Default.getSearchResult_start=this.GetSearchResult_textBox_Start.Text;
            Properties.Settings.Default.getSearchResult_count=this.GetSearchResult_textBox_Count.Text;
            Properties.Settings.Default.getSearchResult_browseInfoStyle=this.GetSearchResult_textBox_BrowseInfoStyle.Text ;


            // 一定要调save函数才能把信息保存下来
            Properties.Settings.Default.Save();
        }
        private void Display(string text)
        {
            //if (text == "Empty")
            //    textBox_result.Text = String.Empty;
            //    //textBox_result.Text = string.Empty;

            //else
            textBox_result.Text += text+"\r\n";
        }
        private void ClearDisplay()
        {
            textBox_result.Clear(); 
        }
        private void 通用练习题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_c dlg = new Form_c();
            dlg.ShowDialog(this);
        }

        #region 登录

        private void button_getVersion_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
                GetVersionResponse response= channel.GetVersion();
                if (response.GetVersionResult.Value == -1)
                {
                    this.textBox_result.Text += "获取版本出错：" + response.GetVersionResult.ErrorInfo;
                    return;
                }
                this.textBox_result.Text += response.GetVersionResult.ErrorInfo;
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
                string userName = this.Login_textBox_userName.Text.Trim();
                string password = this.Login_textBox_password.Text.Trim();
                string parameters = this.Login_textBox_parameters.Text.Trim();//"type=worker,client=resttest|0.01";
                if (userName == "")
                {
                    MessageBox.Show(this, "用户名不能为空");
                    return;
                }
                // 登录接口
                /// <para>-1:   出错</para>
                /// <para>0:    登录未成功</para>
                /// <para>1:    登录成功</para>
                LoginResponse response = channel.Login(userName,
                    password,
                    parameters);
                if (response.LoginResult.Value == 1)
                {
                    this.textBox_result.Text = "登录成功\r\n";
                }
                else
                {
                    this.textBox_result.Text = "登录失败\r\n";
                }

                this.textBox_result.Text += "Result:" 
                    + response.LoginResult.ErrorCode 
                    + response.LoginResult.ErrorInfo + "\r\n"
                    + "Rights:" + response.strRights + "\r\n"
                    + "UserName:" + response.strOutputUserName;
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_logout_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
               LogoutResponse response = channel.Logout();
                this.textBox_result.Text = "Result:" 
                    + response.LogoutResult.ErrorCode 
                    + response.LogoutResult.ErrorInfo;
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        #endregion

        private void button_SearchBiblio_Click(object sender, EventArgs e)
        {

            RestChannel channel = this.GetChannel();
            try
            {
                int nPerMax = 0;
                if (this.SearchBiblio_textBox_PerMax.Text == "")
                    nPerMax = -1;
                else
                    nPerMax = Convert.ToInt32(this.SearchBiblio_textBox_PerMax.Text);


                //SearchBiblioResponse response 
                long lRet= channel.SearchBiblio(this.SearchBiblio_textBox_BiblioDbNames.Text,
                    this.SearchBiblio_textBox_QueryWord.Text,
                    nPerMax,
                    this.SearchBiblio_textBox_FromStyle.Text,
                    this.SearchBiblio_comboBox_MatchStyle.Text,
                    this.SearchBiblio_textBox_ResultSetName.Text,
                    SearchBiblio_textBox_OutputStyle.Text,// this.SearchBiblio_textBox_SearchStyle.Text,
                    this.SearchBiblio_textBox_filter.Text,
                    out string strQueryXml,
            out string strError);

                if (lRet == -1)
                {
                    this.textBox_result.Text = "error:" + strError;
                }
                else
                {
                    this.textBox_result.Text = "count:" + lRet + "\r\n"
                        + strQueryXml;
                }
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_GetSearchResult_Click(object sender, EventArgs e)
        {
            this.textBox_result.Text = "";

            RestChannel channel = this.GetChannel();
            try
            {
                //GetSearchResultResponse response
                long lRet = channel.GetSearchResult(this.GetSearchResult_textBox_ResultSetName.Text,
                    Convert.ToInt64(this.GetSearchResult_textBox_Start.Text),
                    Convert.ToInt64(this.GetSearchResult_textBox_Count.Text),
                    this.GetSearchResult_textBox_BrowseInfoStyle.Text,
                    "",
                    out Record[] records,
                    out string strError);

                if (lRet == -1)
                {
                    this.textBox_result.Text = "errorInfo:" + strError;
                }



                if (records != null && records.Length > 0)
                {
                    this.textBox_result.Text = "命中'" + lRet + "'条，本次返回'" + records.Length + "'条。\r\n";

                    StringBuilder browse = new StringBuilder();
                    foreach (Record record in records)
                    {
                        browse.AppendLine(record.Path);

                        if (record.Cols != null && record.Cols.Length > 0)
                            browse.AppendLine(string.Join(",", record.Cols));
                        if (record.RecordBody != null && string.IsNullOrEmpty(record.RecordBody.Xml) == false)
                            browse.AppendLine(record.RecordBody.Xml);
                    }
                    this.textBox_result.Text += browse.ToString();
                }
                else
                {
                    this.textBox_result.Text = "命中'" + lRet + "'条，返回'0'条";
                }
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_GetBiblioInfo_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
                GetBiblioInfoResponse response = channel.GetBiblioInfo(this.GetBiblioInfo_textBox_BiblioRecPath.Text,
                    this.GetBiblioInfo_textBox_BiblioType.Text);

                this.textBox_result.Text = "Result:" + response.GetBiblioInfoResult.ErrorCode
                    + response.GetBiblioInfoResult.ErrorInfo + "\r\n"
                    + response.strBiblio + "\r\n";
                    //+ response.
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_GetBiblioInfos_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {

                string strformats = GetBiblioInfos_textBox_Formats.Text;
                string[] formats = strformats.Split(new char[] { ','});

                GetBiblioInfosResponse response = channel.GetBiblioInfos(this.GetBiblioInfos_textBox_BiblioRecPath.Text,
                    formats);

                string strResult = "result:\r\n";
                if (response.results != null && response.results.Length > 0)
                {
                    foreach (string s in response.results)
                    {
                        strResult += s + "\r\n";
                    }
                }

                string strTimestamp = ByteArray.GetHexTimeStampString(response.baTimestamp);

                this.textBox_result.Text = "Result:" + response.GetBiblioInfosResult.ErrorCode
                    + response.GetBiblioInfosResult.ErrorInfo + "\r\n"
                    + "timestamp:" + strTimestamp + "\r\n"
                + strResult;
                    
                //+ response.
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button__Reservation_start_Click(object sender, EventArgs e)
        {
            RestChannel channel = this.GetChannel();
            try
            {
                ReservationResponse response = channel.Reservation(
                    this.comboBox_Reservation_action.Text,
                    this.textBox__Reservation_readerBarcode.Text,
                    this.textBox_Reservation_itemBarcodeList.Text
                    );

                this.textBox_result.Text = "Result:"
                    + "ErrorCode:" + response.ReservationResult.ErrorCode
                    + "\r\n" + "ErrorInfo:" + response.ReservationResult.ErrorInfo;
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        // 创建书目
        private void button_setBiblioInfo_Click(object sender, EventArgs e)
        {
            string strError = "";
            string strAction = this.textBox_action.Text;
            string biblioRecPath = this.textBox_biblioPath.Text;
            string biblioType = this.textBox_biblioType.Text;
            string biblio = this.textBox_biblio.Text;

            string timestamp=this.textBox_timestamp.Text;
            byte[] baTimestamp = ByteArray.GetTimeStampByteArray(timestamp);
            string strComment = "";
            string strStyle = this.textBox_style.Text;

            RestChannel channel = this.GetChannel();
            try
            {


                byte[] baNewTimestamp = null;
                string strOutputPath = "";

                long lRet = channel.SetBiblioInfo(
                    strAction,
                    biblioRecPath,
                    biblioType,
                    biblio,
                    baTimestamp,
                    strComment,
                    strStyle,
                    out strOutputPath,
                    out baNewTimestamp,
                    out strError);
                if (lRet == -1)
                {
                    this.textBox_result.Text = strError;
                    MessageBox.Show(this, strError);
                    return;
                }

                this.textBox_result.Text = "strOutputPath=" + strOutputPath + "\r\n"
                    + "baNewTimestamp=" + ByteArray.GetHexTimeStampString(baNewTimestamp) +"\r\n"
                    + "strError=" + strError
                    + "lRet=" + lRet.ToString();

            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_getField_Click(object sender, EventArgs e)
        {
            string strFieldMap = this.textBox_map.Text.Trim();
            if (string.IsNullOrEmpty(strFieldMap) == true)
            {
                MessageBox.Show(this, "尚未配置字段提取规则。");
                return;
            }

            //
            string strMarc = this.textBox_biblio.Text.Trim();
            if (string.IsNullOrEmpty(strMarc) == true)
            {
                MessageBox.Show(this, "尚未设置marc数据。");
                return;
            }

            // 显示在界面上
            this.textBox_result.Text =MarcHelper.GetFields(strMarc, strFieldMap);
            
        }

        private void button_setField_Click(object sender, EventArgs e)
        {
            string strFieldMap = this.textBox_map.Text.Trim();
            if (string.IsNullOrEmpty(strFieldMap) == true)
            {
                MessageBox.Show(this, "尚未配置字段提取规则。");
                return;
            }
            List<FieldItem> fieldList = null;
            try
            {
                fieldList = MarcHelper.ParseFieldMap(strFieldMap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                return;
            }

            //
            string strMarc = this.textBox_biblio.Text.Trim();
            if (string.IsNullOrEmpty(strMarc) == true)
            {
                MessageBox.Show(this, "尚未设置marc数据。");
                return;
            }

            string marc =MarcHelper.SetFields(strMarc, fieldList);

            this.textBox_result.Text = marc;


        }

        private void button_searchItem_Click(object sender, EventArgs e)
        {
            
            RestChannel channel = this.GetChannel();
            try
            {
                int nPerMax = 0;
                if (this.searchItem_nPerMax.Text == "")
                    nPerMax = -1;
                else
                    nPerMax = Convert.ToInt32(this.searchItem_nPerMax.Text);
                /*
                 * 
public long SearchItem(string strItemDbName,
    string strQueryWord,
    int nPerMax,
    string strFrom,
    string strMatchStyle,
    string strResultSetName,
    string strSearchStyle,
     string strOutputStyle,
    out string strError)
                 */
                //SearchBiblioResponse response 
                long lRet = channel.SearchItem(this.searchItem_strItemDbName.Text,
                    this.searchItem_strQueryWord.Text,
                    nPerMax,
                    this.searchItem_strFrom.Text,
                    this.searchItem_strMatchStyle.Text,
                    this.searchItem_strResultSetName.Text,
                    this.searchItem_strSearchStyle.Text,
                    this.searchItem_strOutputStyle.Text,
                    out string strError);
                if (lRet == -1)
                {
                    this.textBox_result.Text = "error:" + strError;
                }
                else
                {
                    this.textBox_result.Text = "count:" + lRet;
                }
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void button_SearchCharging_begin_Click(object sender, EventArgs e)
        {
            this.textBox_result.Text = "";

            RestChannel channel = this.GetChannel();
            try
            {
                
                string strStart = this.textBox_searchCharging_start.Text.Trim();
                if (strStart == "")
                    strStart = "0";
                long start = Convert.ToInt64(strStart);

                string strCount = this.textBox_searchCharging_count.Text.Trim();
                if (strCount == "")
                    strCount = "10";
                long count = Convert.ToInt64(strCount);

                ChargingItemWrapper[] itemWarpperList = null;

                //SearchBiblioResponse response 
                long lRet = channel.SearchCharging(this.textBox_SearchCharging_patronBarcode.Text.Trim(),
                    this.textBox_SearchCharging_timeRange.Text.Trim(),
                    this.textBox_searchCharging_actions.Text.Trim(),
                    this.textBox_searchCharging_order.Text.Trim(),
                    start,
                    count,
                    out itemWarpperList,
                    out string strError);
                if (lRet == -1)
                {
                    this.textBox_result.Text += "error:" + strError;
                }
                else
                {
                    this.textBox_result.Text += "count:" + lRet;

                    if (itemWarpperList != null && itemWarpperList.Length > 0)
                    {
                        string temp = "";
                        foreach (ChargingItemWrapper one in itemWarpperList)
                        {
                            temp += one.Item.ItemBarcode + "\r\n";
                        }

                        this.textBox_result.Text+= temp;
                    }
                }
            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }
        }

        private void z3950ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Z3950 dlg = new Form_Z3950();
            dlg.ShowDialog(this);
        }

        private void button_getres_Click(object sender, EventArgs e)
        {
            RestChannel channel = GetChannel();
            string path = textBox_strrespath.Text.Trim();
            long start = long.Parse(textBox_nstart.Text.Trim());
            int length = Convert.ToInt32(textBox_nlength.Text.Trim());
            string style = textBox_style1.Text.Trim();
            string[] words = path.Split('/');

            try
            {
                GetResResponse getRes = channel.GetRes(path, start, length, style);

                //获得16进制时间戳
                string strTimestamp = ByteArray.GetHexTimeStampString(getRes.baOutputTimestamp);
                //value
                // string strbaContent = GetHexTimeStampString(getRes.baContent);

                //获得文件总尺寸
                long start1 = getRes.GetResResult.Value;

                //当路径为“XXX/1/object/1”时，是想获取对象文件
                if (words.Length == 4)
                {
                    //判断是否为空
                    if (getRes.baContent == null)
                        // Display("Empty");
                        ClearDisplay();
                        Display(getRes.GetResResult.ErrorInfo);

                    if (getRes.baContent != null)
                    {
                        //获得初始偏移量，为循环做准备
                        long size = getRes.baContent.Length;

                        //创建文件，开始以二进制方式写入
                        if (string.IsNullOrEmpty(path) == false)
                        {
                            //如果对象文件尺寸不为空
                            if (getRes.GetResResult.Value != -1)
                            {
                                using (FileStream fs = new FileStream(textBox_path.Text, FileMode.Create, FileAccess.Write))
                                {
                                    //textBox_result.Text += "data:\r\n" + baContent;
                                    fs.Write(getRes.baContent, 0, getRes.baContent.Length);
                                    //+ "Metadata:\r\n" + getRes.strMetadata + "\r\n"
                                    //+ "strOutputResPath:\r\n" + getRes.strOutputResPath + "\r\n"
                                    //+ "baOutputTimestamp\r\n" + strTimestamp;

                                    //循环写入对象文件，但是不 Dispose
                                    for (; ; )
                                    {
                                        if (size == start1)
                                            break;
                                        else
                                        {
                                            GetResResponse getRes1 = channel.GetRes(path, size, length, "data");
                                            fs.Write(getRes1.baContent, 0, getRes1.baContent.Length);
                                            size += getRes1.baContent.Length;
                                        }
                                    }
                                    //循环结束，Dispose
                                    fs.Dispose();
                                    MessageBox.Show("对象文件下载成功");
                                }

                                //textBox_result.Text = String.Empty;
                                //Display("Empty");
                                ClearDisplay();
                                //textBox_result.Text += "获取成功\r\n";
                                //textBox_result.Text += getRes.GetResResult.Value + "\r\n";
                                Display("获取成功");
                                Display(getRes.GetResResult.Value.ToString());
                            }
                            //当尺寸为空时
                            else
                            {
                                //Display("Empty");
                                ClearDisplay();
                                Display(getRes.GetResResult.ErrorInfo);
                                //textBox_result.Text += getRes.GetResResult.ErrorInfo;
                            }
                        }
                    }
                }

                //写入时间戳
               // textBox_result.Text += "\r\n" + "strTimestamp" + "\r\n" + strTimestamp;

                //如果是“XXX/1”格式的，表示获得XML记录
                if (words.Length == 2)
                {
                    //将二进制数据转换为string（XML格式）
                    if (getRes.baContent != null)
                    {
                        string strbaContent = System.Text.Encoding.UTF8.GetString(getRes.baContent);

                        //路径不为空
                        if (string.IsNullOrEmpty(path) == false)
                        {
                            //参照上面同样的判断，值不为1，注意这里的value表示XML元数据。
                            if (getRes.GetResResult.Value != -1)
                            {
                                ClearDisplay();
                                Display("获取成功");
                                Display(getRes.GetResResult.Value.ToString());
                                Display("strTimestamp");
                                Display(strTimestamp);
                                Display("元数据");
                                Display(getRes.strMetadata);
                                Display("数据");
                                Display(strbaContent);
                              //  textBox_result.Text = "获取成功\r\n";
                              //  textBox_result.Text += getRes.GetResResult.Value + "\r\n";
                              //  textBox_result.Text += "\r\n" + "strTimestamp" + "\r\n"
                              //      + strTimestamp + "\r\n"
                              // + "元数据" + "\r\n"
                              //+ getRes.strMetadata + "\r\n"
                              //+ "数据\r\n" + strbaContent;
                                //+"XMl\r\n"+newStr;
                            }
                        }
                        else
                        {
                            ClearDisplay();

                            //Display("Empty");
                            Display(getRes.GetResResult.ErrorInfo);
                            //textBox_result.Text = getRes.GetResResult.ErrorInfo;
                        }
                    }
                    else
                    {
                        ClearDisplay();
                        //Display("Empty");
                        Display(getRes.GetResResult.ErrorInfo);

                        //textBox_result.Text = getRes.GetResResult.ErrorInfo;
                    }
                        
                }

                //其他情况
                if(words.Length!=2)
                {
                    if (words.Length != 4)
                    {
                        ClearDisplay();
                        //Display("Empty");
                        Display(getRes.GetResResult.ErrorInfo);
                    }
                    //textBox_result.Text = getRes.GetResResult.ErrorInfo;
                }
            }
            finally { this._channelPool.ReturnChannel(channel); }

        }

        private void button_getrecord_Click(object sender, EventArgs e)
        {
            RestChannel restChannel = GetChannel();
            // string[] timestamp = 

            try
            {
                string path = textBox_strPath.Text.Trim();
                GetRecordResponse getRecord = restChannel.GetRecord(path);
                //string baContent = GetHexTimeStampString(getRecord.timestamp);
                //string newStr = Encoding.UTF8.GetString(getRecord.timestamp);
                string value = getRecord.strXml.Length.ToString();
                if (getRecord.GetRecordResult.Value == 0)
                {
                    Display("Empty");
                    Display("获取成功");
                    //textBox_result.Text = "获取成功\r\n";
                    string strTimestamp = ByteArray.GetHexTimeStampString(getRecord.timestamp);
                    Display("strXML");
                    Display(getRecord.strXml);
                    Display("时间戳");
                    Display(strTimestamp);
                    //textBox_result.Text +=
                    //    "strXML:\r\n" +
                    //    getRecord.strXml +
                    //    "时间戳:\r\n" + strTimestamp;
                    //  + "时间戳（文本）"+newStr
                    //+ "\r\n"+value;

                }
                else
                {
                    Display("Empty");
                    Display(getRecord.GetRecordResult.ErrorInfo);
                    //textBox_result.Text = "获取失败";
                }
            }
            finally { this._channelPool.ReturnChannel(restChannel); }
        }



        private void WriteRes_Click(object sender, EventArgs e)
        {
        }

        static byte[] GetRandomTimestamp()
        {
            List<byte> results = new List<byte>();
            for (int i = 0; i < 16; i++)
            {
                results.Add((byte)i);
            }

            return results.ToArray();
        }

#if NO
        //参考dp2kernelTestApi

        static long TryWriteRes(this RestChannel channel,
            string strResPath,
            string strRanges,
            long lTotalLength,
            byte[] baContent,
            string strMetadata,
            string strStyle,
            byte[] baInputTimestamp,
            out string strOutputResPath,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            // 新增资源的情况
            if (strResPath.EndsWith("?"))
            {
                return channel.WriteRes(strResPath,
strRanges,
lTotalLength,
baContent,
strMetadata,
strStyle,
baInputTimestamp,
out strOutputResPath,
out baOutputTimestamp,
out strError);
            }

            // 先尝试用一个不合适的 timestamp 写入
            var ret = channel.WriteRes(strResPath,
            strRanges,
            lTotalLength,
            baContent,
            strMetadata,
            strStyle + ",checkcreatingtimestamp",   // checkcreatingtimestamp 表示，如果记录以前不存在，新创建，则要检查请求参数中的 timestamp 必须是 null，否则就报错
            GetRandomTimestamp(),
            out strOutputResPath,
            out baOutputTimestamp,
            out strError);
            if (ret == -1 && channel.ErrorCode == ChannelErrorCode.TimestampMismatch)
            {
                // 再正式写入
                return channel.WriteRes(strResPath,
strRanges,
lTotalLength,
baContent,
strMetadata,
strStyle,
baInputTimestamp,
out strOutputResPath,
out baOutputTimestamp,
out strError);
            }

            strError = $"在用随机 timestamp 写入 '{strResPath}' 时居然没有出错，这是错误的效果。strRanges='{strRanges}' lTotalLength={lTotalLength} baContent.Length={(baContent == null ? 0 : baContent.Length)}";
            return -1;
        }

        // 用片段方式创建记录
        public static CreateResult FragmentCreateRecords(
            CancellationToken token,
            int count,
            int fragment_length = 1,
            string style = "")
        {
            var channel = DataModel.GetChannel();

            if (fragment_length < 1)
                throw new ArgumentException("fragment_length 必须大于等于 1");

            var overlap = StringUtil.IsInList("overlap", style);

            List<string> created_paths = new List<string>();
            List<AccessPoint> created_accesspoints = new List<AccessPoint>();

            for (int i = 0; i < count; i++)
            {
                token.ThrowIfCancellationRequested();

                string path = $"{strDatabaseName}/?";
                string current_barcode = (i + 1).ToString().PadLeft(10, '0');
                string xml = @"<root xmlns:dprms='http://dp2003.com/dprms'>
<barcode>{barcode}</barcode>
<dprms:file id='1' />
<dprms:file id='2' />
<dprms:file id='3' />
<dprms:file id='4' />
<dprms:file id='5' />
<dprms:file id='6' />
<dprms:file id='7' />
<dprms:file id='8' />
<dprms:file id='9' />
<dprms:file id='10' />
</root>".Replace("{barcode}", current_barcode);

                {
                    // 增大记录尺寸
                    XmlDocument dom = new XmlDocument();
                    dom.LoadXml(xml);

                    StringBuilder text = new StringBuilder();
                    for (int k = 0; k < 1024; k++)
                    {
                        text.Append(k.ToString());
                    }
                    DomUtil.SetElementText(dom.DocumentElement, "comment", text.ToString());

                    xml = dom.DocumentElement.OuterXml;
                }

                byte[] bytes = Encoding.UTF8.GetBytes(xml);

                string current_path = path;
                byte[] timestamp = null;
                long start = 0;
                long end = 0;

                string progress_id = DataModel.NewProgressID();
                DataModel.ShowProgressMessage(progress_id, $"正在用 Fragment 方式{style}创建记录 {i}，请耐心等待 ...");

                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    int chunk_length = fragment_length;

                    end = start + chunk_length - 1;

                    if (end > bytes.Length - 1)
                    {
                        end = bytes.Length - 1; // chunk_length
                        chunk_length = (int)(end - start + 1);
                    }

                    Debug.Assert(end >= start);

                    long delta = 0;  // 调整长度
                    if (overlap)
                    {
                        delta = -10;
                        if (start + delta < 0)
                            delta = -1 * start;
                    }

                    byte[] fragment = new byte[end - (start + delta) + 1];
                    Array.Copy(bytes, (start + delta), fragment, 0, fragment.Length);

                    DataModel.ShowProgressMessage(progress_id, $"正在用 Fragment 方式{style}创建记录 {current_path} {start + delta}-{end} {StringUtil.GetPercentText(end + 1, bytes.Length)}...");

                    var ret = channel.TryWriteRes(current_path,
                        $"{start + delta}-{end}",
                        bytes.Length,
                        fragment,
                        "", // strMetadata
                        "", // strStyle,
                        timestamp,
                        out string output_path,
                        out byte[] output_timestamp,
                        out string strError);
                    if (ret == -1)
                        return new CreateResult
                        {
                            Value = -1,
                            ErrorInfo = $"写入 {current_path} {start}-{end} 时出错: {strError}",
                            CreatedPaths = created_paths,
                            AccessPoints = created_accesspoints,
                        };

                    timestamp = output_timestamp;
                    current_path = output_path;

                    start += chunk_length;
                    if (start > bytes.Length - 1)
                        break;
                }

                DataModel.ShowProgressMessage(progress_id, $"用 Fragment 方式{style}创建记录 {current_path} 完成");

                token.ThrowIfCancellationRequested();

                // TODO: 读出记录检查内容是否和发出的一致
                {
                    var ret = channel.GetRes(current_path,
                        out string strResult,
                        out string _,
                        out byte[] _,
                        out string _,
                        out string strError);
                    if (ret == -1)
                        return new CreateResult
                        {
                            Value = -1,
                            ErrorInfo = strError,
                            CreatedPaths = created_paths,
                            AccessPoints = created_accesspoints,
                        };

                    if (xml != strResult)
                        return new CreateResult
                        {
                            Value = -1,
                            ErrorInfo = $"读出记录 {current_path} 内容和创建时的不一致",
                            CreatedPaths = created_paths,
                            AccessPoints = created_accesspoints,
                        };
                }

                created_paths.Add(current_path);
                created_accesspoints.Add(new AccessPoint
                {
                    Key = current_barcode,
                    From = "册条码号",
                    Path = current_path,
                });

                // 上载对象
                for (int j = 0; j < 1; j++)
                {
                    token.ThrowIfCancellationRequested();

                    int length = 1024 * 1024;
                    byte[] contents = new byte[length];
                    for (int k = 0; k < length; k++)
                    {
                        contents[k] = (byte)k;
                    }

                    string object_path = $"{current_path}/object/{j + 1}";

                    int chunk = 10 * 1024;
                    long start_offs = 0;
                    byte[] object_timestamp = null;
                    while (start_offs < length)
                    {
                        token.ThrowIfCancellationRequested();

                        long end_offs = start_offs + chunk - 1;
                        if (end_offs >= length)
                            end_offs = length - 1;

                        long delta = 0;  // 调整长度
                        if (overlap)
                        {
                            delta = -10;
                            if (start_offs + delta < 0)
                                delta = -1 * start_offs;
                        }

                        byte[] chunk_contents = new byte[end_offs - (start_offs + delta) + 1];
                        Array.Copy(contents, (start_offs + delta), chunk_contents, 0, chunk_contents.Length);
                        var ret = channel.TryWriteRes(object_path,
                            $"{start_offs + delta}-{end_offs}",
                            length,
                            chunk_contents,
                            "",
                            "content,data",
                            object_timestamp,
                            out string output_object_path,
                            out byte[] output_object_timestamp,
                            out string strError);
                        if (ret == -1)
                            return new CreateResult
                            {
                                Value = -1,
                                ErrorInfo = strError,
                                CreatedPaths = created_paths,
                                AccessPoints = created_accesspoints,
                            };
                        object_timestamp = output_object_timestamp;

                        start_offs += chunk_contents.Length;
                    }

                    token.ThrowIfCancellationRequested();

                    // 读出比较
                    using (var stream = new MemoryStream())
                    {
                        var ret = channel.GetRes(object_path,
                            stream,
                            null,
                            "content,data",
                            null,
                            out string _,
                            out byte[] download_timestamp,
                            out string _,
                            out string strError);
                        if (ret == -1)
                            return new CreateResult
                            {
                                Value = -1,
                                ErrorInfo = strError,
                                CreatedPaths = created_paths,
                                AccessPoints = created_accesspoints,
                            };
                        byte[] download_bytes = new byte[length];
                        stream.Seek(0, SeekOrigin.Begin);
                        var read_len = stream.Read(download_bytes, 0, length);
                        if (read_len != length)
                            return new CreateResult
                            {
                                Value = -1,
                                ErrorInfo = "read file error"
                            };

                        if (ByteArray.Compare(contents, download_bytes) != 0)
                            return new CreateResult
                            {
                                Value = -1,
                                ErrorInfo = $"对象 {object_path} 下载后和原始数据比对发现不一致",
                                CreatedPaths = created_paths,
                                AccessPoints = created_accesspoints,
                            };
                    }
                }

                // 检查检索点是否被成功创建
                foreach (var accesspoint in created_accesspoints)
                {
                    token.ThrowIfCancellationRequested();

                    string strQueryXml = $"<target list='{ strDatabaseName}:{accesspoint.From}'><item><word>{accesspoint.Key}</word><match>exact</match><relation>=</relation><dataType>string</dataType><maxCount>-1</maxCount></item><lang>chi</lang></target>";

                    var ret = channel.DoSearch(strQueryXml, "default", out string strError);
                    if (ret == -1)
                        return new CreateResult
                        {
                            Value = -1,
                            ErrorInfo = $"DoSearch() 出错: {strError}"
                        };
                    if (ret != 1)
                        return new CreateResult
                        {
                            Value = -1,
                            ErrorInfo = $"检索 '{accesspoint.Key}' 应当命中 1 条。但命中了 {ret} 条",
                        };
                }

                DataModel.SetMessage($"Fragment 方式创建记录 {current_path} 成功");
            }

            return new CreateResult
            {
                CreatedPaths = created_paths,
                AccessPoints = created_accesspoints,
            };
        }

        // 用 Fragment 方式覆盖已经创建好的记录
        public static NormalResult FragmentOverwriteRecords(
            CancellationToken token,
            IEnumerable<string> paths,
    int fragment_length = 1,
    string style = "")
        {
            var channel = DataModel.GetChannel();

            int i = 1;
            foreach (var path in paths)
            {
                token.ThrowIfCancellationRequested();

                string origin_xml = "";
                byte[] origin_timestamp = null;
                // 先获得一次原始记录。然后 Fragment 覆盖，在覆盖完以前，每次中途再获取一次记录，应该是看到原始记录
                {
                    var ret = channel.GetRes(path,
                        out origin_xml,
                        out string _,
                        out origin_timestamp,
                        out string _,
                        out string strError);
                    if (ret == -1)
                        return new CreateResult
                        {
                            Value = -1,
                            ErrorInfo = strError,
                        };
                }

                XmlDocument dom = new XmlDocument();
                dom.LoadXml(origin_xml);
                var old_text = DomUtil.GetElementText(dom.DocumentElement, "changed");
                if (old_text == null)
                    old_text = "";
                DomUtil.SetElementText(dom.DocumentElement, "changed", old_text + CreateString(i++));

                if (i > 2000)
                    i = 0;

                string new_xml = dom.DocumentElement.OuterXml;
                byte[] bytes = Encoding.UTF8.GetBytes(new_xml);

                byte[] timestamp = origin_timestamp;
                long start = 0;
                long end = 0;

                var overlap = StringUtil.IsInList("overlap", style);

                string progress_id = DataModel.NewProgressID();
                DataModel.ShowProgressMessage(progress_id, $"正在用 Fragment 方式{style}覆盖记录 {path}，请耐心等待 ...");

                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    int chunk_length = fragment_length;

                    if (chunk_length == -1)
                        chunk_length = bytes.Length;

                    end = start + chunk_length - 1;

                    if (end > bytes.Length - 1)
                    {
                        end = bytes.Length - 1;
                        chunk_length = (int)(end - start + 1);
                    }

                    Debug.Assert(end >= start);

                    long delta = 0;  // 调整长度
                    if (overlap)
                    {
                        delta = -10;
                        if (start + delta < 0)
                            delta = -1 * start;
                    }

                    byte[] fragment = new byte[end - (start + delta) + 1];
                    Array.Copy(bytes, start + delta, fragment, 0, fragment.Length);

                    DataModel.ShowProgressMessage(progress_id, $"正在用 Fragment 方式{style}覆盖记录 {path} {start + delta}-{end} {StringUtil.GetPercentText(end + 1, bytes.Length)}...");

                    byte[] timestamp_param = timestamp;
                    // 如果是从头覆盖，则需要使用读出时的完成时间戳
                    if (start + delta == 0)
                        timestamp_param = origin_timestamp;

                    var ret = channel.TryWriteRes(path,
                        $"{start + delta}-{end}",
                        bytes.Length,
                        fragment,
                        "", // strMetadata
                        "", // strStyle,
                        timestamp_param,
                        out string output_path,
                        out byte[] output_timestamp,
                        out string strError);
                    if (ret == -1)
                        return new CreateResult
                        {
                            Value = -1,
                            ErrorInfo = strError,
                        };

                    // 马上读取检验
                    if (end < bytes.Length - 1)
                    {
                        token.ThrowIfCancellationRequested();

                        ret = channel.GetRes(path,
    out string read_xml,
    out string _,
    out byte[] read_timestamp,
    out string _,
    out strError);
                        if (ret == -1)
                            return new CreateResult
                            {
                                Value = -1,
                                ErrorInfo = strError,
                            };
                        if (read_xml != origin_xml)
                            return new NormalResult
                            {
                                Value = -1,
                                ErrorInfo = $"记录 {path} {start}-{end}轮次 读取出来和 origin_xml 不一致"
                            };
                        if (ByteArray.Compare(read_timestamp, origin_timestamp) != 0)
                            return new NormalResult
                            {
                                Value = -1,
                                ErrorInfo = $"记录 {path} 的时间戳({ByteArray.GetHexTimeStampString(read_timestamp)})读取出来和 origin_timestamp({ByteArray.GetHexTimeStampString(origin_timestamp)}) 不一致"
                            };
                    }

                    timestamp = output_timestamp;

                    start += chunk_length;
                    if (start > bytes.Length - 1)
                        break;
                }

                DataModel.ShowProgressMessage(progress_id, $"用 Fragment 方式{style}覆盖记录 {path} 完成");

                token.ThrowIfCancellationRequested();

                // 覆盖成功后，马上读取检验
                {
                    var ret = channel.GetRes(path,
out string read_xml,
out string _,
out byte[] read_timestamp,
out string _,
out string strError);
                    if (ret == -1)
                        return new CreateResult
                        {
                            Value = -1,
                            ErrorInfo = strError,
                        };
                    if (read_xml != new_xml)
                        return new NormalResult
                        {
                            Value = -1,
                            ErrorInfo = $"记录 {path} 读取出来和 new_xml 不一致"
                        };
                }

                DataModel.SetMessage($"覆盖记录 {path} 成功");
            }

            return new NormalResult();
        }

#endif

        #region wuyang原来函数

        /*
        private void button_writeres_Click(object sender, EventArgs e)
        {
            RestChannel channel = GetChannel();
            try
            {
                //this.strResPath.Text = textBox_strrespath.Text;
                // this.strRanges.Text = 


                string strPath = textBox_WriteRes_strResPath.Text.Trim();
                string data = textBox_WriteRes_baContent.Text.Trim();

                //将string转换成utf8编码
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(data);

                //得到XML长度，范围。
                long Lenth = long.Parse(byteArray.Length.ToString());
                string Ranges = "0" + "-" + (byteArray.Length - 1).ToString();

                string Metadata = textBox_WriteRes_strMetadata.Text.Trim();
                string Style = textBox_WriteRes_strStyle.Text.Trim();

                //转换时间戳
                string Timestamp = textBox_WriteRes_baInputTimestamp.Text;
                byte[] stamp = ByteArray.GetTimeStampByteArray(Timestamp);
                WriteResResponse writeres = channel.WriteRes(
                    strPath,
                    Ranges,
                    Lenth,
                    byteArray,
                    Metadata,
                    Style,
                    stamp);
                //textBox_result.Text = //"获取成功\r\n";
                string strTimestamp =ByteArray.GetHexTimeStampString(writeres.baOutputTimestamp);
                Display("Empty");
                Display(writeres.WriteResResult.Value.ToString());
                Display("报错信息");
                Display(writeres.WriteResResult.ErrorInfo);
                Display("strOutputResPath");
                Display(writeres.strOutputResPath);
                Display("strTimestamp");
                Display(strTimestamp);
                //textBox_result.Text += "\r\n" + writeres.WriteResResult.Value
                //    + "\r\n" + writeres.WriteResResult.ErrorInfo
                //    + "\r\n" + writeres.strOutputResPath
                //    + "\r\n" + strTimestamp;

            }
            finally { this._channelPool.ReturnChannel(channel); }
        }
        */

        #endregion



        // 选择一个文件上传
        private void button_selectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "请指定一个上传的文件";
                dlg.FileName = "";
                dlg.Filter = "All files (*.*)|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                this.textBox_WriteRes_fileName.Text = dlg.FileName;
            }

        }

        private void button_writeresForText_Click(object sender, EventArgs e)
        {
            string strResPath = textBox_WriteRes_strResPath.Text.Trim();
            if (string.IsNullOrEmpty(strResPath) == true)
            {
                MessageBox.Show(this, "资源路径不能为空。");
                return;
            }

            string strStyle = textBox_WriteRes_strStyle.Text.Trim();// 可输入ignorechecktimestamp忽略时间戳
            //string strMetadata = textBox_WriteRes_strMetadata.Text.Trim();
                        
            //时间戳
            string timestamp = textBox_WriteRes_baInputTimestamp.Text;
            byte[] baTimestamp = ByteArray.GetTimeStampByteArray(timestamp);

            // 写入的内容
            string strText = textBox_WriteRes_baContent.Text.Trim();

            RestChannel channel = this.GetChannel();
            try
            {
                long lRet =channel.WriteText(
                     strResPath,
                     strText,
                    false, //bInlucdePreamble,
                     strStyle,
                    baTimestamp,
                    out byte[] baOutputTimestamp,
                    out string strOutputPath,
                    out string strError);
                if (lRet == -1)  //出错的情况
                {

                    this.textBox_result.Text = "出错：" + strError;

                    MessageBox.Show(this, "出错：" + strError);
                    return;
                }

                // 成功的情况
                string strOutuptTimestamp = ByteArray.GetHexTimeStampString(baOutputTimestamp);

                string info = "路径：" + strOutputPath + "\r\n"
                    + "时间戳：" + strOutuptTimestamp;
                this.textBox_result.Text= info;

                MessageBox.Show(this, "成功");

            }
            finally
            {
                this._channelPool.ReturnChannel(channel);
            }

        }

        // 帮助
        private void button_WriteRes_help_Click(object sender, EventArgs e)
        {
            // WriteRes API 帮助文档
            Process.Start("https://jihulab.com/DigitalPlatform/dp2doc/-/issues/39#note_1981539");
        }

        private void button_WriteRes_autoTimestamp_Click(object sender, EventArgs e)
        {
            this.textBox_WriteRes_baInputTimestamp.Text = ByteArray.GetHexTimeStampString(GetRandomTimestamp());

        }


        // 分块写入文件
        private void button_autoWriteRes_Click(object sender, EventArgs e)
        {
            // 校验一下参数
            string fileName = this.textBox_WriteRes_fileName.Text.Trim();
            if (string.IsNullOrEmpty(fileName) == true)
            {
                MessageBox.Show(this, "请先选择要上传的文件");
                return;
            }
            //检查是否输入了包尺寸
            string strChunkSize = this.textBox_WriteRes_chunkSize.Text.Trim();
            long inputChunkSize = 0;
            try
            {
                inputChunkSize = Convert.ToInt64(strChunkSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "包尺寸格式不合法，须为数值型。"+ex.Message);
                return;
            }

            string strResPath = textBox_WriteRes_strResPath.Text.Trim();
            if (string.IsNullOrEmpty(strResPath) == true)
            {
                MessageBox.Show(this, "资源路径不能为空。");
                return;
            }
            string strStyle = textBox_WriteRes_strStyle.Text.Trim();// 可输入ignorechecktimestamp忽略时间戳
            string strMetadata = textBox_WriteRes_strMetadata.Text.Trim();


            // 开始做事
            string strError = "";
            RestChannel channel = this.GetChannel();
            try
            {
                using (FileStream s = new FileStream(fileName, FileMode.Open))
                {
                    byte[] baInputTimestamp = null;  // 分片写入，界面输入的时间戳无意义
                    int nStart = 0;

                    long lTotalLength = s.Length;

                    while (s.Position < s.Length)
                    {
                        // 与文件剩余尺寸比对，谁小用小
                        long realChunkSize = Math.Min(inputChunkSize, s.Length - nStart);
                        byte[] baContent = new byte[realChunkSize];

                        // 从文件中读出指出尺寸的数据到baContent
                        int nLength = s.Read(baContent, 0, baContent.Length);

                        // 尺寸范围
                        string strRanges = nStart.ToString() + "-" + (nStart + nLength - 1).ToString();

                        WriteResResponse response = channel.WriteRes(strResPath,
                            strRanges,
                            lTotalLength,
                            baContent,
                            strMetadata,  //todo可以在最后一次再传metadata
                            strStyle,
                            baInputTimestamp);
                        if (response.WriteResResult.Value == -1)
                        {

                            // 第一次的时间戳不匹配，自动重试
                            if (response.WriteResResult.ErrorCode == ErrorCode.TimestampMismatch
                            && nStart == 0)
                            {
                                baInputTimestamp = response.baOutputTimestamp;
                                s.Position = nStart;
                                continue;
                            }
                            strError = response.WriteResResult.ErrorInfo;
                            goto ERROR1;
                        }

                        // 下一轮取文件的开始位置
                        nStart += nLength;

                        strResPath = response.strOutputResPath;  //// 如果第一次的strPath中包含'?'id, 必须用outputpath才能正确继续
                        baInputTimestamp = response.baOutputTimestamp;
                    }
                }
            }
            catch (Exception ex)
            {
                strError = "Exception :" + ex.Message;
                goto ERROR1;
            }

            MessageBox.Show(this, "WriteRes() complete");
            return;
        ERROR1:
            MessageBox.Show(this, strError);
        }


        // 2016/9/26 增加 if xxx != null 判断
        public static string BuildMetadata(string strMime,
            string strLocalPath)
        {
            // string strMetadata = "<file mimetype='" + strMime + "' localpath='" + strLocalPath + "'/>";
            XmlDocument dom = new XmlDocument();
            dom.LoadXml("<file />");
            if (strMime != null)
                dom.DocumentElement.SetAttribute(
                    "mimetype",
                    strMime);
            if (strLocalPath != null)
                dom.DocumentElement.SetAttribute(
                    "localpath",
                    strLocalPath);
            return dom.DocumentElement.OuterXml;
        }
    }


}
