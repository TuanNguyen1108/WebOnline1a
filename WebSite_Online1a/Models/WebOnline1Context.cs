using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebSite_Online1a.Models;

public partial class WebOnline1Context : DbContext
{
    public WebOnline1Context()
    {
    }

    public WebOnline1Context(DbContextOptions<WebOnline1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<OderDetail> OderDetails { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Specification> Specifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress; Database=WebOnline_1; Integrated Security=true; Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("ACCOUNT");

            entity.Property(e => e.AccountId).HasColumnName("Account_ID");
            entity.Property(e => e.Avatar).HasMaxLength(550);
            entity.Property(e => e.ConfirmPassword).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.HoTen).HasMaxLength(250);
            entity.Property(e => e.NewPassword).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.Sdt)
                .HasMaxLength(250)
                .HasColumnName("SDT");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_ACCOUNT_ROLE");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("BRAND");

            entity.Property(e => e.BrandId).HasColumnName("Brand_ID");
            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.NameBrand).HasMaxLength(250);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("CATEGORY");

            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.BrandId).HasColumnName("Brand_ID");
            entity.Property(e => e.CategoryImage).HasMaxLength(500);
            entity.Property(e => e.NameCategory).HasMaxLength(250);

            entity.HasOne(d => d.Brand).WithMany(p => p.Categories)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_CATEGORY_BRAND");
        });

        modelBuilder.Entity<OderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId);

            entity.ToTable("ODER_DETAILS");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetail_ID");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.PriceId).HasColumnName("Price_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.OderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_ODER_DETAILS_ORDER");

            entity.HasOne(d => d.PriceNavigation).WithMany(p => p.OderDetails)
                .HasForeignKey(d => d.PriceId)
                .HasConstraintName("FK_ODER_DETAILS_PRICE");

            entity.HasOne(d => d.Product).WithMany(p => p.OderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ODER_DETAILS_PRODUCT1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("ORDER");

            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.AccountId).HasColumnName("Account_ID");
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.OderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatus_ID");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_ORDER_ACCOUNT");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("ORDER_STATUS");

            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatus_ID");
            entity.Property(e => e.Status).HasMaxLength(250);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("POST");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.MetaDesc).HasMaxLength(255);
            entity.Property(e => e.MetaKey).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.ToTable("PRICE");

            entity.Property(e => e.PriceId).HasColumnName("Price_ID");
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.ColorImage).HasMaxLength(500);
            entity.Property(e => e.Gb).HasMaxLength(50);
            entity.Property(e => e.NamePrice).HasMaxLength(250);
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");

            entity.HasOne(d => d.Product).WithMany(p => p.Prices)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_PRICE_PRODUCT");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("PRODUCT");

            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.BrandId).HasColumnName("Brand_ID");
            entity.Property(e => e.CategoryId).HasColumnName("Category_ID");
            entity.Property(e => e.Gb).HasMaxLength(50);
            entity.Property(e => e.NameProduct).HasMaxLength(250);
            entity.Property(e => e.PhanTramGiamGia).HasMaxLength(50);
            entity.Property(e => e.ProductImage).HasMaxLength(500);
            entity.Property(e => e.SpecificationId).HasColumnName("Specification_ID");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_PRODUCT_BRAND");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_PRODUCT_CATEGORY1");

            entity.HasOne(d => d.Specification).WithMany(p => p.Products)
                .HasForeignKey(d => d.SpecificationId)
                .HasConstraintName("FK_PRODUCT_SPECIFICATION1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("ROLE");

            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Specification>(entity =>
        {
            entity.ToTable("SPECIFICATION");

            entity.Property(e => e.SpecificationId).HasColumnName("Specification_ID");
            entity.Property(e => e.Battery)
                .HasMaxLength(50)
                .HasColumnName("battery");
            entity.Property(e => e.Camera).HasMaxLength(50);
            entity.Property(e => e.CameraSelfie)
                .HasMaxLength(50)
                .HasColumnName("Camera_Selfie");
            entity.Property(e => e.Cpu).HasMaxLength(50);
            entity.Property(e => e.NameSpecification).HasMaxLength(250);
            entity.Property(e => e.OperatingSystem).HasMaxLength(50);
            entity.Property(e => e.Ram).HasMaxLength(50);
            entity.Property(e => e.SizeCreen).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
