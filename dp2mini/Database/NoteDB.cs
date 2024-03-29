﻿using DigitalPlatform.CirculationClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dp2mini
{
    public class NoteDB : DbContext
    {
        public NoteDB() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Set the filename of the database to be created
            string filePath = ClientInfo.UserDir + "/db.sqlite";
            optionsBuilder.UseSqlite("Data Source="+ filePath);
        }

        public DbSet<Note> Notes { get; set; }

        public DbSet<ReservationItem> Items { get; set; }

        public DbSet<Entity> Entities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Define the Table(s) and References to be created automatically
            modelBuilder.Entity<Note>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).ValueGeneratedOnAdd();
                b.ToTable("note");
            });

            modelBuilder.Entity<ReservationItem>(b =>
            {
                b.HasKey(e => e.RecPath);
                b.ToTable("item");
            });


            modelBuilder.Entity<Entity>(b =>
            {
                b.HasKey(e => e.path);
                b.ToTable("entity");
            });


        }
    }

}
