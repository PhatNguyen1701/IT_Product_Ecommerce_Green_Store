using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class MainCommentConfiguration : IEntityTypeConfiguration<MainComment>
    {
        public void Configure(EntityTypeBuilder<MainComment> builder)
        {
            builder.HasKey(c => c.MainCommentId);

            builder.Property(c => c.MainCommentId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.Message)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.HasOne(c => c.Customer)
                .WithMany(mc => mc.MainComments)
                .HasForeignKey(mc => mc.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Product)
                .WithMany(mc => mc.MainComments)
                .HasForeignKey(mc => mc.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
