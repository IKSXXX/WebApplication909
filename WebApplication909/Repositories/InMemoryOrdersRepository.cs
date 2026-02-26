using WebApplication909.Interfaces;
using WebApplication909.Models;
using static NuGet.Packaging.PackagingConstants;

namespace WebApplication909.Repositories
{
    public class InMemoryOrdersRepository : IOrdersRepository
    {
        private readonly List<Order> _orders = [];

        public void Add(Order order)
        {
            order.Id = Guid.NewGuid();
            order.CreationDateTime = DateTime.Now;
            order.DeliveryUser.Id = Guid.NewGuid();
            order.Status = OrderStatus.Created;

            _orders.Add(order);
        }

        public List<Order> GetAll() => _orders;

        public Order? TryGetById(Guid orderId) =>
            _orders.FirstOrDefault(order => order.Id == orderId);

        public void UpdateStatus(Guid orderId, OrderStatus newStatus)
        {
            var existingOrder = TryGetById(orderId);

            if (existingOrder != null)
            {
                existingOrder.Status = newStatus;
            }
        }

    }
}
