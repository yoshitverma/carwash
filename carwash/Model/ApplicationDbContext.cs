using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carwash.Model;
using Microsoft.EntityFrameworkCore;
namespace carwash.Migrations
{
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Leaderboard> Leaderboards { get; set; }
    public DbSet<Washer> Washers { get; set; }
    public ApplicationDbContext (DbContextOptions options) : base(options) 
    {

    }
    // Seed the admin user
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed admin credentials
        modelBuilder.Entity<User>().HasData(new User
        {
            UserId = 1,
            Email = "admin@greenwash.com",
            Password = "admin123", // This should be hashed in a real-world application
            Name = "Admin User",
            IsActive = true,
            Role = "Admin"
        });

        modelBuilder.Entity<Order>()
        .HasOne(o => o.User) // Each order has one user (customer)
        .WithMany(u => u.Orders) // A user can have many orders
        .HasForeignKey(o => o.UserId) // Foreign key in Order table
        .OnDelete(DeleteBehavior.Restrict); // Optionally define delete behavior (Restrict, Cascade, etc.)

    // Configure relationship between Order and Washer
    modelBuilder.Entity<Order>()
        .HasOne(o => o.Washer) // Each order has one washer
        .WithMany() // A washer can have many orders (no navigation property in Washer)
        .HasForeignKey(o => o.WasherId) // Foreign key in Order table
        .OnDelete(DeleteBehavior.Restrict); // Optionally define delete behavior (Restrict, Cascade, etc.)
    }
}

}
