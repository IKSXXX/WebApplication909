using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShop.Db;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Repositories;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Repositories;
using WebApplication909.Models; // для ViewModel, не для User

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

            var connectionString = Configuration.GetConnectionString("OnlineShopConnection");

            // === Контекст для Identity ===
            services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(connectionString));

            // === Контекст для основного приложения ===
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(connectionString));

            // === Настройка Identity ===
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            // === Регистрация репозиториев ===
            services.AddTransient<IProductsRepository, DbProductsRepository>();
            services.AddTransient<ICartsRepository, DbCartsRepository>();
            services.AddTransient<IFavoritesRepository, DbFavouritesRepository>();
            services.AddTransient<IComparisonsRepository, DbComparisonsRepository>();
            services.AddTransient<IOrdersRepository, DbOrdersRepository>();

            // === Регистрация сервисов для работы с пользователями (Identity уже сделал это) ===
            // IUsersRepository и IRolesRepository больше не нужны, если вы переходите на Identity.
            // Если они всё же используются в админке, их нужно заменить на UserManager/RoleManager.
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

            // Добавляем аутентификацию и авторизацию (обязательно для Identity)
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // === Автоматическое применение миграций для обоих контекстов ===
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                dbContext.Database.Migrate();

                var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                identityContext.Database.Migrate();
            }
        }
    }
}