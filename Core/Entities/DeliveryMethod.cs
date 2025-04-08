namespace Core.Entities;

public class DeliveryMethod: BaseEntity
{
    public required string ShortNamr { get; set; }
    public required string DeleviryTime { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
}
