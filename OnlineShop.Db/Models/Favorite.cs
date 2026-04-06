namespace OnlineShop.Db.Models
{
    public class Favorite
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
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