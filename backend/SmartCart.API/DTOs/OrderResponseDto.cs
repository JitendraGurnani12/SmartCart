public class OrderResponseDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<OrderItemResponseDto> OrderItems { get; set; }
}