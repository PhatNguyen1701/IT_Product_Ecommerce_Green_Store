using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.CustomerId)
                .IsRequired()
                .HasColumnType("nvarchar(20)");

            builder.Property(c => c.Password)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.CustomerName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.Gender)
                .IsRequired();

            builder.Property(c => c.DoB)
                .IsRequired();

            builder.Property(c => c.Address)
                .HasColumnType("nvarchar(60)");

            builder.Property(c => c.PhoneNumber)
                .HasColumnType("nvarchar(24)");

            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.AvatarURL)
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.IsActive)
                .IsRequired();

            builder.Property(c => c.RoleId)
                .IsRequired();

            builder.Property(c => c.RandomKey)
                .HasColumnType("varchar(50)");

            builder.HasOne(r => r.Role)
                .WithMany(c => c.Customers)
                .HasForeignKey(c => c.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.DiscountProgram)
                .WithMany(c => c.Customers)
                .HasForeignKey(c => c.DiscountId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
