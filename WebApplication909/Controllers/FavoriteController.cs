using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using WebApplication909.Interfaces;

namespace WebApplication909.Controllers
{
    public class FavoriteController(IProductsRepository productsRepository, IFavoritesRepository favoritesRepository) : Controller
    {
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;

        public IActionResult Index()
        {
            var favorites = _favoritesRepository.TryGetByUserId(Constants.UserId);

            return View(favorites);
        }

        public IActionResult Add(int productId)
        {
            var product = _productsRepository.TryGetById(productId);

            if (product != null)
            {
                _favoritesRepository.Add(product, Constants.UserId);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int productId)
        {
            _favoritesRepository.Delete(productId, Constants.UserId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            _favoritesRepository.Clear(Constants.UserId);

            return RedirectToAction(nameof(Index));
        }
    }
}
