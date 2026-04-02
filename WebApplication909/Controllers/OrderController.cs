using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using WebApplication909.Helpers;
using WebApplication909.Models;

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

            var cartView = cart.ToCartViewModel();

            var order = new OrderViewModel()
            {
                Items = cartView?.Items ?? [],
            };

            return View(order);
        }

        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Buy(OrderViewModel order)
        {
            var cart = _cartsRepository.TryGetByUserId(Constants.UserId);

            if (cart == null)
            {
                return View(nameof(Index), order);
            }

            order.Items = cart.Items.ToCartItemViewModels();
            order.UserId = Constants.UserId;

            if (!ModelState.IsValid)
            {
                return View(nameof(Index), order);
            }

            var orderDb = new Order()
            {
                Id = order.Id,
                UserId = order.UserId,
                Items = cart.Items,
                DeliveryUser = order.DeliveryUser.ToDeliveryUserDb(),
                CreationDateTime = order.CreationDateTime,
                Status = (OrderStatus)order.Status,
            };

            _ordersRepository.Add(orderDb);

            _cartsRepository.Clear(Constants.UserId);

            return RedirectToAction(nameof(Success));
        }

    }
}
