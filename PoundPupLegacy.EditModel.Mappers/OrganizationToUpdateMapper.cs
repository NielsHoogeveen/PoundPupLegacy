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
                InterOrganizationalRelationsFromToUpdate = interOrganizationalRelationFromToUpdateMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFrom.OfType<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate>()).ToList(),
                InterOrganizationalRelationsToToUpdate = interOrganizationalRelationToToUpdateMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsTo.OfType<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate>()).ToList(),
                InterOrganizationalRelationsFromToCreate = interOrganizationalRelationFromToCreateForExistingOrganizationMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFrom.OfType<InterOrganizationalRelation.From.Complete.Resolved.ToCreate>()).ToList(),
                InterOrganizationalRelationsToToCreate = interOrganizationalRelationToToCreateForExistingOrganizationMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsTo.OfType<InterOrganizationalRelation.To.Complete.Resolved.ToCreate>()).ToList(),
                OrganizationTypeIdsToCreate = source.OrganizationDetails.OrganizationOrganizationTypes.Where(x => !x.HasBeenStored).Select(x => x.OrganizationTypeId).ToList(),
                OrganizationTypeIdsToRemove = source.OrganizationDetails.OrganizationOrganizationTypes.Where(x => x.HasBeenDeleted).Select(x => x.OrganizationTypeId).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.OrganizationDetailsForUpdate.OrganizationPoliticalEntityRelations.OfType<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization>()).ToList(),
                PartyPoliticalEntityRelationsToUpdates = partyPoliticalEntityRelationUpdateMapper.Map(source.OrganizationDetailsForUpdate.OrganizationPoliticalEntityRelations.OfType<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate>()).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.OrganizationDetailsForUpdate.PersonOrganizationRelations.OfType<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate>()).ToList(),
                PersonOrganizationRelationsToUpdate = personOrganizationRelationUpdateMapper.Map(source.OrganizationDetailsForUpdate.PersonOrganizationRelations.OfType<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate>()).ToList(),
            },
        };
    }
}
