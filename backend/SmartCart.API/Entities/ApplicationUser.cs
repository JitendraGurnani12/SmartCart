using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string Addresses { get; set; }
    public string DisplayName{ get; set; }

    //Basic Common IsActive,IsDelete properties//
    public bool IsDeleted{ get; set; }
    public bool IsActive{ get; set; }
    public string CreatedBy{get; set; } // userId
    public string ModifiedBy{get; set; }
    public DateTime CreatedOn{get; set; }
    public DateTime? ModifiedOn{get; set; }
    //Basic Common IsActive,IsDelete properties//
}