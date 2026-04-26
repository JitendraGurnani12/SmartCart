public class PageResponse<T>
{
    public int PageSize{get;set;}
    public int Page{get;set;}
    public int TotalRecords{get;set;}
    public IEnumerable<T> Data { get; set; }
}