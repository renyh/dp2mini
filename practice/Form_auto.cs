using common;
using DigitalPlatform;
using DigitalPlatform.LibraryRestClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using DigitalPlatform.Xml;
using System.Xml;
using System.Threading;
using DigitalPlatform.CirculationClient;
using DigitalPlatform.Marc;
using DigitalPlatform.Text;

namespace practice
{
    public partial class Form_auto : Form
    {
        Form_main mainForm = null;
        public Form_auto(Form_main main)
        {
            InitializeComponent();

            this.mainForm = main;
        }

        #region 创建帐户相关

        public void NewUser1(UserInfo u)
        {
            this.SetUser1(u, "new");
        }

        public void DelUser(UserInfo u)
        {
            this.SetUser1(u, "delete");
        }

        public void SetUser1(UserInfo user, string strAction)
        {
            List<UserInfo> list = new List<UserInfo>();
            list.Add(user);
            this.SetUsers(list, strAction);
        }



        public void DeleteAutoUsers()
        {

            List<UserInfo> users = new List<UserInfo>();
            users.Add(new UserInfo { UserName = "u1" });
            users.Add(new UserInfo { UserName = "u2" });
            users.Add(new UserInfo { UserName = "u3" });
            users.Add(new UserInfo { UserName = "u4" });
            users.Add(new UserInfo { UserName = "u5" });
            users.Add(new UserInfo { UserName = "u6" });
            users.Add(new UserInfo { UserName = "u7" });
            users.Add(new UserInfo { UserName = "u8" });
            users.Add(new UserInfo { UserName = "u9" });

            foreach (UserInfo u in users)
            {
                try
                {
                    DelUser(u);
                }
                catch (Exception ex)
                {
                    // 有可能帐户不存天，没关系的。
                }
            }

        }

