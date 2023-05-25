using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class ChildTraffickingCaseEditService(
    IDbConnection connection,
    ILogger<ChildTraffickingCaseEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewChildTraffickingCase> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingChildTraffickingCase> updateViewModelReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableChildTraffickingCase> creatorFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableChildTraffickingCase> updaterFactory,
    ITextService textService
) : NodeEditServiceBase<
        EditModel.ChildTraffickingCase, 
        ResolvedChildTraffickingCase, 
        ExistingChildTraffickingCase, 
        NewChildTraffickingCase, 
        ResolvedNewChildTraffickingCase, 
        CreateModel.ChildTraffickingCase, 
        EventuallyIdentifiableChildTraffickingCase, 
        ImmediatelyIdentifiableChildTraffickingCase>
(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<ChildTraffickingCase, ResolvedChildTraffickingCase>
{

    protected sealed override EventuallyIdentifiableChildTraffickingCase Map(ResolvedNewChildTraffickingCase childTraffickingCase)
    {
        var now = DateTime.Now;
        return new CreateModel.NewChildTraffickingCase {
            Id = null,
            Title = childTraffickingCase.Title,
            Description = childTraffickingCase.Description is null ? "" : textService.FormatText(childTraffickingCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = childTraffickingCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = childTraffickingCase.PublisherId,
            TenantNodes = childTraffickingCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = childTraffickingCase.Date,
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = childTraffickingCase.Title,
                    ParentNames = new List<string>(),
                }
            },
            NumberOfChildrenInvolved = childTraffickingCase.NumberOfChildrenInvolved,
            CountryIdFrom = childTraffickingCase.CountryFrom.Id,
            NodeTermIds = new List<int>(),
            NewLocations = new List<EventuallyIdentifiableLocation>(),
            CaseParties = new List<NewCaseNewCaseParties>(),
        };
    }

    protected sealed override ImmediatelyIdentifiableChildTraffickingCase Map(ExistingChildTraffickingCase viewModel)
    {
        return new CreateModel.ExistingChildTraffickingCase {
            Id = viewModel.NodeId,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            CountryIdFrom = viewModel.CountryFrom.Id,
            Date = viewModel.Date,
            NumberOfChildrenInvolved = viewModel.NumberOfChildrenInvolved,
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
