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
            // ID�������գ�����ʱ�䣬�ʻ���
            // ��Ʒ���ͣ���Դ·�������׽��ʻ���
            // �Է��ʻ���ժҪ����ע��

            string strAmount = this.textBox_amount.Text.Trim();
            decimal amount = 0;
            try
            {
                amount = Convert.ToDecimal(strAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "���׽���������ֵ�͡�" + ex.Message);
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
                MessageBox.Show(this, "�����ʵ�ʧ�ܣ�" + result.errorInfo);
                return;
            }

            MessageBox.Show(this, "�ɹ������ʵ���");

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            //ClientInfo.Initial("dp2billing");
        }

        // ��ʼ�����ݿ�
        private void button_initDb_Click(object sender, EventArgs e)
        {
            try
            {
                this._dbManager.InitDb();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "��ʼ�����ݿ�ʧ�ܣ�" + ex.Message);
                return;
            }

            MessageBox.Show(this, "��ʼ�����ݿ�ɹ�");
        }

        #region ������������������

        // ��ֵ
        private void button_recharge_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_transactionType.Text = DbManager.C_TransactionType_��ֵ;
            this.textBox_resPath.Text = "";
            this.textBox_amount.Text = "50";
            this.textBox_reciprocalAccount.Text = "a";
        }


        // ��������
        private void button_auto_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_transactionType.Text = DbManager.C_TransactionType_��������;
            this.textBox_resPath.Text = "����ͼ��/1";
            this.textBox_amount.Text = "-1";
            this.textBox_reciprocalAccount.Text = "b";
        }

        // �����Ʒ
        private void button_buy_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_transactionType.Text = DbManager.C_TransactionType_�����Ʒ;
            this.textBox_resPath.Text = "����MARC�ű�1";
            this.textBox_amount.Text = "-10";
            this.textBox_reciprocalAccount.Text = "����ƽ̨";
        }



        // ����
        private void button_withdraw_Click(object sender, EventArgs e)
        {
            this.textBox_account.Text = "a";
            this.textBox_transactionType.Text = DbManager.C_TransactionType_����;
            this.textBox_resPath.Text = "";
            this.textBox_amount.Text = "-10";
            this.textBox_reciprocalAccount.Text = "a";
        }

        #endregion


        // ��ȡ�ʵ�
        private void button_getBills_Click(object sender, EventArgs e)
        {
            //ʱ�䷶Χ
            string startDate = this.dateTimePicker_start.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string endDate = this.dateTimePicker_end.Value.ToString("yyyy-MM-dd")+" 23:59:59";

            // �ʺ�
            string account = this.textBox_account.Text.Trim();

            // ����ս����
            this.textBox_result.Text = "";
            List<BillItem> items = this._dbManager.GetBills(startDate, endDate,account);
            if (items.Count == 0)
            {
                this.textBox_result.Text = "δ����";
            }
            else
            {
                this.textBox_result.Text = "����"+items.Count+"����ʱ�䷶Χ:"+startDate+"-"+endDate+",�ʻ�:"+account+"\r\n";
            }

            foreach (BillItem item in items)
            {
                this.textBox_result.Text += item.Dump() + "\r\n";
                Application.DoEvents();
            }

        }


        // ��ȡ������
        private void button_getDownloadCount_Click(object sender, EventArgs e)
        {
            //ʱ�䷶Χ
            string startDate = this.dateTimePicker_start.Value.ToString("yyyy-MM-dd") + " 00:00:00";
            string endDate = this.dateTimePicker_end.Value.ToString("yyyy-MM-dd") + " 23:59:59";

            // �ʺ�
            string account = this.textBox_account.Text.Trim();
            if (string.IsNullOrEmpty(account) == true)
            {
                MessageBox.Show(this, "��δ�����ʺ�");
                return;
            }

            int count = this._dbManager.GetDownloadCount(startDate, endDate, account);
            this.textBox_result.Text += "\r\n�ʻ�" + account + "��������Ϊ[" + count + "],ʱ�䷶Χ: " + startDate + " - " + endDate;
        }
    }
}