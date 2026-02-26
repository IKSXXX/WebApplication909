using Microsoft.AspNetCore.Mvc;
using WebApplication909.Models;
using WebApplication909.Interfaces;

namespace WebApplication909.Controllers
{
    public class CartController(ICartsRepository cartRepository, IProductsRepository productsRepository) : Controller
    {
        private readonly ICartsRepository cartRepository = cartRepository;
        private readonly IProductsRepository productsRepository = productsRepository;

        public IActionResult Index()
        {
            var vart = cartRepository.TryGetByUserId(Constants.UserId);
            return View(vart);
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
