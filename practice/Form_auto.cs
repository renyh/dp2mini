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
          string objectPath)
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
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password);

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
                    +RestChannel.GetResultInfo(response));

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
                    + RestChannel.GetResultInfo(response));

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
            string strResPath)
        {
            GetResResponse response = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password);

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

        public SetReaderInfoResponse DelReader1(UserInfo u,
            string strResPath)
        {
            return this.SetReaderInfo(u, "delete", strResPath, "");
        }


        public LibraryServerResult DelXml(UserInfo u,
            string type,
            string strResPath)
        {
            if (type == C_Type_reader)
                return this.SetReaderInfo(u, "delete", strResPath, "").SetReaderInfoResult;
            else if (type == C_Type_biblio)
                return this.SetBiblio(u, "delete", strResPath, "").SetBiblioInfoResult;
            else if (type == C_Type_item
                || type==C_Type_order
                || type== C_Type_comment
                || type == C_Type_issue
                || type==C_Type_Amerce
                || type==C_Type_Arrived)
            {
                // todo
                this.displayLine("暂不支持删除"+type);
                return new LibraryServerResult();
            }

            throw new Exception("DelXml不支持的类型" + type);
        }

        public SetReaderInfoResponse SetReaderInfo(UserInfo u,
            string strAction,
            string strResPath,
             string strNewXml)
        {
            SetReaderInfoResponse response = null;

            this.displayLine("strResPath=" + strResPath);

            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password);

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

                this.displayLine(HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
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


        public SetBiblioInfoResponse SetBiblio(UserInfo u,
    string strAction,
    string strResPath,
     string strNewXml)
        {
            SetBiblioInfoResponse response = null;

            this.displayLine("strResPath=" + strResPath);

            byte[] baTimestamp = null;

            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password);

            REDO:
                response = channel.SetBiblioInfo(strAction,
                    strResPath,
                    "xml",
                    strNewXml,
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

                this.displayLine(HttpUtility.HtmlEncode(RestChannel.GetResultInfo(response)));
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

        #endregion

        #region 书目存取定义与对象权限

        // 存取定义与字段
        private void button_accessAndObject_Click(object sender, EventArgs e)
        {
            this.EnableCtrls(false);
            try
            {

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
                MessageBox.Show(this, "异常:" + e1.Message);
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
            line = line.Replace("\r\n", "<br/>");

            AppendHtml(this.webBrowser1, line + "<br/>");

            Application.DoEvents();
        }

        public void ClearResult()
        {
            this.webBrowser1.DocumentText = "<html><body>test</body></html>";

            //Thread.Sleep(1000); 
            //this.textBox_result.Text = "";
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

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
                nsmgr.AddNamespace("dprms", DpNs.dprms);

                XmlNodeList nodes = root.SelectNodes("//dprms:file", nsmgr);

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

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());
                nsmgr.AddNamespace("dprms", DpNs.dprms);

                XmlNodeList nodes = root.SelectNodes("//dprms:file", nsmgr);

                // 删除第3个file
                if (nodes.Count > 2)
                {
                    XmlNode node = nodes[2];
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
                    + "<location>流通库</location>"
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
                              <index>11</index>
                              <content>一本好书</content>"
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
            else if (type == C_Type_Amerce)
            {
                return @"<root>
                            <itemBarcode>B001</itemBarcode>
                            <location>流通库</location>
                            <readerBarcode>P001</readerBarcode>
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
            else if (type == C_Type_Arrived)
            {
                return @"<root>
                            <state></state>
                            <itemBarcode>B001</itemBarcode>
                            <onShelf>true</onShelf>
                            <readerBarcode>P001</readerBarcode>
                            <notifyDate>Thu, 21 Jul 2022 15:31:16 +0800</notifyDate>
                            <location>流通库</location>
                            <accessNo>I206.2/W460</accessNo>"
                              + dprmsfile
                        +"</root>";
            }

            throw new Exception("GetXml不支持的数据类型" + type);
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
                LibraryServerResult res = this.DelXml(u,type, tempPath);
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
                res = this.DelXml(u,type, tempPath);
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
                this.displayLine(GetBR() + getBold(u.UserName + "对xml新增两个dprms:file，此时应有3个dprms:file"));
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
                        //tempResponse.
                        this.displayLine(this.getWarn1("请核对是否有3个dprms:file"));
                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));

                // 修改第1个dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "对xml修改了第1个dprms:file，增加了a属性"));
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
                        //tempResponse.
                        this.displayLine(this.getWarn1("请核对第1个dprms:file是否增加了a属性"));
                    }
                }
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除第3个dprms:file
                this.displayLine(GetBR() + getBold(u.UserName + "对xml删除第3个dprms:file，应成功"));
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
                        //tempResponse.
                        this.displayLine(this.getWarn1("请核对是否删除了第3个dprms:file"));
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
                    this.displayLine(getWarn1("请核对返回结果是否过滤了dprms:file。"));
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
                    this.displayLine(getWarn1("请核对返回结果中应包含dprms:file。"));
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

        private void button_readerLogin_Click(object sender, EventArgs e)
        {
            // 管理员先创建两条读者
            // 用supervisor帐户创建一条书目
            string strReaderPath = "读者/?";

            string readerBarcode = "";
            string readerxml=this.GetXml(C_Type_reader,false,out readerBarcode);
            // 修改一下权限
            XmlDocument dom = new XmlDocument ();
            dom.LoadXml(readerxml);
            XmlNode root = dom.DocumentElement;
            root.InnerXml += "<rights>setreaderinfo,getreaderinfo,setbiblioinfo,getbiblioinfo</rights>";
            readerxml = dom.OuterXml;

            this.displayLine("用supervisor创建读者"+readerBarcode+"，后面将用此读者帐户进行操作。");

            WriteResResponse response = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                strReaderPath,
                 readerxml);

            if (response.WriteResResult.Value == -1)
                throw new Exception("用supervisor创建读者异常:" + response.WriteResResult.ErrorInfo);

            // 修改读者的密码
            this.ChangeReaderPasswordBySupervisor(readerBarcode);

            
            UserInfo u = new UserInfo
            {
                UserName = readerBarcode,
                Password = "1",
            };

            //====
            // 读者身份，应能写书目和获取书目。

            string strBiblioPath = this.GetAppendPath(C_Type_biblio);
            // 用WriteRes获取书目
            this.displayLine(GetBR() + getBold(u.UserName + "用WriteRes()新建书目xml，应成功。"));
            response = this.WriteXml(u, strBiblioPath, this.GetXml(C_Type_biblio, false),true);
            if (response.WriteResResult.Value == 0)
                this.displayLine("符合预期");
            else
                this.displayLine(getRed("不符合预期"));

            // 用SetBiblioInfo新建书目
            this.displayLine(GetBR() + getBold(u.UserName + "用SetBiblioInfo()新建书目xml，应成功。"));
            SetBiblioInfoResponse res = this.SetBiblioInfo(u, "new", strBiblioPath, this.GetXml(C_Type_biblio, false), true);
            if (res.SetBiblioInfoResult.Value ==0)
                this.displayLine("符合预期");
            else
                this.displayLine(getRed("不符合预期"));

            string tempBiblioPath = res.strOutputBiblioRecPath;

            // 用SetBiblioInfo修改书目
            this.displayLine(GetBR() + getBold(u.UserName + "用SetBiblioInfo()修改书目xml，应成功。"));
             res = this.SetBiblioInfo(u, "change", tempBiblioPath, this.GetXml(C_Type_biblio, false), true);
            if (res.SetBiblioInfoResult.Value == 0)
                this.displayLine("符合预期");
            else
                this.displayLine(getRed("不符合预期"));

            // 用SetBiblioInfo删除书目
            this.displayLine(GetBR() + getBold(u.UserName + "用SetBiblioInfo()删除书目xml，应成功。"));
            res = this.SetBiblioInfo(u, "delete", tempBiblioPath,"", true);
            if (res.SetBiblioInfoResult.Value == 0)
                this.displayLine("符合预期");
            else
                this.displayLine(getRed("不符合预期"));

        }
    }
}
