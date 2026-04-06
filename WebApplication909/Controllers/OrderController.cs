using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using WebApplication909.Extensions;
using WebApplication909.Helpers;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly IOrdersRepository _ordersRepository;

        public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository)
        {
            _cartsRepository = cartsRepository;
            _ordersRepository = ordersRepository;
        }

        [AllowAnonymous]
        public IActionResult Success() => View();

        public IActionResult Index()
        {
            var userId = User.GetUserId();
            var cart = _cartsRepository.TryGetByUserId(userId);

            var cartView = cart.ToCartViewModel();

            var order = new OrderViewModel()
            {
                Items = cartView?.Items ?? [],
            };

            return View(order);
        }


        [HttpPost]
        public IActionResult Buy(OrderViewModel order)
        {
            var userId = User.GetUserId();
            var cart = _cartsRepository.TryGetByUserId(userId);

            if (cart == null)
            {
                return View(nameof(Index), order);
            }

            order.Items = cart.Items.ToCartItemViewModels();
            order.UserId = userId;

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

            _cartsRepository.Clear(userId);

            return RedirectToAction(nameof(Success));
        }

    }
}
