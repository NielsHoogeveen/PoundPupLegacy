using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class PartyEditServiceBase<TEntity, TExisting, TNew, TCreate> : NodeEditServiceBase<TEntity, TExisting, TNew, TCreate>
    where TEntity : class, Party
    where TExisting: TEntity, ExistingNode
    where TNew : TEntity, NewNode
    where TCreate : CreateModel.Party
{
    protected readonly ITextService _textService;
    protected PartyEditServiceBase(
        IDbConnection connection,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITextService textService,
        ITenantRefreshService tenantRefreshService
        ) : base(
            connection,
            tagSaveService,
            tenantNodesSaveService,
            filesSaveService,
            tenantRefreshService)
    {
        _textService = textService;
    }
}
