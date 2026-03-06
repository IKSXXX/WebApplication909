using WebApplication909.Models;
using OnlineShop.Db.Models;

namespace WebApplication909.Interfaces
{
    public interface IFavoritesRepository
    {
        void Add(Product product, string userId);
        void Clear(string uId);
        void Delete(int productId, string userId);
        Favorite? TryGetByUserId(string userId);
    }
}