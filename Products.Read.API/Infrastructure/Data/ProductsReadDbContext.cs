using Microsoft.EntityFrameworkCore;
using Products.Read.API.Domain.Models;

namespace Products.Read.API.Infrastructure.Data
{
    public class ProductsReadDbContext : DbContext
    {
        public ProductsReadDbContext(DbContextOptions<ProductsReadDbContext> options) : base(options)
        { }
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ImageData> ImageData { get; set; } = null!;
        public DbSet<DocumentData> DocumentData { get; set; } = null!;
    }
}
