using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using WebApplication909.Extensions;
using WebApplication909.Helpers;

namespace WebApplication909.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartsRepository cartRepository;
        private readonly IProductsRepository productsRepository;

        public CartController(ICartsRepository cartRepository, IProductsRepository productsRepository)
        {
            this.cartRepository = cartRepository;
            this.productsRepository = productsRepository;
        }

        public IActionResult Index()
        {
            var userId = User.GetUserId();
            var cart = cartRepository.TryGetByUserId(userId);
            return View(cart.ToCartViewModel());
        }

        public ActionResult Add(int productId)
        {
            var userId = User.GetUserId();
            var product = productsRepository.TryGetById(productId);
            if (product != null)
                cartRepository.Add(product, userId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Subtract(int productId)
        {
            var userId = User.GetUserId();
            cartRepository.Subtract(productId, userId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            var userId = User.GetUserId();
            cartRepository.Clear(userId);
            return RedirectToAction(nameof(Index));
        }
    }
}