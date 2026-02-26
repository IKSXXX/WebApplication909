using System.Xml.Linq;
using WebApplication909.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Repositories
{
    public class InMemoryFavouritesRepository : IFavoritesRepository
    {
        private readonly List<Favorite> _favorites = [];

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
                    Items = [product]
                };

                _favorites.Add(existingFavorite);
            }
            else
            {
                var existingFavoriteItem = existingFavorite.Items.FirstOrDefault(x => x.Id == product.Id);

                if (existingFavoriteItem == null)
                {
                    existingFavorite.Items.Add(product);
                }
            }
        }

        public void Delete(int productId, string userId)
        {
            var existingFavorite = TryGetByUserId(userId);

            var existingFavoriteItem = existingFavorite?.Items.FirstOrDefault(x => x.Id == productId);

            if (existingFavoriteItem != null)
            {
                existingFavorite?.Items.Remove(existingFavoriteItem);
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