using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dp2mini
{
    public class DbManager
    {
        //#region 单一实例

        //static DbManager _instance;
        //private DbManager()
        //{
        //    this.ConnectionDb();
        //}
        //private static object _lock = new object();
        //static public DbManager Instance
        //{
        //    get
        //    {
        //        if (null == _instance)
        //        {
        //            lock (_lock)  //线程安全的
        //            {
        //                _instance = new DbManager();
        //            }
        //        }
        //        return _instance;
        //    }
        //}
        //#endregion

        ////声明事件
        //public event AddNoteDelegate AddNoteHandler;


        // 数据库对象
        BillDB _dbclient = null;


        string _dataDir = "";
        string _dbFile = "";
        public DbManager(string dataDir)
        {
            this._dataDir = dataDir;


            this._dbFile = this._dataDir + "/db.sqlite";
            this.ConnectionDb(this._dbFile);
        }

        // 连接数据库
        public void ConnectionDb(string dbFile)
        {
            this._dbclient = new BillDB(dbFile);
            //Create the database file at a path defined in SimpleDataStorage
            this._dbclient.Database.EnsureCreated();
            //Create the database tables defined in SimpleDataStorage
            this._dbclient.Database.Migrate();
        }

        // 初始化数据库
        public void InitDb()
        {
            
            if (File.Exists(this._dbFile))
            {
                //因为文件被占用，不能直接删除数据库文件
                //File.Delete(this._dbFile);

                // 删除全部记录
                this._dbclient.Items.Where(b=>true).ExecuteDelete();

            }
        }


        // 新增帐单
        // 返回一个结果对象
        public ApiResult AddBill(BillItem item)
        {
            ApiResult result = new ApiResult();

            // 校验一下输入参数
            if (item==null || string.IsNullOrEmpty(item.Account)==true)
            {
                result.value = -1;
                result.errorInfo = "item参数不正确。";
                goto ERROR1;
            }

            // 如果是消费记录，余额不足，不能新增
            if (item.Amount < 0)
            {
                decimal balance = this.GetBalanceForAccount(item.Account);
                if (balance + item.Amount<0) {
                    result.value = -1;
                    result.errorInfo = "余额不足";
                    goto ERROR1;
                }
            }

            // 保存当前帐户的记录到底层数据库
            int nRet = this.AddBillInternal(item, out string error);
            if (nRet == -1)
            {
                result.value = -1;
                result.errorInfo = error;
                goto ERROR1;
            }


            // 如果金额是负数，并且当前帐户与对方帐户不同，则同时给对方帐户创建一笔正数金额的记录。
            // 特殊情况下，有可能下载的记录是免费，即金额是0，那还是要给对方帐户创建一笔记录
            if (item.Amount <= 0 && item.Account != item.ReciprocalAccount)
            {
                BillItem reciprocalItem = BillItem.NewItem(item.ReciprocalAccount,
                    "关联-"+item.TransactionType,
                    item.ResPath,
                    -item.Amount,
                    item.Account,
                    "");

                // 保存对方帐户的记录到底层数据库
                 nRet = this.AddBillInternal(reciprocalItem, out  error);
                if (nRet == -1)
                {
                    result.value = -1;
                    result.errorInfo = error;
                    goto ERROR1;
                }
            }

            //当正数时，应该是充值的情况，不可能出现当前帐户与对方帐户不同的情况，也不需要给对方帐户创建记录。
            //当前帐户与对方帐户相同，只有充值/提现 两种情况。
            if (item.Amount > 0)
            {
                // 检查当前帐号 与 对方帐户是否一致
                if (item.Account != item.ReciprocalAccount || item.TransactionType != C_TransactionType_充值)
                {
                    result.value= -1;
                    result.errorInfo = "当交易金额>0，必须是充值类型，且对方帐号与当前帐户一致。";
                }
            }

            return result;

        ERROR1:

            return result;

        }

        public const string C_TransactionType_下载数据 = "下载数据";
        public const string C_TransactionType_购买产品 = "购买产品";
        public const string C_TransactionType_充值 = "充值";
        public const string C_TransactionType_提现 = "提现";



        // 内部函数，新增bill到底层数据库
        private int AddBillInternal(BillItem item,out string error)
        {
            error = "";

            // 保持记录到底层数据库
            try
            {
                // 得到原来余额
                decimal oldBalance = this.GetBalanceForAccount(item.Account);
                // 算出新余额
                item.Balance= item.Amount + oldBalance;


                this._dbclient.Items.Add(item);
                this._dbclient.SaveChanges(true);
            }
            catch (Exception ex)
            {
                error= "保存到数据库时异常：" + ex.Message;
                return -1;
            }

            return 0;
        }


        // 取出帐户查看余额
        public decimal GetBalanceForAccount(string account)
        {
            // 先找到此帐户最后一条记录
            BillItem item = null;
            try
            {
                item = this._dbclient.Items
                     .Where(b => b.Account == account)
                     .OrderBy(b => b.CreateTime)
                     .Last();
            }
            catch(InvalidOperationException ex)  //如果没有对应的记录，会报这种异常
            {
                return 0;
            }

            // 存在，返回余额
            if (item != null)
                return item.Balance;

            // 不存在，返回0
            return 0;
        }

        // 直接计算的办法
        private decimal GetBalanceForAccount2(string account)
        {
            List<BillItem> items = this._dbclient.Items
                .Where(b => b.Account == account)
                .ToList();

            return items.Sum(b => b.Balance);
        }

        public List<BillItem> GetBills(string startDate, string endDate, string account)
        {

            IQueryable<BillItem> iqm = this._dbclient.Items
                .Where(b => b.CreateTime.CompareTo(startDate) >= 0
                                   && b.CreateTime.CompareTo(endDate) <= 0);

            // 增加一个条件
            if (string.IsNullOrEmpty(account)==false)
            {
                iqm=iqm.Where(b => b.Account == account);
            }


            List<BillItem> items = iqm.OrderBy(b => b.CreateTime)
                .ToList();

            return items;
        }

        public int GetDownloadCount(string startDate, string endDate, string account)
        {

            return this._dbclient.Items
                .Where(b => b.CreateTime.CompareTo(startDate) >= 0
                                   && b.CreateTime.CompareTo(endDate) <= 0)
                .Where(b => b.Account == account)
                .Where(b => b.TransactionType == C_TransactionType_下载数据)
                .Count();


        }




#if NO

        //public void SetItem(string action, BillItem item)
        //{
        //    if (action == "add")
        //    {
        //        this.AddItem(item);
        //        return;
        //    }
        //    else if (action == "change")
        //    {
        //        this.ChangeItem(item);
        //        return;
        //    }
        //    else if (action == "delete")
        //    {
        //        this.DeleteItem(item.Id);
        //        return;
        //    }

        //    throw new Exception("不支持的action["+action+"]");


        //}






        /// <summary>
        /// 根据路径查找一项预约记录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private BillItem GetBill(string billId)
        {
            List<BillItem> items = this._dbclient.Items
                .Where(b => b.Id == billId)
                //.OrderBy(b => b.RequestTime)
                .ToList();

            if (items.Count > 0)
                return items[0];

            return null;
        }

        // 暂不支持修改帐单
        private void Change(BillItem item)
        {
            BillItem old = this.GetBill(item.Id);
            if (item == null)
                throw new Exception("在item表中未找到id为" + item.Id + "的记录。");

            // 保存到库中
            this._dbclient.Items.Update(item);
            this._dbclient.SaveChanges(true);
        }


        // 暂不支持删除帐单
        private void Delete(string billId)
        {
            //BillItem item = this.GetItem(billId);

            BillItem item = new BillItem();
            item.Id = billId;
            this._dbclient.Items.Remove(item);
            this._dbclient.SaveChanges();
        }

#endif


    }

    // API函数结果
    public class ApiResult
    {
        // -1表示出错，>=0 表示成功
        public long value { get; set; }

        //// 错误码，字符串类型，目前值如下
        //// noError:成功
        //// systemError:系统错误，一般是指严重错误，例如帐户密码不正确，系统内部出现严重错误，导致没法处理记录，此时value=-1。
        //// partError:部分错误，在作为整体返回结果时，如果处理的记录部分成功，部分出错，value=-1，errorCode=partError
        //// notFound:未找到，根据传入的路径或条码未找到对应记录
        //public string errorCode { get; set; }

        // 错误描述信息
        public string errorInfo { get; set; }
    }

    //// 增加记录委托
    //public delegate void AddNoteDelegate(Note note);

}
