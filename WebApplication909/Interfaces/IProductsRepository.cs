using WebApplication909.Models;

namespace WebApplication909.Interfaces
{
    public interface IProductsRepository
    {
        List<Product> GetAll();
        Product? TryGetById(int productId);
        void Add(Product product);
        void Delete(int productId);
        void Update(Product product);
        List<Product> Search(string text);
    }
}