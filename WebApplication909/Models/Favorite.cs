using WebApplication909.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;

namespace WebApplication909.Models
{
    public class Favorite 
    {
        public List<Product> Items;
        public Guid Id { get; set; }
        public string? UserId { get; set; }
    }
}
