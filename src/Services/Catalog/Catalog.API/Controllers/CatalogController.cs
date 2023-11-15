using System.Net;
using Catalog.API.Entities;
using Catalog.API.Models;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
  private readonly IProductRepository _repository;

  public CatalogController(IProductRepository repository)
  {
    _repository = repository;
  }

  [HttpGet]
  [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> GetProducts()
  {
    var products = await _repository.GetProducts();

    return Ok(products);
  }

  [HttpGet("{id:length(24)}", Name = "GetProduct")]
  [ProducesResponseType((int)HttpStatusCode.NotFound)]
  [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> GetProductById(string id)
  {
    var product = await _repository.GetProduct(id);

    if (product is null)
    {
      return NotFound();
    }

    return Ok(product);
  }

  [HttpGet("[action]/{category}")]
  [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> GetProductByCategory(string category)
  {
    var products = await _repository.GetProductByCategory(category);

    return Ok(products);
  }

  [HttpPost]
  [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> CreateProduct([FromBody] ProductDTO product)
  {
    var response = await _repository.CreateProduct(product);

    return CreatedAtRoute("GetProduct", new { id = response }, product);
  }

  [HttpPut]
  [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> UpdateProduct([FromBody] Product product)
  {
    return Ok(await _repository.UpdateProduct(product));
  }

  [HttpDelete("{id:length(24)}")]
  [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
  public async Task<IActionResult> DeleteProductsById(string id)
  {
    return Ok(await _repository.DeleteProduct(id));
  }
}