using PoundPupLegacy.EditModel;
using PoundPupLegacy.Services.Implementation;
using PoundPupLegacy.Deleters;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.Readers;
using PoundPupLegacy.Updaters;
using PoundPupLegacy.ViewModel.Readers;
using PoundPupLegacy.CreateModel.Creators;

namespace PoundPupLegacy.Services;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddEditReaders();
        services.AddSystemReaders();
        services.AddSystemDeleters();
        services.AddSystemUpdaters();
        services.AddViewModelReaders();
        services.AddEntityCreators();

        services.AddTransient<IFetchNodeService, FetchNodeService>();
        services.AddTransient<IFetchBlogService, FetchBlogService>();
        services.AddTransient<IFetchBlogsService, FetchBlogsService>();
        services.AddTransient<IFetchArticlesService, FetchArticlesService>();
        services.AddTransient<IFetchPollsService, FetchPollsService>();
        services.AddTransient<IFetchOrganizationsService, FetchOrganizationsService>();
        services.AddTransient<IFetchCasesService, FetchCasesService>();
        services.AddTransient<IFetchCountriesService, FetchCountriesService>();
        services.AddTransient<IFetchSearchService, FetchSearchService>();
        services.AddTransient<IRazorViewToStringService, RazorViewToStringService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IEditService<Article>, ArticleEditService>();
        services.AddTransient<IEditService<BlogPost>, BlogPostEditService>();
        services.AddTransient<IEditService<Discussion>, DiscussionEditService>();
        services.AddTransient<IEditService<Organization>, OrganizationEditService>();
        services.AddTransient<IEditService<Document>, DocumentEditService>();
        services.AddTransient<ISaveService<IEnumerable<EditModel.File>>, FilesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<TenantNode>>, TenantNodesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<Tag>>, TagsSaveService>();
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
        services.AddSingleton<INodeCacheService, NodeCacheService>();

    }
}
