using Microsoft.EntityFrameworkCore;

public class OrderService :IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly AppDbContext _context;
    public OrderService(IOrderRepository orderRepository,ICartRepository cartRepository, AppDbContext context)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _context = context;
    }
    //Order Api ---> all things should belong to transacation 
    // transaction means all db operations should be execute successsfully ,
    // //if even one db operation fail then all things should be roll back
    // BIG DESIGN RULE
    //Transactions should be handled in Service Layer, not Repository
    public async Task<Order> AddOrder(OrderDto orderDto)
    {
        //step 1 Check cart Items
        var cart = await _context.Carts.Include(c => c.CartItems)
                                        .FirstOrDefaultAsync(c => c.Id == orderDto.CartId);
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            //step2 Cart is empty or not
            if (cart == null || !cart.CartItems.Any())
            {
                throw new Exception("Cart is empty");
            }
            //step 3 copy cart item into order item

            var orderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity
            }).ToList();
            //step 4 --create Orderrrr
            var order = new Order
            {
                UserId = orderDto.UserId,
                CreatedAt = DateTime.UtcNow,
                OrderItems = orderItems
            };

            await _context.Orders.AddAsync(order);

            //Clear the cartitems
            _context.CartItems.RemoveRange(cart.CartItems);

            //do all changes into db
            await _context.SaveChangesAsync();
            //Transcation is committed
            await transaction.CommitAsync();

            return order;
        }
        catch( Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}