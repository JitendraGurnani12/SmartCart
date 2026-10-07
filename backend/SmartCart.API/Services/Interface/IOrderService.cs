public interface IOrderService
{
    Task<Order> AddOrder(OrderDto orderDto);
    Task<List<OrderResponseDto>> GetOrderByUserId(string userId);
    Task<OrderResponseDto> GetOrderDetailById(int orderId,string userId);
    // Task<OrderResponseDto> GetSellerOrderDetailById(int orderId, string sellerId);
}