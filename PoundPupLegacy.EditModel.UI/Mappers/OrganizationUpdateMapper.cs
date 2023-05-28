using PoundPupLegacy.CreateModel.Deleters;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class OrganizationUpdateMapper(
    ITextService textService,
    IEnumerableMapper<TenantNode.NewTenantNodeForExistingNode, NewTenantNodeForExistingNode> newTenantNodeMapper,
    IEnumerableMapper<TenantNode.ExistingTenantNode, ExistingTenantNode> tenantNodeToUpdateMapper,
    IEnumerableMapper<TenantNode.ExistingTenantNode, TenantNodeToDelete> tenantNodeToRemoveMapper,
    IEnumerableMapper<Tags, NodeTermToAdd> nodeTermsToAddMapper,
    IEnumerableMapper<Tags, NodeTermToRemove> nodeTermsToRemoveMapper,
    IEnumerableMapper<Location.ExistingLocation, int> locationsToDeleteMapper,
    IEnumerableMapper<Location.ExistingLocation, EventuallyIdentifiableLocation> locationsToAddMapper,
    IEnumerableMapper<Location.ExistingLocation, ImmediatelyIdentifiableLocation> locationsToUpdateMapper
) : IMapper<Organization.ExistingOrganization, CreateModel.ExistingOrganization>
{
    public CreateModel.ExistingOrganization Map(Organization.ExistingOrganization source)
    {
        return new CreateModel.ExistingOrganization {
            Id = source.NodeIdentification.NodeId,
            Title = source.NodeDetails.Title,
            Description = source.NameableDetails.Description is null ? "" : textService.FormatText(source.NameableDetails.Description),
            EmailAddress = source.OrganizationDetails.EmailAddress,
            Established = source.OrganizationDetails.Establishment,
            Terminated = source.OrganizationDetails.Termination,
            WebsiteUrl = source.OrganizationDetails.WebSiteUrl,
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            FileIdTileImage = null,
            NodeTermsToAdd = nodeTermsToAddMapper.Map(source.NodeDetails.Tags).ToList(),
            NodeTermsToRemove = nodeTermsToRemoveMapper.Map(source.NodeDetails.Tags).ToList(),
            OrganizationTypeIdsToAdd = new List<int>(),
            OrganizationTypeIdsToRemove = new List<int>(),
            TenantNodesToAdd = newTenantNodeMapper.Map(source.ExistingTenantNodeDetails.TenantNodesToAdd).ToList(),
            TenantNodesToRemove = tenantNodeToRemoveMapper.Map(source.ExistingTenantNodeDetails.TenantNodesToUpdate).ToList(),
            TenantNodesToUpdate = tenantNodeToUpdateMapper.Map(source.ExistingTenantNodeDetails.TenantNodesToUpdate).ToList(),
            TermsToAdd = new List<NewTermForExistingNameable>(),
            LocationsToAdd = new List<EventuallyIdentifiableLocation>(),
            LocationsToDelete = new List<int>(),
            LocationsToUpdate = new List<ImmediatelyIdentifiableLocation>(),
            PartyPoliticalEntityRelationsToAdd = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty>(),
            PartyPoliticalEntityRelationsToUpdate = new List<ImmediatelyIdentifiablePartyPoliticalEntityRelation>(),
            PersonOrganizationRelationsToUpdate = new List<ImmediatelyIdentifiablePersonOrganizationRelation>(),
            PersonOrganizationRelationsToAdd = new List<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants>(),
            InterOrganizationalRelationsToAdd = new List<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants>(),
            InterOrganizationalRelationsToUpdate = new List<ImmediatelyIdentifiableInterOrganizationalRelation>(),
        };
    }
}
