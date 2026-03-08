using OnlineShop.Db.Models;

namespace WebApplication909.Models
{
    public class Favorite
    {
        public List<Product> Items;
        public Guid Id { get; set; }
        public string? UserId { get; set; }
    }
}
