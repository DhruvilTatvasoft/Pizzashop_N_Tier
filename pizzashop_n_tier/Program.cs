using System.Text;
using System.Text.Json.Serialization;
using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddBusinessServices();
builder.Services.AddRepositoryServices();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddDbContext<PizzashopCContext>(option =>
option.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionString")));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, PermissionHandler>();
builder.Services.AddAuthorization(options =>
{
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var rolesAndPermissionServices = scope.ServiceProvider.GetRequiredService<IPermissionService>();
    var policies = rolesAndPermissionServices.GetAllPolicies();
    foreach (var policy in policies)
    {
        options.AddPolicy(policy, policyBuilder =>
            policyBuilder.Requirements.Add(new PermissionRequirement(policy)));
    }
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
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
app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStatusCodePagesWithReExecute("/Error/NotFound");
app.MapHub<KotHub>("/chathub");
app.UseDeveloperExceptionPage();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.Run();

// dotnet ef dbcontext scaffold "Server=localhost,5432;Database=pizzashop_new;User id=postgres;password=Tatva@123;TrustServerCertificate=True" Npgsql.EntityFrameworkCore.PostgreSQL -o Data --context PizzashopCContext --context-dir Data -f