        // 创建或删除一批帐户
        public void SetUsers(List<UserInfo> users, string strAction)
        {
            if (strAction != "new" && strAction != "delete")
            {
                throw new Exception("SetUsers()不支持" + strAction);
            }

            RestChannel channelSuper = null;
            try
            {
                // 用超级管理员帐户登录
                channelSuper = mainForm.GetChannelAndLogin(this.mainForm.GetSupervisorAccount());// this._channelPool.GetChannel(this.ServerUrl, supervisorName);

                foreach (UserInfo u in users)
                {
                    SetUserResponse response = channelSuper.SetUser(strAction, u);
                    if (response.SetUserResult.Value == -1)
                    {
                        throw new Exception("创建帐户出错：" + response.SetUserResult.ErrorInfo);
                    }

                    if (strAction == "new")
                    {
                        this.displayLine(strAction + "帐户" + u.UserName
                            + "<br/>Rights=" + u.Rights
                            + "<br/>Access=" + u.Access
                            + "<br/>LibraryCode=" + u.LibraryCode);
                    }
                    else
                    {
                        this.displayLine("<br/>" + strAction + "帐户" + u.UserName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(strAction + "帐户异常：" + ex.Message);
            }
            finally
            {
                if (channelSuper != null)
                    this.mainForm._channelPool.ReturnChannel(channelSuper);
            }
        }

        #endregion

        #region 通用函数



        public void EnableCtrls(bool bEnable)
        {
            this.button_accessAndObject.Enabled = bEnable;
            //this.button_reader.Enabled = bEnable;
            //this.button_biblio.Enabled = bEnable;

            this.comboBox_TestRight_type.Enabled = bEnable;
            this.button_testRight.Enabled = bEnable;
        }

        // 写对象
        public WriteResResponse WriteObject(UserInfo u,
          string objectPath,
          bool isReader = false)
        {

            this.displayLine("strResPath=" + objectPath);

            // 一个图片对象
            string hexObject = "ffd8ffe000104a4649460001010100d800d80000ffdb004300080606070605080707070909080a0c140d0c0b0b0c1912130f141d1a1f1e1d1a1c1c20242e2720222c231c1c2837292c30313434341f27393d38323c2e333432ffdb0043010909090c0b0c180d0d1832211c213232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232ffc00011080043007803012200021101031101ffc4001f0000010501010101010100000000000000000102030405060708090a0bffc400b5100002010303020403050504040000017d01020300041105122131410613516107227114328191a1082342b1c11552d1f02433627282090a161718191a25262728292a3435363738393a434445464748494a535455565758595a636465666768696a737475767778797a838485868788898a92939495969798999aa2a3a4a5a6a7a8a9aab2b3b4b5b6b7b8b9bac2c3c4c5c6c7c8c9cad2d3d4d5d6d7d8d9dae1e2e3e4e5e6e7e8e9eaf1f2f3f4f5f6f7f8f9faffc4001f0100030101010101010101010000000000000102030405060708090a0bffc400b51100020102040403040705040400010277000102031104052131061241510761711322328108144291a1b1c109233352f0156272d10a162434e125f11718191a262728292a35363738393a434445464748494a535455565758595a636465666768696a737475767778797a82838485868788898a92939495969798999aa2a3a4a5a6a7a8a9aab2b3b4b5b6b7b8b9bac2c3c4c5c6c7c8c9cad2d3d4d5d6d7d8d9dae2e3e4e5e6e7e8e9eaf2f3f4f5f6f7f8f9faffda000c03010002110311003f005f30f1d3a0eded4bbc839e307af02abf53c93d054a8ab9e80fd69202c4730cf55fc055a494f65ffc745544f978fc8fad598cd319691e43fdc1f866aaea52dedbdb33da143210480c80827d2aca5492279b0b2f7ea3eb4981c8c3ad5f5d46923cf218d8ec6d910501bf1a86ff005386c032df30495588db23649fc8547a9dc4da6fdb5a657fb39fdf0c108ad8cf1d3af6ae059afbc417ed34ae5998e49cf007a0a9bf7292bbb23a0b8f15c20b88ad818f3c3aa283fae6bb0f0e7c48d1fecb0da5e2cb6aea02f98e8193a63248e47e558fa47816cef2d544adb988e70fd0d6837c28491de386e1a2761f2331f949f4353ed23735f613b5cf49b6b88ee2249a178e48dc6e57400861ea0d5b0c71d17fef915e59e169efbc21a9cba66a7bd6d4b0dc8c3fd53138dc3d8f19fcebd4d7a715aa316ada12073dc2ff00df2292176d8bc2f3cfdd1de9b210b1373ce314f42a7001071e94c92e5b3b79a9c2fde1fc228a20e244c2b751da8a04792679fc054f1b557279c8049c0fc69f1b13d01e3ae6a465d5e463f23e9534679f71d45568c9f6153ec63f36e391d87714319710d58438ea7154a3c7a93f8d598c2fa0a434703f12d5122b351210b2c8d9c1e30067f9d51d1ac8c70abdadb2ba3f31472360631d58f526b4be28a6f8b48e783248bf9edab5a5ba5b1db81c0c0cd44df43a2842edb2ec17315b5ddba3d8c569395fde2c323346c723079e477c8aeb7c44d751a5ab5ad85bb4cfc843315423b60fd2b82d4a3bcbbbe5fb3bc31282a0cb29c81ebc57a6d95aea72e8a0cd2daea16502ab46aab89100fbfce7e607a8e9c5667472e9b991e31b2b14d260d4b54865b4125b326636f344328202807b87cf43dc76ae86d151ad2120960517049ebc56a78874cb6d4be1e6a36a0008d68d221eb8651bd4fe6056368a59b45b267fbc615cfe55d11d8e1a9b978c2a42e140f9b3d3d2a741e9516f5f300dc381eb52a11fe455105a8173227fbc28a75b7fad4383f785141363c6fb8fa0a91573c8eb4c20e460f61fcaa44ce6a0a278cd5946aac141fad381db4c0b5b829c82307f4ab11b5558dc1183d2a656c1c134868c0f1ce9726a1a7dadca8256d5d8b8ff006580e7f303f3acc46dbb58fa5774a5590ab00ca46083c822b90d7e08ecb5129145b21740e981c7a103fcf7acea2ea75e1e493e530e6899e5cbccc501e141c57a7783552f34b905bcd79a7dc451958e557dc98c7756eb5e6865403af22bd27c11e2cb54b55b3ba500f4c9f4ace2cea95f96c75ba0892efe1d4f0c92e2578658989fe1ea303daa2b58120b78a150311a051f80a945d243606d61428257de4118f97a8fcfad3118fa8fcaba21b1e6d57ef3245fbec40ef8a9e3aab1b65412dd79ab0846475fceacccbd170f18ee585145bec6954e01e4628a093c736b03d3b0a95549ab2d0ed23a741fcaa29a193cbfdccca8c3fbc0106b3b96380c521193d4e6a8f9979921f72e0f3c8c7e07b8a63ea4220a1d096242861f749345c2c69c6706a65963762a1d0baf604645508e396ef23cc0a47f02f159f79a0ca6413dacaf6d74bf72453c673d08f434f52923a37bb8ada079a67db1a0cb1c74ac0f106ab617f6f0c504e1e547cedc1fba47ff00aab32fb5fd4b12d9cb1451b152927c9c90460f15ce69d633da29cc4ccd9e5d79047b7a7d2a24f435a7171926cd4f2d1e400f735eadf0ebc2b604bde5c0f38a91b437415e65616ed73202aad953ce057aff0085af459695e4b3619bd7bd6314754e5756449e29d6ec6c3c413c534bb6448918a85edb7354ed75ab5bbdab1bba348309e6260313db3eb583e3bb3b7d52e2e352b96fb3dc2c491c4cadcfcb9e4fd41c62a9f862216f0dac8f334ae1777cc7ee9ef5d09bbd8e39534a373bef2e6419314b81df61a69b855c02d866e9934df0e6bb346935acd292aae793d429ae7eefc636e1f50b6452c6572ecce73e5aa02491ee718fcab47b18d8ecec668f720565dd918c9a2bc32d7c5daf5edfc57763235bc6ac705c82a07a723e6a2b3722953b9ac92c8f7112b3b1564c919fa5694b6d122ee55c305c83939eb451534f707b1144048595c065cf4233d8d3deceddec4c6631b08ce01206474e94515b12577765b68a60c448d6a5cb67a9c75ae9b4a45bab0479943b30e49ef4514219cbf8d6cade3104e910597637cc09cf0462b9764536f1c98c391c91c5145672dceba3f0a34b4324c9202cc73d72c6bd2b44b788daa929ce0739a28a70d8b96e71de3525afd2224f960e76e6a3d3490fc13c7bd1452fb66753e0475da6468d22b11c95e4e6b8496245d7ef005e0a3ff35a28aa7d0e68f530ac915628540c00a3028a28ae67b9dab63fffd9";
            byte[] baContent_object = ByteArray.GetTimeStampByteArray(hexObject);
            long lTotalLength_object = baContent_object.Length;
            string strRanges_object = "0-" + (baContent_object.Length - 1).ToString();

            byte[] baTimestamp = null;

            // 用u_a3帐户，写xml
            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            REDO:
                // a1写object
                WriteResResponse response = channel.WriteRes(objectPath,
                    strRanges_object,
                    lTotalLength_object,
                    baContent_object,
                    "",
                    "",
                    baTimestamp);
                // 间戳不匹配，自动重试
                if (response.WriteResResult.ErrorCode == ErrorCode.TimestampMismatch)
                {
                    // 设上时间戳
                    baTimestamp = response.baOutputTimestamp;
                    goto REDO;
                }

                this.displayLine("WriteRes()\r\n"
                    + RestChannel.GetResultInfo(response));

                //this.displayLine("<br/>帐户=" + u.UserName + "<br/>"
                //    + "Rights=" + u.Rights + "<br/>"
                //    + "Access=" + u.Access + "<br/><br/>"
                //    + RestChannel.GetResultInfo(response));

                // 时间戳
                baTimestamp = response.baOutputTimestamp;

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }


        // 用WriteRes写xml
        public WriteResResponse WriteXml(UserInfo u,
          string strResPath,
          string strXml,
          bool isReader = false)
        {
            WriteResResponse response = null;

            this.displayLine("strResPath=" + strResPath);
            this.displayLine("提交的xml<br/>"
                + HttpUtility.HtmlEncode(DomUtil.GetIndentXml(strXml))
                + "<br/>");


            byte[] baContent = Encoding.UTF8.GetBytes(strXml);  //文本到二进制
            long lTotalLength = baContent.Length;
            string strRanges = "0-" + (baContent.Length - 1).ToString();

            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            REDO:
                response = channel.WriteRes(strResPath,
                   strRanges,
                   lTotalLength,
                   baContent,
                   "",
                   "",
                   baTimestamp);
                // 间戳不匹配，自动重试
                if (response.WriteResResult.ErrorCode == ErrorCode.TimestampMismatch)
                {
                    // 设上时间戳
                    baTimestamp = response.baOutputTimestamp;
                    goto REDO;
                }

                this.displayLine("WriteRes()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));

                // 时间戳
                baTimestamp = response.baOutputTimestamp;


                // 如果是违约金库记录 或者 预约到书库记录，加到待删除列表中，自动测试界面关闭时，会删除这些记录。
                string path = response.strOutputResPath;
                if (string.IsNullOrEmpty(path) == false)
                {
                    string dbName = StringUtil.GetDbName(path);
                    if (dbName == Env_arrivedDbName || dbName == Env_amerceDbName)
                    {
                        if (this._needDeletePath.IndexOf(path)==-1)
                            this._needDeletePath.Add(path);
                    }
                }

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        // 待删除记录
        public List<string> _needDeletePath = new List<string>();

        public WriteResResponse WriteResForDel(UserInfo u,
          string strResPath,
          bool isReader = false)
        {
            WriteResResponse response = null;

            this.displayLine("strResPath=" + strResPath);



            //byte[] baContent = Encoding.UTF8.GetBytes(strXml);  //文本到二进制
            //long lTotalLength = baContent.Length;
            //string strRanges = "0-" + (baContent.Length - 1).ToString();

            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            REDO:
                response = channel.WriteRes(strResPath,
                   "",//strRanges,
                   0,//lTotalLength,
                   null,//baContent,
                   "",
                   "delete",//strStyle
                   baTimestamp);
                // 间戳不匹配，自动重试
                if (response.WriteResResult.ErrorCode == ErrorCode.TimestampMismatch)
                {
                    // 设上时间戳
                    baTimestamp = response.baOutputTimestamp;
                    goto REDO;
                }

                this.displayLine("WriteRes()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));

                // 时间戳
                baTimestamp = response.baOutputTimestamp;

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        // 用WriteRes写xml
        public void Borrow(UserInfo u,
          string readerBarcode,
          string itemBarcode,
          bool isReader = false)
        {
            WriteResResponse response = null;

            this.displayLine("为读者" + readerBarcode + "借册" + itemBarcode);


            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

                int nRet = channel.Borrow(false,
                    readerBarcode,
                    itemBarcode,
                    out string outputReaderBarcode,
                    out string readerXml,
                    out BorrowInfo info,
                    out string strError);
                if (nRet == -1)
                    throw new Exception("借书失败:" + strError);


            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }


        public void ChangeReaderPasswordBySupervisor(string readerBarcode)
        {
            ChangeReaderPasswordResponse response = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(this.mainForm.GetSupervisorAccount());

                response = channel.ChangeReaderPassword(readerBarcode,
                    null,
                    "1");
                if (response.ChangeReaderPasswordResult.Value != 1)
                    throw new Exception("superviosr修改读者密码出错:" + response.ChangeReaderPasswordResult.ErrorInfo);


            }
            catch (Exception ex)
            {
                throw new Exception("superviosr修改读者密码异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        public GetResResponse GetRes(UserInfo u,
            string strResPath,
            bool isReader = false)
        {
            GetResResponse response = null;

            this.displayLine("strResPath=" + strResPath);

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

                response = channel.GetRes(strResPath,
                    0,
                    -1,
                    "data,metadata,outputpath,timestamp");

                this.displayLine("GetRes()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "GetRes异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }


        public GetRecordResponse GetRecord(UserInfo u,
    string strResPath,
    bool isReader = false)
        {
            GetRecordResponse response = null;

            this.displayLine("strResPath=" + strResPath);

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

                response = channel.GetRecord(strResPath);

                this.displayLine("GetRecord()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "GetRecord：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }


        public GetBrowseRecordsResponse GetBrowseRecords(UserInfo u,
string strResPath,
bool isReader = false)
        {
            GetBrowseRecordsResponse response = null;

            this.displayLine("strResPath=" + strResPath);

            return this.GetBrowseRecords(u,new string[] { strResPath},isReader);

            //RestChannel channel = null;
            //try
            //{
            //    // 用户登录
            //    channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            //    response = channel.GetBrowseRecords(new string[] { strResPath }, "id,cols,xml");

            //    this.displayLine("GetBrowseRecords()\r\n"
            //        + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
            //    return response;

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(u.UserName + "GetBrowseRecords：" + ex.Message);
            //}
            //finally
            //{
            //    if (channel != null)
            //        this.mainForm._channelPool.ReturnChannel(channel);
            //}
        }


        public GetBrowseRecordsResponse GetBrowseRecords(UserInfo u,
string[] pathList,
bool isReader = false)
        {
            GetBrowseRecordsResponse response = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

                response = channel.GetBrowseRecords(pathList, "id,cols,xml");

                this.displayLine("GetBrowseRecords()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "GetBrowseRecords：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        public LibraryServerResult DelXml(UserInfo u,
            string type,
            string strResPath,
            bool isReader)
        {
            if (type == C_Type_reader)
                return this.SetReaderInfo(u, "delete", strResPath, "", isReader).SetReaderInfoResult;
            else if (type == C_Type_biblio)
                return this.SetBiblioInfo(u, "delete", strResPath, "", isReader).SetBiblioInfoResult;
            else if (type == C_Type_item
                || type == C_Type_order
                || type == C_Type_comment
                || type == C_Type_issue)
            {
                return this.SetItemInfo1(u, type, "delete", strResPath, "", isReader, out string temp);

            }
            else if (type == C_Type_amerce
            || type == C_Type_arrived)
            {
                return this.WriteResForDel(u, strResPath, isReader).WriteResResult;

            }

            throw new Exception("DelXml不支持的类型" + type);
        }

        //SetReaderInfo
        public SetReaderInfoResponse SetReaderInfo(UserInfo u,
            string strAction,
            string strResPath,
             string strNewXml,
            bool isReader)
        {
            SetReaderInfoResponse response = null;

            this.displayLine("strResPath=" + strResPath);
            this.displayLine("提交的xml<br/>"
                + HttpUtility.HtmlEncode(DomUtil.GetIndentXml(strNewXml))
                + "<br/>");

            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            REDO:
                response = channel.SetReaderInfo(strAction,
                    strResPath,
                    strNewXml,
                    "",
                    baTimestamp);
                // 间戳不匹配，自动重试
                if (response.SetReaderInfoResult.ErrorCode == ErrorCode.TimestampMismatch)
                {
                    // 设上时间戳
                    baTimestamp = response.baNewTimestamp;
                    goto REDO;
                }

                this.displayLine("SetReaderInfo()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "GetRes异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        public GetReaderInfoResponse GetReaderInfo(UserInfo u,
    string strResPath,
    bool isReader)
        {
            GetReaderInfoResponse response = null;

            this.displayLine("strResPath=" + strResPath);


            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            REDO:
                response = channel.GetReaderInfo("@path:" + strResPath, "xml");


                this.displayLine("GetReaderInfo()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "GetReaderInfo 异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        // 用SetBiblioInfo
        public CopyBiblioInfoResponse CopyBiblio(UserInfo u,
          string strAction,
          string strBiblioRecPath,
          string strNewBiblioRecPath,
          string strNewBiblio,
          string strMergeStyle,
          bool isReader = false)
        {
            CopyBiblioInfoResponse response = null;

            //this.displayLine("strResPath=" + strResPath);
            //this.displayLine("提交的xml<br/>"
            //    + HttpUtility.HtmlEncode(DomUtil.GetIndentXml(strXml))
            //    + "<br/>");

            byte[] baTimestamp = null;
            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

                response = channel.CopyBiblioInfo(
                strAction,
                strBiblioRecPath,
                "xml",//strBiblioType,
                null,//strBiblio,
                baTimestamp,
                strNewBiblioRecPath,
                strNewBiblio,
                strMergeStyle);

                this.displayLine("CopyBiblioInfo()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));

                //// 显示返回信息
                //this.SetResultInfo("CopyBiblioInfo()\r\n" + RestChannel.GetResultInfo(response));

                return response;
            }
            catch (Exception ex)
            {
                this.displayLine(this.getRed("CopyBiblioInfo()异常\r\n"));
                return response;
                //throw new Exception(u.UserName + "CopyBiblioInfo()异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        // 用SetBiblioInfo
        public SetBiblioInfoResponse SetBiblioInfo(UserInfo u,
            string strAction,
          string strResPath,
          string strXml,
          bool isReader = false)
        {
            SetBiblioInfoResponse response = null;

            this.displayLine("strResPath=" + strResPath);
            this.displayLine("提交的xml<br/>"
                + HttpUtility.HtmlEncode(DomUtil.GetIndentXml(strXml))
                + "<br/>");

            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            REDO:
                response = channel.SetBiblioInfo(strAction,
                    strResPath,
                    "xml",
                    strXml,
                   baTimestamp,
                   "",
                   "");
                // 间戳不匹配，自动重试
                if (response.SetBiblioInfoResult.ErrorCode == ErrorCode.TimestampMismatch)
                {
                    // 设上时间戳
                    baTimestamp = response.baOutputTimestamp;
                    goto REDO;
                }

                this.displayLine("SetBiblioInfo()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
                //+ RestChannel.GetResultInfo(response)); ; ;

                // 时间戳
                baTimestamp = response.baOutputTimestamp;

                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "SetBiblioInfo()异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        public GetBiblioInfosResponse GetBiblioInfos(UserInfo u,
            string strResPath,
            bool isReader = false)
        {
            GetBiblioInfosResponse response = null;

            this.displayLine("strResPath=" + strResPath);

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

                string strformats = "xml";
                string[] formats = strformats.Split(new char[] { ',' });
                response = channel.GetBiblioInfos(strResPath, formats);

                this.displayLine("GetBiblioInfos()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));


                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "GetBiblioInfos()异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        public LibraryServerResult SetItemInfo1(UserInfo u,
            string type,
    string strAction,
  string strResPath,
  string strXml,
  bool isReader,
  out string strOutputRecPath)
        {

            string strDbType = this.type2dbtype(type);

            strOutputRecPath = "";
            this.displayLine("strResPath=" + strResPath);
            this.displayLine("提交的xml<br/>"
                + HttpUtility.HtmlEncode(DomUtil.GetIndentXml(strXml))
                + "<br/>");

            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            REDO:
                LibraryServerResult response = channel.SetItemInfo(strDbType,
                    strAction,
                    strResPath,
                    strXml,
                    baTimestamp,
                    "",//strStyle,
                    out strOutputRecPath,
                    out byte[] baOutputTimestamp);
                // 间戳不匹配，自动重试
                if (response.ErrorCode == ErrorCode.TimestampMismatch)
                {
                    // 设上时间戳
                    baTimestamp = baOutputTimestamp;
                    goto REDO;
                }

                this.displayLine("SetItemInfo()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response) + "\r\n"
                    + "strOutputRecPath:" + strOutputRecPath + "\r\n"
                    + "baOutputTimestamp:" + ByteArray.GetHexTimeStampString(baOutputTimestamp) + "\r\n"
                    ));


                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "SetBiblioInfo()异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        public GetItemInfoResponse GetItemInfo(UserInfo u,
                    string type,
                   string strResPath,
                    bool isReader = false)
        {

            string strItemDbType = type2dbtype(type);
            //if (type == C_Type_item)
            //    strItemDbType = "item";
            //else if (type == C_Type_issue)
            //    strItemDbType = "issue";
            //else if (type == C_Type_comment)
            //    strItemDbType = "comment";
            //else if (type == C_Type_order)
            //    strItemDbType = "order";
            //else
            //    throw new Exception("不支持的type[" + type + "]");

            string strBarcode = "@path:" + strResPath;

            return this.GetItemInfoInternel(u,
                strItemDbType,
                strBarcode,
                "xml",
                "xml",
                isReader);
        }

        // 获取册记录
        public GetItemInfoResponse GetItemInfoInternel(UserInfo u,
                    string strItemDbType,
                   string strBarcode,
                    string strResultType,
                    string strBiblioType,
                    bool isReader = false)
        {
            GetItemInfoResponse response = null;

            this.displayLine("strBarcode=" + strBarcode);

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

                response = channel.GetItemInfo(strItemDbType,
                   strBarcode,
                   "",//strItemXml,
                   strResultType,
                   strBiblioType);

                this.displayLine("GetItemInfo()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));


                return response;

            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "GetItemInfo()异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        private LibraryServerResult GetFirstEntity(UserInfo u,
            string type,
            string strBiblioRecPath,
            out EntityInfo entity,
            bool isReader = false)
        {
            entity = null;

            this.displayLine("strBiblioRecPath=" + strBiblioRecPath);


            EntityInfo[] entityinfos;

            LibraryServerResult result = this.GetEntities(u, type, strBiblioRecPath, 0, 1, "xml", out entityinfos, isReader);
            if (entityinfos != null && entityinfos.Length > 0)
                entity = entityinfos[0];

            this.displayLine("GetEntities()\r\n"
    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(result)));


            this.displayLine(HttpUtility.HtmlEncode(RestChannel.DisplayEntityInfos(entityinfos)));


            return result;

        }


        private LibraryServerResult GetEntities(UserInfo u,
                    string type,
                    string strBiblioRecPath,
                    long lStart,
                    long lCount,
                    string strStyle,
                    out EntityInfo[] entityinfos,
                    bool isReader = false)
        {
            string strLang = "";
            entityinfos = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);
                if (type == C_Type_item)
                {
                    GetEntitiesResponse response = channel.GetEntities(strBiblioRecPath,
                        lStart,
                        lCount,
                        strStyle,
                        strLang);

                    entityinfos = response.entityinfos;
                    return response.GetEntitiesResult;

                    // 显示返回信息
                    //this.SetResultInfo("GetEntities()\r\n" + RestChannel.GetResultInfo(response));
                }
                else if (type == C_Type_order)
                {
                    GetOrdersResponse response = channel.GetOrders(strBiblioRecPath,
                        lStart,
                        lCount,
                        strStyle,
                        strLang);

                    entityinfos = response.orderinfos;
                    return response.GetOrdersResult;

                    // 显示返回信息
                    //this.SetResultInfo("GetOrders()\r\n" + RestChannel.GetResultInfo(response));
                }
                else if (type == C_Type_issue)
                {
                    GetIssuesResponse response = channel.GetIssues(strBiblioRecPath,
                        lStart,
                        lCount,
                        strStyle,
                        strLang);

                    entityinfos = response.issueinfos;
                    return response.GetIssuesResult;

                    // 显示返回信息
                    //this.SetResultInfo("GetIssues()\r\n" + RestChannel.GetResultInfo(response));
                }
                else if (type == C_Type_comment)
                {
                    GetCommentsResponse response = channel.GetComments(strBiblioRecPath,
                        lStart,
                        lCount,
                        strStyle,
                        strLang);

                    entityinfos = response.commentinfos;
                    return response.GetCommentsResult;

                    // 显示返回信息
                    //this.SetResultInfo("GetComments()\r\n" + RestChannel.GetResultInfo(response));
                }
                else
                {
                    //MessageBox.Show(fun + "未完成");
                    throw new Exception("不支持的类型" + type);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("GetXXXs()异常：" + ex.Message);

            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        #endregion

        #region 书目存取定义与对象权限

        // 存取定义与对象权限
        private void button_accessAndObject_Click(object sender, EventArgs e)
        {
            this.EnableCtrls(false);
            try
            {

                // 清空输出
                ClearResult();

                //===
                //先用管理员身从那写一条书目记录， 带dprms:file
                string strBiblioPath = this.CreateBiblioBySupervisor(false, true);

                //===
                this.displayLine("以下是用不同帐户测试修改对象数据。");
                string objectPath = strBiblioPath + "/object/0";


                #region 第1组测试  存取定义重点为：setbiblioinfo=change

                //===
                // 第1组测试  存取定义重点为：setbiblioinfo=change
                this.displayLine(getLarge("第1组测试"));

                UserInfo u = NewUser(this.SetObjectRights(C_Type_biblio, true), ""
                    , this.Env_biblioDbName + ":setbiblioinfo=change|getbiblioinfo=*");
                // new UserInfo
                //{
                //    UserName = "u1",
                //    Password = "1",
                //    SetPassword = true,
                //    Rights = "setbiblioobject,getbiblioobject",
                //    Access = this.Env_biblioDbName+":setbiblioinfo=change|getbiblioinfo=*"
                //};
                ////创建帐号
                //this.NewUser(u);

                this.displayLine(GetBR() + getBold(u.UserName + "写对象，应成功。"));
                WriteResResponse response = this.WriteObject(u, objectPath);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                //删除帐号
                this.DelUser(u);

                #endregion

                #region 第2组测试  存取定义重点为：setbiblioinfo=change(606)

                this.displayLine(getLarge("第2组测试"));
                u = NewUser(this.SetObjectRights(C_Type_biblio, true), ""
                    , this.Env_biblioDbName + ":setbiblioinfo=change(606)|getbiblioinfo=*");

                //// 用u_a1_1帐户
                //u = new UserInfo
                //{
                //    UserName = "u2",
                //    Password = "1",
                //    SetPassword = true,
                //    Rights = "setbiblioobject,getbiblioobject",
                //    Access = this.Env_biblioDbName+":setbiblioinfo=change(606)|getbiblioinfo=*"
                //};
                ////创建帐号
                //this.NewUser(u);

                this.displayLine(GetBR() + getBold(u.UserName + "写对象，应成功。"));
                response = this.WriteObject(u, objectPath);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                //删除帐号
                this.DelUser(u);

                #endregion

                #region 第3组测试  存取定义重点为：setbiblioinfo=new

                this.displayLine(getLarge("第3组测试"));
                u = NewUser(this.SetObjectRights(C_Type_biblio, true), ""
                    , this.Env_biblioDbName + ":setbiblioinfo=new|getbiblioinfo=*");

                //// 用u_a1_1帐户
                //u = new UserInfo
                //{
                //    UserName = "u3",
                //    Password = "1",
                //    SetPassword = true,

                //    //new,应不能change对象
                //    Rights = "setbiblioobject,getbiblioobject",
                //    Access = this.Env_biblioDbName+ ":setbiblioinfo=new|getbiblioinfo=*"
                //};
                ////创建帐号
                //this.NewUser(u);

                this.displayLine(GetBR() + getBold(u.UserName + "写对象，应不成功。"));
                response = this.WriteObject(u, objectPath);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                //删除帐号
                this.DelUser(u);

                #endregion

                #region 第4组测试  存取定义重点为：setbiblioinfo=delete

                this.displayLine(getLarge("第4组测试"));
                u = NewUser(this.SetObjectRights(C_Type_biblio, true), ""
                    , this.Env_biblioDbName + ":setbiblioinfo=delete|getbiblioinfo=*");

                //u = new UserInfo
                //{
                //    UserName = "u4",
                //    Password = "1",
                //    SetPassword = true,

                //    //delete,应不能change对象
                //    Rights = "setbiblioobject,getbiblioobject",
                //    Access = this.Env_biblioDbName+":setbiblioinfo=delete|getbiblioinfo=*"
                //};
                ////创建帐号
                //this.NewUser(u);

                this.displayLine(GetBR() + getBold(u.UserName + "写对象，应不成功。"));
                response = this.WriteObject(u, objectPath);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                //删除帐号
                this.DelUser(u);

                #endregion

                #region 第5组测试  存取定义重点为：setbiblioinfo=*

                this.displayLine(getLarge("第5组测试"));
                u = NewUser(this.SetObjectRights(C_Type_biblio, true), ""
                    , this.Env_biblioDbName + ":setbiblioinfo=*|getbiblioinfo=*");

                ////u = new UserInfo
                ////{
                ////    UserName = "u5",
                ////    Password = "1",
                ////    SetPassword = true,

                ////    //通配符*表示new,change,and，应能change对象
                ////    Rights = "setbiblioobject,getbiblioobject",
                ////    Access = this.Env_biblioDbName+":setbiblioinfo=*|getbiblioinfo=*"
                ////};
                //////创建帐号
                ////this.NewUser(u);

                this.displayLine(GetBR() + getBold(u.UserName + "写对象，应成功。"));
                response = this.WriteObject(u, objectPath);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                //删除帐号
                this.DelUser(u);

                #endregion

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "书目存取定义与对象权限异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        #endregion

        #region 显示信息

        public string GetBR()
        {
            return "<br/>===<br/>";
        }
        public string getRed(string text)
        {
            return "<span style='color: red; font-weight:bold'>" + text + "</span>";
        }

        public string getWarn1(string text)
        {
            return "<span style='background-color:yellow; font-weight:bold'>" + text + "</span>";
        }

        public string getGreenBackgroud(string text)
        {
            return "<span style='background-color:green;'>" + text + "</span>";
        }

        public string getBold(string text)
        {
            return "<span style='font-weight:bold'>" + text + "</span>";
        }

        public string getLarge(string text)
        {
            return "<br/><span style='font-weight:bold;font-size:24px'>==" + text + "==</span><br/>";
        }

        public void displayLine(string line)
        {

            if (line == "符合预期")
                line = this.getGreenBackgroud(line);

            line = line.Replace("\r\n", "<br/>");

            AppendHtml(this.webBrowser1, line + "<br/>");

            Application.DoEvents();
        }

        public void ClearResult()
        {
            this.webBrowser1.DocumentText = "<html><body>test</body></html>";

            //Thread.Sleep(1000); 
            //this.textBox_result.Text = "";

            this.displayLine("");
        }

        /*public*/
        static void AppendHtml(WebBrowser webBrowser,
             string strHtml,
             bool bClear = false)
        {

            HtmlDocument doc = webBrowser.Document;

            if (doc == null)
            {
                webBrowser.Navigate("about:blank");
                doc = webBrowser.Document;
            }

            if (bClear == true)
                doc = doc.OpenNew(true);
            doc.Write(strHtml);

            // 保持末行可见
            // ScrollToEnd(webBrowser);
        }

        #endregion


        #region 检查xml与对象权限

        public const string C_Type_reader = "读者";
        public const string C_Type_biblio = "书目";

        public const string C_Type_item = "册";
        public const string C_Type_order = "订购";
        public const string C_Type_comment = "评注";
        public const string C_Type_issue = "期";

        public const string C_Type_amerce = "违约金";
        public const string C_Type_arrived = "预约到书";

        public string GetAppendPath(string type, string dbName = "")
        {
            if (type == C_Type_reader)
                return string.IsNullOrEmpty(dbName) == true ? this.Env_ZG_ReaderDbName + "/?" : dbName + "/?";
            else if (type == C_Type_biblio)
                return string.IsNullOrEmpty(dbName) == true ? this.Env_biblioDbName + "/?" : dbName + "/?";

            else if (type == C_Type_item)
                return this.Env_biblioDbName + "实体/?";
            else if (type == C_Type_order)
                return this.Env_biblioDbName + "订购/?";
            else if (type == C_Type_comment)
                return this.Env_biblioDbName + "评注/?";

            else if (type == C_Type_issue)
                return this.Env_biblioDbName + "期/?";

            else if (type == C_Type_amerce)
                return this.Env_amerceDbName + "/?";//"违约金/?";
            else if (type == C_Type_arrived)
                return this.Env_arrivedDbName + "/?";//"预约到书/?";


            throw new Exception("不支持的类型" + type);
        }


        // action:new/change/delete
        public string ChangeDprmsFile(string xml, string action)
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);

            XmlNode root = dom.DocumentElement;

            if (action == "new")
            {
                // 加两项file
                root.InnerXml = root.InnerXml
                    + "<dprms:file id='1' xmlns:dprms='http://dp2003.com/dprms' />"
                    + "<dprms:file id='2' xmlns:dprms='http://dp2003.com/dprms' />";

            }
            else if (action == "change")
            {

                //XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
                //nsmgr.AddNamespace("dprms", DpNs.dprms);

                XmlNodeList nodes = this.GetFileNodes(root);// root.SelectNodes("//dprms:file", nsmgr);

                // 修改第一项
                if (nodes.Count > 0)
                {
                    XmlNode node = nodes[0];
                    DomUtil.SetAttr(node, "a", Guid.NewGuid().ToString());   //给第一个dprms:file元素，加一个test属性，是guid
                }
                else
                {
                    throw new Exception("提供的xml缺dprms:file元素，无法修改");
                }
            }
            else if (action == "delete")
            {

                //XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
                //nsmgr.AddNamespace("dprms", DpNs.dprms);

                XmlNodeList nodes = this.GetFileNodes(root);// root.SelectNodes("//dprms:file", nsmgr);

                // 删除第3个file
                if (nodes.Count > 2)
                {
                    // 找到id=2的node，进行删除
                    XmlNode node = nodes[2];
                    if (DomUtil.GetAttr(node, "id") != "2")
                    {
                        node = nodes[1];
                        if (DomUtil.GetAttr(node, "id") != "2")
                            node = nodes[0];

                        if (DomUtil.GetAttr(node, "id") != "2")
                            throw new Exception("xml中3个dprms:file的元素，id属性都不为2。异常的数据。");
                    }


                    root.RemoveChild(node);
                }
                else
                {
                    throw new Exception("提供的xml不足3个dprms:file无法，不能删除第3个dprms:file。");
                }
            }


            return dom.OuterXml;

        }

        public string GetXml(string type, bool hasFile)
        {
            string xml = this.GetXml(type, hasFile, out string barcode);
            return xml;
        }

        // barcode，当读者 和 册时有意义
        public string GetXml(string type, bool hasFile, out string barcode, string parent = "")
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);
            barcode = temp.ToString().PadLeft(6, '0'); //Guid.NewGuid().ToString().ToUpper(); //

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='" + temp.ToString() + "'/>";

            // 得到parent
            if (string.IsNullOrEmpty(parent) == true)
            {
                parent = "";
                if (type == C_Type_item
                    || type == C_Type_order
                    || type == C_Type_comment)
                {
                    parent = this.GetBiblioParent(this.GetBiblioPath());
                }
                else if (type == C_Type_issue)
                {
                    parent = this.GetIssueParent();
                }
            }


            // 根据不同类型，准备xml
            if (type == C_Type_reader)
            {
                //barcode = "P" + barcode;
                //return "<root>"
                //    + "<barcode>" + barcode + "</barcode>"
                //    + "<name>小张" + barcode + "</name>"
                //    + "<readerType>"+this._readerType+"</readerType>"
                //    + "<displayName>显示名" + barcode + "</displayName>"
                //    + "<preference>个性化参数" + barcode + "</preference>"
                //    + dprmsfile
                //    + "</root>";

                return this.GetReaderXml(this.Env_ZG_PatronType, hasFile, out barcode);
            }
            else if (type == C_Type_biblio)
            {
                return @"<unimarc:record xmlns:dprms='http://dp2003.com/dprms' xmlns:unimarc='http://dp2003.com/UNIMARC'>
                                  <unimarc:leader>01887nam0 2200325   450 </unimarc:leader>
                                  <unimarc:datafield tag='200' ind1=' ' ind2=' '>"
                                   + "<unimarc:subfield code='a'>" + temp + "我爱我家</unimarc:subfield>"
                                  + "</unimarc:datafield>"
                                    + dprmsfile
                                    + "</unimarc:record>";
            }
            else if (type == C_Type_item)
            {
                return this.GetItemXml(this.Env_ZG_Location, this.Env_ZG_BookType, hasFile, out barcode, parent);


            }
            else if (type == C_Type_order)
            {
                return @"<root>
                              <parent>" + parent + @"</parent>
                              <index>1</index>
                              <catalogNo>1</catalogNo>
                              <seller>a</seller>
                              <source>b</source>
                              <range>20240101-20241231</range>
                              <issueCount>1</issueCount>
                              <copy>1</copy>
                              <price>CNY10</price>
                              <distribute>流通库:5</distribute>
                              <class>d</class>
                              <batchNo>2023-2-1</batchNo>"
                              + dprmsfile
                            + "</root>";

            }
            else if (type == C_Type_comment)
            {
                return @"<root>
                              <parent>" + parent + @"</parent>
                              <title>评注标题" + temp + @"</title>
                              <content>一本好书" + temp + @"</content>"
                              + "<creator>P001</creator>"   //实际不会按这个值来，而是自动写创建者。
                              + dprmsfile
                            + "</root>";

            }
            else if (type == C_Type_issue)
            {
                return @"<root>
                              <parent>" + parent + @"</parent>
                              <publishTime>20230101</publishTime>
                              <issue>1</issue>
                              <orderInfo>
                                <root>
                                  <parent>" + parent + @"</parent>
                                  <index>1</index>
                                  <seller>邮局</seller>
                                  <source>a</source>
                                  <range>20230101-20231231</range>
                                  <issueCount>12</issueCount>
                                  <copy>1</copy>
                                  <price>CNY20</price>
                                  <distribute>阅览室:1</distribute>
                                  <class>社科</class>
                                  <batchNo>2023-1-26</batchNo>
                                </root>
                              </orderInfo>"
                              + dprmsfile
                            + "</root>";
            }
            else if (type == C_Type_amerce)
            {
                return this.GetAmerceXml(Env_ZG_LibraryCode, "P001", "B001", Env_ZG_Location, hasFile);
            }
            else if (type == C_Type_arrived)
            {
                return this.GetArrivedXml(Env_ZG_LibraryCode, "P001", "B001", Env_ZG_Location, hasFile);  //这个读者和册也可能是不存在的，没有关系。

            }

            throw new Exception("GetXml不支持的数据类型" + type);
        }

        // 获取读者xml
        public string GetReaderXml(string readerType, bool hasFile, out string barcode)
        {

            Random rd = new Random();
            int temp = rd.Next(1, 999999);
            barcode = temp.ToString().PadLeft(6, '0'); //Guid.NewGuid().ToString().ToUpper(); //

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='" + temp.ToString() + "'/>";

            barcode = "P" + barcode;
            return "<root>"
                + "<barcode>" + barcode + "</barcode>"
                + "<name>小张" + barcode + "</name>"
                + "<readerType>" + readerType + "</readerType>"
                + "<displayName>显示名" + barcode + "</displayName>"
                + "<preference>个性化参数" + barcode + "</preference>"
                + dprmsfile
                + "</root>";
        }

        // 得到预约到书
        public string GetArrivedXml(string libraryCode, string readerBarcode, string itemBarcode, string location, bool hasFile)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='" + temp.ToString() + "'/>";

            return @"<root>
                        <state>arrived</state>
                        <itemBarcode>" + itemBarcode + @"</itemBarcode>
                        <onShelf>true</onShelf>
                        <readerBarcode>" + readerBarcode + @"</readerBarcode>
                        <notifyDate>Thu, 23 Feb 2023 14:36:41 +0800</notifyDate>
                        <refID>4c267d45-dd1e-474f-8709-e9d5c084002d</refID>
                        <libraryCode>" + libraryCode + @"</libraryCode>
                        <location>" + location + @"</location>
                        <accessNo>I242.43/S495</accessNo>"
                        + dprmsfile
                    + "</root>";


            /*
             <root>
    <state>arrived</state>
    <itemBarcode>B441924</itemBarcode>
    <itemRefID>
    </itemRefID>
    <onShelf>true</onShelf>
    <libraryCode>A馆</libraryCode>
    <readerBarcode>P713445</readerBarcode>
    <notifyDate>Sun, 05 Mar 2023 22:53:59 +0800</notifyDate>
    <refID>8e378e72-671f-4d11-95eb-bf2f6d00705e</refID>
    <location>A馆/A馆图书馆</location>
    <accessNo>
    </accessNo>
</root>
             */


        }


        // 创建违约金记录
        public string GetAmerceXml(string libraryCode, string readerBarcode, string itemBarcode, string location, bool hasFile)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='" + temp.ToString() + "'/>";

            return @"<root>
                            <itemBarcode>" + itemBarcode + @"</itemBarcode>
                            <location>" + location + @"</location>
                            <readerBarcode>" + readerBarcode + @"</readerBarcode>
                            <libraryCode>" + libraryCode + @"</libraryCode>
                            <state>amerced</state>
                            <id>637986612496654587-1</id>
                            <reason>超期。超 23天; 违约金因子: CNY1.0/day</reason>
                            <overduePeriod>23day</overduePeriod>
                            <price>CNY23</price>
                            <comment>
                            </comment>
                            <borrowDate>Thu, 21 Jul 2022 15:14:48 +0800</borrowDate>
                            <borrowPeriod>31day</borrowPeriod>
                            <borrowOperator>supervisor</borrowOperator>
                            <returnDate>Tue, 13 Sep 2022 10:20:49 +0800</returnDate>
                            <returnOperator>supervisor</returnOperator>
                            <operator>supervisor</operator>
                            <operTime>Tue, 13 Sep 2022 10:21:07 +0800</operTime>"
                          + dprmsfile
                    + "</root>";


            /*
<root>
   <itemBarcode>DPB000005</itemBarcode>
   <location>星洲学校/图书馆</location>
   <readerBarcode>P101</readerBarcode>
   <libraryCode>星洲学校</libraryCode>
   <state>amerced</state>
   <id>637999543337013048-1</id>
   <reason>超期。超 33天; 违约金因子: CNY1.0/day</reason>
   <overduePeriod>33day</overduePeriod>
   <price>CNY33</price>
   <comment>
   </comment>
   <borrowDate>Tue, 16 Aug 2022 17:25:45 +0800</borrowDate>
   <borrowPeriod>10day</borrowPeriod>
   <borrowOperator>supervisor</borrowOperator>
   <returnDate>Wed, 28 Sep 2022 09:32:13 +0800</returnDate>
   <returnOperator>supervisor</returnOperator>
   <operator>supervisor</operator>
   <operTime>Wed, 28 Sep 2022 09:32:50 +0800</operTime>
   <pauseStart>Wed, 28 Sep 2022 09:32:13 +0800</pauseStart>
</root>
            */
        }

        // 得到册记录
        public string GetItemXml(string location, string bookType, bool hasFile, out string barcode, string parent = "")
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);
            barcode = temp.ToString().PadLeft(6, '0'); //Guid.NewGuid().ToString().ToUpper(); //

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='" + temp.ToString() + "'/>";

            if (string.IsNullOrEmpty(parent) == true)
                parent = this.GetBiblioParent(this.GetBiblioPath());

            barcode = "B" + barcode;
            return "<root>"
                + "<parent>" + parent + "</parent>"
                + "<barcode>" + barcode + "</barcode>"
                + "<location>" + location + "</location>"
                + "<bookType>" + bookType + "</bookType>"
                + dprmsfile
                + "</root>";
        }

        // 供册/订购/评注使用的父亲路径
        public string _biblioPath = "";
        public string GetBiblioPath()
        {
            if (string.IsNullOrEmpty(_biblioPath) == true)
            {
                this._biblioPath = this.CreateBiblioBySupervisor(false, false);//response.strOutputResPath;
            }
            return _biblioPath;
        }

        public string CreateBiblioBySupervisor(bool bIssue, bool hasFile)
        {
            string info = "";
            // 用supervisor帐户创建一条书目
            string strResPath = this.Env_biblioDbName + "/?";
            //if (bIssue == true)
            //{
            //    strResPath = "中文期刊/?";
            //    info = "期刊";
            //}

            this.displayLine("用supervisor创建一条" + info + "书目，为相关测试提供支持。");

            WriteResResponse response = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                strResPath,
                 this.GetXml(C_Type_biblio, hasFile));

            if (response.WriteResResult.Value == -1)
                throw new Exception("用supervisor创建书目异常:" + response.WriteResResult.ErrorInfo);


            return response.strOutputResPath;
        }

        public string GetBiblioParent(string biblioPath)
        {
            int nIndex = biblioPath.LastIndexOf("/");
            if (nIndex != -1)
                return biblioPath.Substring(nIndex + 1);

            throw new Exception("书目路径[" + biblioPath + "]异常，未找到id部分。");
        }


        // 供册/订购/评注使用的父亲路径
        public string _issueBiblioPath = "";
        public string GetIssueBiblioPath()
        {
            if (string.IsNullOrEmpty(_issueBiblioPath) == true)
            {
                this._issueBiblioPath = this.CreateBiblioBySupervisor(true, false);//response.strOutputResPath;
            }
            return _issueBiblioPath;
        }

        public string GetIssueParent()
        {
            string biblioPath = GetIssueBiblioPath();
            int nIndex = biblioPath.LastIndexOf("/");
            if (nIndex != -1)
                return biblioPath.Substring(nIndex + 1);

            throw new Exception("书目路径[" + biblioPath + "]异常，未找到id部分。");
        }

        public UserInfo NewUser(string rights, string libraryCode, string access)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);

            UserInfo u = new UserInfo
            {
                UserName = "_u" + temp.ToString(),
                Password = "1",
                SetPassword = true,
                Rights = rights,
                Access = access,
                LibraryCode = libraryCode
            };

            // 先删除再新建
            try
            {
                DelUser(u);
            }
            catch (Exception ex)
            {
                // 有可能帐户不存天，没关系的。
            }

            //创建帐号
            this.NewUser1(u);

            return u;
        }


        public void checkRight(string type,
            List<string> rightsList,
            string workerLibraryCode)
        {
            if (rightsList == null || rightsList.Count != 9)
            {
                throw new Exception("必须输入规定的9组权限");
            }


            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                // 删除一下自动创建的帐户，有时中途退出，有的帐户没删除干净。
                DeleteAutoUsers();

                this.displayLine("");

                this.displayLine(getLarge(type + "及下级对象所需权限的测试结果"));


                string strResPath = GetAppendPath(type); //"读者/?";
                WriteResResponse response = null;

                // 第4组权限会写好xml路径和对象路径，供后面权限使用。
                string path_xmlHasFile = "";
                string path_object = "";

                #region 第1组测试  仅有setXXXinfo

                //===
                // 第1组测试  仅有setXXXinfo
                this.displayLine(getLarge("第1组测试"));
                UserInfo u = this.NewUser(rightsList[0], workerLibraryCode, "");

                this.displayLine(GetBR() + getBold(u.UserName + "写简单xml，由于没有连带的读xml权限，应不成功。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type, false));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，应拒绝，因为只定义了写权限未定义读权限，违反了权限规则。"));

                //删除帐号
                this.DelUser(u);

                #endregion


                #region 第2组测试 setXXXinfo, getXXXinfo

                //===
                // 第2组测试 setXXXinfo, getXXXinfo
                //可写简单读者xml，包括add/new/delete
                //不可操作xml中dprms:file，不能写对象
                this.displayLine(getLarge("第2组测试"));
                u = this.NewUser(rightsList[1], workerLibraryCode, "");

                // 新建简单xml
                this.displayLine(GetBR() + getBold(u.UserName + "新建简单xml，有读写xml权限，应新建成功。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type, false));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                string tempPath = response.strOutputResPath;

                // 修改简单xml
                this.displayLine(GetBR() + getBold(u.UserName + "修改简单xml，应修改成功，且错误码应为NoError。"));
                response = this.WriteXml(u, tempPath, this.GetXml(type, false));
                if (response.WriteResResult.Value == 0)
                {
                    if (type == C_Type_comment)  // 评注，因为提交的xml涉及到creator变化，所以错误码是部分成功
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.PartialDenied)
                            this.displayLine(getRed("不符合预期，错误码不对，应为PartialDenied。"));
                        else
                            this.displayLine("符合预期");
                    }
                    else
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                            this.displayLine(getRed("不符合预期，错误码不对，应为NoError。"));
                        else
                            this.displayLine("符合预期");
                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除简单xml
                this.displayLine(GetBR() + getBold(u.UserName + "删除简单xml，应删除成功。"));
                LibraryServerResult res = this.DelXml(u, type, tempPath, false);
                if (res.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));



                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "新建带dprms:file的xml，由于没有对象权限，应写入失败 或者 部分写入且过滤了file(注意观察提示)。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type, true));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                {
                    if (response.WriteResResult.ErrorCode == ErrorCode.NoError)
                    {
                        this.displayLine(getRed("不符合预期，错误码应为部分写入。"));
                    }
                    else
                    {
                        this.displayLine("符合预期");
                    }

                }
                path_xmlHasFile = response.strOutputResPath;
                path_object = path_xmlHasFile + "/object/0";

                // 不能写对象
                this.displayLine(GetBR() + getBold(u.UserName + "写对象数据，由于没有对象权限，应不能成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(u);

                #endregion


                #region 第3组测试  setXXXinfo,getXXXinfo,writeXXXobject

                //===
                // 第3组测试  setXXXinfo,getXXXinfo,writeXXXobject
                // 应报权限违约规则，直接拒绝
                this.displayLine(getLarge("第3组测试"));
                u = this.NewUser(rightsList[2], workerLibraryCode, "");

                // 无法写简单xml，因为写权限大于读者权限。
                this.displayLine(GetBR() + getBold(u.UserName + "新建简单xml，应报权限配置不合理，因为有写对象权限，缺读对象权限。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type, false));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，应报权限配置违反规则。"));

                /*

                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold(u.UserName+"新建带dprms:file的xml，由于没有连带的读对象权限，应不能成功，或者部分写入且过滤了file。"));
                response = this.WriteXml(u, strResPath, this.GetReaderXml(true));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                {
                    if (response.WriteResResult.Value == 0 && response.WriteResResult.ErrorCode != ErrorCode.NoError)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期"));
                }
                path_xmlHasFile = response.strOutputResPath;
                path_object = path_xmlHasFile + "/object/0";

                // 不能写对象
                this.displayLine(GetBR() + getBold(u.UserName+"写对象，由于没有连带的读对象权限，应不能成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));
                */


                // 删除帐号
                this.DelUser(u);

                #endregion


                #region 第4组测试 setXXXinfo,getXXXinfo,writeXXXobject,getXXXobject
                //===
                // 第4组测试 setXXXinfo,getXXXinfo,writeXXXobject,getXXXobject
                // 这是完整权限，即可读xml及操作里面的dprms:file，又可以写对象。
                this.displayLine(getLarge("第4组测试"));
                //u = new UserInfo
                //{
                //    UserName = "u4",
                //    Password = "1",
                //    SetPassword = true,
                //    Rights = rightsList[3],//"setreaderinfo,getreaderinfo,setreaderobject,getreaderobject",
                //    Access = ""
                //};
                ////创建帐号
                //this.NewUser(u);
                u = this.NewUser(rightsList[3], workerLibraryCode, "");

                // 此case无意义，因为前面当有setreaderinfo,getreaderinfo，已测过对简单xml的增删改
                //// 写简单xml
                //this.displayLine(GetBR() + getBold(u.UserName+"新建简单xml，应新建成功。"));
                //response = this.WriteXml(u, strResPath, this.GetReaderXml(false));
                //if (response.WriteResResult.Value == 0)
                //    this.displayLine("符合预期");
                //else
                //    this.displayLine(getRed("不符合预期"));

                #region 对带dprms:file的xml的新建/修改/删除

                // 新建带file的xml
                this.displayLine(GetBR() + getBold(u.UserName + "新建带dprms:file的xml，应新建成功。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type, true));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 下面针对上面新建的这条进行修改和删除
                tempPath = response.strOutputResPath;

                // 修改带file的xml
                this.displayLine(GetBR() + getBold(u.UserName + "修改带dprms:file的xml，应修改成功且NoError。"));
                response = this.WriteXml(u, tempPath, this.GetXml(type, true));
                if (response.WriteResResult.Value == 0)
                {
                    if (type == C_Type_comment)  // 评注，因为提交的xml涉及到creator变化，所以错误码是部分成功
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.PartialDenied)
                            this.displayLine(getRed("不符合预期，错误码不对，应为PartialDenied。"));
                        else
                            this.displayLine("符合预期");
                    }
                    else
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                            this.displayLine(getRed("不符合预期，错误码不对，应为NoError。"));
                        else
                            this.displayLine("符合预期");
                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除带dprms:file的xml              
                this.displayLine(GetBR() + getBold(u.UserName + "删除带dprms:file的xml，应删除成功。"));
                res = this.DelXml(u, type, tempPath, false);
                if (res.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 修改xml中的dprms:file,包括对dprms:file的new/change/delete

                // 再新建一条带dprms:file的记录，以便对其下级的dprms:file进行增/删/改
                this.displayLine(GetBR() + getBold(u.UserName + "再新建一条带dprms:file的记录，以便对其下级的dprms:file进行增/删/改。"));

                string originXml = this.GetXml(type, true);
                response = this.WriteXml(u, strResPath, originXml);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 方便后面的帐户使用
                path_xmlHasFile = response.strOutputResPath;
                path_object = path_xmlHasFile + "/object/0";


                // 本次对这个路径的记录中dprms:file进行增删改
                tempPath = response.strOutputResPath;

                // 新增两个dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "对xml新增两个dprms:file，此时应有3个dprms:file。"));
                string newFileXml = this.ChangeDprmsFile(originXml, "new");

                response = this.WriteXml(u, tempPath, newFileXml);
                if (response.WriteResResult.Value == 0)
                {

                    if (type == C_Type_comment)  // 评注，因为提交的xml涉及到creator变化，所以错误码是部分成功
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.PartialDenied)
                            this.displayLine(getRed("不符合预期，错误码不对，应为PartialDenied。"));
                    }
                    else
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                            this.displayLine(getRed("注意错误码不对，应为NoError。"));
                    }

                    // 获取出来看下
                    GetResResponse tempResponse = this.GetRes(u, tempPath);
                    if (tempResponse.GetResResult.Value >= 0)
                    {
                        string tempXml = Encoding.UTF8.GetString(tempResponse.baContent);
                        XmlNodeList fileNodes = this.GetFileNodes(tempXml);
                        if (fileNodes.Count == 3)
                        {
                            this.displayLine(this.getGreenBackgroud("xml中有3个dprms:file元素，符合预期"));
                        }
                        else
                        {
                            this.displayLine(this.getRed("xml中有" + fileNodes.Count + "个dprms:file元素，不符合预期，应有3个。"));
                        }

                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));

                // 修改第1个dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "对xml修改了第1个dprms:file，增加了a属性。"));
                string changeFileXml = this.ChangeDprmsFile(newFileXml, "change");

                response = this.WriteXml(u, tempPath, changeFileXml);
                if (response.WriteResResult.Value == 0)
                {
                    if (type == C_Type_comment)  // 评注，因为提交的xml涉及到creator变化，所以错误码是部分成功
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.PartialDenied)
                            this.displayLine(getRed("不符合预期，错误码不对，应为PartialDenied。"));
                    }
                    else
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                            this.displayLine(getRed("注意错误码不对，应为NoError。"));
                    }

                    // 获取出来看下
                    GetResResponse tempResponse = this.GetRes(u, tempPath);
                    if (tempResponse.GetResResult.Value >= 0)
                    {

                        string tempXml = Encoding.UTF8.GetString(tempResponse.baContent);
                        XmlNodeList fileNodes = this.GetFileNodes(tempXml);
                        if (fileNodes.Count >= 1)
                        {
                            XmlNode node = fileNodes[0];
                            string a = DomUtil.GetAttr(node, "a");
                            if (string.IsNullOrEmpty(a) == false)
                            {
                                this.displayLine(this.getGreenBackgroud("第1个dprms:file是否增加了a属性，符合预期"));
                            }
                            else
                            {
                                this.displayLine(this.getRed("第1个dprms:file没有增加a属性，修改没有兑现，不符合预期"));
                            }
                        }
                        else
                        {
                            this.displayLine(this.getRed("xml中dprms:file元素小于1，不符合预期。"));

                        }
                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除第3个dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "对xml删除第3个dprms:file，应成功。"));
                string delFileXml = this.ChangeDprmsFile(changeFileXml, "delete");

                response = this.WriteXml(u, tempPath, delFileXml);
                if (response.WriteResResult.Value == 0)
                {
                    if (type == C_Type_comment)  // 评注，因为提交的xml涉及到creator变化，所以错误码是部分成功
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.PartialDenied)
                            this.displayLine(getRed("不符合预期，错误码不对，应为PartialDenied。"));
                    }
                    else
                    {
                        if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                            this.displayLine(getRed("注意错误码不对，应为NoError。"));
                    }

                    // 获取出来看下
                    GetResResponse tempResponse = this.GetRes(u, tempPath);
                    if (tempResponse.GetResResult.Value >= 0)
                    {
                        string tempXml = Encoding.UTF8.GetString(tempResponse.baContent);
                        XmlNodeList fileNodes = this.GetFileNodes(tempXml);
                        if (fileNodes.Count == 2)
                        {
                            string id0 = DomUtil.GetAttr(fileNodes[0], "id");
                            string id1 = DomUtil.GetAttr(fileNodes[1], "id");
                            if (id0 == "0" && id1 == "1")
                            {
                                this.displayLine(this.getGreenBackgroud("确实将xml中的第3个dprms:file元素(@id='2')删除了，符合预期"));
                            }
                            else
                            {
                                this.displayLine(this.getRed("未将xml中的第3个dprms:file元素(@id='2')删除，不符合预期"));
                            }

                        }
                        else
                        {
                            this.displayLine(this.getRed("xml中有" + fileNodes.Count + "个dprms:file元素，不符合预期，删除一个后应剩2个。"));
                        }
                        //this.displayLine(this.getWarn1("请核对是否删除了第3个dprms:file。"));
                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));


                #endregion


                // 写对象
                this.displayLine(GetBR() + getBold(u.UserName + "写对象数据，应成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(u);

                #endregion


                #region 第5组测试 writeXXXobject

                //===
                // 第5组测试 writeXXXobject
                // 不能操作xml中的dprms:file，也不能change对象数据，写对象需要先有读对象权限，且还需读写xml的权限
                this.displayLine(getLarge("第5组测试"));
                //u = new UserInfo
                //{
                //    UserName = "u5",
                //    Password = "1",
                //    SetPassword = true,
                //    Rights = rightsList[4],//"setreaderobject",
                //    Access = ""
                //};
                ////创建帐号
                //this.NewUser(u);
                u = this.NewUser(rightsList[4], workerLibraryCode, "");

                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "写对象数据，由于没有连带的读对象权限 和 读写xml的权，应写不成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(u);

                #endregion


                #region 第6组测试  writeXXXobject, getXXXobject

                //==
                // 第6组测试  writeXXXobject, getXXXobject
                // 不能修改对象数据，因为没有连带的读者xml权限
                this.displayLine(getLarge("第6组测试"));
                //u = new UserInfo
                //{
                //    UserName = "u6",
                //    Password = "1",
                //    SetPassword = true,
                //    Rights = rightsList[5],//"setreaderobject, getreaderobject",
                //    Access = ""
                //};
                ////创建帐号
                //this.NewUser(u);
                u = this.NewUser(rightsList[5], workerLibraryCode, "");

                this.displayLine(GetBR() + getBold(u.UserName + "修改对象数据，因为没有连带的读者xml权限，应不成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 修改xml及dprms:file,应不能成功。
                this.displayLine(GetBR() + getBold(u.UserName + "修改xml及dprms:file，由于没有读写xml的权限，应不成功。"));
                response = this.WriteXml(u, path_xmlHasFile, this.GetXml(type, true));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(u);

                #endregion


                //=====以前为读权限=======

                #region 第7组测试 getXXXinfo

                //==
                //第7组测试 getXXXinfo
                //仅能获取xml普通节点，不能获取对象。
                this.displayLine(getLarge("第7组测试"));
                //u = new UserInfo
                //{
                //    UserName = "u7",
                //    Password = "1",
                //    SetPassword = true,
                //    Rights = rightsList[6],//"getreaderinfo",
                //    Access = ""
                //};
                ////创建帐号
                //this.NewUser(u);
                u = this.NewUser(rightsList[6], workerLibraryCode, "");

                //可获取xml，须过滤了dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "获取xml，应获取成功，且须过滤了dprms:file。"));
                GetResResponse getResponse = this.GetRes(u, path_xmlHasFile);

                if (getResponse.GetResResult.Value >= 0)
                {
                    string tempXml = Encoding.UTF8.GetString(getResponse.baContent);
                    XmlNodeList fileNodes = this.GetFileNodes(tempXml);
                    if (fileNodes == null || fileNodes.Count == 0)
                    {
                        this.displayLine(this.getGreenBackgroud("确实过滤了xml中的dprms:file元素，符合预期"));
                    }
                    else
                    {
                        this.displayLine(getRed("发现xml中存在dprms:file元素，不符合预期。无对象权限时，应过滤掉dprms:file。"));
                    }
                    //this.displayLine(getWarn1("请核对返回结果是否过滤了dprms:file。"));
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //删除帐号
                this.DelUser(u);

                #endregion

                #region 第8组测试 getXXXinfo,getXXXobject

                //===
                //第8组测试 getXXXinfo,getXXXobject
                //可读取xml且包含dprms:file，可获取对象
                this.displayLine(getLarge("第8组测试"));
                //u = new UserInfo
                //{
                //    UserName = "u8",
                //    Password = "1",
                //    SetPassword = true,
                //    Rights = rightsList[7],//"getreaderinfo,getreaderobject",
                //    Access = ""
                //};
                ////创建帐号
                //this.NewUser(u);
                u = this.NewUser(rightsList[7], workerLibraryCode, "");

                //可获取xml，须过滤了dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "获取xml，应获取成功，且包含dprms:file。"));
                getResponse = this.GetRes(u, path_xmlHasFile);
                if (getResponse.GetResResult.Value >= 0)
                {
                    string tempXml = Encoding.UTF8.GetString(getResponse.baContent);
                    XmlNodeList fileNodes = this.GetFileNodes(tempXml);
                    if (fileNodes.Count > 0)
                    {
                        this.displayLine(this.getGreenBackgroud("xml中包含dprms:file元素，符合预期"));
                    }
                    else
                    {
                        this.displayLine(getRed("发现xml中没有dprms:file元素，不符合预期。有对象权限时，应能返回dprms:file。"));
                    }
                    //this.displayLine(getWarn1("请核对返回结果中应包含dprms:file。"));
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //可获取对象
                this.displayLine(GetBR() + getBold(u.UserName + "获取对象，应获取成功。"));
                getResponse = this.GetRes(u, path_object);
                if (getResponse.GetResResult.Value >= 0)
                {
                    this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //删除帐号
                this.DelUser(u);

                #endregion

                #region 第9组测试 getXXXobject

                //===
                //第9组测试 getXXXobject
                //什么都不能做，由于对象权限不能独立存在
                this.displayLine(getLarge("第9组测试"));
                //u = new UserInfo
                //{
                //    UserName = "u9",
                //    Password = "1",
                //    SetPassword = true,
                //    Rights = rightsList[8],//"getreaderobject",
                //    Access = ""
                //};

                ////创建帐号
                //this.NewUser(u);
                u = this.NewUser(rightsList[8], workerLibraryCode, "");

                //不能获取对象
                this.displayLine(GetBR() + getBold(u.UserName + "获取对象，应不成功，由于对象权限不能独立存在。"));
                getResponse = this.GetRes(u, path_object);
                if (getResponse.GetResResult.Value == -1)
                {
                    this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //不能获取xml
                this.displayLine(GetBR() + getBold(u.UserName + "获取xml，由于没有读xml权限，应不成功。"));
                getResponse = this.GetRes(u, path_xmlHasFile);
                if (getResponse.GetResResult.Value == -1)
                {
                    this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //删除帐号
                this.DelUser(u);

                #endregion

            }
            catch (Exception ex)
            {

                MessageBox.Show( ex.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        public XmlNodeList GetFileNodes(XmlNode node)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("dprms", DpNs.dprms);

            XmlNodeList nodes = node.SelectNodes("//dprms:file", nsmgr);
            return nodes;
        }

        public XmlNodeList GetFileNodes(string xml)
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);
            return this.GetFileNodes(dom.DocumentElement);
        }



        #endregion

        #region 权限

        public string GetInfoRights1(string type)
        {
            if (type == C_Type_reader)
                return "getreaderinfo";
            else if (type == C_Type_biblio)
                return "getbiblioinfo";
            else if (type == C_Type_item)
                return "getiteminfo";
            else if (type == C_Type_comment)
                return "getcommentinfo";
            else if (type == C_Type_order)
                return "getorderinfo";
            else if (type == C_Type_issue)
                return "getissueinfo";
            else if (type == C_Type_amerce)
                return "getamerceinfo";
            else if (type == C_Type_arrived)
                return "getarrivedinfo";
            else
                throw new Exception("GetFullRights不支持的类型");

        }
        public string SetInfoRights(string type, bool full = false)
        {
            string rights = "";
            if (type == C_Type_reader)
                rights = "setreaderinfo";
            else if (type == C_Type_biblio)
                rights = "setbiblioinfo";
            else if (type == C_Type_item)
                rights = "setiteminfo";
            else if (type == C_Type_comment)
                rights = "setcommentinfo";
            else if (type == C_Type_order)
                rights = "setorderinfo";
            else if (type == C_Type_issue)
                rights = "setissueinfo";
            else if (type == C_Type_amerce)
                rights = "setamerceinfo";
            else if (type == C_Type_arrived)
                rights = "setarrivedinfo";
            else
                throw new Exception("GetFullRights不支持的类型");

            if (full == true)
                rights += "," + this.GetInfoRights1(type);

            return rights;
        }

        public string SetObjectRights(string type, bool full = false)
        {
            string rights = "";
            if (type == C_Type_reader)
                rights = "setreaderobject";
            else if (type == C_Type_biblio)
                rights = "setbiblioobject";
            else if (type == C_Type_item)
                rights = "setitemobject";
            else if (type == C_Type_comment)
                rights = "setcommentobject";
            else if (type == C_Type_order)
                rights = "setorderobject";
            else if (type == C_Type_issue)
                rights = "setissueobject";
            else if (type == C_Type_amerce)
                rights = "setamerceobject";
            else if (type == C_Type_arrived)
                rights = "setarrivedobject";
            else
                throw new Exception("GetFullRights不支持的类型");

            // 如果是完整权限，加上getxxxobject
            if (full == true)
                rights += "," + this.getObjectRight(type);

            return rights;
        }

        public string getObjectRight(string type)
        {
            if (type == C_Type_reader)
                return "getreaderobject";
            else if (type == C_Type_biblio)
                return "getbiblioobject";
            else if (type == C_Type_item)
                return "getitemobject";
            else if (type == C_Type_comment)
                return "getcommentobject";
            else if (type == C_Type_order)
                return "getorderobject";
            else if (type == C_Type_issue)
                return "getissueobject";
            else if (type == C_Type_amerce)
                return "getamerceobject";
            else if (type == C_Type_arrived)
                return "getarrivedobject";
            else
                throw new Exception("GetFullRights不支持的类型");
        }


        public string SetFullRights(string type)
        {
            return this.SetInfoRights(type, true) + "," + this.SetObjectRights(type, true);

            /*
            if (type == C_Type_reader)
            {
                return "setreaderinfo,getreaderinfo,setreaderobject,getreaderobject";
            }
            else if (type == C_Type_biblio)
            {
                return "setbiblioinfo,getbiblioinfo,setbiblioobject,getbiblioobject";
            }
            else if (type == C_Type_item)
            {
                return "setiteminfo,getiteminfo,setitemobject,getitemobject";
            }
            else if (type == C_Type_comment)
            {
                return "setcommentinfo,getcommentinfo,setcommentobject,getcommentobject";
            }
            else if (type == C_Type_order)
            {
                return "setorderinfo,getorderinfo,setorderobject,getorderobject";
            }
            else if (type == C_Type_issue)
            {
                return "setissueinfo,getissueinfo,setissueobject,getissueobject";
            }
            else if (type == C_Type_Amerce)
            {
                return "setamerceinfo,getamerceinfo,setamerceobject,getamerceobject";
            }
            else if (type == C_Type_Arrived)
            {
                return "setarrivedinfo,getarrivedinfo,setarrivedobject,getarrivedobject";
            }
            else
            {
                throw new Exception("GetFullRights不支持的类型");
            }
            */
        }


        public List<string> Get9rights(string type)
        {
            List<string> rightsList = new List<string>();

            rightsList.Add(this.SetInfoRights(type));//"setreaderinfo");
            rightsList.Add(this.SetInfoRights(type, true));//"setreaderinfo,getreaderinfo");//有用
            rightsList.Add(this.SetInfoRights(type, true) + "," + this.SetObjectRights(type));//"setreaderinfo,getreaderinfo,setreaderobject");
            rightsList.Add(this.SetInfoRights(type, true) + "," + this.SetObjectRights(type, true));//"setreaderinfo,getreaderinfo,setreaderobject,getreaderobject");//有用

            rightsList.Add(this.SetObjectRights(type));// "setreaderobject");
            rightsList.Add(this.SetObjectRights(type, true));//"setreaderobject, getreaderobject");

            rightsList.Add(this.GetInfoRights1(type));//"getreaderinfo");//有用
            rightsList.Add(this.GetInfoRights1(type) + "," + this.getObjectRight(type));// "getreaderinfo,getreaderobject");//有用
            rightsList.Add(this.getObjectRight(type));//"getreaderobject");

            return rightsList;
        }

        #endregion

        private void button_testRight_Click(object sender, EventArgs e)
        {
            string type = this.comboBox_TestRight_type.Text.Trim();


            List<string> rightsList = this.Get9rights(type);


            // 调检查权限函数
            this.checkRight(type,
                rightsList,
                "");
        }

        //// 个人书斋，册的馆藏地
        //public string _location = "流通库";

        //public string _readerType = "本科生";
        //public string _bookType = "普通";

        public string CreateReaderBySuperviosr(string readerDbName,
            string readerType,
            string rights,
            string personalLibrary,
            out string readerBarcode)
        {
            string xmlPath = "";

            string readerxml = this.GetReaderXml(readerType, true, out readerBarcode);

            if (string.IsNullOrEmpty(rights) == false
                || string.IsNullOrEmpty(personalLibrary) == false)
            {
                // 修改一下权限
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(readerxml);
                XmlNode root = dom.DocumentElement;

                if (string.IsNullOrEmpty(rights) == false)
                    root.InnerXml += "<rights>" + rights + "</rights>";

                if (string.IsNullOrEmpty(personalLibrary) == false)
                    root.InnerXml += "<personalLibrary>" + personalLibrary + "</personalLibrary>";

                readerxml = dom.OuterXml;
            }

            this.displayLine(this.getBold("用管理员身份创建读者" + readerBarcode + "，为相关操作提供支持。"));

            //if (string.IsNullOrEmpty(readerDbName))

            string strReaderPath = readerDbName + "/?";//this.GetAppendPath(C_Type_reader, readerDbName);
            WriteResResponse response = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                strReaderPath,
                 readerxml);
            if (response.WriteResResult.Value == -1)
                throw new Exception("用管理员身份创建读者异常:" + response.WriteResResult.ErrorInfo);


            //写一下对象，后面获取的时候方便看。
            xmlPath = response.strOutputResPath;
            string objectPath = xmlPath + "/object/0";
            response = this.WriteObject(this.mainForm.GetSupervisorAccount(),
                objectPath,
                false);
            if (response.WriteResResult.Value == -1)
                throw new Exception("用管理员身份创建读者对象异常:" + response.WriteResResult.ErrorInfo);

            return xmlPath;
        }

        public UserInfo NewReaderUser(string readerDbName, string readerType,
            string rights, string personalLibrary,
            out string readerBarcode,
            out string readerPath)
        {
            // 用管理员帐号创建一个读者
            readerPath = this.CreateReaderBySuperviosr(readerDbName, readerType,
                 rights,//"setreaderinfo,getreaderinfo,setreaderobject,getreaderobject",
                 personalLibrary,
                 out readerBarcode);

            // 修改读者的密码
            this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
            this.ChangeReaderPasswordBySupervisor(readerBarcode);
            UserInfo u = new UserInfo
            {
                UserName = readerBarcode,
                Password = "1",
            };

            return u;
        }




        private void button_readerLogin_comment_Click(object sender, EventArgs e)
        {
            /*
            准备环境:
            管理员创建一个读者
            管理员写一条评注，作为别人的评注，看读者是否能操作。

            读者新建评注，用WriteRes/SetItemInfo来测

            读者修改评注：可修改自己的，不能修改别人。

            P477571修改自己的评注xml的title，应成功。
            P477571修改自己的评注xml的creator，应失败。
            P477571修改自己的评注的对象，应成功。

            读者删除评注：可删除自己的，不能删除别人

            ===
            读者帐号如果有 managecomment 权限，可以修改和删除他人评注。
            注意managecomment权限，目前是按增强权限的思路开发，即不能少了基础的setcommentinfo,getcommentinfo,setcommentobject,getcommentobject。
            也就是说读者有了基本的4个权限，就可以发表修改删除自己的书评，有了managecomment权限后，还可以管理别人的书评。
            这个设计思路挺清晰的，目前不需要调整。



            工作人员与有管理权限的读者 修改他人书评时，针对parent 和create的说明:

            parent元素表示这条书评归属的书目，关于parent的修改有两种做法：
            一是change api只管修改其它信息，不让修改parent，另外专门做一个changeparent修改他人。
            二是工作人员可以直接通过change api修改，修改人直接负责好parent。
            目前程序是按第二种方式实现的，可以接受。

            creator元素表示这条书评的创建者，目前工作人员或者有管理权限的读者，是可以直接修改或删除creator的，这里有一定安全风险。
            比如一个人发表（创建）了书评，被后台管理员或者有管理权限的读者 通过接口 改为了其它人，就说不清楚了。
            所以建议不论是什么身份修改书评，都不能修改creator这个元素。
            还有一个理由就是creator元素是自动创建的，不论前端提交的xml有没有creator都不认。所以也不支持修改。
            
            *** 注意这里
            可以理解为一个受保护的元素，前端不提交的时候也不报错。

            



            目前馆员是可以直接修改的。
            */

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;
                LibraryServerResult ret = null;
                string newPath = this.GetAppendPath(C_Type_comment);
                string otherPath = "";  //他人的评注路径
                string otherObject = "";

                string ownerPathForChange = "";  //自己创建的，用于修改
                string ownerObjectForChange = "";

                string ownerPathForDelete = "";   // 自己创建的，用于删除

                #region 创建环境

                this.displayLine(this.getLarge("创建环境"));

                // 用管理员帐号创建一个读者，以下此读者帐号操作
                UserInfo u = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType, this.SetFullRights(C_Type_comment), "",
                    out string readerBarcode,
                    out string ownerReaderPath);

                //// 用supervisor帐户创建一个读者，有册的读权限，后面用此读者帐户操作
                //this.displayLine(this.getLarge("用supervisor帐户创建一个读者，有评论的完整权限，后面用此读者帐户操作"));
                //string readerBarcode = "";
                //writeRes = this.CreateReaderBySuperviosr("读者",this.GetFullRights(C_Type_comment),// "setcommentinfo,getcommentinfo,setcommentobject,getcommentobject",
                //    "",  //个人书斋
                //    out readerBarcode);
                //if (writeRes.WriteResResult.Value == -1)
                //    throw new Exception("管理员创建读者异常：" + writeRes.WriteResResult.ErrorInfo);
                //string readerOwnerPath = writeRes.strOutputResPath;

                //// 修改读者的密码
                //this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                //this.ChangeReaderPasswordBySupervisor(readerBarcode);
                //UserInfo u = new UserInfo
                //{
                //    UserName = readerBarcode,
                //    Password = "1",
                //};


                // 用supervisor创建一条评注
                this.displayLine(this.getLarge("用supervisor创建一条评注，作为别人的评注，看读者是否能操作"));

                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     GetXml(C_Type_comment, true));
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建评注异常：" + writeRes.WriteResResult.ErrorInfo);

                // 他人评注路径
                otherPath = writeRes.strOutputResPath;
                otherObject = otherPath + "/object/0";




                #endregion




                #region 第1组测试：读者应可以新建评注

                this.displayLine(this.getLarge("第1组测试：读者应可以新建评注"));


                // 用WriteRes新建评注
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes新建评注。"));
                writeRes = this.WriteXml(u, newPath,
                    this.GetXml(C_Type_comment, true), true);
                if (writeRes.WriteResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 这条记录后面用来修改
                ownerPathForChange = writeRes.strOutputResPath;
                ownerObjectForChange = ownerPathForChange + "/object/0";


                ///用 SetItemInfo 新建评注
                this.displayLine(GetBR() + getBold(u.UserName + "用 SetItemInfo 新建评注。"));
                ret = this.SetItemInfo1(u,
                    C_Type_comment,//"comment",
                    "new",
                    newPath,
                    this.GetXml(C_Type_comment, true), true, out ownerPathForDelete);
                if (ret.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // ownerPathForDelete= writeRes.strOutputResPath;

                #endregion


                #region 第2组测试：读者修改评注

                this.displayLine(this.getLarge("第2组测试：读者修改评注，应可修改自己的部分字段，不能修改他人。"));


                // 修改自己的评注

                this.displayLine(GetBR() + getBold(u.UserName + "先获取自己的书评xml，再在此基础上修改，应成功。"));
                GetItemInfoResponse getItemResponse = this.GetItemInfo(u, C_Type_comment, ownerPathForChange, true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                {
                    //this.displayLine("符合预期");

                    string tempXml = getItemResponse.strResult;
                    // 修改一下title
                    XmlDocument dom = new XmlDocument();
                    dom.LoadXml(tempXml);
                    DomUtil.SetElementText(dom.DocumentElement, "title", "标题修改1");
                    DomUtil.SetElementText(dom.DocumentElement, "content", "内容修改1");

                    this.displayLine(GetBR() + getBold(u.UserName + "修改自己的评注xml的title，应成功。"));
                    writeRes = this.WriteXml(u, ownerPathForChange,
                        dom.OuterXml, true);
                    if (writeRes.WriteResResult.Value >= 0)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期"));



                    // 修改自己的creator等元素
                    DomUtil.SetElementText(dom.DocumentElement, "title", "标题修改2");
                    DomUtil.SetElementText(dom.DocumentElement, "content", "内容修改2");
                    DomUtil.SetElementText(dom.DocumentElement, "creator", "创建者修改2");

                    this.displayLine(GetBR() + getBold(u.UserName + "修改自己的评注xml的creator等元素，应部分成功。"));
                    writeRes = this.WriteXml(u, ownerPathForChange,
                        dom.OuterXml, true);
                    if (writeRes.WriteResResult.Value == 0
                        && writeRes.WriteResResult.ErrorCode == ErrorCode.PartialDenied)  // 返回值是0，但有错误码，部分成功
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期,应部分成功。"));
                }
                else
                    this.displayLine(getRed("不符合预期"));



                // 修改自己评注下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "修改自己的评注的对象，应成功。"));
                writeRes = this.WriteObject(u, ownerObjectForChange, true);
                if (writeRes.WriteResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                //==修改他人的评注和对象

                //修改他的人评注
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的评注xml，应失败。"));
                ret = this.SetItemInfo1(u,
                    C_Type_comment,//"comment",
                    "change",
                    otherPath,
                    this.GetXml(C_Type_comment, false), true, out string temp);
                if (ret.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                // 修改他人评注下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的评注的对象，应失败。"));
                writeRes = this.WriteObject(u, otherObject, true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion


                #region 第3组测试：读者获取书评和对象，应成功

                this.displayLine(this.getLarge("第2组测试：读者获取自己和他人的书评和对象。"));


                // 用GetItemInfo()获取他人书评
                this.displayLine(GetBR() + getBold(u.UserName + "获取他人的书评xml，应成功。"));
                getItemResponse = this.GetItemInfo(u, C_Type_comment, otherPath, true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetItemInfo()获取他人书评对象
                this.displayLine(GetBR() + getBold(u.UserName + "获取他人的书评下对象，应成功。"));
                GetResResponse getRes = this.GetRes(u, otherObject, true);
                if (getRes.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));



                // 用GetItemInfo()获取自己书评
                this.displayLine(GetBR() + getBold(u.UserName + "获取自己的书评xml，应成功。"));
                getItemResponse = this.GetItemInfo(u, C_Type_comment, ownerPathForChange, true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetItemInfo()获取自己书评对象
                this.displayLine(GetBR() + getBold(u.UserName + "获取自己的书评下对象，应成功。"));
                getRes = this.GetRes(u, ownerObjectForChange, true);
                if (getRes.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第4组测试：读者删除评注,可删除自己的？不能删除别人

                this.displayLine(this.getLarge("第4测试：读者删除评注"));


                // 删除自己的评注
                this.displayLine(GetBR() + getBold(u.UserName + "删除自己的评注，应成功。"));
                ret = this.DelXml(u, C_Type_comment, ownerPathForDelete, true);
                if (ret.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应可以删除自己的评注。"));

                // 删除他人的
                this.displayLine(GetBR() + getBold(u.UserName + "删除他人的评注，应失败。"));
                ret = this.DelXml(u, C_Type_comment, otherPath, true);
                if (ret.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者不能删除他人的评注。"));

                #endregion


                #region 第5组测试：读者帐号有 managecomment 权限，应可以修改和删除他人评注。

                // 用管理员帐号创建一个读者，有managecomment权限，以下此读者帐号操作
                u = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType, "managecomment," + this.SetFullRights(C_Type_comment), "",
                    out string readerBarcode1,
                   out string tempPath);

                //// 用supervisor帐户创建一个读者，有managecomment权限，后面用此读者帐号操作他人评注，应成功。
                //this.displayLine(this.getLarge("读者帐号有 managecomment 权限，应可以修改和删除他人评注"));
                //string reader2 = "";
                //writeRes = this.CreateReaderBySuperviosr("读者","managecomment,"+this.GetFullRights(C_Type_comment),//setcommentinfo,getcommentinfo,setcommentobject,getcommentobject",
                //    "",  //个人书斋
                //    out reader2);
                //if (writeRes.WriteResResult.Value == -1)
                //    throw new Exception("管理员创建读者异常：" + writeRes.WriteResResult.ErrorInfo);
                ////string readerOwnerPath = writeRes.strOutputResPath;

                //// 修改读者的密码
                //this.displayLine(this.getBold("修改读者" + reader2 + "的密码，后面用此读者身份登录。"));
                //this.ChangeReaderPasswordBySupervisor(reader2);
                //u = new UserInfo
                //{
                //    UserName = reader2,
                //    Password = "1",
                //};


                //修改他的人评注
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的评注xml，应成功。"));
                ret = this.SetItemInfo1(u,
                    C_Type_comment,//"comment",
                    "change",
                    otherPath,
                    this.GetXml(C_Type_comment, true), true, out string temp1);
                if (ret.Value == 0 && ret.ErrorCode == ErrorCode.PartialDenied)  //因为提交的xml涉及到creator，所以应部分成功
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者有managecomment权限，应可修改他人的评注。如果提交的xml涉及到改变creator，则应部分成功。"));


                // 修改他人评注下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的评注的对象，应成功。"));
                writeRes = this.WriteObject(u, otherObject, true);
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者有managecomment权限，应可修改他人评注的对象。"));


                // 删除他人的
                this.displayLine(GetBR() + getBold(u.UserName + "删除他人的评注，应成功。"));
                ret = this.DelXml(u, C_Type_comment, otherPath, true);
                if (ret.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者有managecomment权限，应可删除他人的评注。"));

                #endregion


                /*
###  2023/2/27 23:10:34
最新代码已经 push 到 GitHub 了。注意测试创建和修改评注记录的时候，
                读者身份可能需要在记录中包含 follow 和 orderSuggestion 元素。
                用法可以参考一下 dp2OPAC 创建和修改的评注记录，比如 follow 应该是跟帖的时候会产生，
                orderSuggestion 应该是订购建议


### 2023/2/28 10:30
最新 dp2 代码已经 push 到 GitHub 了。
关于评注记录的测试，还需要注意一下几点：
1) 读者已经注销的(读者记录 state 元素中包含“注销”)，不能用该读者身份创建、修改、删除任何评注记录；
2) 评注记录的 state 元素中包含“锁定”的，非 managecomment 身份不允许修改和删除;
锁定功能可以参考 dp2OPAC 界面，应该就是给评注记录状态添加一个“锁定”值

                1）读者加了managecomment，则完全操作他人评注。
2）不能修改creator，可修改title和content。
3）读者不能修改状态。
4）管理员也需要加managecomment才能修改评注。

                 */

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份获取评注-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        private void button_readerLogin_arrived_Click(object sender, EventArgs e)
        {

            //确保准备好总分馆环境
            EnsureZfgEnv();

            string accountType = this.comboBox_accountType.Text.Trim();

            if (accountType == C_accountType_zgworker)
            {
                this.CheckArrivedAmerceForZgWorker(C_Type_arrived);
            }
            else if (accountType == C_accountType_zgreader)
            {
                this.CheckArrivedAmerceForZgReader(C_Type_arrived);
            }
            else if (accountType == C_accountType_fgworker)
            {
                CheckArrivedAmerceForFgWorker(C_Type_arrived);
            }
            else if (accountType == C_accountType_fgreader)
            {
                this.CheckArrivedAmerceForFgReader(C_Type_arrived);
            }
            else
                throw new Exception("不支持的身份类型" + accountType);



        }


        // 总馆读者身份操作预约到书 或 违约金
        public void CheckArrivedAmerceForZgReader(string type)
        {
            /*
            准备环境：
            管理员为总馆创建两个读者，两个册，
            读者，应可以获取/创建/修改/删除 预约者是自己的预约到书记录。
            不能获取/创建/修改/删除 他人的预约到书记录
            */
            string rights = this.SetFullRights(type);

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;

                // 目前此函数仅支持 预约到书 和 违约金
                if (type != C_Type_arrived && type != C_Type_amerce)
                    throw new Exception("CheckArrivedAmerce()不支持的类型" + type);


                #region 环境准备

                // 用管理员帐号创建一个读者，有managecomment权限，以下此读者帐号操作
                UserInfo u = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType,
                    rights, "",
                    out string readerBarcode,
                   out string tempPath);

                // 用supervisor创建第2个读者
                this.CreateReaderBySuperviosr(Env_ZG_ReaderDbName, Env_ZG_PatronType, "", "", out string reader2);


                // 用管理员身份创建两个册记录
                this.displayLine(this.getLarge("用管理员身份创建2条册记录。"));
                this.CreateItemBySupervisor(Env_ZG_Location, Env_ZG_BookType, out string itemBarcode1);
                this.CreateItemBySupervisor(Env_ZG_Location, Env_ZG_BookType, out string itemBarcode2);

                // 用管理员身份创建两条预约到书记录
                this.displayLine(this.getLarge("用管理员身份创建2条预约到书记录。"));
                // 预约者是自己的记录
                string ownerXmlPath = this.CreateArrivedAmerceBySupervisor(type, Env_ZG_LibraryCode, readerBarcode, itemBarcode1, Env_ZG_Location);
                // 预约者是他人的记录
                string otherXmlPath = this.CreateArrivedAmerceBySupervisor(type, Env_ZG_LibraryCode, reader2, itemBarcode2, Env_ZG_Location);


                #endregion

                //==读者操作自己的记录===
                this.displayLine(this.getLarge("第1组测试：读者操作自己的" + type + "记录，应成功"));
                this.DoRes1(u, type, ownerXmlPath, "get", true, true, "");

                string xml = this.GetArrivedOrAmerceXml(type, Env_ZG_LibraryCode, readerBarcode, itemBarcode1 + "-1", Env_ZG_Location);
                this.DoResMultiple(u, type, ownerXmlPath, "change,delete", true, true, xml);

                string newPath = this.GetAppendPath(type);
                xml = this.GetArrivedOrAmerceXml(type, Env_ZG_LibraryCode, readerBarcode, itemBarcode1, Env_ZG_Location);
                this.DoRes1(u, type, newPath, "new", true, true, xml);


                //==读者操作他人的记录===
                this.displayLine(this.getLarge("第2组测试：读者操作他人的" + type + "记录。"));
                this.DoRes1(u, type, otherXmlPath, "get", false, true, "");

                xml = this.GetArrivedOrAmerceXml(type, Env_ZG_LibraryCode, reader2, itemBarcode2 + "-1", Env_ZG_Location);
                this.DoResMultiple(u, type, otherXmlPath, "change,delete", false, true, xml);

                this.DoRes1(u, type, newPath, "new", false, true, xml);

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份操作" + type + "-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // 总馆读者身份操作预约到书 或 违约金
        public void CheckArrivedAmerceForFgReader(string type)
        {
            /*
            准备环境：
            管理员为分馆A创建两个读者，两个册，
            管理员为分馆B创建一读者一册

            分馆A读者，应可以获取/创建/修改/删除 自己的预约到书记录。
            不能获取/创建/修改/删除 本馆他人的预约到书记录
            不能获取/创建/修改/删除 他馆他人的预约到书记录
            */

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;

                // 目前此函数仅支持 预约到书 和 违约金
                if (type != C_Type_arrived && type != C_Type_amerce)
                    throw new Exception("CheckArrivedAmerce()不支持的类型" + type);


                #region 环境准备

                // 用管理员帐号创建一个读者，有managecomment权限，以下此读者帐号操作
                UserInfo u = this.NewReaderUser(Env_A_ReaderDbName, Env_A_PatronType,
                    this.SetFullRights(type), "",
                    out string reader1,
                   out string tempPath);

                // 用supervisor创建第2个读者
                this.CreateReaderBySuperviosr(Env_A_ReaderDbName, Env_A_PatronType, "", "", out string reader2);

                // 用管理员身份创建两个册记录
                string aLocation = Env_A_LibraryCode + "/" + Env_A_Location;
                this.CreateItemBySupervisor(aLocation, Env_A_BookType, out string itemBarcode1);
                this.CreateItemBySupervisor(aLocation, Env_A_BookType, out string itemBarcode2);

                // 用管理员为分馆B创建一个读者，一册
                this.CreateReaderBySuperviosr(Env_B_ReaderDbName, Env_B_PatronType, "", "", out string readerB);
                string bLocation = Env_B_LibraryCode + "/" + Env_B_Location;
                this.CreateItemBySupervisor(bLocation, Env_B_BookType, out string itemBarcodeB);

                // 用管理员身份创建两条预约到书记录
                //this.displayLine(this.getLarge("用管理员身份创建2条预约到书记录。"));
                // 预约者是自己的记录

                // 预约者是他人的记录
                //string otherXmlPath = this.CreateArrivedAmerceBySupervisor(type, reader2, itemBarcode2, Env_ZG_Location);


                #endregion

                //==读者操作自己的记录===

                this.displayLine(this.getLarge("第1组测试：读者操作自己的" + type + "记录，应成功"));


                string newPath = this.GetAppendPath(type);
                string xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, reader1, itemBarcode1, aLocation);
                string ownerXmlPath = this.DoRes1(u, type, newPath, "new", true, true, xml);
                if (string.IsNullOrEmpty(ownerXmlPath) == true)//不成功的时候，管理员创建一下
                {
                    ownerXmlPath = this.CreateArrivedAmerceBySupervisor(type, Env_A_LibraryCode, reader1, itemBarcode1, aLocation);
                }
                this.DoRes1(u, type, ownerXmlPath, "get", true, true, "");

                xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, reader1, itemBarcode1 + "-1", aLocation);
                this.DoResMultiple(u, type, ownerXmlPath, "change,delete", true, true, xml);


                //==读者操作本馆他人的记录===
                this.displayLine(this.getLarge("第2组测试：读者操作本馆他人的" + type + "记录。"));

                string otherXml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, reader2, itemBarcode2, aLocation);
                string otherXmlPath = this.DoRes1(u, type, newPath, "new", false, true, otherXml);
                if (string.IsNullOrEmpty(otherXmlPath) == true)  //不成功的时候，管理员创建一下
                {
                    otherXmlPath = this.CreateArrivedAmerceBySupervisor(type, Env_A_LibraryCode, reader2, itemBarcode2, aLocation);
                }
                this.DoRes1(u, type, otherXmlPath, "get", false, true, "");

                xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, reader2, itemBarcode2 + "-1", aLocation);
                this.DoResMultiple(u, type, otherXmlPath, "change,delete", false, true, xml);



                //==读者操作他馆他人的记录===
                this.displayLine(this.getLarge("第3组测试：读者操作他馆他人的" + type + "记录。"));

                xml = this.GetArrivedOrAmerceXml(type, Env_B_LibraryCode, readerB, itemBarcodeB, bLocation);
                string bXmlPath = this.DoRes1(u, type, newPath, "new", false, true, xml);
                if (string.IsNullOrEmpty(bXmlPath) == true)  //不成功的时候，管理员创建一下
                {
                    bXmlPath = this.CreateArrivedAmerceBySupervisor(type, Env_B_LibraryCode, readerB, itemBarcodeB, bLocation);
                }
                this.DoRes1(u, type, bXmlPath, "get", false, true, "");

                xml = this.GetArrivedOrAmerceXml(type, Env_B_LibraryCode, readerB, itemBarcodeB + "-1", bLocation);
                this.DoResMultiple(u, type, bXmlPath, "change,delete", false, true, xml);

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份操作" + type + "-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }


        // 总馆工作人员身份操作 预约到书 或 违约金
        public void CheckArrivedAmerceForZgWorker(string type)
        {
            /*
            准备环境：
            管理员创建一个总馆读者，一个总馆册。
            管理员创建一个分馆读者，一个分馆册



            创建一个总馆工作人员，拥有预约到书完整权限，登录
            为总馆读者预约到书，new/get/change/delete
            为分馆读者预约到书，new/get/change/delete

            ？？？读者是否跨馆预约，例另一分馆读者，预约另一分馆的册。
            目前是可以的，实际不会直接写预约到书记录，只是一个特殊管理接口。所以不判断是否跨馆，是可以的，暂不做多的业务判断。


 */


            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();


                // 目前此函数仅支持 预约到书 和 违约金
                if (type != C_Type_arrived && type != C_Type_amerce)
                    throw new Exception("CheckArrivedAmerce()不支持的类型" + type);


                #region 环境准备

                // 用管理员身份为总馆创建一个读者，一条册
                this.CreateReaderBySuperviosr(Env_ZG_ReaderDbName, Env_ZG_PatronType, "", "", out string zgReader);
                this.CreateItemBySupervisor(Env_ZG_Location, Env_ZG_BookType, out string zgItemBarcode);


                // 用管理员身份为分馆创建一个读者，一条册
                this.CreateReaderBySuperviosr(Env_A_ReaderDbName, Env_A_PatronType, "", "", out string fgReader);
                this.CreateItemBySupervisor(Env_A_LibraryCode + "/" + Env_A_Location, Env_A_BookType, out string fgItemBarcode);



                #endregion

                // 创建一个总工作人员帐户
                UserInfo u = this.NewUser(this.SetFullRights(type), Env_ZG_LibraryCode, "");

                //==操作总馆的预约到书/违约金===
                this.displayLine(this.getLarge("第1组测试：操作总馆的" + type + "，应成功"));
                string newPath = this.GetAppendPath(type);
                string xml = this.GetArrivedOrAmerceXml(type, Env_ZG_LibraryCode, zgReader, zgItemBarcode, Env_ZG_Location);
                string zgXmlPath = this.DoRes1(u, type, newPath, "new", true, false, xml);


                xml = this.GetArrivedOrAmerceXml(type, Env_ZG_LibraryCode, zgReader, zgItemBarcode + "-1", Env_ZG_Location);
                this.DoResMultiple(u, type, zgXmlPath, "get,change,delete", true, false, xml);




                //==操作分馆的预约到书/违约金===
                this.displayLine(this.getLarge("第2组测试：操作分馆的" + type + "，应成功"));
                xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, fgReader, fgItemBarcode, Env_A_LibraryCode + "/" + Env_A_Location);
                string fgXmlPath = this.DoRes1(u, type, newPath, "new", true, false, xml);

                xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, fgReader, fgItemBarcode + "-1", Env_A_LibraryCode + "/" + Env_A_Location);
                this.DoResMultiple(u, type, fgXmlPath, "get,change,delete", true, false, xml);

                //== 测试读者跨馆预约，实现不会走这个接口
                this.displayLine(this.getLarge("第3组测试：测试跨馆的" + type + "。"));
                xml = this.GetArrivedOrAmerceXml(type, Env_ZG_LibraryCode, zgReader, fgItemBarcode, Env_ZG_Location);
                string kgXmlPath = this.DoRes1(u, type, newPath, "new", true, false, xml);

                xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, fgReader, zgItemBarcode, Env_A_LibraryCode + "/" + Env_A_Location);
                kgXmlPath = this.DoRes1(u, type, newPath, "new", true, false, xml);

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份操作" + type + "-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }


        public void CheckArrivedAmerceForFgWorker(string type)
        {
            /*
            准备环境：
            管理员为分馆A创建好读者，册记录
            管理员为分馆B创建好读者，册记录


            给分馆A创建一工作人员，拥有预约到书完整权限，登录
            针对分馆A的预约到书，new/get/change/delete，应成功
            针对分馆B的预约到书，new/get/change/delete，应失败

 */


            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();


                // 目前此函数仅支持 预约到书 和 违约金
                if (type != C_Type_arrived && type != C_Type_amerce)
                    throw new Exception("CheckArrivedAmerce()不支持的类型" + type);


                #region 环境准备

                // 用管理员身份为总馆创建一个读者，一条册
                this.CreateReaderBySuperviosr(Env_A_ReaderDbName, Env_A_PatronType, "", "", out string aReader);
                this.CreateItemBySupervisor(Env_A_LibraryCode + "/" + Env_A_Location, Env_A_BookType, out string aItemBarcode);


                // 用管理员身份为分馆创建一个读者，一条册
                this.CreateReaderBySuperviosr(Env_B_ReaderDbName, Env_B_PatronType, "", "", out string bReader);
                this.CreateItemBySupervisor(Env_B_LibraryCode + "/" + Env_B_Location, Env_B_BookType, out string bItemBarcode);



                #endregion

                // 给分馆A创建一工作帐户
                UserInfo u = this.NewUser(this.SetFullRights(type), Env_A_LibraryCode, "");

                //==操作总馆的预约到书/违约金===
                this.displayLine(this.getLarge("第1组测试：操作本馆的" + type + "，应成功"));
                string newPath = this.GetAppendPath(type);
                string loc = Env_A_LibraryCode + "/" + Env_A_Location;
                string xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, aReader, aItemBarcode, loc);
                string xmlPath = this.DoRes1(u, type, newPath, "new", true, false, xml);
                if (string.IsNullOrEmpty(xmlPath) == true)
                {
                    xmlPath = this.CreateArrivedAmerceBySupervisor(type, Env_A_LibraryCode, bReader, bItemBarcode, loc);
                }

                xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, aReader, aItemBarcode + "-1", loc);
                this.DoResMultiple(u, type, xmlPath, "get,change,delete", true, false, xml);




                //==操作分馆的预约到书/违约金===
                this.displayLine(this.getLarge("第2组测试：操作他馆的" + type + "，应失败"));
                loc = Env_B_LibraryCode + "/" + Env_B_Location;
                xml = this.GetArrivedOrAmerceXml(type, Env_B_LibraryCode, bReader, bItemBarcode, loc);
                xmlPath = this.DoRes1(u, type, newPath, "new", false, false, xml);

                // 如果当前帐户不能创建，则由管理员创建一条
                if (string.IsNullOrEmpty(xmlPath) == true)
                {
                    xmlPath = this.CreateArrivedAmerceBySupervisor(type, Env_B_LibraryCode, bReader, bItemBarcode, loc);
                }
                xml = this.GetArrivedOrAmerceXml(type, Env_B_LibraryCode, bReader, bItemBarcode + "-1", loc);
                this.DoResMultiple(u, type, xmlPath, "get,change,delete", false, false, xml); ;

                //== 测试读者跨馆预约，实现不会走这个接口
                this.displayLine(this.getLarge("第3组测试：测试跨馆的" + type + "。"));

                // 只要librarycode是帐户的馆代码，不论图书实际是哪个馆的，都能写成功？？？
                xml = this.GetArrivedOrAmerceXml(type, Env_A_LibraryCode, aReader, bItemBarcode, Env_A_LibraryCode + "/" + Env_A_Location);
                string kgXmlPath = this.DoRes1(u, type, newPath, "new", true, false, xml);

                xml = this.GetArrivedOrAmerceXml(type, Env_B_LibraryCode, bReader, aItemBarcode, Env_B_LibraryCode + "/" + Env_B_Location);
                kgXmlPath = this.DoRes1(u, type, newPath, "new", false, false, xml);

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份操作" + type + "-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // 测试GetItemInfo接口获取册信息，以及测试有无书目权限时，返回书目是否正常。
        public void CheckGetItemInfo(string type)
        {
            /*
做成一个通用函数，每种资源类型调一次此函数。册/评注/订购/期

用管理员身份创建记录xml
用管理员创建一个用户，有 册/评注/订购/期 权限，但没有书权限。
1）验证获取册，仅xml
2）验证获取多种内容 xml,html,uii。_format要对应，没有_errorCode属性。
3）验证多种格式中，请求参数包括异常格式的情况。异常的格式应有_errorCode。
<results><result _format="test" _errorCode="SystemError" _errorInfo="未知的书目格式 'test'"></result>...
4）通证多种格式中，请求参数包括重复的格式，应按顺序正常返回。_format要对应，数量要对应
5）验证获取书目，应不成功

用管理员创建一个用户，有 册/评注/订购/期 权限，也有书权限，重要观察书目信息。
6）验证获取书目，仅xml。
7）获取书目，多种格式，xml,html,table。
8）获取书目，请求中有异常格式，异常的格式应有_errorCode。
9）获取书目，有重复格式，应依次返回。
             */

            this.displayLine(this.getLarge("针对'" + type + "'测试GetItemInfo()"));

            string strItemDbType = "";
            string rights = "";
            string fullrights = "";
            if (type == C_Type_item)
            {
                strItemDbType = "item";
                rights = "getiteminfo,getitemobject";
                fullrights = "getiteminfo,getitemobject,getbiblioinfo,getbiblioobject";
            }
            else if (type == C_Type_issue)
            {
                strItemDbType = "issue";
                rights = "getissueinfo,getissueobject";
                fullrights = "getissueinfo,getissueobject,getbiblioinfo,getbiblioobject";
            }
            else if (type == C_Type_comment)
            {
                strItemDbType = "comment";
                rights = "getcommentinfo,getcommentobject";
                fullrights = "getcommentinfo,getcommentobject,getbiblioinfo,getbiblioobject";
            }
            else if (type == C_Type_order)
            {
                strItemDbType = "order";
                rights = "getorderinfo,getorderobject";
                fullrights = "getorderinfo,getorderobject,getbiblioinfo,getbiblioobject";
            }
            else
                throw new Exception("不支持的type[" + type + "]");

            WriteResResponse writeRes = null;
            GetItemInfoResponse getItemResponse = null;
            string strError = "";
            bool bRet = false;

            // 用supervisor创建记录xml
            string newPath = this.GetAppendPath(type);
            string resPath = "";
            writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                 newPath,
                 GetXml(type, true));
            if (writeRes.WriteResResult.Value == -1)
                throw new Exception("管理员创建记录xml异常：" + writeRes.WriteResResult.ErrorInfo);
            resPath = writeRes.strOutputResPath;

            // 创建用户，有下级资源权限，但没有书目权限
            this.displayLine(getLarge("创建用户，有下级资源权限，但没有书目权限"));
            UserInfo u = this.NewUser(rights, "", "");
            //UserInfo u = new UserInfo
            //{
            //    UserName = "u1",
            //    Password = "1",
            //    SetPassword = true,
            //    Rights =rights,
            //    Access = ""
            //};
            ////创建帐号
            //this.NewUser(u);


            string barcode = "@path:" + resPath;

            //1）验证获取册，仅xml
            this.displayLine(getLarge("1）验证获取册，仅xml"));
            string resultType = "xml";
            this.displayLine("strResultType=" + resultType);
            List<bool> shoudSuccList = new List<bool>() { true };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, resultType, "", false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {

                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strResult, resultType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));


            //2）验证获取多种内容 xml,html,uii。_format要对应，没有_errorCode属性。
            this.displayLine(getLarge("2）验证获取多种内容 xml,html,uii。_format要对应，没有_errorCode属性。"));
            resultType = "xml,html,uii";
            this.displayLine("strResultType=" + resultType);
            shoudSuccList = new List<bool>() { true, true, true };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, resultType, "", false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strResult, resultType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));


            //3）验证多种格式中，请求参数包括异常格式的情况。异常的格式应有_errorCode。
            //<results><result _format="test" _errorCode="SystemError" _errorInfo="未知的书目格式 'test'"></result>...
            this.displayLine(getLarge("3）验证多种格式中，请求参数包括异常格式的情况。异常的格式应有_errorCode。"));
            resultType = "a,xml,b,uii,test";
            this.displayLine("strResultType=" + resultType);
            shoudSuccList = new List<bool>() { false, true, false, true, false };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, resultType, "", false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strResult, resultType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));

            //4）验证多种格式中，请求参数包括重复的格式，应按顺序正常返回。_format要对应，数量要对应
            this.displayLine(getLarge("4）验证多种格式中，请求参数包括重复的格式，应按顺序正常返回。_format要对应，数量要对应。"));
            resultType = "xml,uii,uii,xml";
            this.displayLine("strResultType=" + resultType);
            shoudSuccList = new List<bool>() { true, true, true, true };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, resultType, "", false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strResult, resultType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));


            //5）验证获取书目，无书目权限，应不成功。
            this.displayLine(getLarge("5）验证获取书目，无书目权限，应不成功。"));
            string biblioType = "xml";
            this.displayLine("strBiblioType=" + biblioType);
            shoudSuccList = new List<bool>() { false };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, "", biblioType, false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strBiblio, biblioType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));

            // 删除用户
            this.DelUser(u);

            //===

            // 创建用户，有下级资源权限，也有书目权限，主要测试GetItemInfo获取书目。
            this.displayLine(getLarge("创建用户，有下级资源权限，也有书目权限，主要测试GetItemInfo获取书目。"));
            u = NewUser(fullrights, "", "");

            //u = new UserInfo
            //{
            //    UserName = "u1",
            //    Password = "1",
            //    SetPassword = true,
            //    Rights = fullrights,
            //    Access = ""
            //};
            ////创建帐号
            //this.NewUser(u);

            /*
用管理员创建一个用户，有 册/评注/订购/期 权限，也有书权限，重要观点书目信息。
6）验证获取书目，仅xml。
7）获取书目，多种格式，xml,html,table。
8）获取书目，请求中有异常格式，异常的格式应有_errorCode。
9）获取书目，有重复格式，应依次返回。
             */


            //6）验证获取书目，仅xml。
            this.displayLine(getLarge("6）验证获取书目，仅xml。"));
            biblioType = "xml";
            this.displayLine("strBiblioType=" + biblioType);
            shoudSuccList = new List<bool>() { true };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, "", biblioType, false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strBiblio, biblioType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));


            // 7）获取书目，多种格式：xml,html,table
            this.displayLine(getLarge("7）获取书目，多种格式：xml,html,table"));
            biblioType = "xml,html,table";
            this.displayLine("strBiblioType=" + biblioType);
            shoudSuccList = new List<bool>() { true, true, true };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, "", biblioType, false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strBiblio, biblioType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));

            // 8）获取书目，请求中有异常格式，异常的格式应有_errorCode。
            this.displayLine(getLarge("8）获取书目，请求中有异常格式，异常的格式应有_errorCode。"));
            biblioType = "a,xml,table,test";
            this.displayLine("strBiblioType=" + biblioType);
            shoudSuccList = new List<bool>() { false, true, true, false };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, "", biblioType, false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strBiblio, biblioType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));

