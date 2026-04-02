using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using WebApplication909.Helpers;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public IActionResult Index()
        {
            var orders = _ordersRepository.GetAll();

            return View(orders.ToOrderViewModels());
        }



        public IActionResult Detail(Guid orderId)
        {
            var order = _ordersRepository.TryGetById(orderId);

            return View(order?.ToOrderViewModel());
        }


        [HttpPost]
        public IActionResult UpdateStatus(Guid orderId, OrderStatusViewModel status)
        {
            _ordersRepository.UpdateStatus(orderId, (OrderStatus)status);

            return RedirectToAction(nameof(Index));
        }
    }
}
