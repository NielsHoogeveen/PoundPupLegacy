//using PoundPupLegacy.CreateModel.Deleters;
//using PoundPupLegacy.CreateModel;
//using PoundPupLegacy.EditModel.UI.Services;

//namespace PoundPupLegacy.EditModel.UI.Mappers;

//internal class OrganizationUpdateMapper(
//    ITextService textService,
//    IMapper<EditModel.NodeDetails.NodeDetailsForUpdate, CreateModel.NodeDetails.NodeDetailsForUpdate> nodeDetailMapper,
//    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.NameableDetailsForUpdate> nameableDetailMapper,
//    IEnumerableMapper<TenantNode.NewTenantNodeForExistingNode, CreateModel.TenantNode.TenantNodeToCreateForExistingNode> newTenantNodeMapper,
//    IEnumerableMapper<TenantNode.ExistingTenantNode, CreateModel.TenantNode.TenantNodeToUpdate> tenantNodeToUpdateMapper,
//    IEnumerableMapper<TenantNode.ExistingTenantNode, TenantNodeToDelete> tenantNodeToRemoveMapper,
//    IEnumerableMapper<Tags, NodeTermToAdd> nodeTermsToAddMapper,
//    IEnumerableMapper<Tags, NodeTermToRemove> nodeTermsToRemoveMapper,
//    IEnumerableMapper<Location.ExistingLocation, int> locationsToDeleteMapper,
//    IEnumerableMapper<Location.ExistingLocation, EventuallyIdentifiableLocation> locationsToAddMapper,
//    IEnumerableMapper<Location.ExistingLocation, ImmediatelyIdentifiableLocation> locationsToUpdateMapper
//) : IMapper<Organization.ExistingOrganization, CreateModel.OrganizationToUpdate>
//{
//    public CreateModel.OrganizationToUpdate Map(Organization.ExistingOrganization source)
//    {
//        return new CreateModel.BasicOrganization.BasicOrganizationToUpdate {
//            IdentificationForUpdate = new Identification.IdentificationForUpdate {
//                Id = source.NodeIdentification.NodeId,
//            },
//            NodeDetailsForUpdate = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
//            NameableDetailsForUpdate=  nameableDetailMapper.Map(source.NameableDetails),
//            OrganizationDetailsForUpdate = new CreateModel.OrganizationDetails.OrganizationDetailsForUpdate {
//                EmailAddress = source.OrganizationDetails.EmailAddress,
//                Established = source.OrganizationDetails.Establishment,
//                Terminated = source.OrganizationDetails.Termination,
//                WebsiteUrl = source.OrganizationDetails.WebSiteUrl,
//            },
//            FileIdTileImage = null,
//            NodeTermsToAdd = nodeTermsToAddMapper.Map(source.NodeDetails.Tags).ToList(),
//            NodeTermsToRemove = nodeTermsToRemoveMapper.Map(source.NodeDetails.Tags).ToList(),
//            OrganizationTypeIdsToAdd = new List<int>(),
//            OrganizationTypeIdsToRemove = new List<int>(),
//            TenantNodesToAdd = newTenantNodeMapper.Map(source.ExistingTenantNodeDetails.TenantNodesToAdd).ToList(),
//            TenantNodesToRemove = tenantNodeToRemoveMapper.Map(source.ExistingTenantNodeDetails.TenantNodesToUpdate).ToList(),
//            TenantNodesToUpdate = tenantNodeToUpdateMapper.Map(source.ExistingTenantNodeDetails.TenantNodesToUpdate).ToList(),
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
//}
