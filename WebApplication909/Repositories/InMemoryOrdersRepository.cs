using WebApplication909.Interfaces;
using WebApplication909.Models;
using static NuGet.Packaging.PackagingConstants;

namespace WebApplication909.Repositories
{
    public class InMemoryOrdersRepository : IOrdersRepository
    {
        private readonly List<Order> orders = [];
        public List<Order> GetAll() => orders;
        public Order? TryGetById(string id) => orders.FirstOrDefault(o => o.UserId == id);
        public void Add(Order order)
        {
            order.Id = Guid.NewGuid();

            orders.Add(order);
        }
    }
}
