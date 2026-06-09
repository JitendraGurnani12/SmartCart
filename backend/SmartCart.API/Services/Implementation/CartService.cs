using Microsoft.Extensions.Configuration.UserSecrets;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }
    public Task<IEnumerable<Cart>> GetCarts()
    {
        try
        {
            return _cartRepository.GetCarts();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public Task<Cart> GetCartById(int id)
    {
        try
        {
            return _cartRepository.GetCartById(id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task<CartResponseDto> GetCartByUserId(string userId)
    {
        var cart = await _cartRepository.GetCartByUserId(userId);

        if (cart == null)
        {
            return null;
        }

        return new CartResponseDto
        {
            Id = cart.Id,

            CartItems = cart.CartItems
                .Select(ci => new CartItemResponseDto
                {
                    ProductId = ci.ProductId,

                    ProductName = ci.Product.Name,

                    Price = ci.Product.Price,

                    ImageUrl = ci.Product.ImageUrl,

                    Quantity = ci.Quantity,
                    CartId = cart.Id

                }).ToList()
        };
    }
    public async Task AddItemIntoCart(CartItemDto cartItemDto)
    {
        try
        {

            var product = await _productRepository.GetByIdAsync(cartItemDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
                //Product is entitykey
            }
            var cart = await _cartRepository.GetCartByUserId(cartItemDto.UserId);


            if (cart == null)
            {
                cart = new Cart()
                {
                    UserId = cartItemDto.UserId,
                };
                await _cartRepository.AddCart(cart);

            }
            var existCartItem = await _cartRepository.GetCartItemsByCartId(cart.Id, cartItemDto.ProductId);
            CartItem cartItem = new CartItem();
            if (existCartItem == null)
            {
                cartItem = new CartItem()
                {
                    Quantity = 1,
                    CartId = cart.Id,
                    ProductId = cartItemDto.ProductId

                };
                await _cartRepository.AddItemIntoCart(cartItem);
            }
            else
            {
                existCartItem.Quantity = existCartItem.Quantity + 1;
                await _cartRepository.UpdateItemQuantityIntoCart(existCartItem);
            }
        }
        catch (Exception ex)
        {
            throw;
        }



        
    }
    public async Task AddCart(Cart cart)
    {
       
        await _cartRepository.AddCart(cart);
        
    }
    public async Task RemoveItemFromCart(CartItemDto cartItemDto)
    {
        try
        {
            var existCartItem = await _cartRepository.GetCartItemsByCartId(cartItemDto.CartId, cartItemDto.ProductId);
            if (existCartItem == null)
            {
                throw new KeyNotFoundException("CartItem not found");
            }
            else
            {
                await _cartRepository.RemoveItemFromCart(existCartItem);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task  UpdateItemQuantityIntoCart(UpdateCartItemDto dto)
    {
        try
        {
            var existCartItem = await _cartRepository.GetCartItemsByCartId(dto.CartId, dto.ProductId);
            if (existCartItem == null)
            {
                throw new KeyNotFoundException("CartItem not found");
            }
            else
            {
                existCartItem.Quantity = dto.Quantity;
                await _cartRepository.UpdateItemQuantityIntoCart(existCartItem);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

}