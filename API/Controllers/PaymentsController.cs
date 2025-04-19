using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/Payments")]
[ApiController]
public class PaymentsController(
    IPaymentService paymentService ,
    IUnitOfWork unitOfWork
)
    : ControllerBase
{
    [HttpPost("{cartId}")]
    [Authorize]
    public async Task<IActionResult> CreateOrUpdatePaymentIntent(string cartId)
    {
        var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);

        if (cart is null)
        {
            return BadRequest("Problem with your cart");
        }

        return Ok(cart);
    }

    [HttpGet("delivery-methods")]
    public async Task<IActionResult> GetDeliveryMethod()
    {
        return Ok(
            await unitOfWork.Repository<DeliveryMethod>().GetAllAsync()
        );
    }

}
