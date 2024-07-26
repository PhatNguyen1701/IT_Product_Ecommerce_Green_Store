using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITProductECommerce.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.AssignmentId);

            builder.Property(a => a.AssignmentId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(a => a.StaffId)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(a => a.DepartmentId)
                .IsRequired()
                .HasColumnType("varchar(7)");

            builder.HasOne(s => s.Staff)
                .WithMany(a => a.Assignments)
                .HasForeignKey(a => a.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Department)
                .WithMany(a => a.Assignments)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
