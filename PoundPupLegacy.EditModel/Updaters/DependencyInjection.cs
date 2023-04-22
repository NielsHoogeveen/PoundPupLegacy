using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Updaters;

public static class DependencyInjection
{
    public static void AddEditModelUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdaterRequest>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationUpdaterRequest>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest>, SimpleTextNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNodeUpdaterRequest>, TenantNodeUpdaterFactory>();
    }
}
