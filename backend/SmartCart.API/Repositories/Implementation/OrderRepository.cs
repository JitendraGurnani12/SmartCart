using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddOrder(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<Order> GetOrderById(int id)
    {
        return await  _context.Orders.Include(c=>c.OrderItems).FirstOrDefaultAsync(c=>c.Id == id);
    }
}