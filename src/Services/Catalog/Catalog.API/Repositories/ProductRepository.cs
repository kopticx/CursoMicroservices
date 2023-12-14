using AutoMapper;
using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Models;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository(ICatalogContext context, IMapper mapper) : IProductRepository
{
  public async Task<IEnumerable<Product>> GetProducts()
  {
    return await context.Products.FindAsync(p => true).Result.ToListAsync();
  }

  public async Task<Product> GetProduct(string id)
  {
    return await context.Products.FindAsync(p => p.Id == id).Result.FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Product>> GetProductByName(string name)
  {
    var filter = Builders<Product>.Filter.Eq(p => p.Name, name);

    return await context.Products.FindAsync(filter).Result.ToListAsync();
  }

  public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
  {
    var filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

    return await context.Products.FindAsync(filter).Result.ToListAsync();
  }

  public async Task<string> CreateProduct(ProductDTO model)
  {
    var product = mapper.Map<Product>(model);

    await context.Products.InsertOneAsync(product);

    return product.Id;
  }

  public async Task<bool> UpdateProduct(Product product)
  {
    var updateResult = await context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

    return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
  }

  public async Task<bool> DeleteProduct(string id)
  {
    var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

    var deleteResult = await context.Products.DeleteOneAsync(filter);

    return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
  }
}