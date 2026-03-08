using Microsoft.AspNetCore.Mvc;
using WebApplication909.Helpers;      // для ToProductViewModels()
using OnlineShop.Db.Interfaces;

namespace WebApplication909.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsRepository productsRepository;

        public HomeController(IProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public IActionResult Index()
        {
            var products = productsRepository.GetAll();                   // List<Product>
            var productViewModels = products.ToProductViewModels();       // преобразуем в List<ProductViewModel>
            return View(productViewModels);                               // передаём правильный тип
        }

        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return View();

            var products = productsRepository.Search(query);              // List<Product>
            var productViewModels = products.ToProductViewModels();       // преобразование
            return View(productViewModels);
        }
    }
}