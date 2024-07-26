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

            builder.Property(s => s.Password)
                .IsRequired()
                .HasColumnType("nvarchar(50)");
        }
    }
}
