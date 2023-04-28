using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dp2mini
{
    public class DbManager
    {
        #region 单一实例

        static DbManager _instance;
        private DbManager()
        {
            this.ConnectionDb();
        }
        private static object _lock = new object();
        static public DbManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (_lock)  //线程安全的
                    {
                        _instance = new DbManager();
                    }
                }
                return _instance;
            }
        }
        #endregion

        ////声明事件
        //public event AddNoteDelegate AddNoteHandler;


        // 数据库对象
        BillDB _dbclient = null;

        // 连接数据库
        public void ConnectionDb()
        {
            this._dbclient = new BillDB();
            //Create the database file at a path defined in SimpleDataStorage
            this._dbclient.Database.EnsureCreated();
            //Create the database tables defined in SimpleDataStorage
            this._dbclient.Database.Migrate();
        }


        public static string NumToString(int num)
        {
            // 将数字转成7位字符串，左边补0
            return num.ToString().PadLeft(7, '0');
        }


        /// <summary>
        /// 根据路径查找一项预约记录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public BillItem GetItem(string id)
        {
            List<BillItem> items = this._dbclient.Items
                .Where(b => b.Id == id)
                //.OrderBy(b => b.RequestTime)
                .ToList();

            if (items.Count > 0)
                return items[0];

            return null;
        }


        public void SetItem(string action, BillItem item)
        {
            if (action == "add")
            {
                this.AddItem(item);
                return;
            }
            else if (action == "change")
            {
                this.ChangeItem(item);
                return;
            }
            else if (action == "delete")
            {
                this.DeleteItem(item.Id);
                return;
            }

            throw new Exception("不支持的action["+action+"]");

        
        }


        /// <summary>
        /// 给本地预约记录表中新增一行
        /// </summary>
        /// <param name="item"></param>
        internal void AddItem(BillItem item)
        {
            this._dbclient.Items.Add(item);
            this._dbclient.SaveChanges(true);
        }

        internal void ChangeItem(BillItem item)
        {
            BillItem old = this.GetItem(item.Id);
            if (item == null)
                throw new Exception("在item表中未找到id为" + item.Id + "的记录。");

            // 保存到库中
            this._dbclient.Items.Update(item);
            this._dbclient.SaveChanges(true);
        }


        // 删除item
        internal void DeleteItem(string billId)
        {
            //BillItem item = this.GetItem(billId);

            BillItem item = new BillItem();
            item.Id = billId;
            this._dbclient.Items.Remove(item);
            this._dbclient.SaveChanges();
        }




    }

    //// 增加记录委托
    //public delegate void AddNoteDelegate(Note note);

}
