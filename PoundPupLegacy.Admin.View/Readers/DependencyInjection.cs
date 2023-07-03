using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.Admin.View.Readers;

public static class DependencyInjection
{
    public static void AddAdminViewModelReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<TenantReaderRequest, Tenant>, TenantReaderFactory>();
    }
}
