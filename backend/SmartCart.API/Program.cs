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
builder.Services.AddSwaggerGen();

//Register the Dbcontext start //
builder.Services.AddDbContext<AppDbContext>(sqlServer=>
    sqlServer.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(); // this line added because if sql connection take time while docker command 
                                            // //then it retry for connection//
                                            //If SQL container still starting:
                                                // retry connection automatically
                                                // Very common problem in Docker/Kubernetes/cloud.
    }));

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

    builder.Services.AddScoped<IFileService, FileService>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAnyOrigins", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    });

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


    
app.UseCors("AllowAnyOrigins");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();
// app.UseHttpsRedirection(); // remove for just development phase//
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();
} // this lines added because at application startup first using docker, migration not run automatically,Db not create,
  // so we need to specify mannualy if DB is missing then run migrations automatically while docker command run//

//Add role in IdentityRole Table ,only once ////Table Name is AspNetRoles this is master table for roles...... Start//

// var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
// if (!await roleManager.RoleExistsAsync("Admin"))
//     await roleManager.CreateAsync(new IdentityRole("Admin"));
// if (!await roleManager.RoleExistsAsync("User"))
//     await roleManager.CreateAsync(new IdentityRole("User"));
// if (!await roleManager.RoleExistsAsync("Manager"))
//     await roleManager.CreateAsync(new IdentityRole("Manager"));// line commented becaues we face below issue//

//Add role in IdentityRole Table ,only once/////....................End //

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Seller", "Customer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
// this line is written because we are facing issue while docker compose command, 
// first db should be create, then table ,
// //then insert role into table sequence matter if role insert before table create so error will show and container 
// not created 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var adminEmail = "admin@smartcart.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "Admin@123");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
    var sellerEmail = "seller2@smartcart.com";
    var sellerUser = await userManager.FindByNameAsync(sellerEmail);
    if(sellerUser == null)
    {
        sellerUser = new ApplicationUser
        {
            UserName = sellerEmail,
            Email = sellerEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(sellerUser,"Admin@123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(sellerUser,"Seller");
        }
    }
}
app.Run();
