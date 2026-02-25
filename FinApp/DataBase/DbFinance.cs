using FinApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinApp.DataBase
{
    class DbFinance: DbContext
    {

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = System.IO.Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "finance.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
