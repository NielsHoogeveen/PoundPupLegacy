using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Deleters;

public static class DependencyInjection
{
    public static void AddEditModelDeleters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseDeleterFactory<FileDeleterRequest>, FileDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<NodeTermDeleterRequest>, NodeTermDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<TenantNodeDeleterRequest>, TenantNodeDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<LocationDeleterRequest>, LocationDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<OrganizationOrganizationTypeDeleterRequest>, OrganizationOrganizationTypeDeleterFactory>();
    }
}
