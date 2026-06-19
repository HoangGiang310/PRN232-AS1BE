using Microsoft.EntityFrameworkCore;
using SimpleShop.Repo.Models;

namespace SimpleShop.Repo.Data;

public class SimpleShopDbContext : DbContext
{
    public SimpleShopDbContext(DbContextOptions<SimpleShopDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.HasKey(e => e.CategoryID);
            entity.Property(e => e.CategoryID).HasColumnName("CategoryID").ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).HasColumnName("CategoryName").IsRequired().HasMaxLength(100);
            entity.Property(e => e.CategoryDescription).HasColumnName("CategoryDescription").IsRequired().HasMaxLength(250);
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.HasKey(e => e.ProductID);
            entity.Property(e => e.ProductID).HasColumnName("ProductID").ValueGeneratedOnAdd();
            entity.Property(e => e.ProductName).HasColumnName("ProductName").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasColumnName("Description");
            entity.Property(e => e.Price).HasColumnName("Price").HasColumnType("numeric(18,2)");
            entity.Property(e => e.StockQuantity).HasColumnName("StockQuantity").HasDefaultValue(0);
            entity.Property(e => e.ImageUrl).HasColumnName("ImageUrl").HasMaxLength(500);
            entity.Property(e => e.CategoryID).HasColumnName("CategoryID").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.ModifiedDate).HasColumnName("ModifiedDate");

            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryID)
                  .HasConstraintName("FK_Product_Category");
        });
    }
}
