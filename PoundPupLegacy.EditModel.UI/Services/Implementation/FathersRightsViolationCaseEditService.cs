using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class FathersRightsViolationCaseEditService(
    IDbConnection connection,
    ILogger<FathersRightsViolationCaseEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewFathersRightsViolationCase> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingFathersRightsViolationCase> updateViewModelReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableFathersRightsViolationCase> creatorFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableFathersRightsViolationCase> updaterFactory,
    ITextService textService
) : NodeEditServiceBase<
        EditModel.FathersRightsViolationCase,
        FathersRightsViolationCase,
        ExistingFathersRightsViolationCase,
        NewFathersRightsViolationCase,
        NewFathersRightsViolationCase,
        CreateModel.FathersRightsViolationCase,
        EventuallyIdentifiableFathersRightsViolationCase,
        ImmediatelyIdentifiableFathersRightsViolationCase>
(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<FathersRightsViolationCase, FathersRightsViolationCase>
{

    protected sealed override EventuallyIdentifiableFathersRightsViolationCase Map(NewFathersRightsViolationCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.NewFathersRightsViolationCase {
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
            NodeTermIds = new List<int>()
        };
    }

    protected sealed override ImmediatelyIdentifiableFathersRightsViolationCase Map(ExistingFathersRightsViolationCase viewModel)
    {
        return new CreateModel.ExistingFathersRightsViolationCase {
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
            VocabularyNames = new List<VocabularyName>()
        };
    }
}
