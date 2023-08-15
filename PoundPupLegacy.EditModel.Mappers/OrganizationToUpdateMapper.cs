using PoundPupLegacy.DomainModel;
using static PoundPupLegacy.EditModel.OrganizationItem;

namespace PoundPupLegacy.EditModel.Mappers;

internal class OrganizationToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper,
    IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate, DomainModel.InterOrganizationalRelation.ToUpdate> interOrganizationalRelationFromToUpdateMapper,
    IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate, DomainModel.InterOrganizationalRelation.ToUpdate> interOrganizationalRelationToToUpdateMapper,
    IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToCreate, DomainModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationFromToCreateForExistingOrganizationMapper,
    IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToCreate, DomainModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationToToCreateForExistingOrganizationMapper,
    IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization, PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityRelationUpdateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate, DomainModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate, DomainModel.PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdateMapper,
    IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate> locatableDetailsMapper
) : IMapper<Organization.ToUpdate, OrganizationToUpdate>
{
    public OrganizationToUpdate Map(Organization.ToUpdate source)
    {
        source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFromToCreate = source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFrom.Where(x => !x.RelationDetails.HasBeenDeleted).Select(x => new InterOrganizationalRelation.From.Complete.Resolved.ToCreate {
            InterOrganizationalRelationDetails = x.InterOrganizationalRelationDetails,
            NodeDetailsForCreate = new NodeDetails.ForCreate {
                Title = x.NodeDetails.Title,
                Files = x.NodeDetails.Files,
                NodeTypeId = x.NodeDetails.NodeTypeId,
                NodeTypeName = x.NodeDetails.NodeTypeName,
                OwnerId = x.NodeDetails.OwnerId,
                PublisherId = x.NodeDetails.PublisherId,
                Tenants = x.NodeDetails.Tenants,
            },
            OrganizationFrom = (x.OrganizationItemFrom as OrganizationListItem)!,
            OrganizationTo = x.OrganizationItemTo!,
            RelationDetails = x.RelationDetails
        }).ToList();
        source.OrganizationDetailsForUpdate.InterOrganizationalRelationsToToCreate = source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFrom.Where(x => x.RelationDetails.HasBeenDeleted).Where(x => !x.RelationDetails.HasBeenDeleted).Select(x => new InterOrganizationalRelation.To.Complete.Resolved.ToCreate {
            InterOrganizationalRelationDetails = x.InterOrganizationalRelationDetails,
            NodeDetailsForCreate = new NodeDetails.ForCreate {
                Title = x.NodeDetails.Title,
                Files = x.NodeDetails.Files,
                NodeTypeId = x.NodeDetails.NodeTypeId,
                NodeTypeName = x.NodeDetails.NodeTypeName,
                OwnerId = x.NodeDetails.OwnerId,
                PublisherId = x.NodeDetails.PublisherId,
                Tenants = x.NodeDetails.Tenants,
            },
            OrganizationFrom = (x.OrganizationItemFrom as OrganizationListItem)!,
            OrganizationTo = x.OrganizationItemTo!,
            RelationDetails = x.RelationDetails,

        }).ToList();

        return new BasicOrganization.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            LocatableDetails = locatableDetailsMapper.Map(source.LocatableDetailsForUpdate),
            OrganizationDetails = new DomainModel.OrganizationDetails.ForUpdate {
                EmailAddress = source.OrganizationDetails.EmailAddress,
                Established = source.OrganizationDetails.Establishment,
                Terminated = source.OrganizationDetails.Termination,
                WebsiteUrl = source.OrganizationDetails.WebSiteUrl,
                InterOrganizationalRelationsFromToUpdate = interOrganizationalRelationFromToUpdateMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFromToUpdate).ToList(),
                InterOrganizationalRelationsToToUpdate = interOrganizationalRelationToToUpdateMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsToToUpdate).ToList(),
                InterOrganizationalRelationsFromToCreate = interOrganizationalRelationFromToCreateForExistingOrganizationMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFromToCreate).ToList(),
                InterOrganizationalRelationsToToCreate = interOrganizationalRelationToToCreateForExistingOrganizationMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsToToCreate).ToList(),
                OrganizationTypeIdsToCreate = source.OrganizationDetails.OrganizationOrganizationTypes.Where(x => !x.HasBeenStored).Select(x => x.OrganizationTypeId).ToList(),
                OrganizationTypeIdsToRemove = source.OrganizationDetails.OrganizationOrganizationTypes.Where(x => x.HasBeenDeleted).Select(x => x.OrganizationTypeId).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.OrganizationDetailsForUpdate.OrganizationPoliticalEntityRelationsToCreate).ToList(),
                PartyPoliticalEntityRelationsToUpdates = partyPoliticalEntityRelationUpdateMapper.Map(source.OrganizationDetailsForUpdate.OrganizationPoliticalEntityRelationsToUpdate).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.OrganizationDetailsForUpdate.PersonOrganizationRelationsToCreate).ToList(),
                PersonOrganizationRelationsToUpdate = personOrganizationRelationUpdateMapper.Map(source.OrganizationDetailsForUpdate.PersonOrganizationRelationsToUpdate).ToList(),
            },
        };
    }
}
