public interface ICartService
{
    public Task<IEnumerable<Cart>> GetCarts();
    Task AddCart(Cart cart);
    public Task AddItemIntoCart(CartItemDto cartItemDto);
    public Task<Cart> GetCartById(int id);
     
   
    //  Task RemoveItemFromCart(CartItemDto cartItemDto);
    //  Task UpdateItemQuantityIntoCart(CartItemDto cartItemDto);
}