using Microsoft.AspNetCore.Localization;
using System.Globalization;
using WebApplication909;
using WebApplication909.Interfaces;
using WebApplication909.Repositories;

namespace OnlineShopWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();
            builder.Services.AddSingleton<ICartsRepository, InMemoryCartsRepository>();
            builder.Services.AddSingleton<IOrdersRepository, InMemoryOrdersRepository>();
            builder.Services.AddSingleton<IFavoritesRepository, InMemoryFavouritesRepository>();
            builder.Services.AddSingleton<IComparisonsRepository, InMemoryComparisonsRepository>();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US")
                };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseRequestLocalization();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
