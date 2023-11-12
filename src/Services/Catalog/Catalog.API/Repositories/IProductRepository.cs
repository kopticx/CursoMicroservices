using Catalog.API.Entities;
using Catalog.API.Models;

namespace Catalog.API.Repositories;

public interface IProductRepository
{
  Task<IEnumerable<Product>> GetProducts();
  Task<Product> GetProduct(string id);
  Task<IEnumerable<Product>> GetProductByName(string name);
  Task<IEnumerable<Product>> GetProductByCategory(string categoryName);

  Task<string> CreateProduct(ProductDTO model);
  Task<bool> UpdateProduct(Product product);
  Task<bool> DeleteProduct(string id);
}