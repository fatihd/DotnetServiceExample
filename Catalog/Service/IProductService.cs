using Catalog.Models;

namespace Catalog.Service
{
    public interface IProductService
    {
        Task DeleteProduct(Guid id);
        Task<Product?> GetProduct(Guid id);
        Task<IEnumerable<Product>> GetProducts();
        Task<Guid> PostProduct(CreateProduct createProduct);
        Task PutProduct(Guid id, UpdateProduct updateProduct);
    }
}