using PoundPupLegacy.Common;

namespace PoundPupLegacy.Deleters;

public static class DependencyInjection
{
    public static void AddSystemDeleters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseDeleterFactory<FileDeleter>, FileDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<NodeTermDeleter>, NodeTermDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<TenantNodeDeleter>, TenantNodeDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<LocationDeleter>, LocationDeleterFactory>();
    }
}
