using Microsoft.AspNetCore.Localization;
using Serilog;
using System.Globalization;
using WebApplication909;
using WebApplication909.Interfaces;
using WebApplication909.Repositories;
using Serilog;

namespace OnlineShopWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            


            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                    .CreateLogger(); //инициализация глобального логера Serilog с базовой конфигурацией

            try //начало блока для обработки ошибок запуска приложения
            {
                Log.Information("Starting server..."); //запись информационного сообщения о запуске сервера

                builder.Host.UseSerilog((context, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                }); //подключение Serilog в качестве службы логирования по умолчанию, конфигурация настроек читается 
                    //из файла конфигурации


                // Add services to the container.
                builder.Services.AddControllersWithViews();

                builder.Services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();
                builder.Services.AddSingleton<ICartsRepository, InMemoryCartsRepository>();
                builder.Services.AddSingleton<IOrdersRepository, InMemoryOrdersRepository>();
                builder.Services.AddSingleton<IFavoritesRepository, InMemoryFavouritesRepository>();
                builder.Services.AddSingleton<IComparisonsRepository, InMemoryComparisonsRepository>();
                builder.Services.AddSingleton<IRolesRepository, InMemoryRolesRepository>();


                var app = builder.Build();

                app.UseSerilogRequestLogging(); //замена логирования, используемого по умолчанию в ASP.NET Core, 
                                                //на ведение журнала запросов Serilog

                app.UseHttpsRedirection();

                app.UseRouting();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.Run();
            }
            catch (Exception ex) //обработка ошибок запуска: логируется критическая ошибка при аварийном завершении, а затем закрываются и очищаются все логи
            {
                Log.Fatal(ex, "server terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
