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
            this.button_reader.Enabled = bEnable;
            this.button_biblio.Enabled = bEnable;
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


        // 写xml
        public WriteResResponse WriteXml(UserInfo u,
          string strResPath,
          string strXml)
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

            // 用u_a3帐户，写xml
            RestChannel channel = null;
            try
            {
                // 用户登录
                channel = mainForm.GetChannelAndLogin(u.UserName, u.Password);

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

        public SetReaderInfoResponse DelReader(UserInfo u,
            string strResPath)
        {
            return this.SetReaderInfo(u, "delete", strResPath, "");
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

        #endregion

        #region 书目存取定义与对象权限

        // 存取定义与字段
        private void button_accessAndObject_Click(object sender, EventArgs e)
        {
            this.EnableCtrls(false);
            try
            {

                #region 创建帐户

                // 创建5个临时帐户
                List<UserInfo> users = new List<UserInfo>();

                UserInfo a1 = new UserInfo
                {
                    UserName = "a1",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=change|getbiblioinfo=*"
                };
                users.Add(a1);

                //===
                UserInfo a1_1 = new UserInfo
                {
                    UserName = "a1_1",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=change(606)|getbiblioinfo=*"
                };
                users.Add(a1_1);

                //===
                UserInfo a2 = new UserInfo
                {
                    UserName = "a2",
                    Password = "1",
                    SetPassword = true,

                    //new,应不能change对象
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=new|getbiblioinfo=*"
                };
                users.Add(a2);

                //===
                UserInfo a3 = new UserInfo
                {
                    UserName = "a3",
                    Password = "1",
                    SetPassword = true,

                    //delete,应不能change对象
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=delete|getbiblioinfo=*"
                };
                users.Add(a3);

                //===
                UserInfo a4 = new UserInfo
                {
                    UserName = "a4",
                    Password = "1",
                    SetPassword = true,

                    //通配符*表示new,change,and，应能change对象
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=*|getbiblioinfo=*"
                };
                users.Add(a4);

                //===
                // 创建这批帐户
                this.SetUsers(users, "new");

                #endregion

                //===
                //先用管理员身从那写一条书目记录， 带dprms:file
                string strBiblioPath = "中文图书/?";
                string xml = @"<unimarc:record xmlns:dprms='http://dp2003.com/dprms' xmlns:unimarc='http://dp2003.com/UNIMARC'>
                                  <unimarc:leader>01887nam0 2200325   450 </unimarc:leader>
                                  <unimarc:datafield tag='200' ind1=' ' ind2=' '>
                                    <unimarc:subfield code='a'>我爱我家</unimarc:subfield>
                                  </unimarc:datafield>
                                  <dprms:file id='0'/>
                                </unimarc:record>";


                WriteResResponse response = this.WriteXml(this.mainForm.GetSupervisorAccount(),
                    strBiblioPath,
                    xml);
                strBiblioPath = response.strOutputResPath;
                this.displayLine("<br/>用管理员身份创建了一条书目记录。" + strBiblioPath);

                //===
                this.displayLine("以下是用不同帐户测试修改对象数据。");
                string objectPath = strBiblioPath + "/object/0";

                // 用u_a1帐户
                response = this.WriteObject(a1, objectPath);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                // 用u_a1_1帐户
                response = this.WriteObject(a1_1, objectPath);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用u_a2帐户
                response = this.WriteObject(a2, objectPath);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用u_a3帐户
                response = this.WriteObject(a3, objectPath);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 用u_a4帐户
                response = this.WriteObject(a4, objectPath);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));


                //====
                // 删除这批帐户
                this.SetUsers(users, "delete");


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
            this.webBrowser1.DocumentText = "<html><body></body></html>";

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


        #region 读写读者xml和对象所需权限




        // 读者及对象所需权限
        private void button_reader_Click(object sender, EventArgs e)
        {

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                this.displayLine(getLarge("读者及下级对象所需权限的测试结果"));


                string strResPath = "读者/?";
                WriteResResponse response = null;

                // 有用的读者xml路径
                string path_xmlHasFile = "";
                string path_object = "";

                #region 第1组测试  仅有setreaderinfo

                //===
                this.displayLine(getLarge("第1组测试"));
                // 仅有setreaderinfo
                UserInfo u = new UserInfo
                {
                    UserName = "r1",
                    Password = "1",
                    SetPassword = true,
                    Rights = "setreaderinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                this.displayLine(GetBR() + getBold(u.UserName+"写简单xml，由于没有连带的读xml权限，应不成功。"));
                response = this.WriteXml(u, strResPath, this.GetReaderXml(false));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期，应拒绝，提示权限不够。"));

                //删除帐号
                this.DelUser(u);

                #endregion


                #region 第2组测试 setreaderinfo, getreaderinfo

                //===
                this.displayLine(getLarge("第2组测试"));
                //setreaderinfo, getreaderinfo
                //可写简单读者xml，包括add/new/delete
                //不可操作xml中dprms:file，不能写对象
                u = new UserInfo
                {
                    UserName = "r2",
                    Password = "1",
                    SetPassword = true,
                    Rights = "setreaderinfo,getreaderinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);  

                // 新建简单xml
                this.displayLine(GetBR() + getBold(u.UserName+"新建简单xml，有读写xml权限，应新建成功。"));
                response = this.WriteXml(u, strResPath, this.GetReaderXml(false));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                string tempPath = response.strOutputResPath;

                // 修改简单xml
                this.displayLine(GetBR() + getBold(u.UserName+"修改简单xml，应修改成功，且错误码应为NoError。"));
                response = this.WriteXml(u, tempPath, this.GetReaderXml(false));
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
                this.displayLine(GetBR() + getBold(u.UserName+"删除简单xml，应删除成功。"));
                SetReaderInfoResponse res = this.DelReader(u, tempPath);
                if (res.SetReaderInfoResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));



                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold(u.UserName+"新建带dprms:file的xml，由于没有对象权限，应写入失败 或者 部分写入且过滤了file(注意观察提示)。"));
                response = this.WriteXml(u, strResPath, this.GetReaderXml(true));
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
                this.displayLine(GetBR() + getBold(u.UserName+"写对象数据，由于没有对象权限，应不能成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(u);

                #endregion


                #region 第3组测试  setreaderinfo,getreaderinfo,writereaderobject

                //===
                this.displayLine(getLarge("第3组测试"));
                // setreaderinfo,getreaderinfo,writereaderobject
                // 效果同r1_2
                u = new UserInfo
                {
                    UserName = "r3",
                    Password = "1",
                    SetPassword = true,
                    //同r2效果，不能操作xml中的dprms:file，也不写对象，因为写对象首先需要getreaderobject。
                    Rights = "setreaderinfo,getreaderinfo,writereaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                // 无法写简单xml，因为写权限大于读者权限。
                this.displayLine(GetBR() + getBold(u.UserName+"新建简单xml，应报权限配置不合理，因为有写对象权限，缺读对象权限。"));
                response = this.WriteXml(u, strResPath, this.GetReaderXml(false));
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


                #region 第4组测试 setreaderinfo,getreaderinfo,writereaderobject,getreaderobject
                //===
                this.displayLine(getLarge("第4组测试"));
                // setreaderinfo,getreaderinfo,writereaderobject,getreaderobject
                // 这是完整权限，即可读xml及操作里面的dprms:file，又可以写对象。
                u = new UserInfo
                {
                    UserName = "r4",
                    Password = "1",
                    SetPassword = true,

                    //可写读者xml，包括add/new/delete
                    //可操作xml中dprms:file，不能写对象
                    Rights = "setreaderinfo,getreaderinfo,writereaderobject,getreaderobject",
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
                this.displayLine(GetBR() + getBold(u.UserName+"新建带dprms:file的xml，应新建成功。"));
                response = this.WriteXml(u, strResPath, this.GetReaderXml(true));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 下面针对上面新建的这条进行修改和删除
                tempPath = response.strOutputResPath; 

                // 修改带file的xml
                this.displayLine(GetBR() + getBold(u.UserName + "修改带dprms:file的xml，应修改成功且NoError。"));
                response = this.WriteXml(u, tempPath, this.GetReaderXml(true));
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
                res = this.DelReader(u, tempPath);
                if (res.SetReaderInfoResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                #endregion

                #region 修改xml中的dprms:file,包括对dprms:file的new/change/delete

                // 再新建一条带dprms:file的记录，以便对其下级的dprms:file进行增/删/改
                this.displayLine(GetBR() + getBold(u.UserName + "再新建一条带dprms:file的记录，以便对其下级的dprms:file进行增/删/改。"));

                string originXml = this.GetReaderXml(true);
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
                    GetResResponse tempResponse= this.GetRes(u,tempPath);
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
                this.displayLine(GetBR() + getBold(u.UserName+"写对象数据，应成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(u);

                #endregion


                #region 第5组测试 writereaderobject

                //===
                this.displayLine(getLarge("第5组测试"));
                // writereaderobject
                // 不能操作xml中的dprms:file，也不能change对象数据，写对象需要先有读对象权限，且还需读写xml的权限
                u = new UserInfo
                {
                    UserName = "r5",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writereaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold(u.UserName+"写对象数据，由于没有连带的读对象权限 和 读写xml的权，应写不成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(u);

                #endregion


                #region 第6组测试  writereaderobject, getreaderobject

                //==
                this.displayLine(getLarge("第6组测试"));
                //writereaderobject, getreaderobject
                //不能修改对象数据，因为没有连带的读者xml权限
               u = new UserInfo
                {
                    UserName = "r6",
                    Password = "1",
                    SetPassword = true,

                    //能change对象数据，但不能操作xml中的dprms:file。
                    Rights = "writereaderobject, getreaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                this.displayLine(GetBR() + getBold(u.UserName+"修改对象数据，因为没有连带的读者xml权限，应不成功。"));
                response = this.WriteObject(u, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 修改xml及dprms:file,应不能成功。
                this.displayLine(GetBR() + getBold(u.UserName+"修改xml及dprms:file，由于没有读写xml的权限，应不成功。"));
                response = this.WriteXml(u, path_xmlHasFile, this.GetReaderXml(true));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(u);

                #endregion


                //=====以前为读权限=======

                #region 第7组测试 getreaderinfo

                //==
                this.displayLine(getLarge("第7组测试"));
                //getreaderinfo
                //仅能获取xml普通节点，不能获取对象。
                u = new UserInfo
                {
                    UserName = "r7",
                    Password = "1",
                    SetPassword = true,
                    Rights = "getreaderinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                //可获取xml，须过滤了dprms:file
                this.displayLine(GetBR() + getBold(u.UserName+"获取xml，应获取成功，且须过滤了dprms:file。"));
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

                #region 第8组测试 getreaderinfo,getreaderobject

                //===
                this.displayLine(getLarge("第8组测试"));
                //getreaderinfo,getreaderobject
                //可读取xml且包含dprms:file，可获取对象
                u = new UserInfo
                {
                    UserName = "r8",
                    Password = "1",
                    SetPassword = true,
                    Rights = "getreaderinfo,getreaderobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(u);

                //可获取xml，须过滤了dprms:file
                this.displayLine(GetBR() + getBold(u.UserName+"获取xml，应获取成功，且包含dprms:file。"));
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
                this.displayLine(GetBR() + getBold(u.UserName+"获取对象，应获取成功。"));
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

                #region 第9组测试 getreaderobject

                //===
                this.displayLine(getLarge("第9组测试"));
                //getreaderobject，可获取对象，不可读xml
                u= new UserInfo
                {
                    UserName = "r9",
                    Password = "1",
                    SetPassword = true,
                    Rights = "getreaderobject",
                    Access = ""
                };

                //创建帐号
                this.NewUser(u);

                //不能获取对象
                this.displayLine(GetBR() + getBold("r9获取对象，应不成功，由于对象权限不能独立存在。"));
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
                this.displayLine(GetBR() + getBold(u.UserName+"获取xml，由于没有读xml权限，应不成功。"));
                getResponse = this.GetRes(u, path_xmlHasFile);
                if (getResponse.GetResResult.Value ==-1)
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


        public string GetReaderXml(bool hasFile)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999999);
            string barcode = temp.ToString().PadLeft(6, '0'); //Guid.NewGuid().ToString().ToUpper(); //

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0' xmlns:dprms='http://dp2003.com/dprms'   test='"+temp.ToString()+"'/>";


            return "<root>"
                + "<barcode>P" + barcode+ "</barcode>"
                + "<name>小张"+barcode+"</name>"
                + "<readerType>本科生</readerType>"
                + dprmsfile
                + "</root>";
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

        public string GetBiblioXml(bool hasFile)
        {
            Random rd = new Random();
            int temp = rd.Next(1, 999);
            //string barcode = temp.ToString().PadLeft(6, '0'); //Guid.NewGuid().ToString().ToUpper(); //

            string dprmsfile = "";
            if (hasFile == true)
                dprmsfile = "<dprms:file id='0'    test='" + temp.ToString() + "'/>";


            string xml = @"<unimarc:record xmlns:dprms='http://dp2003.com/dprms' xmlns:unimarc='http://dp2003.com/UNIMARC'>
                                  <unimarc:leader>01887nam0 2200325   450 </unimarc:leader>
                                  <unimarc:datafield tag='200' ind1=' ' ind2=' '>"
                                   +"<unimarc:subfield code='a'>"+temp+"我爱我家</unimarc:subfield>"
                                  +"</unimarc:datafield>"
                                    + dprmsfile
                                    + "</unimarc:record>";
            return xml;
        }

        #endregion

        private void button_biblio_Click(object sender, EventArgs e)
        {

            this.EnableCtrls(false);
            try
            {
                // 清空输出
                ClearResult();

                string strResPath = "中文图书/?";
                WriteResResponse response = null;

                // 有用的读者xml路径
                string path_xmlHasFile = "";
                string path_object = "";

                //===
                this.displayLine(getLarge("第1组测试"));
                // r1_1  仅有setbiblioinfo
                UserInfo b1 = new UserInfo
                {
                    UserName = "b1",
                    Password = "1",
                    SetPassword = true,
                    Rights = "setbiblioinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(b1);

                this.displayLine(GetBR() + getBold("b1写简单xml，由于没有连带的读xml权限，应成功失败。"));
                response = this.WriteXml(b1, strResPath, this.GetBiblioXml(false));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                //删除帐号
                this.DelUser(b1);

                //===
                this.displayLine(getLarge("第2组测试"));
                // r1_2  setbiblioinfo, getbiblioinfo
                //可写简单读者xml，包括add/new/delete
                //不可操作xml中dprms:file，不能写对象
                //写的xml会忽略保护字段，单独测
                UserInfo b2 = new UserInfo
                {
                    UserName = "b2",
                    Password = "1",
                    SetPassword = true,
                    Rights = "setbiblioinfo,getbiblioinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(b2);

                // 可写简单xml
                this.displayLine(GetBR() + getBold("b2新建简单xml，有读写xml权限，应写成功。"));
                response = this.WriteXml(b2, strResPath, this.GetBiblioXml(false));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                string tempPath = response.strOutputResPath;
                // 修改简单xml
                this.displayLine(GetBR() + getBold("b2修改简单xml，应修改成功，且错误码应为NoError。"));
                response = this.WriteXml(b2, tempPath, this.GetBiblioXml(false));
                if (response.WriteResResult.Value == 0
                    && response.WriteResResult.ErrorCode == ErrorCode.NoError)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除xml
                this.displayLine(GetBR() + getBold("b2删除简单xml，应删除成功。"));
                SetReaderInfoResponse res = this.DelReader(b2, tempPath);
                if (res.SetReaderInfoResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));



                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold("b2新建带dprms:file的xml，由于没有对象权限，应部分写入且过滤了dprms:file(注意观察提示)。或者直接拒绝。"));
                response = this.WriteXml(b2, strResPath, this.GetBiblioXml(true));
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
                this.displayLine(GetBR() + getBold("b2写对象数据，由于没有对象权限，应不能成功。"));
                response = this.WriteObject(b2, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(b2);


                //===
                this.displayLine(getLarge("第3组测试"));
                // b3  setbiblioinfo, getbiblioinfo ,writebiblioobject 效果同b2，应该仅是写对象必须要有读对象才行。
                UserInfo b3 = new UserInfo
                {
                    UserName = "b3",
                    Password = "1",
                    SetPassword = true,
                    //同r1_2效果，不能操作xml中的dprms:file，也不写对象，因为写对象首先需要getreaderobject。
                    Rights = "setbiblioinfo, getbiblioinfo ,writebiblioobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(b3);

                // 可写简单xml
                this.displayLine(GetBR() + getBold("b3新建简单xml，应成功。"));
                response = this.WriteXml(b3, strResPath, this.GetBiblioXml(false));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold("b3新建带dprms:file的xml，由于没有连带的读对象权限，应部分写入且过滤了dprms:file，或直接拒绝。"));
                response = this.WriteXml(b3, strResPath, this.GetBiblioXml(true));
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
                this.displayLine(GetBR() + getBold("b3写对象，由于没有连带的读对象权限，应不能成功。"));
                response = this.WriteObject(b3, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(b3);

                //===
                this.displayLine(getLarge("第4组测试"));
                // b4 setbiblioinfo,getbiblioinfo,writebiblioobject,getbiblioobject
                UserInfo b4 = new UserInfo
                {
                    UserName = "b4",
                    Password = "1",
                    SetPassword = true,

                    //可写读者xml，包括add/new/delete
                    //可操作xml中dprms:file，不能写对象
                    Rights = "setbiblioinfo,getbiblioinfo,writebiblioobject,getbiblioobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(b4);

                // 可写简单xml
                this.displayLine(GetBR() + getBold("b4新建简单xml，应能成功。"));
                response = this.WriteXml(b4, strResPath, this.GetBiblioXml(false));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 操作xml中的dprms:file
                this.displayLine(GetBR() + getBold("b4新建带dprms:file的xml，应能成功。"));
                response = this.WriteXml(b4, strResPath, this.GetBiblioXml(true));
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                path_xmlHasFile = response.strOutputResPath;
                path_object = path_xmlHasFile + "/object/0";

                // todo change/delete
                // change时，对dprms:file的new/change/delete


                // 可写对象
                this.displayLine(GetBR() + getBold("r1_4写对象数据，应成功。"));
                response = this.WriteObject(b4, path_object);
                if (response.WriteResResult.Value == 0)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(b4);


                //===
                this.displayLine(getLarge("第5组测试"));
                // r1_5 writebiblioobject
                UserInfo b5 = new UserInfo
                {
                    UserName = "b5",
                    Password = "1",
                    SetPassword = true,

                    //不能操作xml中的dprms:file，也不能change对象数据，写对象需要先有getreaderobject。
                    Rights = "writebiblioobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(b5);

                // 不可操作xml中的dprms:file
                this.displayLine(GetBR() + getBold("r1_5写对象数据，由于没有连带的读对象权限，应不能成功。"));
                response = this.WriteObject(b5, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(b5);



                //==
                this.displayLine(getLarge("第6组测试"));
                //r1_6 writebiblioobject, getbiblioobject
                UserInfo b6 = new UserInfo
                {
                    UserName = "b6",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject, getbiblioobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(b6);

                this.displayLine(GetBR() + getBold("r1_6写对象数据，由于没有xml权限，不能成功。"));
                response = this.WriteObject(b6, path_object);
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 修改xml及dprms:file,应不能成功。
                this.displayLine(GetBR() + getBold("r1_6修改xml及dprms:file，由于没有读写xml的权限，应不能成功。"));
                response = this.WriteXml(b6, path_xmlHasFile, this.GetBiblioXml(true));
                if (response.WriteResResult.Value == -1)
                    this.displayLine("符合预期");
                else
                    this.displayLine(getRed("不符合预期"));

                // 删除帐号
                this.DelUser(b6);


                //==
                this.displayLine(getLarge("第7组测试"));
                //r1_7 getbiblioinfo

                //不能获取对象。
                UserInfo b7 = new UserInfo
                {
                    UserName = "b7",
                    Password = "1",
                    SetPassword = true,
                    Rights = "getbiblioinfo",
                    Access = ""
                };
                //创建帐号
                this.NewUser(b7);

                //可获取xml，须过滤了dprms:file
                this.displayLine(GetBR() + getBold("b7获取xml，应获取成功，且须过滤了dprms:file。"));
                GetResResponse getResponse = this.GetRes(b7, path_xmlHasFile);

                if (getResponse.GetResResult.Value >= 0)
                {
                    this.displayLine(getWarn1("注意检查是否过滤了dprms:file。"));

                    //this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //删除帐号
                this.DelUser(b7);


                //===
                this.displayLine(getLarge("第8组测试"));
                //b8 getbiblioinfo,gritebiblioobject 可读取xml且不会过滤dprms:file，可获取对象

                UserInfo b8 = new UserInfo
                {
                    UserName = "b8",
                    Password = "1",
                    SetPassword = true,
                    Rights = "getbiblioinfo,getbiblioobject",
                    Access = ""
                };
                //创建帐号
                this.NewUser(b8);

                //可获取xml，须过滤了dprms:file
                this.displayLine(GetBR() + getBold("b8获取xml，应获取成功，且不过滤dprms:file。"));
                getResponse = this.GetRes(b8, path_xmlHasFile);
                if (getResponse.GetResResult.Value >= 0)
                {
                    this.displayLine(getWarn1("注意检查结果中应有dprms:file。"));
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //可获取对象
                this.displayLine(GetBR() + getBold("b8获取对象，应获取成功。"));
                getResponse = this.GetRes(b8, path_object);
                if (getResponse.GetResResult.Value >= 0)
                {
                    this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //删除帐号
                this.DelUser(b8);


                //===
                this.displayLine(getLarge("第9组测试"));
                //b9 getbiblioobject，可获取对象，不可读xml
                UserInfo b9 = new UserInfo
                {
                    UserName = "b9",
                    Password = "1",
                    SetPassword = true,
                    Rights = "getbiblioobject",
                    Access = ""
                };

                //创建帐号
                this.NewUser(b9);

                //可获取xml，应不能获取
                this.displayLine(GetBR() + getBold("b9获取xml，应不成功。"));
                getResponse = this.GetRes(b9, path_xmlHasFile);
                if (getResponse.GetResResult.Value == -1)
                {
                    this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //应能获取对象
                this.displayLine(GetBR() + getBold("b9获取对象，应不成功，由于对象权限不能独立存在。"));
                getResponse = this.GetRes(b9, path_object);
                if (getResponse.GetResResult.Value == -1)
                {
                    this.displayLine("符合预期");
                }
                else
                {
                    this.displayLine(getRed("不符合预期"));
                }

                //删除帐号
                this.DelUser(b9);

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
    }
}
