using WebApplication909.Models;

namespace WebApplication909.Interfaces
{
    public interface IComparisonsRepository
    {
        Comparison? TryGetByUserId(string userId);
        void Add(Product product, string userId);
        void Delete(int productId, string userId);
        void Clear(string userId);
    }
}
