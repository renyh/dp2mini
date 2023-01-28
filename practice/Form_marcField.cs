using common;
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
    public partial class Form_marcField : Form
    {
        public Form_marcField()
        {
            InitializeComponent();
        }

        private void button_getField_Click(object sender, EventArgs e)
        {
            string strFieldMap = this.textBox_map.Text.Trim();
            if (string.IsNullOrEmpty(strFieldMap) == true)
            {
                MessageBox.Show(this, "尚未配置字段提取规则。");
                return;
            }

            //
            string strMarc = this.textBox_biblio.Text.Trim();
            if (string.IsNullOrEmpty(strMarc) == true)
            {
                MessageBox.Show(this, "尚未设置marc数据。");
                return;
            }

            // 显示在界面上
            this.textBox_result.Text = MarcHelper.GetFields(strMarc, strFieldMap);

        }

        private void button_setField_Click(object sender, EventArgs e)
        {
            string strFieldMap = this.textBox_map.Text.Trim();
            if (string.IsNullOrEmpty(strFieldMap) == true)
            {
                MessageBox.Show(this, "尚未配置字段提取规则。");
                return;
            }
            List<FieldItem> fieldList = null;
            try
            {
                fieldList = MarcHelper.ParseFieldMap(strFieldMap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                return;
            }

            //
            string strMarc = this.textBox_biblio.Text.Trim();
            if (string.IsNullOrEmpty(strMarc) == true)
            {
                MessageBox.Show(this, "尚未设置marc数据。");
                return;
            }

            string marc = MarcHelper.SetFields(strMarc, fieldList);

            this.textBox_result.Text = marc;


        }
    }
}
