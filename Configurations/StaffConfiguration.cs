using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasKey(s => s.StaffId);

            builder.Property(s => s.StaffId)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(s => s.StaffName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(s => s.Email)
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

            builder.Property(s => s.Password)
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
                .WithMany(s => s.Staffs)
                .HasForeignKey(s => s.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
