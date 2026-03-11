namespace OnlineShop.Db.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public Guid CartId { get; set; } // внешний ключ
        public Cart Cart { get; set; }    // навигационное свойство
        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }
    }
}