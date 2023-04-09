using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

public static class DependencyInjection
{
    public static void AddSystemUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdater>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationUpdater>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<SimpleTextNodeUpdater>, SimpleTextNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNodeUpdater>, TenantNodeUpdaterFactory>();
    }
}
