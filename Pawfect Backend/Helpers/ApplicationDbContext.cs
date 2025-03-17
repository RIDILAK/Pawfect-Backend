using Microsoft.EntityFrameworkCore;
using Pawfect_Backend.Models;

namespace Pawfect_Backend.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<WishList>WishList { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>()
                .HasOne(p => p.category)
                .WithMany(c => c.Product)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<WishList>()
                .HasOne(w => w.User)
                .WithMany(u => u.wishList)
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<WishList>()
                .HasOne(w => w.Product)
                .WithMany(p => p.wishList)
                .HasForeignKey(w => w.ProductId);

            modelBuilder.Entity<Cart>()
                .HasMany(c => c.CartItems)
                .WithOne(c => c.Cart)
                .HasForeignKey(c => c.CartId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Order>()
     .HasOne(o => o.User)
     .WithMany(u => u.Orders)
     .HasForeignKey(o => o.userId)
     .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>() 
                .HasOne(o => o.Address)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.TransactionId)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderStatus)
                .HasDefaultValue("Pending");




                modelBuilder.Entity<User>().HasData(
                        new User { Id = 2, Name = "Admin", Email = "Admin@gmail.com", Password = "$2a$10$Pp53mB2wEb.Ku3Yy/FQ7AuFs.1HnFUMZ0h9.ZRHQvOlpoFdcxYH4m", Role = "Admin" },
                        new User { Id = 8, Name = "Admin2", Email = "Admin2@gmail.com", Password = "$2a$10$hLKRni9HNoLgVZsE8cYqUeR2KxgoHOhHr1EFVIXACaAqZJYjYsm9W", Role = "Admin", isBlocked = false },
                        new User { Id = 1, Name = "admin", Email = "admin@gmail.com", Password = "$2b$10$9gTfHIrTSjyOSbgMeSaFteRBqPJzaHdjAnnXpZ9JWKUT6MuH1b8pu", Role = "Admin", isBlocked = false });
                }
        }


    }



