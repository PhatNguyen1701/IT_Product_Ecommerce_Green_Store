using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(od => od.OrderDetailId);

            builder.Property(od => od.OrderDetailId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(od => od.OrderId)
                .IsRequired();

            builder.Property(od => od.ProductId)
                .IsRequired();

            builder.Property(od => od.Price)
                .IsRequired();

            builder.Property(od => od.Quantity)
                .IsRequired();

            builder.HasOne(c => c.Order)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
