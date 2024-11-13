using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(c => c.Gender)
                .IsRequired();

            builder.Property(c => c.DoB)
                .IsRequired();

            builder.Property(c => c.Address)
                .HasColumnType("nvarchar(60)");

            builder.Property(c => c.AvatarURL)
                .HasColumnType("nvarchar(50)");
        }
    }
}
