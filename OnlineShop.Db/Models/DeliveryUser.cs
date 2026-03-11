using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models
{
    public class DeliveryUser
    {
        public required string Name { get; set; }
        public required string Address { get; set; }

        public required string Phone { get; set; }

        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public Guid Id { get; set; }
    }
}
