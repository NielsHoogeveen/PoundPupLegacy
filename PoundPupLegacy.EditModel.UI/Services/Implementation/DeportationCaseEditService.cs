using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DeportationCaseEditService(
    IDbConnection connection,
    ILogger<DeportationCaseEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDeportationCase> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDeportationCase> updateViewModelReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableDeportationCase> creatorFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableDeportationCase> updaterFactory,
    ITextService textService
) : NodeEditServiceBase<
        EditModel.DeportationCase,
        DeportationCase,
        ExistingDeportationCase,
        NewDeportationCase,
        NewDeportationCase,
        CreateModel.DeportationCase,
        EventuallyIdentifiableDeportationCase,
        ImmediatelyIdentifiableDeportationCase>
(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<DeportationCase, DeportationCase>
{

    protected sealed override EventuallyIdentifiableDeportationCase Map(NewDeportationCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.NewDeportationCase {
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
            CountryIdTo = viewModel.CountryTo?.Id,
            SubdivisionIdFrom = viewModel.SubdivisionFrom?.Id,
            Locations = new List<EventuallyIdentifiableLocation>(),
            CaseParties = new List<NewCaseNewCaseParties>(),
        };
    }

    protected sealed override ImmediatelyIdentifiableDeportationCase Map(ExistingDeportationCase viewModel)
    {
        return new CreateModel.ExistingDeportationCase {
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
            CountryIdTo = viewModel.CountryTo?.Id,
            SubdivisionIdFrom = viewModel.SubdivisionFrom?.Id,
            LocationsToDelete = new List<int>(),
            LocationsToUpdate = new List<ImmediatelyIdentifiableLocation>(),
            LocationsToAdd = new List<EventuallyIdentifiableLocation>(),
            CasePartiesToAdd = new List<ExistingCaseNewCaseParties>(),
            CasePartiesToUpdate = new List<ExistingCaseExistingCaseParties>(),
        };
    }
}
