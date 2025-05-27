using BAL.Implementations;
using BAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class BALServicesDependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {

        services.AddScoped<ILogin, LoginService>();
        services.AddScoped<IAESService, AESService>();
        services.AddScoped<IJwtTokenGenService, JwtTokenService>();
        services.AddScoped<IEmailGenService, EmailGenService>();
        services.AddScoped<IEmailGenService, EmailGenService>();
        services.AddScoped<IUser, UserService>();
        services.AddScoped<IAuthServices,AuthService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IItemService, ItemsService>();
        services.AddScoped<IImagePath, ImagePathService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IModifierService, ModifierService>();
        services.AddScoped<ITableService, TableService>();
        services.AddScoped<ISectionService, SectionService>();
        services.AddScoped<ITaxService, TaxesService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IWaitingTokenService, WaitingTokenService>();
        services.AddScoped<IMenuOrderAppService, MenuOrderAppService>();
        return services;
    }
}