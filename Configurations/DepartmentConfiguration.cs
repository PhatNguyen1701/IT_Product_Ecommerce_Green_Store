using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentId);

            builder.Property(d => d.DepartmentId)
                .IsRequired()
                .HasColumnType("varchar(7)");

            builder.Property(d => d.DepartmentName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(d => d.Description)
                .HasColumnType("nvarchar(max)");
        }
    }
}
