using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.HasKey(s => s.StatusId);

            builder.Property(s => s.StatusId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(s => s.StatusName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(s => s.Description)
                .HasColumnType("nvarchar(max)");
        }
    }
}
