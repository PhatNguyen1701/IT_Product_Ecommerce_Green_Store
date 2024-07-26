using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.OrderId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(o => o.CustomerId)
                .IsRequired()
                .HasColumnType("nvarchar(20)");

            builder.Property(o => o.OrderDate)
                .IsRequired();

            builder.Property(o => o.ReceiverName)
                .HasColumnType("nvarchar(50)");

            builder.Property(o => o.Address)
                .IsRequired()
                .HasColumnType("nvarchar(60)");

            builder.Property(o => o.PhoneNumber)
                .IsRequired()
                .HasColumnType("nvarchar(24)");

            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(o => o.TypeShipping)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(o => o.ShippingFee)
                .IsRequired();

            builder.Property(o => o.ReceiverName)
                .HasColumnType("nvarchar(50)");

            builder.Property(o => o.StatusId)
                .IsRequired();

            builder.Property(o => o.StaffId)
                .HasColumnType("nvarchar(50)");

            builder.Property(o => o.Note)
                .HasColumnType("nvarchar(50)");

            builder.HasOne(c => c.Customer)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Status)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Staff)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.StaffId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
