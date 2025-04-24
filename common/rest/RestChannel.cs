//test 20250121


using System;
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
using System.Linq;
using System.Data.SqlClient;

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
            try
            {
                this.Logout();
            }
            catch (WebException ex) // 2023/1/23 增加，这里捕捉web异常，可能服务器已经访问不通了，
            {
            }
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

        public static string GetResultInfo(object o)
        {
            if (o == null)
            {
                return "GetResultInfo()传入的参数不能为null。";
            }
            if (o is LibraryServerResult)
            {
                LibraryServerResult r = (LibraryServerResult)o;
                return GetServerResultInfo(r);
            }

            else if (o is ManageDatabaseResponse)
            {
                ManageDatabaseResponse r = (ManageDatabaseResponse)o;
                return GetServerResultInfo(r.ManageDatabaseResult) + "\r\n"
                    + "strOutputInfo:" + r.strOutputInfo + "\r\n";
            }
            else if (o is WriteResResponse)
            {
                WriteResResponse r = (WriteResResponse)o;

                return GetServerResultInfo(r.WriteResResult) + "\r\n"
                    + "strOutputResPath:" + r.strOutputResPath + "\r\n"
                    + "baOutputTimestamp:" + ByteArray.GetHexTimeStampString(r.baOutputTimestamp) + "\r\n";
            }
            else if (o is GetResResponse)
            {
                GetResResponse r = (GetResResponse)o;
                string info = GetServerResultInfo(r.GetResResult) + "\r\n"
                    + "strOutputResPath:" + r.strOutputResPath + "\r\n"
                    + "baOutputTimestamp:" + ByteArray.GetHexTimeStampString(r.baOutputTimestamp) + "\r\n"
                    + "strMetadata:" + r.strMetadata + "\r\n";

                if (r.baContent != null)
                {
                    info += "baContent:" + r.baContent.Length + "字节" + "\r\n";

                    // 资源的话，转一下xml
                    if (r.strOutputResPath.IndexOf("object") == -1)
                    {
                        string xml = Encoding.UTF8.GetString(r.baContent);
                        xml = DomUtil.GetIndentXml(xml);

                        info += xml + "\r\n";

                        //info += HttpUtility.HtmlEncode(xml) + "\r\n";

                        //HttpServerUtility a = new HttpServerUtility();
                        //a.HtmlEncode(xml);
                    }
                    else
                    {
                        byte[] temp = null;
                        if (r.baContent.Length > 400)
                        {
                            temp = new byte[20];
                            //r.baContent.(temp, 0);
                            Array.Copy(r.baContent, temp, temp.Length);
                            info += "(截短)";
                        }
                        else
                        {
                            temp = r.baContent;
                        }
                        string hex = ByteArray.GetHexTimeStampString(temp);
                        info += hex + "\r\n";
                    }
                }

                return info;
            }
            else if (o is GetRecordResponse)
            {
                GetRecordResponse r = (GetRecordResponse)o;

                string xml = "";
                if (string.IsNullOrEmpty(r.strXml) == false)
                    xml = DomUtil.GetIndentXml(r.strXml);


                return GetServerResultInfo(r.GetRecordResult) + "\r\n"
                    + "timestamp:" + ByteArray.GetHexTimeStampString(r.timestamp) + "\r\n"
                    + "strXml:" + xml + "\r\n";

            }
            else if (o is SetBiblioInfoResponse)
            {
                SetBiblioInfoResponse r = (SetBiblioInfoResponse)o;
                return GetServerResultInfo(r.SetBiblioInfoResult) + "\r\n"
                    + "strOutputBiblioRecPath:" + r.strOutputBiblioRecPath + "\r\n"
                    + "baOutputTimestamp:" + ByteArray.GetHexTimeStampString(r.baOutputTimestamp) + "\r\n"
                    +"\r\n"
                    + "strOutputBiblio:"+r.strOutputBiblio;
            }
            else if (o is GetBiblioInfoResponse)
            {
                GetBiblioInfoResponse r = (GetBiblioInfoResponse)o;
                return GetServerResultInfo(r.GetBiblioInfoResult) + "\r\n"
                    + "strBiblio:" + r.strBiblio + "\r\n";
            }
            else if (o is GetBiblioInfosResponse)
            {
                GetBiblioInfosResponse r = (GetBiblioInfosResponse)o;
                string info = GetServerResultInfo(r.GetBiblioInfosResult) + "\r\n"
                    + "baTimestamp:" + ByteArray.GetHexTimeStampString(r.baTimestamp) + "\r\n";

                info += "result:\r\n";
                if (r.results != null)
                {
                    foreach (string one in r.results)
                    {
                        info += one + "\r\n";
                    }
                }
                return info;
            }
            else if (o is SetReaderInfoResponse)
            {
                SetReaderInfoResponse r = (SetReaderInfoResponse)o;

                return GetServerResultInfo(r.SetReaderInfoResult) + "\r\n"
                    + "strSavedRecPath:" + r.strSavedRecPath + "\r\n"
                    + "baNewTimestamp:" + ByteArray.GetHexTimeStampString(r.baNewTimestamp) + "\r\n"
                    + "strSavedXml:" + r.strSavedXml + "\r\n"
                    + "strExistingXml:" + r.strExistingXml + "\r\n";
            }
            else if (o is MoveReaderInfoResponse)
            {
                MoveReaderInfoResponse r = (MoveReaderInfoResponse)o;

                return GetServerResultInfo(r.MoveReaderInfoResult) + "\r\n"
                    + "target_timestamp:" + ByteArray.GetHexTimeStampString(r.target_timestamp) + "\r\n";
            }
            else if (o is VerifyBarcodeResponse)
            {
                VerifyBarcodeResponse r = (VerifyBarcodeResponse)o;

                return GetServerResultInfo(r.VerifyBarcodeResult) + "\r\n"
                    + "strOutputBarcode:" + r.strOutputBarcode + "\r\n";
            }
            else if (o is GetReaderInfoResponse)
            {
                GetReaderInfoResponse r = (GetReaderInfoResponse)o;
                string info = GetServerResultInfo(r.GetReaderInfoResult) + "\r\n"
                    + "baTimestamp:" + ByteArray.GetHexTimeStampString(r.baTimestamp) + "\r\n";

                info += "result:\r\n";
                if (r.results != null)
                {
                    foreach (string one in r.results)
                    {
                        info += one + "\r\n";
                    }
                }

                return info;
            }
            //BindPatronResponse
            else if (o is BindPatronResponse)
            {
                BindPatronResponse r = (BindPatronResponse)o;
                string info = GetServerResultInfo(r.BindPatronResult) + "\r\n";

                info += "result:\r\n";
                if (r.results != null)
                {
                    foreach (string one in r.results)
                    {
                        info += one + "\r\n";
                    }
                }

                return info;
            }
            else if (o is ChangeReaderPasswordResponse)
            {
                ChangeReaderPasswordResponse r = (ChangeReaderPasswordResponse)o;
                return GetServerResultInfo(r.ChangeReaderPasswordResult);

            }
            else if (o is ResetPasswordResponse)
            {
                ResetPasswordResponse r = (ResetPasswordResponse)o;
                return GetServerResultInfo(r.ResetPasswordResult) + "\r\n"
                    + "strMessage:" + r.strMessage;

            }

            else if (o is ChangeUserPasswordResponse)
            {
                ChangeUserPasswordResponse r = (ChangeUserPasswordResponse)o;
                return GetServerResultInfo(r.ChangeUserPasswordResult);

            }
            else if (o is SearchBiblioResponse)
            {
                SearchBiblioResponse r = (SearchBiblioResponse)o;

                return GetServerResultInfo(r.SearchBiblioResult) + "\r\n"
                    + "strQueryXml:" + r.strQueryXml + "\r\n"
                    + "explain:"+r.explain+"\r\n";
            }
            else if (o is GetSearchResultResponse)
            {
                GetSearchResultResponse r = (GetSearchResultResponse)o;

                string info = GetServerResultInfo(r.GetSearchResultResult) + "\r\n";
                Record[] records = r.searchresults;
                if (records != null)
                {
                    info += "命中'" + r.GetSearchResultResult.Value + "'条，本次返回'" + records.Length + "'条。\r\n";

                    info += GetRecordsDisplayInfo(records);
                }
                return info;
            }
            else if (o is SearchItemResponse)
            {
                SearchItemResponse r = (SearchItemResponse)o;

                return GetServerResultInfo(r.SearchItemResult);
            }
            else if (o is SettlementResponse)
            {
                SettlementResponse r = (SettlementResponse)o;

                return GetServerResultInfo(r.SettlementResult);
            }
            else if (o is HireResponse)
            {
                HireResponse r = (HireResponse)o;

                return GetServerResultInfo(r.HireResult) + "\r\n"
                + "strOutputReaderXml=" + r.strOutputReaderXml + "\r\n"
                + "strOutputID=" + r.strOutputID + "\r\n"
                ;
            }
            else if (o is ForegiftResponse)
            {
                ForegiftResponse r = (ForegiftResponse)o;

                return GetServerResultInfo(r.ForegiftResult) + "\r\n"
                + "strOutputReaderXml=" + r.strOutputReaderXml + "\r\n"
                + "strOutputID=" + r.strOutputID + "\r\n"
                ;
            }
            else if (o is GetBrowseRecordsResponse)
            {
                GetBrowseRecordsResponse r = (GetBrowseRecordsResponse)o;
                string info = GetServerResultInfo(r.GetBrowseRecordsResult) + "\r\n";
                Record[] records = r.searchresults;
                if (records != null)
                {
                    info += GetRecordsDisplayInfo(records);
                }
                return info;
            }
            else if (o is SearchReaderResponse)
            {
                SearchReaderResponse r = (SearchReaderResponse)o;
                return GetServerResultInfo(r.SearchReaderResult);
            }
            else if (o is SearchResponse)
            {
                SearchResponse r = (SearchResponse)o;
                return GetServerResultInfo(r.SearchResult);

            }
            //VerifyReaderPasswordResponse
            if (o is VerifyReaderPasswordResponse)
            {
                VerifyReaderPasswordResponse r = (VerifyReaderPasswordResponse)o;
                return GetServerResultInfo(r.VerifyReaderPasswordResult);
            }
            else if (o is GetItemInfoResponse)
            {
                GetItemInfoResponse r = (GetItemInfoResponse)o;

                return GetServerResultInfo(r.GetItemInfoResult) + "\r\n"
                    + "item_timestamp:" + ByteArray.GetHexTimeStampString(r.item_timestamp) + "\r\n"
                    + "strItemRecPath:" + r.strItemRecPath + "\r\n"
                    + "strResult:" + r.strResult + "\r\n"
                    + "\r\n"
                    + "strBiblioRecPath:" + r.strBiblioRecPath + "\r\n"
                    + "strBiblio:" + r.strBiblio + "\r\n";
            }
            else if (o is GetEntitiesResponse)
            {
                GetEntitiesResponse r = (GetEntitiesResponse)o;
                return GetServerResultInfo(r.GetEntitiesResult) + "\r\n"
                    + DisplayEntityInfos(r.entityinfos);
            }
            else if (o is GetOrdersResponse)
            {
                GetOrdersResponse r = (GetOrdersResponse)o;
                return GetServerResultInfo(r.GetOrdersResult) + "\r\n"
                    + DisplayEntityInfos(r.orderinfos);
            }
            else if (o is GetIssuesResponse)
            {
                GetIssuesResponse r = (GetIssuesResponse)o;
                return GetServerResultInfo(r.GetIssuesResult) + "\r\n"
                    + DisplayEntityInfos(r.issueinfos);
            }
            else if (o is GetCommentsResponse)
            {
                GetCommentsResponse r = (GetCommentsResponse)o;
                return GetServerResultInfo(r.GetCommentsResult) + "\r\n"
                    + DisplayEntityInfos(r.commentinfos);
            }
            else if (o is GetUserResponse)
            {
                GetUserResponse r = (GetUserResponse)o;
                string info = "";

                if (r.contents != null)
                {
                    foreach (UserInfo u in r.contents)
                    {

                        info += "UserName:" + u.UserName + "\r\n"
                         + "Rights:" + u.Rights + "\r\n"
                         + "Access:" + u.Access + "\r\n";
                        //+ "====\r\n";
                    }
                }

                //info += GetServerResultInfo(r.GetUserResult) + "\r\n";

                return info;
            }
            else if (o is SetUserResponse)
            {
                SetUserResponse r = (SetUserResponse)o;
                return GetServerResultInfo(r.SetUserResult);

            }
            //else if (o is SetItemInfoResponse)
            //{
            //    SetItemInfoResponse r = (SetItemInfoResponse)o;

            //    return "Value:"+ r.SetItemInfoResult.Value+ "\r\n"
            //        + "ErrorCode:"+r.ErrorCode + "\r\n"
            //        + "ErrorInfo:" + r.ErrorInfo + "\r\n"
                    
            //        + "strOutputRecPath:" + r.strOutputRecPath + "\r\n"
            //        + "baOutputTimestamp:" + ByteArray.GetHexTimeStampString(r.baOutputTimestamp) + "\r\n";

            //}
            else if (o is CopyBiblioInfoResponse)
            {
                CopyBiblioInfoResponse r = (CopyBiblioInfoResponse)o;
                return GetServerResultInfo(r.CopyBiblioInfoResult) + "\r\n"
                    + "strOutputBiblio:" + r.strOutputBiblio + "\r\n"
                    + "strOutputBiblioRecPath:" + r.strOutputBiblioRecPath + "\r\n"
                    + "baOutputTimestamp:" + ByteArray.GetHexTimeStampString(r.baOutputTimestamp);
            }
            else if (o is GetOperLogResponse)
            {
                GetOperLogResponse r = (GetOperLogResponse)o;
                return GetServerResultInfo(r.GetOperLogResult) + "\r\n"
                    + "strXml:" + r.strXml + "\r\n"
                    + "lHintNext:" + r.lHintNext + "\r\n"
                    + "attachment_data:" + ByteArray.GetHexTimeStampString(r.attachment_data) + "\r\n"
                    + "lAttachmentTotalLength:" + r.lAttachmentTotalLength + "\r\n";
            }
            else if (o is GetOperLogsResponse)
            {
                GetOperLogsResponse r = (GetOperLogsResponse)o;
                string info= GetServerResultInfo(r.GetOperLogsResult) + "\r\n";
                //+ "strXml:" + r.records + "\r\n"
                //+ "lHintNext:" + r.lHintNext + "\r\n"
                //+ "attachment_data:" + ByteArray.GetHexTimeStampString(r.attachment_data) + "\r\n"
                //+ "lAttachmentTotalLength:" + r.lAttachmentTotalLength + "\r\n";

                if (r.records != null)
                {
                    foreach (OperLogInfo log in r.records)
                    {
                        info += "Xml:" + log.Xml + "\r\n"
                            + "Index:" + log.Index + "\r\n"
                            + "HintNext:" + log.HintNext + "\r\n"
                            + "AttachmentLength:" + log.AttachmentLength + "\r\n" + "\r\n";
                    }
                }

                return info;
            }

            else if (o is GetCalendarResponse)
            {
                GetCalendarResponse r = (GetCalendarResponse)o;
                string info = GetServerResultInfo(r.GetCalendarResult) + "\r\n";



                if (r.contents != null)
                {
                    foreach (CalenderInfo u in r.contents)
                    {
                        info += "Name:" + u.Name + "\r\n"
                         + "Range:" + u.Range + "\r\n"
                         + "Content:" + u.Content + "\r\n"
                         + "Comment:" + u.Comment + "\r\n";
                        //+ "====\r\n";
                    }
                }

                return info;
            }
            else if (o is SetCalendarResponse)
            {
                SetCalendarResponse r = (SetCalendarResponse)o;
                string info = GetServerResultInfo(r.SetCalendarResult) + "\r\n";



                return info;  //
            }
            else if (o is BatchTaskResponse)
            {
                BatchTaskResponse r = (BatchTaskResponse)o;
                string info = GetServerResultInfo(r.BatchTaskResult) + "\r\n";

                info += "ResultText=" + r.resultInfo.ResultText;



                return info;  //BatchTaskResponse
            }
            else if (o is RepairBorrowInfoResponse)
            {
                RepairBorrowInfoResponse r = (RepairBorrowInfoResponse)o;
                string info = GetServerResultInfo(r.RepairBorrowInfoResult) + "\r\n";

                info += "nProcessedBorrowItems=" + r.nProcessedBorrowItems.ToString() + "\r\n"
                    + "nTotalBorrowItems=" + r.nTotalBorrowItems.ToString() + "\r\n"
                    + "strOutputReaderBarcode=" + r.strOutputReaderBarcode.ToString() + "\r\n";

                if (r.aDupPath != null && r.aDupPath.Length > 0)
                {
                    info += "aDupPath=" + r.aDupPath.Length.ToString() + "\r\n";
                }

                return info;

            }
            else if (o is BorrowResponse)
            {
                /*
                    public class BorrowResponse
                    {
                        [DataMember]
                        public LibraryServerResult BorrowResult { get; set; }
                        [DataMember]
                        public string[] item_records { get; set; }
                        [DataMember]
                        public string[] reader_records { get; set; }

                        [DataMember]
                        public string[] biblio_records { get; set; }

                        [DataMember]
                        public BorrowInfo borrow_info { get; set; }

                        [DataMember]
                        public string[] aDupPath { get; set; }

                        [DataMember]
                        public string strOutputReaderBarcode { get; set; }
                    }

                    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
                    public class BorrowInfo
                    {
                        [DataMember]
                        public string LatestReturnTime = "";  // 应还日期/时间
                        [DataMember]
                        public string Period = "";  //   // 借书期限。例如“20day”
                        [DataMember]
                        public long BorrowCount = 0;   // // 当前为续借的第几次？0表示初次借阅
                        [DataMember]
                        public string BorrowOperator = "";  //  借书操作者

                    }
                 */


                BorrowResponse r = (BorrowResponse)o;
                string info = GetServerResultInfo(r.BorrowResult) + "\r\n";

                if (r.item_records != null && r.item_records.Length > 0)
                {
                    info += "item_records:" + string.Join(",", r.item_records) + "\r\n";
                }
                if (r.reader_records != null && r.reader_records.Length > 0)
                {
                    info += "reader_records:" + string.Join(",", r.reader_records) + "\r\n";
                }
                if (r.biblio_records != null && r.biblio_records.Length > 0)
                {
                    info += "biblio_records:" + string.Join(",", r.biblio_records) + "\r\n";
                }
                if (r.aDupPath != null && r.aDupPath.Length > 0)
                {
                    info += "aDupPath:" + string.Join(",", r.aDupPath) + "\r\n";
                }

                info += "strOutputReaderBarcode:" + r.strOutputReaderBarcode + "\r\n";


                if (r.borrow_info != null)
                {
                    info += "LatestReturnTime:[" + r.borrow_info.LatestReturnTime + "] Period:[" + r.borrow_info.Period + "] BorrowCount:[" + r.borrow_info.BorrowCount + "] BorrowOperator:[" + r.borrow_info.BorrowOperator + "]\r\n";
                }

                return info;  //BatchTaskResponse
            }
            else if (o is ReturnResponse)
            {

                ReturnResponse r = (ReturnResponse)o;
                string info = GetServerResultInfo(r.ReturnResult) + "\r\n";

                if (r.item_records != null && r.item_records.Length > 0)
                {
                    info += "item_records:" + string.Join(",", r.item_records) + "\r\n";
                }
                if (r.reader_records != null && r.reader_records.Length > 0)
                {
                    info += "reader_records:" + string.Join(",", r.reader_records) + "\r\n";
                }
                if (r.biblio_records != null && r.biblio_records.Length > 0)
                {
                    info += "biblio_records:" + string.Join(",", r.biblio_records) + "\r\n";
                }
                if (r.aDupPath != null && r.aDupPath.Length > 0)
                {
                    info += "aDupPath:" + string.Join(",", r.aDupPath) + "\r\n";
                }

                info += "strOutputReaderBarcode:" + r.strOutputReaderBarcode + "\r\n";


                return info;  //BatchTaskResponse
            }
            else if (o is GetSystemParameterResponse)
            {
                GetSystemParameterResponse r = (GetSystemParameterResponse)o;
                string info = GetServerResultInfo(r.GetSystemParameterResult) + "\r\n";

                info += "strValue=" + r.strValue;
                return info;  
            }
            else if (o is SetSystemParameterResponse)
            {
                SetSystemParameterResponse r = (SetSystemParameterResponse)o;
                string info = GetServerResultInfo(r.SetSystemParameterResult) + "\r\n";

                info += "strValue=" + r.strValue;
                return info;
            }

            return "未识别的对象" + o.ToString();
        }

        public static string DisplayEntityInfos(EntityInfo[] entityinfos)
        {
            if (entityinfos == null)
                return "";


            string info = "\r\n";
            int nIndex = 1;
            foreach (EntityInfo e in entityinfos)
            {
                info += nIndex.ToString() + "\r\n";


                info += "Action:" + e.Action + "\r\n"
                    + "Style:" + e.Style + "\r\n"
                    + "ErrorInfo:" + e.ErrorInfo + "\r\n"
                    + "ErrorCode:" + e.ErrorCode + "\r\n"
                    + "\r\n"
                    + "RefID:" + e.RefID + "\r\n"
                    + "OldRecPath:" + e.OldRecPath + "\r\n"
                    + "OldRecord:" + e.OldRecord + "\r\n"
                    + "OldTimestamp:" + ByteArray.GetHexTimeStampString(e.OldTimestamp) + "\r\n"
                    + "\r\n"
                    + "NewRecPath:" + e.NewRecPath + "\r\n"
                    + "NewRecord:" + e.NewRecord + "\r\n"
                    + "NewTimestamp:" + ByteArray.GetHexTimeStampString(e.NewTimestamp) + "\r\n"
                    + "========\r\n";



                info += "\r\n";
                nIndex++;

            }

            return info;
        }

        public static string GetRecordsDisplayInfo(Record[] records)
        {
            if (records == null)
                return "";


            StringBuilder browse = new StringBuilder();
            foreach (Record record in records)
            {
                if (record == null)
                    continue;
                
                browse.AppendLine("path:" + record.Path);

                if (record.Cols != null)
                    browse.AppendLine("cols:" + string.Join(",", record.Cols));

                if (record.Keys != null)
                {
                    browse.AppendLine("\r\n=以下为返回的Keys=");
                    foreach (KeyFrom one in record.Keys)
                    {
                        browse.AppendLine("Logic=" + one.Logic + "--" + "Key=" + one.Key + "" + "--From=" + one.From + "");
                    }
                }

                if (record.RecordBody != null)
                {
                    browse.AppendLine("\r\n=以下为返回的RecordBody=");

                    //browse.AppendLine("Result.Value:" + record.RecordBody.Result.Value);
                    //browse.AppendLine("Result.ErrorCode:" + record.RecordBody.Result.ErrorCode);
                    //browse.AppendLine("Result.ErrorString:" + record.RecordBody.Result.ErrorString);

                    browse.AppendLine("Result.Value:" + record.RecordBody.Result?.Value);
                    browse.AppendLine("Result.ErrorCode:" + record.RecordBody.Result?.ErrorCode);
                    browse.AppendLine("Result.ErrorString:" + record.RecordBody.Result?.ErrorString);


                    browse.AppendLine("Timestamp:" + ByteArray.GetHexTimeStampString(record.RecordBody.Timestamp));
                    browse.AppendLine("Metadata:" + record.RecordBody.Metadata);
                    browse.AppendLine("xml:" + record.RecordBody.Xml);
                }

                browse.AppendLine("\r\n=================");


            }//end foreach

            return browse.ToString();


        }

        public static string GetServerResultInfo(LibraryServerResult result)
        {
            return "Value:" + result.Value + "\r\n"
                + "ErrorCode:" + result.ErrorCode + "\r\n"
                + "ErrorInfo:" + result.ErrorInfo + "\r\n";
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
            byte[] result = null;
            //try
            //{
            result = client.UploadData(GetRestfulApiUrl("logout"),
                "POST",
                data);
            //}
            //catch (WebException ex)
            //{
            //    //System.Net.WebException:“远程服务器返回错误: (404) 未找到。”
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}

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

        #region getuser , setuser

        // 获得用户
        // parameters:
        //      strAction   暂未使用。保持空即可。
        //      strName     用户名。如果为空，表示希望列出全部用户信息
        // return:
        //      result.Value    -1 错误; 其他 返回总结果数量
        public GetUserResponse GetUser(
            string strAction,
            string strName,
            int nStart,
            int nCount)
        {
        REDO:
            CookieAwareWebClient client = this.GetClient();

            // 设置接口参数
            GetUserRequest request = new GetUserRequest();
            request.strAction = strAction;
            request.strName = strName;
            request.nStart = nStart;
            request.nCount = nCount;

            // 将参数序列号，转成二进制
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

            // 调接口
            byte[] result = client.UploadData(this.GetRestfulApiUrl("GetUser"),
                                                "POST",
                                                baData);
            string strResult = Encoding.UTF8.GetString(result);

            // 返回值序列号为对象
            GetUserResponse response = Deserialize<GetUserResponse>(strResult);

            // 未登录时，按需登录
            if (response.GetUserResult.Value == -1
                && response.GetUserResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                // 把按需登录的错误信息包括进去
                if (string.IsNullOrEmpty(strError) == false)
                {
                    response.GetUserResult.ErrorInfo += "\r\n" + strError;
                }
            }

            return response;
        }

        // 修改用户
        // parameters:
        //      strAction   new delete change resetpassword
        //              当 action 为 "change" 时，如果要在修改其他信息的同时修改密码，info.SetPassword必须为true；
        //              而当action为"resetpassword"时，则info.ResetPassword状态不起作用，无论怎样都要修改密码。resetpassword并不修改其他信息，也就是说info中除了Password/UserName以外其他成员的值无效。
        //              当 action 为 "changeandclose" 时，效果同 "change"，只是最后还要自动切断此用户的 session
        // return:
        //      result.Value    -1 错误
        public SetUserResponse SetUser(
            string strAction,
            UserInfo info)
        {
        REDO:
            CookieAwareWebClient client = this.GetClient();

            // 设置接口参数
            SetUserRequest request = new SetUserRequest();
            request.strAction = strAction;
            request.info = info;

            // 将参数序列号，转成二进制
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

            // 调接口
            byte[] result = client.UploadData(this.GetRestfulApiUrl("SetUser"),
                                                "POST",
                                                baData);
            string strResult = Encoding.UTF8.GetString(result);

            // 返回值序列号为对象
            SetUserResponse response = Deserialize<SetUserResponse>(strResult);

            // 未登录时，按需登录
            if (response.SetUserResult.Value == -1
                && response.SetUserResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                // 把按需登录的错误信息包括进去
                if (string.IsNullOrEmpty(strError) == false)
                {
                    response.SetUserResult.ErrorInfo += "\r\n" + strError;
                }
            }

            return response;
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

        //public GetSystemParamete

        // 2021/3/1
        // 获取一条记录
        public GetRecordResponse GetRecord(string strPath)
        {
            string strError = "";

        REDO:

            CookieAwareWebClient client = this.GetClient();

            GetRecordRequest request = new GetRecordRequest();
            request.strPath = strPath;

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
        public ReservationResponse Reservation(string strFunction,
            string strReaderBarcode,
            string strItemBarcodeList)
        {
            string strError = "";

        REDO:

            CookieAwareWebClient client = this.GetClient();


            ReservationRequest request = new ReservationRequest();
            request.strFunction = strFunction;
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


        public RepairBorrowInfoResponse RepairBorrowInfo(
string strAction,
string strReaderBarcode,
string strItemBarcode,
string strConfirmItemRecPath,
int nStart,   // 2008/10/27
int nCount)
        {
        REDO:
            CookieAwareWebClient client = this.GetClient();

            // 设置接口参数
            RepairBorrowInfoRequest request = new RepairBorrowInfoRequest();
            request.strAction = strAction;
            request.strReaderBarcode = strReaderBarcode;
            request.strItemBarcode = strItemBarcode;
            request.strConfirmItemRecPath = strConfirmItemRecPath;
            request.nStart = nStart;
            request.nCount = nCount;



            // 将参数序列号，转成二进制
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

            // 调接口
            byte[] result = client.UploadData(this.GetRestfulApiUrl("RepairBorrowInfo"),
                                                "POST",
                                                baData);
            string strResult = Encoding.UTF8.GetString(result);

            // 返回值序列号为对象
            RepairBorrowInfoResponse response = Deserialize<RepairBorrowInfoResponse>(strResult);

            // 未登录时，按需登录
            if (response.RepairBorrowInfoResult.Value == -1
                && response.RepairBorrowInfoResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                // 把按需登录的错误信息包括进去
                if (string.IsNullOrEmpty(strError) == false)
                {
                    response.RepairBorrowInfoResult.ErrorInfo += "\r\n" + strError;
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

        // 移动读者
        public VerifyBarcodeResponse VerifyBarcode(
            string strAction,
            string strLibraryCode,
            string strBarcode)
        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();


                VerifyBarcodeRequest request = new VerifyBarcodeRequest();
                request.strAction = strAction;
                request.strLibraryCode = strLibraryCode;
                request.strBarcode = strBarcode;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("VerifyBarcode"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                VerifyBarcodeResponse response = Deserialize<VerifyBarcodeResponse>(strResult);
                if (response.VerifyBarcodeResult != null)
                {
                    if (response.VerifyBarcodeResult.Value == -1
                        && response.VerifyBarcodeResult.ErrorCode == ErrorCode.NotLogin)
                    {
                        if (DoNotLogin(ref strError) == 1)
                            goto REDO;

                        return response;
                    }

                }
                this.ClearRedoCount();

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                goto REDO;
            }

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
        public SearchReaderResponse SearchReader(string strReaderDbNames,
            string strQueryWord,
            int nPerMax,
            string strFrom,
            string strMatchStyle,
            string strLang,
            string strResultSetName,
            string strOutputStyle)
        {
            string strError = "";

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
            if (response.SearchReaderResult.Value == -1
                && response.SearchReaderResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;

                return response;
            }

            return response;//.SearchReaderResult.Value;
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

        public GetOperLogResponse GetOperLog(string strFileName,
long lIndex,
long lHint,
string strStyle,
string strFilter,
long lAttachmentFragmentStart,
int nAttachmentFragmentLength)
        {
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

            {
                string strError = response.GetOperLogResult.ErrorInfo;
                if (response.GetOperLogResult.Value == -1
                    && response.GetOperLogResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return new GetOperLogResponse
                    {
                        GetOperLogResult = new LibraryServerResult
                        {
                            Value = -1,
                            ErrorInfo = strError,
                            ErrorCode = ErrorCode.SystemError,
                        },
                        strXml = null,
                    };
                }
            }

            return response;
        }

        public LibraryServerResult GetOperLog(string strFileName,
    long lIndex,
    long lHint,
    string strStyle,
    string strFilter,
    out string strXml,
    out long lHintNext,
    long lAttachmentFragmentStart,
    int nAttachmentFragmentLength,
    out byte[] attachment_data,
    out long lAttachmentTotalLength)
        {
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

            {
                string strError = response.GetOperLogResult.ErrorInfo;
                if (response.GetOperLogResult.Value == -1
                    && response.GetOperLogResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return new LibraryServerResult
                    {
                        Value = -1,
                        ErrorInfo = strError,
                        ErrorCode = ErrorCode.SystemError,
                    };
                }
            }

            // 给返回值赋值
            strXml = response.strXml;
            lHintNext = response.lHintNext;
            attachment_data = response.attachment_data;
            lAttachmentTotalLength = response.lAttachmentTotalLength;

            return response.GetOperLogResult;
        }


        public long GetOperLogs(
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


        // 20250218增加
        public GetOperLogsResponse GetOperLogs(
    string strFileName,
    long lIndex,
    long lHint,
    int nCount,
    string strStyle,
    string strFilter)
        {


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

            string strError = response.GetOperLogsResult.ErrorInfo;

            if (response.GetOperLogsResult.Value == -1
                && response.GetOperLogsResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
            }


            return response;
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


        // 更原始的接口，返回GetSearchResultResponse
        public GetSearchResultResponse GetSearchResult(
            string strResultSetName,
            long lStart,
            long lCount,
            string strBrowseInfoStyle,
            string strLang)
        {

            if (string.IsNullOrEmpty(strLang) == true)
                strLang = "zh";//strLang;

            GetSearchResultResponse response = null;

            string strError = "";
        REDO:
            try
            {
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
                response = Deserialize<GetSearchResultResponse>(strResult);

                if (response.GetSearchResultResult.Value == -1
                    && response.GetSearchResultResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    return response;
                }

                //???
                this.ClearRedoCount();

                return response;//.GetSearchResultResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

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
                    strError = "ErrorCode:" + response.GetReaderInfoResult.ErrorCode + "--ErrorInfo:" + response.GetReaderInfoResult.ErrorInfo;
                    return -1;
                }
                else if (response.GetReaderInfoResult.Value == 0)
                {
                    strError = "获取读者记录'" + strBarcode + "'未命中。";// + readerRet.GetReaderInfoResult.ErrorInfo;
                    return -1;
                }
                else if (response.GetReaderInfoResult.Value > 1)
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


        public GetReaderInfoResponse GetReaderInfo(string strBarcode,
            string strResultTypeList)
        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

                GetReaderInfoRequest request = new GetReaderInfoRequest();
                request.strBarcode = strBarcode;
                request.strResultTypeList = strResultTypeList;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("getreaderinfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);
                GetReaderInfoResponse response = Deserialize<GetReaderInfoResponse>(strResult);
                if (response.GetReaderInfoResult.Value == -1
                    && response.GetReaderInfoResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                // 网络问题重做
                goto REDO; ;
            }
        }


        // 修改读者密码
        //		工作人员或者读者，必须有changereaderpassword权限
        //		如果为读者, 附加限制还只能修改属于自己的密码
        //      2021/7/5
        //      工作人员身份调用本 API 修改读者密码，应先用工作人员身份登录。而读者调用本 API 修改自己的密码不需要先登录
        //      如果调用本 API 前已经用读者身份登录过了，并且打算调用本 API 修改一个不是登录者自己的读者的密码，那需要先 Logout() 再调用本 API 才行
        // parameters:
        //      strReaderOldPassword    旧密码。如果想达到不验证旧密码的效果，可以用 null 调用，但仅限工作人员身份调用的情况。读者身份是必须验证旧密码的
        // Result.Value
        //      -1  出错
        //      0   旧密码不正确
        //      1   旧密码正确,已修改为新密码
        // 权限: 
        //		工作人员或者读者，必须有changereaderpassword权限
        //		如果为读者, 附加限制还只能修改属于自己的密码
        // 日志:
        //      要产生日志
        public ChangeReaderPasswordResponse ChangeReaderPassword(string strReaderBarcode,
            string strReaderOldPassword,
            string strReaderNewPassword)
        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

                ChangeReaderPasswordRequest request = new ChangeReaderPasswordRequest();
                request.strReaderBarcode = strReaderBarcode;
                request.strReaderOldPassword = strReaderOldPassword;
                request.strReaderNewPassword = strReaderNewPassword;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("ChangeReaderPassword"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);
                ChangeReaderPasswordResponse response = Deserialize<ChangeReaderPasswordResponse>(strResult);
                //if (response.ChangeReaderPasswordResult.Value == -1
                //     && response.ChangeReaderPasswordResult.ErrorCode == ErrorCode.NotLogin)
                //{
                //    if (DoNotLogin(ref strError) == 1)
                //        goto REDO;
                //}
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                // 网络问题重做
                goto REDO; ;
            }
        }


        public ResetPasswordResponse ResetPassword(string strParameters,
            string strMessageTemplate)
        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

                ResetPasswordRequest request = new ResetPasswordRequest();
                request.strParameters = strParameters;
                request.strMessageTemplate = strMessageTemplate;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("ResetPassword"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);
                ResetPasswordResponse response = Deserialize<ResetPasswordResponse>(strResult);
                //if (response.ResetPasswordResult.Value == -1
                //     && response.ResetPasswordResult.ErrorCode == ErrorCode.NotLogin)
                //{
                //    if (DoNotLogin(ref strError) == 1)
                //        goto REDO;
                //}
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                // 网络问题重做
                goto REDO; ;
            }
        }


        // 修改工作人员密码，需要提供老密码
        public ChangeUserPasswordResponse ChangeUserPassword(string strUserName,
            string strOldPassword,
            string strNewPassword)
        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

                ChangeUserPasswordRequest request = new ChangeUserPasswordRequest();
                request.strUserName = strUserName;
                request.strOldPassword = strOldPassword;
                request.strNewPassword = strNewPassword;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("ChangeUserPassword"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);
                ChangeUserPasswordResponse response = Deserialize<ChangeUserPasswordResponse>(strResult);
                //if (response.ChangeUserPasswordResult.Value == -1
                //     && response.ChangeUserPasswordResult.ErrorCode == ErrorCode.NotLogin)
                //{
                //    if (DoNotLogin(ref strError) == 1)
                //        goto REDO;
                //}
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                // 网络问题重做
                goto REDO; ;
            }
        }

        public GetItemInfoResponse GetItemInfo(string strItemDbType,
             string strBarcode,
             string strItemXml,
             string strResultType,
             string strBiblioType)
        {
            GetItemInfoResponse response = null;
            string strError = "";

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

                GetItemInfoRequest request = new GetItemInfoRequest()
                {
                    strItemDbType = strItemDbType,
                    strBarcode = strBarcode,
                    strItemXml = strItemXml,
                    strResultType = strResultType,
                    strBiblioType = strBiblioType,
                };
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("getiteminfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                response = Deserialize<GetItemInfoResponse>(strResult);

                if (response.GetItemInfoResult.Value == -1
                    && response.GetItemInfoResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }

                return response;
            }
            catch (Exception ex)
            {
                // 检查是不是网络原因
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                // 网络原因的话，重试一下
                goto REDO;
            }

        }


        /// <summary>
        /// 获得实体记录信息和其从属的书目记录信息
        /// </summary>
        /// <param name="barcode">册条码</param>
        /// <param name="resultType">希望在strResult参数中返回何种格式的信息，xml/html/text之一</param>
        /// <param name="biblioType">希望返回的书目信息类型，xml/html/text之一</param>
        /// <returns></returns>
        public int GetItemInfo(string strBarcode,
             string strResultType,
             string strBiblioType,
            out string itemXml,
            out string biblio,
            out string strError)
        {
            strError = "";
            itemXml = "";
            biblio = "";


            GetItemInfoResponse response = this.GetItemInfo("",
                strBarcode,
                "",
                strResultType,
                strBiblioType);
            if (response.GetItemInfoResult.Value == -1)
            {
                strError = "获取册记录'" + strBarcode + "'出错:" + response.GetItemInfoResult.ErrorInfo;
                return -1;
            }
            else if (response.GetItemInfoResult.Value == 0)
            {
                strError = "册'" + strBarcode + "'对应的册记录不存在。";
                return -1;
            }
            else if (response.GetItemInfoResult.Value > 1)
            {
                strError = "册条码'" + strBarcode + "'对应的册记录存在多条，数据异常，请联系管理员";
                return -1;
            }

            itemXml = response.strResult;
            biblio = response.strBiblio;
            return (int)response.GetItemInfoResult.Value;


            /*

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

                GetItemInfoRequest request = new GetItemInfoRequest()
                {
                    strBarcode = strBarcode,
                    strResultType = strResultType,
                    strBiblioType = strBiblioType,
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
            */
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
        //public long SetReaderInfoForWeiXin(
        //    string strRecPath,
        //    string strNewXml,
        //    string strOldTimestamp,
        //    out string strError)
        //{
        //    string strExistingXml = "";
        //    string strSavedXml = "";
        //    string strSavedRecPath = "";
        //    byte[] baNewTimestamp = null;

        //    return this.SetReaderInfo("change",
        //        strRecPath,
        //        strNewXml,
        //        "",
        //        strOldTimestamp,
        //        out strExistingXml,
        //        out strSavedXml,
        //        out strSavedRecPath,
        //        out baNewTimestamp,
        //        out strError);
        //}


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
        public SetReaderInfoResponse SetReaderInfo(
            string strAction,
            string strRecPath,
            string strNewXml,
            string strOldXml,
            byte[] baOldTimestamp,
            string strStyle)
        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();


                SetReaderInfoRequest request = new SetReaderInfoRequest();
                request.strAction = strAction;
                request.strRecPath = strRecPath;
                request.strNewXml = strNewXml;
                request.strOldXml = strOldXml;
                request.baOldTimestamp = baOldTimestamp;// ByteArray.GetTimeStampByteArray(strOldTimestamp);
                request.strStyle = strStyle;
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

                        return response;
                    }

                }
                this.ClearRedoCount();

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                goto REDO;
            }

        }

        // 移动读者
        public MoveReaderInfoResponse MoveReaderInfo(
            string strSourceRecPath,
            string strTargetRecPath,
            string strNewReader,
            string strStyle)
        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();


                MoveReaderInfoRequest request = new MoveReaderInfoRequest();
                request.strSourceRecPath = strSourceRecPath;
                request.strTargetRecPath = strTargetRecPath;
                request.strNewReader = strNewReader;
                request.strStyle = strStyle;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("movereaderinfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                MoveReaderInfoResponse response = Deserialize<MoveReaderInfoResponse>(strResult);
                if (response.MoveReaderInfoResult != null)
                {
                    if (response.MoveReaderInfoResult.Value == -1
                        && response.MoveReaderInfoResult.ErrorCode == ErrorCode.NotLogin)
                    {
                        if (DoNotLogin(ref strError) == 1)
                            goto REDO;

                        return response;
                    }

                }
                this.ClearRedoCount();

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                goto REDO;
            }

        }

        /*
        // 写册/订购/评注/期 记录
        public SetItemInfoResponse SetItemInfo(
    string strAction,
    string strRecPath,
    string strXml,
    byte[] item_timestamp,
    string strStyle)
        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();


                SetItemInfoRequest request = new SetItemInfoRequest();
                request.strAction = strAction;
                request.strRecPath = strRecPath;
                request.strXml = strXml;
                request.item_timestamp = item_timestamp;
                request.strStyle = strStyle;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("SetItemInfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                SetItemInfoResponse response = Deserialize<SetItemInfoResponse>(strResult);
                if (response.SetItemInfoResult != null)
                {
                    if (response.SetItemInfoResult.Value == -1
                        && response.SetItemInfoResult.ErrorCode == ErrorCode.NotLogin)
                    {
                        if (DoNotLogin(ref strError) == 1)
                            goto REDO;

                        return response;
                    }

                }
                this.ClearRedoCount();

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                goto REDO;
            }

        }
        */
        public SetBiblioInfoResponse SetBiblioInfo(
            string strAction,
            string strBiblioRecPath,
            string strBiblioType,
            string strBiblio,
            byte[] baTimestamp,
            string strComment,
            string strStyle)
        {
            string strError = "";
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

                        return response;
                    }

                }

                this.ClearRedoCount();
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                goto REDO;
            }

        }

        public CopyBiblioInfoResponse CopyBiblioInfo(
         string strAction,
         string strBiblioRecPath,
         string strBiblioType,
         string strBiblio,
         byte[] baTimestamp,
         string strNewBiblioRecPath,
         string strNewBiblio,
         string strMergeStyle)

        {
            string strError = "";
        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                CopyBiblioInfoRequest request = new CopyBiblioInfoRequest();
                request.strAction = strAction;
                request.strBiblioRecPath = strBiblioRecPath;
                request.strBiblioType = strBiblioType;
                request.strBiblio = strBiblio;
                request.baTimestamp = baTimestamp;// ByteArray.GetTimeStampByteArray(strOldTimestamp);
                request.strNewBiblioRecPath = strNewBiblioRecPath;
                request.strNewBiblio = strNewBiblio;
                request.strMergeStyle = strMergeStyle;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                byte[] result = client.UploadData(this.GetRestfulApiUrl("CopyBiblioInfo"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                CopyBiblioInfoResponse response = Deserialize<CopyBiblioInfoResponse>(strResult);
                if (response.CopyBiblioInfoResult != null)
                {
                    if (response.CopyBiblioInfoResult.Value == -1
                        && response.CopyBiblioInfoResult.ErrorCode == ErrorCode.NotLogin)
                    {
                        if (DoNotLogin(ref strError) == 1)
                            goto REDO;

                        return response;
                    }

                }

                this.ClearRedoCount();
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

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


        public BorrowResponse Borrow(BorrowRequest request)
        {
            string strError = "";

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

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
                    return response;

                }

                this.ClearRedoCount();
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

                goto REDO;
            }
        }

        public ReturnResponse Return(ReturnRequest request)
        {
            string strError = "";

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(this.GetRestfulApiUrl("Return"),
                    "POST",
                    baData);

                string strResult = Encoding.UTF8.GetString(result);

                ReturnResponse response = Deserialize<ReturnResponse>(strResult);
                if (response.ReturnResult.Value == -1
                    && response.ReturnResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                    return response;

                }

                this.ClearRedoCount();
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    throw ex;

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
        public SearchBiblioResponse SearchBiblio(
            string strBiblioDbNames,
            string strQueryWord,
            int nPerMax,
            string strFromStyle,
            string strMatchStyle,
            string strLang,
            string strResultSetName,
            string strSearchStyle,
             string strOutputStyle,
             string strLocationFilter)//,
                                      //out string strQueryXml,
                                      //out string strError)
        {
            string strError = "";
            //strQueryXml = "";
            SearchBiblioResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();

                

                SearchBiblioRequest request = new SearchBiblioRequest();
                request.strBiblioDbNames = strBiblioDbNames;
                request.strQueryWord = strQueryWord;
                request.nPerMax = nPerMax;
                request.strFromStyle = strFromStyle;
                request.strMatchStyle = strMatchStyle;

                request.strLang = strLang;// "zh";
                request.strResultSetName = strResultSetName;
                request.strSearchStyle = strSearchStyle;// "desc";
                request.strOutputStyle = strOutputStyle;

                request.strLocationFilter = strLocationFilter;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                string strRequest = Encoding.UTF8.GetString(baData);

                byte[] result = client.UploadData(this.GetRestfulApiUrl("SearchBiblio"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<SearchBiblioResponse>(strResult);

                // 未登录的情况
                if (response.SearchBiblioResult.Value == -1
                    && response.SearchBiblioResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    return response;
                }

                //strQueryXml = response.strQueryXml;
                //strError = response.SearchBiblioResult.ErrorInfo;
                this.ClearRedoCount();

                return response;//.SearchBiblioResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

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
        public SearchItemResponse SearchItem(string strItemDbName,
            string strQueryWord,
            int nPerMax,
            string strFrom,
            string strMatchStyle,
            string strResultSetName,
            string strSearchStyle,
             string strOutputStyle)//,
                                   //out string strError)
        {
            string strError = "";

            SearchItemResponse response = null;

        REDO:
            try
            {
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
                response = Deserialize<SearchItemResponse>(strResult);

                // 未登录的情况
                if (response.SearchItemResult.Value == -1
                    && response.SearchItemResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    return response;
                }

                //strError = response.SearchItemResult.ErrorInfo;
                //// this.ErrorCode = response.SearchBiblioResult.ErrorCode;

                //???
                this.ClearRedoCount();

                return response;//.SearchItemResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;


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

        public SetItemInfoResponse SetItemInfo(
                 string strDbType,
                string strAction,
                string newRecPath,
                string newRecord,
                string oldRecPath,
                string oldRecord,
                byte[] baTimestamp,
                string strStyle,
                out string strOutputRecPath,
                out byte[] baOutputTimestamp)
        {
            strOutputRecPath = "";
            baOutputTimestamp = null;

            //LibraryServerResult result = new LibraryServerResult();
            string strError = "";

            SetItemInfoResponse r = new SetItemInfoResponse();
            //r.SetItemInfoResult1 = result;
            

            var entity = new EntityInfo();
            entity.Action = strAction;
            entity.Style= strStyle;
            //if (strAction == "delete")
            //{
            //    entity.OldRecPath = strResPath;
            //}
            //else
            //{
            //    entity.NewRecPath = strResPath;
            //}
            entity.OldRecPath=oldRecPath;
            entity.NewRecPath = newRecPath;


            // xml记录体
            //entity.NewRecord = strXml;
            entity.OldRecord = oldRecord;
            entity.NewRecord = newRecord;

            // 时间戳
            //if (strAction == "change" || strAction == "delete")
            entity.OldTimestamp = baTimestamp;
            //else
            //    entity.NewTimestamp = baTimestamp;  //2024/5/13 听开发老师讲，new时，也是把时间戳传到OldTimestamp参数。

            /*
            if (strAction == "change")
                entity.OldRecPath = strRecPath;
            */

            EntityInfo[] errorinfos = null;

            if (strDbType == "item" || strDbType == "entity")
            {
                SetEntitiesResponse ret = SetEntities(
                     "", // strBiblioRecPath,
                     new EntityInfo[] { entity });

                errorinfos = ret.errorinfos;
                r.SetItemInfoResult = ret.SetEntitiesResult;
            }
            else if (strDbType == "order")
            {
                SetOrdersResponse ret = this.SetOrders(
                     "", // strBiblioRecPath,
                     new EntityInfo[] { entity });

                errorinfos = ret.errorinfos;
                r.SetItemInfoResult = ret.SetOrdersResult;
            }
            else if (strDbType == "issue")
            {
                SetIssuesResponse ret = this.SetIssues(
                     "", // strBiblioRecPath,
                     new EntityInfo[] { entity });

                errorinfos = ret.errorinfos;
                r.SetItemInfoResult = ret.SetIssuesResult;
            }
            else if (strDbType == "comment")
            {
                SetCommentsResponse ret = this.SetComments(
                     "", // strBiblioRecPath,
                     new EntityInfo[] { entity });

                errorinfos = ret.errorinfos;
                r.SetItemInfoResult = ret.SetCommentsResult;
            }
            else
            {
                r.SetItemInfoResult.Value = -1;
                r.SetItemInfoResult.ErrorInfo = $"无法识别的数据库类型 '{strDbType}'";
                r.SetItemInfoResult.ErrorCode = ErrorCode.SystemError;
                return r;
            }


            // 取第1个成员
            if (errorinfos != null && errorinfos.Length > 0)
            {
                //foreach (var error in errorinfos)
                var error = errorinfos[0];
                r.OneErrorCode = error.ErrorCode;
                r.OneErrorInfo = error.ErrorInfo;
                strOutputRecPath = error.NewRecPath;

                if (error.ErrorCode == ErrorCodeValue.TimestampMismatch)
                {
                    baOutputTimestamp = error.OldTimestamp;
                }
                else
                    baOutputTimestamp = error.NewTimestamp;
            }

            return r;
        }

        //// 把内核错误码转换为 dp2library 错误码
        //public static ErrorCodeValue FromErrorValue(ErrorCodeValue error_code,
        //    bool throw_exception = false)
        //{
        //    string text = error_code.ToString();
        //    if (Enum.TryParse<ErrorCodeValue>(text, out ErrorCodeValue result) == false)
        //    {
        //        if (throw_exception == true)
        //            throw new Exception("无法将字符串 '" + text + "' 转换为 LibraryServer.ErrorCode 类型");
        //        else
        //            return ErrorCode.SystemError;
        //    }
        //    return result;
        //}

        // 设置/保存册信息
        // parameters:
        //      strBiblioRecPath    书目记录路径，仅包含库名和id部分
        //      entityinfos 要提交的的实体信息数组
        // 权限：需要有setiteminfo 权限(兼容setentities权限) 或 writerecord 权限
        // 日志：
        //      要产生日志
        public SetEntitiesResponse SetEntities(
            string strBiblioRecPath,
            EntityInfo[] entityinfos)
        {
            SetEntitiesResponse response = null;

            string strError = "";

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                SetEntitiesRequest request = new SetEntitiesRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.entityinfos = entityinfos;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));


                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("SetEntities"),
                                "POST",
                                 baData);


                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<SetEntitiesResponse>(strResult);

                // 未登录的情况
                if (response.SetEntitiesResult.Value == -1
                    && response.SetEntitiesResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }

        }


        public SetOrdersResponse SetOrders(
            string strBiblioRecPath,
            EntityInfo[] orderinfos)
        {
            SetOrdersResponse response = null;

            string strError = "";

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                SetOrdersRequest request = new SetOrdersRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.orderinfos = orderinfos;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));


                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("SetOrders"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<SetOrdersResponse>(strResult);

                // 未登录的情况
                if (response.SetOrdersResult.Value == -1
                    && response.SetOrdersResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }

        }

        public SetCommentsResponse SetComments(
            string strBiblioRecPath,
            EntityInfo[] commentinfos)
        {
            SetCommentsResponse response = null;

            string strError = "";

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                SetCommentsRequest request = new SetCommentsRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.commentinfos = commentinfos;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));


                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("SetComments"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<SetCommentsResponse>(strResult);

                // 未登录的情况
                if (response.SetCommentsResult.Value == -1
                    && response.SetCommentsResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }
        }

        public SetIssuesResponse SetIssues(
                string strBiblioRecPath,
                EntityInfo[] issueinfos)
        {
            SetIssuesResponse response = null;

            string strError = "";

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                SetIssuesRequest request = new SetIssuesRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.issueinfos = issueinfos;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));


                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("SetIssues"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<SetIssuesResponse>(strResult);

                // 未登录的情况
                if (response.SetIssuesResult.Value == -1
                    && response.SetIssuesResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

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
        public GetEntitiesResponse GetEntities(string strBiblioRecPath,
            long lStart,
            long lCount,
            string strStyle,
            string strLang)
        {
            string strError = "";
            GetEntitiesResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                GetEntitiesRequest request = new GetEntitiesRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.lStart = lStart;
                request.lCount = lCount;
                request.strStyle = strStyle; //"";// "onlygetpath";//strStyle;
                request.strLang = strLang;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("GetEntities"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<GetEntitiesResponse>(strResult);

                // 未登录的情况
                if (response.GetEntitiesResult.Value == -1
                    && response.GetEntitiesResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }
        }


        public GetOrdersResponse GetOrders(string strBiblioRecPath,
            long lStart,
            long lCount,
            string strStyle,
            string strLang)
        {
            string strError = "";
            GetOrdersResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                GetOrdersRequest request = new GetOrdersRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.lStart = lStart;
                request.lCount = lCount;
                request.strStyle = strStyle;
                request.strLang = strLang;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("GetOrders"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<GetOrdersResponse>(strResult);

                // 未登录的情况
                if (response.GetOrdersResult.Value == -1 && response.GetOrdersResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }
        }


        public GetIssuesResponse GetIssues(string strBiblioRecPath,
            long lStart,
            long lCount,
            string strStyle,
            string strLang)
        {
            string strError = "";
            GetIssuesResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                GetIssuesRequest request = new GetIssuesRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.lStart = lStart;
                request.lCount = lCount;
                request.strStyle = strStyle;
                request.strLang = strLang;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("GetIssues"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<GetIssuesResponse>(strResult);

                // 未登录的情况
                if (response.GetIssuesResult.Value == -1 && response.GetIssuesResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }
        }


        public GetCommentsResponse GetComments(string strBiblioRecPath,
            long lStart,
            long lCount,
            string strStyle,
            string strLang)
        {
            string strError = "";
            GetCommentsResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();
                GetCommentsRequest request = new GetCommentsRequest();
                request.strBiblioRecPath = strBiblioRecPath;
                request.lStart = lStart;
                request.lCount = lCount;
                request.strStyle = strStyle;
                request.strLang = strLang;
                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));

                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("GetComments"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<GetCommentsResponse>(strResult);

                // 未登录的情况
                if (response.GetCommentsResult.Value == -1 && response.GetCommentsResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

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


        // 获得浏览列
        public GetBrowseRecordsResponse GetBrowseRecords(string[] paths,
            string strBrowseInfoStyle)
        {
            string strError = "";

            GetBrowseRecordsResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();


                GetBrowseRecordsRequest request = new GetBrowseRecordsRequest();
                request.paths = paths;//new string[] { strRecPath };
                request.strBrowseInfoStyle = strBrowseInfoStyle;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("GetBrowseRecords"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<GetBrowseRecordsResponse>(strResult);
                if (response.GetBrowseRecordsResult.Value == -1 && response.GetBrowseRecordsResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    return response;
                }


                this.ClearRedoCount();  //???
                return response;//.GetBrowseRecordsResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }
        }

        // 结算
        public SettlementResponse Settlement(string strAction,
            string[] ids)
        {
            string strError = "";

            SettlementResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();


                SettlementRequest request = new SettlementRequest();
                request.ids = ids;//new string[] { strRecPath };
                request.strAction = strAction;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("Settlement"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<SettlementResponse>(strResult);
                if (response.SettlementResult.Value == -1 && response.SettlementResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    return response;
                }


                this.ClearRedoCount();  //???
                return response;//.GetBrowseRecordsResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }
        }


        // 租金
        public HireResponse Hire(string strAction,
            string strReaderBarcode)
        {
            string strError = "";

            HireResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();


                HireRequest request = new HireRequest();
                request.strAction = strAction;
                request.strReaderBarcode = strReaderBarcode;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("Hire"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<HireResponse>(strResult);
                if (response.HireResult.Value == -1 && response.HireResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    return response;
                }


                this.ClearRedoCount();  //???
                return response;//.GetBrowseRecordsResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

                goto REDO;
            }
        }


        public ForegiftResponse Foregift(string strAction,
    string strReaderBarcode)
        {
            string strError = "";

            ForegiftResponse response = null;

        REDO:
            try
            {
                CookieAwareWebClient client = this.GetClient();


                ForegiftRequest request = new ForegiftRequest();
                request.strAction = strAction;
                request.strReaderBarcode = strReaderBarcode;

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                string strRequest = Encoding.UTF8.GetString(baData);
                byte[] result = client.UploadData(this.GetRestfulApiUrl("Foregift"),
                                "POST",
                                 baData);

                string strResult = Encoding.UTF8.GetString(result);
                response = Deserialize<ForegiftResponse>(strResult);
                if (response.ForegiftResult.Value == -1 && response.ForegiftResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;

                    return response;
                }


                this.ClearRedoCount();  //???
                return response;//.GetBrowseRecordsResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return response;

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

        public int GetSystemParameter(
    // DigitalPlatform.Stop stop,
    string strCategory,
    string strName,
    out string strValue,
    out string strError)
        {

            GetSystemParameterResponse response = this.GetSystemParameter(strCategory,
                strName);
            strValue = response.strValue;
            strError = response.GetSystemParameterResult.ErrorInfo;

            return (int)response.GetSystemParameterResult.Value;
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


        public SearchResponse Search(string strQueryXml,
            string strResultSetName,
            string strOutputStyle)
        {
        REDO:

            CookieAwareWebClient client = this.GetClient();


            SearchRequest request = new SearchRequest()
            {
                strQueryXml = strQueryXml,
                strResultSetName = strResultSetName,
                strOutputStyle = strOutputStyle
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

        public long WriteText(
            string strResPath,
            string strText,
            //bool bInlucdePreamble,  //是否包括utf8的标志
            string strStyle,
            byte[] baInputTimestamp1,
            bool redoWhenTimestampError,  //当时间戳不对时，是否重做。
            out byte[] baOutputTimestamp,
            out string strOutputPath,
            out string strError)
        {
            strError = "";
            strOutputPath = "";
            baOutputTimestamp = null;

            // 小包尺寸
            int nChunkMaxLength = 4096;	// chunk

            int nStart = 0;
            byte[] baTotal = Encoding.UTF8.GetBytes(strText);


            /* 关于增加utf前3个字符
            byte[] baPreamble = Encoding.UTF8.GetPreamble();
            if (bInlucdePreamble == true
                && baPreamble != null && baPreamble.Length > 0)
            {
                byte[] temp = null;
                temp = ByteArray.Add(temp, baPreamble);
                baTotal = ByteArray.Add(temp, baTotal);
            }
            */


            int nTotalLength = baTotal.Length;

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

            REDO:

                WriteResResponse response = WriteRes(strResPath,
                    strRange,
                    nTotalLength,
                    baChunk,
                    strMetadata,
                    strStyle,
                    baInputTimestamp1);
                if (response.WriteResResult.Value == -1)
                {
                    // 间戳不匹配，自动重试
                    if (response.WriteResResult.ErrorCode == ErrorCode.TimestampMismatch
                    && redoWhenTimestampError == true)
                    {
                        baInputTimestamp1 = response.baOutputTimestamp;
                        goto REDO;
                    }

                    strError = response.WriteResResult.ErrorInfo;
                    return -1;
                }

                baOutputTimestamp = response.baOutputTimestamp;
                strOutputPath = response.strOutputResPath;

                nStart += baChunk.Length;

                if (nStart >= nTotalLength)
                    break;

                Debug.Assert(strOutputPath != "", "outputpath不能为空");

                strResPath = strOutputPath;	// 如果第一次的strPath中包含'?'id, 必须用outputpath才能正确继续
                baInputTimestamp1 = baOutputTimestamp;	//baOutputTimeStamp;
            }

            return 0;
        }

        // 把字节数组分块写入文件
        public WriteResResponse WriteResByChunk(
                string strResPath,
                byte[] baContent,
                string strStyle,
                string strMetadata,
                byte[] baInputTimestamp,
                int chunkSize,  //4096
                bool redoWhenTimestampError)  //当时间戳不对时，是否重做。
                                              //out byte[] baOutputTimestamp,
                                              //out string strOutputResPath,
                                              //out string strError)
        {
            //strError = "";
            //strOutputResPath = "";
            //baOutputTimestamp = null;

            WriteResResponse response = null;
            int nStart = 0;
            int nTotalLength = baContent.Length;
            while (true)
            {
                DoIdle();

                int nThisChunkSize = chunkSize;
                if (nThisChunkSize + nStart > nTotalLength)
                {
                    // 剩余尺寸
                    nThisChunkSize = nTotalLength - nStart;	// 最后一次
                    if (nThisChunkSize <= 0)
                        break;
                }

                byte[] baChunk = new byte[nThisChunkSize];
                Array.Copy(baContent, nStart, baChunk, 0, baChunk.Length);

                // 拼range
                string strRange = Convert.ToString(nStart) + "-" + Convert.ToString(nStart + baChunk.Length - 1);

            REDO:

                response = WriteRes(strResPath,
                    strRange,
                    nTotalLength,
                    baChunk,
                    strMetadata,
                    strStyle,
                    baInputTimestamp);
                if (response.WriteResResult.Value == -1)
                {
                    // 第一次间戳不匹配，自动重试
                    if (response.WriteResResult.ErrorCode == ErrorCode.TimestampMismatch
                        && nStart == 0
                        && redoWhenTimestampError == true)
                    {
                        baInputTimestamp = response.baOutputTimestamp;
                        goto REDO;
                    }

                    // 其它错误，直接返回
                    return response;
                    //strError = response.WriteResResult.ErrorInfo;
                    //return -1;
                }

                //// 返回值
                //baOutputTimestamp = response.baOutputTimestamp;
                //strOutputResPath = response.strOutputResPath;

                nStart += baChunk.Length;

                Debug.Assert(response.strOutputResPath != "", "outputpath不能为空");
                strResPath = response.strOutputResPath;	// 如果第一次的strPath中包含'?'id, 必须用outputpath才能正确继续
                baInputTimestamp = response.baOutputTimestamp;	//baOutputTimeStamp;


                if (nStart >= nTotalLength)
                    break;
            }

            return response;
        }

        // 以分块的方式把文件写入服务器
        public WriteResResponse WriteResOfFile(
            string strResPath,
            string fileName,
            string strStyle,
            string strMetadata,
            byte[] baInputTimestamp,
            int chunkSize,  //4096
            bool redoWhenTimestampError)  //当时间戳不对时，是否重做。
                                          //out byte[] baOutputTimestamp,
                                          //out string strOutputPath,
                                          //out string strError)
        {
            //strError = "";
            //strOutputPath = "";
            //baOutputTimestamp = null;

            WriteResResponse response = null;

            using (FileStream s = new FileStream(fileName, FileMode.Open))
            {
                int nStart = 0;

                // 资源总尺寸
                long lTotalLength = s.Length;

                while (s.Position < s.Length)
                {
                    // 与文件剩余尺寸比对，谁小用小
                    long realChunkSize = Math.Min(chunkSize, s.Length - nStart);


                    // 从文件中读出指出尺寸的数据到baContent
                    byte[] baContent = new byte[realChunkSize];
                    int nLength = s.Read(baContent, 0, baContent.Length);

                    // 本次尺寸范围
                    string strRanges = nStart.ToString() + "-" + (nStart + nLength - 1).ToString();

                    // 调WriteRes
                    response = this.WriteRes(strResPath,
                       strRanges,
                       lTotalLength,
                       baContent,
                       strMetadata,  //todo可以在最后一次再传metadata
                       strStyle,
                       baInputTimestamp);
                    if (response.WriteResResult.Value == -1)
                    {
                        // 第一次的时间戳不匹配，根据界面设置，自动重试
                        if (response.WriteResResult.ErrorCode == ErrorCode.TimestampMismatch
                                && nStart == 0
                                && redoWhenTimestampError == true)
                        {
                            baInputTimestamp = response.baOutputTimestamp;
                            s.Position = nStart;
                            continue;
                        }

                        return response;

                        //strError = response.WriteResResult.ErrorInfo;
                        //return -1;
                    }

                    // 下一轮取文件的开始位置
                    nStart += nLength;

                    strResPath = response.strOutputResPath;  //// 如果第一次的strPath中包含'?'id, 必须用outputpath才能正确继续
                    baInputTimestamp = response.baOutputTimestamp;


                    // 返回值
                    //strOutputPath = response.strOutputResPath;
                    //baOutputTimestamp = response.baOutputTimestamp;

                } //end of while

            } //end of using

            return response;
        }

        public GetResResponse GetRes(
            string strResPath,
            long lStart,
            int nLength,
            string strStyle)
        {
        REDO:
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

            strResult = ByteArray.ToString(baTotal, Encoding.UTF8);

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


        #region 创建环境相关函数

        public int SetSystemParameter(
    string strCategory,
    string strName,
    string strValue,
    out string strError)
        {

            strError = "";

        REDO:
            CookieAwareWebClient client = this.GetClient();


            /*
            SetSystemParameterRequest request = new SetSystemParameterRequest()
            {
                strCategory = strCategory,
                strName = strName,
                strValue = strValue
            };
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(GetRestfulApiUrl("SetSystemParameter"),
                "POST",
                baData);

            string strResult = Encoding.UTF8.GetString(result);
            SetSystemParameterResponse response = Deserialize<SetSystemParameterResponse>(strResult);
            if (response.SetSystemParameterResult.Value == -1
                && response.SetSystemParameterResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
            }
            */

            SetSystemParameterResponse response = this.SetSystemParameter(strCategory, strName, strValue);

            if (response != null)
            {
                strError = response.SetSystemParameterResult.ErrorInfo;

                return (int)response.SetSystemParameterResult.Value;
            }
            else
            {
                strError = "response为null";
                return -1;
            }
        }





        public SetSystemParameterResponse SetSystemParameter(string strCategory,
            string strName,
            string strValue)
        {

            string strError = "";
        REDO:

            CookieAwareWebClient client = this.GetClient();


            SetSystemParameterRequest request = new SetSystemParameterRequest()
            {
                strCategory = strCategory,
                strName = strName,
                strValue = strValue
            };
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(GetRestfulApiUrl("SetSystemParameter"),
                "POST",
                baData);

            string strResult = Encoding.UTF8.GetString(result);
            SetSystemParameterResponse response = Deserialize<SetSystemParameterResponse>(strResult);
            if (response.SetSystemParameterResult.Value == -1
                && response.SetSystemParameterResult.ErrorCode == ErrorCode.NotLogin)
            {
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
            }


            return response;
        }



        public long ManageDatabase(string strAction,
                string strDatabaseName,
                string strDatabaseInfo,
               out string strOutputInfo,
               out string strError)
        {
            strError = "";
            strOutputInfo = "";

            ManageDatabaseResponse r = this.ManageDatabase(strAction,
                strDatabaseName,
                strDatabaseInfo,
                "");
            strOutputInfo = r.strOutputInfo;
            strError = r.ManageDatabaseResult.ErrorInfo;

            return r.ManageDatabaseResult.Value;
        }


        // 管理数据库
        /// <summary>
        /// 管理数据库
        /// </summary>
        /// <param name="stop"></param>
        /// <param name="strAction">动作参数</param>
        /// <param name="strDatabaseName">数据库名</param>
        /// <param name="strDatabaseInfo">数据库信息</param>
        /// <param name="strOutputInfo">返回操作后的数据库信息</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0 或 1:    成功</para>
        /// </returns>
        public ManageDatabaseResponse ManageDatabase(string strAction,
            string strDatabaseName,
            string strDatabaseInfo,
            string strStyle)
        {
        //strError = "";
        //strOutputInfo = "";

        REDO:
            try
            {

                CookieAwareWebClient client = this.GetClient();

                ManageDatabaseRequest request = new ManageDatabaseRequest()
                {
                    strAction = strAction,
                    strDatabaseName = strDatabaseName,
                    strDatabaseInfo = strDatabaseInfo,
                    strStyle = strStyle
                };

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(GetRestfulApiUrl("ManageDatabase"),
                        "POST",
                        baData);

                string strResult = Encoding.UTF8.GetString(result);
                ManageDatabaseResponse response = Deserialize<ManageDatabaseResponse>(strResult);
                if (response.ManageDatabaseResult.Value == -1
    && response.ManageDatabaseResult.ErrorCode == ErrorCode.NotLogin)
                {
                    string strError = "";
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out string strError);
                if (nRet == 0)
                    return null;
                goto REDO;
            }


        }




        // 为读者记录绑定新的号码
        // parameters:
        //      strAction   动作。有 bind/unbind
        //      strQueryWord    用于定位读者记录的检索词。
        //          0) 如果以"RI:"开头，表示利用 参考ID 进行检索
        //          1) 如果以"NB:"开头，表示利用姓名生日进行检索。姓名和生日之间间隔以'|'。姓名必须完整，生日为8字符形式
        //          2) 如果以"EM:"开头，表示利用email地址进行检索。注意 email 本身应该是 email:xxxx 这样的形态。也就是说，整个加起来是 EM:email:xxxxx
        //          3) 如果以"TP:"开头，表示利用电话号码进行检索
        //          4) 如果以"ID:"开头，表示利用身份证号进行检索
        //          5) 如果以"CN:"开头，表示利用证件号码进行检索
        //          6) 如果以"UN:"开头，表示利用用户名进行检索，意思就是工作人员的账户名了，不是针对读者绑定
        //          7) 否则用证条码号进行检索
        //      strPassword     读者记录的密码
        //      strBindingID    要绑定的号码。格式如 email:xxxx 或 weixinid:xxxxx
        //      strStyle    风格。multiple/single/singlestrict。默认 single
        //                  multiple 表示允许多次绑定同一类型号码；single 表示同一类型号码只能绑定一次，如果多次绑定以前的同类型号码会被清除; singlestrict 表示如果以前存在同类型号码，本次绑定会是失败
        //                  如果包含 null_password，表示不用读者密码，strPassword 参数无效。但这个功能只能被工作人员使用
        //      strResultTypeList   结果类型数组 xml/html/text/calendar/advancexml/recpaths/summary
        //              其中calendar表示获得读者所关联的日历名；advancexml表示经过运算了的提供了丰富附加信息的xml，例如具有超期和停借期附加信息
        //              advancexml_borrow_bibliosummary/advancexml_overdue_bibliosummary/advancexml_history_bibliosummary
        //      results 返回操作成功后的读者记录
        //public LibraryServerResult BindPatron(
        //    string strAction,
        //    string strQueryWord,
        //    string strPassword,
        //    string strBindingID,
        //    string strStyle,
        //    string strResultTypeList,
        //    out string[] results)
        public BindPatronResponse BindPatron(//string strAction,
    //string strQueryWord,
    //string strPassword,
    //string strBindingID,
    //string strStyle,
    //string strResultTypeList
            BindPatronRequest request
            )
        {
        //strError = "";
        //strOutputInfo = "";

        REDO:
            try
            {

                CookieAwareWebClient client = this.GetClient();

                //BindPatronRequest request = new BindPatronRequest()
                //{
                //    strAction = strAction,
                //    strQueryWord = strQueryWord,
                //    strPassword = strPassword,
                //    strBindingID = strBindingID,
                //    strStyle = strStyle,
                //    strResultTypeList = strResultTypeList
                //};

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(GetRestfulApiUrl("BindPatron"),
                        "POST",
                        baData);

                string strResult = Encoding.UTF8.GetString(result);
                BindPatronResponse response = Deserialize<BindPatronResponse>(strResult);
                if (response.BindPatronResult.Value == -1
    && response.BindPatronResult.ErrorCode == ErrorCode.NotLogin)
                {
                    string strError = "";
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out string strError);
                if (nRet == 0)
                    return null;
                goto REDO;
            }


        }


        /*
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
        DigitalPlatform.Stop stop,
        string strReaderBarcode,
        string strReaderPassword,
        out string strError)
*/
        public VerifyReaderPasswordResponse VerifyReaderPassword(
            VerifyReaderPasswordRequest request)
        {
        //strError = "";
        //strOutputInfo = "";

        REDO:
            try
            {

                CookieAwareWebClient client = this.GetClient();

                //VerifyReaderPasswordRequest request = new VerifyReaderPasswordRequest()
                //{
                //    strReaderBarcode = strReaderBarcode,
                //    strReaderPassword = strReaderPassword
                //};

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(GetRestfulApiUrl("VerifyReaderPassword"),
                        "POST",
                        baData);

                string strResult = Encoding.UTF8.GetString(result);
                VerifyReaderPasswordResponse response = Deserialize<VerifyReaderPasswordResponse>(strResult);
                if (response.VerifyReaderPasswordResult.Value == -1
    && response.VerifyReaderPasswordResult.ErrorCode == ErrorCode.NotLogin)
                {
                    string strError = "";
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }
                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out string strError);
                if (nRet == 0)
                    return null;
                goto REDO;
            }


        }




        // 获得日历
        /// <summary>
        /// 获得流通日历
        /// </summary>
        /// <param name="stop"></param>
        /// <param name="strAction">动作参数</param>
        /// <param name="strName">日历名</param>
        /// <param name="nStart">要获得的元素开始位置。从 0 开始计数</param>
        /// <param name="nCount">要获得的元素数量。若为 -1 表示希望获得尽可能多的元素</param>
        /// <param name="contents">返回的日历信息数组</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>&gt;=0:   结果数量</para>
        /// </returns>
        public GetCalendarResponse GetCalendar(
            // DigitalPlatform.Stop stop,
            string strAction,
            string strName,
            int nStart,
            int nCount)
        {
            string strError = "";

        REDO:
            try
            {

                CookieAwareWebClient client = this.GetClient();

                GetCalendarRequest request = new GetCalendarRequest()
                {
                    strAction = strAction,
                    strName = strName,
                    nStart = nStart,
                    nCount = nCount
                };

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(GetRestfulApiUrl("GetCalendar"),
                        "POST",
                        baData);

                string strResult = Encoding.UTF8.GetString(result);
                GetCalendarResponse response = Deserialize<GetCalendarResponse>(strResult);
                if (response.GetCalendarResult.Value == -1
    && response.GetCalendarResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return null;
                goto REDO;
            }


        }

        // 设置、修改日历
        /// <summary>
        /// 设置流通日历
        /// </summary>
        /// <param name="stop"></param>
        /// <param name="strAction">动作参数</param>
        /// <param name="info">日历信息</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        /// <para>-1:   出错</para>
        /// <para>0:    成功</para>
        /// </returns>
        public long SetCalendar(
    // DigitalPlatform.Stop stop,
    string strAction,
    CalenderInfo info,
    out string strError)
        {
            strError = "";


        REDO:
            try
            {

                CookieAwareWebClient client = this.GetClient();

                SetCalendarRequest request = new SetCalendarRequest()
                {
                    strAction = strAction,
                    info = info
                };

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(GetRestfulApiUrl("SetCalendar"),
                        "POST",
                        baData);

                string strResult = Encoding.UTF8.GetString(result);
                SetCalendarResponse response = Deserialize<SetCalendarResponse>(strResult);
                if (response.SetCalendarResult.Value == -1
    && response.SetCalendarResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }

                strError = response.SetCalendarResult.ErrorInfo;
                return response.SetCalendarResult.Value;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return -1;
                goto REDO;
            }

        }


        public SetCalendarResponse SetCalendar(
// DigitalPlatform.Stop stop,
string strAction,
CalenderInfo info)
        {
            string strError = "";


        REDO:
            try
            {

                CookieAwareWebClient client = this.GetClient();

                SetCalendarRequest request = new SetCalendarRequest()
                {
                    strAction = strAction,
                    info = info
                };

                byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
                byte[] result = client.UploadData(GetRestfulApiUrl("SetCalendar"),
                        "POST",
                        baData);

                string strResult = Encoding.UTF8.GetString(result);
                SetCalendarResponse response = Deserialize<SetCalendarResponse>(strResult);
                if (response.SetCalendarResult.Value == -1
    && response.SetCalendarResult.ErrorCode == ErrorCode.NotLogin)
                {
                    if (DoNotLogin(ref strError) == 1)
                        goto REDO;
                }

                return response;
            }
            catch (Exception ex)
            {
                int nRet = ConvertWebError(ex, out strError);
                if (nRet == 0)
                    return null;
                goto REDO;
            }

        }

        #endregion


        // 批处理任务
        /// <summary>
        /// 批处理任务
        /// </summary>
        /// <param name="stop"></param>
        /// <param name="strName">批处理任务名</param>
        /// <param name="strAction">动作参数</param>
        /// <param name="info">任务信息</param>
        /// <param name="resultInfo">返回任务信息</param>
        /// <param name="strError">返回出错信息</param>
        /// <returns>
        ///// <para>-1:   出错</para>
        ///// <para>0 或 1:    成功</para>
        ///// </returns>
        //public long BatchTask1(
        //    DigitalPlatform.Stop stop,
        //    string strName,
        //    string strAction,
        //    BatchTaskInfo info,
        //    out BatchTaskInfo resultInfo,
        //    out string strError)
        //{
        //    strError = "";
        //    resultInfo = null;

        //REDO:
        //    try
        //    {
        //        IAsyncResult soapresult = this.ws.BeginBatchTask(
        //            strName,
        //            strAction,
        //            info,
        //            null,
        //            null);

        //        WaitComplete(soapresult);

        //        if (this.m_ws == null)
        //        {
        //            strError = "用户中断";
        //            this.ErrorCode = localhost.ErrorCode.RequestCanceled;
        //            return -1;
        //        }

        //        LibraryServerResult result = this.ws.EndBatchTask(
        //            out resultInfo,
        //            soapresult);
        //        if (result.Value == -1 && result.ErrorCode == ErrorCode.NotLogin)
        //        {
        //            if (DoNotLogin(ref strError) == 1)
        //                goto REDO;
        //            return -1;
        //        }
        //        strError = result.ErrorInfo;
        //        this.ErrorCode = result.ErrorCode;
        //        this.ClearRedoCount();
        //        return result.Value;
        //    }
        //    catch (Exception ex)
        //    {
        //        int nRet = ConvertWebError(ex, out strError);
        //        if (nRet == 0)
        //            return -1;
        //        goto REDO;
        //    }
        //}


        public BatchTaskResponse BatchTask(string strName,
            string strAction,
            BatchTaskInfo info)
        {
        REDO:

            CookieAwareWebClient client = this.GetClient();


            BatchTaskRequest request = new BatchTaskRequest()
            {
                strName = strName,
                strAction = strAction,
                info = info
            };
            byte[] baData = Encoding.UTF8.GetBytes(Serialize(request));
            byte[] result = client.UploadData(GetRestfulApiUrl("BatchTask"),
                "POST",
                baData);

            string strResult = Encoding.UTF8.GetString(result);
            BatchTaskResponse response = Deserialize<BatchTaskResponse>(strResult);
            if (response.BatchTaskResult.Value == -1
                && response.BatchTaskResult.ErrorCode == ErrorCode.NotLogin)
            {
                string strError = "";
                if (DoNotLogin(ref strError) == 1)
                    goto REDO;
            }

            return response;
        }

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
