namespace WebApplication909.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public List<CartItemViewModel> Items { get; set; }

        public decimal? TotalCost => Items?.Sum(item => item.Cost);

        public decimal Quantity => Items.Sum(item => item.Quantity);
    }
}
