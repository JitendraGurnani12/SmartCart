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
        // try
        // {
            
            var _user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                Addresses = registerDto.Addresses
            };
            var result = await _userManager.CreateAsync(_user, registerDto.Password);

        if (!result.Succeeded)
        {
            // Sabse simple tarika: Saare errors ko jod kar 1 plain text string bana lo
            var errorString = string.Join(", ", result.Errors.Select(e => e.Description));

            // Direct string return karo, isse frontend ko koi complex object nahi milega
            return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = errorString,
                    Data = null
                });
        }

        await _userManager.AddToRoleAsync(_user, "Customer");  //entry goes into AspNetUserRoles which have userId and roleId
            return Ok(registerDto);
        // }
        // catch (Exception ex)
        // {
        //     return BadRequest(ex);
        // }
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
        // try
        // {
            
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Your entered email is incorrect",
                    Data = null
                });

            var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isValid)
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Your entered password is incorrect",
                    Data = null
                });

            var token = await GenerateToken(user);
            return Ok(new { Token = token , user.Email, user.DisplayName});
        // }
        // catch (Exception ex)
        // {
        //     return BadRequest(ex);
        // }

    }


}
