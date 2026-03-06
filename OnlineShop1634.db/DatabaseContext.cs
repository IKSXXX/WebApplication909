using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineShop1634.db.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace OnlineShop1634.db
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