            // 9）获取书目，有重复格式，应依次返回。
            this.displayLine(getLarge("9）获取书目，有重复格式，应依次返回。"));
            biblioType = "xml,xml,table,html,table";
            this.displayLine("strBiblioType=" + biblioType);
            shoudSuccList = new List<bool>() { true, true, true, true, true };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, "", biblioType, false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strBiblio, biblioType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。" + strError));
            }
            else
                this.displayLine(getRed("不符合预期"));


            // 删除用户
            this.DelUser(u);
        }

        private bool CheckFormat(string xml, string formats, List<bool> shoudSuccList, out string strError)
        {
            strError = "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);


            XmlNodeList list = null;
            string[] formatList = formats.Split(new char[] { ',' });
            if (formatList.Length != shoudSuccList.Count)
            {
                strError = "参数错误：formats数量与shoudSuccList数量不对应。";
                return false;
            }


            if (formatList.Length == 1)
                list = dom.ChildNodes;
            else
                list = dom.DocumentElement.ChildNodes;

            if (list.Count != formatList.Length)
            {
                strError = "返回的数据的数量 与 请求格式数量不匹配。";
                return false;
            }



            for (int i = 0; i < list.Count; i++)
            {
                string requestF = formatList[i];
                XmlNode node = list[i];
                bool shoudSucc = shoudSuccList[i];

                string f = DomUtil.GetAttr(node, "_format");
                if (f != requestF)
                {
                    strError = "第" + i + "个formate不对应";
                    return false;
                }

                string errorCode = DomUtil.GetAttr(node, "_errorCode");
                if (shoudSucc == true)
                {
                    if (string.IsNullOrEmpty(errorCode) == false)
                    {
                        strError = "预期应成功返回" + f + ",但实际出错，_errorCode=" + errorCode;
                        return false;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(errorCode) == true)
                    {
                        strError = "预期应失败，实际出错，_errorCode为空";
                        return false;
                    }
                }
            }


            return true;
        }

        private void button_GetItemInfo_Click(object sender, EventArgs e)
        {
            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                //每种资源类型调一次。

                // 册
                CheckGetItemInfo(C_Type_item);

                // 订购
                CheckGetItemInfo(C_Type_order);

                // 评注
                CheckGetItemInfo(C_Type_comment);

                // 期
                CheckGetItemInfo(C_Type_issue);
            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "测试GetItemInfo-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // 订购
        private void button_readerLogin_order_Click(object sender, EventArgs e)
        {
            this.CheckOrderIssueForReader(C_Type_order);
        }

        // 期
        private void button_readerLogin_issue_Click(object sender, EventArgs e)
        {
            this.CheckOrderIssueForReader(C_Type_issue);
        }



        public string type2dbtype(string type)
        {
            if (type == C_Type_order)
                return "order";
            else if (type == C_Type_issue)
                return "issue";
            else if (type == C_Type_comment)
                return "comment";
            else if (type == C_Type_item)
                return "item";
            else
                throw new Exception("type2dbtype()不支持类型=" + type);


        }

        public void CheckOrderIssueForReader(string type)
        {
            /*
    环境准备：       
    用管理员身份创建一个读者，拥有完整的订购权限。
    用管理员身份创建1条订购记录。

    ==
    读者应不能获取订购记录：
    1）读者不能获取订购xml,用getres,getorders,getiteminfo三种接口测试
    2) 读者不能获取订购object,用getres接口

    ==
    读者应不能新建/修改/删除 订购xml 和 对象。
    1）新建，writeres,setiteminfo
    2）修改，writeres,setiteminfo
    3）删除，writeres,setiteminfo
*/

            string rights = this.SetFullRights(type);

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeResponse = null;
                GetResResponse getResponse = null;
                LibraryServerResult result = null;

                // 用supervisor帐户创建一个读者，有操作xml与对象的完整，后面用此读者帐户操作
                this.displayLine(this.getLarge("用管理员身份创建一个读者，有完整的" + type + "权限，后面用此读者帐户操作"));

                // 用管理员帐号创建一个读者，有managecomment权限，以下此读者帐号操作
                UserInfo u = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType, rights, "",
                    out string readerBarcode,
                   out string tempPath);

                //string readerBarcode = "";
                //writeResponse = this.CreateReaderBySuperviosr("读者",rights,
                //    "",  //个人书斋
                //    out readerBarcode);
                //if (writeResponse.WriteResResult.Value == -1)
                //    throw new Exception("管理员创建读者异常：" + writeResponse.WriteResResult.ErrorInfo);
                //string readerOwnerPath = writeResponse.strOutputResPath;

                //// 修改读者的密码
                //this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                //this.ChangeReaderPasswordBySupervisor(readerBarcode);
                //UserInfo u = new UserInfo
                //{
                //    UserName = readerBarcode,
                //    Password = "1",
                //};


                // 用管理员身份创建一条记录xml
                string newPath = this.GetAppendPath(type);
                string xmlPath = "";

                string xml = this.GetXml(type, true);


                writeResponse = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     GetXml(type, true));
                if (writeResponse.WriteResResult.Value == -1)
                    throw new Exception("管理员创建" + type + "记录异常：" + writeResponse.WriteResResult.ErrorInfo);
                xmlPath = writeResponse.strOutputResPath;
                string objectPath = xmlPath + "/object/0";

                // 书目记录
                string parentPath = "";

                if (type == C_Type_issue)
                    parentPath = this.GetIssueBiblioPath();
                else
                    parentPath = GetBiblioPath();




                #region 第1组测试：读者获取记录

                this.displayLine(this.getLarge("第1组测试：读者获取" + type + "记录"));

                //用 GetRes 获取
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetRes 获取" + type + "xml，应成功。"));
                getResponse = this.GetRes(u, xmlPath, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));



                // 用 GetItemInfo 获取
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetItemInfo 获取" + type + "xml，应成功。"));
                GetItemInfoResponse getItemResponse = this.GetItemInfo(u, type, xmlPath, true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用 GetEntities等 获取
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetOrders 获取" + type + "xml，应成功。"));
                result = this.GetFirstEntity(u, type, parentPath, out EntityInfo entity, true);
                if (result.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));



                //用 GetRes 获取 订购下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetRes 获取" + type + "的对象，应成功。"));
                getResponse = this.GetRes(u, objectPath, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第2组测试：读者创建订购记录

                string ownerXmlPath = "";

                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 创建" + type + "xml，应失败。"));
                writeResponse = this.WriteXml(u,
                     newPath,
                     GetXml(type, true), true);
                if (writeResponse.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能创建" + type + "记录。"));

                ownerXmlPath = writeResponse.strOutputResPath;
                string ownerObjectPath = ownerXmlPath + "/object/0";


                if (type == C_Type_order || type == C_Type_issue)
                {
                    // 用 SetItemInfo 创建
                    this.displayLine(GetBR() + getBold(u.UserName + "用 SetItemInfo 创建" + type + "xml，应失败。"));
                    result = this.SetItemInfo1(u,
                       type,
                        "new",
                        newPath,
                       GetXml(type, true),
                       true,
                       out ownerObjectPath);
                    if (result.Value == -1)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期，读者应不能创建" + type + "记录。"));
                }

                #endregion


                #region 第2组测试：读者修改记录

                // 修改订购xml
                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 修改" + type + "xml，应失败。"));
                writeResponse = this.WriteXml(u,
                     xmlPath,
                     GetXml(type, true), true);
                if (writeResponse.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能修改" + type + "xml。"));


                if (type == C_Type_order || type == C_Type_issue)
                {
                    // 用 SetItemInfo 修改
                    this.displayLine(GetBR() + getBold(u.UserName + "用 SetItemInfo 修改" + type + "xml，应失败。"));
                    result = this.SetItemInfo1(u,
                        type, //type2dbtype(type),
                        "change",
                        xmlPath,
                       GetXml(type, true),
                       true,
                       out string tempPath1);
                    if (result.Value == -1)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期，读者应不能修改" + type + "记录xml。"));
                }


                // 修改订购object
                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 修改" + type + "的对象，应失败。"));
                writeResponse = this.WriteObject(u,
                     objectPath, true);
                if (writeResponse.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能修改" + type + "的对象。"));


                #endregion


                #region 第3组测试：读者删除记录

                // 删除订购xml
                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 删除" + type + "xml，应失败。"));
                writeResponse = this.WriteResForDel(u,
                     xmlPath,
                      true);
                if (writeResponse.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能删除" + type + "xml。"));


                if (type == C_Type_order || type == C_Type_issue)
                {
                    // 用 SetItemInfo 删除
                    this.displayLine(GetBR() + getBold(u.UserName + "用 SetItemInfo 删除" + type + "xml，应失败。"));
                    result = this.SetItemInfo1(u,
                        type,//type2dbtype(type),
                        "delete",
                        xmlPath,
                       "",
                       true,
                       out string temp2);
                    if (result.Value == -1)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期，读者应不能删除" + type + "记录。"));
                }

                #endregion

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "用读者身份操作" + type + "-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // 用读者身份测试违约金
        private void button_readerLogin_amerce_Click(object sender, EventArgs e)
        {
            //确保准备好总分馆环境
            EnsureZfgEnv();

            string accountType = this.comboBox_accountType.Text.Trim();

            if (accountType == C_accountType_zgworker)
            {
                this.CheckArrivedAmerceForZgWorker(C_Type_amerce);
            }
            else if (accountType == C_accountType_zgreader)
            {
                this.CheckArrivedAmerceForZgReader(C_Type_amerce);
            }
            else if (accountType == C_accountType_fgworker)
            {
                CheckArrivedAmerceForFgWorker(C_Type_amerce);
            }
            else if (accountType == C_accountType_fgreader)
            {
                this.CheckArrivedAmerceForFgReader(C_Type_amerce);
            }
            else
                throw new Exception("不支持的身份类型" + accountType);

            //this.CheckArrivedAmerce(C_Type_Amerce);
        }


        #region 总分馆

        #region 常量

        public string Env_biblioDbName = "测试中文图书";
        public string Env_biblioDbName_forCopy = "测试目标库";
        public string Env_arrivedDbName = "预约到书";  //测试
        public string Env_amerceDbName = "违约金";//测试

        public string Env_ZG_LibraryCode = "";
        public string Env_A_LibraryCode = "A馆";
        public string Env_B_LibraryCode = "B馆";
        //public string Env_C_LibraryCode = "C馆";

        public string Env_ZG_ReaderDbName = "总馆读者";
        public string Env_A_ReaderDbName = "A馆读者";
        public string Env_B_ReaderDbName = "B馆读者";
        //public string Env_C_ReaderDbName = "C馆读者";

        public string Env_ZG_Location = "总馆图书馆";
        public string Env_ZG_Location_阅览室 = "总馆阅览室";
        public string Env_A_Location = "A馆图书馆";
        public string Env_A_Location_阅览室 = "A馆阅览室";
        public string Env_B_Location = "B馆图书馆";
        //public string Env_C_Location = "C馆图书馆";

        public const string Env_ZG_CalenderName = "总馆日历";
        public const string Env_A_CalenderName = "A馆日历";
        public const string Env_B_CalenderName = "B馆日历";
        //public const string Env_C_CalenderName = "C馆日历";

        public string Env_ZG_PatronType = "总馆学生";
        //public string Env_ZG_PatronType_teacher = "总馆教师";
        public string Env_A_PatronType = "A馆学生";
        //public string Env_A_PatronType_teacher = "A馆教师";
        public string Env_B_PatronType = "B馆学生";
        //public string Env_B_PatronType_teacher = "B馆教师";
        //public string Env_C_PatronType = "C馆学生";
        //public string Env_C_PatronType_teacher = "C馆教师";

        public string Env_ZG_BookType = "总馆普通图书";
        public string Env_A_BookType = "A馆普通图书";
        public string Env_B_BookType = "B馆普通图书";
        //public string Env_C_BookType = "C馆普通图书";

        #endregion

        // 确保初始化环境
        bool _existZfgEnv = false;
        public void EnsureZfgEnv()
        {
            // 如果已经存在分馆环境，则不需要再次创建了。
            if (_existZfgEnv == true)
                return;

            // 先获取一下
            RestChannel channel = null;
            try
            {
                // 用管理员身份登录
                UserInfo u = this.mainForm.GetSupervisorAccount();
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, false);

                int nRet = channel.GetAllDatabase(out string dbXml, out string error);
                if (nRet == -1)
                {
                    MessageBox.Show(this, "GetAllDatabase()出错：" + error);
                    return;
                }


                XmlDocument dom = new XmlDocument();
                dom.LoadXml(dbXml);  //测试中文图书
                XmlNode node = dom.DocumentElement.SelectSingleNode("database[@name='" + this.Env_biblioDbName + "']");
                if (node != null)
                {
                    this._existZfgEnv = true;
                    return;
                }
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }

            CreateZfgEnv();
        }

        private void button_createZfgEnv_Click(object sender, EventArgs e)
        {
            this.CreateZfgEnv();
        }
        // 删除环境
        private void button_delEnv_Click(object sender, EventArgs e)
        {
            int nRet = this.DeleteLibEnv(out string error);
            if (nRet == -1)
            {
                MessageBox.Show(this, "删除测试环境出错：" + error);
                return;
            }
        }



        public List<DigitalPlatform.CirculationClient.ManageHelper.LocationItem> GetLocationList()
        {
            List<DigitalPlatform.CirculationClient.ManageHelper.LocationItem> items = new List<DigitalPlatform.CirculationClient.ManageHelper.LocationItem>();
            items.Add(new DigitalPlatform.CirculationClient.ManageHelper.LocationItem("", Env_ZG_Location, true, true));
            items.Add(new DigitalPlatform.CirculationClient.ManageHelper.LocationItem("", Env_ZG_Location_阅览室, true, true));

            // A馆
            items.Add(new DigitalPlatform.CirculationClient.ManageHelper.LocationItem(Env_A_LibraryCode, Env_A_Location, true, true));
            items.Add(new DigitalPlatform.CirculationClient.ManageHelper.LocationItem(Env_A_LibraryCode, Env_A_Location_阅览室, true, true));

            // B馆
            items.Add(new DigitalPlatform.CirculationClient.ManageHelper.LocationItem(Env_B_LibraryCode, Env_B_Location, true, true));
            // 为了测 分馆读者的个人书斋名称 与 另一个分馆的馆藏地相同的情况。给B馆创建了一个A馆图书馆
            items.Add(new DigitalPlatform.CirculationClient.ManageHelper.LocationItem(Env_B_LibraryCode, Env_A_Location, true, true));


            return items;
        }

        // 创建测试环境
        public void CreateZfgEnv()
        {
            long lRet = 0;
            string error = "";

            // 清空界面
            this.ClearResult();

            //先删除测试环境
            int nRet = this.DeleteLibEnv(out error);
            if (nRet == -1)
            {
                MessageBox.Show(this, "删除测试环境出错：" + error);
                return;
            }

            // 用管理员帐号创建总分馆环境
            UserInfo u = this.mainForm.GetSupervisorAccount();

            displayLine("\r\n开始初始化测试环境 ...");
            RestChannel channel = null;
            try
            {
                // 用管理员身份登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, false);

                // ***创建测试所需的书目库
                displayLine("开始创建书目库：" + this.Env_biblioDbName);
                nRet = ManageHelper.CreateBiblioDatabase(
                    channel,
                    // this.Progress,
                    this.Env_biblioDbName, //Env_BiblioDbName, //C_BiblioDbName,
                    "series",//"book",
                    "unimarc",
                    out error);
                if (nRet == -1)
                    goto ERROR1;

                // 这个库用于测copy函数
                displayLine("开始创建书目库：" + this.Env_biblioDbName_forCopy);
                nRet = ManageHelper.CreateBiblioDatabase(
                    channel,
                    // this.Progress,
                    this.Env_biblioDbName_forCopy,
                    "book",
                    "unimarc",
                    out error);
                if (nRet == -1)
                    goto ERROR1;

                //// 预约到书
                //nRet= ManageHelper.CreateSimpleDatabase(channel,
                //    Env_arrivedDbName, //"预约到书",
                //    "arrived",
                //    out error);
                //if (nRet == -1)
                //    goto ERROR1;

                //// 违约金
                //nRet = ManageHelper.CreateSimpleDatabase(channel,
                //    Env_amerceDbName, //"违约金",
                //    "amerce",
                //    out error);
                //if (nRet == -1)
                //    goto ERROR1;

                // 先要创建读者库
                displayLine("开始创建总馆读者库 ...");
                // 总馆读者库
                ManageDatabaseResponse response = CreateReaderDb(channel, Env_ZG_ReaderDbName, Env_ZG_LibraryCode);
                if (lRet == -1)
                {
                    error = response.ManageDatabaseResult.ErrorInfo;
                    goto ERROR1;
                }

                // 创建A馆读者库
                displayLine("开始创建A馆读者库 ...");
                //ProgressSetMessage(info);
                //LogManager.Logger.Info(info);
                response = CreateReaderDb(channel,
                   Env_A_ReaderDbName,
                   Env_A_LibraryCode);
                if (lRet == -1)
                {
                    error = response.ManageDatabaseResult.ErrorInfo;
                    goto ERROR1;
                }


                // 创建B馆读者库
                displayLine("开始创建B馆读者库 ...");
                //ProgressSetMessage(info);
                //LogManager.Logger.Info(info);
                response = CreateReaderDb(channel,
                    Env_B_ReaderDbName,
                    Env_B_LibraryCode);
                if (lRet == -1)
                {
                    error = response.ManageDatabaseResult.ErrorInfo;
                    goto ERROR1;
                }

                this.displayLine("开始创建馆藏地 ...");
                List<DigitalPlatform.CirculationClient.ManageHelper.LocationItem> items = this.GetLocationList();
                nRet = ManageHelper.AddLocationTypes(
                    channel,
                    // this.Progress,
                    "add",
                    items,
                    out error);
                if (nRet == -1)
                    goto ERROR1;


                this.displayLine("开始创建工作日历 ...");
                // 创建总馆日历
                CalenderInfo cInfo = new CalenderInfo();
                cInfo.Name = Env_ZG_CalenderName;
                cInfo.Range = "20220101-20241231";
                cInfo.Comment = "";
                cInfo.Content = "";
                lRet = channel.SetCalendar(
                   // _stop,
                   "new",
                   cInfo,
                   out error);
                if (lRet == -1)
                    goto ERROR1;

                // A馆
                cInfo = new CalenderInfo();
                cInfo.Name = Env_A_LibraryCode + "/" + Env_A_CalenderName;
                cInfo.Range = "20220101-20241231";
                cInfo.Comment = "";
                cInfo.Content = "";
                lRet = channel.SetCalendar(
                   // _stop,
                   "new",
                   cInfo,
                   out error);
                if (lRet == -1)
                    goto ERROR1;
                // B馆
                cInfo = new CalenderInfo();
                cInfo.Name = Env_B_LibraryCode + "/" + Env_B_CalenderName;
                cInfo.Range = "20220101-20241231";
                cInfo.Comment = "";
                cInfo.Content = "";
                lRet = channel.SetCalendar(
                   // _stop,
                   "new",
                   cInfo,
                   out error);
                if (lRet == -1)
                    goto ERROR1;



                // 创建流通权限
                this.displayLine("开始创建流通权限...");
                List<rightTable> rightList = new List<rightTable>();
                //总馆
                rightTable zg = new rightTable(Env_ZG_LibraryCode,
                    Env_ZG_PatronType, true,
                    Env_ZG_BookType,
                    Env_ZG_CalenderName);
                rightList.Add(zg);

                // A馆
                rightTable a = new rightTable(Env_A_LibraryCode,
                    this.Env_A_PatronType,
                    true,
                    this.Env_A_BookType,
                    Env_A_LibraryCode + "/" + Env_A_CalenderName);
                rightList.Add(a);

                //B馆
                rightTable b = new rightTable(Env_B_LibraryCode,
                    this.Env_B_PatronType,
                    true,
                   this.Env_B_BookType,
                    Env_B_LibraryCode + "/" + Env_B_CalenderName);
                rightList.Add(b);

                nRet = this.AddTestRightsTable(channel, null, rightList,
    out error);
                if (nRet == -1)
                    goto ERROR1;


                // 初始过一次之后，不再初始化
                this._existZfgEnv = true;


                this.displayLine("创建测试环境完成。");

                return;

            ERROR1:
                throw new Exception(error);
            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "创建测试环境异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }

        }

        // 删除测试环境
        public int DeleteLibEnv(out string error)
        {
            error = "";
            int nRet = 0;
            long lRet = 0;

            // 清空界面
            this.ClearResult();

            // 用管理员帐号创建总分馆环境
            UserInfo u = this.mainForm.GetSupervisorAccount();

            //===
            RestChannel channel = null;
            this.displayLine("\r\n开始删除测试环境 ...");
            EnableCtrls(false);
            try
            {
                // 用管理员身份登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, false);

                // 删除书目库
                displayLine("开始删除测试书目库 ...");
                ManageDatabaseResponse r = channel.ManageDatabase(
                    "delete",
                    Env_biblioDbName + "," + Env_biblioDbName_forCopy,  // 2023/4/4 增加了一个copy的目标库
                    "",
                    "");
                if (r.ManageDatabaseResult.Value == -1
                    && r.ManageDatabaseResult.ErrorCode != ErrorCode.NotFound)
                {
                    goto ERROR1;
                }

                //// 2023/4/4 增加 删除测试用的预约到书 和 违约金库
                //displayLine("开始删除测试 预约到书 和 违约金 库 ...");
                // r = channel.ManageDatabase(
                //    "delete",
                //    Env_arrivedDbName + "," + Env_amerceDbName,  
                //    "",
                //    "");
                //if (r.ManageDatabaseResult.Value == -1
                //    && r.ManageDatabaseResult.ErrorCode != ErrorCode.NotFound)
                //{
                //    goto ERROR1;
                //}

                // 删除读者库
                displayLine("开始删除测试用读者库 ...");
                string strDatabaseNames = Env_ZG_ReaderDbName
                        + "," + Env_A_ReaderDbName
                        + "," + Env_B_ReaderDbName;
                r = channel.ManageDatabase(
                  "delete",
                  strDatabaseNames,
                  "",
                  "");
                if (r.ManageDatabaseResult.Value == -1 && r.ManageDatabaseResult.ErrorCode != ErrorCode.NotFound)
                {
                    goto ERROR1;
                }



                // *** 删除馆藏地配置
                displayLine("开始删除馆藏地 ...");
                //ProgressSetMessage(info);
                //LogManager.Logger.Info(info);
                List<DigitalPlatform.CirculationClient.ManageHelper.LocationItem> items = this.GetLocationList();
                nRet = ManageHelper.AddLocationTypes(
                    channel,
                    // this.Progress,
                    "remove",
                    items,
                    out error);
                if (nRet == -1)
                    goto ERROR1;

                //***删除工作日历
                CalenderInfo cInfo = null;
                displayLine("开始删除工作日历 ...");
                //ProgressSetMessage(info);
                //LogManager.Logger.Info(info);
                CalenderInfo[] infos1 = null;
                GetCalendarResponse getCalendarResponse = channel.GetCalendar(
                    // this.Progress,
                    "get",
                    Env_ZG_CalenderName,
                    0,
                    -1);
                if (getCalendarResponse.GetCalendarResult.Value == -1)
                    goto ERROR1;
                if (getCalendarResponse.GetCalendarResult.Value > 0)
                {
                    // 册总馆日历
                    cInfo = new CalenderInfo();
                    cInfo.Name = Env_ZG_CalenderName;
                    cInfo.Range = "20220101-20241231";
                    cInfo.Comment = "";
                    cInfo.Content = "";
                    lRet = channel.SetCalendar(
                       // _stop,
                       "delete",
                       cInfo,
                       out error);
                    if (lRet == -1)
                        goto ERROR1;
                }

                // 删除分馆的开馆日历

                getCalendarResponse = channel.GetCalendar(
                    // this.Progress,
                    "get",
                    Env_A_LibraryCode + "/" + Env_A_CalenderName,
                    0,
                    -1);
                if (getCalendarResponse.GetCalendarResult.Value == -1)
                    goto ERROR1;
                if (getCalendarResponse.GetCalendarResult.Value > 0)
                {
                    // 册A馆日历
                    cInfo = new CalenderInfo();
                    cInfo.Name = Env_A_LibraryCode + "/" + Env_A_CalenderName;
                    cInfo.Range = "20220101-20241231";
                    cInfo.Comment = "";
                    cInfo.Content = "";
                    lRet = channel.SetCalendar(
                       // _stop,
                       "delete",
                       cInfo,
                       out error);
                    if (lRet == -1)
                        goto ERROR1;
                }

                getCalendarResponse = channel.GetCalendar(
                    // this.Progress,
                    "get",
                    Env_B_LibraryCode + "/" + Env_B_CalenderName,
                    0,
                    -1);
                if (getCalendarResponse.GetCalendarResult.Value == -1)
                    goto ERROR1;
                if (getCalendarResponse.GetCalendarResult.Value > 0)
                {
                    // 册B馆日历
                    cInfo = new CalenderInfo();
                    cInfo.Name = Env_B_LibraryCode + "/" + Env_B_CalenderName;
                    cInfo.Range = "20220101-20241231";
                    cInfo.Comment = "";
                    cInfo.Content = "";
                    lRet = channel.SetCalendar(
                       // _stop,
                       "delete",
                       cInfo,
                       out error);
                    if (lRet == -1)
                        goto ERROR1;

                }




                // ***删除权限流通权限
                displayLine("开始删除流通权限 ...");
                //ProgressSetMessage(info);
                //LogManager.Logger.Info(info);

                List<string> zgReaderTypes = new List<string>();
                zgReaderTypes.Add(Env_ZG_PatronType);

                List<string> zgBookTypes = new List<string>();
                zgBookTypes.Add(Env_ZG_BookType);

                List<string> fglist = new List<string>();
                fglist.Add(Env_A_LibraryCode);
                fglist.Add(Env_B_LibraryCode);

                nRet = this.RemoveTestRightsTable(channel, null,
                    zgReaderTypes,
                    zgBookTypes,
                    fglist,
                    out error);
                if (nRet == -1)
                    goto ERROR1;


                // 删除创建的用户
                GetUserResponse result1 = channel.GetUser("",
                    "",
                    0, -1);
                if (result1.contents != null && result1.contents.Length > 0)
                {
                    foreach (UserInfo one in result1.contents)
                    {
                        // _u开头的帐户是自动测试创建的，删除
                        if (one.UserName.Length >= 2 && one.UserName.Substring(0, 2) == "_u")
                        {
                            channel.SetUser("delete", one);
                        }
                    }
                }

                displayLine("删除环境完成 ...");

                // 分馆环境变量
                this._existZfgEnv = false;

                return 0;
            }
            catch (Exception ex)
            {
                error = "Exception: " + ExceptionUtil.GetExceptionText(ex);
                goto ERROR1;
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);

                EnableCtrls(true);
            }


        ERROR1:

            //LogManager.Logger.Error(error);
            return -1;
        }

        void MessageBoxShow(Form form, string strText)
        {
            this.Invoke(new Action(() =>
            {
                MessageBox.Show(form, strText);
            }));
        }


        public class LocItem
        {
            public LocItem(string location, string prefix, string bookType)
            {
                this.Location = location;
                this.Prefix = prefix;
                this.BookType = bookType;
            }
            public string Location { get; set; }
            public string Prefix { get; set; }

            public string BookType { get; set; }
        }




        // 从路径中获取id
        public static string GetRecordID(string strPath)
        {
            int nRet = strPath.LastIndexOf("/");
            if (nRet == -1)
                return "";

            return strPath.Substring(nRet + 1).Trim();
        }




        // 创建读者记录
        int CreateReaderRecord(RestChannel channel,
            string readerDbName,
            string readerType,
            string prefix,
            int count,
            out string strError)
        {
            strError = "";

            // 创建测试用读者记录
            string strTargetRecPath = readerDbName + "/?";
            for (int i = 1; i <= count; i++)
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml("<root />");
                XmlNode root = dom.DocumentElement;

                string barcode = prefix + i.ToString().PadLeft(3, '0');
                DomUtil.SetElementText(root, "barcode", barcode);

                string name = prefix + "测试读者" + i.ToString().PadLeft(3, '0'); ;
                DomUtil.SetElementText(root, "name", name);

                DomUtil.SetElementText(root, "readerType", readerType);


                //string strExistingXml = "";
                //string strSavedXml = "";
                //string strSavedPath = "";
                //byte[] baNewTimestamp = null;
                ErrorCodeValue kernel_errorcode = ErrorCodeValue.NoError;
                //long lRet = channel.SetReaderInfo(
                //   // _stop,
                //   "new",  // this.m_strSetAction,
                //   strTargetRecPath,
                //   dom.OuterXml,
                //   null,
                //   null,

                //   out strExistingXml,
                //   out strSavedXml,
                //   out strSavedPath,
                //   out baNewTimestamp,
                //   out kernel_errorcode,
                //   out strError);
                //if (lRet == -1)
                //{
                //    return -1;
                //}

                SetReaderInfoResponse response = channel.SetReaderInfo("new",
                    strTargetRecPath,
                    dom.OuterXml,
                    null,
                    null);
                if (response.SetReaderInfoResult.Value == -1)
                    return -1;

                string strExistingXml = response.strExistingXml;
                string strSavedXml = response.strSavedXml;
                string strSavedPath = response.strSavedRecPath;
                byte[] baNewTimestamp = response.baNewTimestamp;
            }

            return 0;

        }

        // 创建帐号
        public int SetUser(RestChannel channel,
            string action,
            string libraryCode,
            string userName,
            out string strError)
        {
            strError = "";

            UserInfo user = new UserInfo();
            user.LibraryCode = libraryCode;
            user.UserName = userName;
            user.Password = "1";
            user.SetPassword = true;
            //user.Binding = "ip:[current]";  // 自动绑定当前请求者的 IP
            // default_capo_rights
            //user.Rights = "getsystemparameter,getres,search,getbiblioinfo,setbiblioinfo,getreaderinfo,writeobject,getbibliosummary,listdbfroms,simulatereader,simulateworker"
            //    + ",getiteminfo,getorderinfo,getissueinfo,getcommentinfo"
            //    + ",borrow,return,getmsmqmessage"
            //    + ",bindpatron,searchbiblio,getpatrontempid,resetpasswordreturnmessage,getuser,changereaderpassword,renew,reservation,getcalendar";


            user.Rights = "borrow,return,renew,lost,reservation,changereaderpassword"
                + ",verifyreaderpassword,getbibliosummary,searchcharging"
                + ",searchreader,getreaderinfo,setreaderinfo,changereaderstate"
                + ",listdbfroms,searchbiblio,getbiblioinfo,searchitem,getiteminfo,setiteminfo"
                + ",getoperlog,amerce,amercemodifyprice,amercemodifycomment,amerceundo"
                + ",search,getrecord,getcalendar,newcalendar,changecalendar,getsystemparameter,setsystemparameter"
                + ",urgentrecover,repairborrowinfo,getres,searchissue,getissueinfo,setissueinfo"
                + ",searchorder,getorderinfo,setorderinfo,getcommentinfo,setcommentinfo,searchcomment"
                + ",writeobject,writetemplate,managecache,managecomment,viewreport"
                + ",getuser,newuser,changeuser,deleteuser";

            SetUserResponse response = channel.SetUser(
    action,//"new",
    user);
            if (response.SetUserResult.Value == -1 && action != "delete")
            {
                strError = "创建代理帐户时发生错误: " + response.SetUserResult.ErrorInfo;
                return -1;
            }

            return 0;
        }

        public ManageDatabaseResponse CreateReaderDb(RestChannel channel, string dbName, string libraryCode)
        {
            //string error = "";
            //string strOutputInfo = "";
            XmlDocument database_dom = new XmlDocument();
            database_dom.LoadXml("<root />");
            // 创建总馆读者库
            ManageHelper.CreateReaderDatabaseNode(database_dom,
                dbName,
                libraryCode,
                true);
            ManageDatabaseResponse r = channel.ManageDatabase(
                 // this._stop,
                 "create",
                 "",
                 database_dom.OuterXml,
                 "");
            return r;

        }

        #region 流通权限





        // 删除测试加的流通权限
        public int RemoveTestRightsTable(RestChannel channel,
            XmlDocument dom,
            List<string> zgReaderTypes,
            List<string> zgBookTypes,
            //string zgReaderType,
            //string zgBookType,
            List<string> fgList,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            // 获取流通权限
            if (dom == null)
            {
                string strRightsTableXml = "";

                // 先获取一下原来的权限，避免覆盖
                nRet = channel.GetSystemParameter(
                    // _stop,
                    "circulation",
                    "rightsTable",
                    out strRightsTableXml,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;


                strRightsTableXml = "<rightsTable>" + strRightsTableXml + "</rightsTable>";
                dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strRightsTableXml);
                }
                catch (Exception ex)
                {
                    strError = "strRightsTableXml装入XMLDOM时发生错误：" + ex.Message;
                    goto ERROR1;
                }
            }

            XmlNode root = dom.DocumentElement;

            // 删除总馆
            foreach (string readerType in zgReaderTypes)
            {
                XmlNode node = root.SelectSingleNode("type[@reader='" + readerType + "']");
                if (node != null)
                {
                    root.RemoveChild(node);
                }
                //XmlNodeList list = root.SelectNodes("//type[@book='" + zgBookType + "']");
                //foreach (XmlNode n in list)
                //{
                //    n.ParentNode.RemoveChild(n);
                //}
                node = root.SelectSingleNode("readerTypes/item[text()='" + readerType + "']");
                if (node != null)
                {
                    node.ParentNode.RemoveChild(node);
                }
            }

            foreach (string bookType in zgBookTypes)
            {
                XmlNode node = root.SelectSingleNode("bookTypes/item[text()='" + bookType + "']");
                if (node != null)
                {
                    node.ParentNode.RemoveChild(node);
                }
            }


            // 删除分馆的权限
            if (fgList != null && fgList.Count > 0)
            {
                foreach (string fg in fgList)
                {
                    XmlNodeList nodeList = root.SelectNodes("library[@code='" + fg + "']");
                    foreach (XmlNode n in nodeList)
                    {
                        root.RemoveChild(n);
                    }
                }
            }



            // 保存到系统
            nRet = channel.SetSystemParameter(
                "circulation",
                "rightsTable",
                root.InnerXml,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            return 0;

        ERROR1:
            return -1;
        }

        // 增加测试用的流通权限
        public int AddTestRightsTable(RestChannel channel,
            XmlDocument dom,
            List<rightTable> rightTables,
            out string strError)
        {
            strError = "";
            int nRet = 0;

            // 获取流通权限
            if (dom == null)
            {
                string strRightsTableXml = "";
                // 先获取一下原来的权限，避免覆盖
                nRet = channel.GetSystemParameter(
                    // _stop,
                    "circulation",
                    "rightsTable",
                    out strRightsTableXml,
                    out strError);
                if (nRet == -1)
                    goto ERROR1;

                strRightsTableXml = "<rightsTable>" + strRightsTableXml + "</rightsTable>";
                dom = new XmlDocument();
                try
                {
                    dom.LoadXml(strRightsTableXml);
                }
                catch (Exception ex)
                {
                    strError = "strRightsTableXml装入XMLDOM时发生错误：" + ex.Message;
                    goto ERROR1;
                }
            }
            XmlNode root = dom.DocumentElement;

            foreach (rightTable one in rightTables)
            {



                // 总馆的情况
                XmlNode patronNode = null;

                // 分馆情况
                if (string.IsNullOrEmpty(one.LibraryCode) == false)
                {
                    patronNode = root.SelectSingleNode("library[@code='" + one.LibraryCode + "']");
                    if (patronNode == null)
                    {
                        //    <library code="B馆">
                        patronNode = dom.CreateElement("library");
                        DomUtil.SetAttr(patronNode, "code", one.LibraryCode);
                        root.AppendChild(patronNode);
                    }
                }
                else
                {
                    patronNode = root;
                }


                XmlNode node = patronNode.SelectSingleNode("type[@reader='" + one.PatronType + "']");
                // 增加测试用权限
                if (node == null)
                {
                    node = dom.CreateElement("type");
                    DomUtil.SetAttr(node, "reader", one.PatronType);
                    node.InnerXml = @"<param name='可借总册数' value='10' />
                                                <param name='可预约册数' value='5' />
                                                <param name='以停代金因子' value='' />
                                                <param name='工作日历名' value='" + one.CalenderName + @"' />
                                                <type book='" + one.BookType + @"'>
                                                  <param name='可借册数' value='10' />
                                                  <param name='借期' value='31day,60day' />
                                                  <param name='超期违约金因子' value='' />
                                                  <param name='丢失违约金因子' value='1.5' />
                                                </type>";
                    patronNode.AppendChild(node);

                    // 2022/3/3 来宾馆读者类型不用创建读者类型
                    if (one.IsNewPatronType == true)
                    {
                        /*
    <readerTypes>
        <item>总馆学生</item>
    </readerTypes>
                         */
                        XmlNode readerTypesNode = patronNode.SelectSingleNode("readerTypes");
                        if (readerTypesNode == null)
                        {
                            readerTypesNode = dom.CreateElement("readerTypes");
                            patronNode.AppendChild(readerTypesNode);
                        }
                        node = dom.CreateElement("item");
                        node.InnerText = one.PatronType;
                        readerTypesNode.AppendChild(node);
                    }

                    /*
                    <bookTypes>
                    <item>总馆普通图书</item>
                    </bookTypes>
                     */
                    XmlNode bookTypesNode = patronNode.SelectSingleNode("bookTypes");
                    if (bookTypesNode == null)
                    {
                        bookTypesNode = dom.CreateElement("bookTypes");
                        patronNode.AppendChild(bookTypesNode);
                    }
                    XmlNode temp = bookTypesNode.SelectSingleNode("item[text()='" + one.BookType + "']");
                    if (temp == null)
                    {
                        node = dom.CreateElement("item");
                        node.InnerText = one.BookType;
                        bookTypesNode.AppendChild(node);
                    }
                }


            }


            // 保存到系统
            nRet = channel.SetSystemParameter(
                "circulation",
                "rightsTable",
                 dom.DocumentElement.InnerXml,
                out strError);
            if (nRet == -1)
                goto ERROR1;

            return 0;

        ERROR1:
            return -1;
        }


        public class rightTable
        {
            public rightTable(string libraryCode,
                string patronType,
                bool isNewPatronType,
                string bookType,
                string calenderName)
            {
                this.LibraryCode = libraryCode;
                this.PatronType = patronType;
                this.IsNewPatronType = isNewPatronType;
                this.BookType = bookType;
                this.CalenderName = calenderName;
            }
            public string LibraryCode { get; set; }
            public string PatronType { get; set; }
            public bool IsNewPatronType { get; set; }
            public string BookType { get; set; }

            //CalenderName
            public string CalenderName { get; set; }

        }







        #endregion

        #endregion


        private void button_fg_reader_Click(object sender, EventArgs e)
        {

        }

        private void button_fg_reader_reader_Click(object sender, EventArgs e)
        {
            /*
             * 分馆读者，
             * 不能新建本馆读者，不能删除读者。
             * 仅能 修改自己的两个字段和对象，
             * 不能操作他馆读者。
             */
        }





        #region 操作各类数据



        public string ChangeBarcode(string xml)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);


            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);

            string oldBarcode = DomUtil.GetElementText(dom.DocumentElement, "barcode");
            string oldDisplayName = DomUtil.GetElementText(dom.DocumentElement, "displayName");



            string newBarcode = oldBarcode + "-" + temp.ToString();
            string newDisplayName = oldDisplayName + "-" + temp.ToString();
            DomUtil.SetElementText(dom.DocumentElement, "barcode", newBarcode);
            DomUtil.SetElementText(dom.DocumentElement, "displayName", newDisplayName);

            return dom.OuterXml;
        }

        public void DoResMultiple(UserInfo u, string type, string xmlPath, string actions, bool expectSucc, bool isReader, string xml)
        {
            string[] actionList = actions.Split(new char[] { ',' });
            foreach (string action in actionList)
            {
                this.DoRes1(u, type, xmlPath, action, expectSucc, isReader, xml);
            }
        }

        public string DoRes1(UserInfo u,
            string type,
            string xmlPath,
            string action,
            bool expectSucc, bool isReader, string xml)
        {
            WriteResResponse writeResponse = null;
            GetResResponse getResResponse = null;

            SetBiblioInfoResponse setBiblioResponse = null;
            GetBiblioInfosResponse getBibliosResponse = null;

            SetReaderInfoResponse setReaderResponse = null;
            GetReaderInfoResponse getReaderResponse = null;

            LibraryServerResult result = null;

            string outputPath = "";


            // 根据xml路径，算出对象路径
            string objectPath = xmlPath + "/object/0";

            string expectResult = "应失败";
            if (expectSucc == true)
                expectResult = "应成功";

            // 删除的时候没必须准备xml
            if (action != "delete" && string.IsNullOrEmpty(xml) == true)
                xml = this.GetXml(type, true);

            // 新增读者的情况，用两个接口测试
            if (action == "new")
            {
                // 用WriteRes()新建读者xml
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes()新建" + type + "xml，" + expectResult));
                writeResponse = this.WriteXml(u, xmlPath, xml, isReader);
                this.CheckResult(expectSucc, writeResponse.WriteResResult);

                //返回第一笔path
                outputPath = writeResponse.strOutputResPath;//.strSavedRecPath;

                if (type == C_Type_reader)
                {
                    // 用SetReaderInfo新建读者
                    xml = this.ChangeBarcode(xml);//this.GetXml(type, true);  //注意要重新获取一下xml,要不证号重复。
                    this.displayLine(GetBR() + getBold(u.UserName + "用SetReaderInfo()新建" + type + "xml，" + expectResult));
                    setReaderResponse = this.SetReaderInfo(u, "new", xmlPath, xml, isReader);
                    this.CheckResult(expectSucc, setReaderResponse.SetReaderInfoResult);
                }
                else if (type == C_Type_biblio)
                {
                    // 用SetBiblioInfo新建书目
                    this.displayLine(GetBR() + getBold(u.UserName + "用SetBiblioInfo()新建" + type + "xml，" + expectResult));
                    setBiblioResponse = this.SetBiblioInfo(u, "new", xmlPath, xml, isReader);
                    this.CheckResult(expectSucc, setBiblioResponse.SetBiblioInfoResult);
                }
                else if (type == C_Type_item
                        || type == C_Type_issue
                        || type == C_Type_comment
                        || type == C_Type_order)
                {
                    // SetItemInfo1
                    this.displayLine(GetBR() + getBold(u.UserName + "SetItemInfo()新增" + type + "xml，" + expectResult));
                    result = this.SetItemInfo1(u,
                       type,
                        "new",
                        xmlPath,
                        xml,
                        isReader, out string temp1);
                    this.CheckResult(expectSucc, result);
                }

            }
            else if (action == "change")
            {
                // 用WriteRes修改读者记录xml
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes()修改" + type + "xml，" + expectResult));
                writeResponse = this.WriteXml(u, xmlPath, xml, isReader);
                this.CheckResult(expectSucc, writeResponse.WriteResResult);

                // 用WriteRes修改读者的对象
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes()修改" + type + "的对象，" + expectResult));
                writeResponse = this.WriteObject(u, objectPath, isReader);
                this.CheckResult(expectSucc, writeResponse.WriteResResult);

                if (type == C_Type_reader)
                {
                    // 用SetReaderInfo修改读者xml
                    this.displayLine(GetBR() + getBold(u.UserName + "用SetReaderInfo()修改读者xml，" + expectResult));
                    setReaderResponse = this.SetReaderInfo(u, "change", xmlPath, xml, isReader);
                    this.CheckResult(expectSucc, setReaderResponse.SetReaderInfoResult);
                }
                else if (type == C_Type_biblio)
                {
                    // 用SetBiblioInfo新建书目
                    this.displayLine(GetBR() + getBold(u.UserName + "用SetBiblioInfo()修改" + type + "xml，" + expectResult));
                    setBiblioResponse = this.SetBiblioInfo(u, "change", xmlPath, xml, isReader);
                    this.CheckResult(expectSucc, setBiblioResponse.SetBiblioInfoResult);
                }
                else if (type == C_Type_item
                    || type == C_Type_issue
                    || type == C_Type_comment
                    || type == C_Type_order)
                {
                    // SetItemInfo1
                    this.displayLine(GetBR() + getBold(u.UserName + "SetItemInfo()修改" + type + "xml，" + expectResult));
                    result = this.SetItemInfo1(u,
                       type,
                        "change",
                        xmlPath,
                        xml,
                        isReader, out string temp1);
                    this.CheckResult(expectSucc, result);
                }

            }
            else if (action == "delete")
            {
                // 用 WriteRes()删除读者
                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes()删除" + type + "xml，" + expectResult));
                writeResponse = this.WriteResForDel(u, xmlPath, isReader);
                this.CheckResult(expectSucc, writeResponse.WriteResResult);


                // 如果是预期失败的情况，可以再用对应函数删一下。
                // 如果是预期成功的，则不必要了，因为记录已经被删掉了。
                if (expectSucc == false)
                {
                    if (type == C_Type_reader)
                    {
                        // 用SetReaderInfo()删除读者
                        this.displayLine(GetBR() + getBold(u.UserName + "用SetReaderInfo()删除" + type + "xml，" + expectResult));
                        setReaderResponse = this.SetReaderInfo(u, "delete", xmlPath, "", isReader);
                        this.CheckResult(expectSucc, setReaderResponse.SetReaderInfoResult);
                    }
                    else if (type == C_Type_biblio)
                    {
                        // 用SetBiblioInfo删除书目
                        this.displayLine(GetBR() + getBold(u.UserName + "用SetBiblioInfo()删除" + type + "xml，" + expectResult));
                        setBiblioResponse = this.SetBiblioInfo(u, "delete", xmlPath, "", isReader);
                        this.CheckResult(expectSucc, setBiblioResponse.SetBiblioInfoResult);
                    }
                    else if (type == C_Type_item
                        || type == C_Type_order
                        || type == C_Type_comment
                        || type == C_Type_issue)
                    {
                        result = this.SetItemInfo1(u, type, "delete", xmlPath, "", isReader, out string temp);
                        this.CheckResult(expectSucc, result);
                    }

                }
            }
            else if (action == "get")
            {
                // 用GetRes()获取xml
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes()获取" + type + "xml，" + expectResult));
                getResResponse = this.GetRes(u, xmlPath, isReader);
                this.CheckResult(expectSucc, getResResponse.GetResResult);

                // 用GetRes()获取对象
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes()获取" + type + "的对象，" + expectResult));
                getResResponse = this.GetRes(u, objectPath, isReader);
                this.CheckResult(expectSucc, getResResponse.GetResResult);

                // 用GetRecord()获取xml
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRecord()获取" + type + "xml，" + expectResult));
                GetRecordResponse r1 = this.GetRecord(u, xmlPath, isReader);
                this.CheckResult(expectSucc, r1.GetRecordResult);

                //GetBrowseRecords
                this.displayLine(GetBR() + getBold(u.UserName + "用GetBrowseRecords()获取" + type + "xml，" + expectResult));
                GetBrowseRecordsResponse r2 = this.GetBrowseRecords(u, xmlPath, isReader);
                //取出第一条记录来比对 
                if (r2.searchresults != null && r2.searchresults.Length > 0 && r2.searchresults[0].RecordBody != null)
                {
                    Record rec = r2.searchresults[0];
                    if (expectSucc == true && (rec.RecordBody.Result == null || rec.RecordBody.Result.Value >= 0))
                        this.displayLine("符合预期");
                    else if (expectSucc == false && (rec.RecordBody.Result == null || rec.RecordBody.Result.Value == -1))
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期"));
                }
                else
                {
                    this.CheckResult(expectSucc, r2.GetBrowseRecordsResult);
                }

                //

                // 用专用函数来调
                if (type == C_Type_reader)
                {
                    // 用GetReaderInfo()获取读者xml
                    this.displayLine(GetBR() + getBold(u.UserName + "用GetReaderInfo() 获取" + type + "xml，" + expectResult));
                    getReaderResponse = this.GetReaderInfo(u, xmlPath, isReader);
                    this.CheckResult(expectSucc, getReaderResponse.GetReaderInfoResult);
                }
                else if (type == C_Type_biblio)
                {
                    // 用GetBiblioInfos()获取书目
                    this.displayLine(GetBR() + getBold(u.UserName + "用GetBiblioInfos()获取" + type + "xml，" + expectResult));
                    getBibliosResponse = this.GetBiblioInfos(u, xmlPath, isReader);
                    this.CheckResult(expectSucc, getBibliosResponse.GetBiblioInfosResult);
                }
                else if (type == C_Type_item
                    || type == C_Type_order
                    || type == C_Type_comment
                    || type == C_Type_issue)
                {
                    // 用GetItemInfo()获取册
                    this.displayLine(GetBR() + getBold(u.UserName + "用GetItemInfo()获取" + type + "xml，" + expectResult));
                    GetItemInfoResponse getItemResponse = this.GetItemInfo(u, type, xmlPath, isReader);
                    this.CheckResult(expectSucc, getItemResponse.GetItemInfoResult);
                }
            }
            else
                throw new Exception("DoRes不支持的action=" + action);

            return outputPath;
        }

        // 检查结果
        public bool CheckResult(bool expectSucc, LibraryServerResult result)
        {
            if (expectSucc == true && result.Value >= 0)
                this.displayLine("符合预期");
            else if (expectSucc == false && result.Value == -1)
                this.displayLine("符合预期");
            else
            {
                this.displayLine(getRed("不符合预期"));
                return false;
            }

            return true;
        }
        #endregion




        // 分馆读者
        private void button_fg_reader_reader_Click_1(object sender, EventArgs e)
        {

        }





        #region 操作读者

        private void button_reader_Click(object sender, EventArgs e)
        {
            string accountType = this.comboBox_accountType.Text.Trim();

            if (accountType == C_accountType_zgworker)
                DoReaderForZgWorker();
            else if (accountType == C_accountType_zgreader)
                DoReaderForZgReader();
            else if (accountType == C_accountType_fgworker)
                DoReaderForFgWorker();
            else if (accountType == C_accountType_fgreader)
                DoReaderForFgReader();
            else
                throw new Exception("不支持的身份类型" + accountType);
        }

        // 总馆工作人员 操作 读者
        public void DoReaderForZgWorker()
        {
            /*
 * 总馆馆员，能 新增/修改/删除 本馆的读者，能操作各分馆读者。
 */

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();
                this.displayLine("");

                WriteResResponse response = null;

                // 测试的总馆工作人员帐号
                UserInfo u = this.NewUser(this.SetFullRights(C_Type_reader), "", "");

                //===
                //第1组测试，操作本馆读者
                this.displayLine(getLarge("第1组测试，操作总馆读者"));
                string newPath = GetAppendPath(C_Type_reader, Env_ZG_ReaderDbName);
                // 先新增，应成功
                string xml = this.GetReaderXml(Env_ZG_PatronType, true, out string b1);
                string outputPath = this.DoRes1(u, C_Type_reader, newPath, "new", true, false, xml);
                // 获取/修改/删除   均应成功
                xml = this.GetReaderXml(Env_ZG_PatronType, true, out string b2);
                this.DoResMultiple(u, C_Type_reader, outputPath, "get,change,delete", true, false, xml);



                //===
                // 第2组测试，操作他馆读者
                this.displayLine(getLarge("第2组测试，操作分馆读者"));

                newPath = GetAppendPath(C_Type_reader, Env_A_ReaderDbName);
                // 先新增，应成功
                xml = this.GetReaderXml(Env_A_PatronType, true, out string b3);
                outputPath = this.DoRes1(u, C_Type_reader, newPath, "new", true, false, xml);
                // 获取/修改/删除   均应成功
                xml = this.GetReaderXml(Env_A_PatronType, true, out string b4);
                this.DoResMultiple(u, C_Type_reader, outputPath, "get,change,delete", true, false, xml);


            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // 总馆读者 操作 读者
        public void DoReaderForZgReader()
        {
            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;
                GetResResponse getRes = null;

                this.displayLine(this.getLarge("准备环境"));

                // 用管理员帐号创建一个读者，以下此读者帐号操作
                UserInfo u = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType
                    , this.SetFullRights(C_Type_reader), "",
                    out string readerBarcode,
                    out string ownerReaderPath);
                string ownerObject = ownerReaderPath + "/object/0";

                // 用superviosr帐户创建另一条读者，用于作为他人记录。
                string otherReaderPath = this.CreateReaderBySuperviosr(Env_ZG_ReaderDbName, Env_ZG_PatronType
                    , "", "", out string tempBarcode);
                string otherObject = otherReaderPath + "/object/0";

                // ===
                this.displayLine(this.getLarge("第一组测试：新建读者"));
                string newPath = this.GetAppendPath(C_Type_reader);
                string xml = this.GetReaderXml(Env_ZG_PatronType, true, out string b1);
                this.DoRes1(u, C_Type_reader, newPath, "new", false, true, xml);

                //===
                this.displayLine(this.getLarge("第二组测试1：读者获取自己的xml和对象"));
                this.DoRes1(u, C_Type_reader, ownerReaderPath, "get", true, true, "");

                //===
                this.displayLine(this.getLarge("第二组测试2：读者修改自己的xml和对象"));
                // 把提交的xml记下来，方便后面与结果比对
                string submitXml = this.GetReaderXml(Env_ZG_PatronType, true, out string b2);
                this.DoRes1(u, C_Type_reader, ownerReaderPath, "change", true, true, submitXml);
                // 修改完，需要把读者记录获取出来比对
                getRes = this.GetRes(u, ownerReaderPath, true);
                if (getRes.GetResResult.Value >= 0)
                {
                    string resultXml = Encoding.UTF8.GetString(getRes.baContent);

                    // 比较提交的xml与返回的xml
                    bool bResult = this.CompareReader(submitXml, resultXml,
                        new List<string> { "displayName", "preference", "dprms:file" },  // 希望相等的字段
                        new List<string> { "name" }, // 希望不等的字段
                        out string info);
                    this.displayLine(info);
                    if (bResult == false)
                        this.displayLine(getRed("不符合预期"));
                    else
                        this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(this.getRed("读者获取自己的记录返回-1，不符合预期。"));
                }

                //===
                this.displayLine(this.getLarge("第二组测试3：读者不能删除自己记录"));
                this.DoRes1(u, C_Type_reader, ownerReaderPath, "delete", false, true, "");

                //===
                this.displayLine(this.getLarge("第三组测试：读者不能获取/修改/删除他人记录"));
                xml = this.GetReaderXml(Env_ZG_PatronType, true, out string b3);
                this.DoResMultiple(u, C_Type_reader, otherReaderPath, "get,change,delete", false, true, xml);
            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份操作读者 异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // 分馆工作人员 操作 读者
        public void DoReaderForFgWorker()
        {
            /*
 * 分馆馆员，能 新增/修改/删除 本馆的读者，不能操作他馆读者。
 */

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();
                this.displayLine("");

                WriteResResponse response = null;

                // 测试的A馆工作人员帐号
                UserInfo u = this.NewUser(this.SetFullRights(C_Type_reader), Env_A_LibraryCode, "");

                //===
                //第1组测试，操作本馆读者
                this.displayLine(getLarge("第1组测试，操作本馆读者"));

                string aNewPath = GetAppendPath(C_Type_reader, Env_A_ReaderDbName);

                // 先新增，应成功
                string xml = this.GetReaderXml(Env_A_PatronType, true, out string b1);
                string outputPath = this.DoRes1(u, C_Type_reader, aNewPath, "new", true, false, xml);
                // 获取/修改/删除   均应成功
                xml = this.GetReaderXml(Env_A_PatronType, true, out string b2);
                this.DoResMultiple(u, C_Type_reader, outputPath, "get,change,delete", true, false, xml);



                //===
                // 第2组测试，操作他馆读者
                this.displayLine(getLarge("第2组测试，操作他馆读者"));

                // 给他馆新增记录，应失败 
                string bNewPath = GetAppendPath(C_Type_reader, Env_B_ReaderDbName);
                xml = this.GetReaderXml(Env_B_PatronType, true, out string b3);
                this.DoRes1(u, C_Type_reader, bNewPath, "new", false, false, xml);

                // 用管理员身份新建一个B馆读者，作为他馆读者，测试A馆馆员操作他馆读者
                string readerBarcode = "";
                string otherReaderPath = this.CreateReaderBySuperviosr(Env_B_ReaderDbName, Env_B_PatronType, "",
                    "",
                    out readerBarcode);
                // 获取/修改/删除   均应失败
                xml = this.GetReaderXml(Env_B_PatronType, true, out string b4);
                this.DoResMultiple(u, C_Type_reader, otherReaderPath, "get,change,delete", false, false, xml);

            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // 总馆读者 操作 读者
        public void DoReaderForFgReader()
        {
            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;
                GetResResponse getRes = null;

                this.displayLine(this.getLarge("准备环境"));

                // 用管理员帐号创建一个A馆的读者，以下此读者帐号操作
                UserInfo u = this.NewReaderUser(Env_A_ReaderDbName, Env_A_PatronType,
                    this.SetFullRights(C_Type_reader), "",
                    out string readerBarcode,
                    out string ownerXmlPath);
                string ownerObjectPath = ownerXmlPath + "/object/0";

                // 用管理员帐号创建另一个A馆读者，用于作为本馆他人记录。
                string aXmlPath = this.CreateReaderBySuperviosr(Env_A_ReaderDbName, Env_A_PatronType,
                    "", "", out string tempBarcode);
                string aObjectPath = aXmlPath + "/object/0";

                // 用管理员帐号创建一个B馆读者，用于作为他馆他人记录。
                string bXmlPath = this.CreateReaderBySuperviosr(Env_B_ReaderDbName, Env_B_PatronType
                    , "", "", out tempBarcode);
                string bObjectPath = bXmlPath + "/object/0";

                // ===
                this.displayLine(this.getLarge("第一组测试1：新建本馆读者"));
                string newPath = Env_A_ReaderDbName + "/?";
                string xml = this.GetReaderXml(Env_A_PatronType, true, out string b1);
                this.DoRes1(u, C_Type_reader, newPath, "new", false, true, xml);

                this.displayLine(this.getLarge("第一组测试2：新建他馆读者"));
                newPath = Env_B_ReaderDbName + "/?";
                xml = this.GetReaderXml(Env_B_PatronType, true, out string b2);
                this.DoRes1(u, C_Type_reader, newPath, "new", false, true, xml);


                //===
                this.displayLine(this.getLarge("第二组测试1：读者获取自己的xml和对象"));
                this.DoRes1(u, C_Type_reader, ownerXmlPath, "get", true, true, "");

                //===
                this.displayLine(this.getLarge("第二组测试2：读者修改自己的xml和对象"));
                // 把提交的xml记下来，方便后面与结果比对
                string submitXml = xml = this.GetReaderXml(Env_A_PatronType, true, out string b3);//this.GetXml(C_Type_reader, true);
                this.DoRes1(u, C_Type_reader, ownerXmlPath, "change", true, true, submitXml);
                // 修改完，需要把读者记录获取出来比对
                getRes = this.GetRes(u, ownerXmlPath, true);
                if (getRes.GetResResult.Value >= 0)
                {
                    string resultXml = Encoding.UTF8.GetString(getRes.baContent);

                    // 比较提交的xml与返回的xml
                    bool bResult = this.CompareReader(submitXml, resultXml,
                        new List<string> { "displayName", "preference", "dprms:file" },  // 希望相等的字段
                        new List<string> { "name" }, // 希望不等的字段
                        out string info);
                    this.displayLine(info);
                    if (bResult == false)
                        this.displayLine(getRed("不符合预期"));
                    else
                        this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(this.getRed("读者获取自己的记录返回-1，不符合预期。"));
                }

                //===
                this.displayLine(this.getLarge("第二组测试3：读者不能删除自己记录"));
                this.DoRes1(u, C_Type_reader, ownerXmlPath, "delete", false, true, "");

                //===
                this.displayLine(this.getLarge("第三组测试：读者不能 获取/修改/删除 本馆他人记录"));
                xml = this.GetReaderXml(Env_A_PatronType, true, out string b4);
                this.DoResMultiple(u, C_Type_reader, aXmlPath, "get,change,delete", false, true, xml);


                //===
                this.displayLine(this.getLarge("第四组测试：读者不能 获取/修改/删除 他馆他人记录"));
                xml = this.GetReaderXml(Env_B_PatronType, true, out string b5);
                this.DoResMultiple(u, C_Type_reader, bXmlPath, "get,change,delete", false, true, xml);
            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份操作读者 异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }


        public string C_accountType_zgworker = "总馆工作人员";
        public string C_accountType_zgreader = "总馆读者";
        public string C_accountType_fgworker = "分馆工作人员";
        public string C_accountType_fgreader = "分馆读者";


        // equalFields 希望相等的字段
        // unequalFiles 希望不等的字段
        public bool CompareReader(string submitXml, string resultXml,
            List<string> equalFields,
            List<string> unequalFiles,
            out string info)
        {
            info = "";

            XmlDocument submitDom = new XmlDocument();
            submitDom.LoadXml(submitXml);
            XmlNode submitRoot = submitDom.DocumentElement;

            XmlDocument resultDom = new XmlDocument();
            resultDom.LoadXml(resultXml);
            XmlNode resultRoot = resultDom.DocumentElement;

            bool bResult = true;

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("dprms", DpNs.dprms);

            //XmlNodeList nodes = root.SelectNodes("//dprms:file", nsmgr);

            // 检查希望相等的字段
            foreach (string f in equalFields)
            {
                string text1 = submitRoot.SelectSingleNode(f, nsmgr).InnerText;// DomUtil.GetElementText(submitRoot, f);
                string text2 = resultRoot.SelectSingleNode(f, nsmgr).InnerText;//DomUtil.GetElementText(resultRoot, f);

                //XmlNode node=submitRoot.SelectSingleNode(f,nsmgr);


                if (text1 == text2)
                    info += f + "相同，符合预期。\r\n";
                else
                {
                    info += f + "不同，不符合预期。\r\n";
                    bResult = false;
                }
            }

            // 检查不希望相等的字段
            foreach (string f in unequalFiles)
            {
                string text1 = submitRoot.SelectSingleNode(f, nsmgr).InnerText;// DomUtil.GetElementText(submitRoot, f);
                string text2 = resultRoot.SelectSingleNode(f, nsmgr).InnerText;

                if (text1 != text2)
                    info += f + "不同，符合预期。\r\n";
                else
                {
                    info += f + "相同，不符合预期。\r\n";
                    bResult = false;
                }
            }



            return bResult;
        }

        #endregion


        #region 操作书目

        private void button_biblio_Click(object sender, EventArgs e)
        {
            // 清空输出
            ClearResult();

            string accountType = this.comboBox_accountType.Text.Trim();

            UserInfo u = null;
            bool isReader = false;

            if (accountType == C_accountType_zgworker)
            {
                //分馆馆员，能 新增/修改/删除 书目，书目与总分馆无关
                u = this.NewUser(this.SetFullRights(C_Type_biblio), "", "");
            }
            else if (accountType == C_accountType_zgreader)
            {
                // 用管理员帐户创建一个总馆读者,下面以此读者身份操作
                u = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType, this.SetFullRights(C_Type_biblio), "",
                   out string readerBarcode,
                   out string ownerReaderPath);
                isReader = true;

            }
            else if (accountType == C_accountType_fgworker)
            {
                //分馆馆员，能 新增/修改/删除 书目，书目与总分馆无关
                u = this.NewUser(this.SetFullRights(C_Type_biblio), Env_A_LibraryCode, "");
            }
            else if (accountType == C_accountType_fgreader)
            {
                // 用管理员帐户创建一个分馆读者,下面以此读者身份操作
                u = this.NewReaderUser(Env_A_ReaderDbName, Env_A_PatronType, this.SetFullRights(C_Type_biblio), "",
                   out string readerBarcode,
                   out string ownerReaderPath);
                isReader = true;
            }
            else
                throw new Exception("不支持的身份类型" + accountType);

            // 操作书目
            this.DoBiblio(u, isReader);
        }


        public void DoBiblio(UserInfo u, bool isReader)
        {
            this.EnableCtrls(false);
            try
            {
                // 先新增，应成功
                string newPath = GetAppendPath(C_Type_biblio);
                string xml = this.GetXml(C_Type_biblio, true);
                string outputPath = this.DoRes1(u, C_Type_biblio, newPath, "new", true, isReader, xml);

                // 获取/修改/删除   均应成功
                this.DoResMultiple(u, C_Type_biblio, outputPath, "get,change,delete", true, isReader, xml);

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, u.UserName + "操作读者异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }



        #endregion

        #region 册
        private void button_item_Click(object sender, EventArgs e)
        {
            //确保准备好总分馆环境
            EnsureZfgEnv();

            string accountType = this.comboBox_accountType.Text.Trim();

            if (accountType == C_accountType_zgworker)
            {
                DoItemForZgWorker();
                //MessageBox.Show(this, "未完成");
            }
            else if (accountType == C_accountType_zgreader)
            {
                DoItemForZgReader();
            }
            else if (accountType == C_accountType_fgworker)
            {
                DoItemForFgWorker();

                //MessageBox.Show(this, "未完成");
            }
            else if (accountType == C_accountType_fgreader)
            {
                DoItemForFgReader();
            }
            else
                throw new Exception("不支持的身份类型" + accountType);
        }

        // 总馆工作人员 操作册
        private void DoItemForZgWorker()
        {
            // 总馆馆员，能 新增/修改/删除 本馆的册，能操作各分馆册。

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();
                this.displayLine("");

                // 测试的总馆工作人员帐号
                UserInfo u = this.NewUser(this.SetFullRights(C_Type_item), "", "");

                //===
                //第1组测试，操作总馆册
                this.displayLine(getLarge("第1组测试，操作总馆的册"));

                string newPath = GetAppendPath(C_Type_item);
                // 先新增，应成功
                string loc = this.Env_ZG_Location;
                string itemXml = this.GetItemXml(loc, this.Env_ZG_BookType, true, out string temp1);
                string outputPath = this.DoRes1(u, C_Type_item, newPath, "new", true, false, itemXml);

                // 获取/修改/删除   均应成功
                itemXml = this.GetItemXml(loc, Env_ZG_BookType, true, out string temp2);
                this.DoResMultiple(u, C_Type_item, outputPath, "get,change,delete", true, false, itemXml);

                //===
                // 第2组测试，操作分馆册
                this.displayLine(getLarge("第2组测试，操作分馆册"));
                loc = Env_B_LibraryCode + "/" + Env_B_Location;

                // 先新增，应成功
                itemXml = this.GetItemXml(loc, this.Env_B_BookType, true, out string temp3);
                outputPath = this.DoRes1(u, C_Type_item, newPath, "new", true, false, itemXml);

                // 获取/修改/删除   均应成功
                itemXml = this.GetItemXml(loc, this.Env_B_BookType, true, out string temp4);
                this.DoResMultiple(u, C_Type_item, outputPath, "get,change,delete", true, false, itemXml);


            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }

        }

        // 分馆工作人员 操作册
        private void DoItemForFgWorker()
        {
            // 分馆馆员，能 新增/修改/删除 本馆的册，不能操作他馆册。

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();
                this.displayLine("");

                // 创建分馆工作人员帐号
                UserInfo u = this.NewUser(this.SetFullRights(C_Type_item), Env_A_LibraryCode, "");  //A馆

                //===
                //第1组测试，操作本馆读者
                this.displayLine(getLarge("第1组测试，操作本馆的册"));

                string newPath = GetAppendPath(C_Type_item);

                // 先新增，应成功
                string loc = Env_A_LibraryCode + "/" + Env_A_Location;
                string itemXml = this.GetItemXml(loc, this.Env_A_BookType, true, out string temp1);
                string outputPath = this.DoRes1(u, C_Type_item, newPath, "new", true, false, itemXml);

                // 获取/修改/删除   均应成功
                itemXml = this.GetItemXml(loc, this.Env_A_BookType, true, out string temp2);
                this.DoResMultiple(u, C_Type_item, outputPath, "get,change,delete", true, false, itemXml);



                //===
                // 第2组测试，操作分馆册
                this.displayLine(getLarge("第2组测试，操作他馆册"));
                loc = Env_B_LibraryCode + "/" + Env_B_Location;

                // 新增，应失败
                itemXml = this.GetItemXml(loc, this.Env_B_BookType, true, out string temp3);
                outputPath = this.DoRes1(u, C_Type_item, newPath, "new", false, false, itemXml);

                // 用管理员身份创建一册
                this.displayLine(GetBR() + getBold("用管理员身份创建一条册为后面使用"));
                WriteResResponse writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     itemXml,
                     false);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建册记录异常：" + writeRes.WriteResResult.ErrorInfo);
                outputPath = writeRes.strOutputResPath;

                // 获取，应成功
                this.DoRes1(u, C_Type_item, outputPath, "get", true, false, itemXml);


                // 修改/删除   应失败
                itemXml = this.GetItemXml(loc, this.Env_B_BookType, true, out string temp4);
                this.DoResMultiple(u, C_Type_item, outputPath, "change,delete", false, false, itemXml);


            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }

        }

        // 总馆读者 操作册
        private void DoItemForZgReader()
        {
            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;


                //===
                this.displayLine(this.getLarge("第一组测试：读者无书斋，可获取册，不能新增/修改/删除册。"));

                // 用管理员帐户创建一个读者,没有配置 个人书斋
                UserInfo u = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType,
                    this.SetFullRights(C_Type_item),
                    "",  //个人书斋
                    out string readerBarcode,
                    out string ownerReaderPath);

                // 新增册,应不成功
                string newPath = this.GetAppendPath(C_Type_item);
                string xml = this.GetItemXml(Env_ZG_Location, Env_ZG_BookType, true, out string b1);
                this.DoRes1(u, C_Type_item, newPath, "new", false, true, xml);

                // 用管理员身份创建一条册为后面使用
                this.displayLine(GetBR() + getBold("用管理员身份创建一条册为后面使用"));
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     xml);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建册记录异常：" + writeRes.WriteResResult.ErrorInfo);
                string itemPath = writeRes.strOutputResPath;

                // 应能获取
                this.DoRes1(u, C_Type_item, itemPath, "get", true, true, "");

                // 不能 修改/删除
                xml = this.GetItemXml(Env_ZG_Location, Env_ZG_BookType, true, out string b2);
                this.DoResMultiple(u, C_Type_item, itemPath, "change,delete", false, true, xml);


                // 
                this.displayLine(this.getLarge("第二组测试：读者有书斋，可get/new/change/delete属于书斋的册，对于不属于书斋的册，能get，不能new/change/delete。"));

                // 用管理员帐户创建一个读者,后面用此读者帐户操作
                u = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType, this.SetFullRights(C_Type_item),
                   this.Env_ZG_Location,
                   out readerBarcode,
                   out ownerReaderPath);

                // ===能操作属于个人书斋的册
                // 新建册的馆藏地属于个人书斋
                string itemXml = this.GetItemXml(this.Env_ZG_Location, this.Env_ZG_BookType, true, out string barcode1);
                string ownerPath = this.DoRes1(u, C_Type_item, newPath, "new", true, true, itemXml);

                // 可get/change/delete
                itemXml = this.GetItemXml(this.Env_ZG_Location, this.Env_ZG_BookType, true, out string barcode2);
                this.DoResMultiple(u, C_Type_item, ownerPath, "get,change,delete", true, true, xml);

                // 不能new非个人书斋的册
                itemXml = this.GetItemXml(this.Env_ZG_Location_阅览室, this.Env_ZG_BookType, true, out string barcode3);  //
                this.DoRes1(u, C_Type_item, newPath, "new", false, true, itemXml);

                // 用管理员身份创建一册
                this.displayLine(GetBR() + getBold("用管理员身份创建一条册为后面使用"));
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     itemXml,
                     false);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建册记录异常：" + writeRes.WriteResResult.ErrorInfo);
                string otherPath = writeRes.strOutputResPath;

                // 应能获取非个人书斋的册
                this.DoRes1(u, C_Type_item, otherPath, "get", true, true, "");

                // 不能 修改/删除 非个人书斋的册
                itemXml = this.GetItemXml(this.Env_ZG_Location_阅览室, this.Env_ZG_BookType, true, out string barcode4);  //
                this.DoResMultiple(u, C_Type_item, otherPath, "change,delete", false, true, xml);

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份操作册-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }


        // 分馆读者 操作册
        private void DoItemForFgReader()
        {
            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;
                string newPath = this.GetAppendPath(C_Type_item);



                // 用管理员帐户创建一个分馆读者,后面用此读者帐户操作
                UserInfo u = this.NewReaderUser(Env_A_ReaderDbName, this.Env_A_PatronType, this.SetFullRights(C_Type_item),  //A馆读者
                  Env_A_Location,  //个人书斋
                   out string readerBarcode,
                   out string ownerReaderPath);

                // ===能操作属于本分馆个人书斋的册
                this.displayLine(this.getLarge("第一组：分馆读者 可以get/new/change/delete属于书斋的册，对于不属于书斋的册，能get，不能new/change/delete。"));
                string loc = Env_A_LibraryCode + "/" + Env_A_Location;  //新建册的馆藏地属于个人书斋
                string itemXml = this.GetItemXml(loc, this.Env_A_BookType, true, out string barcode1);

                // 可新建个人书斋
                string ownerPath = this.DoRes1(u, C_Type_item, newPath, "new", true, true, itemXml);

                // 可get/change/delete
                itemXml = this.GetItemXml(loc, this.Env_A_BookType, true, out string barcode2);
                this.DoResMultiple(u, C_Type_item, ownerPath, "get,change,delete", true, true, itemXml);

                // ====对本馆非个人书斋，可get，不可change/delete
                this.displayLine(this.getLarge("第二组：分馆读者 对于不属于书斋的册，能get，不能new/change/delete。"));
                // 不能new非个人书斋的册
                loc = Env_A_LibraryCode + "/" + Env_A_Location_阅览室;
                itemXml = this.GetItemXml(loc, this.Env_A_BookType, true, out string barcode3);
                this.DoRes1(u, C_Type_item, newPath, "new", false, true, itemXml);

                // 用管理员身份创建一册
                this.displayLine(GetBR() + getBold("用管理员身份创建一条册为后面使用"));
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     itemXml,
                     false);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建册记录异常：" + writeRes.WriteResResult.ErrorInfo);
                string otherPath = writeRes.strOutputResPath;

                // 应能获取本馆非个人书斋的册
                this.DoRes1(u, C_Type_item, otherPath, "get", true, true, "");

                // 不能 修改/删除 非个人书斋的册
                itemXml = this.GetItemXml(loc, this.Env_A_BookType, true, out string barcode4);
                this.DoResMultiple(u, C_Type_item, otherPath, "change,delete", false, true, itemXml);


                // 对本馆非个人书斋，可读，不可change/delete
                this.displayLine(this.getLarge("第三组：分馆读者 对于他馆册馆藏地与个人书斋相同的册，能get，不能new/change/delete。"));
                // 不能new他馆的册
                loc = Env_B_LibraryCode + "/" + Env_A_Location; //注意：这里馆藏地设为”A馆图书馆“是故意的
                itemXml = this.GetItemXml(loc, this.Env_B_BookType, true, out string barcode5);
                this.DoRes1(u, C_Type_item, newPath, "new", false, true, itemXml);

                // 用管理员身份创建一册
                this.displayLine(GetBR() + getBold("用管理员身份创建一条册为后面使用"));
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     itemXml,
                     false);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建册记录异常：" + writeRes.WriteResResult.ErrorInfo);
                otherPath = writeRes.strOutputResPath;

                // 应能获取他馆的册
                this.DoRes1(u, C_Type_item, otherPath, "get", true, true, "");

                // 不能 修改/删除 他馆的册
                itemXml = this.GetItemXml(loc, this.Env_B_BookType, true, out string barcode6);
                this.DoResMultiple(u, C_Type_item, otherPath, "change,delete", false, true, itemXml);

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "分馆读者身份操作册-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }


        // 用管理员身份创建册记录
        public string CreateItemBySupervisor(string location, string bookType, out string itemBarcode)
        {
            // 用管理员帐号给总馆馆藏地创建建册记录
            string newPath = this.GetAppendPath(C_Type_item);
            string xml = this.GetItemXml(location, bookType, true, out itemBarcode);
            WriteResResponse writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                 newPath,
                 xml);
            if (writeRes.WriteResResult.Value == -1)
                throw new Exception("管理员创建册异常：" + writeRes.WriteResResult.ErrorInfo);
            return writeRes.strOutputResPath;
        }

        // 用管理身份创建预约到书 或 违约金记录
        public string CreateArrivedAmerceBySupervisor(string type, string libraryCode, string readerBarcode, string itemBarcode, string location)
        {
            // 获取xml
            string strXml = GetArrivedOrAmerceXml(type, libraryCode, readerBarcode, itemBarcode, location);

            this.displayLine("管理员为读者" + readerBarcode + "针对册" + itemBarcode + "创建'" + type + "'记录");
            string newPath = this.GetAppendPath(type);
            WriteResResponse writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                                                 newPath,
                                                 strXml,
                                                 false);
            if (writeRes.WriteResResult.Value == -1)
                throw new Exception("管理员写" + type + "记录异常：" + writeRes.WriteResResult.ErrorInfo);

            return writeRes.strOutputResPath;
        }

        public string GetArrivedOrAmerceXml(string type, string libraryCode, string readerBarcode, string itemBarcode, string location)
        {
            if (type == C_Type_arrived)
                return GetArrivedXml(libraryCode, readerBarcode, itemBarcode, location, true);
            else if (type == C_Type_amerce)
                return this.GetAmerceXml(libraryCode, readerBarcode, itemBarcode, location, true);
            else
                throw new Exception("GetArrivedOrAmerceXml()不支持的类型" + type);
        }



        private void button_readerLogin_item3_Click(object sender, EventArgs e)
        {
            // 准备环境：
            // 用管理员帐户，给总馆创建两个读者，两册，读者分别借这两册
            // 给分馆创建两个读者，两册，用分馆读者分别借分馆两册
            // 然后用4种身份获取记录测试

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                #region 用超级管理员身份创建环境

                // 用管理员帐户创建第1个总馆读者,后面用此读者帐户操作
                UserInfo uZgReader = this.NewReaderUser(Env_ZG_ReaderDbName, Env_ZG_PatronType,
                    "getiteminfo,getitemobject", "",
                    out string zgReader1,
                    out string zgReaderPath);
                // 创建第2个总馆读者
                this.CreateReaderBySuperviosr(Env_ZG_ReaderDbName, Env_ZG_PatronType,
                    "", "",
                    out string zgReader2);

                // 用管理员帐号给总馆馆藏地创建建两个册记录
                string zgItemPath1 = this.CreateItemBySupervisor(this.Env_ZG_Location, this.Env_ZG_BookType, out string zgItemBarcode1);
                string zgItemPath2 = this.CreateItemBySupervisor(this.Env_ZG_Location, this.Env_ZG_BookType, out string zgItemBarcode2);

                // 为总馆两个读者分别各借一册
                this.Borrow(this.mainForm.GetSupervisorAccount(), zgReader1, zgItemBarcode1, false);
                this.Borrow(this.mainForm.GetSupervisorAccount(), zgReader2, zgItemBarcode2, false);


                //==创建分馆读者/册/借书
                UserInfo uFgReader = this.NewReaderUser(Env_A_ReaderDbName, Env_A_PatronType,
                    "getiteminfo,getitemobject", "",
                    out string fgReader1,
                    out string fgReaderPath);
                // 创建第2个分馆读者
                this.CreateReaderBySuperviosr(Env_A_ReaderDbName, Env_A_PatronType, "", "", out string fgReader2);

                // 用管理员帐号给总馆馆藏地创建建两个册记录
                string loc = this.Env_A_LibraryCode + "/" + this.Env_A_Location;
                string fgItemPath1 = this.CreateItemBySupervisor(loc, this.Env_A_BookType, out string fgItemBarcode1);
                string fgItemPath2 = this.CreateItemBySupervisor(loc, this.Env_A_BookType, out string fgItemBarcode2);

                // 为总馆两个读者分别各借一册
                this.Borrow(this.mainForm.GetSupervisorAccount(), fgReader1, fgItemBarcode1, false);
                this.Borrow(this.mainForm.GetSupervisorAccount(), fgReader2, fgItemBarcode2, false);

                // 创建一个总馆工作人员
                UserInfo uZgWorker = this.NewUser("", "", "");  //todo 为什么没有权限呢
                // 创建一个分馆工作人员
                UserInfo uFgWorker = this.NewUser("", Env_A_LibraryCode, "");

                #endregion

                //==总馆读者获取册
                this.displayLine(this.getLarge("总馆读者获取总馆册（借阅者是自己）"));
                this.DoRes1(uZgReader, C_Type_item, zgItemPath1, "get", true, true, "");

                this.displayLine(this.getLarge("总馆读者获取总馆册（借阅者是别人），注意观察是否脱敏。"));
                this.DoRes1(uZgReader, C_Type_item, zgItemPath2, "get", true, true, "");

                this.displayLine(this.getLarge("总馆读者获取分馆册（借阅者是别人），注意观察是否脱敏。"));
                this.DoRes1(uZgReader, C_Type_item, fgItemPath1, "get", true, true, "");


                //==分馆读者获取册
                this.displayLine(this.getLarge("分馆读者获取分馆册（借阅者是自己）"));
                this.DoRes1(uFgReader, C_Type_item, fgItemPath1, "get", true, true, "");

                this.displayLine(this.getLarge("分馆读者获取分馆册（借阅者是别人），注意观察是否脱敏。"));
                this.DoRes1(uFgReader, C_Type_item, fgItemPath2, "get", true, true, "");

                this.displayLine(this.getLarge("分馆读者获取总馆册（借阅者是别人），注意观察是否脱敏。"));
                this.DoRes1(uFgReader, C_Type_item, zgItemPath1, "get", true, true, "");



            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份获取册-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }




        #endregion

        private void button_deleteBiblioHHasChildren_Click(object sender, EventArgs e)
        {
            //册
            this.TestBiblioHasChildren(C_Type_item);
            // 订购
            this.TestBiblioHasChildren(C_Type_order);
            // 评注
            this.TestBiblioHasChildren(C_Type_comment);
            //期
            this.TestBiblioHasChildren(C_Type_issue);

        }

        public void TestBiblioHasChildren(string type)
        {
            //===
            // 书目下，两个下级都没有file
            this.displayLine(this.getLarge("第1组测试：书目下两个下级" + type + "都没有file"));

            // 无下级setXXXinfo，应失败。
            this.displayLine("有书目权限，无下级" + type + "的setXXXinfo，应失败");
            string biblioPath = this.CreateBiblioHasChildrenBySupervisor(type, new List<bool> { false, false });
            string rights = this.SetInfoRights(C_Type_biblio, true);//this.GetFullRights(C_Type_biblio);
            UserInfo u = this.NewUser(rights, "", "");
            this.DoRes1(u, C_Type_biblio, biblioPath, "delete", false, false, "");

            //有下级的setXXXinfo权限，无连带的getXXXinfo，应失败。
            this.displayLine("有书目权限，有下级" + type + "的setXXXinfo权限，无连带的getXXXinfo，应失败。");
            //biblioPath = this.CreateBiblioHasChildrenBySupervisor(type, new List<bool> { false, false });
            rights = this.SetInfoRights(C_Type_biblio, true) + "," + this.SetInfoRights(type, false);
            u = this.NewUser(rights, "", "");
            this.DoRes1(u, C_Type_biblio, biblioPath, "delete", false, false, "");

            //有下级的setXXXinfo权限，有连带的getXXXinfo，应成功。
            this.displayLine("有书目权限，有下级" + type + "的set/getXXXinfo权限，应成功。");
            //biblioPath = this.CreateBiblioHasChildrenBySupervisor(type, new List<bool> { false, false });
            rights = this.SetInfoRights(C_Type_biblio, true) + "," + this.SetInfoRights(type, true); ;
            u = this.NewUser(rights, "", "");
            this.DoRes1(u, C_Type_biblio, biblioPath, "delete", true, false, "");


            //===
            // 书目下两个下级其中1个有file
            this.displayLine(this.getLarge("第2组测试：书目下两个下级" + type + "其中1个有file"));

            //有书目权限，有下级的set/getXXXinfo，无下级setXXXobject，应失败。
            this.displayLine("有书目权限，有下级" + type + "的set/getXXXinfo，无对象set/getXXXobject，应失败。");
            biblioPath = this.CreateBiblioHasChildrenBySupervisor(type, new List<bool> { false, true });
            rights = this.SetInfoRights(C_Type_biblio, true) + "," + this.SetInfoRights(type, true);
            u = this.NewUser(rights, "", "");
            this.DoRes1(u, C_Type_biblio, biblioPath, "delete", false, false, "");

            //有书目权限，有下级的set/getXXXinfo，有下级setXXXobject，无连带的getXXXobject,应失败。
            this.displayLine("有书目权限，有下级" + type + "的set/getXXXinfo，有对象setXXXobject，无连带的getXXXobject,应失败。");
            // biblioPath = this.CreateBiblioHasChildrenBySupervisor(type, new List<bool> { false, true });
            rights = this.SetInfoRights(C_Type_biblio, true) + "," + this.SetInfoRights(type, true) + "," + this.SetObjectRights(type);
            u = this.NewUser(rights, "", "");
            this.DoRes1(u, C_Type_biblio, biblioPath, "delete", false, false, "");


            //有书目权限，有下级的set/getXXXinfo，有下级setXXXobject，有连带的getXXXobject，应成功。
            this.displayLine("有书目权限，有下级" + type + "的set/getXXXinfo，有对象set/getXXXobject，应成功。");
            //biblioPath = this.CreateBiblioHasChildrenBySupervisor(type, new List<bool> { false, true });
            rights = this.SetInfoRights(C_Type_biblio, true) + "," + this.SetInfoRights(type, true) + "," + this.SetObjectRights(type, true);
            u = this.NewUser(rights, "", "");
            this.DoRes1(u, C_Type_biblio, biblioPath, "delete", true, false, "");
        }

        // 用管理员身份创建册记录
        public string CreateBiblioHasChildrenBySupervisor(string type, List<bool> childHasFile)
        {
            UserInfo u = this.mainForm.GetSupervisorAccount();
            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, false);


                // 创建一条书目
                // 先新增，应成功
                string newPath = GetAppendPath(C_Type_biblio);
                string xml = this.GetXml(C_Type_biblio, false);

                // 用WriteRes()新建读者xml
                WriteResResponse writeResponse = this.WriteXml(u, newPath, xml, false);
                string biblioPath = writeResponse.strOutputResPath;
                string parent = this.GetBiblioParent(biblioPath);

                // 创建下级
                foreach (bool hasFile in childHasFile)
                {
                    string childPath = GetAppendPath(type);
                    string childXml = this.GetXml(type, hasFile, out string barcode1, parent);
                    this.WriteXml(u, childPath, childXml, false);
                }

                return biblioPath;
            }
            catch (Exception ex)
            {
                throw new Exception(u.UserName + "异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }

        }

        public void CreateBiblioChildren(string biblioPath, string type, bool childHasFile)
        {
            // 创建下级册
            string parent = this.GetBiblioParent(biblioPath);

            string newPath = this.GetAppendPath(C_Type_item);
            string xml = this.GetXml(type, childHasFile, out string barcode, parent);
            WriteResResponse writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                 newPath,
                 xml);
            if (writeRes.WriteResResult.Value == -1)
                throw new Exception("管理员创建册异常：" + writeRes.WriteResResult.ErrorInfo);
            //return writeRes.strOutputResPath;
        }

        // 获取测试头标区相关的xml
        private string GetXmlForHeader(string header, string field200a)
        {
            string leaderXml = "";
            if (string.IsNullOrEmpty(header) == false)
                leaderXml = "<unimarc:leader>" + header + "</unimarc:leader>";

            return "<unimarc:record xmlns:dprms='http://dp2003.com/dprms' xmlns:unimarc='http://dp2003.com/UNIMARC'>"
                  + leaderXml
                  + @"<unimarc:datafield tag='200' ind1='1' ind2=' '>
                    <unimarc:subfield code='a'>" + field200a + @"</unimarc:subfield>
                  </unimarc:datafield>
                </unimarc:record>";
        }

        // 4种书目xml：正常的头标区，24个?号，*?，无leader元素
        string _headerCommon = "01992nam0 22003851  450 ";
        string _xmlCommon = "";

        string _header24 = "????????????????????????";
        string _xml24 = "";

        string _headerAutoChanged = "*???????????????????????";
        string _xmlAutoChanged = "";//this.GetXmlForHeader(headerAutoChanged, "头标区输入的*?");

        string _xmlNoHeader = "";//this.GetXmlForHeader(null, "无leader元素");

        // 头标区测试
        private void button_leader_Click(object sender, EventArgs e)
        {
            string rights = "";
            string access = "";

            // 给4种书目xml的变量赋好值：正常的头标区，24个?号，*?，无leader元素
            this._xmlCommon = this.GetXmlForHeader(this._headerCommon, "正常的头标区");
            this._xml24 = this.GetXmlForHeader(_header24, "头标区输入的24个?号");
            this._xmlAutoChanged = this.GetXmlForHeader(_headerAutoChanged, "头标区输入的*?");
            this._xmlNoHeader = this.GetXmlForHeader(null, "无leader元素");

            // 三种头标区形态的路径
            string pathCommon = "";
            string path24 = "";
            string pathAutoChanged = "";

            #region setbiblioinfo

            //===========
            // setbiblioinfo()
            // 有###权限（3种帐户配置）
            List<string> expectHeadersForHasRights = new List<string> {
                this._headerCommon,
                this._headerAutoChanged,
                this._headerAutoChanged,
                this._headerAutoChanged
            };
            List<bool> expectSetNoErrorList = new List<bool> {
                true,
                true,
                true,
                true };
            // 帐号1，有大权限
            this.displayLine(this.getLarge("setbiblioinfo()，有权限帐号1，有大权限"));
            rights = "getbiblioinfo,setbiblioinfo";
            UserInfo u = this.NewUser(rights, "", "");
            List<string> pathList1 = this.Write4HeaderData(u, expectHeadersForHasRights, expectSetNoErrorList);

            // 取出2条路径，方便后面测试getbiblioinfo使用。
            pathCommon = pathList1[0];
            pathAutoChanged = pathList1[1];


            // 帐号2，在存取定义中配置的*号,表示完整字段权限
            this.displayLine(this.getLarge("setbiblioinfo()，有权限帐号2，在存取定义中配置的*号,表示完整字段权限"));
            access = Env_biblioDbName + ":setbiblioinfo=*|getbiblioinfo=*";  //中文图书:setbiblioinfo=*|getbiblioinfo=*
            u = this.NewUser("", "", access);
            List<string> pathList2 = this.Write4HeaderData(u, expectHeadersForHasRights, expectSetNoErrorList);

            // 帐号3，在存取定义中指定了###字段
            this.displayLine(this.getLarge("setbiblioinfo()，有权限帐号3，在存取定义中指定了###字段"));
            access = Env_biblioDbName + ":setbiblioinfo=*(###,200)|getbiblioinfo=*"; //"中文图书:setbiblioinfo=*(###,200)|getbiblioinfo=*";
            u = this.NewUser("", "", access);
            List<string> pathList3 = this.Write4HeaderData(u, expectHeadersForHasRights, expectSetNoErrorList);

            // 无###权限
            List<string> expectHeadersForNoRights = new List<string> {
                this._header24,
                this._header24,
                this._header24,
                this._header24
            };
            expectSetNoErrorList = new List<bool> {
                false,  //部分写入，头标区拒绝
                true,
                false,//部分写入，头标区拒绝
                true
            };
            // 中文图书:setbiblioinfo=*(200)|getbiblioinfo=*
            this.displayLine(this.getLarge("setbiblioinfo()，无###权限帐号"));
            access = Env_biblioDbName + ":setbiblioinfo=*(200)|getbiblioinfo=*";
            u = this.NewUser("", "", access);
            List<string> pathList4 = this.Write4HeaderData(u, expectHeadersForNoRights, expectSetNoErrorList);

            // 取出1条路径，方便后面测试getbiblioinfo使用。
            path24 = pathList4[0];

            #endregion

            #region getbiblioinfo

            //=========
            //getbiblioinfo()
            //有###权限（3种帐户配置）
            rights = "getbiblioinfo,setbiblioinfo";
            u = this.NewUser(rights, "", "");
            this.displayLine(this.getLarge("getbiblioinfo()，有权限帐号1"));
            bool bRet = CheckHeader(u, pathCommon, _headerCommon);
            bRet = CheckHeader(u, path24, _header24);
            bRet = CheckHeader(u, pathAutoChanged, _headerAutoChanged);

            //==
            access = Env_biblioDbName + ":getbiblioinfo=*";
            u = this.NewUser("", "", access);
            this.displayLine(this.getLarge("getbiblioinfo()，有权限帐号2"));
            bRet = CheckHeader(u, pathCommon, _headerCommon);
            bRet = CheckHeader(u, path24, _header24);
            bRet = CheckHeader(u, pathAutoChanged, _headerAutoChanged);

            //==
            access = Env_biblioDbName + ":getbiblioinfo=*(###,200)";
            u = this.NewUser("", "", access);
            this.displayLine(this.getLarge("getbiblioinfo()，有权限帐号3"));
            bRet = CheckHeader(u, pathCommon, _headerCommon);
            bRet = CheckHeader(u, path24, _header24);
            bRet = CheckHeader(u, pathAutoChanged, _headerAutoChanged);

            //==
            // 无权限
            access = Env_biblioDbName + ":getbiblioinfo=*(200)";
            u = this.NewUser("", "", access);
            this.displayLine(this.getLarge("getbiblioinfo()，无###权限帐号"));
            this.displayLine("\r\n无权限帐户，获取普通头标区的记录。");
            bRet = CheckHeader(u, pathCommon, _header24);
            this.displayLine("\r\n无权限帐户，获取头标区为24个?号的记录。");
            bRet = CheckHeader(u, path24, _header24);
            this.displayLine("\r\n无权限帐户，获取头标区为*?的记录。");
            bRet = CheckHeader(u, pathAutoChanged, _header24);


            #endregion

            #region copybiblioinfo

            #region copybiblioinfo 第一种情况：来源有###权限，目标有###权限

            //===第一种帐户================
            string appendTargetPath = this.GetAppendPath(C_Type_biblio, Env_biblioDbName_forCopy);
            this.displayLine(this.getLarge("CopyBiblioInfo()，第一种帐户：来源有###权限，目标有###权限。"));
            access = Env_biblioDbName + ":getbiblioinfo=*;" + Env_biblioDbName_forCopy + ":setbiblioinfo=*|getbiblioinfo=*";  //中文图书:getbiblioinfo=*;测试中文:setbiblioinfo=*
            u = this.NewUser("", "", access);

            // 源头标区正常，预期目标记录为正常的头标区
            this.displayLine("源头标区正常，预期目标记录为正常的头标区");
            CopyBiblioInfoResponse copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, "", "", false);
            string path = copyResponse.strOutputBiblioRecPath;

            this.CheckHeader(u, path, _headerCommon);

            //源头标区24个?,预期目标记录为*?
            this.displayLine("源头标区24个?,预期目标记录为*?");
            copyResponse = this.CopyBiblio(u, "copy", path24, appendTargetPath, "", "", false);
            path = copyResponse.strOutputBiblioRecPath;
            this.CheckHeader(u, path, _headerAutoChanged);

            // 源头标区*?,预期目标记录为*?
            this.displayLine("源头标区*?,预期目标记录为*?");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, "", "", false);
            path = copyResponse.strOutputBiblioRecPath;
            this.CheckHeader(u, path, _headerAutoChanged);

            #endregion

            #region copybiblioinfo 第二种情况：来源有###权限，目标无###权限

            //==第二种帐户====
            this.displayLine(this.getLarge("CopyBiblioInfo()，第二种帐户：来源有###权限，目标无###权限。"));
            access = Env_biblioDbName + ":getbiblioinfo=*;" + Env_biblioDbName_forCopy + ":setbiblioinfo=*(200)|getbiblioinfo=*";  //中文图书:getbiblioinfo=*;测试中文:setbiblioinfo=*(200)
            u = this.NewUser("", "", access);


            //==
            // 源头标区正常，预期目标记录为正常的头标区
            this.displayLine("源头标区正常，因无权限，预期目标记录为24个?");
            copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, "", "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。目标无###权限时，源数据有正常头标区，CopyBiblioInfo()应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, "", "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            if (copyResponse.CopyBiblioInfoResult.ErrorCode != ErrorCode.PartialDenied)
            {
                this.displayLine(this.getRed("不符合预期。错误码应为PartialDenied"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _header24);

            //===
            //源头标区24个?,预期目标记录为*?
            this.displayLine("源头标区24个?,因无权限，预期目标记录为24个?，由于来源本身是24个?，所以不会有权限不足的提示，不需要用loose。");
            copyResponse = this.CopyBiblio(u, "copy", path24, appendTargetPath, "", "", false);
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _header24);

            //===
            // 源头标区*?,预期目标记录为*?
            this.displayLine("源头标区*?,因无权限，预期目标记录为24个?");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, "", "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。目标无###权限时，源数据为*?，CopyBiblioInfo()应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, "", "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            if (copyResponse.CopyBiblioInfoResult.ErrorCode != ErrorCode.PartialDenied)
            {
                this.displayLine(this.getRed("不符合预期。错误码应为PartialDenied"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _header24);

            #endregion


            #region copybiblioinfo 第三种情况：来源无###权限，目标有###权限

            //==第三种帐户====
            this.displayLine(this.getLarge("CopyBiblioInfo()，第三种情况：来源无###权限，目标有###权限。"));
            access = Env_biblioDbName + ":getbiblioinfo=*(200,997);" + Env_biblioDbName_forCopy + ":setbiblioinfo=*|getbiblioinfo=*";  //中文图书:getbiblioinfo=*(200,997);测试中文:setbiblioinfo=*
            u = this.NewUser("", "", access);


            //==
            // 源头标区正常，预期目标记录为*?
            this.displayLine("源头标区正常，预期目标记录为*?");
            copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, "", "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。来源无###权限时，源数据有正常头标区，应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, "", "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _headerAutoChanged);

            //===
            //源头标区24个?,预期目标记录为*?
            this.displayLine("源头标区24个?,预期目标记录为*?，由于来源本身是24个?，所以不会有权限不足的提示，不需要用loose。");
            copyResponse = this.CopyBiblio(u, "copy", path24, appendTargetPath, "", "", false);
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _headerAutoChanged);

            //===
            // 源头标区*?,预期目标记录为*?
            this.displayLine("源头标区*?,因无权限，预期目标记录为24个?");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, "", "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。无目标###权限时，源数据为*?，应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, "", "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _headerAutoChanged);

            #endregion


            #region copybiblioinfo 第四种情况：来源无###权限，目标无###权限


            //==第四种帐户====
            this.displayLine(this.getLarge("CopyBiblioInfo()，第四种情况：来源无###权限，目标无###权限。"));
            access = Env_biblioDbName + ":getbiblioinfo=*(200,997);" + Env_biblioDbName_forCopy + ":setbiblioinfo=*(200)|getbiblioinfo=*";  //中文图书:getbiblioinfo=*(200,997);测试中文:setbiblioinfo=*(200)|getbiblioinfo=*
            u = this.NewUser("", "", access);


            //==
            // 源头标区正常，预期目标记录为*?
            this.displayLine("源头标区正常，预期目标记录为24个问题");
            copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, "", "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。来源无###权限时，源数据有正常头标区，应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, "", "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _header24);

            //===
            //源头标区24个?,预期目标记录为*?
            this.displayLine("源头标区24个?,因无权限，预期目标记录为24个?，由于来源本身是24个?，所以读取时不会有权限不足的提示，不需要用loose。");
            copyResponse = this.CopyBiblio(u, "copy", path24, appendTargetPath, "", "", false);
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _header24);

            //===
            // 源头标区*?,预期目标记录为*?
            this.displayLine("源头标区*?,因无权限，预期目标记录为24个?");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, "", "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。无目标###权限时，源数据为*?，应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, "", "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            // 检查目标记录，因无权限应为24个?
            this.CheckHeader(u, path, _header24);

            #endregion

            #region copybiblioinfo 第五种情况：来源无###权限，目标有###权限，测strNewBiblio

            //==第五种情况====
            this.displayLine(this.getLarge("CopyBiblioInfo()，第五种情况：来源无###权限，目标有###权限，测strNewBiblio"));
            access = Env_biblioDbName + ":getbiblioinfo=*(200,997);" + Env_biblioDbName_forCopy + ":setbiblioinfo=*|getbiblioinfo=*";  //中文图书:getbiblioinfo=*(200,997);测试中文:setbiblioinfo=*
            u = this.NewUser("", "", access);

            //==
            this.displayLine("strNewBiblio为正常头标区，预期目标记录为正常头标区");
            copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, this._xmlCommon, "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。来源无###权限时，源数据有正常头标区，应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathCommon, appendTargetPath, this._xmlCommon, "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            this.CheckHeader(u, path, _headerCommon);

            //===
            this.displayLine("strNewBiblio为24个?,预期目标记录为*?");
            copyResponse = this.CopyBiblio(u, "copy", path24, appendTargetPath, _xml24, "", false);
            path = copyResponse.strOutputBiblioRecPath;
            this.CheckHeader(u, path, _headerAutoChanged);

            //===
            this.displayLine("strNewBiblio为*?,预期目标记录为*?");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, _xmlAutoChanged, "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。无目标###权限时，源数据为*?，应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, _xmlAutoChanged, "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            this.CheckHeader(u, path, _headerAutoChanged);

            //===
            this.displayLine("strNewBiblio为无leader元素,预期目标记录为*?");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, _xmlNoHeader, "", false);
            if (copyResponse.CopyBiblioInfoResult.Value != -1)
            {
                this.displayLine(this.getRed("不符合预期。无目标###权限时，源数据为*?，应报权限不足"));
                return;
            }
            // 用loose参数再次写入
            this.displayLine("使用loose参数再次copy。");
            copyResponse = this.CopyBiblio(u, "copy", pathAutoChanged, appendTargetPath, _xmlNoHeader, "loose", false);
            if (copyResponse.CopyBiblioInfoResult.Value == -1)
            {
                this.displayLine(this.getRed("不符合预期。用loose参数，应能正常copy。"));
                return;
            }
            path = copyResponse.strOutputBiblioRecPath;
            this.CheckHeader(u, path, _headerAutoChanged);

            #endregion

            #endregion

        }

        private List<string> Write4HeaderData(UserInfo u, List<string> expectHeaders, List<bool> expectSetNoErrorList)
        {
            List<string> pathList = new List<string>();

            // 写普通的头标区，预期完全一致
            this.displayLine(this.getBold("\r\n第1种：写普通的头标区，预期为" + expectHeaders[0]));
            string path = this.writeLeaderXml(u, this._xmlCommon, expectHeaders[0], expectSetNoErrorList[0]);
            pathList.Add(path);

            // 写24个?号，预期为*?
            this.displayLine(this.getBold("\r\n第2种：写24个?号，预期为" + expectHeaders[1]));
            path = this.writeLeaderXml(u, this._xml24, expectHeaders[1], expectSetNoErrorList[1]);
            pathList.Add(path);


            // 写*?，预期为*?
            this.displayLine(this.getBold("\r\n第3种：写*?，预期为" + expectHeaders[2]));
            path = this.writeLeaderXml(u, this._xmlAutoChanged, expectHeaders[2], expectSetNoErrorList[2]);
            pathList.Add(path);

            // 写无header，预期为*?
            this.displayLine(this.getBold("\r\n第4种：写无header，预期为" + expectHeaders[3]));
            path = this.writeLeaderXml(u, this._xmlNoHeader, expectHeaders[3], expectSetNoErrorList[3]);
            pathList.Add(path);


            return pathList;
        }


        // 返回写入记录的路径
        public string writeLeaderXml(UserInfo u, string xml, string expectHeader, bool expectSetNoError)
        {
            string outputPath = "";

            string appendPath = this.GetAppendPath(C_Type_biblio, Env_biblioDbName);

            // 写xml
            SetBiblioInfoResponse setResponse = this.SetBiblioInfo(u, "new", appendPath, xml, false);
            if (setResponse.SetBiblioInfoResult.Value == -1)
            {
                this.displayLine(getRed("写入记录返回-1，不符合预期"));
                goto END1;
            }
            //bool bRet = this.CheckResult(true, setResponse.SetBiblioInfoResult);  // 预期写入成功
            //if (bRet == false)  //不符合预期的话，退出不再继续。
            //    goto END1;

            // 这里要比对错误码
            if (expectSetNoError == true)
            {
                if (setResponse.SetBiblioInfoResult.ErrorCode != ErrorCode.NoError)
                {
                    this.displayLine(this.getRed("不符合预期，错误码须为NoError"));
                }
            }
            else
            {
                if (setResponse.SetBiblioInfoResult.ErrorCode != ErrorCode.PartialDenied)
                {
                    this.displayLine(this.getRed("不符合预期，错误码须为PartialDenied"));
                }
            }

            // 再取出来这条记录，比对写入的头标区是否与提交的一致
            outputPath = setResponse.strOutputBiblioRecPath;
            bool bRet = CheckHeader(u, outputPath, expectHeader);

        END1:

            return outputPath;
        }


        // 检查头标区
        public bool CheckHeader(UserInfo u, string resPath, string expectHeader)
        {
            bool bRet = false;

            this.displayLine("\r\n");//间隔一下

            GetBiblioInfosResponse getResponse = this.GetBiblioInfos(u, resPath, false);
            if (getResponse.GetBiblioInfosResult.Value == -1)
            {
                this.displayLine(getRed("获取记录返回-1，不符合预期"));
                goto END1;
            }
            if (getResponse.results == null || getResponse.results.Length != 1)
            {
                this.displayLine(this.getRed("不符合预期，用GetBiblioInfos获取刚写入的记录[" + resPath + "]，result应该等于1才对。"));
                goto END1;
            }
            string okXml = getResponse.results[0];

            int nRet = MarcUtil.Xml2Marc(okXml,
                false,
                "", // 自动识别 MARC 格式
                out string strOutMarcSyntax,
                out string strMARC,
                out string strError);
            if (nRet == -1)
            {
                this.displayLine(this.getRed("调Xml2Marc()将写入的xml转为marc时出错" + strError));
                goto END1;
            }

            this.displayLine("\r\n");//加一空格，阅读起来容易些
            MarcRecord marcRecord = new MarcRecord(strMARC);
            string okHeader = marcRecord.Header.ToString();
            if (okHeader != expectHeader)
            {
                this.displayLine(getRed("校验库中的头标区为[" + okHeader + "]" +
                    ",不是预期的值[" + expectHeader + "]，不符合预期。"));
            }
            else
            {
                bRet = true;
                this.displayLine(this.getGreenBackgroud("校验库中的头标区与预期的值[" + expectHeader + "]一致，符合预期。"));
            }

        END1:

            return bRet;
        }

        private void Form_auto_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._needDeletePath == null || this._needDeletePath.Count == 0)
                return;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(this.mainForm.GetSupervisorAccount());
                byte[] baTimestamp = null;

                // 逐条删除记录
                foreach (string path in this._needDeletePath)
                {
                REDO:
                    WriteResResponse response = channel.WriteRes(path,
                       "",//strRanges,
                       0,//lTotalLength,
                       null,//baContent,
                       "",
                       "delete",//strStyle
                       baTimestamp);
                    // 间戳不匹配，自动重试
                    if (response.WriteResResult.ErrorCode == ErrorCode.TimestampMismatch)
                    {
                        // 设上时间戳
                        baTimestamp = response.baOutputTimestamp;
                        goto REDO;
                    }
                }

            }
            catch (Exception ex)
            {
                //throw new Exception(u.UserName + "异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        // 测试getrecord
        private void button_getrecord_Click(object sender, EventArgs e)
        {

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse r = null;

                // 先用超级管理员创建各种类型数据
                UserInfo s = this.mainForm.GetSupervisorAccount();
                string biblioPath = this.WriteXml(s, this.GetAppendPath(C_Type_biblio), this.GetXml(C_Type_biblio, true)).strOutputResPath;
                string itemPath = this.WriteXml(s, this.GetAppendPath(C_Type_item), this.GetXml(C_Type_item, true)).strOutputResPath;
                string orderPath = this.WriteXml(s, this.GetAppendPath(C_Type_order), this.GetXml(C_Type_order, true)).strOutputResPath;
                string issuePath = this.WriteXml(s, this.GetAppendPath(C_Type_issue), this.GetXml(C_Type_issue, true)).strOutputResPath;

                string arrivedPath = this.WriteXml(s, this.GetAppendPath(C_Type_arrived), this.GetXml(C_Type_arrived, true)).strOutputResPath;
                string amercePath = this.WriteXml(s, this.GetAppendPath(C_Type_amerce), this.GetXml(C_Type_amerce, true)).strOutputResPath;
                string commentPath = this.WriteXml(s, this.GetAppendPath(C_Type_comment), this.GetXml(C_Type_comment, true)).strOutputResPath;
                string readerPath = this.WriteXml(s, this.GetAppendPath(C_Type_reader), this.GetXml(C_Type_reader, true)).strOutputResPath;

                // 新建一个帐户，只有getrecord权限
                UserInfo u = NewUser("getrecord,getobject", "", "");
                this.DoRes1(u, C_Type_biblio, biblioPath, "get", true, false, "");
                this.DoRes1(u, C_Type_item, itemPath, "get", true, false, "");
                this.DoRes1(u, C_Type_order, orderPath, "get", true, false, "");
                this.DoRes1(u, C_Type_issue, issuePath, "get", true, false, "");

                this.DoRes1(u, C_Type_arrived, arrivedPath, "get", true, false, "");
                this.DoRes1(u, C_Type_amerce, amercePath, "get", true, false, "");
                this.DoRes1(u, C_Type_comment, commentPath, "get", true, false, "");
                this.DoRes1(u, C_Type_reader, readerPath, "get", true, false, "");

            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "getrecord权限测试-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // GetBrowseRecords 
        private void button_GetBrowseRecords_Click(object sender, EventArgs e)
        {
            // 新建一个帐户，只有getrecord权限
            UserInfo u = NewUser("getrecord", "", "");

            // 先用超级管理员创建各种类型数据
            UserInfo s = this.mainForm.GetSupervisorAccount();

            this.displayLine(this.getLarge("获取书目"));
            string biblioPath = this.WriteXml(s, this.GetAppendPath(C_Type_biblio), this.GetXml(C_Type_biblio, true)).strOutputResPath;
            string biblioXml = @":<unimarc:record xmlns:dprms='http://dp2003.com/dprms' xmlns:unimarc='http://dp2003.com/UNIMARC'><unimarc:leader>????????????????????????</unimarc:leader><unimarc:datafield tag='200' ind1='1' ind2=' '><unimarc:subfield code='a'>大家好</unimarc:subfield><unimarc:subfield code='e' /><unimarc:subfield code='f' /><unimarc:subfield code='g' /></unimarc:datafield></unimarc:record>";
            this.GetBrowseRecords(u, new string[] { biblioPath
                , biblioPath + biblioXml
                ,this.GetAppendPath(C_Type_biblio) + biblioXml});
            //,this.GetAppendPath(C_Type_biblio) });   //不能用仅?的形态

            this.displayLine(this.getLarge("获取册"));
            string itemPath = this.WriteXml(s, this.GetAppendPath(C_Type_item), this.GetXml(C_Type_item, true)).strOutputResPath;
            string itemXml = ":<root><parent>3</parent><location>流通库</location><price>CNY62.30</price><bookType>普通</bookType><accessNo>B123/L595</accessNo><batchNo>图书验收2022-1-26</batchNo><barcode>B003</barcode></root>";
            this.GetBrowseRecords(u, new string[] { itemPath
                , itemPath + itemXml
                ,this.GetAppendPath(C_Type_item) + itemXml});

            this.displayLine(this.getLarge("获取读者"));
            string readerPath = this.WriteXml(s, this.GetAppendPath(C_Type_reader), this.GetXml(C_Type_reader, true)).strOutputResPath;
            string readerXml = ":<root><barcode>P002</barcode><readerType>本科生</readerType><name>小王</name><department>一班</department></root>";
            this.GetBrowseRecords(u, new string[] { readerPath
                , readerPath + readerXml
                ,this.GetAppendPath(C_Type_reader) + readerXml});

            this.displayLine(this.getLarge("获取订购"));
            string orderPath = this.WriteXml(s, this.GetAppendPath(C_Type_order), this.GetXml(C_Type_order, true)).strOutputResPath;
            string orderXml = ":<root><parent>30</parent><index>1</index><catalogNo>123</catalogNo><seller>新华出版社</seller><source>本馆经费</source><range>20240101-20241231</range><issueCount>1</issueCount><copy>1</copy><price>CNY10</price><distribute>流通库:5</distribute><class>d</class><batchNo>2023-2-1</batchNo></root>";
            this.GetBrowseRecords(u, new string[] { orderPath
                , orderPath + orderXml
                ,this.GetAppendPath(C_Type_order) + orderXml});

            //===以下 期/预约到书/违约金/评注 不支持path:xml格式

            this.displayLine(this.getWarn1("以下 期/预约到书/违约金/评注 不支持path:xml格式"));

            this.displayLine(this.getLarge("获取期"));
            string issuePath = this.WriteXml(s, this.GetAppendPath(C_Type_issue), this.GetXml(C_Type_issue, true)).strOutputResPath;
            string issuerXml = "<root><parent>36</parent><publishTime>20240101</publishTime><issue>1</issue><orderInfo><root><parent>36</parent>\r\n<index>1</index><seller>邮局</seller><source>a</source><range>20230101-20231231</range><issueCount>12</issueCount><copy>1</copy><price>CNY20</price><distribute>阅览室:1</distribute><class>社科</class><batchNo>2023-1-26</batchNo></root></orderInfo></root>";
            this.GetBrowseRecords(u, new string[] { issuePath
                , issuePath + issuerXml
                ,this.GetAppendPath(C_Type_issue) + issuerXml});

            this.displayLine(this.getLarge("获取预约到书"));
            string arrivedPath = this.WriteXml(s, this.GetAppendPath(C_Type_arrived), this.GetXml(C_Type_arrived, true)).strOutputResPath;
            string arrivedXml = "<root><state>arrived</state><itemBarcode>B002</itemBarcode><onShelf>true</onShelf><readerBarcode>P001</readerBarcode><notifyDate>Thu, 23 Feb 2023 14:36:41 +0800</notifyDate><refID>4c267d45-dd1e-474f-8709-e9d5c084002d</refID><location>总馆图书馆</location><accessNo>I242.43/S495</accessNo></root>";
            this.GetBrowseRecords(u, new string[] { arrivedPath
                , arrivedPath + arrivedXml
                ,this.GetAppendPath(C_Type_arrived) + arrivedXml});

            this.displayLine(this.getLarge("获取违约金"));
            string amercePath = this.WriteXml(s, this.GetAppendPath(C_Type_amerce), this.GetXml(C_Type_amerce, true)).strOutputResPath;
            string amerceXml = "<root><itemBarcode>B002</itemBarcode><location>总馆图书馆</location><readerBarcode>P001</readerBarcode><state>amerced</state><id>637986612496654587-1</id><reason>超期。超 23天; 违约金因子: CNY1.0/day</reason><overduePeriod>23day</overduePeriod><price>CNY23</price><borrowDate>Thu, 21 Jul 2022 15:14:48 +0800</borrowDate><borrowPeriod>31day</borrowPeriod><borrowOperator>supervisor</borrowOperator><returnDate>Tue, 13 Sep 2022 10:20:49 +0800</returnDate><returnOperator>supervisor</returnOperator><operator>supervisor</operator><operTime>Tue, 13 Sep 2022 10:21:07 +0800</operTime></root>";
            this.GetBrowseRecords(u, new string[] { amercePath
                , amercePath + amerceXml
                ,this.GetAppendPath(C_Type_amerce) + amerceXml});

            this.displayLine(this.getLarge("获取评注"));
            string commentPath = this.WriteXml(s, this.GetAppendPath(C_Type_comment), this.GetXml(C_Type_comment, true)).strOutputResPath;
            string commentXml = "<root><parent>35</parent><title>评注标题test</title><content>一本好书62537</content></root>";
            this.GetBrowseRecords(u, new string[] { commentPath
                , commentPath + commentXml
                ,this.GetAppendPath(C_Type_comment) + commentXml});

        }

        // 测试CopyBiblioInfo函数
        private void button_CopyBiblioInfo_Click(object sender, EventArgs e)
        {

        }


    }
}
