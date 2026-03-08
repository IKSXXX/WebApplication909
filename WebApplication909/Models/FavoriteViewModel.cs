using OnlineShop.Db.Models;

namespace WebApplication909.Models
{
    public class FavoriteViewModel
    {
        public List<ProductViewModel> Items;
        public Guid Id { get; set; }
        public string? UserId { get; set; }
    }
}
