using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class CoercedAdoptionCaseEditService(
    IDbConnection connection,
    ILogger<CoercedAdoptionCaseEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewCoercedAdoptionCase> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingCoercedAdoptionCase> updateViewModelReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableCoercedAdoptionCase> creatorFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableCoercedAdoptionCase> updaterFactory,
    ITextService textService
) : NodeEditServiceBase<
        EditModel.CoercedAdoptionCase,
        CoercedAdoptionCase,
        ExistingCoercedAdoptionCase,
        NewCoercedAdoptionCase,
        NewCoercedAdoptionCase,
        CreateModel.CoercedAdoptionCase,
        EventuallyIdentifiableCoercedAdoptionCase,
        ImmediatelyIdentifiableCoercedAdoptionCase>
(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<CoercedAdoptionCase, CoercedAdoptionCase>
{

    protected sealed override EventuallyIdentifiableCoercedAdoptionCase Map(NewCoercedAdoptionCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.NewCoercedAdoptionCase {
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
                    VocabularyId = Constants.VOCABULARY_ID_TOPICS,
                    TermName = viewModel.Title,
                    ParentTermIds = new List<int>(),
                }
            },
            NodeTermIds = new List<int>(),
            NewLocations = new List<EventuallyIdentifiableLocation>(),
            CaseParties = new List<NewCaseNewCaseParties>(),
        };
    }

    protected sealed override ImmediatelyIdentifiableCoercedAdoptionCase Map(ExistingCoercedAdoptionCase viewModel)
    {
        return new CreateModel.ExistingCoercedAdoptionCase {
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
            CasePartiesToAdd = new List<ExistingCaseNewCaseParties>(),
            CasePartiesToRemove = new List<int>(),
            CasePartiesToUpdate = new List<ExistingCaseExistingCaseParties>(),
        };
    }
}
