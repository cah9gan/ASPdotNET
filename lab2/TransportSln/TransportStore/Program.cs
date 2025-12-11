using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportStore.Domain.Models; // Added this namespace to find DbContexts and Repositories
using TransportStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => 
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddDbContext<StoreDbContext>(opts => {
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:TransportStoreConnection"],
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    );
});

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:IdentityConnection"],
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    ));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => {
    opts.SignIn.RequireConfirmedAccount = false;
    opts.SignIn.RequireConfirmedEmail = false;
    opts.SignIn.RequireConfirmedPhoneNumber = false;

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