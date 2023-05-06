using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dp2mini
{
    public class BillItem
    {
        public BillItem()
        { }

        // ID，记帐时间，帐户，
        // 产品类型，资源路径，交易金额，帐户余额，
        // 对方帐户，摘要，备注。

        public static BillItem NewItem(string account,
            string transactionType,
            string resPath,
            decimal amount,
            string reciprocalAccount,
            string remark)
        {
            BillItem item = new BillItem();
            item.Id=Guid.NewGuid().ToString();
            //item.CreateDate= DateTime.Now.ToString("yyyy-MM-dd");
            item.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            item.Account = account; //这个金额会有正负两种情况

            item.TransactionType = transactionType;
            item.ResPath = resPath;
            item.Amount = amount;
            item.Balance = 0; //todo这个值需要计算出来

            item.ReciprocalAccount = reciprocalAccount;
            item.Summary = "";//这个值暂不使用
            item.remark = remark;

            return item;
        }



        // 记录id,guid
        public string Id { get; set; }

        // 记帐日,格式为2023/04/28
        //这个字段冗余，无意义
        //public string CreateDate { get; set; }

        // 记帐时间，带日期和时间的完整格式，2023/04/28 09:38:11
        public string CreateTime { get; set; }

        // 帐户
        public string Account { get; set; }

        // 交易类型，目前支持4种类型：下载数据，购买产品，充值，提现
        public string TransactionType { get; set; }

        // 资源路径
        public string ResPath { get; set; }

        // 交易金额
        public decimal Amount { get; set; }

        // 帐户余额
        public decimal Balance { get; set; }


        // 对方帐户，针对购买脚本，可以使用一个数字平台的帐户
        public string ReciprocalAccount { get; set; }

        // 摘要，比较产品更详细一些
        public string Summary { get; set; }

        // 备注
        public string remark { get; set; }


        // ID，记帐时间，帐户，
        // 产品类型，资源路径，交易金额，帐户余额，
        // 对方帐户，摘要，备注。
        public string Dump()
        {
            return Id + "\t"
                + this.CreateTime + "\t"
                + this.Account + "\t"

                + this.TransactionType + "\t"
                + this.ResPath + "\t"
                + this.Amount + "\t"
                + this.Balance + "\t"

                + this.ReciprocalAccount + "\t"
                + this.Summary + "\t"
                + this.remark;

        }
    }
}
