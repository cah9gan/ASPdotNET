using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; 
using System;
using System.Linq;

namespace TransportStore.Domain.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                var context = scopedServices.GetRequiredService<StoreDbContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Transports.Any())
                {
                    context.Transports.AddRange(
                        new Transport
                        {
                            Type = "Автомобіль",
                            Model = "Toyota Camry",
                            Description = "Комфортний седан бізнес-класу. Кондиціонер, шкіряний салон.",
                            PricePerHour = 250
                        },
                        new Transport
                        {
                            Type = "Автомобіль",
                            Model = "Tesla Model 3",
                            Description = "Сучасний електромобіль. Автопілот, панорамний дах.",
                            PricePerHour = 400
                        },
                        new Transport
                        {
                            Type = "Велосипед",
                            Model = "Giant Escape 3",
                            Description = "Легкий міський велосипед для прогулянок парком.",
                            PricePerHour = 50
                        },
                        new Transport
                        {
                            Type = "Велосипед",
                            Model = "Trek Marlin 5",
                            Description = "Гірський велосипед з амортизацією для активного відпочинку.",
                            PricePerHour = 60
                        },
                        new Transport
                        {
                            Type = "Самокат",
                            Model = "Xiaomi Mi Scooter Pro 2",
                            Description = "Електросамокат із запасом ходу до 45 км.",
                            PricePerHour = 40
                        },
                        new Transport
                        {
                            Type = "Самокат",
                            Model = "Ninebot Max G30",
                            Description = "Потужний самокат для довгих поїздок містом.",
                            PricePerHour = 45
                        },
                        new Transport
                        {
                            Type = "Яхта",
                            Model = "Azimut 55",
                            Description = "Розкішна яхта для вечірок та відпочинку на воді.",
                            PricePerHour = 5000
                        }
                    );
                    context.SaveChanges();
                }

                var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();

                var adminUser = userManager.FindByNameAsync("admin").Result;
                if (adminUser == null)
                {
                    adminUser = new IdentityUser { UserName = "admin", Email = "admin@transport.store", EmailConfirmed = true };
                    var result = userManager.CreateAsync(adminUser, "Secret123$").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                    }
                }

                var standardUser = userManager.FindByNameAsync("user").Result;
                if (standardUser == null)
                {
                    standardUser = new IdentityUser { UserName = "user", Email = "user@transport.store", EmailConfirmed = true };
                    var result = userManager.CreateAsync(standardUser, "Secret123$").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(standardUser, "User").Wait();
                    }
                }
            }
        }
    }
}