//using Microsoft.Extensions.Logging;
//using PoundPupLegacy.CreateModel;
//using PoundPupLegacy.CreateModel.Deleters;

//namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

//internal sealed class OrganizationEditService(
//        IDbConnection connection,
//        ILogger<OrganizationEditService> logger,
//        ITenantRefreshService tenantRefreshService,
//        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewOrganization> createViewModelReaderFactory,
//        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingOrganization> updateViewModelReaderFactory,
//        IDatabaseUpdaterFactory<ImmediatelyIdentifiableOrganization> updaterFactory,
//        IEntityCreatorFactory<EventuallyIdentifiableOrganization> creatorFactory,
//        ITextService textService
//    ) : NodeEditServiceBase<
//        Organization, 
//        Organization,
//        ExistingOrganization, 
//        NewOrganization, 
//        NewOrganization, 
//        CreateModel.Organization,
//        EventuallyIdentifiableOrganization,
//        ImmediatelyIdentifiableOrganization>
//    (
//        connection,
//        logger,
//        tenantRefreshService,
//        creatorFactory,
//        updaterFactory,
//        createViewModelReaderFactory,
//        updateViewModelReaderFactory
//   ), IEditService<Organization, Organization>
//{

//    protected override ImmediatelyIdentifiableOrganization Map(ExistingOrganization viewModel)
//    {
//        return new CreateModel.ExistingOrganization {
//            Id = viewModel.NodeId,
//            Title = viewModel.Title,
//            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
//            EmailAddress = viewModel.EmailAddress,
//            Established = viewModel.Establishment,
//            Terminated = viewModel.Termination,
//            WebsiteUrl = viewModel.WebSiteUrl,
//            ChangedDateTime = DateTime.Now,
//            AuthoringStatusId = 1,
//            FileIdTileImage = null,
//            NodeTermsToAdd = new List<NodeTermToAdd>(),
//            TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
//            NodeTermsToRemove = new List<NodeTermToRemove>(),
//            OrganizationTypeIdsToAdd = new List<int>(),
//            OrganizationTypeIdsToRemove = new List<int>(),
//            TenantNodesToRemove = new List<TenantNodeToDelete>(),
//            TenantNodesToUpdate = new List<CreateModel.ExistingTenantNode>(),
//            TermsToAdd = new List<NewTermForExistingNameable>(),
//            LocationsToAdd = new List<EventuallyIdentifiableLocation>(),
//            LocationsToDelete = new List<int>(),
//            LocationsToUpdate = new List<ImmediatelyIdentifiableLocation>(),
//            PartyPoliticalEntityRelationsToAdd = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty>(),
//            PartyPoliticalEntityRelationsToUpdate = new List<ImmediatelyIdentifiablePartyPoliticalEntityRelation>(),
//            PersonOrganizationRelationsToUpdate = new List<ImmediatelyIdentifiablePersonOrganizationRelation>(),
//            PersonOrganizationRelationsToAdd = new List<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants>(),
//            InterOrganizationalRelationsToAdd = new List<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants>(),
//            InterOrganizationalRelationsToUpdate = new List<ImmediatelyIdentifiableInterOrganizationalRelation>(),
//        };
//    }

//    protected override EventuallyIdentifiableOrganization Map(NewOrganization newViewModel)
//    {
//        return new CreateModel.NewBasicOrganization {
//            Id = null,
//            Title = newViewModel.Title,
//            Description = newViewModel.Description is null ? "" : textService.FormatText(newViewModel.Description),
//            EmailAddress = newViewModel.EmailAddress,
//            Established = newViewModel.Establishment,
//            Terminated = newViewModel.Termination,
//            WebsiteUrl = newViewModel.WebSiteUrl,
//            ChangedDateTime = DateTime.Now,
//            CreatedDateTime = DateTime.Now,
//            NodeTypeId = Constants.ORGANIZATION,
//            OwnerId = newViewModel.OwnerId,
//            AuthoringStatusId = 1,
//            PublisherId = newViewModel.PublisherId,
//            FileIdTileImage = null,
//            TermIds = new List<int>(),
//            OrganizationTypeIds = new List<int>(),
//            TenantNodes = new List<NewTenantNodeForNewNode>(),
//            Terms = new List<NewTermForNewNameable>(),
//            Locations = new List<EventuallyIdentifiableLocation>(),
//            PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
//            PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization>(),
//            InterOrganizationalRelationsToAddFrom = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom>(),
//            InterOrganizationalRelationsToAddTo = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo>(),
//        };
//    }
//}
