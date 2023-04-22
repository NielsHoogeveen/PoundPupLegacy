using PoundPupLegacy.EditModel;
using System.Data;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class PartyEditServiceBase<T, TCreate> : NodeEditServiceBase<T, TCreate>
    where T : Party
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
