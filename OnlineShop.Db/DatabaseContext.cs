using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // Создаёт базу данных при первом обращении
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<DeliveryUser> DeliveryUsers { get; set; }
        //public DbSet<Favorite> Favorites { get; set; }
        //public DbSet<Comparison> Comparisons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь Cart → CartItem (один ко многим)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);

            base.OnModelCreating(modelBuilder);
        }
    }
}