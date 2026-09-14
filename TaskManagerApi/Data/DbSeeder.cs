using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;
using TaskManagerApi.Services;

namespace TaskManagerApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedUsersAsync(
            AppDbContext context,
            IConfiguration configuration,
            IPasswordHasher passwordHasher)
        {
            if (await context.Users.AnyAsync())
                return;

            var adminPassword =
                configuration["SeedUsers:AdminPassword"];

            var userPassword =
                configuration["SeedUsers:UserPassword"];

            if (string.IsNullOrWhiteSpace(adminPassword) ||
                string.IsNullOrWhiteSpace(userPassword))
            {
                throw new InvalidOperationException(
                    "Seed user passwords are missing. Configure " +
                    "SeedUsers:AdminPassword and " +
                    "SeedUsers:UserPassword using User Secrets.");
            }

            context.Users.AddRange(
                new User
                {
                    Username = "admin",
                    PasswordHash =
                        passwordHasher.Hash(adminPassword),
                    Role = "Admin"
                },
                new User
                {
                    Username = "user",
                    PasswordHash =
                        passwordHasher.Hash(userPassword),
                    Role = "User"
                });

            await context.SaveChangesAsync();
        }
    }
}