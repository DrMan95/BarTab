using Bar.WebApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bar.WebApi.Data
{
    public class BarDbContext : DbContext
    {
        public BarDbContext(DbContextOptions<BarDbContext> options)
            : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems => Set<MenuItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var menu = modelBuilder.Entity<MenuItem>();

            menu.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200);

            menu.Property(m => m.Category)
                .IsRequired()
                .HasMaxLength(100);

            menu.Property(m => m.Price)
                .HasColumnType("decimal(10,2)");

            // You can seed a few items here if you like:
            // menu.HasData(
            //     new MenuItem { Id = 1, Name = "Espresso", Category = "Coffee", Price = 2.50m, Active = true, StockQuantity = null }
            // );
        }
    }
}
