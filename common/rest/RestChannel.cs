﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization.Json;
using System.Web;

using DigitalPlatform;
//using DigitalPlatform.Script;
using DigitalPlatform.Text;
using DigitalPlatform.Xml;

namespace DigitalPlatform.LibraryRestClient
{
    /// <summary>
    /// 通讯通道
    /// </summary>
    public class RestChannel
    {


        /// dp2Library 服务器的 URL
        public string Url { get; set; }
        public string UserName { get; set; }

        //public string Password = "";
        //public string Parameters = "";

        /// <summary>
        /// 当前通道所使用的 HTTP Cookies
        /// </summary>
        private CookieContainer Cookies = new System.Net.CookieContainer();

        // 重登录次数
        int _loginCount = 0;

        /// <summary>
        /// 按需登录,当前通道的登录前事件
        /// </summary>
        public event BeforeLoginEventHandle BeforeLogin;

        ///// <summary>
        ///// 空闲事件
        ///// </summary>
        public event IdleEventHandler Idle = null;




        ///// <summary>
        ///// 最近一次调用从 dp2Library 返回的错误码
        ///// </summary>
        //public ErrorCode ErrorCode = ErrorCode.NoError;

        // 关闭通道
        public void Close()
        {
            // 调logout接口
            this.Logout();
        }

        /// <summary>
        /// 最近一次调用从 dp2Library 返回的错误码
        /// </summary>
        public ErrorCode ErrorCode = ErrorCode.NoError;

        /// <summary>
        /// 当前已登录用户所管辖的馆代码(列表)
        /// </summary>
        public string LibraryCodeList = ""; // 当前已登录用户所管辖的馆代码 2020/10/12

        // return:
        //      -1  error
        //      0   dp2Library的版本号过低。警告信息在strError中
        //      1   dp2Library版本号符合要求
        public GetVersionResponse GetVersion()
        {
            //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();


            byte[] data = new byte[0];
            byte[] result = client.UploadData(GetRestfulApiUrl("getversion"),
                    "POST",
                    data);

            string strResult = Encoding.UTF8.GetString(result);
            GetVersionResponse response = Deserialize<GetVersionResponse>(strResult);
            return response;
        }

        #region 登录相关函数

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public LogoutResponse Logout()
        {
            //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();

            byte[] data = new byte[0];
            byte[] result = client.UploadData(GetRestfulApiUrl("logout"),
                "POST",
                data);

            string strResult = Encoding.UTF8.GetString(result);
            LogoutResponse response = Deserialize<LogoutResponse>(strResult);
            return response;
        }

        /// <summary>
        /// 登录。
        /// 请参考关于 dp2Library API Login() 的详细说明。
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <param name="strParameters">登录参数。这是一个逗号间隔的列表字符串</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    登录未成功</para>
        /// <para>1:    登录成功</para>
        /// </returns>
        public LoginResponse Login(string strUserName,
            string strPassword,
            string strParameters)
        {
            //CookieAwareWebClient client = new CookieAwareWebClient(Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();


            LoginRequest request = new LoginRequest();
            request.strUserName = strUserName;
            request.strPassword = strPassword;
            request.strParameters = strParameters;// "location=#web,type=reader"; 
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

            byte[] result = client.UploadData(this.GetRestfulApiUrl("login"),
                "POST",
                baData);

            string strResult = Encoding.UTF8.GetString(result);

            LoginResponse response = Deserialize<LoginResponse>(strResult);
            return response;
        }


        /// <summary>
        /// 清零重新登录次数
        /// </summary>
        void ClearRedoCount()
        {
            this.m_nRedoCount = 0;
        }
        int m_nRedoCount = 0;   // MessageSecurityException以后重试的次数

        /// <summary>
        /// 处理登录事宜
        /// </summary>
        /// <param name="strError">返回出错信息</param>
        /// <returns>-1: 出错; 1: 登录成功</returns>
        public int DoNotLogin(ref string strError)
        {
            this.ClearRedoCount();

            if (this.BeforeLogin != null)
            {
                BeforeLoginEventArgs ea = new BeforeLoginEventArgs();
                ea.LibraryServerUrl = this.Url;
                ea.FirstTry = true;
                ea.ErrorInfo = strError;

            REDOLOGIN:
                this.BeforeLogin(this, ea);

                if (ea.Cancel == true)
                {
                    if (String.IsNullOrEmpty(ea.ErrorInfo) == true)
                        strError = "用户放弃登录";
                    else
                        strError = ea.ErrorInfo;
                    return -1;
                }

                if (ea.Failed == true)
                {
                    strError = ea.ErrorInfo;
                    return -1;
                }

                // 2006/12/30
                if (this.Url != ea.LibraryServerUrl)
                {
                    this.Close();   // 迫使重新构造m_ws 2011/11/22
                    this.Url = ea.LibraryServerUrl;
                }

                string strMessage = "";
                if (ea.FirstTry == true)
                    strMessage = strError;

                if (_loginCount > 100)
                {
                    strError = "重新登录次数太多，超过 100 次，请检查登录 API 是否出现了逻辑问题";
                    _loginCount = 0;    // 重新开始计算
                    return -1;
                }
                _loginCount++;

                LoginResponse lRet = this.Login(ea.UserName,
                    ea.Password,
                    ea.Parameters);
                if (lRet.LoginResult.Value == -1 || lRet.LoginResult.Value == 0)
                {
                    if (String.IsNullOrEmpty(strMessage) == false)
                        ea.ErrorInfo = strMessage + "\r\n\r\n首次自动登录报错: ";
                    else
                        ea.ErrorInfo = "";
                    ea.ErrorInfo += strError;
                    ea.FirstTry = false;
                    ea.LoginFailCondition = LoginFailCondition.PasswordError;
                    goto REDOLOGIN;
                }

                /*
                // this.m_nRedoCount = 0;
                if (this.AfterLogin != null)
                {
                    AfterLoginEventArgs e1 = new AfterLoginEventArgs();
                    this.AfterLogin(this, e1);
                    if (string.IsNullOrEmpty(e1.ErrorInfo) == false)
                    {
                        strError = e1.ErrorInfo;
                        return -1;
                    }
                }
                 */
                return 1;   // 登录成功,可以重做API功能了
            }

            return -1;
        }

        #endregion

        #region 预约相关

        // 2021/2/20
        // 发送通知
        public SetMessageResponse SetMessage(string action,
            string strStyle,
            MessageData message)
        {
            string strError = "";

        REDO:

            CookieAwareWebClient client = this.GetClient();

            MessageData[] messageList = new MessageData[] { message };


            SetMessageRequest request = new SetMessageRequest();
            request.strAction = action;
            request.strStyle = "";
            request.messages = messageList;

            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            string strRequest = Encoding.UTF8.GetString(baData);
            byte[] result = client.UploadData(this.GetRestfulApiUrl("SetMessage"),
                            "POST",
                             baData);

            string strResult = Encoding.UTF8.GetString(result);
            SetMessageResponse response = Deserialize<SetMessageResponse>(strResult);
            // 未登录时，按需登录
            if (response.SetMessageResult.Value == -1
                && response.SetMessageResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                // 把按需登录的错误信息包括进去
                if (string.IsNullOrEmpty(strError) == false)
                {
                    response.SetMessageResult.ErrorInfo += "\r\n" + strError;
                }
            }

            return response;
        }


        // 2021/3/1
        // 获取一条预约记录
        public GetRecordResponse GetRecord(string strPath)
        {
            string strError = "";

        REDO:

            CookieAwareWebClient client = this.GetClient();

            GetRecordRequest request = new GetRecordRequest();
            request.strPath=strPath;

            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            string strRequest = Encoding.UTF8.GetString(baData);
            byte[] result = client.UploadData(this.GetRestfulApiUrl("GetRecord"),
                            "POST",
                             baData);

            string strResult = Encoding.UTF8.GetString(result);
            GetRecordResponse response = Deserialize<GetRecordResponse>(strResult);
            // 未登录时，按需登录
            if (response.GetRecordResult.Value == -1
                && response.GetRecordResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                // 把按需登录的错误信息包括进去
                if (string.IsNullOrEmpty(strError) == false)
                {
                    response.GetRecordResult.ErrorInfo += "\r\n" + strError;
                }
            }



            return response;
        }


        // 2011/1/21
        // 预约
        // parameters:
        //      strItemBarcodeList  册条码号列表，逗号间隔
        // 权限：需要有reservation权限
        public ReservationResponse Reservation(string action,
            string strReaderBarcode,
            string strItemBarcodeList)
        {
            string strError = "";

        REDO:

            //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();


            ReservationRequest request = new ReservationRequest();
            request.strFunction = action;
            request.strReaderBarcode = strReaderBarcode;
            request.strItemBarcodeList = strItemBarcodeList;

            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            string strRequest = Encoding.UTF8.GetString(baData);
            byte[] result = client.UploadData(this.GetRestfulApiUrl("Reservation"),
                            "POST",
                             baData);

            string strResult = Encoding.UTF8.GetString(result);
            ReservationResponse response = Deserialize<ReservationResponse>(strResult);
            // 未登录时，按需登录
            if (response.ReservationResult.Value == -1
                && response.ReservationResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                // 把按需登录的错误信息包括进去
                if (string.IsNullOrEmpty(strError) == false)
                {
                    response.ReservationResult.ErrorInfo += "\r\n" + strError;
                }
            }

            return response;
        }

