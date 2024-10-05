using System;
using System.Collections.Generic;
using Craftera_MVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Craftera_MVC.Models
{
    public partial class EXE202_CrafteraContext : DbContext
    {
        public EXE202_CrafteraContext()
        {
        }

        public EXE202_CrafteraContext(DbContextOptions<EXE202_CrafteraContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Material> Materials { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Size> Sizes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserDetail> UserDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var conf = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

                optionsBuilder.UseSqlServer(conf.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(e => e.TotalMoney).HasColumnType("money");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Carts_Users");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).HasMaxLength(500);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_Items_Carts");

                entity.HasOne(d => d.ProductDetail)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => new { d.ProductId, d.SizeId, d.MaterialId })
                    .HasConstraintName("FK_Items_ProductDetails");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.MaterialName).HasMaxLength(500);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.AcceptedDate).HasColumnType("date");

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.ReceivedDate).HasColumnType("date");

                entity.Property(e => e.TotalMoney).HasColumnType("money");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_Orders_Payment");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.ProductDetail)
                    .WithMany()
                    .HasForeignKey(d => new { d.ProductId, d.SizeId, d.MaterialId })
                    .HasConstraintName("FK_OrderDetails_ProductDetails");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentName).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductName).HasMaxLength(500);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Categories");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.SizeId, e.MaterialId });

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductDetails_Materials");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductDetails_Products");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductDetails_Sizes");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(e => e.SizeName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserDetail)
                    .HasForeignKey<UserDetail>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserDetails_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
