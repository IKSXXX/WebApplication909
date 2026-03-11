using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using WebApplication909.Models;

namespace OnlineShop.Db
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DatabaseContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
             var connectionString = Environment.GetEnvironmentVariable("OnlineShopConnection")
                    ?? "Host=localhost;Port=5432;Database=OnlineShop_1634;Username=postgres;Password=Chipikao";
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryUser> DeliveryUsers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Comparison> Comparisons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь Cart → CartItem (один ко многим)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);

            // Точность для decimal (важно для PostgreSQL)
            modelBuilder.Entity<Product>()
                .Property(p => p.Cost)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}