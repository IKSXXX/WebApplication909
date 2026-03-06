namespace OnlineShop.Db.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new();
        public DeliveryUser DeliveryUser { get; set; }
        public DateTime CreationDateTime { get; set; }
        public OrderStatus Status { get; set; }
    }
}