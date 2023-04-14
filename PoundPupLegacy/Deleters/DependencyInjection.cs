using PoundPupLegacy.Common;

namespace PoundPupLegacy.Deleters;

public static class DependencyInjection
{
    public static void AddSystemDeleters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseDeleterFactory<FileDeleterRequest>, FileDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<NodeTermDeleterRequest>, NodeTermDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<TenantNodeDeleterRequest>, TenantNodeDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<LocationDeleterRequest>, LocationDeleterFactory>();
    }
}
