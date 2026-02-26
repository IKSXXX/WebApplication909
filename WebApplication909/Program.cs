
using WebApplication909;
using WebApplication909.Interfaces;
using WebApplication909.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IProductsRepository, InMemoryProductsRepository>();
builder.Services.AddSingleton<ICartsRepository, InMemoryCartsRepository>();
builder.Services.AddSingleton<IOrdersRepository, InMemoryOrdersRepository>();
builder.Services.AddSingleton<IFavoritesRepository, InMemoryFavouritesRepository>();
builder.Services.AddSingleton<IComparisonsRepository, InMemoryComparisonsRepository>();


var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();