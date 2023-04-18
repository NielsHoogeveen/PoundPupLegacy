using PoundPupLegacy.CreateModel;
using PoundPupLegacy.Deleters;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.Inserters;
using PoundPupLegacy.Readers;
using PoundPupLegacy.Services.Implementation;
using PoundPupLegacy.Updaters;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.Services;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddEditReaders();
        services.AddSystemReaders();
        services.AddSystemDeleters();
        services.AddSystemUpdaters();
        services.AddSystemInserters();
        services.AddViewModelReaders();
        services.AddCreateModelAccessors();

        services.AddTransient<IFetchNodeService, FetchNodeService>();
        services.AddTransient<IFetchBlogService, FetchBlogService>();
        services.AddTransient<IFetchBlogsService, FetchBlogsService>();
        services.AddTransient<IFetchArticlesService, FetchArticlesService>();
        services.AddTransient<IFetchPollsService, FetchPollsService>();
        services.AddTransient<IFetchOrganizationsService, FetchOrganizationsService>();
        services.AddTransient<IFetchCasesService, FetchCasesService>();
        services.AddTransient<IFetchCountriesService, FetchCountriesService>();
        services.AddTransient<IFetchSearchService, FetchSearchService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IEditService<EditModel.Article>, ArticleEditService>();
        services.AddTransient<IEditService<EditModel.BlogPost>, BlogPostEditService>();
        services.AddTransient<IEditService<EditModel.Discussion>, DiscussionEditService>();
        services.AddTransient<IEditService<EditModel.Organization>, OrganizationEditService>();
        services.AddTransient<IEditService<EditModel.Document>, DocumentEditService>();
        services.AddTransient<ISaveService<IEnumerable<EditModel.File>>, FilesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<EditModel.TenantNode>>, TenantNodesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<EditModel.Tag>>, TagsSaveService>();
        services.AddTransient<ISaveService<IEnumerable<EditModel.Location>>, LocationsSaveService>();
        services.AddTransient<ITextService, TextService>();
        services.AddTransient<ITopicSearchService, TopicSearchService>();
        services.AddTransient<ICongressionalDataService, CongressionalDataService>();
        services.AddTransient<ISubgroupService, SubgroupService>();
        services.AddTransient<ITopicService, TopicService>();
        services.AddTransient<IPersonService, FetchPersonsService>();
        services.AddTransient<IDocumentableDocumentsSearchService, DocumentableDocumentsSearchService>();
        services.AddTransient<IAttachmentService, AttachmentService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ILocationService, LocationService>();
        services.AddSingleton<ISiteDataService, SiteDataService>();

    }
}
