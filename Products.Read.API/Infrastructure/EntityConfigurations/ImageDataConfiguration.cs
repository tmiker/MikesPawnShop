using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Read.API.Domain.Models;

namespace Products.Read.API.Infrastructure.EntityConfigurations
{
    public class ImageDataConfiguration : IEntityTypeConfiguration<ImageData>
    {
        public void Configure(EntityTypeBuilder<ImageData> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Name).HasMaxLength(200);
            builder.Property(i => i.Caption).HasMaxLength(200);
            builder.Property(i => i.SequenceNumber).IsRequired();
            builder.Property(i => i.ImageUrl).HasMaxLength(500);
            builder.Property(i => i.ThumbnailUrl).HasMaxLength(500);
            builder.HasOne(i => i.Product)
                   .WithMany(p => p.Images)
                   .HasForeignKey(i => i.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    {
    }
}
