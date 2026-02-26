using Microsoft.AspNetCore.Mvc;
using WebApplication909.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public AdminController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }
        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult Roles()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _productsRepository.Add(product);

            return RedirectToAction("Products", "Admin");
        }
        public IActionResult DeleteProduct(int id)
        {
            _productsRepository.Delete(id);

            return RedirectToAction("Products", "Admin");
        }

        public IActionResult UpdateProduct(int id)
        {
            var existingProduct = _productsRepository.TryGetById(id);

            return View(existingProduct);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            _productsRepository.Update(product);

            return RedirectToAction("Products", "Admin");
        }
    }
}
