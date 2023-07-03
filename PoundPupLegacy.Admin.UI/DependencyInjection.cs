global using PoundPupLegacy.Common;
using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.Admin.UI.Services;
using PoundPupLegacy.Admin.UI.Services.Implementation;
using PoundPupLegacy.Admin.View.Readers;

namespace PoundPupLegacy.Admin.UI;

public static class DependencyInjection
{
    public static void AddAdminServices(this IServiceCollection services)
    {
        services.AddAdminViewModelReaders();
        services.AddTransient<ITenantRetrieveService, TenantRetrieveService>();
    }
}
