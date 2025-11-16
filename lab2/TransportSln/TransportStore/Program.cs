using Microsoft.EntityFrameworkCore;
using TransportStore.Models; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:TransportStoreConnection"]);
});

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();


var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();


SeedData.EnsurePopulated(app);

app.Run();
