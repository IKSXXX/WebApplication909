using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication909.Interfaces;
using WebApplication909.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication909.Controllers
{
    public class HomeController(IProductsRepository productsRepository) : Controller
    {
        private readonly IProductsRepository productsRepository = productsRepository;

        public IActionResult Index()
        {
            var products = productsRepository.GetAll();

            return View(products);
        }
        public IActionResult Search(string query)
        {
            if (query == null)
            {
                return View();
            }

            var products = productsRepository.Search(query);

            return View(products);
        }
    }
}
