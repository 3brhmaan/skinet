using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> genericRepository) : BaseApiController
{
    private readonly IGenericRepository<Product> genericRepository = genericRepository;

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery]ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);

        return await CreatePageResult(
            genericRepository, spec, specParams.PageIndex, specParams.PageSize 
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await genericRepository.GetByIdAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        genericRepository.Add(product);

        if (await genericRepository.SaveAllAsync())
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

        genericRepository.Update(product);
        if (await genericRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the product");
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await genericRepository.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await genericRepository.ListAsync(spec));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await genericRepository.GetByIdAsync(id);
        if (product is null)
            return NotFound();

        genericRepository.Remove(product);
        if (await genericRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem Deleting This Product");
    }

    private bool ProductExist(int id)
    {
        return genericRepository.Exists(id);
    }
}
