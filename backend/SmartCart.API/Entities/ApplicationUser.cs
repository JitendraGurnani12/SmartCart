using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string Addresses { get; set; }
    public string DisplayName{ get; set; }
}