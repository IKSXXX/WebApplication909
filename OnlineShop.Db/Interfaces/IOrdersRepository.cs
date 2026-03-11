using OnlineShop.Db.Models;

namespace OnlineShop.Db.Interfaces
{
    public interface IOrdersRepository
    {
        void Add(Order order);
        List<Order> GetAll();
        Order? TryGetById(Guid orderId);
        void UpdateStatus(Guid orderId, OrderStatus status);
    }
}