using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Read.API.Domain.Models;

namespace Products.Read.API.Infrastructure.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Category).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(1000);
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.Currency).IsRequired().HasMaxLength(10);
            builder.Property(p => p.Status).IsRequired().HasMaxLength(50);
            builder.Property(p => p.DateCreated).IsRequired();
            builder.Property(p => p.DateUpdated).IsRequired();
            builder.HasMany(p => p.Images)
                   .WithOne(i => i.Product)
                   .HasForeignKey(i => i.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.Documents)
                   .WithOne(d => d.Product)
                   .HasForeignKey(d => d.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
