using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public static class DependencyInjection
{
    public static void AddEditReaders(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseReaderFactory<ArticleCreateDocumentReader>, ArticleCreateDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<BlogPostCreateDocumentReader>, BlogPostCreateDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<DiscussionCreateDocumentReader>, DiscussionCreateDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<ArticleUpdateDocumentReader>, ArticleUpdateDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<BlogPostUpdateDocumentReader>, BlogPostUpdateDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<DiscussionUpdateDocumentReader>, DiscussionUpdateDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<DocumentUpdateDocumentReader>, DocumentUpdateDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<OrganizationUpdateDocumentReader>, OrganizationUpdateDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<SubdivisionListItemsReader>, SubdivisionListItemsReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<DocumentableDocumentsDocumentReader>, DocumentableDocumentsDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<TagDocumentsReader>, TagDocumentsReaderFactory>();
    }
}
