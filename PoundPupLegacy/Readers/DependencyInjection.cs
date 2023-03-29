using PoundPupLegacy.Common;

namespace PoundPupLegacy.Readers;

public static class DependencyInjection
{
    public static void AddSystemReaders(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseReaderFactory<MenuItemsReader>, MenuItemsReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<PasswordValidationReader>, PasswordValidationReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<TenantNodesReader>, TenantNodesReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<TenantsReader>, TenantsReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<UserTenantActionReader>, UserTenantActionReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<UserTenantEditActionReader>, UserTenantEditActionReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<UserTenantEditOwnActionReader>, UserTenantEditOwnActionReaderFactory>();
    }
}
