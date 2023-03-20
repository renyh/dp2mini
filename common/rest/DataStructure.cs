using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DigitalPlatform.LibraryRestClient
{

    #region 返回值结构

    // API函数结果
    [DataContract]
    public class LibraryServerResult
    {
        [DataMember]
        public long Value { get; set; }

        [DataMember]
        public string ErrorInfo { get; set; }

        [DataMember]
        public ErrorCode ErrorCode { get; set; }

        //[DataMember]
        //public ErrorCodeValue ErrorCode = ErrorCodeValue.NoError;
        //[DataMember]
        //public string ErrorString = "错误信息未初始化...";
    }

    // dp2Library API错误码
    public enum ErrorCode
    {
        /*
        NoError = 0,
        SystemError = 1,    // 系统错误。指application启动时的严重错误。
        NotFound = 2,   // 没有找到
        ReaderBarcodeNotFound = 3,  // 读者证条码号不存在
        ItemBarcodeNotFound = 4,  // 册条码号不存在
        Overdue = 5,    // 还书过程发现有超期情况（已经按还书处理完毕，并且已经将超期信息记载到读者记录中，但是需要提醒读者及时履行超期违约金等手续）
        NotLogin = 6,   // 尚未登录
        DupItemBarcode = 7, // 预约中本次提交的某些册条码号被本读者先前曾预约过
        InvalidParameter = 8,   // 不合法的参数
        ReturnReservation = 9,    // 还书操作成功, 因属于被预约图书, 请放入预约保留架
        BorrowReservationDenied = 10,    // 借书操作失败, 因属于被预约(到书)保留的图书, 非当前预约者不能借阅
        RenewReservationDenied = 11,    // 续借操作失败, 因属于被预约的图书
        AccessDenied = 12,  // 存取被拒绝
        ChangePartDenied = 13,    // 部分修改被拒绝
        ItemBarcodeDup = 14,    // 册条码号重复
        Hangup = 15,    // 系统挂起
        ReaderBarcodeDup = 16,  // 读者证条码号重复
        HasCirculationInfo = 17,    // 包含流通信息(不能删除)
        SourceReaderBarcodeNotFound = 18,  // 源读者证条码号不存在
        TargetReaderBarcodeNotFound = 19,  // 目标读者证条码号不存在
        FromNotFound = 20,  // 检索途径(from caption或者style)没有找到
        ItemDbNotDef = 21,  // 实体库没有定义
        IdcardNumberDup = 22,   // 身份证号检索点命中读者记录不唯一。因为无法用它借书还书。但是可以用证条码号来进行
        IdcardNumberNotFound = 23,  // 身份证号不存在


        // 以下为兼容内核错误码而设立的同名错误码
        AlreadyExist = 100, // 兼容
        AlreadyExistOtherType = 101,
        ApplicationStartError = 102,
        EmptyRecord = 103,
        // None = 104, 采用了NoError
        NotFoundSubRes = 105,
        NotHasEnoughRights = 106,
        OtherError = 107,
        PartNotFound = 108,
        RequestCanceled = 109,
        RequestCanceledByEventClose = 110,
        RequestError = 111,
        RequestTimeOut = 112,
        TimestampMismatch = 113,
        */


        NoError = 0,
        SystemError = 1,    // 系统错误。指application启动时的严重错误。
        NotFound = 2,   // 没有找到
        ReaderBarcodeNotFound = 3,  // 读者证条码号不存在
        ItemBarcodeNotFound = 4,  // 册条码号不存在
        Overdue = 5,    // 还书过程发现有超期情况（已经按还书处理完毕，并且已经将超期信息记载到读者记录中，但是需要提醒读者及时履行超期违约金等手续）
        NotLogin = 6,   // 尚未登录
        DupItemBarcode = 7, // 预约中本次提交的某些册条码号被本读者先前曾预约过 TODO: 这个和 ItemBarcodeDup 是否要合并?
        InvalidParameter = 8,   // 不合法的参数
        ReturnReservation = 9,    // 还书操作成功, 因属于被预约图书, 请放入预约保留架
        BorrowReservationDenied = 10,    // 借书操作失败, 因属于被预约(到书)保留的图书, 非当前预约者不能借阅
        RenewReservationDenied = 11,    // 续借操作失败, 因属于被预约的图书
        AccessDenied = 12,  // 存取被拒绝
        // ChangePartDenied = 13,    // 部分修改被拒绝
        ItemBarcodeDup = 14,    // 册条码号重复
        Hangup = 15,    // 系统挂起
        ReaderBarcodeDup = 16,  // 读者证条码号重复(以后将改用 BarcodeDup)
        HasCirculationInfo = 17,    // 包含流通信息(不能删除)
        SourceReaderBarcodeNotFound = 18,  // 源读者证条码号不存在
        TargetReaderBarcodeNotFound = 19,  // 目标读者证条码号不存在
        FromNotFound = 20,  // 检索途径(from caption或者style)没有找到
        ItemDbNotDef = 21,  // 实体库没有定义
        IdcardNumberDup = 22,   // 身份证号检索点命中读者记录不唯一。因为无法用它借书还书。但是可以用证条码号来进行
        IdcardNumberNotFound = 23,  // 身份证号不存在
        PartialDenied = 24,  // 有部分修改被拒绝
        ChannelReleased = 25,   // 通道先前被释放过，本次操作失败
        OutofSession = 26,   // 通道达到配额上限
        InvalidReaderBarcode = 27,  // 读者证条码号不合法
        InvalidItemBarcode = 28,    // 册条码号不合法
        NeedSmsLogin = 29,  // 需要改用短信验证码方式登录
        RetryLogin = 30,    // 需要补充验证码再次登录
        TempCodeMismatch = 31,  // 验证码不匹配
        BiblioDup = 32,     // 书目记录发生重复
        Borrowing = 33,    // 图书尚未还回(盘点前需修正此问题)
        ClientVersionTooOld = 34, // 前端版本太旧
        NotBorrowed = 35,   // 册记录处于未被借出状态 2017/6/20
        NotChanged = 36,    // 没有发生修改 2019/11/10
        ServerTimeout = 37, // 服务器发生 ApplicationException 超时
        AlreadyBorrowed = 38,   // 已经被当前读者借阅 2020/3/26
        AlreadyBorrowedByOther = 39,    // 已经被其他读者借阅 2020/3/26
        SyncDenied = 40,    // 同步操作被拒绝(因为实际操作时间之后又发生过借还操作) 2020/3/27
        PasswordExpired = 41,   // 密码已经失效 2021/7/4
        BarcodeDup = 42,        // 条码号重复了 2021/8/9
        DisplayNameDup = 43,  // 显示名重复了 2021/8/9
        RefIdDup = 44,    // 参考 ID 重复了 2021/8/9
        Canceled = 45,  // 2021/11/6

        // 以下为兼容内核错误码而设立的同名错误码
        AlreadyExist = 100, // 兼容
        AlreadyExistOtherType = 101,
        ApplicationStartError = 102,
        EmptyRecord = 103,
        // None = 104, 采用了NoError
        NotFoundSubRes = 105,
        NotHasEnoughRights = 106,
        OtherError = 107,
        PartNotFound = 108,
        RequestCanceled = 109,
        RequestCanceledByEventClose = 110,
        RequestError = 111,
        RequestTimeOut = 112,
        TimestampMismatch = 113,
        Compressed = 114,   // 2017/10/7
        NotFoundObjectFile = 115, // 2019/10/7
    }





    #endregion


    #region Reservation

    // ReservationRequest
    [DataContract]
    public class ReservationRequest
    {

        [DataMember]
        public string strFunction { get; set; }

        [DataMember]
        public string strReaderBarcode { get; set; }

        [DataMember]
        public string strItemBarcodeList { get; set; }

    }

    //SearchBiblioResult
    [DataContract]
    public class ReservationResponse
    {
        [DataMember]
        public LibraryServerResult ReservationResult { get; set; }

    }

    #endregion

    #region ManageDatabase

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
    [DataContract]
    public class ManageDatabaseRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public string strDatabaseName { get; set; }

        [DataMember]
        public string strDatabaseInfo { get; set; }

        //
        [DataMember]
        public string strStyle { get; set; }
    }

    [DataContract]
    public class ManageDatabaseResponse
    {
        [DataMember]
        public LibraryServerResult ManageDatabaseResult { get; set; }

        [DataMember]
        public string strOutputInfo { get; set; }

    }

    #endregion


    #region VerifyBarcode

    // 校验条码号
    // parameters:
    //      strLibraryCode  分馆代码
    //      strBarcode 条码号
    // return:
    //      result.Value 0: 不是合法的条码号 1:合法的读者证条码号 2:合法的册条码号
    // 权限：暂时不需要任何权限
    [DataContract]
    public class VerifyBarcodeRequest
    {
        [DataMember]
        public string strLibraryCode { get; set; }
        [DataMember]
        public string strBarcode { get; set; }
    }

    [DataContract]
    public class VerifyBarcodeResponse
    {
        [DataMember]
        public LibraryServerResult VerifyBarcodeResult { get; set; }

    }





    #endregion



    #region SearchCharging


    /*
        // parameters:
        //      actions noResult 表示不返回 results，只返回 result.Value(totalCount)
        public LibraryServerResult SearchCharging(
            string patronBarcode,
            string timeRange,
            string actions,
            string order,
            long start,
            long count,
            out ChargingItemWrapper[] results) 
     */

    // SearchItemRequest
    [DataContract]
    public class SearchChargingRequest
    {
        [DataMember]
        public string patronBarcode { get; set; }

        [DataMember]
        public string timeRange { get; set; }


        [DataMember]
        public string actions { get; set; }

        [DataMember]
        public string order { get; set; }

        [DataMember]
        public long start { get; set; }

        [DataMember]
        public long count { get; set; }

    }


    //SearchItemResult
    [DataContract]
    public class SearchChargingResponse
    {
        [DataMember]
        public LibraryServerResult SearchChargingResult { get; set; }

        [DataMember]
        public ChargingItemWrapper[] results { get; set; }

    }

    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public class ChargingItemWrapper
    {
        // 基本 Item
        [DataMember]
        public ChargingItem Item { get; set; }

        // 相关的 Item。比如一个 return 动作的 item 就可能具有一个 borrow 动作的 item
        [DataMember]
        public ChargingItem RelatedItem { get; set; }
    }

    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public class ChargingItem
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string LibraryCode { get; set; } // 访问者的图书馆代码
        [DataMember]
        public string Operation { get; set; } // 操作名
        [DataMember]
        public string Action { get; set; }  // 动作

        [DataMember]
        public string ItemBarcode { get; set; }
        [DataMember]
        public string PatronBarcode { get; set; }
        [DataMember]
        public string BiblioRecPath { get; set; }

        [DataMember]
        public string Period { get; set; }  // 期限
        [DataMember]
        public string No { get; set; }  // 续借次，序号

        // 2017/5/22
        [DataMember]
        public string Volume { get; set; }  // 卷册

        [DataMember]
        public string ClientAddress { get; set; }  // 访问者的IP地址

        [DataMember]
        public string Operator { get; set; }  // 操作者(访问者)
        [DataMember]
        public string OperTime { get; set; } // 操作时间。? 格式

    }
    #endregion


    #region SearchItem

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
    // SearchItemRequest
    [DataContract]
    public class SearchItemRequest
    {
        [DataMember]
        public string strItemDbName { get; set; }
        [DataMember]
        public string strQueryWord { get; set; }
        [DataMember]
        public int nPerMax { get; set; }
        [DataMember]

        public string strFrom{ get; set; }
        [DataMember]
        public string strMatchStyle { get; set; }
        [DataMember]
        public string strLang { get; set; }
        [DataMember]

        public string strResultSetName { get; set; }
        [DataMember]
        public string strSearchStyle { get; set; }
        [DataMember]
        public string strOutputStyle { get; set; }
    }

    /*
成员	返回值	说明
LibraryServerResult.Value		
	-1	错误
	0	没有命中
	>=1	命中。返回值为命中的记录条数
LibraryServerResult.ErrorInfo		出错信息

     */

    //SearchItemResult
    [DataContract]
    public class SearchItemResponse
    {
        [DataMember]
        public LibraryServerResult SearchItemResult { get; set; }

        //// strQueryXml
        //[DataMember]
        //public string strQueryXml { get; set; }
    }

    #endregion

    #region SearchBiblio

    // SearchBiblioRequest
    [DataContract]
    public class SearchBiblioRequest
    {
        [DataMember]
        public string strBiblioDbNames { get; set; }
        [DataMember]
        public string strQueryWord { get; set; }
        [DataMember]
        public int nPerMax { get; set; }
        [DataMember]
        public string strFromStyle { get; set; }
        [DataMember]
        public string strMatchStyle { get; set; }
        [DataMember]
        public string strLang { get; set; }
        [DataMember]
        public string strResultSetName { get; set; }
        [DataMember]
        public string strSearchStyle { get; set; }
        [DataMember]
        public string strOutputStyle { get; set; }

        [DataMember]
        public string strLocationFilter { get; set; }
    }

    //SearchBiblioResult
    [DataContract]
    public class SearchBiblioResponse
    {
        [DataMember]
        public LibraryServerResult SearchBiblioResult { get; set; }

        // strQueryXml
        [DataMember]
        public string strQueryXml { get; set; }
    }

    #endregion

    #region GetVersion

    // GetVersion()
    [DataContract]
    public class GetVersionResponse
    {
        [DataMember]
        public LibraryServerResult GetVersionResult { get; set; }
    }

    #endregion

    #region Login()

    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public string strUserName { get; set; }
        [DataMember]
        public string strPassword { get; set; }
        [DataMember]
        public string strParameters { get; set; }
    }

    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public LibraryServerResult LoginResult { get; set; }

        [DataMember]
        public string strOutputUserName { get; set; }

        [DataMember]
        public string strRights { get; set; }

        [DataMember]
        public string strLibraryCode { get; set; }

    }

    #endregion

    #region logout

    // Logout()
    [DataContract]
    public class LogoutResponse
    {
        [DataMember]
        public LibraryServerResult LogoutResult { get; set; }
    }

    #endregion

    #region SetLang

    // SetLang()
    [DataContract]
    public class SetLangRequest
    {
        [DataMember]
        public string strLang { get; set; }
    }

    [DataContract]
    public class SetLangResponse
    {
        [DataMember]
        public LibraryServerResult SetLangResult { get; set; }
        [DataMember]
        public string strOldLang { get; set; }
    }

    #endregion

    #region GetReaderInfo

    // GetReaderInfo
    [DataContract]
    public class GetReaderInfoRequest
    {
        [DataMember]
        public string strBarcode { get; set; }
        [DataMember]
        public string strResultTypeList { get; set; }
    }

    [DataContract]
    public class GetReaderInfoResponse
    {
        [DataMember]
        public LibraryServerResult GetReaderInfoResult { get; set; }
        [DataMember]
        public string[] results { get; set; }
        [DataMember]
        public string strRecPath { get; set; }
        [DataMember]
        public byte[] baTimestamp { get; set; }
    }

    #endregion

    #region SetReaderInfo

    // SetReaderInfo
    [DataContract]
    public class SetReaderInfoRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public string strRecPath { get; set; }
        [DataMember]
        public string strNewXml { get; set; }
        [DataMember]
        public string strOldXml { get; set; }
        [DataMember]
        public byte[] baOldTimestamp { get; set; }
    }

    [DataContract]
    public class SetReaderInfoResponse
    {
        [DataMember]
        public LibraryServerResult SetReaderInfoResult { get; set; }
        [DataMember]
        public string strExistingXml { get; set; }
        [DataMember]
        public string strSavedXml { get; set; }
        [DataMember]
        public string strSavedRecPath { get; set; }
        [DataMember]
        public byte[] baNewTimestamp { get; set; }

        //[DataMember]
        //public ErrorCodeValue kernel_errorcode { get; set; }
    }

    #endregion

    #region SetReaderInfo

    // SetReaderInfo
    [DataContract]
    public class SetItemInfoRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public string strRecPath { get; set; }
        [DataMember]
        public string strXml { get; set; }
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public byte[] baTimestamp { get; set; }
    }

    [DataContract]
    public class SetItemInfoResponse
    {
        [DataMember]
        public LibraryServerResult SetItemInfoResult { get; set; }

        [DataMember]
        public string strOutputRecPath { get; set; }
        [DataMember]
        public byte[] baOutputTimestamp { get; set; }
    }

    #endregion

    #region SetBiblioInfo

    /*
        public LibraryServerResult SetBiblioInfo(
            string strAction,
            string strBiblioRecPath,
            string strBiblioType,
            string strBiblio,
            byte[] baTimestamp,
            string strComment,
            string strStyle,    // 2016/12/22
            out string strOutputBiblioRecPath,
            out byte[] baOutputTimestamp) 
     */

    // SetReaderInfo
    [DataContract]
    public class SetBiblioInfoRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public string strBiblioType { get; set; }
        [DataMember]
        public string strBiblio { get; set; }
        [DataMember]
        public byte[] baTimestamp { get; set; }
        [DataMember]
        public string strComment { get; set; }
        [DataMember]
        public string strStyle { get; set; }
    }

    [DataContract]
    public class SetBiblioInfoResponse
    {
        [DataMember]
        public LibraryServerResult SetBiblioInfoResult { get; set; }

        [DataMember]
        public string strOutputBiblioRecPath { get; set; }

        [DataMember]
        public byte[] baOutputTimestamp { get; set; }
    }

    #endregion

    #region CopyBiblioInfo

    /*
        LibraryServerResult CopyBiblioInfo(
            string strAction,
            string strBiblioRecPath,
            string strBiblioType,
            string strBiblio,
            byte[] baTimestamp,
            string strNewBiblioRecPath,
            string strNewBiblio,
            out string strOutputBiblioRecPath,
            out byte[] baOutputTimestamp);
 
     */

    // SetReaderInfo
    [DataContract]
    public class CopyBiblioInfoRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public string strBiblioType { get; set; }
        [DataMember]
        public string strBiblio { get; set; }
        [DataMember]
        public byte[] baTimestamp { get; set; }
        [DataMember]
        public string strNewBiblioRecPath { get; set; }
        [DataMember]
        public string strNewBiblio { get; set; }

        [DataMember]
        public string strMergeStyle { get; set; }
    }

    [DataContract]
    public class CopyBiblioInfoResponse

    {
        [DataMember]
        public LibraryServerResult CopyBiblioInfoResult { get; set; }

        [DataMember]
        public string strOutputBiblio { get; set; }

        [DataMember]
        public string strOutputBiblioRecPath { get; set; }

        [DataMember]
        public byte[] baOutputTimestamp { get; set; }
    }

    #endregion

    #region VerifyReader

    // VerifyReaderPassword
    [DataContract]
    public class VerifyReaderPasswordRequest
    {
        [DataMember]
        public string strReaderBarcode { get; set; }
        [DataMember]
        public string strReaderPassword { get; set; }
    }

    [DataContract]
    public class VerifyReaderPasswordResponse
    {
        [DataMember]
        public LibraryServerResult VerifyReaderPasswordResult { get; set; }
    }

    #endregion

    #region ChangeReaderPassword

    [DataContract]
    public class ChangeReaderPasswordRequest
    {
        [DataMember]
        public string strReaderBarcode { get; set; }
        [DataMember]
        public string strReaderOldPassword { get; set; }
        [DataMember]
        public string strReaderNewPassword { get; set; }
    }

    [DataContract]
    public class ChangeReaderPasswordResponse
    {
        [DataMember]
        public LibraryServerResult ChangeReaderPasswordResult { get; set; }
    }

    #endregion


    #region WriteRes

    // WriteRes()
    [DataContract]
    public class WriteResRequest
    {
        [DataMember]
        public string strResPath { get; set; }
        [DataMember]
        public string strRanges { get; set; }
        [DataMember]
        public long lTotalLength { get; set; }
        [DataMember]
        public byte[] baContent { get; set; }
        [DataMember]
        public string strMetadata { get; set; }
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public byte[] baInputTimestamp { get; set; }
    }

    [DataContract]
    public class WriteResResponse
    {
        [DataMember]
        public LibraryServerResult WriteResResult { get; set; }
        [DataMember]
        public string strOutputResPath { get; set; }
        [DataMember]
        public byte[] baOutputTimestamp { get; set; }
    }



    #endregion

    #region GetRes

    [DataContract]
    public class GetResRequest
    {
        [DataMember]
        public string strResPath { get; set; }
        [DataMember]
        public long nStart { get; set; }
        [DataMember]
        public int nLength { get; set; }
        [DataMember]
        public string strStyle { get; set; }
    }

    [DataContract]
    public class GetResResponse
    {
        [DataMember]
        public LibraryServerResult GetResResult { get; set; }

        [DataMember]
        public byte[] baContent { get; set; }
        [DataMember]
        public string strMetadata { get; set; }
        [DataMember]
        public string strOutputResPath { get; set; }
        [DataMember]
        public byte[] baOutputTimestamp { get; set; }
    }

    #endregion


    #region GetRecord

    [DataContract]
    public class GetRecordRequest
    {
        [DataMember]
        public string strPath { get; set; }
    }

    [DataContract]
    public class GetRecordResponse
    {
        [DataMember]
        public LibraryServerResult GetRecordResult { get; set; }

        [DataMember]
        public byte[] timestamp { get; set; }

        [DataMember]
        public string strXml { get; set; }
    }

    #endregion

    #region GetSearchResult()

    [DataContract]
    public class GetSearchResultRequest
    {
        [DataMember]
        public string strResultSetName { get; set; }
        [DataMember]
        public long lStart { get; set; }
        [DataMember]
        public long lCount { get; set; }
        [DataMember]
        public string strBrowseInfoStyle { get; set; }
        [DataMember]
        public string strLang { get; set; }

    }

    [DataContract]
    public class GetSearchResultResponse
    {
        [DataMember]
        public LibraryServerResult GetSearchResultResult { get; set; }
        [DataMember]
        public Record[] searchresults { get; set; }
    }

    [DataContract(Namespace = "http://dp2003.com/dp2kernel/")]
    public class Record
    {
        [DataMember]
        public string Path = ""; // 带库名的全路径 原来叫ID 2010/5/17 changed
        [DataMember]
        public KeyFrom[] Keys = null; // 检索命中的key from字符串数组 
        [DataMember]
        public string[] Cols = null;

        [DataMember]
        public RecordBody RecordBody = null; // 记录体。2012/1/5
    }

    [DataContract(Namespace = "http://dp2003.com/dp2kernel/")]
    public class RecordBody
    {
        [DataMember]
        public string Xml = "";
        [DataMember]
        public byte[] Timestamp = null;
        [DataMember]
        public string Metadata = "";

        [DataMember]
        public Result Result = new Result(); // 结果信息
    }

    [DataContract(Namespace = "http://dp2003.com/dp2kernel/")]
    public class Result
    {
        [DataMember]
        public long Value = 0; // 命中条数，>=0:正常;<0:出错

        [DataMember]
        public ErrorCodeValue ErrorCode = ErrorCodeValue.NoError;
        [DataMember]
        public string ErrorString = "错误信息未初始化...";
    }

    public enum ErrorCodeValue : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NoError = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        CommonError = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotLogin = 2,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        UserNameEmpty = 3,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        UserNameOrPasswordMismatch = 4,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotHasEnoughRights = 5,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        TimestampMismatch = 9,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotFound = 10,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        EmptyContent = 11,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotFoundDb = 12,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        PathError = 14,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        PartNotFound = 15,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ExistDbInfo = 16,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        AlreadyExist = 17,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        AlreadyExistOtherType = 18,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        ApplicationStartError = 19,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotFoundSubRes = 20,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Canceled = 21,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        AccessDenied = 22,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        PartialDenied = 23,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        NotFoundObjectFile = 24,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Compressed = 25,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RequestError = 100,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        RequestTimeOut = 112,
    }

    [DataContract(Namespace = "http://dp2003.com/dp2kernel/")]
    public class KeyFrom
    {
        [DataMember]
        public string Logic = "";
        [DataMember]
        public string Key = "";
        [DataMember]
        public string From = "";
    }

    #endregion




    #region GetUser

    [DataContract]
    public class GetUserRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public string strName { get; set; }
        [DataMember]
        public int nStart { get; set; }

        [DataMember]
        public int nCount { get; set; }
    }

    [DataContract]
    public class GetUserResponse
    {
        [DataMember]
        public LibraryServerResult GetUserResult { get; set; }
        [DataMember]
        public UserInfo[] contents { get; set; }
    }

    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public class UserInfo
    {
        [DataMember]
        public string UserName = "";    // 用户名

        [DataMember]
        public bool SetPassword = false;    // 是否设置密码
        [DataMember]
        public string Password = "";    // 密码

        [DataMember]
        public string Rights = "";  // 权限值
        [DataMember]
        public string Type = "";    // 账户类型

        [DataMember]
        public string LibraryCode = ""; // 图书馆代码 2007/12/15 

        [DataMember]
        public string Access = "";  // 关于存取权限的定义 2008/2/28 

        [DataMember]
        public string Comment = "";  // 注释 2012/10/8

        [DataMember]
        public string Binding = ""; // 绑定 2016/6/15

        [DataMember]
        public string Location = "";    // 默认位置 2022/2/21
    }

    #endregion


    #region SetUser

    /*
        // 修改用户
        // parameters:
        //      strAction   new delete change resetpassword
        //              当 action 为 "change" 时，如果要在修改其他信息的同时修改密码，info.SetPassword必须为true；
        //              而当action为"resetpassword"时，则info.ResetPassword状态不起作用，无论怎样都要修改密码。resetpassword并不修改其他信息，也就是说info中除了Password/UserName以外其他成员的值无效。
        //              当 action 为 "changeandclose" 时，效果同 "change"，只是最后还要自动切断此用户的 session
        // return:
        //      result.Value    -1 错误
        public LibraryServerResult SetUser(
            string strAction,
            UserInfo info) 
     */

    [DataContract]
    public class SetUserRequest
    {
        [DataMember]
        public string strAction { get; set; }

        [DataMember]
        public UserInfo info { get; set; }

    }

    [DataContract]
    public class SetUserResponse
    {
        [DataMember]
        public LibraryServerResult SetUserResult { get; set; }

    }



    #endregion

    #region SearchReader

    //searchReader
    [DataContract]
    public class SearchReaderRequest
    {
        [DataMember]
        public string strReaderDbNames { get; set; }
        [DataMember]
        public string strQueryWord { get; set; }
        [DataMember]
        public int nPerMax { get; set; }
        [DataMember]
        public string strFrom { get; set; }
        [DataMember]
        public string strMatchStyle { get; set; }
        [DataMember]
        public string strLang { get; set; }
        [DataMember]
        public string strResultSetName { get; set; }
        [DataMember]
        public string strOutputStyle { get; set; }


    }
    [DataContract]
    public class SearchReaderResponse
    {
        [DataMember]
        public LibraryServerResult SearchReaderResult { get; set; }
    }

    #endregion

    #region GetItemInfo

    // 获得册信息
    // TODO: 需要改进为，如果册记录存在，但是书目记录不存在，也能够适当返回
    // parameters:
    //      strItemDbType   2015/1/30
    //      strBarcode  册条码号。特殊情况下，可以使用"@path:"引导的册记录路径(只需要库名和id两个部分)作为检索入口。在@path引导下，路径后面还可以跟随 "$prev"或"$next"表示方向
    //      strResultType   指定需要在strResult参数中返回的数据格式。为"xml" "html" "uii"之一。
    //                      如果为空，则表示strResult参数中不返回任何数据。无论这个参数为什么值，strItemRecPath中都回返回册记录路径(如果命中了的话)
    //      strItemRecPath  返回册记录路径。可能为逗号间隔的列表，包含多个路径
    //      strBiblioType   指定需要在strBiblio参数中返回的数据格式。为"xml" "html"之一。
    //                      如果为空，则表示strBiblio参数中不返回任何数据，strBilbioRecPath中也不返回路径。
    //                      如果要仅仅在strBiblioRecPath中返回路径，请使用"recpath"作为strBiblioType参数的值。
    //                      如果为"html"或"xml"之一，则会在strBiblioRecPath中返回路径。
    //                      之所以要这样设计，主要是为了效率考虑。用""调用时，甚至不需要返回书目记录路径，这会更多地省去一些关于种的操作。
    //      strBiblioRecPath    返回书目记录路径
    // return:
    // Result.Value -1出错 0册记录没有找到 1册记录找到 >1册记录命中多于1条
    // 权限:   需要具有getiteminfo权限
    //public LibraryServerResult GetItemInfo(
    //    string strItemDbType,
    //    string strBarcode,
    //    string strItemXml,  // 前端提供给服务器的记录内容。例如，需要模拟创建检索点，就需要前端提供记录内容
    //    string strResultType,
    //    out string strResult,
    //    out string strItemRecPath,
    //    out byte[] item_timestamp,
    //    string strBiblioType,
    //    out string strBiblio,
    //    out string strBiblioRecPath)

    [DataContract]
    public class GetItemInfoRequest
    {
        [DataMember]
        public string strBarcode { get; set; }
        [DataMember]
        public string strResultType { get; set; }
        [DataMember]
        public string strBiblioType { get; set; }

        // 2022/11/9 把原来省掉的两个参数加在这里，使用的时候可以不赋值
        [DataMember]
        public string strItemDbType { get; set; }
        [DataMember]
        public string strItemXml { get; set; }
    }
    [DataContract]
    public class GetItemInfoResponse
    {
        [DataMember]
        public LibraryServerResult GetItemInfoResult { get; set; }
        [DataMember]
        public string strResult { get; set; }
        [DataMember]
        public string strItemRecPath { get; set; }
        [DataMember]
        public byte[] baTimestamp { get; set; }
        [DataMember]
        public string strBiblio { get; set; }
        [DataMember]
        public string strBiblioRecPath { get; set; }
    }

    #endregion

    #region GetBiblioInfo

    [DataContract]
    public class GetBiblioInfoRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public string strBiblioXml { get; set; }
        [DataMember]
        public string strBiblioType { get; set; }
    }

    [DataContract]
    public class GetBiblioInfoResponse
    {
        [DataMember]
        public LibraryServerResult GetBiblioInfoResult { get; set; }
        [DataMember]
        public string strBiblio { get; set; }
    }

    #endregion

    
    #region GetBiblioInfos  2022/10新增


    [DataContract]
    public class GetBiblioInfosRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public string strBiblioXml { get; set; }
        [DataMember]
        public string[] formats { get; set; }
    }

    [DataContract]
    public class GetBiblioInfosResponse
    {
        [DataMember]
        public LibraryServerResult GetBiblioInfosResult { get; set; }
        [DataMember]
        public string[] results { get; set; }

        [DataMember]
        public byte[] baTimestamp { get; set; }

    }

    #endregion

    #region SetEnties

    [DataContract]
    public class SetEntitiesRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public EntityInfo[] entityinfos { get; set; }
    }

    [DataContract]
    public class SetEntitiesResponse
    {
        [DataMember]
        public LibraryServerResult SetEntitiesResult { get; set; }
        [DataMember]
        public EntityInfo[] errorinfos { get; set; }
    }

    [DataContract]
    public class SetOrdersRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public EntityInfo[] orderinfos { get; set; }
    }

    [DataContract]
    public class SetOrdersResponse
    {
        [DataMember]
        public LibraryServerResult SetOrdersResult { get; set; }
        [DataMember]
        public EntityInfo[] errorinfos { get; set; }
    }


    [DataContract]
    public class SetCommentsRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public EntityInfo[] commentinfos { get; set; }
    }

    [DataContract]
    public class SetCommentsResponse
    {
        [DataMember]
        public LibraryServerResult SetCommentsResult { get; set; }
        [DataMember]
        public EntityInfo[] errorinfos { get; set; }
    }


    [DataContract]
    public class SetIssuesRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public EntityInfo[] issueinfos { get; set; }
    }

    [DataContract]
    public class SetIssuesResponse
    {
        [DataMember]
        public LibraryServerResult SetIssuesResult { get; set; }
        [DataMember]
        public EntityInfo[] errorinfos { get; set; }
    }


    #endregion

    #region GetEntities/GetComments/GetIssues/GetOrders

    [DataContract]
    public class GetCommentsRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public long lStart { get; set; }
        [DataMember]
        public long lCount { get; set; }
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public string strLang { get; set; }
    }

    [DataContract]
    public class GetCommentsResponse
    {
        [DataMember]
        public LibraryServerResult GetCommentsResult { get; set; }
        [DataMember]
        public EntityInfo[] commentinfos { get; set; }
    }


    [DataContract]
    public class GetIssuesRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public long lStart { get; set; }
        [DataMember]
        public long lCount { get; set; }
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public string strLang { get; set; }
    }

    [DataContract]
    public class GetIssuesResponse
    {
        [DataMember]
        public LibraryServerResult GetIssuesResult { get; set; }
        [DataMember]
        public EntityInfo[] issueinfos { get; set; }
    }

    [DataContract]
    public class GetOrdersRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public long lStart { get; set; }
        [DataMember]
        public long lCount { get; set; }
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public string strLang { get; set; }
    }

    [DataContract]
    public class GetOrdersResponse
    {
        [DataMember]
        public LibraryServerResult GetOrdersResult { get; set; }
        [DataMember]
        public EntityInfo[] orderinfos { get; set; }
    }

    [DataContract]
    public class GetEntitiesRequest
    {
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public long lStart { get; set; }
        [DataMember]
        public long lCount { get; set; }
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public string strLang { get; set; }
    }

    [DataContract]
    public class GetEntitiesResponse
    {
        [DataMember]
        public LibraryServerResult GetEntitiesResult { get; set; }
        [DataMember]
        public EntityInfo[] entityinfos { get; set; }
    }

    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public class EntityInfo
    {
        [DataMember]
        public string RefID = "";  // 参考ID
        [DataMember]
        public string OldRecPath = "";  // 旧记录路径
        [DataMember]
        public string OldRecord = "";   // 旧记录
        [DataMember]
        public byte[] OldTimestamp = null;  // 旧记录的时间戳

        [DataMember]
        public string NewRecPath = ""; // 新记录路径
        [DataMember]
        public string NewRecord = "";   // 新记录
        [DataMember]
        public byte[] NewTimestamp = null;  // 新记录的时间戳

        [DataMember]
        public string Action = "";   // 要执行的操作(get时此项无用)
        [DataMember]
        public string Style = "";   // 附加的特性参数

        [DataMember]
        public string ErrorInfo = "";   // 出错信息

        [DataMember]
        public ErrorCodeValue ErrorCode = ErrorCodeValue.NoError;   // 出错码（表示属于何种类型的错误）

    }

    #endregion

    #region GetBiblioSummary

    [DataContract]
    public class GetBiblioSummaryRequest
    {
        [DataMember]
        public string strItemBarcode { get; set; }
        [DataMember]
        public string strConfirmItemRecPath { get; set; }
        [DataMember]
        public string strBiblioRecPathExclude { get; set; }
    }
    [DataContract]
    public class GetBiblioSummaryResponse
    {
        [DataMember]
        public LibraryServerResult GetBiblioSummaryResult { get; set; }
        [DataMember]
        public string strBiblioRecPath { get; set; }
        [DataMember]
        public string strSummary { get; set; }
    }

    #endregion

    #region ListMessage

    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public enum MessageLevel
    {
        [EnumMember]
        ID = 0,    // 只返回ID
        [EnumMember]
        Summary = 1,    // 摘要级，不返回body
        [EnumMember]
        Full = 2,   // 全部级，返回全部信息
    }

    [DataContract]
    public class ListMessageRequest
    {
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public string strResultsetName { get; set; }
        [DataMember]
        public string strBoxType { get; set; }
        [DataMember]
        public MessageLevel messagelevel { get; set; }
        [DataMember]
        public int nStart { get; set; }
        [DataMember]
        public int nCount { get; set; }
    }
    [DataContract]
    public class ListMessageResponse
    {
        [DataMember]
        public LibraryServerResult ListMessageResult { get; set; }
        [DataMember]
        public int nTotalCount { get; set; }
        [DataMember]
        public List<MessageData> messages { get; set; }
    }

    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public class MessageData
    {
        [DataMember]
        public string strUserName = ""; // 消息所从属的用户ID
        [DataMember]
        public string strBoxType = "";   // 信箱类型
        [DataMember]
        public string strSender = "";    // 发送者
        [DataMember]
        public string strRecipient = "";    // 接收者
        [DataMember]
        public string strSubject = "";  // 主题
        [DataMember]
        public string strMime = ""; // 媒体类型
        [DataMember]
        public string strBody = "";      // 正文内容
        [DataMember]
        public string strCreateTime = "";   // 邮件创建(收到)时间
        [DataMember]
        public string strSize = "";     // 尺寸
        [DataMember]
        public bool Touched = false;    // 是否阅读过
        [DataMember]
        public string strRecordID = ""; // 记录ID。用于唯一定位一条消息
        [DataMember]
        public byte[] TimeStamp = null;
    }

    #endregion

    #region GetOperLog

    /*
         LibraryServerResult GetOperLog(
                    string strFileName,
                    long lIndex,
                    long lHint,
                    string strStyle,
                    string strFilter,
                    out string strXml,
                    out long lHintNext,
                    long lAttachmentFragmentStart,
                    int nAttachmentFragmentLength,
                    out byte[] attachment_data,
                    out long lAttachmentTotalLength);
     */
    [DataContract]
    public class GetOperLogRequest
    {
        [DataMember]
        public string strFileName { get; set; }
        [DataMember]
        public long lIndex { get; set; }
        [DataMember]
        public long lHint { get; set; }
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public string strFilter { get; set; }

        public long lAttachmentFragmentStart { get; set; }
        [DataMember]
        public int nAttachmentFragmentLength { get; set; }
    }
    [DataContract]
    public class GetOperLogResponse
    {
        [DataMember]
        public LibraryServerResult GetOperLogResult { get; set; }
        [DataMember]
        public string strXml { get; set; }
        [DataMember]
        public long lHintNext { get; set; }
        [DataMember]
        public byte[] attachment_data { get; set; }
        [DataMember]
        public long lAttachmentTotalLength { get; set; }
    }

    /*
        LibraryServerResult GetOperLogs(
            string strFileName,
            long lIndex,
            long lHint,
            int nCount,
            string strStyle,
            string strFilter,
            out OperLogInfo[] records);
     */
    [DataContract]
    public class GetOperLogsRequest
    {
        [DataMember]
        public string strFileName { get; set; }
        [DataMember]
        public long lIndex { get; set; }
        [DataMember]
        public long lHint { get; set; }
        [DataMember]
        public int nCount { get; set; }

        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public string strFilter { get; set; }
    }



        [DataContract]
    public class GetOperLogsResponse
    {
        //[DataMember]
        //public long Value { get; set; }

        //[DataMember]
        //public string ErrorInfo { get; set; }

        //[DataMember]
        //public ErrorCode ErrorCode { get; set; }

        [DataMember]
        public LibraryServerResult GetOperLogsResult { get; set; }

        [DataMember]
        public OperLogInfo[] records { get; set; }
    }

    // API GetOperLogs()所使用的结构
    [DataContract]
    public class OperLogInfo
    {
        [DataMember]
        public long Index = -1; // 日志记录序号
        [DataMember]
        public long HintNext = -1; // 下一记录暗示

        [DataMember]
        public string Xml = ""; // 日志记录XML
        [DataMember]
        public long AttachmentLength = 0;   // 附件尺寸
    }

    #endregion


    #region Borrow

    /*
        LibraryServerResult Borrow(
                    bool bRenew,
                    string strReaderBarcode,
                    string strItemBarcode,
                    string strConfirmItemRecPath,
                    bool bForce,

                    string[] saBorrowedItemBarcode,
                    string strStyle,
     
                    string strItemFormatList,
                    out string[] item_records,
                    string strReaderFormatList,
                    out string[] reader_records,
                    string strBiblioFormatList,
                    out string[] biblio_records,
                    out BorrowInfo borrow_info,
                    out string[] aDupPath,
                    out string strOutputReaderBarcode)
    */
    // WriteRes()
    [DataContract]
    public class BorrowRequest
    {
        [DataMember]
        public bool bRenew { get; set; }
        [DataMember]
        public string strReaderBarcode { get; set; }
        [DataMember]
        public string strItemBarcode { get; set; }
        [DataMember]
        public string strConfirmItemRecPath { get; set; }
        [DataMember]
        public bool bForce { get; set; }


        [DataMember]
        public string[] saBorrowedItemBarcode { get; set; }

        [DataMember]
        public string strStyle { get; set; }

        [DataMember]
        public string strItemFormatList { get; set; }

        [DataMember]
        public string strReaderFormatList { get; set; }

        [DataMember]
        public string strBiblioFormatList { get; set; }
    }

    [DataContract]
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

    #endregion

    #region Return

    //=======================
    /*
            LibraryServerResult Return(
                    string strAction,
                    string strReaderBarcode,
                    string strItemBarcode,
                    string strComfirmItemRecPath,
     * 
                    bool bForce,
                    string strStyle,
                    string strItemFormatList,
                    out string[] item_records,
     * 
                    string strReaderFormatList,
                    out string[] reader_records,
                    string strBiblioFormatList,
                    out string[] biblio_records,
                    out string[] aDupPath,
                    out string strOutputReaderBarcode,
                    out ReturnInfo return_info);
    */

    [DataContract]
    public class ReturnRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public string strReaderBarcode { get; set; }
        [DataMember]
        public string strItemBarcode { get; set; }
        [DataMember]
        public string strConfirmItemRecPath { get; set; }

        //======
        [DataMember]
        public bool bForce { get; set; }
        [DataMember]
        public string strStyle { get; set; }

        [DataMember]
        public string strItemFormatList { get; set; }

        [DataMember]
        public string strReaderFormatList { get; set; }

        [DataMember]
        public string strBiblioFormatList { get; set; }
    }


    [DataContract]
    public class ReturnResponse
    {
        [DataMember]
        public LibraryServerResult ReturnResult { get; set; }

        [DataMember]
        public string[] item_records { get; set; }
        [DataMember]
        public string[] reader_records { get; set; }

        [DataMember]
        public string[] biblio_records { get; set; }

        [DataMember]
        public ReturnInfo return_info { get; set; }

        [DataMember]
        public string[] aDupPath { get; set; }

        [DataMember]
        public string strOutputReaderBarcode { get; set; }
    }

    // 还书成功后的信息
    [DataContract(Namespace = "http://dp2003.com/dp2library/")]
    public class ReturnInfo
    {
        // 借阅日期/时间
        [DataMember]
        public string BorrowTime = "";    // RFC1123格式，GMT时间

        // 应还日期/时间
        [DataMember]
        public string LatestReturnTime = "";    // RFC1123格式，GMT时间

        // 原借书期限。例如“20day”
        [DataMember]
        public string Period = "";

        // 当前为续借的第几次？0表示初次借阅
        [DataMember]
        public long BorrowCount = 0;

        // 违约金描述字符串。XML格式
        [DataMember]
        public string OverdueString = "";

        // 借书操作者
        [DataMember]
        public string BorrowOperator = "";

        // 还书操作者
        [DataMember]
        public string ReturnOperator = "";

        // 2008/5/9
        /// <summary>
        /// 所还的册的图书类型
        /// </summary>
        [DataMember]
        public string BookType = "";

        // 2008/5/9
        /// <summary>
        /// 所还的册的馆藏地点
        /// </summary>
        [DataMember]
        public string Location = "";
    }

    #endregion

    #region

    /*
        // Settlement 结算
        // parameters:
        //      ids    ID的集合
        // return:
        //      result.Value    -1 错误 其他 本次的累计量
        public LibraryServerResult Settlement(
            string strAction,
            string[] ids)
     */
    public class SettlementRequest
    {
        [DataMember]
        public string strAction { get; set; }

        [DataMember]
        public string[] ids { get; set; }


    }

    [DataContract]
    public class SettlementResponse
    {
        [DataMember]
        public LibraryServerResult SettlementResult { get; set; }

    }

    #endregion


    #region GetBrowseRecords
    /*
            string[] paths,
            string strBrowseInfoStyle,
            out Record[] searchresults,
            out string strError)
     */
    public class GetBrowseRecordsRequest
    {
        [DataMember]
        public string[] paths { get; set; }

        [DataMember]
        public string strBrowseInfoStyle { get; set; }
    }

    [DataContract]
    public class GetBrowseRecordsResponse
    {
        [DataMember]
        public LibraryServerResult GetBrowseRecordsResult { get; set; }

        [DataMember]
        public Record[] searchresults { get; set; }
    }


    #endregion

    #region GetSystemParameter

    [DataContract]
    public class GetSystemParameterRequest
    {
        [DataMember]
        public string strCategory { get; set; }
        [DataMember]
        public string strName { get; set; }
    }

    [DataContract]
    public class GetSystemParameterResponse
    {
        [DataMember]
        public LibraryServerResult GetSystemParameterResult { get; set; }

        [DataMember]
        public string strValue { get; set; }
    }

    #endregion

    #region SetSystemParameter

    [DataContract]
    public class SetSystemParameterRequest
    {
        [DataMember]
        public string strCategory { get; set; }
        [DataMember]
        public string strName { get; set; }

        //strValue
        [DataMember]
        public string strValue { get; set; }
    }

    [DataContract]
    public class SetSystemParameterResponse
    {
        [DataMember]
        public LibraryServerResult SetSystemParameterResult { get; set; }

        [DataMember]
        public string strValue { get; set; }
    }

    #endregion

    #region Search

    [DataContract]
    public class SearchRequest
    {
        [DataMember]
        public string strQueryXml { get; set; }

        [DataMember]
        public string strResultSetName { get; set; }

        [DataMember]
        public string strOutputStyle { get; set; }
    }

    [DataContract]
    public class SearchResponse
    {
        [DataMember]
        public LibraryServerResult SearchResult { get; set; }
    }

    #endregion



    #region SetMessage

    [DataContract]
    public class SetMessageRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public string strStyle { get; set; }
        [DataMember]
        public MessageData[] messages { get; set; }

    }


    [DataContract]
    public class SetMessageResponse
    {
        [DataMember]
        public LibraryServerResult SetMessageResult { get; set; }

        [DataMember]
        public MessageData[] output_messages { get; set; }
    }


    #endregion

    #region GetCalendar

    /*
        public LibraryServerResult GetCalendar(
            string strAction,
            string strName,
            int nStart,
            int nCount,
            out CalenderInfo[] contents)
     */

    [DataContract]
    public class GetCalendarRequest
    {
        [DataMember]
        public string strAction { get; set; }

        [DataMember]
        public string strName { get; set; }

        [DataMember]
        public int nStart { get; set; }

        [DataMember]
        public int nCount { get; set; }
    }


    [DataContract]
    public class GetCalendarResponse
    {
        [DataMember]
        public LibraryServerResult GetCalendarResult { get; set; }

        [DataMember]
        public CalenderInfo contents { get; set; }
    }


    #endregion

    #region SetCalendar

    [DataContract]
    public class SetCalendarRequest
    {
        [DataMember]
        public string strAction { get; set; }
        [DataMember]
        public CalenderInfo info { get; set; }

    }


    [DataContract]
    public class SetCalendarResponse
    {
        [DataMember]
        public LibraryServerResult SetCalendarResult { get; set; }


    }


    #endregion

    public class CalenderInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Range { get; set; }

        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string Content { get; set; }
    }
}
