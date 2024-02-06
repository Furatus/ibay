using ibay.Model;
using Microsoft.EntityFrameworkCore;

namespace ibay;

public class IbayContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Product>? Products { get; set; }
    public DbSet<Cart>? Carts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=192.168.242.135;Database=ibay_api;Username=postgres;Password=postgres");
        base.OnConfiguring(optionsBuilder);

    }
}