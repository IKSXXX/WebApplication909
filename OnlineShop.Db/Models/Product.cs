namespace OnlineShop.Db.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public string? PhotoPath { get; set; } = "/img/anyProduct.png";
    }
}