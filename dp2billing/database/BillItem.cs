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

        // ID，记帐日，记帐时间，帐户，
        // 产品类型，资源路径，交易金额，帐户余额，
        // 对方帐户，摘要，备注。

        public static BillItem NewItem(string account,
            string productType,
            string resPath,
            double amount,
            string reciprocalAccount)
        {
            BillItem item = new BillItem();
            item.Id=Guid.NewGuid().ToString();
            item.CreateDate= DateTime.Now.ToString("yyyy-MM-dd");
            item.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            item.Account = account; //这个金额会有正负两种情况

            item.ProductType = productType;
            item.ResPath = resPath;
            item.Amount = amount;
            item.Balance = 0; //todo这个值需要计算出来

            item.ReciprocalAccount = reciprocalAccount;
            item.Summary = "";//这个值暂不使用
            item.remark = "";//这个值暂不使用

            return item;
        }



        // 记录id,guid
        public string Id { get; set; }

        // 记帐日,格式为2023/04/28
        public string CreateDate { get; set; }

        // 记帐时间，带日期和时间的完整格式，2023/04/28 09:38:11
        public string CreateTime { get; set; }

        // 帐户
        public string Account { get; set; }

        // 产品类型，例如：下载marc数据，购买脚本
        public string ProductType { get; set; }

        // 资源路径
        public string ResPath { get; set; }

        // 交易金额
        public double Amount { get; set; }

        // 帐户余额
        public double Balance { get; set; }


        // 对方帐户，针对购买脚本，可以使用一个数字平台的帐户
        public string ReciprocalAccount { get; set; }

        // 摘要，比较产品更详细一些
        public string Summary { get; set; }

        // 备注
        public string remark { get; set; } 
    }
}
