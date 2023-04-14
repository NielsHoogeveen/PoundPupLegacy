using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

public static class DependencyInjection
{
    public static void AddSystemUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdaterRequest>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationUpdaterRequest>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest>, SimpleTextNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNodeUpdaterRequest>, TenantNodeUpdaterFactory>();
    }
}
