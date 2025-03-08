using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Route("api/Products")]
[ApiController]
public class ProductController(StoredContext context) : ControllerBase
{
    private readonly StoredContext context = context;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        return Ok(await context.Products
            .ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return Created();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id , Product product)
    {
        if (product.Id != id || !ProductExist(id))
        {
            return BadRequest("Can't Update This Product");
        }

        context.Entry(product).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if(product is null)
            return NotFound();

        context.Products.Remove(product);
        await context.SaveChangesAsync();

        return NoContent();
    }
    
    private bool ProductExist(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }
}
