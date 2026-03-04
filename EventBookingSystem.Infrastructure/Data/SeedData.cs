using EventBookingSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.DependencyInjection;


namespace EventBookingSystem.Infrastructure.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        // Create a scope for scoped services
        using var scope = services.GetService<IServiceScopeFactory>()!.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Apply pending migrations if any
        try
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
        catch
        {
            // ignore migration errors here - assume migrations were applied by tooling
        }

        // Seed users and wallets
        // Seed admins (separate admin records)
        if (!await context.Set<Admin>().AnyAsync())
        {
            var admins = new List<Admin>
            {
                new Admin
                {
                    Email = "admin@eventhub.com",
                    Password = HashPassword("AdminSecure123!")
                },
                new Admin
                {
                    Email = "support@eventhub.com",
                    Password = HashPassword("AdminSecure123!")
                },
                new Admin
                {
                    Email = "operations@eventhub.com",
                    Password = HashPassword("AdminSecure123!")
                }
            };

            await context.AddRangeAsync(admins);
            await context.SaveChangesAsync();
        }

        // Seed users and wallets
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new User
            {
                Email = "abdul.salaudeen@gmail.com",
                FirstName = "Abdul",
                LastName = "Salaudeen",
                FullName = "Abdul Salaudeen",
                PasswordHash = HashPassword("StrongPass123!")
            },
            new User
            {
                Email = "chinedu.okafor@gmail.com",
                FirstName = "Chinedu",
                LastName = "Okafor",
                FullName = "Chinedu Okafor",
                PasswordHash = HashPassword("StrongPass123!")
            },
            new User
            {
                Email = "amina.bello@gmail.com",
                FirstName = "Amina",
                LastName = "Bello",
                FullName = "Amina Bello",
                PasswordHash = HashPassword("StrongPass123!")
            },
            new User
            {
                Email = "tunde.adeyemi@gmail.com",
                FirstName = "Tunde",
                LastName = "Adeyemi",
                FullName = "Tunde Adeyemi",
                PasswordHash = HashPassword("StrongPass123!")
            }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            // Create wallets for seeded users
            foreach (var u in users)
            {
                var wallet = new Wallet { UserId = u.Id, Balance = 100.00m };
                await context.Wallets.AddAsync(wallet);
            }
            await context.SaveChangesAsync();
        }

        // Seed events
        if (!await context.Events.AnyAsync())
        {
            var now = DateTime.UtcNow;
            var events = new List<Event>
            {
                new Event
                {
                    Title = "Summer Music Festival",
                    Description = "Enjoy live music from top artists.",
                    EventDate = now.AddDays(30),
                    TicketPrice = 49.99m,
                    TotalTickets = 500,
                    AvailableTickets = 500
                },
                new Event
                {
                    Title = "Tech Conference 2026",
                    Description = "A gathering of technology enthusiasts and professionals.",
                    EventDate = now.AddDays(60),
                    TicketPrice = 199.00m,
                    TotalTickets = 300,
                    AvailableTickets = 300
                }
            };

            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();
        }
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
