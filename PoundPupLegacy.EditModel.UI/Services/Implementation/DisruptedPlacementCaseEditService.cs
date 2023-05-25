using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DisruptedPlacementCaseEditService(
    IDbConnection connection,
    ILogger<DisruptedPlacementCaseEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDisruptedPlacementCase> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDisruptedPlacementCase> updateViewModelReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableDisruptedPlacementCase> creatorFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableDisruptedPlacementCase> updaterFactory,
    ITextService textService
) : NodeEditServiceBase<
        EditModel.DisruptedPlacementCase,
        DisruptedPlacementCase,
        ExistingDisruptedPlacementCase,
        NewDisruptedPlacementCase,
        NewDisruptedPlacementCase,
        CreateModel.DisruptedPlacementCase,
        EventuallyIdentifiableDisruptedPlacementCase,
        ImmediatelyIdentifiableDisruptedPlacementCase>
(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<DisruptedPlacementCase, DisruptedPlacementCase>
{

    protected sealed override EventuallyIdentifiableDisruptedPlacementCase Map(NewDisruptedPlacementCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.NewDisruptedPlacementCase {
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
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = viewModel.Date,
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName> {
                new  VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = viewModel.Title,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
            NewLocations = new List<EventuallyIdentifiableLocation>(),
        };
    }

    protected sealed override ImmediatelyIdentifiableDisruptedPlacementCase Map(ExistingDisruptedPlacementCase viewModel)
    {
        return new CreateModel.ExistingDisruptedPlacementCase {
            Id = viewModel.NodeId,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            Date = viewModel.Date,
            FileIdTileImage = null,
            NewNodeTerms = new List<NodeTerm>(),
            NewTenantNodes = new List<NewTenantNodeForExistingNode>(),
            NodeTermsToRemove = new List<NodeTerm>(),
            TenantNodesToRemove = new List<ExistingTenantNode>(),
            TenantNodesToUpdate = new List<ExistingTenantNode>(),
            VocabularyNames = new List<VocabularyName>(),
            LocationsToDelete = new List<int>(),
            LocationsToUpdate = new List<ImmediatelyIdentifiableLocation>(),
            NewLocations = new List<EventuallyIdentifiableLocation>(),
        };
    }
}