        #endregion

        /// <summary>
        /// 获取书目信息
        /// </summary>
        /// <param name="strBiblioRecPath"></param>
        /// <param name="strBiblioType"></param>
        /// <returns></returns>
        public GetBiblioInfoResponse GetBiblioInfo(
            string strBiblioRecPath,
            string strBiblioType)
        {
        REDO:
            CookieAwareWebClient client = this.GetClient();

            // 设置接口参数
            GetBiblioInfoRequest request = new GetBiblioInfoRequest();
            request.strBiblioRecPath = strBiblioRecPath;
            request.strBiblioType = strBiblioType;
            request.strBiblioXml = "";
            // 将参数序列号，转成二进制
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

            // 调接口
            byte[] result = client.UploadData(this.GetRestfulApiUrl("GetBiblioInfo"),
                                                "POST",
                                                baData);
            string strResult = Encoding.UTF8.GetString(result);

            // 返回值序列号为对象
            GetBiblioInfoResponse response = Deserialize<GetBiblioInfoResponse>(strResult);

            // 未登录时，按需登录
            if (response.GetBiblioInfoResult.Value == -1
                && response.GetBiblioInfoResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                // 把按需登录的错误信息包括进去
                if (string.IsNullOrEmpty(strError) == false)
                {
                    response.GetBiblioInfoResult.ErrorInfo += "\r\n" + strError;
                }
            }

            return response;
        }


        /// <summary>
        /// 获取书目信息
        /// </summary>
        /// <param name="strBiblioRecPath"></param>
        /// <param name="strBiblioType"></param>
        /// <returns></returns>
        public GetBiblioInfosResponse GetBiblioInfos(
            string strBiblioRecPath,
            string[] formats)
        {
        REDO:
            CookieAwareWebClient client = this.GetClient();

            // 设置接口参数
            GetBiblioInfosRequest request = new GetBiblioInfosRequest();
            request.strBiblioRecPath = strBiblioRecPath;
            request.formats = formats;

            // 将参数序列号，转成二进制
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

            // 调接口
            byte[] result = client.UploadData(this.GetRestfulApiUrl("GetBiblioInfos"),
                                                "POST",
                                                baData);
            string strResult = Encoding.UTF8.GetString(result);

            // 返回值序列号为对象
            GetBiblioInfosResponse response = Deserialize<GetBiblioInfosResponse>(strResult);

            // 未登录时，按需登录
            if (response.GetBiblioInfosResult.Value == -1
                && response.GetBiblioInfosResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                // 把按需登录的错误信息包括进去
                if (string.IsNullOrEmpty(strError) == false)
                {
                    response.GetBiblioInfosResult.ErrorInfo += "\r\n" + strError;
                }
            }

            return response;
        }




        // 校验barcode
        public long VerifyBarcode(string strLibraryCode,
            string strBarcode,
            out string strError)
        {
            strError = "";
            long nRet = 0;
            try
            {
                REDO:

                CookieAwareWebClient client = this.GetClient();


                VerifyBarcodeRequest request = new VerifyBarcodeRequest();
                request.strLibraryCode = strLibraryCode;
                request.strBarcode = strBarcode;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(GetRestfulApiUrl("VerifyBarcode"),
                        "POST",
                        baData);

                string strResult = Encoding.UTF8.GetString(result);
                VerifyBarcodeResponse response = Deserialize<VerifyBarcodeResponse>(strResult);
                nRet = response.VerifyBarcodeResult.Value;
                strError = response.VerifyBarcodeResult.ErrorInfo;

                // 未登录时，按需登录
                if (response.VerifyBarcodeResult.Value == -1
                    && response.VerifyBarcodeResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    // 把按需登录的错误信息包括进去
                    if (string.IsNullOrEmpty(strError) == false)
                    {
                        response.VerifyBarcodeResult.ErrorInfo += "\r\n" + strError;
                    }
                }

                return nRet;
            }
            catch (Exception ex)
            {
                strError = "Exception :" + ex.Message;
                return -1;
            }
        }


        /// <summary>
        /// 得到所有数据库
        /// </summary>
        /// <param name="strDbXml"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public int GetAllDatabase(out string strDbXml,
            out string strError)
        {
            strDbXml = "";
            strError = "";

            CookieAwareWebClient client = this.GetClient();
            /*
        // 管理数据库
        // parameters:
        //      strAction   动作。create delete initialize backup getinfo
        // return:
        //      result.Value    -1 错误
        public LibraryServerResult ManageDatabase(string strAction,
            string strDatabaseName,
            string strDatabaseInfo,
            out string strOutputInfo)   
*/
            ManageDatabaseRequest request = new ManageDatabaseRequest();
            //strAction 动作。create delete initialize backup getinfo
            request.strAction = "getinfo"; 
            request.strDatabaseName = "";
            request.strDatabaseInfo = "";

            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(GetRestfulApiUrl("ManageDatabase"),
                    "POST",
                    baData);

            string strResult = Encoding.UTF8.GetString(result);
            ManageDatabaseResponse response = Deserialize<ManageDatabaseResponse>(strResult);
            if (response.ManageDatabaseResult.Value == -1)
            {
                strError = response.ManageDatabaseResult.ErrorInfo;
                return -1;
            }

            strDbXml = response.strOutputInfo;

            return 0;

        }




        /// <summary>
        /// 检索读者记录。
        /// 请参考 dp2Library API SearchReader() 的详细说明
        /// </summary>
        /// <param name="stop">Stop 对象</param>
        /// <param name="strReaderDbNames">读者库名。可以为单个库名，也可以是逗号(半角)分割的读者库名列表。还可以为 &lt;全部&gt;/&lt;all&gt; 之一，表示全部读者库。</param>
        /// <param name="strQueryWord">检索词</param>
        /// <param name="nPerMax">一批检索命中的最大记录数。-1表示不限制</param>
        /// <param name="strFrom">检索途径</param>
        /// <param name="strMatchStyle">匹配方式。值为left/right/exact/middle之一</param>
        /// <param name="strLang">界面语言代码。例如 "zh"</param>
        /// <param name="strResultSetName">结果集名。可使用null，等同于 "default"。而指定有区分的结果集名，可让两批以上不同目的的检索结果集可以共存</param>
        /// <param name="strOutputStyle">输出风格。keyid / keycount 之一。缺省为 keyid</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>&gt;=0:  检索命中的记录数</para>
        /// </returns>
        public long SearchReader(string strReaderDbNames,
            string strQueryWord,
            int nPerMax,
            string strFrom,
            string strMatchStyle,
            string strLang,
            string strResultSetName,
            string strOutputStyle,
            out string strError)
        {
            strError = "";

        REDO:

            CookieAwareWebClient client = this.GetClient();

            // 请求参数
            SearchReaderRequest request = new SearchReaderRequest();
            request.strReaderDbNames = strReaderDbNames;// "";           
            request.strQueryWord = strQueryWord;// "";
            request.nPerMax = nPerMax;// -1;
            request.strFrom = strFrom;// "email";
            request.strMatchStyle = strMatchStyle;// "left";
            request.strLang = strLang;// "zh";
            request.strResultSetName = strResultSetName;//"";
            request.strOutputStyle = strOutputStyle;// "id,cols";      、

            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(this.GetRestfulApiUrl("SearchReader"),
                                                "POST",
                                                baData);
            string strResult = Encoding.UTF8.GetString(result);
            SearchReaderResponse response = Deserialize<SearchReaderResponse>(strResult);
            if (response.SearchReaderResult.Value == -1 && response.SearchReaderResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
                return -1;
            }

            return response.SearchReaderResult.Value;
        }


