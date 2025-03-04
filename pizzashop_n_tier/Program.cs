using BAL.Implementations;
using BAL.Interfaces;
using DAL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddDbContext<PizzashopCContext>(option=>
option.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionString")));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICookieService,CookieService>();
builder.Services.AddScoped<IGenericRepository,GenericRepository>();
builder.Services.AddScoped<ILogin,LoginImpl>();
builder.Services.AddScoped<IAESService,AESImple>();
builder.Services.AddScoped<IEmailGenService,EmailGenService>();
builder.Services.AddScoped<IUser,UserImpl>();
builder.Services.AddScoped<IMenuService,MenuImpl>();
builder.Services.AddScoped<IItemService,ItemsImple>();
builder.Services.AddScoped<IItemRepository,ItemRepository>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(150);
    options.Cookie.Name = ".AdventureWorks.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
