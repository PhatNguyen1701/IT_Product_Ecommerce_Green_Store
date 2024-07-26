using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.RoleId);

            builder.Property(r => r.RoleId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(r => r.DepartmentId)
                .IsRequired()
                .HasColumnType("varchar(7)");

            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasColumnType("varchar(25)");

            builder.Property(r => r.IsAdd)
                .IsRequired();

            builder.Property(r => r.IsEdit)
                .IsRequired();

            builder.Property(r => r.IsDelete)
                .IsRequired();

            builder.Property(r => r.IsViewed)
                .IsRequired();

            builder.HasOne(d => d.Department)
                .WithMany(r => r.Roles)
                .HasForeignKey(r => r.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(w => w.Web)
                .WithMany(r => r.Roles)
                .HasForeignKey(r => r.WebId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
