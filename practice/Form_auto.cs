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

namespace practice
{
    public partial class Form_auto : Form
    {
        public Form_auto(Form_main main)
        {
            InitializeComponent();

            this.mainForm = main;
        }

        Form_main mainForm = null;

        // 存取定义与字段
        private void button_accessAndObject_Click(object sender, EventArgs e)
        {
            // 创建5个临时帐户
            UserInfo u_a1 = new UserInfo();
            UserInfo u_a1_1 = new UserInfo();
            UserInfo u_a2 = new UserInfo();
            UserInfo u_a3 = new UserInfo();
            UserInfo u_a4 = new UserInfo();
            SetUserResponse response = null;
            WriteResResponse writeResResponse = null;
            string strBiblioPath = "";

            RestChannel channelSuper = null;
            try
            {
                // 用超级管理员帐户登录
                channelSuper = mainForm.GetSuprevisorChannel();// this._channelPool.GetChannel(this.ServerUrl, supervisorName);


                // 创建帐户a1 重点change，指定字段
                u_a1 = new UserInfo
                {
                    UserName = "a1",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=change|getbiblioinfo=*"
                };
                response = channelSuper.SetUser("new", u_a1);
                this.displayLine("创建帐户a1");

                u_a1_1 = new UserInfo
                {
                    UserName = "a1_1",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=change(606)|getbiblioinfo=*"
                };

                response = channelSuper.SetUser("new", u_a1_1);
                this.displayLine("创建帐户a1_1");

                // 创建帐户a2  new，指定字段，应不能写对象
                u_a2 = new UserInfo
                {
                    UserName = "a2",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=new|getbiblioinfo=*"
                };
                response = channelSuper.SetUser("new", u_a2);
                this.displayLine("创建帐户a2");

                // 创建帐户a3  delete，指定字段,应不能写对象
                u_a3 = new UserInfo
                {
                    UserName = "a3",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=delete|getbiblioinfo=*"
                };
                response = channelSuper.SetUser("new", u_a3);
                this.displayLine("创建帐户a3");


                // 创建帐户a4 通配符*表示new,change,and，应能写对象
                u_a4 = new UserInfo
                {
                    UserName = "a4",
                    Password = "1",
                    SetPassword = true,
                    Rights = "writebiblioobject,getbiblioobject",
                    Access = "中文图书:setbiblioinfo=*|getbiblioinfo=*"
                };
                response = channelSuper.SetUser("new", u_a4);
                this.displayLine("创建帐户a4");



                // 先用管理员身从那写一条书目记录
                // 准备带dprms:file的xml
                string xml = @"<unimarc:record xmlns:dprms='http://dp2003.com/dprms' xmlns:unimarc='http://dp2003.com/UNIMARC'>
                                  <unimarc:leader>01887nam0 2200325   450 </unimarc:leader>
                                  <unimarc:datafield tag='200' ind1=' ' ind2=' '>
                                    <unimarc:subfield code='a'>我爱我家</unimarc:subfield>
                                  </unimarc:datafield>
                                  <dprms:file id='0'/>
                                </unimarc:record>";
                byte[] baContent_xml = Encoding.UTF8.GetBytes(xml);  //文本到二进制
                long lTotalLength_xml = baContent_xml.Length;
                string strRanges_xml = "0-" + (baContent_xml.Length - 1).ToString();
                string strResPath_xml = "中文图书/?";
                // 写xml
                writeResResponse = channelSuper.WriteRes(strResPath_xml,
                    strRanges_xml,
                    lTotalLength_xml,
                    baContent_xml,
                    "",
                    "",
                    null);

                strBiblioPath = writeResResponse.strOutputResPath;

                this.displayLine("<br/>用管理员身份创建一条书目记录。"+strBiblioPath+"\r\n"
                    + RestChannel.GetResultInfo(writeResResponse));

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "创建帐户异常：" + ex.Message);
                return;
            }
            finally
            {
                if (channelSuper != null)
                    this.mainForm._channelPool.ReturnChannel(channelSuper);
            }

            this.displayLine("以下是不同帐户测试存取定义设了setbiblioinfo权限，是否可以修改对象。");


