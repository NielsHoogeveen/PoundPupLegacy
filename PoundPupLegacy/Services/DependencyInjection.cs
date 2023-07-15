using PoundPupLegacy.Admin.UI;
using PoundPupLegacy.DomainModel;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.UI;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.Readers;
using PoundPupLegacy.Services.Implementation;
using PoundPupLegacy.ViewModel.UI;

namespace PoundPupLegacy.Services;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddEditModelReaders();
        services.AddSystemReaders();
        services.AddViewModels();
        services.AddEditModels();
        services.AddDomainModelAccessors();
        services.AddAdminServices();

        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IUserService, UserService>();
        services.AddSingleton<ISiteDataService, SiteDataService>();
        services.AddSingleton<INodeAccessReadService, NodeAccessReadService>();
        services.AddSingleton<INodeAccessService, NodeAccessService>();
        services.AddTransient<ITenantRefreshService, TenantRefreshService>();
        services.AddTransient<IDefaultCountryService, DefaultCountryService>();
    }
}
