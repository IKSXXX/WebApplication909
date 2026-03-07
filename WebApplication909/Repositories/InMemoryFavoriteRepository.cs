using OnlineShop.Db.Models;
using WebApplication909.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Repositories
{
    public class InMemoryFavoritesRepository : IFavoritesRepository
    {
        private readonly List<Favourite> _favourites = new();

        public Favourite? TryGetByUserId(string userId)
        {
            return _favourites.FirstOrDefault(x => x.UserId == userId);
        }

        public void Add(Product product, string userId)
        {
            var existingFavourite = TryGetByUserId(userId);
            if (existingFavourite == null)
            {
                existingFavourite = new Favourite()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Items = new List<Product> { product }
                };
                _favourites.Add(existingFavourite);
            }
            else
            {
                var existingItem = existingFavourite.Items.FirstOrDefault(x => x.Id == product.Id);
                if (existingItem == null)
                {
                    existingFavourite.Items.Add(product);
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
                _favourites.Remove(existingFavorite);
            }
        }

        Favourite? IFavoritesRepository.TryGetByUserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}