        /*
                // 获得日志记录
                // parameters:
                //      strFileName 纯文件名,不含路径部分。但要包括".log"部分。
                //      lIndex  记录序号。从0开始计数。
                //              lIndex 为 -1 调用本函数：
                //              1) 若 strStyle 中不包含 "getcount" 时，表示希望获得整个文件尺寸值，将返回在 lHintNext 中；
                //              2) 若 strStyle 中包含 "getcount"，表示希望获得整个文件中包含的日志记录数，将返回在 lHintNext 中。
                //      lHint   记录位置暗示性参数。这是一个只有服务器才能明白含义的值，对于前端来说是不透明的。
                //              目前的含义是记录起始位置。
                //      strStyle    level-0/level-1/level-2 表示详略级别
                //                  level-0   全部
                //                  level-1   删除 读者记录和册记录
                //                  level-2   删除 读者记录和册记录中的 <borrowHistory>
                //                  getcount    表示希望获得指定日志文件中的记录总数。返回在 lHintNext 参数中
                //                  如果包含 dont_return_xml 表示在 strXml 不返回内容
                // 权限：需要getoperlog权限
                // return:
                // result.Value
                //      -1  error
                //      0   file not found
                //      1   succeed
                //      2   超过范围
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="lIndex"></param>
        /// <param name="lHint"></param>
        /// <param name="strStyle"></param>
        /// <param name="strFilter"></param>
        /// <param name="strXml"></param>
        /// <param name="lHintNext"></param>
        /// <param name="lAttachmentFragmentStart"></param>
        /// <param name="nAttachmentFragmentLength"></param>
        /// <param name="attachment_data"></param>
        /// <param name="lAttachmentTotalLength"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public long GetOperLog(string strFileName,
            long lIndex,
            long lHint,
            string strStyle,
            string strFilter,
            out string strXml,
            out long lHintNext,
            long lAttachmentFragmentStart,
            int nAttachmentFragmentLength,
            out byte[] attachment_data,
            out long lAttachmentTotalLength,
            out string strError)
        {
            strError = "";

            strXml = "";
            lHintNext = -1;

            attachment_data = null;
            lAttachmentTotalLength = 0;
        REDO:
            CookieAwareWebClient client = this.GetClient();

            // 请求参数
            GetOperLogRequest request = new GetOperLogRequest();
            request.strFileName = strFileName;       
            request.lIndex = lIndex;
            request.lHint = lHint;
            request.strStyle = strStyle;
            request.strFilter = strFilter;
            request.lAttachmentFragmentStart = lAttachmentFragmentStart;
            request.nAttachmentFragmentLength = nAttachmentFragmentLength;

            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(this.GetRestfulApiUrl("GetOperLog"),
                                                "POST",
                                                baData);
            string strResult = Encoding.UTF8.GetString(result);
            GetOperLogResponse response = Deserialize<GetOperLogResponse>(strResult);
            if (response.GetOperLogResult.Value == -1 
                && response.GetOperLogResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
                return -1;
            }

            // 给返回值赋值
            strXml = response.strXml;
            lHintNext = response.lHintNext;
            attachment_data = response.attachment_data;
            lAttachmentTotalLength = response.lAttachmentTotalLength;

            strError = response.GetOperLogResult.ErrorInfo;
            this.ErrorCode = response.GetOperLogResult.ErrorCode;
            this.ClearRedoCount();
            return response.GetOperLogResult.Value;

            /*
        REDO:
            try
            {
                IAsyncResult soapresult = this.ws.BeginGetOperLog(
                    strFileName,
                    lIndex,
                    lHint,
                    strStyle,
                    strFilter,
                    lAttachmentFragmentStart,
                    nAttachmentFragmentLength,
                    null,
                    null);

                WaitComplete(soapresult);

                if (this.m_ws == null)
                {
                    strError = "用户中断";
                    this.ErrorCode = localhost.ErrorCode.RequestCanceled;
                    return -1;
                }

                LibraryServerResult result = this.ws.EndGetOperLog(
                    out strXml,
                    out lHintNext,
                    out attachment_data,
                    out lAttachmentTotalLength,
                    soapresult);
                if (result.Value == -1 && result.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }
                strError = result.ErrorInfo;
                this.ErrorCode = result.ErrorCode;
                this.ClearRedoCount();
                return result.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }

            */

        }

    public long  GetOperLogs(
            string strFileName,
            long lIndex,
            long lHint,
            int nCount,
            string strStyle,
            string strFilter,
            out OperLogInfo[] records,
            out string strError)
        {

            records = null;
            strError = "";

        REDO:
            CookieAwareWebClient client = this.GetClient();

            // 请求参数
            GetOperLogsRequest request = new GetOperLogsRequest();
            request.strFileName = strFileName;
            request.lIndex = lIndex;
            request.lHint = lHint;
            request.strStyle = strStyle;
            request.strFilter = strFilter;
            request.nCount = nCount;

            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(this.GetRestfulApiUrl("GetOperLogs"),
                                                "POST",
                                                baData);
            string strResult = Encoding.UTF8.GetString(result);
            GetOperLogsResponse response = Deserialize<GetOperLogsResponse>(strResult);
            if (response.GetOperLogsResult.Value == -1
                && response.GetOperLogsResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
                return -1;
            }

            // 返回信息
            records = response.records;
            strError = response.GetOperLogsResult.ErrorInfo;

            this.ErrorCode = response.GetOperLogsResult.ErrorCode;
            this.ClearRedoCount();
            return response.GetOperLogsResult.Value;



        }


        /// <summary>
        /// 获得检索结果。
        /// 请参考关于 dp2Library API GetSearchResult() 的介绍
        /// </summary>
        /// <param name="strResultSetName">结果集名。如果为空，表示使用当前缺省结果集"default"</param>
        /// <param name="lStart"> 要获取的开始位置。从0开始计数</param>
        /// <param name="lCount">要获取的个数</param>
        /// <param name="strBrowseInfoStyle">返回信息的方式。
        /// id / cols / xml / timestamp / metadata / keycount / keyid 的组合。keycount 和 keyid 二者只能使用一个，缺省为 keyid。
        /// 还可以组合使用 format:???? 这样的子串，表示使用特定的浏览列格式
        /// </param>
        /// <param name="strLang">语言代码,一般为"zh"</param>
        /// <param name="searchresults">返回 Record 对象数组</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>&gt;=0:  结果集内的记录数。注意，不是本次调用返回的结果数</para>
        /// </returns>
        public long GetSearchResult(
            string strResultSetName,
            long lStart,
            long lCount,
            string strBrowseInfoStyle,
            string strLang,
            out Record[] searchresults,
            out string strError)
        {

            if (string.IsNullOrEmpty(strLang) == true)
                strLang = "zh";//strLang;

            searchresults = null;
            strError = "";
        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();


                GetSearchResultRequest request = new GetSearchResultRequest();
                request.strResultSetName = strResultSetName;
                request.lStart = lStart;
                request.lCount = lCount;
                request.strBrowseInfoStyle = strBrowseInfoStyle;
                request.strLang = strLang;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("getsearchresult"),
                                                    "POST",
                                                    baData);

                string strResult = Encoding.UTF8.GetString(result);
                GetSearchResultResponse response = Deserialize<GetSearchResultResponse>(strResult);

                if (response.GetSearchResultResult.Value == -1
                    && response.GetSearchResultResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }
                strError = response.GetSearchResultResult.ErrorInfo;

                searchresults = response.searchresults;
                this.ClearRedoCount();

                return response.GetSearchResultResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }

        }

        // 获得读者记录
        /// <summary>
        /// 获得读者记录
        /// 请参考 dp2Library API GetReaderInfo() 的详细说明
        /// </summary>
        /// <param name="stop">Stop 对象</param>
        /// <param name="strBarcode">读者证条码号，或者命令参数</param>
        /// <param name="strResultTypeList">希望获得的返回结果类型的列表。为 xml / html / text / calendar / advancexml / timestamp 的组合</param>
        /// <param name="results">返回结果信息的字符串数组</param>
        /// <param name="strRecPath">返回实际获取的记录的路径</param>
        /// <param name="baTimestamp">返回时间戳</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    没有找到读者记录</para>
        /// <para>1:    找到读者记录</para>
        /// <para>&gt;>1:   找到多于一条读者记录，返回值是找到的记录数，这是一种不正常的情况</para>
        /// </returns>
        public int GetReaderInfo(string strBarcode,
            string strResultTypeList,
            out string[] results,
            out string strRecPath,
            out string strError)
        {
            strError = "";
            results = null;
            strRecPath = "";

        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();


                GetReaderInfoRequest request = new GetReaderInfoRequest();
                request.strBarcode = strBarcode;
                request.strResultTypeList = strResultTypeList;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("getreaderinfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);
                if (string.IsNullOrEmpty(strResult) == true)
                {
                    strError = "返回消息为空内容。";
                    return -1;
                }

                GetReaderInfoResponse response = Deserialize<GetReaderInfoResponse>(strResult);
                strRecPath = response.strRecPath;

                if (response.GetReaderInfoResult.Value == -1 
                    && response.GetReaderInfoResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                if (response.GetReaderInfoResult.Value == -1)
                {
                    return -1;
                }
                else if (response.GetReaderInfoResult.Value == 0)
                {
                    strError = "获取读者记录'" + strBarcode + "'未命中。";// + readerRet.GetReaderInfoResult.ErrorInfo;
                    return -1;
                }
                else if (response.GetReaderInfoResult.Value >1)
                {
                    strError = "获取读者记录'" + strBarcode + "'出现多条，数据异常，请联系系统管理员。";// + readerRet.GetReaderInfoResult.ErrorInfo;
                    return -1;
                }

                // 返回的数据数组
                results = response.results;


                return 0;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;

                // 网络问题重做
                goto REDO; ;
            }
        }


        /// <summary>
        /// 获得实体记录信息和其从属的书目记录信息
        /// </summary>
        /// <param name="barcode">册条码</param>
        /// <param name="resultType">希望在strResult参数中返回何种格式的信息，xml/html/text之一</param>
        /// <param name="biblioType">希望返回的书目信息类型，xml/html/text之一</param>
        /// <returns></returns>
        public int GetItemInfo(string itemBarcode,
            string resultType,
            string biblioType,
            out string itemXml,
            out string biblio,
            out string strError)
        {
             strError = "";
            itemXml = "";
            biblio = "";

        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();

                GetItemInfoRequest request = new GetItemInfoRequest()
                {
                    strBarcode = itemBarcode,
                    strResultType = resultType,
                    strBiblioType = biblioType,
                };
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("getiteminfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                GetItemInfoResponse response = Deserialize<GetItemInfoResponse>(strResult);
                
                if (response.GetItemInfoResult.Value == -1 
                    && response.GetItemInfoResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    strError = response.GetItemInfoResult.ErrorInfo;
                    return -1;
                }

                if (response.GetItemInfoResult.Value == -1)
                {
                    strError = "获取册记录'" + itemBarcode + "'出错:" + response.GetItemInfoResult.ErrorInfo;
                    return -1;
                }
                else if (response.GetItemInfoResult.Value == 0)
                {
                    strError = "册'" + itemBarcode + "'对应的册记录不存在。";
                    return -1;
                }
                else if (response.GetItemInfoResult.Value  > 1)
                {
                    strError = "册条码'"+ itemBarcode + "'对应的册记录存在多条，数据异常，请联系管理员";
                    return -1;
                }

                itemXml = response.strResult;
                biblio = response.strBiblio;
                return (int)response.GetItemInfoResult.Value ;
            }
            catch (Exception ex)
            {
                // 检查是不是网络原因
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;

                // 网络原因的话，重试一下
                 goto REDO; 
            }

        }

        /// <summary>
        /// 写入读者记录,主要用于绑定和解绑微信用户
        /// </summary>
        /// <param name="strRecPath">读者记录路径</param>
        /// <param name="strNewXml">读者xml</param>
        /// <param name="strOldTimestamp">时间戳</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    成功</para>
        /// <para>1:    成功，但部分字段被拒绝</para>
        /// </returns>
        public long SetReaderInfoForWeiXin(
            string strRecPath,
            string strNewXml,
            string strOldTimestamp,
            out string strError)
        {
            string strExistingXml = "";
            string strSavedXml = "";
            string strSavedRecPath = "";
            byte[] baNewTimestamp = null;

            return this.SetReaderInfo("change",
                strRecPath,
                strNewXml,
                "",
                strOldTimestamp,
                out strExistingXml,
                out strSavedXml,
                out strSavedRecPath,
                out baNewTimestamp,
                out strError);
        }


        /// <summary>
        /// 写入读者记录。
        /// 请参考 dp2Library API SetReaderInfo() 的详细信息
        /// </summary>
        /// <param name="strAction">动作。为 new / change / delete /changestate / changeforegift 之一</param>
        /// <param name="strRecPath">记录路径</param>
        /// <param name="strNewXml">新记录 XML</param>
        /// <param name="strOldXml">旧记录 XML</param>
        /// <param name="baOldTimestamp">时间戳</param>
        /// <param name="strExistingXml">返回数据库中已经存在的记录的 XML</param>
        /// <param name="strSavedXml">返回实际保存的记录 XML</param>
        /// <param name="strSavedRecPath">返回实际保存记录的路径</param>
        /// <param name="baNewTimestamp">返回最新时间戳</param>
        /// <param name="kernel_errorcode">内核错误码</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    成功</para>
        /// <para>1:    成功，但部分字段被拒绝</para>
        /// </returns>
        public long SetReaderInfo(
            string strAction,
            string strRecPath,
            string strNewXml,
            string strOldXml,
            string strOldTimestamp,
            out string strExistingXml,
            out string strSavedXml,
            out string strSavedRecPath,
            out byte[] baNewTimestamp,
            out string strError)
        {
            strError = "";
            strExistingXml = "";
            strSavedXml = "";
            strSavedRecPath = "";
            baNewTimestamp = null;
        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();


                SetReaderInfoRequest request = new SetReaderInfoRequest();
                request.strAction = strAction;
                request.strRecPath = strRecPath;
                request.strNewXml = strNewXml;
                request.strOldXml = strOldXml;
                request.baOldTimestamp = ByteArray.GetTimeStampByteArray(strOldTimestamp);
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("setreaderinfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                SetReaderInfoResponse response = Deserialize<SetReaderInfoResponse>(strResult);
                if (response.SetReaderInfoResult != null)
                {
                    if (response.SetReaderInfoResult.Value == -1
                        && response.SetReaderInfoResult.ErrorCode == ErrorCode.NotLogin)
                    {
                        if (DoNotLogin(ref strError) == 1)
                            goto REDO;
                        return -1;
                    }
                    strError = response.SetReaderInfoResult.ErrorInfo;
                    //this.ErrorCode = response.SetReaderInfoResult.ErrorCode;
                }
                this.ClearRedoCount();
                return response.SetReaderInfoResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }

        }

        public long SetBiblioInfo(
            string strAction,
            string strBiblioRecPath,
            string strBiblioType,
            string strBiblio,
            byte[] baTimestamp,
            string strComment, 
            string strStyle,
            out string strOutputBiblioRecPath,
            out byte[] baOutputTimestamp,
            out string strError)
        {
            strError = "";
            strOutputBiblioRecPath = "";
            baOutputTimestamp = null;
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                SetBiblioInfoRequest request = new SetBiblioInfoRequest();
                request.strAction = strAction;
                request.strBiblioRecPath = strBiblioRecPath;
                request.strBiblioType = strBiblioType;
                request.strBiblio = strBiblio;
                request.baTimestamp = baTimestamp;// ByteArray.GetTimeStampByteArray(strOldTimestamp);
                request.strComment = strComment;
                request.strStyle = strStyle;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                byte[] result = client.UploadData(this.GetRestfulApiUrl("SetBiblioInfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                SetBiblioInfoResponse response = Deserialize<SetBiblioInfoResponse>(strResult);
                if (response.SetBiblioInfoResult != null)
                {
                    if (response.SetBiblioInfoResult.Value == -1
                        && response.SetBiblioInfoResult.ErrorCode == ErrorCode.NotLogin)
                    {
                        if (DoNotLogin(ref strError) == 1)
                            goto REDO;
                        return -1;
                    }
                    strError = response.SetBiblioInfoResult.ErrorInfo;
                    //this.ErrorCode = response.SetReaderInfoResult.ErrorCode;
                }
                this.ClearRedoCount();
                baOutputTimestamp = response.baOutputTimestamp;
                strOutputBiblioRecPath=response.strOutputBiblioRecPath;
                strError = response.SetBiblioInfoResult.ErrorInfo;
                return response.SetBiblioInfoResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }

        }


        /// <summary>
        /// 借/续借
        /// </summary>
        /// <param name="strReaderBarcode">读者证条码号</param>
        /// <param name="strItemBarcode">册条码</param>
        /// <param name="borrow_info">返回借阅信息</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    操作成功</para>/// 
        /// </returns>
        public int Borrow(bool bRenew,
            string strReaderBarcode,
            string strItemBarcode,
            out string strOutputReaderBarcode,
            out string strReaderXml,
            out BorrowInfo borrow_info,
            out string strError)
        {
            borrow_info = null;
            strError = "";
            strOutputReaderBarcode = "";
            strReaderXml = "";

        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();



                /// <param name="bRenew">是否为续借。true 表示xujie；false 表示普通借阅</param>
                /// <param name="strReaderBarcode">读者证条码号，或读者身份证号</param>
                /// <param name="strItemBarcode">要借阅的册条码号</param>
                /// <param name="strConfirmItemRecPath">用于确认册记录的路径</param>
                /// <param name="bForce">此参数目前未使用，设为 false 即可</param>
                /// <param name="saBorrowedItemBarcode">针对同一读者的连续操作中已经借阅的册条码号数组。用于在读者信息 HTML 界面上为这些册的信息行设置特殊背景色</param>
                /// <param name="strStyle">操作风格</param>
                /// <param name="strItemFormatList">指定在 item_records 参数中返回信息的格式列表</param>
                /// <param name="item_records">返回册相关的信息数组</param>
                /// <param name="strReaderFormatList">指定在 reader_records 参数中返回信息的各式列表</param>
                /// <param name="reader_records">返回读者相关的信息数组</param>
                /// <param name="strBiblioFormatList">指定在 biblio_records 参数中返回信息的格式列表</param>
                /// <param name="biblio_records">返回书目相关的信息数组</param>
                /// <param name="aDupPath">如果发生条码号重复，这里返回了相关册记录的路径</param>
                /// <param name="strOutputReaderBarcode">返回实际操作针对的读者证条码号</param>
                /// <param name="borrow_info">返回 BorrowInfo 结构对象，里面是一些关于借阅的详细信息</param>
                /// <param name="strError">返回出错信息</param>
                BorrowRequest request = new BorrowRequest();
                request.bRenew = bRenew;
                request.strReaderBarcode = strReaderBarcode;
                request.strItemBarcode = strItemBarcode;
                request.strConfirmItemRecPath = "";
                request.bForce = false;
                request.saBorrowedItemBarcode = null;
                request.strStyle = "reader"; //返回读者信息
                request.strReaderFormatList = "advancexml";//
                request.strItemFormatList = "";
                request.strBiblioFormatList = "";

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("Borrow"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                BorrowResponse response = Deserialize<BorrowResponse>(strResult);
                // 未登录的情况


                if (response.BorrowResult.Value == -1 
                    && response.BorrowResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }



                strOutputReaderBarcode = response.strOutputReaderBarcode;
                borrow_info = response.borrow_info;
                strError = response.BorrowResult.ErrorInfo;
                //this.ErrorCode = response.BorrowResult.ErrorCode;


                // 多笔读者记录
                if (response.BorrowResult.Value == -1 && response.BorrowResult.ErrorCode == ErrorCode.IdcardNumberDup)
                {
                    return 2;
                }

#if REMOVED
                // 未找到对应的册条码，检索是不是isbn
                if (response.BorrowResult.Value == -1 && response.BorrowResult.ErrorCode == ErrorCode.ItemBarcodeNotFound)
                {
                    string strTemp = strItemBarcode;
                    if (IsbnSplitter.IsISBN(ref strTemp) == true)
                    {
                        strError = strTemp;
                        return 3;
                    }
                }
#endif

                if (response.reader_records != null && response.reader_records.Length > 0)
                    strReaderXml = response.reader_records[0];

                this.ClearRedoCount();
                return (int)response.BorrowResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }
        }

        public int Read(string strReaderBarcode,
            string strItemBarcode,
            out string strOutputReaderBarcode,
            out string strReaderXml,
            out BorrowInfo borrow_info,
            out string strError)
        {
            borrow_info = null;
            strError = "";
            strOutputReaderBarcode = "";
            strReaderXml = "";

#if REMOVED
            string strTemp = strItemBarcode;
            if (IsbnSplitter.IsISBN(ref strTemp) == true)
            {
                strError = strTemp;
                return 3;
            }
#endif

            /*

        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();



                /// <param name="bRenew">是否为续借。true 表示xujie；false 表示普通借阅</param>
                /// <param name="strReaderBarcode">读者证条码号，或读者身份证号</param>
                /// <param name="strItemBarcode">要借阅的册条码号</param>
                /// <param name="strConfirmItemRecPath">用于确认册记录的路径</param>
                /// <param name="bForce">此参数目前未使用，设为 false 即可</param>
                /// <param name="saBorrowedItemBarcode">针对同一读者的连续操作中已经借阅的册条码号数组。用于在读者信息 HTML 界面上为这些册的信息行设置特殊背景色</param>
                /// <param name="strStyle">操作风格</param>
                /// <param name="strItemFormatList">指定在 item_records 参数中返回信息的格式列表</param>
                /// <param name="item_records">返回册相关的信息数组</param>
                /// <param name="strReaderFormatList">指定在 reader_records 参数中返回信息的各式列表</param>
                /// <param name="reader_records">返回读者相关的信息数组</param>
                /// <param name="strBiblioFormatList">指定在 biblio_records 参数中返回信息的格式列表</param>
                /// <param name="biblio_records">返回书目相关的信息数组</param>
                /// <param name="aDupPath">如果发生条码号重复，这里返回了相关册记录的路径</param>
                /// <param name="strOutputReaderBarcode">返回实际操作针对的读者证条码号</param>
                /// <param name="borrow_info">返回 BorrowInfo 结构对象，里面是一些关于借阅的详细信息</param>
                /// <param name="strError">返回出错信息</param>
                BorrowRequest request = new BorrowRequest();
                request.strReaderBarcode = strReaderBarcode;
                request.strItemBarcode = strItemBarcode;
                request.strConfirmItemRecPath = "";
                request.bForce = false;
                request.saBorrowedItemBarcode = null;
                request.strStyle = "reader"; //返回读者信息
                request.strReaderFormatList = "advancexml";//
                request.strItemFormatList = "";
                request.strBiblioFormatList = "";

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("Borrow"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                BorrowResponse response = Deserialize<BorrowResponse>(strResult);
                // 未登录的情况


                if (response.BorrowResult.Value == -1 && response.BorrowResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }



                strOutputReaderBarcode = response.strOutputReaderBarcode;
                borrow_info = response.borrow_info;
                strError = response.BorrowResult.ErrorInfo;
                this.ErrorCode = response.BorrowResult.ErrorCode;


                // 多笔读者记录
                if (response.BorrowResult.Value == -1 && response.BorrowResult.ErrorCode == ErrorCode.IdcardNumberDup)
                {
                    return 2;
                }

                // 未找到对应的册条码，检索是不是isbn
                if (response.BorrowResult.Value == -1 && response.BorrowResult.ErrorCode == ErrorCode.ItemBarcodeNotFound)
                {
                    string strTemp = strItemBarcode;
                    if (IsbnSplitter.IsISBN(ref strTemp) == true)
                    {
                        strError = strTemp;
                        return 3;
                    }
                }

                if (response.reader_records != null && response.reader_records.Length > 0)
                    strReaderXml = response.reader_records[0];

                this.ClearRedoCount();
                return (int)response.BorrowResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }
             */

            strError = "未执行";
            return -1;
        }

        ///还书
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    操作成功</para>
        /// <para>1:    操作成功，并且有值得操作人员留意的情况。提示信息在 strError 中</para>
        /// </returns>
        public int Return(string strAction,
            string strReaderBarcode,
            string strItemBarcode,
            out string strOutputReaderBarcode,
            out string strReaderXml,
            out ReturnInfo return_info,
            out string strError)
        {
            strOutputReaderBarcode = "";
            strReaderXml = "";
            return_info = null;
            strError = "";

        /*
        // 对于还回，不会同时输入证条码与册条码，所以不用检索读者是否有重，可能先判断是否isbn
        if (strAction == "return")
        {
            string strTemp = strItemBarcode;
            if (IsbnSplitter.IsISBN(ref strTemp) == true)
            {
                strError = strTemp;
                return 3;
            }
        }
         */

        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();


                // 还书
                // return:
                //      -1  出错
                //      0   正常
                //      1   有超期情况
                /// <summary>
                /// 还书或声明丢失
                /// </summary>
                /// <param name="stop">Stop 对象</param>
                /// <param name="strAction">动作参数。为 return lost 之一</param>
                /// <param name="strReaderBarcode">读者证条码号，或读者身份证号</param>
                /// <param name="strItemBarcode">要还回或声明丢失的册条码号</param>
                /// <param name="strConfirmItemRecPath">用于确认册记录的路径</param>

                /// <param name="bForce">此参数目前未使用，设为 false 即可</param>
                /// <param name="strStyle">操作风格</param>
                /// <param name="strItemFormatList">指定在 item_records 参数中返回信息的格式列表</param>
                /// <param name="item_records">返回册相关的信息数组</param>

                /// <param name="strReaderFormatList">指定在 reader_records 参数中返回信息的各式列表</param>
                /// <param name="reader_records">返回读者相关的信息数组</param>
                /// <param name="strBiblioFormatList">指定在 biblio_records 参数中返回信息的格式列表</param>
                /// <param name="biblio_records">返回书目相关的信息数组</param>
                /// <param name="aDupPath">如果发生条码号重复，这里返回了相关册记录的路径</param>
                /// <param name="strOutputReaderBarcode">返回实际操作针对的读者证条码号</param>
                /// <param name="return_info">返回 ReturnInfo 结构对象，里面是一些关于还书的详细信息</param>
                /// <param name="strError">返回出错信息</param>
                /// <returns>
                /// <para>-1:   出错</para>
                /// <para>0:    操作成功</para>
                /// <para>1:    操作成功，并且有值得操作人员留意的情况。提示信息在 strError 中</para>
                /// </returns>
                ReturnRequest request = new ReturnRequest();
                request.strAction = strAction;
                request.strReaderBarcode = strReaderBarcode;
                request.strItemBarcode = strItemBarcode;
                request.strConfirmItemRecPath = "";
                request.bForce = false;
                request.strStyle = "reader"; //返回读者信息
                request.strItemFormatList = "";
                request.strReaderFormatList = "advancexml";
                request.strBiblioFormatList = "";

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("Return"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);
                ReturnResponse response = Deserialize<ReturnResponse>(strResult);
                // 未登录的情况
                if (response.ReturnResult.Value == -1 && response.ReturnResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }
                return_info = response.return_info;
                strError = response.ReturnResult.ErrorInfo;
                strOutputReaderBarcode = response.strOutputReaderBarcode;
                // this.ErrorCode = response.ReturnResult.ErrorCode;

                // 多笔读者记录
                if (response.ReturnResult.Value == -1 && response.ReturnResult.ErrorCode == ErrorCode.IdcardNumberDup)
                {
                    return 2;
                }

#if REMOVED
                // 未找到对应的册条码，检索是不是isbn
                if (response.ReturnResult.Value == -1 && response.ReturnResult.ErrorCode == ErrorCode.ItemBarcodeNotFound)
                {
                    string strTemp = strItemBarcode;
                    if (IsbnSplitter.IsISBN(ref strTemp) == true)
                    {
                        strError = strTemp;
                        return 3;
                    }
                }
#endif

                if (response.reader_records != null && response.reader_records.Length > 0)
                    strReaderXml = response.reader_records[0];

                this.ClearRedoCount();
                return (int)response.ReturnResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }
        }



        /// <summary>
        /// 检索书目
        /// </summary>
        /// <param name="strWord">检索词</param>
        /// <param name="strError">返回出错信息</param>
        /// <para>-1:   出错</para>
        /// <para>0:    没有命中</para>
        /// <para>&gt;=1:   命中。值为命中的记录条数</para>
        /// <returns></returns>
        /*
            string strBiblioDbNames,
            string strQueryWord,
            int nPerMax,
            string strFromStyle,
            string strMatchStyle,
            string strLang,
            string strResultSetName,
            string strSearchStyle,
            string strOutputStyle,
            out string strQueryXml)
         */
        public long SearchBiblio(
            string strBiblioDbNames,
            string strQueryWord,
            int nPerMax,
            string strFromStyle,
            string strMatchStyle,
            string strResultSetName,
             string strOutputStyle,
             string strLocationFilter,
            out string strQueryXml,
            out string strError)
        {
            strError = "";
            strQueryXml = "";
        REDO:
            try
            {

                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();

                SearchBiblioRequest request = new SearchBiblioRequest();
                request.strBiblioDbNames = strBiblioDbNames;
                request.strQueryWord = strQueryWord;
                request.nPerMax = nPerMax;
                request.strFromStyle = strFromStyle;
                request.strMatchStyle = strMatchStyle;

                request.strLang = "zh";
                request.strResultSetName = strResultSetName;
                request.strSearchStyle = "";// "desc";
                request.strOutputStyle = strOutputStyle;

                request.strLocationFilter = strLocationFilter;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                string strRequest = Encoding.UTF8.GetString(baData);

                byte[] result = client.UploadData(this.GetRestfulApiUrl("SearchBiblio"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);

                SearchBiblioResponse response = Deserialize<SearchBiblioResponse>(strResult);

                // 未登录的情况
                if (response.SearchBiblioResult.Value == -1 && response.SearchBiblioResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }
                strQueryXml = response.strQueryXml;
                strError = response.SearchBiblioResult.ErrorInfo;
                // this.ErrorCode = response.SearchBiblioResult.ErrorCode;
                this.ClearRedoCount();
                return response.SearchBiblioResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }
        }


        // 检索册
        /*
        LibraryServerResult SearchItem(
            string strItemDbName,
            string strQueryWord,
            int nPerMax,
            string strFrom,
            string strMatchStyle,
            string strLang,
            string strResultSetName,
            string strSearchStyle,
            string strOutputStyle);

         */
        public long SearchItem(string strItemDbName,
            string strQueryWord,
            int nPerMax,
            string strFrom,
            string strMatchStyle,
            string strResultSetName,
            string strSearchStyle,
             string strOutputStyle,
            out string strError)
        {
            strError = "";
        REDO:
            try
            {

                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();

                SearchItemRequest request = new SearchItemRequest();
                request.strItemDbName = strItemDbName;
                request.strQueryWord = strQueryWord;
                request.nPerMax = nPerMax;
                request.strFrom = strFrom;
                request.strMatchStyle = strMatchStyle;

                request.strLang = "zh";
                request.strResultSetName = strResultSetName;
                request.strSearchStyle = strSearchStyle;// "";// "desc";
                request.strOutputStyle = strOutputStyle;



                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                string strRequest = Encoding.UTF8.GetString(baData);

                byte[] result = client.UploadData(this.GetRestfulApiUrl("SearchItem"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);

                SearchItemResponse response = Deserialize<SearchItemResponse>(strResult);

                // 未登录的情况
                if (response.SearchItemResult.Value == -1 && response.SearchItemResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }
                strError = response.SearchItemResult.ErrorInfo;
                // this.ErrorCode = response.SearchBiblioResult.ErrorCode;
                this.ClearRedoCount();
                return response.SearchItemResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }

        }


        // timeRange 2022/10/11 00:00~2022/11/09 24:00
        
        public long SearchCharging(string patronBarcode,
    string timeRange,
    string actions,
    string order,
    long start,
    long count,
    out ChargingItemWrapper[] chargeItems,
    out string strError)
        {
            strError = "";
            chargeItems = null;
        REDO:
            try
            {

                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();

                SearchChargingRequest request = new SearchChargingRequest();
                request.patronBarcode = patronBarcode;
                request.timeRange = timeRange;
                request.actions = actions;
                request.order = order;
                request.start = start;
                request.count = count;



                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                string strRequest = Encoding.UTF8.GetString(baData);

                // 调接口
                byte[] result = client.UploadData(this.GetRestfulApiUrl("SearchCharging"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);

                SearchChargingResponse response = Deserialize<SearchChargingResponse>(strResult);

                // 未登录的情况
                if (response.SearchChargingResult.Value == -1
                    && response.SearchChargingResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    return -1;
                }
                strError = response.SearchChargingResult.ErrorInfo;
                // this.ErrorCode = response.SearchBiblioResult.ErrorCode;
                this.ClearRedoCount();

                // 检索到的借阅历史
                chargeItems = response.results;
                return response.SearchChargingResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }

        }

        // 获得册信息
        // parameters:
        //      strBiblioRecPath    书目记录路径，仅包含库名和id部分
        //      lStart  返回从第几个开始    2009/6/7 add
        //      lCount  总共返回几个。0和-1都表示全部返回(0是为了兼容旧API)
        //      strStyle    "opac" 把实体记录按照OPAC要求进行加工，增补一些元素
        //                  "onlygetpath"   仅返回每个路径
        //                  "getfirstxml"   是对onlygetpath的补充，仅获得第一个元素的XML记录，其余的依然只返回路径
        //                  "getotherlibraryitem"    返回全部分馆的记录的详情。这个用法只对分馆用户有用。因为分馆用户如果不用这个style，则只获得属于自己管辖分馆的册记录的详情
        //      entityinfos 返回的实体信息数组
        //      Result.Value    -1出错 0没有找到 其他 总的实体记录的个数(本次返回的，可以通过entities.Count得到)
        // 权限：需要有getiteminfo或order权限(兼容getentities权限)
        public long GetEntities(string strBiblioRecPath,
            long lStart,
            long lCount,
            string strStyle,
            string strLang,
            out EntityInfo[] entities,
            out string strError)
        {
            strError = "";
            entities = null;
        REDO:
            try
            {

                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();


                GetEntitiesRequest request = new GetEntitiesRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.lStart = lStart;
                request.lCount = lCount;
                request.strStyle = "";// "onlygetpath";//strStyle;
                request.strLang = strLang;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                string strRequest = Encoding.UTF8.GetString(baData);

                byte[] result = client.UploadData(this.GetRestfulApiUrl("GetEntities"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);

                GetEntitiesResponse response = Deserialize<GetEntitiesResponse>(strResult);

                // 未登录的情况
                if (response.GetEntitiesResult.Value == -1 && response.GetEntitiesResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }
                entities = response.entityinfos;
                strError = response.GetEntitiesResult.ErrorInfo;
                //this.ErrorCode = response.GetEntitiesResult.ErrorCode;
                this.ClearRedoCount();
                return response.GetEntitiesResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }
        }

        /// <summary>
        /// 获得书目检索结果
        /// </summary>
        /// <param name="lStart">开始序号，从0开始</param>
        /// <param name="lCount">数量</param>
        /// <param name="resultPathList">路径数组，格式为：路径*书名</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    没有命中</para>
        /// <para>&gt;=1:   命中。值为命中的记录条数</para>
        /// </returns>
        public long GetBiblioSearchResult(long lStart,
            long lCount,
            out List<string> resultPathList,
            out string strError)
        {
            resultPathList = new List<string>();
            Record[] searchresults = null;
            long lRet = this.GetSearchResult("",//weixin-biblio",
                lStart,
                lCount,
                "id,cols",
                "zh",
                out searchresults,
                out strError);
            if (searchresults.Length <= 0)
            {
                strError = "获取目录结果集异常：" + strError;
                return -1;
            }

            for (int i = 0; i < searchresults.Length; i++)
            {
                resultPathList.Add(searchresults[i].Path + "*" + searchresults[i].Cols[0]);
            }

            return searchresults.Length;
        }


        /// <summary>
        /// 得到一条书目记录的浏览格式
        /// </summary>
        /// <param name="strRecPath">书目记录路径</param>
        /// <param name="strBrowse">返回浏览格式</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    没有找到指定路径的书目记录</para>
        /// <para>1:    成功</para>/// 
        /// </returns>
        public long GetBiblioDetail(string strRecPath,
            out string strBrowse,
            out string strError)
        {
            strBrowse = "";
            strError = "";

        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();


                GetBrowseRecordsRequest request = new GetBrowseRecordsRequest();
                request.paths = new string[] { strRecPath };
                request.strBrowseInfoStyle = "cols";

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("GetBrowseRecords"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);

                GetBrowseRecordsResponse response = Deserialize<GetBrowseRecordsResponse>(strResult);
                if (response.GetBrowseRecordsResult.Value == -1 && response.GetBrowseRecordsResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }
                strError = response.GetBrowseRecordsResult.ErrorInfo;

                Record record = response.searchresults[0];
                strBrowse = "题名:" + record.Cols[0] + "\n"
                    + "责任说明:" + record.Cols[1] + "\n"
                    + "分类号:" + record.Cols[2] + "\n"
                    + "主题词:" + record.Cols[3] + "\n"
                    + "出版者:" + record.Cols[4] + "\n"
                    + "出版时间:" + record.Cols[5];

                this.ClearRedoCount();
                return response.GetBrowseRecordsResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }
        }

        /// <summary>
        /// 获取书目摘要
        /// </summary>
        /// <param name="strItemBarcode"></param>
        /// <returns></returns>
        public GetBiblioSummaryResponse GetBiblioSummary(string strItemBarcode,
            string strBiblioRecPathExclude)
        {
            string strError = "";
        REDO:


            //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();


            GetBiblioSummaryRequest request = new GetBiblioSummaryRequest();
            request.strItemBarcode = strItemBarcode; // 册条码号
            request.strConfirmItemRecPath = "";//this.GetBiblioSummary_textBox_strConfirmItemRecPath.Text; // 记录路径
            request.strBiblioRecPathExclude = strBiblioRecPathExclude;//null; // 希望排除掉的书目记录路径


            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(this.GetRestfulApiUrl("GetBiblioSummary"),
                                                "POST",
                                                baData);

            string strResult = Encoding.UTF8.GetString(result);

            GetBiblioSummaryResponse response = Deserialize<GetBiblioSummaryResponse>(strResult);
            if (response.GetBiblioSummaryResult.Value == -1 && response.GetBiblioSummaryResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
                return response;
            }

            return response;


        }

        // 验证读者密码
        /// <summary>
        /// 验证读者帐户的密码
        /// </summary>
        /// <param name="stop"></param>
        /// <param name="strReaderBarcode">读者证条码号</param>
        /// <param name="strReaderPassword">要验证的读者帐户密码</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   验证过程出错</para>
        /// <para>0:    密码不正确</para>
        /// <para>1:    密码正确</para>
        /// </returns>
        public long VerifyReaderPassword(
            string strReaderBarcode,
            string strReaderPassword,
            out string strError)
        {
            strError = "";
        REDO:
            try
            {
                //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
                //client.Headers["Content-type"] = "application/json; charset=utf-8";
                //client.Headers["User-Agent"] = "dp2LibraryClient";
                CookieAwareWebClient client = this.GetClient();


                VerifyReaderPasswordRequest request = new VerifyReaderPasswordRequest();
                request.strReaderBarcode = strReaderBarcode;
                request.strReaderPassword = strReaderPassword;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("verifyreaderpassword"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);
                VerifyReaderPasswordResponse response = Deserialize<VerifyReaderPasswordResponse>(strResult);

                if (response.VerifyReaderPasswordResult.Value == -1
                    && response.VerifyReaderPasswordResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return -1;
                }
                strError = response.VerifyReaderPasswordResult.ErrorInfo;
                //this.ErrorCode = response.VerifyReaderPasswordResult.ErrorCode;
                this.ClearRedoCount();
                return response.VerifyReaderPasswordResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }
        }




        public GetSystemParameterResponse GetSystemParameter(string strCategory,
            string strName)
        {
        REDO:
            //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();


            GetSystemParameterRequest request = new GetSystemParameterRequest()
            {
                strCategory = strCategory,
                strName = strName
            };
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(GetRestfulApiUrl("GetSystemParameter"),
                "POST",
                baData);

            string strResult = Encoding.UTF8.GetString(result);
            GetSystemParameterResponse response = Deserialize<GetSystemParameterResponse>(strResult);
            if (response.GetSystemParameterResult.Value == -1
                && response.GetSystemParameterResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
            }

            return response;
        }


        public SearchResponse Search(string queryXml,
            string resultSetName,
            string outputStyle)
        {
        REDO:
            //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();


            SearchRequest request = new SearchRequest()
            {
                strQueryXml = queryXml,
                strResultSetName = resultSetName,
                strOutputStyle = outputStyle
            };
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(GetRestfulApiUrl("Search"),
                "POST",
                baData);

            string strResult = Encoding.UTF8.GetString(result);
            SearchResponse response = Deserialize<SearchResponse>(strResult);
            if (response.SearchResult.Value == -1
                && response.SearchResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
            }

            return response;
        }


        public WriteResResponse WriteRes(
            string strResPath,
            string strRanges,
            long lTotalLength,
            byte[] baContent,
            string strMetadata,
            string strStyle,
            byte[] baInputTimestamp)
        {
        REDO:
            //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();


            WriteResRequest request = new WriteResRequest()
            {
                strResPath = strResPath,
                strRanges = strRanges,
                lTotalLength = lTotalLength,
                baContent = baContent,
                strMetadata = strMetadata,
                strStyle = strStyle,
                baInputTimestamp = baInputTimestamp
            };

            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(GetRestfulApiUrl("WriteRes"),
                "POST",
                baData);

            string strResult = Encoding.UTF8.GetString(result);
            WriteResResponse response = Deserialize<WriteResResponse>(strResult);
            if (response.WriteResResult.Value == -1
                && response.WriteResResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
            }

            return response;
        }

        public long WriteRes(
            string strPath,
            string strXml,
            bool bInlucdePreamble,
            string strStyle,
            byte[] timestamp,
            out byte[] baOutputTimestamp,
            out string strOutputPath,
            out string strError)
        {
            strError = "";
            strOutputPath = "";
            baOutputTimestamp = null;

            int nChunkMaxLength = 4096;	// chunk

            int nStart = 0;

            byte[] baInputTimeStamp = null;

            byte[] baPreamble = Encoding.UTF8.GetPreamble();

            byte[] baTotal = Encoding.UTF8.GetBytes(strXml);

            if (bInlucdePreamble == true
                && baPreamble != null && baPreamble.Length > 0)
            {
                byte[] temp = null;
                temp = ByteArray.Add(temp, baPreamble);
                baTotal = ByteArray.Add(temp, baTotal);
            }

            int nTotalLength = baTotal.Length;

            if (timestamp != null)
            {
                baInputTimeStamp = ByteArray.Add(baInputTimeStamp, timestamp);
            }

            while (true)
            {
                DoIdle();

                int nThisChunkSize = nChunkMaxLength;

                if (nThisChunkSize + nStart > nTotalLength)
                {
                    nThisChunkSize = nTotalLength - nStart;	// 最后一次
                    if (nThisChunkSize <= 0)
                        break;
                }

                byte[] baChunk = new byte[nThisChunkSize];
                Array.Copy(baTotal, nStart, baChunk, 0, baChunk.Length);

                string strMetadata = "";
                string strRange = Convert.ToString(nStart) + "-" + Convert.ToString(nStart + baChunk.Length - 1);

                WriteResResponse response = WriteRes(strPath,
                    strRange,
                    nTotalLength,
                    baChunk,
                    strMetadata,
                    strStyle,
                    baInputTimeStamp);
                if (response.WriteResResult.Value == -1)
                {
                    strError = response.WriteResResult.ErrorInfo;
                    return -1;
                }

                baOutputTimestamp = response.baOutputTimestamp;
                strOutputPath = response.strOutputResPath;

                nStart += baChunk.Length;

                if (nStart >= nTotalLength)
                    break;

                Debug.Assert(strOutputPath != "", "outputpath不能为空");

                strPath = strOutputPath;	// 如果第一次的strPath中包含'?'id, 必须用outputpath才能正确继续
                baInputTimeStamp = baOutputTimestamp;	//baOutputTimeStamp;
            }

            return 0;
        }

        public GetResResponse GetRes(
            string strResPath,
            long lStart,
            int nLength,
            string strStyle)
        {
        REDO:
            //CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            //client.Headers["Content-type"] = "application/json; charset=utf-8";
            //client.Headers["User-Agent"] = "dp2LibraryClient";
            CookieAwareWebClient client = this.GetClient();


            GetResRequest request = new GetResRequest()
            {
                strResPath = strResPath,
                nStart = lStart,
                nLength = nLength,
                strStyle = strStyle
            };
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(GetRestfulApiUrl("GetRes"),
                "POST",
                baData);

            string strResult = Encoding.UTF8.GetString(result);
            GetResResponse response = Deserialize<GetResResponse>(strResult);
            if (response.GetResResult.Value == -1
                && response.GetResResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strStyle">一般设置为"content,data,metadata,timestamp,outputpath"</param>
        /// <param name="strResult"></param>
        /// <param name="strMetaData"></param>
        /// <param name="baOutputTimeStamp"></param>
        /// <param name="strOutputResPath"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public long GetRes(
            string strPath,
            string strStyle,
            out string strResult,
            //out byte[] baContent,
            out string strMetaData,
            out byte[] baOutputTimeStamp,
            out string strOutputResPath,
            out string strError)
        {
            strMetaData = "";
            strResult = "";
            strError = "";
            strOutputResPath = "";
            baOutputTimeStamp = null;

            byte[] baContent = null;

            int nStart = 0;
            int nPerLength = -1;

            byte[] baTotal = null;

            while (true)
            {
                DoIdle();

                GetResResponse response = GetRes(strPath,
                    nStart,
                    nPerLength,
                    strStyle);

                long lRet = response.GetResResult.Value;
                if (lRet == -1)
                {
                    strError = response.GetResResult.ErrorInfo;
                    return -1;
                }

                strMetaData = response.strMetadata;
                strOutputResPath = response.strOutputResPath;
                baOutputTimeStamp = response.baOutputTimestamp;
                baContent = response.baContent;

                if (StringUtil.IsInList("data", strStyle) != true)
                    break;

                if (StringUtil.IsInList("content", strStyle) == false)
                    return lRet;

                baTotal = ByteArray.Add(baTotal, baContent);

                Debug.Assert(baContent != null, "");
                Debug.Assert(baContent.Length <= (int)lRet, "每次返回的包尺寸[" + Convert.ToString(baContent.Length) + "]应当小于result.Value[" + Convert.ToString(lRet) + "]");

                nStart += baContent.Length;
                if (nStart >= (int)lRet)
                    break;	// 结束
            }

            if (StringUtil.IsInList("data", strStyle) != true)
                return 0;

            strResult = ByteArray.ToString(baTotal,Encoding.UTF8);

            return 0;
        }

        public void Close1()
        {
            /*
            this.m_lock.AcquireWriterLock(m_nLockTimeout);
            try
            {
                if (this.m_ws != null)
                {
                    // TODO: Search()要单独处理
                    try
                    {
                        if (this.m_ws.State != CommunicationState.Faulted)
                            this.m_ws.Close();
                    }
                    catch
                    {
                        if (this.m_ws != null)
                            this.m_ws.Abort();
                    }
                    this.m_ws = null;
                }
            }
            finally
            {
                this.m_lock.ReleaseWriterLock();
            }
             */
        }

        /// <summary>
        /// 立即放弃通讯。而 Abort() 要文雅一些
        /// </summary>
        public void AbortIt()
        {
            /*
            this.m_lock.AcquireWriterLock(m_nLockTimeout);
            try
            {
                if (this.m_ws != null)
                {
                    this.m_ws.Abort();
                    this.m_ws = null;
                }
            }
            finally
            {
                this.m_lock.ReleaseWriterLock();
            }
             */
        }

        #region 公共函数

        /// <summary>
        /// 设置http头信息
        /// </summary>
        /// <param name="client"></param>
        public CookieAwareWebClient GetClient()
        {
            CookieAwareWebClient client = new CookieAwareWebClient(this.Cookies);
            client.Headers["Content-type"] = "application/json; charset=utf-8";
            client.Headers["User-Agent"] = "dp2LibraryClient";
            return client;
        }

        void DoIdle()
        {
            System.Threading.Thread.Sleep(1);	// 避免CPU资源过度耗费

            // bool bDoEvents = true;
            if (this.Idle != null)
            {
                IdleEventArgs e = new IdleEventArgs();
                this.Idle(this, e);
                // bDoEvents = e.bDoEvents;
            }
            System.Threading.Thread.Sleep(1);	// 避免CPU资源过度耗费
        }
        /// <summary>
        /// 拼出接口url地址
        /// </summary>
        /// <param name="strMethod"></param>
        /// <returns></returns>
        private string GetRestfulApiUrl(string strMethod)
        {
            if (string.IsNullOrEmpty(this.Url) == true)
                return strMethod;

            if (this.Url[this.Url.Length - 1] == '/')
                return this.Url + strMethod;

            return this.Url + "/" + strMethod;
        }

        /// <summary>
        /// 获得异常信息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception ex)
        {
            string strResult = ex.GetType().ToString() + ":" + ex.Message;
            while (ex != null)
            {
                if (ex.InnerException != null)
                    strResult += "\r\n" + ex.InnerException.GetType().ToString() + ": " + ex.InnerException.Message;

                ex = ex.InnerException;
            }

            return strResult;
        }

        // return:
        //      0   主流程需返回-1
        //      1   需要重做API
        int ConvertWebError(Exception ex0,
            out string strError)
        {
            strError = "";

            // this.WcfException = ex0;

            // System.TimeoutException
            if (ex0 is System.TimeoutException)
            {
                //this.ErrorCode = ErrorCode.RequestTimeOut;
                //this.AbortIt();
                strError = GetExceptionMessage(ex0);
                return 0;
            }

#if NO
            if (ex0 is System.ServiceModel.Security.MessageSecurityException)
            {
                System.ServiceModel.Security.MessageSecurityException ex = (System.ServiceModel.Security.MessageSecurityException)ex0;
                this.ErrorCode = ErrorCode.RequestError;	// 一般错误
                //this.AbortIt();
                // return ex.Message + (ex.InnerException != null ? " InnerException: " + ex.InnerException.Message : "") ;
                strError = GetExceptionMessage(ex);
                if (this.m_nRedoCount == 0)
                {
                    this.m_nRedoCount++;
                    return 1;   // 重做
                }
                return 0;
            }

            if (ex0 is CommunicationObjectFaultedException)
            {
                CommunicationObjectFaultedException ex = (CommunicationObjectFaultedException)ex0;
                this.ErrorCode = ErrorCode.RequestError;	// 一般错误
                // this.AbortIt();
                strError = GetExceptionMessage(ex);
                // 2011/7/2
                if (this.m_nRedoCount == 0)
                {
                    this.m_nRedoCount++;
                    return 1;   // 重做
                }
                return 0;
            }

            if (ex0 is EndpointNotFoundException)
            {
                EndpointNotFoundException ex = (EndpointNotFoundException)ex0;
                this.ErrorCode = ErrorCode.RequestError;	// 一般错误
                //this.AbortIt();
                strError = "服务器 " + this.Url + " 没有响应";
                return 0;
            }

            if (ex0 is System.ServiceModel.CommunicationException
                && ex0.InnerException is System.ServiceModel.QuotaExceededException)
            {
                /*
                this.ErrorCode = ErrorCode.RequestError;	// 一般错误
                this.MaxReceivedMessageSize *= 2;    // 下次扩大一倍
                this.AbortIt();
                strError = GetExceptionMessage(ex0);
                if (this.m_nRedoCount == 0
                    && this.MaxReceivedMessageSize < 1024 * 1024 * 10)
                {
                    this.m_nRedoCount++;
                    return 1;   // 重做
                }
                 */
                return 0;
            }
#endif

            //this.ErrorCode = ErrorCode.RequestError;	// 一般错误
            /*
            if (this.m_ws != null)
            {
                this.AbortIt();
                // 一般来说异常都需要重新分配Client()。如果有例外，可以在前面分支
            }
             */
            strError = GetExceptionMessage(ex0);
            return 0;
        }

        #endregion

        #region 2个json序列化为类的方法

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
                return obj;
            }
        }

        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        #endregion


    }

    /// <summary>
    /// 登录失败的原因
    /// </summary>
    public enum LoginFailCondition
    {
        /// <summary>
        /// 没有出错
        /// </summary>
        None = 0,   // 没有出错
        /// <summary>
        /// 一般错误
        /// </summary>
        NormalError = 1,    // 一般错误
        /// <summary>
        /// 密码不正确
        /// </summary>
        PasswordError = 2,  // 密码不正确
    }

    /// <summary>
    /// 登录前的事件
    /// </summary>
    /// <param name="sender">发送者</param>
    /// <param name="e">事件参数</param>
    public delegate void BeforeLoginEventHandle(object sender,
    BeforeLoginEventArgs e);

    /// <summary>
    /// 登录前时间的参数
    /// </summary>
    public class BeforeLoginEventArgs : EventArgs
    {
        /// <summary>
        /// [in] 是否为第一次触发
        /// </summary>
        public bool FirstTry = true;    // [in] 是否为第一次触发
        /// <summary>
        /// [in] 图书馆应用服务器 URL
        /// </summary>
        public string LibraryServerUrl = "";    // [in] 图书馆应用服务器URL

        /// <summary>
        /// [out] 用户名
        /// </summary>
        public string UserName = "";    // [out] 用户名
        /// <summary>
        /// [out] 密码
        /// </summary>
        public string Password = "";    // [out] 密码
        /// <summary>
        /// [out] 工作台号
        /// </summary>
        public string Parameters = "";    // [out] 工作台号

        /// <summary>
        /// [out] 事件调用是否失败
        /// </summary>
        public bool Failed = false; // [out] 事件调用是否失败
        /// <summary>
        /// [out] 事件调用是否被放弃
        /// </summary>
        public bool Cancel = false; // [out] 事件调用是否被放弃

        /// <summary>
        /// [in, out] 事件调用错误信息
        /// </summary>
        public string ErrorInfo = "";   // [in, out] 事件调用错误信息

        /// <summary>
        /// [in, out] 前次登录失败的原因，本次登录失败的原因
        /// </summary>
        public LoginFailCondition LoginFailCondition = LoginFailCondition.NormalError;
    }

    /// <summary>
    /// 空闲事件
    /// </summary>
    /// <param name="sender">发送者</param>
    /// <param name="e">事件参数</param>
    public delegate void IdleEventHandler(object sender,
    IdleEventArgs e);

    /// <summary>
    /// 空闲事件的参数
    /// </summary>
    public class IdleEventArgs : EventArgs
    {
        // public bool bDoEvents = true;
    }

}
