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

        #region 通用函数

        public void NewUser(UserInfo u)
        {
            this.SetUser(u,"new");
        }

        public void DelUser(UserInfo u)
        {
            this.SetUser(u, "delete");
        }

        public void SetUser(UserInfo user, string strAction)
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
                            + "<br/>Access=" + u.Access);
                    }
                    else
                    {
                        this.displayLine("<br/>"+strAction + "帐户" + u.UserName);
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
          bool isReader=false)
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
                    +RestChannel.GetResultInfo(response));

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
          bool isReader=false)
        {
            WriteResResponse response = null;

            this.displayLine("strResPath="+ strResPath);
            this.displayLine("提交的xml<br/>"
                +HttpUtility.HtmlEncode(DomUtil.GetIndentXml(strXml))
                +"<br/>");


            byte[] baContent = Encoding.UTF8.GetBytes(strXml);  //文本到二进制
            long lTotalLength = baContent.Length;
            string strRanges = "0-" + (baContent.Length - 1).ToString();

            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password,isReader);

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

            this.displayLine("为读者"+readerBarcode+"借册"+itemBarcode);


            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password, isReader);

            REDO:
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
                    throw new Exception("superviosr修改读者密码出错:"+response.ChangeReaderPasswordResult.ErrorInfo);


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
            bool isReader=false)
        {
            GetResResponse response = null;

            this.displayLine("strResPath=" + strResPath);

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password,isReader);

                response = channel.GetRes(strResPath,
                    0,
                    -1,
                    "data,metadata,outputpath,timestamp");

                this.displayLine("GetRes()\r\n"
                    +HttpUtility.HtmlEncode( RestChannel.GetResultInfo(response)));
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
                string strDbType = "";
                if (type == C_Type_item)
                    strDbType = "item";
                else if (type == C_Type_order)
                    strDbType = "order";
                else if (type == C_Type_comment)
                    strDbType = "comment";
                else if (type == C_Type_issue)
                    strDbType = "issue";
            
                return this.SetItemInfo(u, strDbType, "delete", strResPath, "", isReader,out string temp);
            
            }
            else if (type == C_Type_Amerce
            || type == C_Type_Arrived)
            {
                return  this.WriteResForDel(u,strResPath,isReader).WriteResResult;

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
                response = channel.GetReaderInfo("@path:"+strResPath,"xml");


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
                response = channel.GetBiblioInfos(strResPath,formats);

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

        public LibraryServerResult SetItemInfo(UserInfo u,
            string strDbType,
    string strAction,
  string strResPath,
  string strXml,
  bool isReader,
  out string strOutputRecPath)
        {
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
                    out  strOutputRecPath,
                    out byte[] baOutputTimestamp) ;
                // 间戳不匹配，自动重试
                if (response.ErrorCode == ErrorCode.TimestampMismatch)
                {
                    // 设上时间戳
                    baTimestamp = baOutputTimestamp;
                    goto REDO;
                }

                this.displayLine("SetItemInfo()\r\n"
                    + HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)+"\r\n"
                    + "strOutputRecPath:" + strOutputRecPath+"\r\n"
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

            string strItemDbType = "";
            if (type == C_Type_item)
                strItemDbType = "item";
            else if (type == C_Type_issue)
                strItemDbType = "issue";
            else if (type== C_Type_comment)
                strItemDbType = "comment";
            else if (type == C_Type_order)
                strItemDbType= "order";
            else
                throw new Exception("不支持的type["+type+"]");

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
                else if (type==C_Type_order)
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
                else if (type==C_Type_issue)
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
                else if (type==C_Type_comment)
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
                    throw new Exception("不支持的类型"+type);
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

        // 存取定义与字段
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
                UserInfo u = new UserInfo
                {
                    UserName = "u1",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=change|getbiblioinfo=*"
                };
                //创建帐号
                this.NewUser(u);

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
                // 用u_a1_1帐户
                u = new UserInfo
                {
                    UserName = "u2",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=change(606)|getbiblioinfo=*"
                };
                //创建帐号
                this.NewUser(u);

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
                // 用u_a1_1帐户
                u = new UserInfo
                {
                    UserName = "u3",
                    Password = "1",
                    SetPassword = true,

                    //new,应不能change对象
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=new|getbiblioinfo=*"
                };
                //创建帐号
                this.NewUser(u);

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
                u = new UserInfo
                {
                    UserName = "u4",
                    Password = "1",
                    SetPassword = true,

                    //delete,应不能change对象
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=delete|getbiblioinfo=*"
                };
                //创建帐号
                this.NewUser(u);

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
                u = new UserInfo
                {
                    UserName = "u5",
                    Password = "1",
                    SetPassword = true,

                    //通配符*表示new,change,and，应能change对象
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=*|getbiblioinfo=*"
                };
                //创建帐号
                this.NewUser(u);

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

        public const string C_Type_reader= "读者";
        public const string C_Type_biblio = "书目";

        public const string C_Type_item = "册";
        public const string C_Type_order = "订购";
        public const string C_Type_comment = "评注";
        public const string C_Type_issue = "期";

        public const string C_Type_Amerce = "违约金";
        public const string C_Type_Arrived = "预约到书";

        public string GetAppendPath(string type)
        {
            if (type == C_Type_reader)
                return "读者/?";
            else if (type == C_Type_biblio)
                return "中文图书/?";

            else if (type == C_Type_item)
                return "中文图书实体/?";
            else if (type == C_Type_order)
                return "中文图书订购/?";
            else if (type == C_Type_comment)
                return "中文图书评注/?";

            else if (type == C_Type_issue)
                return "中文期刊期/?";

            else if (type == C_Type_Amerce)
                return "违约金/?";
            else if (type == C_Type_Arrived)
                return "预约到书/?";


            throw new Exception("不支持的类型"+type);
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
            string xml =this.GetXml(type, hasFile,out string barcode);
            return xml;
        }

            // barcode，当读者 和 册时有意义
         public string GetXml(string type, bool hasFile,out string barcode)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);
            barcode = temp.ToString().PadLeft(6, '0'); //Guid.NewGuid().ToString().ToUpper(); //

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='" + temp.ToString() + "'/>";

            // 得到parent
            string parent = "";
            if (type == C_Type_item
                || type == C_Type_order
                || type == C_Type_comment)
            {
                parent = this.GetBiblioParent();
            }
            else if (type == C_Type_issue)
            {
                parent = this.GetIssueParent();
            }


            // 根据不同类型，准备xml
            if (type == C_Type_reader)
            {
                barcode = "P" + barcode;
                return "<root>"
                    + "<barcode>" + barcode + "</barcode>"
                    + "<name>小张" + barcode + "</name>"
                    + "<readerType>本科生</readerType>"
                    + "<displayName>显示名"+barcode + "</displayName>"
                    + "<preference>个性化参数" + barcode + "</preference>"
                    + dprmsfile
                    + "</root>";
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
                barcode = "B" + barcode;
                return "<root>"
                    + "<parent>" + parent + "</parent>"
                    + "<barcode>" + barcode + "</barcode>"
                    + "<location>"+this._location+"</location>"
                    + "<bookType>普通</bookType>"
                    + dprmsfile
                    + "</root>";
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
                              <title>评注标题"+temp+@"</title>
                              <content>一本好书"+temp+@"</content>"
                              + dprmsfile
                            + "</root>";

                /*
<root>
  
  <content>asdasfdfd</content>
  <parent>4</parent>
</root>
                 */

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
            else if (type == C_Type_Amerce)
            {
                return this.GetAmerceXml("P001", "B001", hasFile);
            }
            else if (type == C_Type_Arrived)
            {
                return this.GetArrivedXml("P001", "B001",hasFile);  //这个读者和册也可能是不存在的，没有关系。

            }

            throw new Exception("GetXml不支持的数据类型" + type);
        }

        public string GetArrivedXml(string readerBarcode, string itemBarcode,bool hasFile)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='" + temp.ToString() + "'/>";

            return @"<root>
                        <state>arrived</state>
                        <itemBarcode>" +itemBarcode+@"</itemBarcode>
                        <box>#reservation</box>
                        <readerBarcode>"+readerBarcode+@"</readerBarcode>
                        <notifyDate>Thu, 23 Feb 2023 14:36:41 +0800</notifyDate>
                        <refID>4c267d45-dd1e-474f-8709-e9d5c084002d</refID>
                        <location>流通库</location>
                        <accessNo>I242.43/S495</accessNo>"
                        +dprmsfile
                    +"</root>";
        }


        // 创建违约金记录
        public string GetAmerceXml(string readerBarcode, string itemBarcode, bool hasFile)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='" + temp.ToString() + "'/>";

            return @"<root>
                            <itemBarcode>"+itemBarcode+@"</itemBarcode>
                            <location>流通库</location>
                            <readerBarcode>"+readerBarcode+@"</readerBarcode>
                            <libraryCode>
                            </libraryCode>
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

        public string CreateBiblioBySupervisor(bool bIssue,bool hasFile)
        {
            string info = "";
            // 用supervisor帐户创建一条书目
            string strResPath = "中文图书/?";
            if (bIssue == true)
            {
                strResPath = "中文期刊/?";
                info = "期刊";
            }

            this.displayLine("用supervisor创建一条"+info+"书目，为相关测试提供支持。");

            WriteResResponse response = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                strResPath,
                 this.GetXml(C_Type_biblio, hasFile));

            if (response.WriteResResult.Value == -1)
                throw new Exception("用supervisor创建书目异常:" + response.WriteResResult.ErrorInfo);


            return response.strOutputResPath;
        }

        public string GetBiblioParent()
        {
            string biblioPath = GetBiblioPath();
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



        public void checkRight(string type,
            List<string> rightsList)
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

                this.displayLine(getLarge(type+"及下级对象所需权限的测试结果"));


                string strResPath = GetAppendPath(type); //"读者/?";
                WriteResResponse response = null;

                // 第4组权限会写好xml路径和对象路径，供后面权限使用。
                string path_xmlHasFile = "";
                string path_object = "";

                #region 第1组测试  仅有setXXXinfo

                //===
                // 第1组测试  仅有setXXXinfo
                this.displayLine(getLarge("第1组测试"));
                UserInfo u = new UserInfo
                {
                    UserName = "u1",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[0], //"setreaderinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);



                this.displayLine(GetBR() + getBold(u.UserName + "写简单xml，由于没有连带的读xml权限，应不成功。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type,false));
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
                u = new UserInfo
                {
                    UserName = "u2",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[1], //"setreaderinfo,getreaderinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                // 新建简单xml
                this.displayLine(GetBR() + getBold(u.UserName + "新建简单xml，有读写xml权限，应新建成功。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type,false));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                string tempPath = response.strOutputResPath;

                // 修改简单xml
                this.displayLine(GetBR() + getBold(u.UserName + "修改简单xml，应修改成功，且错误码应为NoError。"));
                response = this.WriteXml(u, tempPath, this.GetXml(type,false));
                if (response.WriteResResult.Value == 0)
                {
                    if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                    {
                        this.displayLine(getRed("不符合预期，错误码不对，应为NoError。"));
                    }
                    else
                    {
                        this.displayLine("符合预期");
                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除简单xml
                this.displayLine(GetBR() + getBold(u.UserName + "删除简单xml，应删除成功。"));
                LibraryServerResult res = this.DelXml(u,type, tempPath,false);
                if (res.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));



                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "新建带dprms:file的xml，由于没有对象权限，应写入失败 或者 部分写入且过滤了file(注意观察提示)。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type,true));
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
                u = new UserInfo
                {
                    UserName = "u3",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[2],//"setreaderinfo,getreaderinfo,writereaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                // 无法写简单xml，因为写权限大于读者权限。
                this.displayLine(GetBR() + getBold(u.UserName + "新建简单xml，应报权限配置不合理，因为有写对象权限，缺读对象权限。"));
                response = this.WriteXml(u, strResPath, this.GetXml(type,false));
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
                u = new UserInfo
                {
                    UserName = "u4",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[3],//"setreaderinfo,getreaderinfo,writereaderobject,getreaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

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
                response = this.WriteXml(u, strResPath, this.GetXml(type,true));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 下面针对上面新建的这条进行修改和删除
                tempPath = response.strOutputResPath;

                // 修改带file的xml
                this.displayLine(GetBR() + getBold(u.UserName + "修改带dprms:file的xml，应修改成功且NoError。"));
                response = this.WriteXml(u, tempPath, this.GetXml(type,true));
                if (response.WriteResResult.Value == 0)
                {
                    if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                        this.displayLine(getRed("不符合预期，错误码不对，应为NoError。"));
                    else
                        this.displayLine("符合预期");
                }
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除带dprms:file的xml              
                this.displayLine(GetBR() + getBold(u.UserName + "删除带dprms:file的xml，应删除成功。"));
                res = this.DelXml(u,type, tempPath,false);
                if (res.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 修改xml中的dprms:file,包括对dprms:file的new/change/delete

                // 再新建一条带dprms:file的记录，以便对其下级的dprms:file进行增/删/改
                this.displayLine(GetBR() + getBold(u.UserName + "再新建一条带dprms:file的记录，以便对其下级的dprms:file进行增/删/改。"));

                string originXml = this.GetXml(type,true);
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
                    if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                        this.displayLine(getRed("注意错误码不对，应为NoError。"));

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
                            this.displayLine(this.getRed("xml中有"+fileNodes.Count+"个dprms:file元素，不符合预期，应有3个。"));
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
                    if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                        this.displayLine(getRed("注意错误码不对，应为NoError。"));

                    // 获取出来看下
                    GetResResponse tempResponse = this.GetRes(u, tempPath);
                    if (tempResponse.GetResResult.Value >= 0)
                    {

                        string tempXml = Encoding.UTF8.GetString(tempResponse.baContent);
                        XmlNodeList fileNodes = this.GetFileNodes(tempXml);
                        if (fileNodes.Count >= 1)
                        {
                            XmlNode node = fileNodes[0];
                           string a= DomUtil.GetAttr(node, "a");
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
                    if (response.WriteResResult.ErrorCode != ErrorCode.NoError)
                        this.displayLine(getRed("注意错误码不对，应为NoError。"));

                    // 获取出来看下
                    GetResResponse tempResponse = this.GetRes(u, tempPath);
                    if (tempResponse.GetResResult.Value >= 0)
                    {
                        string tempXml = Encoding.UTF8.GetString(tempResponse.baContent);
                        XmlNodeList fileNodes = this.GetFileNodes(tempXml);
                        if (fileNodes.Count == 2)
                        {
                            string id0=DomUtil.GetAttr(fileNodes[0], "id");
                            string id1=DomUtil.GetAttr(fileNodes[1], "id");
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
                u = new UserInfo
                {
                    UserName = "u5",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[4],//"writereaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

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
                u = new UserInfo
                {
                    UserName = "u6",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[5],//"writereaderobject, getreaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                this.displayLine(GetBR() + getBold(u.UserName + "修改对象数据，因为没有连带的读者xml权限，应不成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 修改xml及dprms:file,应不能成功。
                this.displayLine(GetBR() + getBold(u.UserName + "修改xml及dprms:file，由于没有读写xml的权限，应不成功。"));
                response = this.WriteXml(u, path_xmlHasFile, this.GetXml(type,true));
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
                u = new UserInfo
                {
                    UserName = "u7",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[6],//"getreaderinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

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
                u = new UserInfo
                {
                    UserName = "u8",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[7],//"getreaderinfo,getreaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

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
                u = new UserInfo
                {
                    UserName = "u9",
                    Password = "1",
                    SetPassword = true,
                    Rights = rightsList[8],//"getreaderobject",
                    Access = ""
                };

                //创建帐号
                this.NewUser(u);

                //不能获取对象
                this.displayLine(GetBR() + getBold(u.UserName+"获取对象，应不成功，由于对象权限不能独立存在。"));
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

                MessageBox.Show(this, ex.Message);
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




            private void button_testRight_Click(object sender, EventArgs e)
        {
            string type = this.comboBox_TestRight_type.Text.Trim();

            //if (type ==C_Type_Amerce 
            //    || type == C_Type_Arrived)
            //{
            //    MessageBox.Show(this, "自动测试尚不支持此类型" + type);
            //    return;
            //}

            List<string> rightsList = new List<string>();

            if (type == C_Type_reader)
            {
                rightsList.Add("setreaderinfo");
                rightsList.Add("setreaderinfo,getreaderinfo");//有用
                rightsList.Add("setreaderinfo,getreaderinfo,writereaderobject");
                rightsList.Add("setreaderinfo,getreaderinfo,writereaderobject,getreaderobject");//有用

                rightsList.Add("writereaderobject");
                rightsList.Add("writereaderobject, getreaderobject");

                rightsList.Add("getreaderinfo");//有用
                rightsList.Add("getreaderinfo,getreaderobject");//有用
                rightsList.Add("getreaderobject");
            }
            else if (type == C_Type_biblio)
            {
                rightsList.Add("setbiblioinfo");
                rightsList.Add("setbiblioinfo,getbiblioinfo");//有用
                rightsList.Add("setbiblioinfo,getbiblioinfo,writebiblioobject");
                rightsList.Add("setbiblioinfo,getbiblioinfo,writebiblioobject,getbiblioobject");//有用

                rightsList.Add("writebiblioobject");
                rightsList.Add("writebiblioobject, getbiblioobject");

                rightsList.Add("getbiblioinfo");//有用
                rightsList.Add("getbiblioinfo,getbiblioobject");//有用
                rightsList.Add("getbiblioobject");
            }
            else if (type == C_Type_item)
            {
                rightsList.Add("setiteminfo");
                rightsList.Add("setiteminfo,getiteminfo");//有用
                rightsList.Add("setiteminfo,getiteminfo,writeitemobject");
                rightsList.Add("setiteminfo,getiteminfo,writeitemobject,getitemobject");//有用

                rightsList.Add("writeitemobject");
                rightsList.Add("writeitemobject, getitemobject");

                rightsList.Add("getiteminfo");//有用
                rightsList.Add("getiteminfo,getitemobject");//有用
                rightsList.Add("getitemobject");
            }
            else if (type == C_Type_order)
            {
                rightsList.Add("setorderinfo");
                rightsList.Add("setorderinfo,getorderinfo");//有用
                rightsList.Add("setorderinfo,getorderinfo,writeorderobject");
                rightsList.Add("setorderinfo,getorderinfo,writeorderobject,getorderobject");//有用

                rightsList.Add("writeorderobject");
                rightsList.Add("writeorderobject, getorderobject");

                rightsList.Add("getorderinfo");//有用
                rightsList.Add("getorderinfo,getorderobject");//有用
                rightsList.Add("getorderobject");
            }
            else if (type == C_Type_comment)
            {
                rightsList.Add("setcommentinfo");
                rightsList.Add("setcommentinfo,getcommentinfo");//有用
                rightsList.Add("setcommentinfo,getcommentinfo,writecommentobject");
                rightsList.Add("setcommentinfo,getcommentinfo,writecommentobject,getcommentobject");//有用

                rightsList.Add("writecommentobject");
                rightsList.Add("writecommentobject, getcommentobject");

                rightsList.Add("getcommentinfo");//有用
                rightsList.Add("getcommentinfo,getcommentobject");//有用
                rightsList.Add("getcommentobject");
            }
            else if (type == C_Type_issue)
            {
                rightsList.Add("setissueinfo");
                rightsList.Add("setissueinfo,getissueinfo");//有用
                rightsList.Add("setissueinfo,getissueinfo,writeissueobject");
                rightsList.Add("setissueinfo,getissueinfo,writeissueobject,getissueobject");//有用

                rightsList.Add("writeissueobject");
                rightsList.Add("writeissueobject, getissueobject");

                rightsList.Add("getissueinfo");//有用
                rightsList.Add("getissueinfo,getissueobject");//有用
                rightsList.Add("getissueobject");
            }
            else if (type == C_Type_Amerce)
            {
                rightsList.Add("setamerceinfo");
                rightsList.Add("setamerceinfo,getamerceinfo");//有用
                rightsList.Add("setamerceinfo,getamerceinfo,writeamerceobject");
                rightsList.Add("setamerceinfo,getamerceinfo,writeamerceobject,getamerceobject");//有用

                rightsList.Add("writeamerceobject");
                rightsList.Add("writeamerceobject, getamerceobject");

                rightsList.Add("getamerceinfo");//有用
                rightsList.Add("getamerceinfo,getamerceobject");//有用
                rightsList.Add("getamerceobject");
            }
            else if (type == C_Type_Arrived)
            {
                rightsList.Add("setarrivedinfo");
                rightsList.Add("setarrivedinfo,getarrivedinfo");//有用
                rightsList.Add("setarrivedinfo,getarrivedinfo,writearrivedobject");
                rightsList.Add("setarrivedinfo,getarrivedinfo,writearrivedobject,getarrivedobject");//有用

                rightsList.Add("writearrivedobject");
                rightsList.Add("writearrivedobject, getarrivedobject");

                rightsList.Add("getarrivedinfo");//有用
                rightsList.Add("getarrivedinfo,getarrivedobject");//有用
                rightsList.Add("getarrivedobject");
            }



            // 调检查权限函数
            this.checkRight(type,
                rightsList);
        }

        // 个人书斋，册的馆藏地
        public string _location = "流通库";

        public WriteResResponse CreateReaderBySuperviosr(string rights,
            string personalLibrary,
            out string readerBarcode)
        {
            string readerxml = this.GetXml(C_Type_reader, true, out readerBarcode);

            if (string.IsNullOrEmpty(rights) == false
                || string.IsNullOrEmpty(personalLibrary) ==false)
            {
                // 修改一下权限
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(readerxml);
                XmlNode root = dom.DocumentElement;
                
                if (string.IsNullOrEmpty(rights)==false)
                    root.InnerXml += "<rights>" + rights + "</rights>";

                if (string.IsNullOrEmpty(personalLibrary) == false)
                    root.InnerXml += "<personalLibrary>" + personalLibrary + "</personalLibrary>";

                readerxml = dom.OuterXml;
            }

            this.displayLine(this.getBold("用supervisor创建读者" + readerBarcode + "，为相关操作提供支持。"));


            string strReaderPath = this.GetAppendPath(C_Type_reader);
            WriteResResponse response = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                strReaderPath,
                 readerxml);

            if (response.WriteResResult.Value == -1)
                throw new Exception("用supervisor创建读者异常:" + response.WriteResResult.ErrorInfo);

            return response;
        }

        private void button_readerLogin_Click(object sender, EventArgs e)
        {
            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;
                GetResResponse getRes = null;

                this.displayLine(this.getLarge("准备环境"));

                // 用supervisor帐户创建一个读者
                string readerBarcode = "";
                writeRes = this.CreateReaderBySuperviosr("setreaderinfo,getreaderinfo,writereaderobject,getreaderobject",
                    "",
                    out readerBarcode);
                // 读者自己的路径
                string ownerReaderPath = writeRes.strOutputResPath;
                string ownerObject = ownerReaderPath + "/object/0";


                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                this.ChangeReaderPasswordBySupervisor(readerBarcode);
                UserInfo u = new UserInfo
                {
                    UserName = readerBarcode,
                    Password = "1",
                };

                // 用superviosr帐户创建另一条读者，用于作为他人记录。
                writeRes = this.CreateReaderBySuperviosr("","", out string tempBarcode);
                string otherReaderPath = writeRes.strOutputResPath;
                string otherObject = otherReaderPath + "/object/0";


                #region 第一组测试：新建读者-WriteRes/SetReaderInfo

                this.displayLine(this.getLarge("第一组测试：新建读者-WriteRes/SetReaderInfo"));

                //====
                // 读者身份 创建读者记录

                // 用WriteRes创建读者记录，应不成功
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes()新建读者xml，应不成功。注意观察提示。"));
                writeRes = this.WriteXml(u,
                    this.GetAppendPath(C_Type_reader),
                    this.GetXml(C_Type_reader, false), true);

                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用SetReaderInfo新建读者,应不成功
                this.displayLine(GetBR() + getBold(u.UserName + "用SetReaderInfo新建读者xml，应不成功，注意观察提示。"));
                SetReaderInfoResponse readerRes = this.SetReaderInfo(u, "new",
                    this.GetAppendPath(C_Type_reader),
                    this.GetXml(C_Type_reader, false), true);
                if (readerRes.SetReaderInfoResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第二组测试：第二组测试：修改他人记录-WriteRes/SetReaderInfo

                this.displayLine(this.getLarge("第二组测试：修改他人记录-WriteRes/SetReaderInfo"));

                // 用WriteRes修改其它读者记录xml，应不成功
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改其它读者xml，应不成功。注意观察提示。"));
                writeRes = this.WriteXml(u,
                   otherReaderPath,
                    this.GetXml(C_Type_reader, false), true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用SetReaderInfo修改其它读者
                this.displayLine(GetBR() + getBold(u.UserName + "用SetReaderInfo修改其它读者xml，应成功。"));
                readerRes = this.SetReaderInfo(u, "change", otherReaderPath, this.GetXml(C_Type_reader, false), true);
                if (readerRes.SetReaderInfoResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                #endregion

                #region 第三组测试：修改他人对象-WriteRes

                this.displayLine(this.getLarge("第三组测试：修改他人对象-WriteRes"));

                // 用WriteRes修改其它读者的对象，应不成功
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改其它读者的对象，应不成功。注意观察提示。"));
                writeRes = this.WriteObject(u,
                   otherObject,
                   true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第四组测试：删除他人记录-SetReaderInfo
                this.displayLine(this.getLarge("第四组测试：删除他人记录-SetReaderInfo"));

                // 用SetReaderInfo删除书目
                this.displayLine(GetBR() + getBold(u.UserName + "用SetReaderInfo删除其它读者xml，应成功。"));
                readerRes = this.SetReaderInfo(u, "delete", otherReaderPath, "", true);
                if (readerRes.SetReaderInfoResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第五组测试：修改自己的xml-SetReaderInfo
                //读者帐户登录，只能修改自己记录的 displayName 和 preference 这两个字段。

                this.displayLine(this.getLarge("第五组测试：修改自己的xml-SetReaderInfo"));

                //===
                // 用WriteRes修改自己记录，应部分成功
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改读者自己的，应部分成功。注意观察提示。"));
                // 提交的xml
                string submitXml = this.GetXml(C_Type_reader, true);

                writeRes = this.WriteXml(u,
                   ownerReaderPath,
                    this.GetXml(C_Type_reader, true), true);
                if (writeRes.WriteResResult.Value == 0)
                {
                    //this.displayLine("符合预期");

                    // 需要把读者记录获取出来，比如
                    getRes = this.GetRes(u, ownerReaderPath, true);
                    if (getRes.GetResResult.Value >= 0)
                    {

                        string resultXml = Encoding.UTF8.GetString(getRes.baContent);

                        // 比较提交的xml与返回的xml
                        bool bResult = this.CompareReader(submitXml, resultXml,
                            new List<string> { "displayName", "preference", "dprms:file" },  // 希望相等的字段
                            new List<string> { "name"}, // 希望不等的字段
                            out string info);

                        this.displayLine(info);
                        if (bResult == false)
                        {
                            this.displayLine(getRed("不符合预期"));
                        }
                        else
                        {
                            this.displayLine("符合预期");
                        }
                    }
                    else
                    {
                        this.displayLine(this.getRed("读者获取自己的记录返回-1，不符合预期。"));
                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));



                // 用writeres来测即可以，不用再测一遍了
                //// 用SetReaderInfo修改读者
                //this.displayLine(GetBR() + getBold(u.UserName + "用SetReaderInfo修改读者自己，应成功。"));
                //readerRes = this.SetReaderInfo(u, "change", readerOwnerPath, this.GetXml(C_Type_reader, false), true);
                ////if (readerRes.SetReaderInfoResult.Value == 0)
                ////    this.displayLine("符合预期");
                ////else
                ////    this.displayLine(getRed("不符合预期"));
                //this.displayLine(this.getWarn1("请人工判断。"));

                #endregion

                #region 第六组测试：修改自己的对象-WriteRes
                //读者帐户登录，只能修改自己记录的 displayName 和 preference 这两个字段。

                this.displayLine(this.getLarge("第六组测试：修改自己的对象-WriteRes"));

                // 用WriteRes修改自己的对象，应不成功
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改自己的对象，应成功。"));
                writeRes = this.WriteObject(u,
                   ownerObject,
                   true);
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第七组测试：读者不能删除自己记录


                this.displayLine(this.getLarge("第七组测试：读者不能删除自己记录"));

                // 用SetReaderInfo删除书目
                this.displayLine(GetBR() + getBold(u.UserName + "用SetReaderInfo删除读者自己，应不成功。"));
                readerRes = this.SetReaderInfo(u, "delete", ownerReaderPath, "", true);
                if (readerRes.SetReaderInfoResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                #endregion

                #region 第八组测试：读者可以获取自己的xml

                this.displayLine(this.getLarge("第八组测试：读者可以获取自己的xml和对象"));

                // 获取xml
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes获取读者自己的xml，应成功。"));

                getRes = this.GetRes(u, ownerReaderPath, true);
                if (getRes.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                // 获取对象
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes获取读者自己的对象，应成功。"));


                getRes = this.GetRes(u, ownerObject, true);
                if (getRes.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第九组测试：读者获取他人的xml和对象

                this.displayLine(this.getLarge("第九组测试：读者获取他人的xml和对象"));

                // 用GetRes获取他人xml
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes获取他人的xml，应不成功。"));

                getRes = this.GetRes(u, otherReaderPath, true);
                if (getRes.GetResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                // 用GetReaderInfo获取他人xml
                this.displayLine(GetBR() + getBold(u.UserName + "用GetReaderInfo获取他人的xml，应不成功。"));
                GetReaderInfoResponse response1 = this.GetReaderInfo(u, otherReaderPath, true);
                if (response1.GetReaderInfoResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));




                // 获取他人对象
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes获取他人的对象，应不成功。"));
                getRes = this.GetRes(u, otherObject, true);
                if (getRes.GetResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion


                // 用superviosr删除这次删除相关的记录
                // todo
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
                string text2= resultRoot.SelectSingleNode(f, nsmgr).InnerText;//DomUtil.GetElementText(resultRoot, f);

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


        // 读者身份 操作书目
        private void button_readerLogin_biblio_Click(object sender, EventArgs e)
        {

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;

                // 用supervisor帐户创建一个读者
                string readerBarcode = "";
                writeRes = this.CreateReaderBySuperviosr("setbiblioinfo,getbiblioinfo,writebiblioobject,getbiblioobject",
                    "",
                    out readerBarcode);
                string readerOwnerPath = writeRes.strOutputResPath;

                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                this.ChangeReaderPasswordBySupervisor(readerBarcode);
                UserInfo u = new UserInfo
                {
                    UserName = readerBarcode,
                    Password = "1",
                };

                // 这个路径有意义，不会被删除，下面的临时记录会被删除
                string biblioPath = "";
                string objectPath = "";

                // 用于删除的路径
                string tempBiblioPath1 = "";

                #region 第一组测试：新建书目WriteRes/SetBiblioInfo

                this.displayLine(this.getLarge("第一组测试：新建书目-WriteRes/SetBiblioInfo"));

                string newPath = this.GetAppendPath(C_Type_biblio);
                // 用WriteRes新建书目
                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 新建书目xml，应成功。"));
                writeRes = this.WriteXml(u, newPath, this.GetXml(C_Type_biblio, true), true);
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 第一条记录作为有意义的记录，不会用来测删除
                biblioPath = writeRes.strOutputResPath;
                objectPath = biblioPath + "/object/0";


                // 用SetBiblioInfo新建书目
                this.displayLine(GetBR() + getBold(u.UserName + "用 SetBiblioInfo() 新建书目xml，应成功。"));
                SetBiblioInfoResponse res = this.SetBiblioInfo(u, "new", newPath, this.GetXml(C_Type_biblio, true), true);
                if (res.SetBiblioInfoResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用于删除
                tempBiblioPath1 = res.strOutputBiblioRecPath;

                #endregion

                #region 第二组测试：修改书目-SetBiblioInfo

                this.displayLine(this.getLarge("第二组测试：修改书目-SetBiblioInfo"));

                // 用SetBiblioInfo修改书目
                this.displayLine(GetBR() + getBold(u.UserName + "用SetBiblioInfo()修改书目xml，应成功。"));
                res = this.SetBiblioInfo(u, "change", biblioPath, this.GetXml(C_Type_biblio, true), true);
                if (res.SetBiblioInfoResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第三组测试，修改书目下的对象数据

                this.displayLine(this.getLarge("第三组测试，修改书目下的对象数据"));

                this.displayLine(GetBR() + getBold(u.UserName + "WriteRes()修改书目下对象数据，应成功。"));
                writeRes = this.WriteObject(u, objectPath, true);
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第四组测试：删除书目-SetBiblioInfo

                this.displayLine(this.getLarge("第四组测试：删除书目-SetBiblioInfo"));

                // 用SetBiblioInfo删除书目
                this.displayLine(GetBR() + getBold(u.UserName + "用SetBiblioInfo()删除书目xml，应成功。"));
                res = this.SetBiblioInfo(u, "delete", tempBiblioPath1, "", true);
                if (res.SetBiblioInfoResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion






                #region 第五组测试：获取书目-GetRes/GetBiblioInfos

                this.displayLine(this.getLarge("第五组测试：获取书目-GetRes/GetBiblioInfos"));

                // 用GetRes获取书目
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetRes()获取书目xml，应成功。"));
                GetResResponse getResponse = this.GetRes(u, biblioPath, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetBiblioInfos()获取书目
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetBiblioInfos()获取书目xml，应成功。"));
                GetBiblioInfosResponse getBiblioResponse = this.GetBiblioInfos(u, biblioPath, true);
                if (getBiblioResponse.GetBiblioInfosResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第六组测试：获取书目下对象

                this.displayLine(this.getLarge("第六组测试：获取书目下对象"));

                // 用GetRes获取书目下对象
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetRes()获取书目下对象，应成功。"));
                getResponse = this.GetRes(u, objectPath, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                #endregion

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

        private void button_readerLogin_item_Click(object sender, EventArgs e)
        {
            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;

                // 用supervisor帐户创建一个读者，有完整册及对象权限
                string readerBarcode = "";
                writeRes = this.CreateReaderBySuperviosr("setiteminfo,getiteminfo,writeitemobject,getitemobject",
                    "",
                    out readerBarcode);
                string readerOwnerPath = writeRes.strOutputResPath;

                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                this.ChangeReaderPasswordBySupervisor(readerBarcode);
                UserInfo u = new UserInfo
                {
                    UserName = readerBarcode,
                    Password = "1",
                };

                // 这个路径有意义，用来修改xml和对象，不会被删除
                string newPath = this.GetAppendPath(C_Type_item);
                string itemPath = "";
                string objectPath = "";

                // 由于读者身份不一定能创建册，所以先用管理员身份创建一条册为后面使用
               writeRes=  this.WriteXml(this.mainForm.GetSupervisorAccount(),
                    newPath,
                    this.GetXml(C_Type_item, true));
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建册记录异常："+writeRes.WriteResResult.ErrorInfo);

                itemPath = writeRes.strOutputResPath;
                objectPath = itemPath + "/object/0";



                #region 第一组测试：新建册-WriteRes

                this.displayLine(this.getLarge("第一组测试：用WriteRes新建册xml"));


                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 新建册xml，预期失败。"));
                writeRes = this.WriteXml(u, newPath, this.GetXml(C_Type_item, true), true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者无个人书斋权限，应不能新建册。"));


                #endregion

                #region 第二组测试：修改册

                this.displayLine(this.getLarge("第二组测试：用WriteRes修改册xml"));

                // 用SetBiblioInfo修改书目
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改册xml，预期失败。"));
                writeRes = this.WriteXml(u,itemPath, this.GetXml(C_Type_biblio, true), true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者无个人书斋权限，应不能修改册。"));

                #endregion

                #region 第三组测试，修改册下的对象数据

                this.displayLine(this.getLarge("第三组测试，修改册下的对象数据。"));

                this.displayLine(GetBR() + getBold(u.UserName + "WriteRes()修改册下对象数据，预期失败。"));
                writeRes = this.WriteObject(u, objectPath, true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，如果册不能操作(例如由于缺个人书斋权限等)，那对象也应不能操作。"));

                #endregion

                #region 第四组测试：删除册

                this.displayLine(this.getLarge("第四组测试：删除册，预期失败。"));

               LibraryServerResult r=  this.DelXml(u,C_Type_item,itemPath,true);
                if (r.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者无个人书斋权限，应不能删除册。"));


                #endregion






                #region 第五组测试：获取册-GetRes/GetItemInfo

                this.displayLine(this.getLarge("第五组测试：获取册"));

                // 用GetRes获取册
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes()获取册xml，应成功。"));
                GetResResponse getResponse = this.GetRes(u, itemPath, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetItemInfo()获取册
                this.displayLine(GetBR() + getBold(u.UserName + "用GetItemInfo()获取册xml，应成功。"));
                GetItemInfoResponse getItemResponse = this.GetItemInfo(u,C_Type_item, itemPath,true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第六组测试：获取册下对象

                this.displayLine(this.getLarge("第六组测试：获取册下对象"));

                // 用GetRes获取书目下对象
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes()获取册下对象，应成功。"));
                getResponse = this.GetRes(u, objectPath, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                #endregion



                // 后面是给读者配了个人书斋的情况下，能操作属于自己书斋的册，不能操作不属于自己书斋的册。

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

        // 得到指定馆藏地的册xml
        public string GetItemXml(string location)
        {
            string xml = this.GetXml(C_Type_item, true);
            if (string.IsNullOrEmpty(location) == false)
            {

                // 修改一下权限
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(xml);
                XmlNode root = dom.DocumentElement;

                DomUtil.SetElementText(root, "location", location);

                xml = dom.OuterXml;
            }

            return xml;
        }

        private void button_readerLogin_item2_Click(object sender, EventArgs e)
        {
            /*
            管理员创建两条册
            一册的馆藏地与读者个人书斋相同的，读者可以new/change/delete册记录及对象
            一册的馆藏地与读者个人书斋不同，读者不能new/change/delete册记录及对象
             */

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;

                // 用supervisor帐户创建一个读者，有完整册及对象权限，有个人书斋
                string readerBarcode = "";
                writeRes = this.CreateReaderBySuperviosr("setiteminfo,getiteminfo,writeitemobject,getitemobject",
                    _location,  //个人书斋
                    out readerBarcode);
                string readerOwnerPath = writeRes.strOutputResPath;

                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                this.ChangeReaderPasswordBySupervisor(readerBarcode);
                UserInfo u = new UserInfo
                {
                    UserName = readerBarcode,
                    Password = "1",
                };

                // 这个路径有意义，用来修改xml和对象，不会被删除
                string newPath = this.GetAppendPath(C_Type_item);

                // 用管理员身份创建两条册，一册的馆藏地与读者个人书斋相同，一册不同。为后面使用。
                this.displayLine(this.getBold("用管理员身份创建两条册，一册的馆藏地与读者个人书斋相同，一册不同。为后面使用。"));

                // 创建馆藏地与个人书斋相同的册
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     this.GetXml(C_Type_item,true)); //册的馆藏地改为与读者个人书斋不同。
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建册记录异常：" + writeRes.WriteResResult.ErrorInfo);

                string itemPathSame = writeRes.strOutputResPath;
                string objectPathSame = itemPathSame + "/object/0";

                // 创建馆藏地与个人书斋不同的册
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     this.GetItemXml("阅览室")); //册的馆藏地改为与读者个人书斋不同。
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建册记录异常：" + writeRes.WriteResResult.ErrorInfo);

                string itemPathDiff = writeRes.strOutputResPath;
                string objectPathDiff = itemPathDiff + "/object/0";



                #region 第一组测试：新建册-WriteRes

                this.displayLine(this.getLarge("第一组测试：用WriteRes新建册xml"));


                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes创建册（馆藏地与个人书斋相同），预期成功。"));
                writeRes = this.WriteXml(u, newPath, this.GetXml(C_Type_item, true), true);  //册的馆藏地与读者个人书斋相同
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，册的馆藏地属于个人书斋，新建册应成功。"));


                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes创建册（馆藏地与个人书斋不同），预期失败。"));
                writeRes = this.WriteXml(u, newPath, this.GetItemXml("阅读室"), true);  //册的馆藏地与读者个人书斋不同
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，册的馆藏地属于个人书斋，新建册应失败。"));

                #endregion

                #region 第二组测试：修改册

                this.displayLine(this.getLarge("第二组测试：用WriteRes修改册xml"));

                // 修改馆藏地与个人书斋相同的册
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改馆藏地与个人书斋相同的册，预期成功。"));
                writeRes = this.WriteXml(u, itemPathSame, this.GetXml(C_Type_item, true), true);
                if (writeRes.WriteResResult.Value ==0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应可以修改馆藏地属于个人书斋的册。"));

                // 修改馆藏地与个人书斋不同的册
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改馆藏地与个人书斋不同的册，预期失败。"));
                writeRes = this.WriteXml(u, itemPathDiff, this.GetXml(C_Type_item, true), true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能修改馆藏地不属于个人书斋的册。"));

                #endregion

                #region 第三组测试，修改册下的对象数据

                this.displayLine(this.getLarge("第三组测试，修改册下的对象数据。"));

                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改属于个人书斋册的对象，预期成功。"));
                writeRes = this.WriteObject(u, objectPathSame, true);
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应该可以修改属于个人书斋册的对象。"));

                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes修改不属于个人书斋册的对象，预期失败。"));
                writeRes = this.WriteObject(u, objectPathDiff, true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者不可以修改非个人书斋册的对象。"));

                #endregion




                #region 第四组测试：获取册-GetRes/GetItemInfo

                this.displayLine(this.getLarge("第四组测试：获取册"));


                // 用GetRes获取属于个人书斋的册
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetRes()获取属于个人书斋的册xml，应成功。"));
                GetResResponse getResponse = this.GetRes(u, itemPathSame, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetItemInfo()获取属于个人书斋的册
                this.displayLine(GetBR() + getBold(u.UserName + "用GetItemInfo()获取属于个人书斋的册xml，应成功。"));
                GetItemInfoResponse getItemResponse = this.GetItemInfo(u, C_Type_item, itemPathSame, true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetRes获取不属于个人书斋的册
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetRes()获取不属于个人书斋的册xml，应成功。"));
                 getResponse = this.GetRes(u, itemPathSame, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetItemInfo()获取不属于个人书斋的册
                this.displayLine(GetBR() + getBold(u.UserName + "用GetItemInfo()获取不属于个人书斋的册xml，应成功。"));
                 getItemResponse = this.GetItemInfo(u, C_Type_item, itemPathDiff, true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第五组测试：获取册下对象

                this.displayLine(this.getLarge("第五组测试：获取册下对象"));

                // 用GetRes获取书目下对象
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes()获取属于个人书斋册下级的对象，应成功。"));
                 getResponse = this.GetRes(u, objectPathSame, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                // 用GetRes获取书目下对象
                this.displayLine(GetBR() + getBold(u.UserName + "用GetRes()获取不属于个人书斋册下级的对象，应成功。"));
                 getResponse = this.GetRes(u, objectPathDiff, true);
                if (getResponse.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                #endregion


                #region 第六组测试：删除册

                this.displayLine(this.getLarge("第六组测试：删除册"));

                this.displayLine(GetBR() + getBold(u.UserName + "用SetItemInfo删除属于个人书斋的册，预期成功。"));
                LibraryServerResult r = this.DelXml(u, C_Type_item, itemPathSame, true);
                if (r.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应可以删除属于个人书斋的册。"));


                this.displayLine(GetBR() + getBold(u.UserName + "用SetItemInfo删除不属于个人书斋的册，预期失败。"));
                r = this.DelXml(u, C_Type_item, itemPathDiff, true);
                if (r.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能删除非个人书斋的。"));

                #endregion

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

        private void button_readerLogin_item3_Click(object sender, EventArgs e)
        {
            /*
            用管理员身份准备环境：
            创建两个读者帐户，一个作为登录的读者帐号。
            创建两册
            为每个读者借一册。

            用读者1获取册1，应看到借阅者信息
            用读者1获取册2，应看不到借阅者。
             */

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;

                // 用supervisor帐户创建一个读者，有册的读权限，后面用此读者帐户操作
                this.displayLine(this.getLarge("用supervisor帐户创建一个读者，有册的读权限，后面用此读者帐户操作"));
                string readerBarcode = "";
                writeRes = this.CreateReaderBySuperviosr("getiteminfo",//setiteminfo, getiteminfo, writeitemobject, getitemobject",
                    "",  //个人书斋
                    out readerBarcode) ;
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建读者异常：" + writeRes.WriteResResult.ErrorInfo);
                string readerOwnerPath = writeRes.strOutputResPath;

                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                this.ChangeReaderPasswordBySupervisor(readerBarcode);
                UserInfo u = new UserInfo
                {
                    UserName = readerBarcode,
                    Password = "1",
                };

                // 用supervisor创建第2个读者
                this.displayLine(this.getLarge("用supervisor帐户创建第2个读者，下面会为这两个读者借书。"));
                string reader2 = "";
                writeRes = this.CreateReaderBySuperviosr("",//setiteminfo, getiteminfo, writeitemobject, getitemobject",
                    "",  //个人书斋
                    out reader2);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建读者异常：" + writeRes.WriteResResult.ErrorInfo);

                // 用supervisor创建两个册记录
                string newPath = this.GetAppendPath(C_Type_item);
                string itemBarcode1 = "";
                string itemPath1 = "";
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     GetXml(C_Type_item, true, out  itemBarcode1)); 
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建第1册异常：" + writeRes.WriteResResult.ErrorInfo);
                itemPath1 = writeRes.strOutputResPath;

                string itemBarcode2 = "";
                string itemPath2 = "";
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     GetXml(C_Type_item, true, out itemBarcode2));
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建第2册异常：" + writeRes.WriteResResult.ErrorInfo);
                itemPath2=writeRes.strOutputResPath; 

                // 为读者1借册1
                this.Borrow(this.mainForm.GetSupervisorAccount(),
                    readerBarcode,
                    itemBarcode1,
                    false);

                // 为读者2借册2
                this.Borrow(this.mainForm.GetSupervisorAccount(),
                    reader2,
                    itemBarcode2,
                    false);


                #region 第1组测试：获取册（借阅者是自己）

                this.displayLine(this.getLarge("第1组测试：获取册（借阅者是自己）"));


                // 用GetItemInfo()获取
                this.displayLine(GetBR() + getBold(u.UserName + "获取册信息（借阅者是自己），应看到完整借阅者。"));
                GetItemInfoResponse getItemResponse = this.GetItemInfo(u, C_Type_item, itemPath1, true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                    this.displayLine(this.getWarn1("请检查是否能看到借阅者完整信息。"));
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第2组测试：获取册（借阅者是别人）

                this.displayLine(this.getLarge("第2组测试：获取册（借阅者是别人）"));


                // 用GetItemInfo()获取
                this.displayLine(GetBR() + getBold(u.UserName + "获取册信息（借阅者是别人），借阅者应脱敏。"));
                 getItemResponse = this.GetItemInfo(u, C_Type_item, itemPath2, true);
                if (getItemResponse.GetItemInfoResult.Value >= 0)
                    this.displayLine(getWarn1("请检查借阅者是否脱敏。"));
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

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
            注意managecomment权限，目前是按增强权限的思路开发，即不能少了基础的setcommentinfo,getcommentinfo,writecommentobject,getcommentobject。
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

                // 用supervisor帐户创建一个读者，有册的读权限，后面用此读者帐户操作
               this.displayLine(this.getLarge("用supervisor帐户创建一个读者，有评论的完整权限，后面用此读者帐户操作"));
                string readerBarcode = "";
                writeRes = this.CreateReaderBySuperviosr("setcommentinfo,getcommentinfo,writecommentobject,getcommentobject",
                    "",  //个人书斋
                    out readerBarcode);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建读者异常：" + writeRes.WriteResResult.ErrorInfo);
                string readerOwnerPath = writeRes.strOutputResPath;

                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                this.ChangeReaderPasswordBySupervisor(readerBarcode);
                UserInfo u = new UserInfo
                {
                    UserName = readerBarcode,
                    Password = "1",
                };


                // 用supervisor创建一条评注
                this.displayLine(this.getLarge("用supervisor创建一条评注，作为别人的评注，看读者是否能操作"));

                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     GetXml(C_Type_comment, true));
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建评注异常：" + writeRes.WriteResResult.ErrorInfo);

                // 他人评注路径
                otherPath = writeRes.strOutputResPath;
                otherObject=otherPath +"/object/0";




                #endregion




                #region 第1组测试：读者应可以新建评注

                this.displayLine(this.getLarge("第1组测试：读者应可以新建评注"));


                // 用WriteRes新建评注
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes新建评注。"));
                writeRes= this.WriteXml(u, newPath,
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
                ret=  this.SetItemInfo(u,
                    "comment",
                    "new",
                    newPath,
                    this.GetXml(C_Type_comment, true), true,out ownerPathForDelete);
                if (ret.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

               // ownerPathForDelete= writeRes.strOutputResPath;

                #endregion


                #region 第2组测试：读者修改评注

                this.displayLine(this.getLarge("第2组测试：读者修改评注，应可修改自己的，不能修改他人。"));


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



                    // 修改自己的creator
                    DomUtil.SetElementText(dom.DocumentElement, "title", "标题修改2");
                    DomUtil.SetElementText(dom.DocumentElement, "content", "内容修改2");
                    DomUtil.SetElementText(dom.DocumentElement, "creator", "创建者修改2");

                    this.displayLine(GetBR() + getBold(u.UserName + "修改自己的评注xml的creator，应失败。"));
                    writeRes = this.WriteXml(u, ownerPathForChange,
                        dom.OuterXml, true);
                    if (writeRes.WriteResResult.Value ==-1)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期"));


                }
                else
                    this.displayLine(getRed("不符合预期"));



                // 修改自己评注下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "修改自己的评注的对象，应成功。"));
                writeRes = this.WriteObject(u,ownerObjectForChange, true);
                if (writeRes.WriteResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                //==修改他人的评注和对象

                //修改他的人评注
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的评注xml，应失败。"));
                 ret = this.SetItemInfo(u,
                     "comment",
                     "change",
                     otherPath,
                     this.GetXml(C_Type_comment, false), true,out string temp);
                if (ret.Value ==-1)
                    this.displayLine(getWarn1("符合预期，注意检查报错信息"));
                else
                    this.displayLine(getRed("不符合预期"));


                // 修改他人评注下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的评注的对象，应失败。"));
                writeRes = this.WriteObject(u,otherObject, true);
                if (writeRes.WriteResResult.Value ==-1)
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
                ret= this.DelXml(u, C_Type_comment, ownerPathForDelete, true);
                if (ret.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应可以删除自己的评注。"));

                // 删除他人的
                this.displayLine(GetBR() + getBold(u.UserName + "删除他人的评注，应失败。"));
                ret = this.DelXml(u, C_Type_comment, otherPath, true);
                if (ret.Value ==-1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者不能删除他人的评注。"));

                #endregion


                #region 第5组测试：读者帐号有 managecomment 权限，应可以修改和删除他人评注。

                
                // 用supervisor帐户创建一个读者，有managecomment权限，后面用此读者帐号操作他人评注，应成功。
                this.displayLine(this.getLarge("读者帐号有 managecomment 权限，应可以修改和删除他人评注"));
                string reader2 = "";
                writeRes = this.CreateReaderBySuperviosr("managecomment,setcommentinfo,getcommentinfo,writecommentobject,getcommentobject",
                    "",  //个人书斋
                    out reader2);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建读者异常：" + writeRes.WriteResResult.ErrorInfo);
                //string readerOwnerPath = writeRes.strOutputResPath;

                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + reader2 + "的密码，后面用此读者身份登录。"));
                this.ChangeReaderPasswordBySupervisor(reader2);
                u = new UserInfo
                {
                    UserName = reader2,
                    Password = "1",
                };


                //修改他的人评注
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的评注xml，应成功。"));
                ret = this.SetItemInfo(u,
                    "comment",
                    "change",
                    otherPath,
                    this.GetXml(C_Type_comment, false), true,out string temp1);
                if (ret.Value == 0)
                    this.displayLine(getWarn1("符合预期"));
                else
                    this.displayLine(getRed("不符合预期，读者有managecomment权限，应可修改他人的评注。"));


                // 修改他人评注下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的评注的对象，应成功。"));
                writeRes = this.WriteObject(u, otherObject, true);
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者有managecomment权限，应可删除他人评注的对象。"));
                
                //===
                /*
                // 删除他人的
                this.displayLine(GetBR() + getBold(u.UserName + "删除他人的评注，应成功。"));
                ret = this.DelXml(u, C_Type_comment, otherPath, true);
                if (ret.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者有managecomment权限，应可删除他人的评注。"));
                */
                #endregion

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

            this.CheckArrivedAmerce(C_Type_Arrived);
        }


        public void CheckArrivedAmerce(string type)
        {
            /*
准备环境：
管理员创建两个读者
创建两个册记录
为读者1预约1，直接用WriteRes来写，主要是使用这条记录。
为读者2预约2，

用读者身份，应可以获取自己的预约到书，不能获取别人的预约到书。
不能 创建/修改/删除 自己和他人的预约记录。
目前读者可以 修改 和 删除预约者是自己的记录？？？


 */
            string rights = this.GetFullRights(type);

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeRes = null;
                LibraryServerResult ret = null;

                // 目前此函数仅支持 预约到书 和 违约金
                if (type != C_Type_Arrived && type != C_Type_Amerce)
                    throw new Exception("CheckArrivedAmerce()不支持的类型" + type);


                #region 环境准备

                // 用管理员身份创建一个读者，有xml和对象的完整权限，后面用此读者帐户操作
                string readerBarcode = "";
                writeRes = this.CreateReaderBySuperviosr(rights,
                    "",  //个人书斋
                    out readerBarcode);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建读者异常：" + writeRes.WriteResResult.ErrorInfo);
                string readerOwnerPath = writeRes.strOutputResPath;
                this.displayLine(this.getLarge("用管理员身份创建一个读者"+readerBarcode+"，有" + type + "xml和对象的完整权限，后面用此读者帐户登录操作"));


                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码。"));
                this.ChangeReaderPasswordBySupervisor(readerBarcode);
                UserInfo u = new UserInfo
                {
                    UserName = readerBarcode,
                    Password = "1",
                };

                // 用supervisor创建第2个读者
                string reader2 = "";
                writeRes = this.CreateReaderBySuperviosr("",//第2个读者不设权限,
                    "",  //个人书斋
                    out reader2);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("用管理员身份创建读者异常：" + writeRes.WriteResResult.ErrorInfo);
                this.displayLine(this.getLarge("用管理员身份创建第2个读者"+reader2));


                // 用supervisor创建两个册记录
                this.displayLine(this.getLarge("用管理员身份创建2条册记录。"));

                string itemNewPath = this.GetAppendPath(C_Type_item);
                string itemBarcode1 = "";
                string itemPath1 = "";
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     itemNewPath,
                     GetXml(C_Type_item, true, out itemBarcode1));
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建第1册异常：" + writeRes.WriteResResult.ErrorInfo);
                itemPath1 = writeRes.strOutputResPath;

                string itemBarcode2 = "";
                string itemPath2 = "";
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     itemNewPath,
                     GetXml(C_Type_item, true, out itemBarcode2));
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员创建第2册异常：" + writeRes.WriteResResult.ErrorInfo);
                itemPath2 = writeRes.strOutputResPath;

                // 预约路径
                string ownerXmlPath = "";  //自己预约的书
                string ownerObjectPath = "";
                string otherXmlPath = "";   // 他人预约的书
                string otherObjectPath = "";
                string newPath = this.GetAppendPath(type);


                // 管理员为读者1 针对 册1 写预约到书 或者 违约金记录
                string strXml1 = "";
                if (type == C_Type_Arrived)
                    strXml1 = GetArrivedXml(readerBarcode, itemBarcode1, true);
                else if (type == C_Type_Amerce)
                    strXml1= this.GetAmerceXml(readerBarcode, itemBarcode1, true);

                this.displayLine(this.getLarge("管理员为读者" + readerBarcode + "针对册" + itemBarcode1 +"创建'"+type+"'记录"));
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                                                     newPath,
                                                     strXml1,
                                                     false);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员写"+type+"记录异常：" + writeRes.WriteResResult.ErrorInfo);
                ownerXmlPath = writeRes.strOutputResPath;
                ownerObjectPath = ownerXmlPath + "/object/0";

                // 管理员为读者2 针对 册2 写预约到书 或者 违约金记录
                string strXml2 = "";
                if (type == C_Type_Arrived)
                    strXml2 = GetArrivedXml(reader2, itemBarcode2, true);
                else if (type == C_Type_Amerce)
                    strXml2 = this.GetAmerceXml(reader2, itemBarcode2, true);
                this.displayLine(this.getLarge("管理员为读者" + reader2 + "针对册" + itemBarcode2 + "创建'" + type + "'记录"));
                writeRes = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                                                     newPath,
                                                    strXml2,
                                                     false);
                if (writeRes.WriteResResult.Value == -1)
                    throw new Exception("管理员写"+type+"异常：" + writeRes.WriteResResult.ErrorInfo);
                otherXmlPath = writeRes.strOutputResPath;
                otherObjectPath = otherXmlPath + "/object/0";

                #endregion



                #region 第1组测试：读者新建预约到书/违约金，应不能。

                this.displayLine(this.getLarge("第1组测试：读者新建"+type+"记录，应不可以。"));


                // 用WriteRes新建自己的预约到书 或 违约金
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes新建自己的"+type+"xml，应失败。"));
                writeRes = this.WriteXml(u, newPath,
                   strXml1,//this.GetXml(C_Type_Arrived, true), 
                    true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能创建属于自己的"+type+"记录。"));


                // 用WriteRes新建别人的预约到书 或 违约金
                this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes新建他人的" + type + "xml，应失败。"));
                writeRes = this.WriteXml(u, newPath,
                   strXml2,//this.GetXml(C_Type_Arrived, true), 
                    true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能创建他人的"+type+"记录。"));

                #endregion

                #region 第2组测试：获取自己的预约到书/违约金

                this.displayLine(this.getLarge("第2组测试：获取自己的" + type));


                // 用GetRes()获取
                this.displayLine(GetBR() + getBold(u.UserName + "获取自己的" + type + "xml，应成功。"));
                GetResResponse getRes = this.GetRes(u, ownerXmlPath, true);
                if (getRes.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetRes()获取自己下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "获取自己的" + type + "的对象，应成功。"));
                getRes = this.GetRes(u, ownerObjectPath, true);
                if (getRes.GetResResult.Value >= 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第3组测试：获取他人的预约到书/违约金

                this.displayLine(this.getLarge("第3组测试：获取他人的" + type));


                // 用GetRes()获取
                this.displayLine(GetBR() + getBold(u.UserName + "获取他人的" + type + "xml，应失败。"));
                getRes = this.GetRes(u, otherXmlPath, true);
                if (getRes.GetResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用GetRes()获取他人下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "获取他人的" + type + "下的对象，应失败。"));
                getRes = this.GetRes(u, otherObjectPath, true);
                if (getRes.GetResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion


                #region 第4组测试：读者修改自己的预约到书 或 违约金

                this.displayLine(this.getLarge("第2组测试：读者修改自己的"+type+"，应成功。"));


                // 修改预约者是自己的预约到书/违约金xml
                this.displayLine(GetBR() + getBold(u.UserName + "修改自己的"+type+"xml，应成功。"));
                writeRes = this.WriteXml(u, ownerXmlPath,
                    this.GetArrivedXml(readerBarcode, "B003", true), true);
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应可以修改自己的"+type+"xml。"));

                // 修改预约者是自己的预约到书的对象
                this.displayLine(GetBR() + getBold(u.UserName + "修改自己的"+type+"的对象，应成功。"));
                writeRes = this.WriteObject(u, ownerObjectPath, true);
                if (writeRes.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应可以修改自己"+type+"下的对象。"));

                #endregion

                #region 第5组测试：读者修改他人的预约到书 或 违约金

                this.displayLine(this.getLarge("第5组测试：读者修改他人的" + type + "，应失败。"));


                //修改预约者是他人的预约到书xml
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的"+type+"xml，应失败。"));
                writeRes = this.WriteXml(u, otherXmlPath,
                    this.GetArrivedXml(reader2, "B004", true), true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                // 修改预约者是他人的预约到书的对象
                this.displayLine(GetBR() + getBold(u.UserName + "修改他人的"+type+"的对象，应失败。"));
                writeRes = this.WriteObject(u, otherObjectPath, true);
                if (writeRes.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion




                #region 第6组测试：读者删除预约到书/违约金，应不可以

                this.displayLine(this.getLarge("第6组测试：读者删除预约到书"));


                // 删除预约者是自己的预约到书
                this.displayLine(GetBR() + getBold(u.UserName + "删除自己的"+type+"xml，应成功。"));
                ret = this.DelXml(u, C_Type_Arrived, ownerXmlPath, true);
                if (ret.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应可以删除自己的"+type+"xml。"));

                // 删除预约者是他人的预约到书
                this.displayLine(GetBR() + getBold(u.UserName + "删除他人的"+type+"xml，应失败。"));
                ret = this.DelXml(u, C_Type_Arrived, otherXmlPath, true);
                if (ret.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者不能删除他人的"+type+"xml。"));

                #endregion




            }
            catch (Exception e1)
            {
                MessageBox.Show(this, "读者身份获取预约到书-异常:" + e1.Message);
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

            this.displayLine(this.getLarge("针对'"+type+"'测试GetItemInfo()"));

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
            UserInfo u = new UserInfo
            {
                UserName = "u1",
                Password = "1",
                SetPassword = true,
                Rights =rights,
                Access = ""
            };
            //创建帐号
            this.NewUser(u);


            string barcode = "@path:" + resPath;

            //1）验证获取册，仅xml
            this.displayLine(getLarge("1）验证获取册，仅xml"));
            string resultType = "xml";
            this.displayLine("strResultType=" + resultType);
            List<bool> shoudSuccList = new List<bool>() { true};
            getItemResponse= this.GetItemInfoInternel(u, strItemDbType, barcode, resultType, "", false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {

                //检查xml，_format 
                bRet=CheckFormat(getItemResponse.strResult, resultType, shoudSuccList ,out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。"+strError));
            }
            else
                this.displayLine(getRed("不符合预期"));


            //2）验证获取多种内容 xml,html,uii。_format要对应，没有_errorCode属性。
            this.displayLine(getLarge("2）验证获取多种内容 xml,html,uii。_format要对应，没有_errorCode属性。"));
            resultType = "xml,html,uii";
            this.displayLine("strResultType=" + resultType);
            shoudSuccList = new List<bool>() { true,true,true };
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode,resultType , "", false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strResult,resultType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。"+ strError));
            }
            else
                this.displayLine(getRed("不符合预期"));


            //3）验证多种格式中，请求参数包括异常格式的情况。异常的格式应有_errorCode。
            //<results><result _format="test" _errorCode="SystemError" _errorInfo="未知的书目格式 'test'"></result>...
            this.displayLine(getLarge("3）验证多种格式中，请求参数包括异常格式的情况。异常的格式应有_errorCode。"));
            resultType = "a,xml,b,uii,test";
            this.displayLine("strResultType=" + resultType);
            shoudSuccList = new List<bool>() { false,true,false, true, false };
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
            shoudSuccList = new List<bool>() { true,true,true,true};
            getItemResponse = this.GetItemInfoInternel(u, strItemDbType, barcode, resultType, "", false);
            if (getItemResponse.GetItemInfoResult.Value >= 0)
            {
                //检查xml，_format 
                bRet = CheckFormat(getItemResponse.strResult, resultType, shoudSuccList, out strError);
                if (bRet == true)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期。"+ strError));
            }
            else
                this.displayLine(getRed("不符合预期"));


            //5）验证获取书目，无书目权限，应不成功。
            this.displayLine(getLarge("5）验证获取书目，无书目权限，应不成功。"));
            string biblioType = "xml";
            this.displayLine("strBiblioType=" + biblioType);
            shoudSuccList = new List<bool>() { false};
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
            u = new UserInfo
            {
                UserName = "u1",
                Password = "1",
                SetPassword = true,
                Rights = fullrights,
                Access = ""
            };
            //创建帐号
            this.NewUser(u);

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
            shoudSuccList = new List<bool>() { true,true,true };
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
            shoudSuccList = new List<bool>() { false,true, true, false };
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
            this.displayLine("strBiblioType="+biblioType);
            shoudSuccList = new List<bool>() { true, true, true, true,true };
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

        private bool CheckFormat(string xml, string formats,List<bool> shoudSuccList,out string strError)
        {
            strError = "";

            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);


            XmlNodeList list = null;
            string[] formatList = formats.Split(new char[] {','});
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
                bool shoudSucc= shoudSuccList[i];

                string f = DomUtil.GetAttr(node, "_format");
                if (f != requestF)
                {
                    strError = "第"+i+"个formate不对应";
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
            this.CheckOrderIssueAmerceForReader(C_Type_order);
        }

        // 期
        private void button_readerLogin_issue_Click(object sender, EventArgs e)
        {
            this.CheckOrderIssueAmerceForReader(C_Type_issue);
        }

        public string GetFullRights(string type)
        {
            if (type == C_Type_order)
            {
                return "setorderinfo,getorderinfo,writeorderobject,getorderobject";
            }
            else if (type == C_Type_issue)
            {
                return "setissueinfo,getissueinfo,writeissueobject,getissueobject";
            }
            else if (type == C_Type_Amerce)
            {
                return "setamerceinfo,getamerceinfo,writeamerceobject,getamerceobject";
            }
            else if (type == C_Type_Arrived)
            {
                return "setarrivedinfo,getarrivedinfo,writearrivedobject,getarrivedobject";
            }
            else
            {
                throw new Exception("GetFullRights不支持的类型");
            }

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

        public void CheckOrderIssueAmerceForReader(string type)
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

            string rights = this.GetFullRights(type);

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                WriteResResponse writeResponse = null;
                GetResResponse getResponse = null;
                LibraryServerResult result = null;

                // 用supervisor帐户创建一个读者，有操作xml与对象的完整，后面用此读者帐户操作
                this.displayLine(this.getLarge("用管理员身份创建一个读者，有完整的"+type+"权限，后面用此读者帐户操作"));
                string readerBarcode = "";
                writeResponse = this.CreateReaderBySuperviosr(rights,
                    "",  //个人书斋
                    out readerBarcode);
                if (writeResponse.WriteResResult.Value == -1)
                    throw new Exception("管理员创建读者异常：" + writeResponse.WriteResResult.ErrorInfo);
                string readerOwnerPath = writeResponse.strOutputResPath;

                // 修改读者的密码
                this.displayLine(this.getBold("修改读者" + readerBarcode + "的密码，后面用此读者身份登录。"));
                this.ChangeReaderPasswordBySupervisor(readerBarcode);
                UserInfo u = new UserInfo
                {
                    UserName = readerBarcode,
                    Password = "1",
                };


                // 用管理员身份创建一条记录xml
                string newPath = this.GetAppendPath(type);
                string xmlPath = "";

                string xml = this.GetXml(type, true);
                    

                writeResponse = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                     newPath,
                     GetXml(type, true));
                if (writeResponse.WriteResResult.Value == -1)
                    throw new Exception("管理员创建"+type+"记录异常：" + writeResponse.WriteResResult.ErrorInfo);
                xmlPath = writeResponse.strOutputResPath;
                string objectPath = xmlPath + "/object/0";

                // 书目记录
                string parentPath = "";

                if (type == C_Type_issue)
                    parentPath = this.GetIssueBiblioPath();
                else
                    parentPath = GetBiblioPath();




                #region 第1组测试：读者获取记录

                this.displayLine(this.getLarge("第1组测试：读者获取"+type+"记录"));

                //用 GetRes 获取
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetRes 获取"+type+"xml，应失败。"));
                getResponse = this.GetRes(u, xmlPath, true);
                if (getResponse.GetResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                if (type == C_Type_order || type == C_Type_issue)
                {

                    // 用 GetItemInfo 获取
                    this.displayLine(GetBR() + getBold(u.UserName + "用 GetItemInfo 获取" + type + "xml，应失败。"));
                    GetItemInfoResponse getItemResponse = this.GetItemInfo(u, type, xmlPath, true);
                    if (getItemResponse.GetItemInfoResult.Value == -1)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期"));

                    // 用 GetEntities等 获取
                    this.displayLine(GetBR() + getBold(u.UserName + "用 GetOrders 获取" + type + "xml，应失败。"));
                    result = this.GetFirstEntity(u, type, parentPath, out EntityInfo entity, true);
                    if (result.Value == -1)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期"));
                }


                //用 GetRes 获取 订购下的对象
                this.displayLine(GetBR() + getBold(u.UserName + "用 GetRes 获取"+type+"的对象，应失败。"));
                getResponse = this.GetRes(u, objectPath, true);
                if (getResponse.GetResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 第2组测试：读者创建订购记录

                string ownerXmlPath = "";

                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 创建"+type+"xml，应失败。"));
                writeResponse = this.WriteXml(u,
                     newPath,
                     GetXml(type, true), true);
                if (writeResponse.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能创建"+type+"记录。"));

                ownerXmlPath = writeResponse.strOutputResPath;
                string ownerObjectPath = ownerXmlPath + "/object/0";


                if (type == C_Type_order || type == C_Type_issue)
                {
                    // 用 SetItemInfo 创建
                    this.displayLine(GetBR() + getBold(u.UserName + "用 SetItemInfo 创建" + type + "xml，应失败。"));
                    result = this.SetItemInfo(u,
                       type2dbtype(type),
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
                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 修改"+type+"xml，应失败。"));
                writeResponse = this.WriteXml(u,
                     xmlPath,
                     GetXml(type, true), true);
                if (writeResponse.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能修改"+type+"xml。"));


                if (type == C_Type_order || type == C_Type_issue)
                {
                    // 用 SetItemInfo 修改
                    this.displayLine(GetBR() + getBold(u.UserName + "用 SetItemInfo 修改" + type + "xml，应失败。"));
                    result = this.SetItemInfo(u,
                        type2dbtype(type),
                        "change",
                        xmlPath,
                       GetXml(type, true),
                       true,
                       out string tempPath);
                    if (result.Value == -1)
                        this.displayLine("符合预期");
                    else
                        this.displayLine(getRed("不符合预期，读者应不能修改" + type + "记录xml。"));
                }


                // 修改订购object
                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 修改"+type+"的对象，应失败。"));
                writeResponse = this.WriteObject(u,
                     objectPath, true);
                if (writeResponse.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能修改"+type+"的对象。"));


                #endregion


                #region 第3组测试：读者删除记录

                // 删除订购xml
                this.displayLine(GetBR() + getBold(u.UserName + "用 WriteRes 删除"+type+"xml，应失败。"));
                writeResponse = this.WriteResForDel(u,
                     xmlPath,
                      true);
                if (writeResponse.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，读者应不能删除"+type+"xml。"));


                if (type == C_Type_order || type == C_Type_issue)
                {
                    // 用 SetItemInfo 删除
                    this.displayLine(GetBR() + getBold(u.UserName + "用 SetItemInfo 删除" + type + "xml，应失败。"));
                    result = this.SetItemInfo(u,
                        type2dbtype(type),
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
                MessageBox.Show(this, "用读者身份操作"+type+"-异常:" + e1.Message);
            }
            finally
            {
                this.EnableCtrls(true);
            }
        }

        // 用读者身份测试违约金
        private void button_readerLogin_amerce_Click(object sender, EventArgs e)
        {
            //this.CheckOrderIssueAmerceForReader(C_Type_Amerce);


            this.CheckArrivedAmerce(C_Type_Amerce);
        }
    }
}
