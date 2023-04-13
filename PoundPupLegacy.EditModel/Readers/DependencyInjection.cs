using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Readers;

public static class DependencyInjection
{
    public static void AddEditReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Article>, ArticleCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, BlogPost>, BlogPostCreateDocumentReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<CountryListItemsReaderRequest, CountryListItem>, CountryListItemsReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Discussion>, DiscussionCreateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Article>, ArticleUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, BlogPost>, BlogPostUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Discussion>, DiscussionUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Document>, DocumentUpdateDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Organization>, OrganizationUpdateDocumentReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<SubdivisionListItemsReaderRequest, SubdivisionListItem>, SubdivisionListItemsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<DocumentableDocumentsDocumentReaderRequest, DocumentableDocument>, DocumentableDocumentsDocumentReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<TagDocumentsReaderRequest, Tag>, TagDocumentsReaderFactory>();
    }
}
