using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/Products")]
[ApiController]
public class ProductController(IProductRepository productRepository) : ControllerBase
{
    private readonly IProductRepository productRepository = productRepository;

    [HttpGet]
    public async Task<IActionResult> GetProducts(string? brand , string? type, string? sort)
    {
        return Ok(await productRepository.GetProductsAsync(brand , type, sort));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        productRepository.AddProduct(product);

        if (await productRepository.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct" , new { id = product.Id } , product);
        }

        return BadRequest("Problem Creating Product");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id , Product product)
    {
        if (product.Id != id || !ProductExist(id))
        {
            return BadRequest("Can't Update This Product");
        }

        productRepository.UpdateProduct(product);
        if (await productRepository.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the product");
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        return Ok(await productRepository.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        return Ok(await productRepository.GetTypesAsync());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);
        if (product is null)
            return NotFound();

        productRepository.DeleteProduct(product);
        if (await productRepository.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Deleting This Product");
    }

    private bool ProductExist(int id)
    {
        return productRepository.ProductExists(id);
    }
}
