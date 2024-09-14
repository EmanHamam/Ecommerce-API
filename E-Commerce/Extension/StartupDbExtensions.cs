using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using EFCore.AutomaticMigrations;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Extension
{
    public static class StartupDbExtensions
    {
        public static async void CreateDbIfNotExists(this IHost host)
        {
            using var scope =host.Services.CreateScope();
            var services=scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            var usermanager = services.GetRequiredService<UserManager<ApplicationUser>>();

            context.Database.EnsureCreated();
            context.MigrateToLatestVersion();
            StoredContextSeed.SeedAsync(context);
            IdentitySeed.SeedUserAsync(usermanager);

            

        }
    }
}
