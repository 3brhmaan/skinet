using API.Extensions;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(
    IUnitOfWork unitOfWork ,
    IPaymentService paymentService
)
    : BaseApiController
{
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders([FromQuery] OrderSpecParams specParams)
    {
        var spec = new OrderSpecification(specParams);

        return await CreatePageResult(
            unitOfWork.Repository<Order>() ,
            spec ,
            x => x.ToDto() ,
            specParams.PageIndex ,
            specParams.PageSize
        );
    }

    [HttpGet("orders/{id:int}")]
    public async Task<IActionResult> GerOrderById(int id)
    {
        var spec = new OrderSpecification(id);

        var order = await unitOfWork.Repository<Order>()
            .GetEntityWithSpec(spec);

        if (order == null)
            return BadRequest("No order with is.");

        return Ok(order.ToDto());
    }

    [HttpPost("orders/refund/{id:int}")]
    public async Task<IActionResult> RefundOrder(int id)
    {
        var spec = new OrderSpecification(id);

        var order = await unitOfWork.Repository<Order>()
            .GetEntityWithSpec(spec);

        if (order == null)
            return BadRequest("No order with that id.");

        if (order.Status == OrderStatus.Pending)
            return BadRequest("Payment not recived for this order");

        var result = await paymentService.RefundPayment(order.PaymentIntentId);

        if (result == "succeeded")
        {
            order.Status = OrderStatus.Refunded;

            await unitOfWork.Complete();

            return Ok(order.ToDto());
        }

        return BadRequest("Problem Refunding Order");
    }
}
