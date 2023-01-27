using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practice
{
    public partial class Form_editBaContent : Form
    {
        public Form_editBaContent()
        {
            InitializeComponent();
        }



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

               this.FileName = dlg.FileName;


                // 在主界面做吧
                //// 获取minitype
                //string minitype = PathUtil.MimeTypeFrom(localPath);

                //// 组成metadata xml字符串
                //this.textBox_WriteRes_strMetadata.Text = BuildMetadata(minitype, localPath);

            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(this.Conent) == false
                && string.IsNullOrEmpty(this.FileName) == false)
            {
                MessageBox.Show(this, "文本 和 文件名 同时存在，二选一，请清除一下。");
                return;
            }


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string Ranges
        {
            get
            {
                return this.textBox_WriteRes_strRanges.Text.Trim();
            }
            set
            {
                this.textBox_WriteRes_strRanges.Text = value;
            }
        }

        public string Conent
        {
            get
            {
                return this.textBox_content.Text.Trim();
            }
            set
            {
                this.textBox_content.Text = value;
            }
        }
        public string FileName
        {
            get
            {
                return this.textBox_fileName.Text.Trim();
            }
            set
            {
                this.textBox_fileName.Text = value;
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