            // 一个图片对象
            string hexObject = "ffd8ffe000104a4649460001010100d800d80000ffdb004300080606070605080707070909080a0c140d0c0b0b0c1912130f141d1a1f1e1d1a1c1c20242e2720222c231c1c2837292c30313434341f27393d38323c2e333432ffdb0043010909090c0b0c180d0d1832211c213232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232ffc00011080043007803012200021101031101ffc4001f0000010501010101010100000000000000000102030405060708090a0bffc400b5100002010303020403050504040000017d01020300041105122131410613516107227114328191a1082342b1c11552d1f02433627282090a161718191a25262728292a3435363738393a434445464748494a535455565758595a636465666768696a737475767778797a838485868788898a92939495969798999aa2a3a4a5a6a7a8a9aab2b3b4b5b6b7b8b9bac2c3c4c5c6c7c8c9cad2d3d4d5d6d7d8d9dae1e2e3e4e5e6e7e8e9eaf1f2f3f4f5f6f7f8f9faffc4001f0100030101010101010101010000000000000102030405060708090a0bffc400b51100020102040403040705040400010277000102031104052131061241510761711322328108144291a1b1c109233352f0156272d10a162434e125f11718191a262728292a35363738393a434445464748494a535455565758595a636465666768696a737475767778797a82838485868788898a92939495969798999aa2a3a4a5a6a7a8a9aab2b3b4b5b6b7b8b9bac2c3c4c5c6c7c8c9cad2d3d4d5d6d7d8d9dae2e3e4e5e6e7e8e9eaf2f3f4f5f6f7f8f9faffda000c03010002110311003f005f30f1d3a0eded4bbc839e307af02abf53c93d054a8ab9e80fd69202c4730cf55fc055a494f65ffc745544f978fc8fad598cd319691e43fdc1f866aaea52dedbdb33da143210480c80827d2aca5492279b0b2f7ea3eb4981c8c3ad5f5d46923cf218d8ec6d910501bf1a86ff005386c032df30495588db23649fc8547a9dc4da6fdb5a657fb39fdf0c108ad8cf1d3af6ae059afbc417ed34ae5998e49cf007a0a9bf7292bbb23a0b8f15c20b88ad818f3c3aa283fae6bb0f0e7c48d1fecb0da5e2cb6aea02f98e8193a63248e47e558fa47816cef2d544adb988e70fd0d6837c28491de386e1a2761f2331f949f4353ed23735f613b5cf49b6b88ee2249a178e48dc6e57400861ea0d5b0c71d17fef915e59e169efbc21a9cba66a7bd6d4b0dc8c3fd53138dc3d8f19fcebd4d7a715aa316ada12073dc2ff00df2292176d8bc2f3cfdd1de9b210b1373ce314f42a7001071e94c92e5b3b79a9c2fde1fc228a20e244c2b751da8a04792679fc054f1b557279c8049c0fc69f1b13d01e3ae6a465d5e463f23e9534679f71d45568c9f6153ec63f36e391d87714319710d58438ea7154a3c7a93f8d598c2fa0a434703f12d5122b351210b2c8d9c1e30067f9d51d1ac8c70abdadb2ba3f31472360631d58f526b4be28a6f8b48e783248bf9edab5a5ba5b1db81c0c0cd44df43a2842edb2ec17315b5ddba3d8c569395fde2c323346c723079e477c8aeb7c44d751a5ab5ad85bb4cfc843315423b60fd2b82d4a3bcbbbe5fb3bc31282a0cb29c81ebc57a6d95aea72e8a0cd2daea16502ab46aab89100fbfce7e607a8e9c5667472e9b991e31b2b14d260d4b54865b4125b326636f344328202807b87cf43dc76ae86d151ad2120960517049ebc56a78874cb6d4be1e6a36a0008d68d221eb8651bd4fe6056368a59b45b267fbc615cfe55d11d8e1a9b978c2a42e140f9b3d3d2a741e9516f5f300dc381eb52a11fe455105a8173227fbc28a75b7fad4383f785141363c6fb8fa0a91573c8eb4c20e460f61fcaa44ce6a0a278cd5946aac141fad381db4c0b5b829c82307f4ab11b5558dc1183d2a656c1c134868c0f1ce9726a1a7dadca8256d5d8b8ff006580e7f303f3acc46dbb58fa5774a5590ab00ca46083c822b90d7e08ecb5129145b21740e981c7a103fcf7acea2ea75e1e493e530e6899e5cbccc501e141c57a7783552f34b905bcd79a7dc451958e557dc98c7756eb5e6865403af22bd27c11e2cb54b55b3ba500f4c9f4ace2cea95f96c75ba0892efe1d4f0c92e2578658989fe1ea303daa2b58120b78a150311a051f80a945d243606d61428257de4118f97a8fcfad3118fa8fcaba21b1e6d57ef3245fbec40ef8a9e3aab1b65412dd79ab0846475fceacccbd170f18ee585145bec6954e01e4628a093c736b03d3b0a95549ab2d0ed23a741fcaa29a193cbfdccca8c3fbc0106b3b96380c521193d4e6a8f9979921f72e0f3c8c7e07b8a63ea4220a1d096242861f749345c2c69c6706a65963762a1d0baf604645508e396ef23cc0a47f02f159f79a0ca6413dacaf6d74bf72453c673d08f434f52923a37bb8ada079a67db1a0cb1c74ac0f106ab617f6f0c504e1e547cedc1fba47ff00aab32fb5fd4b12d9cb1451b152927c9c90460f15ce69d633da29cc4ccd9e5d79047b7a7d2a24f435a7171926cd4f2d1e400f735eadf0ebc2b604bde5c0f38a91b437415e65616ed73202aad953ce057aff0085af459695e4b3619bd7bd6314754e5756449e29d6ec6c3c413c534bb6448918a85edb7354ed75ab5bbdab1bba348309e6260313db3eb583e3bb3b7d52e2e352b96fb3dc2c491c4cadcfcb9e4fd41c62a9f862216f0dac8f334ae1777cc7ee9ef5d09bbd8e39534a373bef2e6419314b81df61a69b855c02d866e9934df0e6bb346935acd292aae793d429ae7eefc636e1f50b6452c6572ecce73e5aa02491ee718fcab47b18d8ecec668f720565dd918c9a2bc32d7c5daf5edfc57763235bc6ac705c82a07a723e6a2b3722953b9ac92c8f7112b3b1564c919fa5694b6d122ee55c305c83939eb451534f707b1144048595c065cf4233d8d3deceddec4c6631b08ce01206474e94515b12577765b68a60c448d6a5cb67a9c75ae9b4a45bab0479943b30e49ef4514219cbf8d6cade3104e910597637cc09cf0462b9764536f1c98c391c91c5145672dceba3f0a34b4324c9202cc73d72c6bd2b44b788daa929ce0739a28a70d8b96e71de3525afd2224f960e76e6a3d3490fc13c7bd1452fb66753e0475da6468d22b11c95e4e6b8496245d7ef005e0a3ff35a28aa7d0e68f530ac915628540c00a3028a28ae67b9dab63fffd9";
            byte[] baContent_object = ByteArray.GetTimeStampByteArray(hexObject);
            long lTotalLength_object = baContent_object.Length;
            string strRanges_object = "0-" + (baContent_object.Length - 1).ToString();
            string objectPath = strBiblioPath + "/object/0";

