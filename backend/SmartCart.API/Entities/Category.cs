using System.Net.Http.Headers;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; }


    //// Collection Navigation: One Category has Many Products
    public ICollection<Product> Products { get; set; }
}