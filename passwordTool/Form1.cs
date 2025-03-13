using DigitalPlatform.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace passwordTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region 密码加密

        // 解密
        //static string EncryptKey = "dp2_key";
        internal static string DecryptPasssword(string strEncryptedText, string encryptKey)
        {
            if (String.IsNullOrEmpty(strEncryptedText) == true)
                return "";


            try
            {
                string strPassword = Cryptography.Decrypt(
                    strEncryptedText,
                    encryptKey);
                return strPassword;
            }
            catch
            {
                return "errorpassword";
            }
        }

        // 加密
        internal static string EncryptPassword(string strPlainText, string encryptKey)
        {
            return Cryptography.Encrypt(strPlainText, encryptKey);
        }

        #endregion

        // 加密
        private void button_encrypt_Click(object sender, EventArgs e)
        {
            string source = this.textBox_source.Text.Trim();
            //if (source == "")
            //{
            //    MessageBox.Show(this, "尚未输入明文");
            //    return;
            //}

            string key = this.textBox_key.Text.Trim();
            if (key == "")
            {
                MessageBox.Show(this, "尚未输入key");
                return;
            }
            this.textBox_target.Text = EncryptPassword(source, key);
        }

        // 解密
        private void button_decrypt_Click(object sender, EventArgs e)
        {
            string target = this.textBox_target.Text.Trim();
            //if (source == "")
            //{
            //    MessageBox.Show(this, "尚未输入明文");
            //    return;
            //}

            string key = this.textBox_key.Text.Trim();
            if (key == "")
            {
                MessageBox.Show(this, "尚未输入key");
                return;
            }

            this.textBox_source.Text = DecryptPasssword(target, key);
        }


    }
}
