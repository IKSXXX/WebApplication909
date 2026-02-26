using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using WebApplication909.Models;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization;
using WebApplication909.Interfaces;

namespace WebApplication909.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly IOrdersRepository _ordersRepository;

        public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository)
        {
            _cartsRepository = cartsRepository;
            _ordersRepository = ordersRepository;
        }

        public IActionResult Index()
        {
            var cart = _cartsRepository.TryGetByUserId(Constants.UserId);

            return View(cart);
        }

        [HttpPost]
        public IActionResult Buy(DeliveryUser deliveryUser)
        {
            var cart = _cartsRepository.TryGetByUserId(Constants.UserId);

            if (cart is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var order = new Order
            {
                UserId = Constants.UserId,
                Items = cart.Items,
                DeliveryUser = deliveryUser
            };

            _ordersRepository.Add(order);

            _cartsRepository.Clear(Constants.UserId);

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

    }
}
