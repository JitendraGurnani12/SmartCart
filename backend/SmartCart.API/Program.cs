using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().
                WriteTo.Console().WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Hour)
                .CreateLogger();

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog();
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });;

builder.Services.AddOpenApi();

//Register the Dbcontext start //
builder.Services.AddDbContext<AppDbContext>(sqlServer=>
    sqlServer.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register the Dbcontext end //

//Register the Identity start //
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders(); 
//Register the Identity  end // 



    builder.Services.AddScoped<IProductService ,ProductService>();
    builder.Services.AddScoped<IProductRepository,ProductRepository>();
    builder.Services.AddScoped<ICategoryService,CategoryService>();
    builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();

     builder.Services.AddScoped<ICartService ,CartService>();
    builder.Services.AddScoped<ICartRepository,CartRepository>();

    builder.Services.AddScoped<IOrderService ,OrderService>();
    builder.Services.AddScoped<IOrderRepository,OrderRepository>();

    //Class register for Memory Cache//
    builder.Services.AddMemoryCache();
    //Class register for Memory Cache//

//Configure the Jwt Barrer token functionality start//
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
//Configure the Jwt Barrer token functionality end//    

var app = builder.Build();

//Add role in IdentityRole Table ,only once ////Table Name is AspNetRoles this is master table for roles...... Start//

    var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));
    if (!await roleManager.RoleExistsAsync("Manager"))
        await roleManager.CreateAsync(new IdentityRole("Manager"));    

//Add role in IdentityRole Table ,only once/////....................End //    

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();
