using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.ProductId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.Alias)
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.CategoryId)
                .IsRequired();

            builder.Property(p => p.UnitDescription)
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.UnitPrice)
                .IsRequired();

            builder.Property(p => p.ImageURL)
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.Discount)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnType("nvarchar(max)");

            builder.Property(p => p.ProviderId)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pv => pv.Provider)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
