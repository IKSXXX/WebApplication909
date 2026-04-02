using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Repositories;
using OnlineShop.Db.Interfaces;
using WebApplication909.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication909.Controllers
{
    [Authorize]
    public class CartController(ICartsRepository cartRepository, IProductsRepository productsRepository) : Controller
    {
        private readonly ICartsRepository cartRepository = cartRepository;
        private readonly IProductsRepository productsRepository = productsRepository;

        public IActionResult Index()
        {
            var cart = cartRepository.TryGetByUserId(Constants.UserId);
            return View(cart.ToCartViewModel());
        }
        public ActionResult Add(int productId)
        {
            var product = productsRepository.TryGetById(productId);

            if (product != null)
            {
                cartRepository.Add(product, Constants.UserId);
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Subtract(int productId)
        {
            cartRepository.Subtract(productId, Constants.UserId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            cartRepository.Clear(Constants.UserId);

            return RedirectToAction(nameof(Index));
        }
    }
}
