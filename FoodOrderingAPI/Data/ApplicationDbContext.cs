using Microsoft.EntityFrameworkCore;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryMan> DeliveryMen { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderID);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)");
                
                // Configure relationships
                entity.HasOne(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(e => e.CustomerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DeliveryMan)
                    .WithMany(d => d.Orders)
                    .HasForeignKey(e => e.DeliveryManID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Restaurant)
                    .WithMany()
                    .HasForeignKey(e => e.RestaurantID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Address)
                    .WithMany()
                    .HasForeignKey(e => e.AddressID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.PromoCode)
                    .WithMany()
                    .HasForeignKey(e => e.PromoCodeID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure DeliveryMan entity
            modelBuilder.Entity<DeliveryMan>(entity =>
            {
                entity.HasKey(e => e.DeliveryManID);
                entity.Property(e => e.Rank).HasDefaultValue(0);
                entity.Property(e => e.AvailabilityStatus).HasDefaultValue(true);
                entity.Property(e => e.AccountStatus).HasDefaultValue(AccountStatusEnum.Pending);

                entity.HasOne(e => e.User)
                    .WithOne()
                    .HasForeignKey<DeliveryMan>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}