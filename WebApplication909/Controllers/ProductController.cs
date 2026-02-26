using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication909.Interfaces;

namespace WebApplication909.Controllers
{
    public class ProductController(IProductsRepository productsRepository) : Controller
    {
        private readonly IProductsRepository productsRepository = productsRepository;

        public ActionResult Index(int id)
        {
            var product = productsRepository.TryGetById(id);
            return View(product);
        }
    }
}



