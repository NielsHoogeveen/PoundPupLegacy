using Microsoft.AspNetCore.Identity.UI.Services;
using PoundPupLegacy.Admin.UI;
using PoundPupLegacy.DomainModel;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.UI;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.Readers;
using PoundPupLegacy.Updaters;
using PoundPupLegacy.Services.Implementation;
using PoundPupLegacy.ViewModel.UI;

namespace PoundPupLegacy.Services;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAdminServices();
        services.AddEditModelReaders();
        services.AddEditModels();
        services.AddDomainModelAccessors();
        services.AddSystemReaders();
        services.AddSystemUpdaters();
        services.AddViewModels();

        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<ICreateOptionsService, CreateOptionsService>();
        services.AddTransient<IDefaultCountryService, DefaultCountryService>();
        services.AddTransient<IListOptionsService, ListOptionsService>();
        services.AddTransient<INodeAccessReadService, NodeAccessReadService>();
        services.AddSingleton<INodeAccessService, NodeAccessService>();
        services.AddTransient<IRemoveExpiredRolesService, RemoveExpiredRolesService>();
        services.AddTransient<ISiteDataLoader, SiteDataLoader>();
        services.AddSingleton<ISiteDataService, SiteDataService>();
        services.AddSingleton<IActiveUserService, ActiveUserService>();
        services.AddTransient<ISiteMapService, SiteMapService>();
        services.AddTransient<ISubgroupsService, SubgroupsService>();
        services.AddTransient<ITenantRefreshService, TenantRefreshService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICommentService, CommentService>();
    }
}
