using Microsoft.AspNetCore.Mvc;
using WebApplication909.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    public class AdminController(IProductsRepository productsRepository,
                               IOrdersRepository ordersRepository,
                               IRolesRepository rolesRepository) : Controller
    {
        #region Orders
        public IActionResult Orders()
        {
            var orders = ordersRepository.GetAll();

            return View(orders);
        }

        public IActionResult DetailOrder(Guid orderId)
        {
            var order = ordersRepository.TryGetById(orderId);

            return View(order);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            ordersRepository.UpdateStatus(orderId, status);

            return RedirectToAction(nameof(Orders));
        }

        #endregion

        public IActionResult Users()
        {
            return View();
        }


        #region Roles
        public IActionResult Roles()
        {
            var roles = rolesRepository.GetAll();

            return View(roles);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRole(Role role)
        {
            if (rolesRepository.TryGetByName(role.Name) != null)
            {
                ModelState.AddModelError("", "Такая роль уже существует!");
            }

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            rolesRepository.Add(role);

            return RedirectToAction(nameof(Roles));
        }

        public IActionResult DeleteRole(Guid roleId)
        {
            rolesRepository.Delete(roleId);

            return RedirectToAction(nameof(Roles));
        }

        #endregion

        #region Products
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
        public ActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            productsRepository.Add(product);

            return RedirectToAction("Products");
        }

        public ActionResult UpdateProduct(int id)
        {
            var product = productsRepository.TryGetById(id);

            return View(product);
        }

        [HttpPost]
        public ActionResult UpdateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            productsRepository.Update(product);

            return RedirectToAction("Products");
        }
        #endregion
    }
}
