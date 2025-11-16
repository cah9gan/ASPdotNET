using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;
using TransportStore.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TransportStore.Models
{
	public static class SeedData
	{
		public static void EnsurePopulated(IApplicationBuilder app)
		{
			StoreDbContext context = app.ApplicationServices
				.CreateScope().ServiceProvider
				.GetRequiredService<StoreDbContext>();

			if (context.Database.GetPendingMigrations().Any())
			{
				context.Database.Migrate();
			}

			if (!context.Transports.Any())
			{
				context.Transports.AddRange(
				new Transport
				{
					Type = "car",
                    Model = "s1",
                    PricePerHour = 200,
				},
				new Transport
				{
					Type = "car",
                    Model = "s2",
                    PricePerHour = 300,
				},
				new Transport
				{
					Type = "car",
                    Model = "s3",
                    PricePerHour = 400,
				},
				new Transport
				{
					Type = "car",
                    Model = "s4",
                    PricePerHour = 500,
				},
				new Transport
				{
					Type = "car",
                    Model = "s5",
                    PricePerHour = 600,
				},
				new Transport
				{
					Type = "car",
                    Model = "s6",
                    PricePerHour = 700,
				},
				new Transport
				{
					Type = "car",
                    Model = "s7",
                    PricePerHour = 800,
				}
				);
				context.SaveChanges();
			}
		}
	}
}
