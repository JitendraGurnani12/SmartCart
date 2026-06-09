public class CartResponseDto
{
    public int Id { get; set; }

    public List<CartItemResponseDto> CartItems { get; set; } = new();
}