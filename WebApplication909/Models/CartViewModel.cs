using OnlineShop.Db.Models;

namespace WebApplication909.Models
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new();

        public decimal? TotalCost => Items.Sum(x => x.Cost);
    }
}
