using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using WebApplication909.Extensions;
using WebApplication909.Helpers;

namespace WebApplication909.Controllers
{
    [Authorize]
    public class ComparisonController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IComparisonsRepository _comparisonsRepository;

        public ComparisonController(IProductsRepository productsRepository, IComparisonsRepository comparisonsRepository)
        {
            _productsRepository = productsRepository;
            _comparisonsRepository = comparisonsRepository;
        }

        public IActionResult Index()
        {
            var userId = User.GetUserId();
            var comparison = _comparisonsRepository.TryGetByUserId(userId);
            var model = comparison.ToComparisonViewModel();
            return View(model);
        }

        public IActionResult Add(int productId)
        {
            var product = _productsRepository.TryGetById(productId);
            if (product != null)
            {
                var userId = User.GetUserId();
                _comparisonsRepository.Add(product, userId);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int productId)
        {
            var userId = User.GetUserId();
            _comparisonsRepository.Delete(productId, userId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            var userId = User.GetUserId();
            _comparisonsRepository.Clear(userId);
            return RedirectToAction(nameof(Index));
        }
    }
}