using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

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
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = childTraffickingCase.Date,
            FileIdTileImage = null,
            Terms = new List<CreateModel.NewTermForNewNameable>(),
            NumberOfChildrenInvolved = childTraffickingCase.NumberOfChildrenInvolved,
            CountryIdFrom = childTraffickingCase.CountryFrom.Id,
            TermIds = new List<int>(),
            Locations = new List<EventuallyIdentifiableLocation>(),
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
            NodeTermsToAdd = new List<NodeTermToAdd>(),
            TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
            NodeTermsToRemove = new List<NodeTermToRemove>(),
            TenantNodesToRemove = new List<TenantNodeToDelete>(),
            TenantNodesToUpdate = new List<CreateModel.ExistingTenantNode>(),
            TermsToAdd = new List<NewTermForExistingNameable>(),
            LocationsToDelete = new List<int>(),
            LocationsToUpdate = new List<ImmediatelyIdentifiableLocation>(),
            LocationsToAdd = new List<EventuallyIdentifiableLocation>(),
            CasePartiesToAdd = new List<ExistingCaseNewCaseParties>(),
            CasePartiesToUpdate = new List<ExistingCaseExistingCaseParties>(),
        };
    }
}
