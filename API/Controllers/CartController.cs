using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet("{id}" , Name = "GetCardById")]
    public async Task<IActionResult> GetCardById(string id)
    {
        var cart = await cartService.GetCartAsync(id);
        return Ok(cart ?? new ShoppingCart { Id = id });
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart(ShoppingCart cart)
    {
        var updatedCart = await cartService.SetCartAsync(cart);

        if (updatedCart is null)
            return BadRequest("Problem With Cart");

        return CreatedAtRoute("GetCardById" , new { id = cart.Id } , cart);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCart(string id)
    {
        var result = await cartService.DeleteCartAsync(id);

        if(!result)
            return BadRequest("Problem Deleting Cart");

        return NoContent();
    }

}
