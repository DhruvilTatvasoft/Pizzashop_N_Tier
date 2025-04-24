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
builder.Services.AddScoped<ITableService, TableImpl>();
builder.Services.AddScoped<ISectionService, SectionImpl>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITaxesRepository, TaxesRepository>();
builder.Services.AddScoped<ITaxService, TaxesImpl>();
builder.Services.AddScoped<IOrderService, OrderImple>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerImpl>();
builder.Services.AddScoped<IWaitingTokenService, WaitingTokenImpl>();
builder.Services.AddScoped<IWaitingTokenRepository, WaitingTokenRepository>();
builder.Services.AddScoped<IMenuOrderAppService, MenuOrderAppImple>();
builder.Services.AddScoped<IMenuOrderAppRepository, MenuOrderAppRepository>();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(150);
    options.Cookie.Name = ".AdventureWorks.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var jwtSettings = builder.Configuration.GetSection("JWT");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });


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
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.Redirect("/Login/Index?error=expired");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.Redirect("/Login/Index?error=unauthorized");
                    context.HandleResponse();
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
            builder.Configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured."))),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    var entities = new[] { "Users", "RolesAndPermissions", "Menu", "TableAndSection", "TaxAndFee", "Order", "Customers" };

    foreach (var entity in entities)
    {
        options.AddPolicy($"CanView_{entity}", policy => policy.RequireClaim($"CanView_{entity}", "True"));
        options.AddPolicy($"CanEdit_{entity}", policy => policy.RequireClaim($"CanEdit_{entity}", "True"));
        options.AddPolicy($"CanDelete_{entity}", policy => policy.RequireClaim($"CanDelete_{entity}", "True"));
    }
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Index";
    options.AccessDeniedPath = "/Login/AccessDenied";
});



builder.Services.AddAuthorization();
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddRazorPages();
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStatusCodePagesWithReExecute("/Error/NotFound");
app.UseDeveloperExceptionPage();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.Run();




// dotnet ef dbcontext scaffold "Server=localhost,5432;Database=Pizzashop_c;User id=postgres;password=Tatva@123;TrustServerCertificate=True" Npgsql.EntityFrameworkCore.PostgreSQL -o Data --context PizzashopCContext --context-dir Data -f