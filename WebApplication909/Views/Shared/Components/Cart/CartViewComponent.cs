using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using WebApplication909.Helpers;
using WebApplication909.Models;
using WebApplication909.Extensions;

namespace WebApplication909.Views.Shared.ViewComponents.CartViewComponents
{
    public class CartViewComponent(ICartsRepository cartsRepository) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var userId = User.GetUserId();
            var cart = cartsRepository.TryGetByUserId(userId).ToCartViewModel();
            var productsCount = cart?.Items.Count() ?? 0;

            return View("Cart", productsCount);
        }
    }
}