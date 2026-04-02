using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.Db.Repositories
{
    public class DbFavouritesRepository : IFavoritesRepository
    {
        private readonly DatabaseContext _context;

        public DbFavouritesRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Favorite? TryGetByUserId(string userId)
        {
            return _context.Favorites
                .Include(f => f)
                .FirstOrDefault(f => f.UserId == userId);
        }

        public void Add(Product product, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId cannot be null or empty", nameof(userId));
            var existing = _context.Favorites
                .FirstOrDefault(f => f.ProductId == product.Id && f.UserId == userId);

            if (existing != null)
                return; 

            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = product.Id,
            };

            _context.Favorites.Add(favorite);
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