using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITProductECommerce.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.CategoryId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.CategoryName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.Alias)
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.Description)
                .HasColumnType("nvarchar(max)");

            builder.Property(c => c.ImageURL)
                .HasColumnType("nvarchar(50)");
        }
    }
}
