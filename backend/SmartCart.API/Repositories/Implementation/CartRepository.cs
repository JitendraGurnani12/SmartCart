using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddCart(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<Cart>> GetCarts()
    {
        return await  _context.Carts.ToListAsync();
    }
    public async Task DeleteCart(Cart cart)
    {
           _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
    }

    public async Task<Cart> GetCartById(int id)
    {
        return await  _context.Carts.Include(c=>c.CartItems).FirstOrDefaultAsync(c=>c.Id == id);
    }
    public async Task AddItemIntoCart(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
    }
    public  async Task RemoveItemFromCart(CartItem cartItem)
    {
         _context.CartItems.Remove(cartItem);
         await _context.SaveChangesAsync();
    }
    public async Task UpdateItemQuantityIntoCart(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync();
    }
    public async Task<Cart> GetCartByUserId(string userId)
    {
        return await  _context.Carts.Include(c=>c.CartItems).ThenInclude(x=>x.Product).FirstOrDefaultAsync(c => c.UserId == userId);
    }
    public async Task<Cart> IsProductExistInCart(int cartId, int productId)
    {
        return await  _context.Carts.FirstOrDefaultAsync(x=>x.Id == cartId && x.CartItems.Any(c=>c.ProductId == productId));
    }
    public async Task<CartItem> GetCartItemsByCartId(int cartId,int productId)
    {
        return await  _context.CartItems.FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId);
    }

}