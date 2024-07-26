using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class WebConfiguration : IEntityTypeConfiguration<Web>
    {
        public void Configure(EntityTypeBuilder<Web> builder)
        {
            builder.HasKey(w => w.WebId);

            builder.Property(w => w.WebId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(w => w.WebName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(w => w.URL)
                .IsRequired()
                .HasColumnType("nvarchar(50)");
        }
    }
}
