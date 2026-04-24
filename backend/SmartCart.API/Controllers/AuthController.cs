using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    //UserManager is built -in class of identity for manage the login and register//

    //Global Exception MiddleWare************************************
    //******Instead of using try-catch block in each and every method we should 
    // use a custom middle ware class for handling the exception,so code look clean and shorter, 
    // this is advance level development

    private readonly IConfiguration _config;
    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var _user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            Addresses = registerDto.Addresses
        };
        var result = await _userManager.CreateAsync(_user, registerDto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(_user, "User");  //entry goes into AspNetUserRoles which have userId and roleId
        return Ok("User Registered");
    }
    private async Task<string> GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email,user.Email),

            };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
            return Unauthorized("Invalid Email");

        var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!isValid)
            return Unauthorized("Invalid Password");

        var token = await GenerateToken(user);
        return Ok(new { Token = token });

    }


}
