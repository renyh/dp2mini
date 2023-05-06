using DigitalPlatform.CirculationClient;
using DigitalPlatform.IO;
using dp2mini;
using System.Reflection;

namespace dp2billing
{
    public partial class Form1 : Form
    {
        DbManager _dbManager;
        public Form1()
        {

            InitializeComponent();


            // MessageBox.Show(this, "network");
            string DataDir = Application.LocalUserAppDataPath;

            //DataDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string UserDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                    "dp2billing");
            PathUtil.TryCreateDir(UserDir);

            this._dbManager = new DbManager(UserDir);// Application.StartupPath);


        }

        private void button_add_Click(object sender, EventArgs e)
        {
            // ID，记帐日，记帐时间，帐户，
            // 产品类型，资源路径，交易金额，帐户余额，
            // 对方帐户，摘要，备注。

            string strAmount = this.textBox_amount.Text.Trim();
            decimal amount = 0;
            try
            {
                amount = Convert.ToDecimal(strAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "交易金额必须是数值型。" + ex.Message);
                return;
            }

            BillItem item = BillItem.NewItem(this.textBox_account.Text.Trim(),
                this.textBox_transactionType.Text.Trim(),
                this.textBox_resPath.Text.Trim(),
                amount,
                this.textBox_reciprocalAccount.Text.Trim(),
                "");

            ApiResult result = this._dbManager.AddBill(item);
            if (result.value == -1)
            {
                MessageBox.Show(this, "创建帐单失败：" + result.errorInfo);
                return;
            }

            MessageBox.Show(this, "成功创建帐单。");

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            //ClientInfo.Initial("dp2billing");
        }

        // 初始化数据库
        private void button_initDb_Click(object sender, EventArgs e)
        {
            try
            {
                this._dbManager.InitDb();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "初始化数据库失败：" + ex.Message);
                return;
            }

            MessageBox.Show(this, "初始化数据库成功");
        }

        #region 给界面设置样例数据

        // 充值
        private void button_recharge_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_transactionType.Text = DbManager.C_TransactionType_充值;
            this.textBox_resPath.Text = "";
            this.textBox_amount.Text = "50";
            this.textBox_reciprocalAccount.Text = "a";
        }


        // 下载数据
        private void button_auto_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_transactionType.Text = DbManager.C_TransactionType_下载数据;
            this.textBox_resPath.Text = "中文图书/1";
            this.textBox_amount.Text = "-1";
            this.textBox_reciprocalAccount.Text = "b";
        }

        // 购买产品
        private void button_buy_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_transactionType.Text = DbManager.C_TransactionType_购买产品;
            this.textBox_resPath.Text = "购买MARC脚本1";
            this.textBox_amount.Text = "-10";
            this.textBox_reciprocalAccount.Text = "数字平台";
        }



        // 提现
        private void button_withdraw_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_transactionType.Text = DbManager.C_TransactionType_提现;
            this.textBox_resPath.Text = "";
            this.textBox_amount.Text = "-10";
            this.textBox_reciprocalAccount.Text = "a";
        }

        #endregion


        // 获取帐单
        private void button_getBills_Click(object sender, EventArgs e)
        {
            //时间范围
            string startDate = this.dateTimePicker_start.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string endDate = this.dateTimePicker_end.Value.ToString("yyyy-MM-dd")+" 23:59:59";

            // 帐号
            string account = this.textBox_account.Text.Trim();

            // 先清空结果区
            this.textBox_result.Text = "";
            List<BillItem> items = this._dbManager.GetBills(startDate, endDate,account);
            if (items.Count == 0)
            {
                this.textBox_result.Text = "未命中";
            }
            else
            {
                this.textBox_result.Text = "命中"+items.Count+"条，时间范围:"+startDate+"-"+endDate+",帐户:"+account+"\r\n";
            }

            foreach (BillItem item in items)
            {
                this.textBox_result.Text += item.Dump() + "\r\n";
                Application.DoEvents();
            }

        }


        // 获取下载量
        private void button_getDownloadCount_Click(object sender, EventArgs e)
        {
            //时间范围
            string startDate = this.dateTimePicker_start.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string endDate = this.dateTimePicker_end.Value.ToString("yyyy-MM-dd") + " 23:59:59";

            // 帐号
            string account = this.textBox_account.Text.Trim();
            if (string.IsNullOrEmpty(account) == true)
            {
                MessageBox.Show(this, "尚未输入帐号");
                return;
            }

            int count = this._dbManager.GetDownloadCount(startDate, endDate, account);
            this.textBox_result.Text += "\r\n帐户" + account + "的下载量为[" + count + "],时间范围: " + startDate + " - " + endDate;
        }
    }
}