using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HireMind.Infrastructure.Configurations
{
    public class LookupConfiguration : IEntityTypeConfiguration<Lookup>
    {
        public void Configure(EntityTypeBuilder<Lookup> builder)
        {
            // Self-referencing relationship
            builder.HasOne(x => x.Parent)
                   .WithMany(x => x.Children)
                   .HasForeignKey(x => x.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Make CategoryName required
            builder.Property(x => x.CategoryName)
                   .IsRequired()
                   .HasMaxLength(200);
        }
    }
}