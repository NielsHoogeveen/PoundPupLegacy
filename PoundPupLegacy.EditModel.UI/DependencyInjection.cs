global using PoundPupLegacy.Common;
global using PoundPupLegacy.EditModel;
using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.EditModel.Deleters;
using PoundPupLegacy.EditModel.Inserters;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.UI.Services;
using PoundPupLegacy.EditModel.UI.Services.Implementation;
using PoundPupLegacy.EditModel.Updaters;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.ViewModel.UI;

public static class DependencyInjection
{
    public static void AddEditModels(this IServiceCollection services)
    {
        services.AddEditModelReaders();
        services.AddEditModelDeleters();
        services.AddEditModelInserters();
        services.AddEditModelUpdaters();
        services.AddTransient<IAttachmentStoreService, AttachmentStoreService>();
        services.AddTransient<IEditService<Article>, ArticleEditService>();
        services.AddTransient<IEditService<BlogPost>, BlogPostEditService>();
        services.AddTransient<IEditService<Discussion>, DiscussionEditService>();
        services.AddTransient<IEditService<Organization>, OrganizationEditService>();
        services.AddTransient<IEditService<Document>, DocumentEditService>();
        services.AddTransient<ILocationService, LocationService>();
        services.AddTransient<ISaveService<IEnumerable<File>>, FilesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<TenantNode>>, TenantNodesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<Tag>>, TagsSaveService>();
        services.AddTransient<ISaveService<IEnumerable<Location>>, LocationsSaveService>();
        services.AddTransient<ITextService, TextService>();
        services.AddTransient<ITopicSearchService, TopicSearchService>();
    }
}
