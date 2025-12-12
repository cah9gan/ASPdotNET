using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportStore.Domain.Models;
using TransportStore.Models;
using TransportStore.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddControllersWithViews(options => 
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:TransportStoreConnection"],
        sqlServerOptions => {
            sqlServerOptions.EnableRetryOnFailure();
            sqlServerOptions.MigrationsAssembly("TransportStore"); 
        }
    );
});

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:IdentityConnection"],
        sqlServerOptions => {
            sqlServerOptions.EnableRetryOnFailure();
            sqlServerOptions.MigrationsAssembly("TransportStore"); 
        }
    ));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => {
    opts.SignIn.RequireConfirmedAccount = false;
    opts.Password.RequiredLength = 6; 
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = false; 
    opts.Password.RequireDigit = true;
    opts.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "TransportStoreAuth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.LoginPath = "/Account/Login"; 
    options.SlidingExpiration = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

app.UseStaticFiles();
app.UseSession(); 
app.UseRouting(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<TransportHub>("/transporthub");

app.MapDefaultControllerRoute();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };
    
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

SeedData.EnsurePopulated(app.Services);
app.Run();