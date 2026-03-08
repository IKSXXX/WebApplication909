using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication909.Models;

namespace OnlineShop.Db.Repositories
{
    public class DbFavoritesRepository : IFavoritesRepository
    {
        private readonly DatabaseContext _context;

        public DbFavoritesRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Favorite? TryGetByUserId(string userId)
        {
            return _context.Favorites
                .Include(f => f.Items)
                .FirstOrDefault(f => f.UserId == userId);
        }

        public void Add(Product product, string userId)
        {
            var favorite = TryGetByUserId(userId);
            if (favorite == null)
            {
                favorite = new Favorite
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Items = new List<Product> { product }
                };
                _context.Favorites.Add(favorite);
            }
            else
            {
                if (!favorite.Items.Any(p => p.Id == product.Id))
                {
                    favorite.Items.Add(product);
                }
            }
            _context.SaveChanges();
        }

        public void Delete(int productId, string userId)
        {
            var favorite = TryGetByUserId(userId);
            if (favorite != null)
            {
                var product = favorite.Items.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    favorite.Items.Remove(product);
                    _context.SaveChanges();
                }
            }
        }

        public void Clear(string userId)
        {
            var favorite = TryGetByUserId(userId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }
        }
    }
}