using PoundPupLegacy.EditModel;
using System.Data;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.Services.Implementation;

internal abstract class PartyEditServiceBase<T, TCreate> : NodeEditServiceBase<T, TCreate>
    where T : Party
    where TCreate : CreateModel.Party
{
    protected readonly ITextService _textService;
    protected PartyEditServiceBase(
        IDbConnection connection,
        ISiteDataService siteDataService,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITextService textService,
        ILogger logger
        ) : base(
            connection,
            siteDataService,
            tagSaveService,
            tenantNodesSaveService,
            filesSaveService,
            logger)
    {
        _textService = textService;
    }
}
