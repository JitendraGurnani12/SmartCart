public interface ICartRepository
{
    public Task<IEnumerable<Cart>> GetCarts();
    Task AddCart(Cart cart);
    public Task<Cart> GetCartById(int id);
    public Task AddItemIntoCart(CartItem cartItem);
     Task RemoveItemFromCart(CartItem cartItem);
     Task UpdateItemQuantityIntoCart(CartItem cartItem);

     Task<Cart> IsProductExistInCart(int cartId, int productId);
     Task<Cart> GetCartByUserId(string userId);
     Task<CartItem> GetCartItemsByCartId(int cartId,int productId);
}