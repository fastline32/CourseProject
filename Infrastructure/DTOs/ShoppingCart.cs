namespace Infrastructure.DTOs;

public class ShoppingCart
{
    public int ProductId { get; init; }
    
    public int TempQuantity { get; set; }
}