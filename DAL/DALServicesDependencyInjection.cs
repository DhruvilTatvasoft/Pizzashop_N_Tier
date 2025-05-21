using DAL.Implementations;
using DAL.interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class DALServicesDependencyInjection
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IGenericRepository, GenericRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IRoleAndPermissionRepository, RoleAndPermissionRepository>();
        services.AddScoped<IModifierRepository, ModifierRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<ITaxesRepository, TaxesRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IWaitingTokenRepository, WaitingTokenRepository>();
        services.AddScoped<IMenuOrderAppRepository, MenuOrderAppRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        return services;
    }
}