using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

public static class DependencyInjection
{
    public static void AddSystemUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseUpdaterFactory<SimpleTextNodeUpdater>, SimpleTextNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNodeUpdater>, TenantNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdater>, LocationUpdaterFactory>();
    }
}
