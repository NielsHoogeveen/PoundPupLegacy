using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Readers;

internal static class DependencyInjection
{
    internal static void AddCreateModelReaders(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseReaderFactory<ActionIdReaderByPath>, ActionIdReaderByPathFactory>();
        services.AddTransient<IDatabaseReaderFactory<CreateNodeActionIdReaderByNodeTypeId>, CreateNodeActionIdReaderByNodeTypeIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<DeleteNodeActionIdReaderByNodeTypeId>, DeleteNodeActionIdReaderByNodeTypeIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<EditNodeActionIdReaderByNodeTypeId>, EditNodeActionIdReaderByNodeTypeIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<EditOwnNodeActionIdReaderByNodeTypeId>, EditOwnNodeActionIdReaderByNodeTypeIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<FileIdReaderByTenantFileId>, FileIdReaderByTenantFileIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<NodeIdReaderByUrlId>, NodeIdReaderByUrlIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<NodeReaderByUrlId>, NodeReaderByUrlIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<ProfessionIdReader>, ProfessionIdReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<SubdivisionIdReaderByIso3166Code>, SubdivisionIdReaderByIso3166CodeFactory>();
        services.AddTransient<IDatabaseReaderFactory<SubdivisionIdReaderByName>, SubdivisionIdReaderByNameFactory>();
        services.AddTransient<IDatabaseReaderFactory<TenantNodeIdReaderByUrlId>, TenantNodeIdReaderByUrlIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<TenantNodeReaderByUrlId>, TenantNodeReaderByUrlIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<TermReaderByName>, TermReaderByNameFactory>();
        services.AddTransient<IDatabaseReaderFactory<TermReaderByNameableId>, TermReaderByNameableIdFactory>();
        services.AddTransient<IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName>, VocabularyIdReaderByOwnerAndNameFactory>();
    }
}
