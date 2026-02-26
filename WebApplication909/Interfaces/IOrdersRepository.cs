using WebApplication909.Models;

namespace WebApplication909.Interfaces
{
    public interface IOrdersRepository
    {
        void Add(Order order);
        List<Order> GetAll();
        Order? TryGetById(Guid id);
        void UpdateStatus(Guid orderId, OrderStatus status);
    }
}