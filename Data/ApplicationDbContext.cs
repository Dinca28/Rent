using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentACars.Models;
using System;
using System.Linq;

namespace RentACars.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarRental> CarRentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasColumnType("DECIMAL(18,2)");
        }
    }

    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            if (!context.Cars.Any())
            {
                var user = userManager.FindByEmailAsync("admin@rentacar.com").Result;

                if (user == null)
                {
                    var newUser = new IdentityUser { UserName = "admin@rentacar.com", Email = "admin@rentacar.com" };
                    userManager.CreateAsync(newUser, "Password123!").Wait();
                    user = newUser;
                }

                context.Cars.AddRange(
                    new Car { Brand = "BMW", Model = "X5", Year = 2020, PricePerDay = 150.00, Location = "Sofia", UserId = user.Id, ImageUrl = "/images/m4.jpg" },
                    new Car { Brand = "Audi", Model = "A4", Year = 2021, PricePerDay = 120.00, Location = "Plovdiv", UserId = user.Id, ImageUrl = "/images/a.jpg" },
                    new Car { Brand = "Mercedes", Model = "E-Class", Year = 2022, PricePerDay = 200.00, Location = "Varna", UserId = user.Id, ImageUrl = "/images/e.jpg" },
                    new Car { Brand = "Tesla", Model = "Model 3", Year = 2021, PricePerDay = 250.00, Location = "Sofia", UserId = user.Id, ImageUrl = "/images/tesla_model_3.jpg" },
                    new Car { Brand = "Ford", Model = "Focus", Year = 2019, PricePerDay = 80.00, Location = "Burgas", UserId = user.Id, ImageUrl = "/images/ford_focus.jpg" },
                    new Car { Brand = "Volkswagen", Model = "Golf", Year = 2020, PricePerDay = 100.00, Location = "Plovdiv", UserId = user.Id, ImageUrl = "/images/volkswagen_golf.jpg" },
                    new Car { Brand = "Audi", Model = "Q7", Year = 2022, PricePerDay = 220.00, Location = "Sofia", UserId = user.Id, ImageUrl = "/images/audi_q7.jpg" },
                    new Car { Brand = "BMW", Model = "M4", Year = 2021, PricePerDay = 300.00, Location = "Varna", UserId = user.Id, ImageUrl = "/images/bmw_m4.jpg" },
                    new Car { Brand = "Opel", Model = "Astra", Year = 2018, PricePerDay = 70.00, Location = "Sofia", UserId = user.Id, ImageUrl = "/images/opel_astra.jpg" },
                    new Car { Brand = "Renault", Model = "Megane", Year = 2017, PricePerDay = 65.00, Location = "Burgas", UserId = user.Id, ImageUrl = "/images/renault_megane.jpg" },
                    new Car { Brand = "Peugeot", Model = "208", Year = 2020, PricePerDay = 90.00, Location = "Plovdiv", UserId = user.Id, ImageUrl = "/images/peugeot_208.jpg" },
                    new Car { Brand = "Nissan", Model = "Qashqai", Year = 2021, PricePerDay = 130.00, Location = "Varna", UserId = user.Id, ImageUrl = "/images/nissan_qashqai.jpg" }
                );

                context.SaveChanges();
            }
        }
    }
}
