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

            var order = new Order()
            {
                Items = cart?.Items ?? [],
            };

            return View(order);
        }

        [HttpPost]
        public IActionResult Buy(Order order)
        {
            var cart = _cartsRepository.TryGetByUserId(Constants.UserId);

            if (cart == null)
            {
                return View(nameof(Index), order);
            }

            order.Items = cart.Items;
            order.UserId = Constants.UserId;

            if (!ModelState.IsValid)
            {
                return View(nameof(Index), order);
            }

            _ordersRepository.Add(order);

            _cartsRepository.Clear(Constants.UserId);

            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {
            return View();
        }

    }
}