            byte[] baTimestamp = null;

            long lRet = 0;
            // 用u_a1帐户，写xml
            lRet=this.WriteObject(u_a1,objectPath,strRanges_object,
                lTotalLength_object,baContent_object,ref baTimestamp);
            if (lRet == 0)
                this.displayLine("符合预期");
            else
                this.displayLine(getWarn("不符合预期"));



            // 用u_a1_1帐户，写xml
            lRet = this.WriteObject(u_a1_1, objectPath, strRanges_object,
                lTotalLength_object, baContent_object, ref baTimestamp);
            if (lRet == 0)
                this.displayLine("符合预期");
            else
                this.displayLine(getWarn("不符合预期"));

            // 用u_a2帐户，写xml
            lRet = this.WriteObject(u_a2, objectPath, strRanges_object,
                lTotalLength_object, baContent_object, ref baTimestamp);
            if (lRet == -1)
                this.displayLine("符合预期");
            else
                this.displayLine(getWarn("不符合预期"));

            // 用u_a3帐户，写xml
            lRet = this.WriteObject(u_a3, objectPath, strRanges_object,
                lTotalLength_object, baContent_object, ref baTimestamp);
            if (lRet == -1)
                this.displayLine("符合预期");
            else
                this.displayLine(getWarn("不符合预期"));

            // 用u_a4帐户，写xml
            lRet = this.WriteObject(u_a4, objectPath, strRanges_object,
                lTotalLength_object, baContent_object, ref baTimestamp);
            if (lRet == 0)
                this.displayLine("符合预期");
            else
                this.displayLine(getWarn("不符合预期"));



            // 
            // 删除帐户
            channelSuper = null;
            try
            {
                // 用超级管理员帐户登录
                channelSuper = mainForm.GetSuprevisorChannel();// this._channelPool.GetChannel(this.ServerUrl, supervisorName);

                channelSuper.SetUser("delete", u_a1);
                this.displayLine("删除帐户a1");

                channelSuper.SetUser("delete", u_a1_1);
                this.displayLine("删除帐户a1_1");

                channelSuper.SetUser("delete", u_a2);
                this.displayLine("删除帐户a2");

                channelSuper.SetUser("delete", u_a3);
                this.displayLine("删除帐户a3");

                channelSuper.SetUser("delete", u_a4);
                this.displayLine("删除帐户a4");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "删除帐户异常：" + ex.Message);
                return;
            }
            finally
            {
                if (channelSuper != null)
                    this.mainForm._channelPool.ReturnChannel(channelSuper);
            }

        }

        public string getWarn(string text)
        {
            return "<span style='color: red; font-weight:bold'>" + text + "</span>";
        }


        public long WriteObject(UserInfo u,
          string objectPath,
          string strRanges_object,
          long lTotalLength_object,
          byte[] baContent_object,
            ref byte[] baTimestamp)
        {
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
                this.displayLine("<br/>帐户=" + u.UserName + "<br/>"
                    + "Rights=" + u.Rights + "<br/>"
                    + "Access=" + u.Access + "<br/>"
                    + RestChannel.GetResultInfo(response));

                // 时间戳
                baTimestamp = response.baOutputTimestamp;

                return response.WriteResResult.Value;

            }
            catch (Exception ex)
            { 
                throw new Exception( u.UserName + "异常：" + ex.Message);
            }
            finally
            {
                if (channel != null)
                    this.mainForm._channelPool.ReturnChannel(channel);
            }
        }

        public void displayLine(string line)
        {
            line = line.Replace("\r\n","<br/>");

            AppendHtml(this.webBrowser1, line + "<br/>");
        }

        public void ClearResult()
        {
            this.webBrowser1.DocumentText = "<html><body></body></html>";

            //this.textBox_result.Text = "";
        }

        private void Form_auto_Load(object sender, EventArgs e)
        {

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
    }
}
