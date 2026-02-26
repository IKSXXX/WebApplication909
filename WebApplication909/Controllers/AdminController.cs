using Microsoft.AspNetCore.Mvc;
using WebApplication909.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    public class AdminController(IProductsRepository productsRepository) : Controller
    {
        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Roles()
        {
            return View();
        }

        public IActionResult Products()
        {
            var products = productsRepository.GetAll();

            return View(products);
        }

        public IActionResult DeleteProduct(int id)
        {
            productsRepository.Delete(id);

            return RedirectToAction("Products");
        }

        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            productsRepository.Add(product);

            return RedirectToAction(nameof(Products));
        }

        public ActionResult UpdateProduct(int id)
        {
            var product = productsRepository.TryGetById(id);

            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            productsRepository.Update(product);

            return RedirectToAction(nameof(Products));
        }
    }
}
