global using PoundPupLegacy.EditModel;
global using PoundPupLegacy.Common;
using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.UI.Services.Implementation;
using PoundPupLegacy.EditModel.UI.Services;
using File = PoundPupLegacy.EditModel.File;
using PoundPupLegacy.EditModel.Deleters;
using PoundPupLegacy.EditModel.Inserters;
using PoundPupLegacy.EditModel.Updaters;

namespace PoundPupLegacy.ViewModel.UI;

public static class DependencyInjection
{
    public static void AddEditModels(this IServiceCollection services)
    {
        services.AddEditModelReaders();
        services.AddEditModelDeleters();
        services.AddEditModelInserters();
        services.AddEditModelUpdaters();
        services.AddTransient<IEditService<Article>, ArticleEditService>();
        services.AddTransient<IEditService<BlogPost>, BlogPostEditService>();
        services.AddTransient<IEditService<Discussion>, DiscussionEditService>();
        services.AddTransient<IEditService<Organization>, OrganizationEditService>();
        services.AddTransient<IEditService<Document>, DocumentEditService>();
        services.AddTransient<ISaveService<IEnumerable<File>>, FilesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<TenantNode>>, TenantNodesSaveService>();
        services.AddTransient<ISaveService<IEnumerable<Tag>>, TagsSaveService>();
        services.AddTransient<ISaveService<IEnumerable<Location>>, LocationsSaveService>();
        services.AddTransient<ITextService, TextService>();
    }
}
