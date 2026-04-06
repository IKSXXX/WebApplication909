using Microsoft.AspNetCore.Mvc;
using WebApplication909.Helpers;
using OnlineShop.Db.Interfaces;

namespace WebApplication909.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsRepository productsRepository;

        public ProductController(IProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public ActionResult Index(int id)
        {
            var product = productsRepository.TryGetById(id);
            if (product == null)
                return NotFound();

            return View(product.ToProductViewModel());  
        }
    }
}