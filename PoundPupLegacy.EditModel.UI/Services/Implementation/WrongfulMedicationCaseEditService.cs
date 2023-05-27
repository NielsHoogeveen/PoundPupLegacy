using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class WrongfulMedicationCaseEditService(
    IDbConnection connection,
    ILogger<WrongfulMedicationCaseEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewWrongfulMedicationCase> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingWrongfulMedicationCase> updateViewModelReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableWrongfulMedicationCase> creatorFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableWrongfulMedicationCase> updaterFactory,
    ITextService textService
) : NodeEditServiceBase<
        EditModel.WrongfulMedicationCase,
        WrongfulMedicationCase,
        ExistingWrongfulMedicationCase,
        NewWrongfulMedicationCase,
        NewWrongfulMedicationCase,
        CreateModel.WrongfulMedicationCase,
        EventuallyIdentifiableWrongfulMedicationCase,
        ImmediatelyIdentifiableWrongfulMedicationCase>
(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<WrongfulMedicationCase, WrongfulMedicationCase>
{

    protected sealed override EventuallyIdentifiableWrongfulMedicationCase Map(NewWrongfulMedicationCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.NewWrongfulMedicationCase {
            Id = null,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = viewModel.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = viewModel.PublisherId,
            TenantNodes = viewModel.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = viewModel.Date,
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>(),
            TermIds = new List<int>(),
            Locations = new List<EventuallyIdentifiableLocation>(),
            CaseParties = new List<NewCaseNewCaseParties>(),
        };
    }

    protected sealed override ImmediatelyIdentifiableWrongfulMedicationCase Map(ExistingWrongfulMedicationCase viewModel)
    {
        return new CreateModel.ExistingWrongfulMedicationCase {
            Id = viewModel.NodeId,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            Date = viewModel.Date,
            FileIdTileImage = null,
            NodeTermsToAdd = new List<NodeTermToAdd>(),
            TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
            NodeTermsToRemove = new List<NodeTermToRemove>(),
            TenantNodesToRemove = new List<TenantNodeToDelete>(),
            TenantNodesToUpdate = new List<CreateModel.ExistingTenantNode>(),
            TermsToAdd = new List<NewTermForExistingNameable>(),
            LocationsToAdd = new List<EventuallyIdentifiableLocation>(),
            LocationsToDelete = new List<int>(),
            LocationsToUpdate = new List<ImmediatelyIdentifiableLocation>(),
            CasePartiesToAdd = new List<ExistingCaseNewCaseParties>(),
            CasePartiesToUpdate = new List<ExistingCaseExistingCaseParties>(),
        };
    }
}
