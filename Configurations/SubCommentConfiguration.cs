using ITProductECommerce.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ITProductECommerce.Configurations
{
    public class SubCommentConfiguration : IEntityTypeConfiguration<SubComment>
    {
        public void Configure(EntityTypeBuilder<SubComment> builder)
        {
            builder.HasKey(c => c.SubCommentId);

            builder.Property(c => c.SubCommentId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(c => c.Message)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.HasOne(sc => sc.MainComment)
                .WithMany(mc => mc.SubComments)
                .HasForeignKey(sc => sc.MainCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sc => sc.User)
                .WithMany(u => u.SubComments)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
