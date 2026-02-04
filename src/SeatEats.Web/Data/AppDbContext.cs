using Microsoft.EntityFrameworkCore;
using SeatEats.Domain.Entities;

namespace SeatEats.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasConversion<int>();
            entity.OwnsOne(e => e.SeatLocation, sl =>
            {
                sl.Property(s => s.Section).HasColumnName("Section");
                sl.Property(s => s.Row).HasColumnName("Row");
                sl.Property(s => s.SeatNumber).HasColumnName("SeatNumber");
            });
            entity.OwnsMany(e => e.Items, item =>
            {
                item.WithOwner().HasForeignKey("OrderId");
                item.Property<Guid>("Id");
                item.HasKey("Id");
            });
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}
