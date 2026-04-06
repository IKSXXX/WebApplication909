using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineShop.Db;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Repositories;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Repositories;

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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Authorization";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromDays(14);
                    options.SlidingExpiration = true;
                });

            var connectionString = Configuration.GetConnectionString("OnlineShopConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(connectionString));

            // Репозитории
            services.AddScoped<IProductsRepository, DbProductsRepository>();
            services.AddScoped<ICartsRepository, DbCartsRepository>();
            services.AddScoped<IFavoritesRepository, DbFavouritesRepository>();
            services.AddScoped<IComparisonsRepository, DbComparisonsRepository>();
            services.AddScoped<IOrdersRepository, DbOrdersRepository>();
            services.AddScoped<IRolesRepository, InMemoryRolesRepository>();
            services.AddScoped<IUsersRepository>(provider =>
                new InMemoryUsersRepository(provider.GetRequiredService<IRolesRepository>()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // МИГРАЦИИ — ТОЛЬКО ЗДЕСЬ (один раз при старте приложения)
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.Migrate();
        }
    }
}