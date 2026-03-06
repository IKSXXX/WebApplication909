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
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db;
using OnlineShop.Db.Models; // если потребуется в этом файле

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

            // Регистрация DbContext
            var connectionString = Configuration.GetConnectionString("OnlineShopConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString)); // Используем SQL Server

            // Регистрация репозиториев (Scoped, так как DbContext Scoped)
            services.AddScoped<IProductsRepository, DbProductsRepository>();
            services.AddScoped<ICartsRepository, DbCartsRepository>();
            // Добавьте остальные репозитории по аналогии
            // services.AddScoped<IOrdersRepository, DbOrdersRepository>();
            // services.AddScoped<IFavoritesRepository, DbFavoritesRepository>();
            // services.AddScoped<IComparisonsRepository, DbComparisonsRepository>();
            // services.AddScoped<IUsersRepository, DbUsersRepository>(); (если есть)
            // services.AddScoped<IRolesRepository, DbRolesRepository>(); (если есть)
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
        }
    }
}