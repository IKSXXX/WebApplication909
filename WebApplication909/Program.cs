using Microsoft.AspNetCore.Localization;
using Serilog;
using System.Globalization;
using WebApplication909;
using WebApplication909.Interfaces;
using WebApplication909.Repositories;
using Serilog;
using WebApplication909.Areas.Admin.Repositories;
using WebApplication909.Areas.Admin.Interfaces;

namespace OnlineShopWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Настройка Serilog (упрощено, убрана пустая инициализация)
            builder.Host.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddControllersWithViews();

            // Регистрация репозиториев
            builder.Services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();
            builder.Services.AddSingleton<ICartsRepository, InMemoryCartsRepository>();
            builder.Services.AddSingleton<IOrdersRepository, InMemoryOrdersRepository>();
            builder.Services.AddSingleton<IFavoritesRepository, InMemoryFavouritesRepository>();
            builder.Services.AddSingleton<IComparisonsRepository, InMemoryComparisonsRepository>();
            builder.Services.AddSingleton<IRolesRepository, InMemoryRolesRepository>();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            // ВАЖНО: включаем раздачу статических файлов (css, js, изображения)
            app.UseStaticFiles();

            app.UseRouting();

            // Маршрут для областей
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            // Маршрут по умолчанию
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}