using EFCoreProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Windows;

namespace EFCoreProject
{
    public partial class OnlineShopContext : DbContext
    {
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Delivery> Deliveries { get; set; } = null!;
        public virtual DbSet<DeliveryProduct> DeliveryProducts { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductGroup> ProductGroups { get; set; } = null!;
        public virtual DbSet<ProductOrder> ProductOrders { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<UnitMeasurement> UnitMeasurements { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {

                entity.ToTable("Client");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(50)
                    .IsUnicode(false);
               
                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnType("numeric(18, 0)");

                entity.HasMany(p => p.Orders)
                    .WithOne(d => d.Client)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Order_Client");

            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.ToTable("Delivery");

                entity.Property(e => e.DateDelivery).HasColumnType("date");

                entity.Property(e => e.Remark)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Deliveries)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Delivery_Supplier");
            });

            modelBuilder.Entity<DeliveryProduct>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Delivery_Product");

                entity.Property(e => e.DeliveryId).HasColumnName("Delivery_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.HasOne(d => d.Delivery)
                    .WithMany()
                    .HasForeignKey(d => d.DeliveryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Delivery_Product_Delivery");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Delivery_Product_Product");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.ClientId).HasColumnName("Client_Id");

                entity.Property(e => e.DateOrder).HasColumnType("date");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remark)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Order_Client");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductGroupId).HasColumnName("ProductGroup_Id");

                entity.Property(e => e.UnitMeasurementId).HasColumnName("UnitMeasurement_Id");

                entity.HasOne(d => d.ProductGroup)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductGroupId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Product_ProductGroup");

                entity.HasOne(d => d.UnitMeasurement)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UnitMeasurementId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Product_UnitMeasurement");
            });

            modelBuilder.Entity<ProductGroup>(entity =>
            {
                entity.ToTable("ProductGroup");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Product_Order");

                entity.Property(e => e.OrderId).HasColumnName("Order_Id");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Product_Order_Order");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Product_Order_Product");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber).HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<UnitMeasurement>(entity =>
            {
                entity.ToTable("UnitMeasurement");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
