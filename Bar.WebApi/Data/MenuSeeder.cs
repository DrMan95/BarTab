using Bar.WebApi.Data;
using Bar.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

public static class MenuSeeder
{
    public static async Task SeedMenuAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BarDbContext>();

        // Correct check: do we HAVE migrations in this project?
        var hasAnyMigrations = db.Database.GetMigrations().Any();

        if (hasAnyMigrations)
        {
            // Creates DB + tables (from migrations) if missing
            await db.Database.MigrateAsync();
        }
        else
        {
            // Creates DB + tables (from model) if no migrations exist
            await db.Database.EnsureCreatedAsync();
        }

        // Now it's safe to query MenuItems
        if (await db.MenuItems.AnyAsync())
            return;

        db.MenuItems.AddRange(
            new MenuItem { Name = "Espresso", Category = "Coffee", Price = 2.50m, Active = true, StockQuantity = null },
            new MenuItem { Name = "Draft Beer 500ml", Category = "Beer", Price = 4.50m, Active = true, StockQuantity = 50 }
        );

        await db.SaveChangesAsync();
    }
}
