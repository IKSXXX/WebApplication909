
using WebApplication909.Interfaces;
using WebApplication909.Models;

namespace WebApplication909
{
    public class InMemoryCartsRepository : ICartsRepository
    {
        private readonly List<Cart> _carts = [];

        public Cart? TryGetByUserId(string userId)
        {
            return _carts.FirstOrDefault(cart => cart.UserId == userId);
        }

        public void Clear(string uId) => _carts?.Remove(TryGetByUserId(uId));

        public void Subtract(int productId, string userId)
        {
            var existingCart = TryGetByUserId(userId);

            var existingCartItem = existingCart?.Items.FirstOrDefault(item => item.Product.Id == productId);

            if (existingCartItem == null)
            {
                return;
            }

            if (existingCartItem.Quantity == 1)
            {
                existingCart?.Items.Remove(existingCartItem);
                return;
            }

            existingCartItem.Quantity--;
        }

        public void Add(Product product, string userId)
        {
            var existingCart = TryGetByUserId(userId);

            if (existingCart == null)
            {
                existingCart = new Cart()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Items = new List<CartItem>()
                    {
                        new CartItem()
                        {
                            Id = Guid.NewGuid(),
                            Product = product,
                            Quantity = 1
                        }
                    }
                };
                _carts.Add(existingCart);
            }
            else
            {
                var existingCartItem = existingCart.Items.FirstOrDefault(item =>
                    item.Product.Id == product.Id);

                if (existingCartItem == null)
                {
                    var newCartItem = new CartItem()
                    {
                        Id = Guid.NewGuid(),
                        Product = product,
                        Quantity = 1
                    };
                    existingCart.Items.Add(newCartItem);
                }
                else
                {
                    existingCartItem.Quantity++;
                }
            }
        }
    }
}