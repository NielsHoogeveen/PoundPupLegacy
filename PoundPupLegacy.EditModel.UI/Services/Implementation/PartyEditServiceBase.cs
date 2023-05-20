using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class PartyEditServiceBase<TEntity, TExisting, TNew, TCreate>(
    IDbConnection connection,
    ILogger logger,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITextService textService,
    ITenantRefreshService tenantRefreshService
) : NodeEditServiceBase<TEntity, TExisting, TNew, TCreate>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
)
    where TEntity : class, Party
    where TExisting : TEntity, ExistingNode
    where TNew : TEntity, NewNode
    where TCreate : CreateModel.Party
{
}
