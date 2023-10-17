using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.Readers;

public static class DependencyInjection
{
    public static void AddSystemReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<CreateOptionsReaderRequest, List<CreateOptions>>, CreateOptionsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<MenuItemsReaderRequest, UserTenantMenuItems>, MenuItemsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<NodeAccessReaderRequest, NodeAccess>, NodeAccessReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<PasswordValidationReaderRequest, PasswordValidationReaderResponse>, PasswordValidationReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<NamedActionsReaderRequest, NamedAction>, NamedActionsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<SiteMapReaderRequest, SiteMapElement>, SiteMapReaderFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<SiteMapCountReaderRequest, SiteMapCount>, SiteMapCountReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<TenantNodesReaderRequest, TenantNode>, TenantNodesReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<TenantsReaderRequest, Tenant>, TenantsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<UserTenantActionReaderRequest, UserTenantAction>, UserTenantActionReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<UserTenantCreateActionReaderRequest, UserTenantCreateAction>, UserTenantCreateActionReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<UserTenantEditActionReaderRequest, UserTenantEditAction>, UserTenantEditActionReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<UserTenantEditOwnActionReaderRequest, UserTenantEditOwnAction>, UserTenantEditOwnActionReaderFactory>();
    }
}
