/// <summary>
/// Class for cart details like it,s Id and UserId
/// </summary>
public class Cart
{
    public int Id{get;set;}
    public string UserId{get;set;}
    public ICollection<CartItem> CartItems{get;set;}
}