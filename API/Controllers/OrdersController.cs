using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController(
    ICartService cartService,
    IUnitOfWork unitOfWork
) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderForCreationDto orderDto)
    {
        var email = User.GetEmail();

        var cart = await cartService.GetCartAsync(orderDto.CartId);

        if (cart == null) 
            return BadRequest("Cart Not Found");

        if(cart.PaymentIntentId == null)
            return BadRequest("No payment inent for this order");
            
        var items = new List<OrderItem>();
        foreach(var item in cart.Items)
        {
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);

            if(product == null)
                return BadRequest("Problem with order");

            var itemOrdered = new ProductItemOrderd
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl,
            };

            var orderItem = new OrderItem
            {
                ItemOrderd = itemOrdered,
                Price = product.Price,
                Quantity = item.Quantity
            };

            items.Add(orderItem);
        }

        var deliverMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);
        if(deliverMethod == null)
            return BadRequest("No delivery method selected");

        var order = new Order
        {
            OrderItems = items,
            DeliveryMethod = deliverMethod,
            ShippingAddress = orderDto.ShippingAddress,
            SubTotal = items.Sum(x => x.Price * x.Quantity),
            PaymentSummary = orderDto.PaymentSummary,
            PaymentIntentId = cart.PaymentIntentId,
            BuyerEmail = email
        };

        unitOfWork.Repository<Order>().Add(order);
        if(await unitOfWork.Complete())
            return Ok(order);

        return BadRequest("Problem Creating order.");
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersForUser()
    {
        var spec = new OrderSpecification(User.GetEmail());

        var orders = await unitOfWork.Repository<Order>()
            .ListAsync(spec);

        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var spec = new OrderSpecification(User.GetEmail(), id);

        var order = await unitOfWork.Repository<Order>()
            .GetEntityWithSpec(spec);

        if(order is null)
            return NotFound();

        return Ok(order);
    }
}
