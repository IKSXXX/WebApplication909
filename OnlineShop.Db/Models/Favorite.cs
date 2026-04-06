using OnlineShop.Db.Models;

namespace OnlineShop.Db.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public virtual DeliveryUser? User { get; set; } 
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!; 
        public Favorite() { }
        public Favorite(string userId, int productId)
        {
            UserId = userId;
            ProductId = productId;
        }
    }
}