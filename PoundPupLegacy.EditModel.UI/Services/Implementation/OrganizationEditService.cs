using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class OrganizationEditService(
        IDbConnection connection,
        ILogger<OrganizationEditService> logger,
        ITenantRefreshService tenantRefreshService,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewOrganization> createViewModelReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingOrganization> updateViewModelReaderFactory,
        IDatabaseUpdaterFactory<ImmediatelyIdentifiableOrganization> updaterFactory,
        IEntityCreatorFactory<EventuallyIdentifiableOrganization> creatorFactory,
        ITextService textService
    ) : NodeEditServiceBase<
        Organization, 
        Organization,
        ExistingOrganization, 
        NewOrganization, 
        NewOrganization, 
        CreateModel.Organization,
        EventuallyIdentifiableOrganization,
        ImmediatelyIdentifiableOrganization>
    (
        connection,
        logger,
        tenantRefreshService,
        creatorFactory,
        updaterFactory,
        createViewModelReaderFactory,
        updateViewModelReaderFactory
   ), IEditService<Organization, Organization>
{

    protected override ImmediatelyIdentifiableOrganization Map(ExistingOrganization viewModel)
    {
        return new CreateModel.ExistingOrganization {
            Id = viewModel.NodeId,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            EmailAddress = viewModel.EmailAddress,
            Established = viewModel.Establishment,
            Terminated = viewModel.Termination,
            WebsiteUrl = viewModel.WebSiteUrl,
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            FileIdTileImage = null,
            NewNodeTerms = new List<NodeTerm>(),
            NewTenantNodes = new List<NewTenantNodeForExistingNode>(),
            NodeTermsToRemove = new List<NodeTerm>(),
            OrganizationTypes = new List<CreateModel.OrganizationOrganizationType>(),
            TenantNodesToRemove = new List<ExistingTenantNode>(),
            TenantNodesToUpdate = new List<ExistingTenantNode>(),
            VocabularyNames = new List<VocabularyName>(),
        };
    }

    protected override EventuallyIdentifiableOrganization Map(NewOrganization newViewModel)
    {
        return new CreateModel.NewBasicOrganization {
            Id = null,
            Title = newViewModel.Title,
            Description = newViewModel.Description is null ? "" : textService.FormatText(newViewModel.Description),
            EmailAddress = newViewModel.EmailAddress,
            Established = newViewModel.Establishment,
            Terminated = newViewModel.Termination,
            WebsiteUrl = newViewModel.WebSiteUrl,
            ChangedDateTime = DateTime.Now,
            CreatedDateTime = DateTime.Now,
            NodeTypeId = Constants.ORGANIZATION,
            OwnerId = newViewModel.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = newViewModel.PublisherId,
            FileIdTileImage = null,
            NodeTermIds = new List<int>(),
            OrganizationTypes = new List<CreateModel.OrganizationOrganizationType>(),
            TenantNodes = new List<NewTenantNodeForNewNode>(),
            VocabularyNames = new List<VocabularyName>(),
        };
    }
}
