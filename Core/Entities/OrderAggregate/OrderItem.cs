﻿namespace Core.Entities.OrderAggregate;
public class OrderItem: BaseEntity
{
    public ProductItemOrderd ItemOrderd { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
