using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Readers;

internal static class DependencyInjection
{
    internal static void AddCreateModelReaders(this IServiceCollection services)
    {
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<ActionIdReaderByPathRequest, int>, ActionIdReaderByPathFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeIdRequest, int>, CreateNodeActionIdReaderByNodeTypeIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<DeleteNodeActionIdReaderByNodeTypeIdRequest, int>, DeleteNodeActionIdReaderByNodeTypeIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<EditNodeActionIdReaderByNodeTypeIdRequest, int>, EditNodeActionIdReaderByNodeTypeIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<EditOwnNodeActionIdReaderByNodeTypeIdRequest, int>, EditOwnNodeActionIdReaderByNodeTypeIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int>, FileIdReaderByTenantFileIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int>, NodeIdReaderByUrlIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<NodeReaderByUrlIdRequest, NodeTitle>, NodeReaderByUrlIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<ProfessionIdReaderRequest, int>, ProfessionIdReaderFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<SubdivisionIdReaderByIso3166CodeRequest, int>, SubdivisionIdReaderByIso3166CodeFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<SubdivisionIdReaderByNameRequest, int>, SubdivisionIdReaderByNameFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<TenantNodeIdReaderByUrlIdRequest, int>, TenantNodeIdReaderByUrlIdFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, NewTenantNodeForExistingNode>, TenantNodeReaderByUrlIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<NameableIdReaderByTermNameRequest, int>, NameableIdReaderByTermNameFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int>, TermIdReaderByNameFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameableIdRequest, int>, TermIdReaderByNameableIdFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<PossibleTermReaderByNameableIdRequest, PossibleTerm>, PossibleTermReaderByNameableIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<TermNameReaderByNameableIdRequest, string>, TermNameReaderByNameableIdFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int>, VocabularyIdReaderByOwnerAndNameFactory>();
    }
    
}
