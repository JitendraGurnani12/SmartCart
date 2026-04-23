public class BaseEntity
{
    //Basic Common IsActive,IsDelete properties//
    public bool IsDeleted{ get; set; } = false;
    // public bool IsActive{ get; set; }
    public string CreatedBy{get;set;} // userId
    public string ModifiedBy{get;set;}
    public DateTime CreatedOn{get;set;} = DateTime.UtcNow;
    public DateTime? ModifiedOn{get;set;}
    //Basic Common IsActive,IsDelete properties//
}