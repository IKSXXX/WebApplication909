using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Views.Shared.ViewComponents.CartViewComponents
{
    public class CartViewComponent(ICartsRepository cartsRepository) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = cartsRepository.TryGetByUserId(Constants.UserId);
            var productsCount = cart?.Quantity ?? 0;

            return View("Cart", productsCount);
        }
    }
}