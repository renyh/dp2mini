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
    public partial class Form_WriteRes_Chunk : Form
    {
        public Form_WriteRes_Chunk()
        {
            InitializeComponent();
        }



        private void button_ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ChunkSizeStr) == true)
            {
                MessageBox.Show(this, "请输入小包尺寸。");
                return;
            }

            try
            {
                this.ChunkSize = Convert.ToInt32(this.ChunkSizeStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "小包尺寸格式不合法，须为数值型。");
                return;
            }


            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int ChunkSize = 0;

        public string ChunkSizeStr
        {
            get
            {
                return this.textBox_WriteRes_chunkSize.Text.Trim();
            }
            set
            {
                this.textBox_WriteRes_chunkSize.Text = value;
            }
        }
        
        public string Info
        {
            get {
            
                return this.label_info.Text;
            }
            set
            {
                this.label_info.Text = value;
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
