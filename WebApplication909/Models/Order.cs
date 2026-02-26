using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;

namespace WebApplication909.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public List<CartItem> Items { get; set; }

        [Required]
        public DeliveryUser DeliveryUser { get; set; }

        [ValidateNever]
        public DateTime CreationDateTime { get; set; }

        public decimal? TotalCost => Items?.Sum(item => item.Cost);

        public int? ItemsQuantity => Items?.Sum(item => item.Quantity);

        [Required]
        public OrderStatus Status { get; set; }
    }
}
