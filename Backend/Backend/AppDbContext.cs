using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.ContactNumber).HasMaxLength(15);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(50).HasDefaultValue("Customer");
                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasMany(u => u.Addresses)
                      .WithOne(a => a.User)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(a => a.LastName).HasMaxLength(50).IsRequired();
                entity.Property(a => a.ContactNumber).HasMaxLength(15).IsRequired();
                entity.Property(a => a.Street).IsRequired().HasMaxLength(255);
                entity.Property(a => a.City).IsRequired().HasMaxLength(100);
                entity.Property(a => a.State).IsRequired().HasMaxLength(100);
                entity.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);

                entity.HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // Category entity configuration
            //modelBuilder.Entity<Category>(entity =>
            //{
            //    entity.HasKey(c => c.Id);
            //    entity.Property(c => c.Name).IsRequired().HasMaxLength(255);
            //    entity.HasMany(c => c.Products)
            //          .WithOne(p => p.Category)
            //          .HasForeignKey(p => p.CategoryId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(c => c.Level)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevents cascading deletes
            });

            // Product entity configuration
            //modelBuilder.Entity<Product>(entity =>
            //{
            //    entity.HasKey(p => p.Id);
            //    entity.Property(p => p.Name).IsRequired().HasMaxLength(255);
            //    entity.Property(p => p.Description).HasMaxLength(1000);
            //    entity.Property(p => p.Specifications).HasMaxLength(1000);
            //    entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            //    entity.Property(p => p.Brand).IsRequired().HasMaxLength(255);

            //    entity.HasOne(p => p.Category)
            //          .WithMany(c => c.Products)
            //          .HasForeignKey(p => p.CategoryId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});

            modelBuilder.Entity<Product>(entity =>
            {
                // Primary Key
                entity.HasKey(p => p.Id);

                // Properties
                entity.Property(p => p.Title)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(p => p.Description)
                      .IsRequired();

                entity.Property(p => p.Price)
                      .IsRequired();

                entity.Property(p => p.DiscountedPrice)
                      .IsRequired();

                entity.Property(p => p.DiscountPercent)
                      .IsRequired();

                entity.Property(p => p.Quantity)
                      .IsRequired();

                entity.Property(p => p.Brand)
                      .HasMaxLength(100); // Optional, max length

                entity.Property(p => p.Color)
                      .HasMaxLength(50); // Optional, max length

                entity.Property(p => p.ImageUrl)
                      .HasMaxLength(500); // Optional, max length

                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("GETUTCDATE()");

                // Relationships
                entity.HasOne(p => p.Category)
                      .WithMany()
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Sizes)
                      .WithOne(ps => ps.Product)
                      .HasForeignKey(ps => ps.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Ratings)
                      .WithOne(r => r.Product)
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Reviews)
                      .WithOne(r => r.Product)
                      .HasForeignKey(r => r.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ProductSize entity
            modelBuilder.Entity<ProductSize>(entity =>
            {
                entity.HasKey(ps => ps.Id);

                entity.Property(ps => ps.SizeName).IsRequired().HasMaxLength(50);
                entity.Property(ps => ps.Quantity).IsRequired();

                entity.HasOne(ps => ps.Product)
                      .WithMany(p => p.Sizes)
                      .HasForeignKey(ps => ps.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ProductRating entity
            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(pr => pr.Id);

                entity.Property(pr => pr.RatingNumber).IsRequired();

                entity.HasOne(pr => pr.Product)
                      .WithMany(p => p.Ratings)
                      .HasForeignKey(pr => pr.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pr => pr.User)
                      .WithMany()
                      .HasForeignKey(pr => pr.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Review entity configuration
            //modelBuilder.Entity<Review>(entity =>
            //{
            //    entity.HasKey(r => r.Id);
            //    entity.Property(r => r.Comment).HasMaxLength(1000);
            //    entity.Property(r => r.Rating).IsRequired();

            //    entity.HasOne(r => r.Product)
            //          .WithMany(p => p.Reviews)
            //          .HasForeignKey(r => r.ProductId)
            //          .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasOne(r => r.User)
            //          .WithMany()
            //          .HasForeignKey(r => r.UserId)
            //          .OnDelete(DeleteBehavior.Restrict);
            //});

            // Configure ProductReview entity
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(pr => pr.Id);

                entity.Property(pr => pr.ReviewText).IsRequired();

                entity.Property(pr => pr.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(pr => pr.Product)
                      .WithMany(p => p.Reviews)
                      .HasForeignKey(pr => pr.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pr => pr.User)
                      .WithMany()
                      .HasForeignKey(pr => pr.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Cart entity configuration
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.TotalPrice).IsRequired().HasDefaultValue(0);
                entity.Property(c => c.TotalDiscountedPrice).IsRequired().HasDefaultValue(0);
                entity.Property(c => c.TotalItems).IsRequired().HasDefaultValue(0);
                entity.Property(c => c.Discount).IsRequired().HasDefaultValue(0);

                entity.HasOne(c => c.User)
                      .WithOne()
                      .HasForeignKey<Cart>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.CartItems)
                      .WithOne(ci => ci.Cart)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CartItem entity configuration
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);
                entity.Property(ci => ci.Quantity).IsRequired().HasDefaultValue(1);
                entity.Property(ci => ci.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(ci => ci.DiscountedPrice).IsRequired();
                entity.Property(ci => ci.Size).IsRequired();

                entity.HasOne(ci => ci.Cart)
                      .WithMany(c => c.CartItems)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Product)
                      .WithMany()
                      .HasForeignKey(ci => ci.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ci => ci.User)
                      .WithMany()
                      .HasForeignKey(ci => ci.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Order entity configuration
            //modelBuilder.Entity<Order>(entity =>
            //{
            //    entity.HasKey(o => o.Id);
            //    entity.Property(o => o.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            //    entity.Property(o => o.OrderStatus).IsRequired().HasMaxLength(50);
            //    modelBuilder.Entity<Order>().Property(o => o.PaymentStatus).HasDefaultValue("Pending");
            //    entity.Property(o => o.ShippingAddress).IsRequired().HasMaxLength(500);
            //    entity.Property(o => o.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
            //    entity.Property(o => o.UpdatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");

            //    entity.HasOne(o => o.User)
            //          .WithMany()
            //          .HasForeignKey(o => o.UserId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.HasOne(o => o.User)
                    .WithMany()
                    .HasForeignKey(o => o.UserId)   
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.ShippingAddress)
                    .WithMany()
                    .HasForeignKey(o => o.ShippingAddressId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(o => o.TotalPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(o => o.TotalDiscountedPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(o => o.Discount)
                    .HasColumnType("decimal(18,2)");

                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem entity configuration
            //modelBuilder.Entity<OrderItem>(entity =>
            //{
            //    entity.HasKey(oi => oi.Id);
            //    entity.Property(oi => oi.Price).IsRequired().HasColumnType("decimal(18,2)");
            //    entity.Property(oi => oi.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            //    entity.Property(oi => oi.Quantity).IsRequired();

            //    entity.HasOne(oi => oi.Order)
            //          .WithMany(o => o.OrderItems)
            //          .HasForeignKey(oi => oi.OrderId)
            //          .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasOne(oi => oi.Product)
            //          .WithMany()
            //          .HasForeignKey(oi => oi.ProductId)
            //          .OnDelete(DeleteBehavior.Restrict);
            //});

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(oi => oi.User)
                    .WithMany()
                    .HasForeignKey(oi => oi.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(oi => oi.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(oi => oi.DiscountedPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(oi => oi.Size);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.PaymentGateway)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18, 2)")
                      .IsRequired();

                entity.Property(e => e.Currency)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.Status)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.TransactionId)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.HasIndex(e => e.TransactionId)
                      .IsUnique();

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()")
                      .IsRequired();

                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql("GETDATE()")
                      .IsRequired();

                entity.HasOne<Order>()
                      .WithMany()
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
