using Microsoft.AspNetCore.Mvc;
using WebApplication909.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductsRepository _productsRepository;


        public ProductController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;

        }


        public IActionResult Index()
        {
            var products = _productsRepository.GetAll();

            return View(products);
        }


        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _productsRepository.Add(product);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            _productsRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            var existingProduct = _productsRepository.TryGetById(id);

            return View(existingProduct);
        }


        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _productsRepository.Update(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
