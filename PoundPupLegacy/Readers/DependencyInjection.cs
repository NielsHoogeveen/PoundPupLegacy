using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

public static class DependencyInjection
{
    public static void AddSystemReaders(this IServiceCollection services)
    {
        services.AddTransient<IEnumerableDatabaseReaderFactory<MenuItemsReaderRequest, UserTenantMenuItems>, MenuItemsReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<PasswordValidationReaderRequest, PasswordValidationReaderResponse>, PasswordValidationReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<TenantNodesReaderRequest, TenantNode>, TenantNodesReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<TenantsReaderRequest, Tenant>, TenantsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<UserTenantActionReaderRequest, UserTenantAction>, UserTenantActionReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<UserTenantEditActionReaderRequest, UserTenantEditAction>, UserTenantEditActionReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<UserTenantEditOwnActionReaderRequest, UserTenantEditOwnAction>, UserTenantEditOwnActionReaderFactory>();
    }
}
