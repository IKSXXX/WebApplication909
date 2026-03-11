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
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Cost).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasMany(c => c.Items)
                      .WithOne(ci => ci.Cart)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);
                entity.HasOne(ci => ci.Product)
                      .WithMany()
                      .HasForeignKey("ProductId");
                entity.HasOne(ci => ci.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(ci => ci.OrderId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasOne(o => o.DeliveryUser)
                      .WithMany()
                      .HasForeignKey("DeliveryUserId")
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(o => o.Status).HasConversion<int>(); 
            });

            modelBuilder.Entity<DeliveryUser>(entity =>
            {
                entity.HasKey(d => d.Id);
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.HasMany(f => f.Items)
                      .WithMany()
                      .UsingEntity(j => j.ToTable("FavoriteProduct"));
            });

            modelBuilder.Entity<Comparison>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasMany(c => c.Items)
                      .WithMany()
                      .UsingEntity(j => j.ToTable("ComparisonProduct"));
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}