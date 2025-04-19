using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery]ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);

        return await CreatePageResult(
            unitOfWork.Repository<Product>(), spec, specParams.PageIndex, specParams.PageSize 
        );
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    //[Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        unitOfWork.Repository<Product>().Add(product);

        if (await unitOfWork.Complete())
        {
            return CreatedAtAction("GetProduct" , new { id = product.Id } , product);
        }

        return BadRequest("Problem Creating Product");
    }

    //[Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id , Product product)
    {
        if (product.Id != id || !ProductExist(id))
        {
            return BadRequest("Can't Update This Product");
        }

        unitOfWork.Repository<Product>().Update(product);
        if (await unitOfWork.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the product");
    }

    [HttpGet("brands")]
    public async Task<IActionResult> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await unitOfWork.Repository<Product>().ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await unitOfWork.Repository<Product>().ListAsync(spec));
    }

    //[Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product is null)
            return NotFound();

        unitOfWork.Repository<Product>().Remove(product);
        if (await unitOfWork.Complete())
        {
            return NoContent();
        }

        return BadRequest("Problem Deleting This Product");
    }

    private bool ProductExist(int id)
    {
        return unitOfWork.Repository<Product>().Exists(id);
    }
}
