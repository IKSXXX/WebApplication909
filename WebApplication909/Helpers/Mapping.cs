using OnlineShop.Db.Models;
using WebApplication909.Models;

namespace WebApplication909.Helpers
{
    public static class Mapping
    {
        #region Product
        public static List<ProductViewModel> ToProductViewModels(this List<Product> productsDb)
        {
            var productsViewModel = new List<ProductViewModel>();

            foreach (var productDb in productsDb)
            {
                productsViewModel.Add(productDb.ToProductViewModel());
            }

            return productsViewModel;
        }

        public static ProductViewModel ToProductViewModel(this Product productDb)
        {
            return new ProductViewModel()
            {
                Id = productDb.Id,
                Name = productDb.Name,
                Cost = productDb.Cost,
                Description = productDb.Description,
                PhotoPath = productDb.PhotoPath
            };
        }

        public static Product ToProductDb(this ProductViewModel product)
        {
            return new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                PhotoPath = product.PhotoPath,
            };
        }
        #endregion

        #region Favorite

        public static List<FavoriteViewModel> ToFavoriteViewModels(this List<Favorite> favoritesDb)
        {
            var favoritesViewModel = new List<FavoriteViewModel>();

            foreach (var cartDbItem in favoritesDb)
            {
                favoritesViewModel.Add(cartDbItem.ToFavoriteViewModel());
            }

            return favoritesViewModel;
        }

        public static FavoriteViewModel? ToFavoriteViewModel(this Favorite? favorite)
        {
            if (favorite == null) return null;
            return new FavoriteViewModel
            {
                Id = favorite.Id,
                UserId = favorite.UserId,
                Items = favorite.Items?.Select(p => p.ToProductViewModel()).ToList() ?? new()
            };
        }
        #endregion

        #region Cart
        public static List<CartItemViewModel> ToCartItemViewModels(this List<CartItem> cartDbItems)
        {
            var cartItemsViewModel = new List<CartItemViewModel>();

            foreach (var cartDbItem in cartDbItems)
            {
                cartItemsViewModel.Add(cartDbItem.ToCartItemViewModel());
            }

            return cartItemsViewModel;
        }

        public static CartItemViewModel ToCartItemViewModel(this CartItem cartDbItem)
        {
            return new CartItemViewModel()
            {
                Id = cartDbItem.Id,
                Product = cartDbItem.Product.ToProductViewModel(),
                Quantity = cartDbItem.Quantity,
            };
        }

        public static CartViewModel? ToCartViewModel(this Cart? cartDb)
        {
            if (cartDb == null)
            {
                return null;
            }

            return new CartViewModel()
            {
                Id = cartDb.Id,
                UserId = cartDb.UserId,
                Items = cartDb.Items.ToCartItemViewModels(),
            };
        }
        #endregion

        #region Order
        public static List<OrderViewModel> ToOrderViewModels(this List<Order> ordersDb)
        {
            var ordersViewModel = new List<OrderViewModel>();

            foreach (var orderDb in ordersDb)
            {
                ordersViewModel.Add(orderDb.ToOrderViewModel());
            }

            return ordersViewModel;
        }

        public static OrderViewModel ToOrderViewModel(this Order orderDb)
        {
            return new OrderViewModel()
            {
                Id = orderDb.Id,
                UserId = orderDb.UserId,
                Items = orderDb.Items.ToCartItemViewModels(),
                DeliveryUser = orderDb.DeliveryUser.ToDeliveryUserViewModel(),
                CreationDateTime = orderDb.CreationDateTime,
                Status = (OrderStatusViewModel)orderDb.Status,
            };
        }

        public static DeliveryUserViewModel ToDeliveryUserViewModel(this DeliveryUser deliveryUserDb)
        {
            return new DeliveryUserViewModel()
            {
                Id = deliveryUserDb.Id,
                Name = deliveryUserDb.Name,
                Address = deliveryUserDb.Address,
                Phone = deliveryUserDb.Phone,
                Date = deliveryUserDb.Date,
                Comment = deliveryUserDb.Comment
            };
        }

        public static DeliveryUser ToDeliveryUserDb(this DeliveryUserViewModel deliveryUser)
        {
            return new DeliveryUser()
            {
                Id = deliveryUser.Id,
                Name = deliveryUser.Name,
                Address = deliveryUser.Address,
                Phone = deliveryUser.Phone,
                Date = deliveryUser.Date,
                Comment = deliveryUser.Comment
            };
        }
        #endregion

        #region User
        public static UserViewModel ToUserViewModel(this User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreationDateTime = DateTime.UtcNow
            };
        }

        public static List<UserViewModel> ToUserViewModels(this List<User> users)
        {
            return users.Select(u => u.ToUserViewModel()).ToList();
        }


        #endregion

        #region Comparison
        public static ComparisonViewModel? ToComparisonViewModel(this Comparison comparison)
        {
            if (comparison == null)
            {
                return null;
            }

            return new ComparisonViewModel()
            {
                Id = comparison.Id,
                Items = comparison.Items,
                UserId = comparison.UserId
            };
        }
        #endregion
    }
}
