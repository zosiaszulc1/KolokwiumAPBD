using APBD_Kolokwium2.Entities;
using Microsoft.EntityFrameworkCore;

namespace APBD_Kolokwium2.Data;

public class AppDbContext : DbContext
{
 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<ProductDelivery> ProductDeliveries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.HasKey(e => e.ProductId);
            
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Price)
                .HasColumnName("price")
                .HasPrecision(10, 2)
                .IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");
            entity.HasKey(e => e.CustomerId);
            
            entity.Property(e => e.CustomerId)
                .HasColumnName("customer_id");
            
            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.DateOfBirth)
                .HasColumnName("date_of_birth")
                .HasColumnType("datetime")
                .IsRequired();
            
        });

        modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("Driver");
                entity.HasKey(e => e.DriverId);

                entity.Property(e => e.DriverId)
                    .HasColumnName("driver_id");
                
                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(100)
                    .IsRequired();
                
                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(100)
                    .IsRequired();
                
                entity.Property(e => e.LicenceNumber)
                    .HasColumnName("licence_number")
                    .HasMaxLength(17)
                    .IsRequired();


            });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("Delivery");
            entity.HasKey(e => e.DeliveryId);

            entity.Property(e => e.DeliveryId)
                .HasColumnName("delivery_id");

            entity.Property(e => e.CustomerId)
                .HasColumnName("customer_id");

            entity.Property(e => e.DriverId)
                .HasColumnName("driver_id");

            entity.Property(e => e.Date)
                .HasColumnName("date")
                .HasColumnType("datetime")
                .IsRequired();

            entity.HasOne(e => e.Customer)
                .WithMany(e => e.Deliveries)
                .HasForeignKey(e => e.CustomerId);

            entity.HasOne(e => e.Driver)
                .WithMany(e => e.Deliveries)
                .HasForeignKey(e => e.DriverId);

        });

        modelBuilder.Entity<ProductDelivery>(entity =>
        {
            entity.ToTable("Product_Delivery");
            entity.HasKey(e => new { e.ProductId, e.DeliveryId });

            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");

            entity.Property(e => e.DeliveryId)
                .HasColumnName("delivery_id");

            entity.Property(e => e.Amount)
                .HasColumnName("amount")
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany(e => e.ProductDeliveries)
                .HasForeignKey(e => e.ProductId);

            entity.HasOne(e => e.Delivery)
                .WithMany(e => e.ProductDeliveries)
                .HasForeignKey(e => e.DeliveryId);
        });



    }
    
    
}