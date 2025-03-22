using System.Text;
using System.Text.Json.Serialization;
using BAL.Implementations;
using BAL.Interfaces;
using DAL.Data;
using DAL.Implementations;
using DAL.interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddDbContext<PizzashopCContext>(option =>
option.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionString")));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IGenericRepository, GenericRepository>();
builder.Services.AddScoped<ILogin, LoginImpl>();
builder.Services.AddScoped<IAESService, AESImple>();
builder.Services.AddScoped<IJwtTokenGenService, JwtTokenImple>();
builder.Services.AddScoped<IEmailGenService, EmailGenService>();
builder.Services.AddScoped<IUser, UserImpl>();
builder.Services.AddScoped<IMenuService, MenuImpl>();
builder.Services.AddScoped<IItemService, ItemsImple>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IImagePath, imagePathImpl>();
builder.Services.AddScoped<IPermissionService, PermissionImple>();
builder.Services.AddScoped<IRoleAndPermissionRepository, RoleAndPermissionRepository>();
builder.Services.AddScoped<IModifierService, ModifierImple>();
builder.Services.AddScoped<IModifierRepository, ModifierRepository>();
builder.Services.AddScoped<ITableService,TableImpl>();
builder.Services.AddScoped<ISectionService,SectionImpl>();
builder.Services.AddScoped<ISectionRepository,SectionRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(150);
    options.Cookie.Name = ".AdventureWorks.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var jwtSettings = builder.Configuration.GetSection("JWT");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("token"))
                {
                    context.Token = context.Request.Cookies["token"];
                    Console.WriteLine("Token extracted from cookie: " + context.Token);
                }
                else
                {
                    Console.WriteLine("No token found in cookie!");
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Authentication failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine("Authorization challenge: Access denied.");
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:SecretKey"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

    // builder.Services.ConfigureApplicationCookie(options =>
    // {
    // options.AccessDeniedPath = "/Login/Index";
    // options.Cookie.Name = "YourAppCookieName";
    // options.Cookie.HttpOnly = true;
    // // options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    // options.LoginPath = "/Login/Index";
    // options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    // options.SlidingExpiration = true;
    // });

builder.Services.AddAuthorization();
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
