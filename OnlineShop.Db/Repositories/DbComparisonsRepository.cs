using System.Xml.Linq;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repositories
{
    public class DbComparisonsRepository : IComparisonsRepository
    {
        private readonly List<Comparison> _comparisons = [];

        public Comparison? TryGetByUserId(string userId)
        {
            return _comparisons.FirstOrDefault(x => x.UserId == userId);
        }

        public void Add(Product product, string userId)
        {
            var existingComparison = TryGetByUserId(userId);

            if (existingComparison == null)
            {
                existingComparison = new Comparison()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Items = [product]
                };

                _comparisons.Add(existingComparison);
            }
            else
            {
                var existingComparisonItem = existingComparison.Items.FirstOrDefault(x => x.Id == product.Id);

                if (existingComparisonItem == null)
                {
                    existingComparison.Items.Add(product);
                }
            }
        }

        public void Delete(int productId, string userId)
        {
            var existingComparison = TryGetByUserId(userId);

            var existingComparisonItem = existingComparison?.Items.FirstOrDefault(x => x.Id == productId);

            if (existingComparisonItem != null)
            {
                existingComparison?.Items.Remove(existingComparisonItem);
            }

        }

        public void Clear(string userId)
        {
            var existingComparison = TryGetByUserId(userId);

            if (existingComparison != null)
            {
                _comparisons.Remove(existingComparison);
            }
        }
    }
}
