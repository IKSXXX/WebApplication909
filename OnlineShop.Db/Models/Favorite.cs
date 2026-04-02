using OnlineShop.Db.Models;

namespace WebApplication909.Models
{
    public class Favorite
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<Product> Items { get; set; }
    }
}
