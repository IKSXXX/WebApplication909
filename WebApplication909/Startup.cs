using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication909.Interfaces;
using WebApplication909.Repositories;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Repositories;
using OnlineShop.Db;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Repositories;

namespace WebApplication909
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Подключение к PostgreSQL
            var connectionString = Configuration.GetConnectionString("OnlineShopConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(connectionString));   // ВАЖНО: UseNpgsql

            // Регистрация репозиториев
            services.AddScoped<IProductsRepository, DbProductsRepository>();
            services.AddScoped<ICartsRepository, DbCartsRepository>();
            services.AddScoped<IFavoritesRepository, DbFavouritesRepository>();
            services.AddScoped<IComparisonsRepository, DbComparisonsRepository>();
            services.AddScoped<IOrdersRepository, InMemoryOrdersRepository>();
            services.AddScoped<IUsersRepository, InMemoryUsersRepository>();
            services.AddScoped<IRolesRepository, InMemoryRolesRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Применение миграций (создание таблиц, если их нет)
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.Migrate();   // Создаст таблицы через миграции
        }
    }
}