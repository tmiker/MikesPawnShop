using Products.Write.Domain.Aggregates;

namespace Products.Write.Infrastructure.Abstractions
{
    public interface IProductRepository
    {
        Task<bool> SaveAsync(Product product);
        Task<Product> GetProductByIdAsync(Guid aggregateId);
        Task<Product> GetProductByIdAndVersionAsync(Guid aggregateId, int minVersion, int maxVersion);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<bool> PurgeAsync();
    }
}
