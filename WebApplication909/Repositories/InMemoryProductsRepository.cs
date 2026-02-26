using System.Runtime.CompilerServices;
using WebApplication909.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Repositories
{
    public class InMemoryProductsRepository : IProductsRepository
    {
        private int _instanceCounter = 0;

        private readonly List<Product> _products;

        public InMemoryProductsRepository()
        {
            _products =
            [
                new Product(++_instanceCounter, "Товар 1", 1000.0M, "Описание товара 1"),
                new Product(++_instanceCounter, "Товар 2", 2000.0M, "Описание товара 2"),
                new Product(++_instanceCounter, "Товар 3", 3000.0M, "Описание товара 3"),
                new Product(++_instanceCounter, "Товар 4", 4000.0M, "Описание товара 4"),
                new Product(++_instanceCounter, "Товар 5", 5000.0M, "Описание товара 5")
            ];
        }

        public List<Product> GetAll() => _products;

        public Product? TryGetById(int productId) =>
            _products.FirstOrDefault(product => product.Id == productId);

        public void Add(Product product)
        {
            product.Id = ++_instanceCounter;

            _products.Add(product);
        }

        public void Delete(int productId)
        {
            var existingProduct = TryGetById(productId);

            if (existingProduct != null)
            {
                _products.Remove(existingProduct);
            }
        }

        public void Update(Product product)
        {
            var excitingProduct = TryGetById(product.Id);

            if (excitingProduct != null)
            {
                excitingProduct.Name = product.Name;
                excitingProduct.Cost = product.Cost;
                excitingProduct.Description = product.Description;
            }
        }

        public List<Product> Search(string text)
        {
            var products = GetAll().Where(product => product.Name!.Contains(text, StringComparison.OrdinalIgnoreCase));

            return products.ToList() ?? [];
        }
    }
}
