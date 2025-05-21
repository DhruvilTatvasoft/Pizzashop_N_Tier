using BAL.Implementations;
using BAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class BALServicesDependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {

        services.AddScoped<ILogin, LoginImpl>();
        services.AddScoped<IAESService, AESImple>();
        services.AddScoped<IJwtTokenGenService, JwtTokenImple>();
        services.AddScoped<IEmailGenService, EmailGenService>();
        services.AddScoped<IEmailGenService, EmailGenService>();
        services.AddScoped<IUser, UserImpl>();
        services.AddScoped<IAuthServices,AuthServiceImpl>();
        services.AddScoped<IMenuService, MenuImpl>();
        services.AddScoped<IItemService, ItemsImple>();
        services.AddScoped<IImagePath, imagePathImpl>();
        services.AddScoped<IPermissionService, PermissionImple>();
        services.AddScoped<IModifierService, ModifierImple>();
        services.AddScoped<ITableService, TableImpl>();
        services.AddScoped<ISectionService, SectionImpl>();
        services.AddScoped<ITaxService, TaxesImpl>();
        services.AddScoped<IOrderService, OrderImple>();
        services.AddScoped<ICustomerService, CustomerImpl>();
        services.AddScoped<IWaitingTokenService, WaitingTokenImpl>();
        services.AddScoped<IMenuOrderAppService, MenuOrderAppImple>();
        return services;
    }
}