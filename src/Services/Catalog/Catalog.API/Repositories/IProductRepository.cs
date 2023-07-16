using Catalog.API.Entities;

namespace Catalog.API.Repositories;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(string id);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);

    Task CreateProductAsync(Product product);
    Task<bool> DeleteProductAsync(string id);
    Task<bool> UpdateProductAsync(Product product);
}
