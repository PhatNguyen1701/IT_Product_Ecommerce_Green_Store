using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey(p => p.ProviderId);

            builder.Property(p => p.ProviderId)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.CompanyName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.LogoURL)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.Representative)
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.PhoneNumber)
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.Address)
                .HasColumnType("nvarchar(50)");

            builder.Property(p => p.Description)
                .HasColumnType("nvarchar(max)");
        }
    }
}
