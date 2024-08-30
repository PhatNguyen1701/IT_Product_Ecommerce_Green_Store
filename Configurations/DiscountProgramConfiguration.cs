using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class DiscountProgramConfiguration : IEntityTypeConfiguration<DiscountProgram>
    {
        public void Configure(EntityTypeBuilder<DiscountProgram> builder)
        {
            builder.HasKey(d => d.DiscountId);

            builder.Property(d => d.DiscountId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(d => d.Title)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(d => d.CouponCode)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(d => d.Title)
                .IsRequired();

            builder.Property(d => d.IsActive)
                .IsRequired();

            builder.HasIndex(d => d.CouponCode)
                .IsUnique();
        }
    }
}
