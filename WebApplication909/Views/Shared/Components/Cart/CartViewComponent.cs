using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using System.Security.Claims;
using WebApplication909.Extensions;
using WebApplication909.Helpers;
using WebApplication909.Models;

namespace WebApplication909.Views.Shared.ViewComponents.CartViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartsRepository _cartsRepository;

        public CartViewComponent(ICartsRepository cartsRepository)
        {
            _cartsRepository = cartsRepository;
        }

        public IViewComponentResult Invoke()
        {
            var userId = UserClaimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return View("Cart", 0);

            var cart = _cartsRepository.TryGetByUserId(userId).ToCartViewModel();
            var productsCount = cart?.Items?.Count() ?? 0;
            return View("Cart", productsCount);
        }
    }
}