using DigitalPlatform.CirculationClient;
using dp2mini;

namespace dp2billing
{
    public partial class Form1 : Form
    {
        DbManager _dbManager;
        public Form1()
        {

            InitializeComponent();

            this._dbManager = new DbManager(Application.StartupPath);


        }

        private void button_add_Click(object sender, EventArgs e)
        {
            // ID，记帐日，记帐时间，帐户，
            // 产品类型，资源路径，交易金额，帐户余额，
            // 对方帐户，摘要，备注。

            string strAmount = this.textBox_amount.Text.Trim();
            double amount = 0;
            try
            {
                amount = Convert.ToDouble(strAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "交易金额必须是数值型。" + ex.Message);
                return;
            }

            BillItem item = BillItem.NewItem(this.textBox_account.Text.Trim(),
                this.textBox_productType.Text.Trim(),
                this.textBox_resPath.Text.Trim(),
                amount,
                this.textBox_reciprocalAccount.Text.Trim());

           this._dbManager.SetItem("add", item);

            MessageBox.Show(this, "完成");


        }

        private void button_change_Click(object sender, EventArgs e)
        {
            string strAmount = this.textBox_amount.Text.Trim();
            double amount = 0;
            try
            {
                amount = Convert.ToDouble(strAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "交易金额必须是数值型。" + ex.Message);
                return;
            }

            BillItem item = new BillItem();
            item.Id = this.textBox_id.Text.Trim();
            item.Account = this.textBox_account.Text.Trim();
            item.ProductType = this.textBox_productType.Text.Trim();
            item.ResPath = this.textBox_resPath.Text.Trim();
            item.Amount = amount;
            item.ReciprocalAccount = this.textBox_reciprocalAccount.Text.Trim();

            this._dbManager.SetItem("change", item);
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            this._dbManager.SetItem("delete", new BillItem() { Id = this.textBox_id.Text.Trim() });
        }

        private void button_auto_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_productType.Text = "下载marc";
            this.textBox_resPath.Text = "中文图书/1";
            this.textBox_amount.Text = "1";
            this.textBox_reciprocalAccount.Text = "b";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ClientInfo.Initial("dp2billing");
        }
    }
}