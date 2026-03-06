using OnlineShop.Db.Models;
using WebApplication909.Interfaces;

namespace WebApplication909.Repositories
{
    public class InMemoryFavoritesRepository : IFavoritesRepository
    {
        private readonly List<Favorite> _favorites = new();

        public Favorite? TryGetByUserId(string userId)
        {
            return _favorites.FirstOrDefault(x => x.UserId == userId);
        }

        public void Add(Product product, string userId)
        {
            var existingFavorite = TryGetByUserId(userId);
            if (existingFavorite == null)
            {
                existingFavorite = new Favorite()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Items = new List<Product> { product }
                };
                _favorites.Add(existingFavorite);
            }
            else
            {
                var existingItem = existingFavorite.Items.FirstOrDefault(x => x.Id == product.Id);
                if (existingItem == null)
                {
                    existingFavorite.Items.Add(product);
                }
            }
        }

        public void Delete(int productId, string userId)
        {
            var existingFavorite = TryGetByUserId(userId);
            if (existingFavorite != null)
            {
                var item = existingFavorite.Items.FirstOrDefault(x => x.Id == productId);
                if (item != null)
                {
                    existingFavorite.Items.Remove(item);
                }
            }
        }

        public void Clear(string userId)
        {
            var existingFavorite = TryGetByUserId(userId);
            if (existingFavorite != null)
            {
                _favorites.Remove(existingFavorite);
            }
        }
    }
}