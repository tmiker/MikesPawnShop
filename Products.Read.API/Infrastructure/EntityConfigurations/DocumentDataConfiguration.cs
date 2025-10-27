using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Read.API.Domain.Models;

namespace Products.Read.API.Infrastructure.EntityConfigurations
{
    public class DocumentDataConfiguration : IEntityTypeConfiguration<DocumentData>
    {
        public void Configure(EntityTypeBuilder<DocumentData> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).HasMaxLength(200);
            builder.Property(d => d.Title).HasMaxLength(200);
            builder.Property(d => d.SequenceNumber).IsRequired();
            builder.Property(d => d.DocumentUrl).HasMaxLength(500);
            builder.HasOne(d => d.Product)
                   .WithMany(p => p.Documents)
                   .HasForeignKey(d => d.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
