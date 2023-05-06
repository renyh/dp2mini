
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dp2mini
{
    public class BillDB : DbContext
    {
        string _dbFile;
        public BillDB(string dbFile)
        {
            this._dbFile = dbFile;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Set the filename of the database to be created
            //string filePath = this._dir + "/db.sqlite";
            optionsBuilder.UseSqlite("Data Source="+ this._dbFile);
        }

        public DbSet<BillItem> Items { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Define the Table(s) and References to be created automatically
            modelBuilder.Entity<BillItem>(i =>
            {
                i.HasKey(e => e.Id);

                // 使用guid
                //b.Property(e => e.Id).ValueGeneratedOnAdd();

                i.ToTable("item");
            });



        }
    }

}
