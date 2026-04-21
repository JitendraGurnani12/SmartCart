public class Product
{
    public int Id{ get; set; }
    public string Name{ get; set; }
    public string Description{ get; set; }
    public decimal Price{ get; set; }
    
    // Foreign Key Property
    public int CategoryId{ get; set; }

    //Reference Navigation -Each Product belongs to one Category
    public Category Category{ get; set; }
}