using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using WebApplication909.Extensions;
using WebApplication909.Helpers;

namespace WebApplication909.Controllers
{
    public class FavoriteController(IProductsRepository productsRepository, IFavoritesRepository favoritesRepository) : Controller
    {
        private readonly IProductsRepository _productsRepository = productsRepository;
        private readonly IFavoritesRepository _favoritesRepository = favoritesRepository;

        public IActionResult Index()
        {
            var userId = User.GetUserId();
            var favorites = _favoritesRepository.TryGetByUserId(userId);

            return View(favorites.ToFavoriteViewModel());
        }

        public IActionResult Add(int productId)
        {
            var product = _productsRepository.TryGetById(productId);

            if (product != null)
            {
                var userId = User.GetUserId();
                _favoritesRepository.Add(product, userId);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int productId)
        {
            var userId = User.GetUserId();
            _favoritesRepository.Delete(productId, userId);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            var userId = User.GetUserId();
            _favoritesRepository.Clear(userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
