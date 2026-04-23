/// <summary>
/// Class for cart details like it,s Id and UserId
/// </summary>
public class Cart
{
    public int Id{get;set;}
    public string UserId{get;set;}
    //Basic Common IsActive,IsDelete properties//
    // public bool IsDeleted{ get; set; }
    // public bool IsActive{ get; set; }
    
    //Basic Common IsActive,IsDelete properties//
    public ICollection<CartItem> CartItems{get;set;}
}