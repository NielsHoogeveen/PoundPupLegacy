using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class OrganizationToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailMapper,
    IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate, CreateModel.InterOrganizationalRelation.ToUpdate> interOrganizationalRelationFromToUpdateMapper,
    IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate, CreateModel.InterOrganizationalRelation.ToUpdate> interOrganizationalRelationToToUpdateMapper,
    IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToCreate, CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationFromToCreateForExistingOrganizationMapper,
    IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToCreate, CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationToToCreateForExistingOrganizationMapper,
    IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization, PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityRelationUpdateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate, CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate, CreateModel.PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdateMapper,
    IMapper<LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableDetailsMapper
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
            OrganizationDetails = new CreateModel.OrganizationDetails.ForUpdate {
                EmailAddress = source.OrganizationDetails.EmailAddress,
                Established = source.OrganizationDetails.Establishment,
                Terminated = source.OrganizationDetails.Termination,
                WebsiteUrl = source.OrganizationDetails.WebSiteUrl,
                InterOrganizationalRelationsFromToUpdate = interOrganizationalRelationFromToUpdateMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFromToUpdate).ToList(),
                InterOrganizationalRelationsToToUpdate = interOrganizationalRelationToToUpdateMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsToToUpdate).ToList(),
                InterOrganizationalRelationsFromToCreate = interOrganizationalRelationFromToCreateForExistingOrganizationMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsFromToCreate).ToList(),
                InterOrganizationalRelationsToToCreate = interOrganizationalRelationToToCreateForExistingOrganizationMapper.Map(source.OrganizationDetailsForUpdate.InterOrganizationalRelationsToToCreate).ToList(),
                OrganizationTypeIds = source.OrganizationDetails.OrganizationTypes.Select(x => x.Id).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.OrganizationDetailsForUpdate.OrganizationPoliticalEntityRelationsToCreate).ToList(),
                PartyPoliticalEntityRelationsToUpdates = partyPoliticalEntityRelationUpdateMapper.Map(source.OrganizationDetailsForUpdate.OrganizationPoliticalEntityRelationsToUpdate).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.OrganizationDetailsForUpdate.PersonOrganizationRelationsToCreate).ToList(),
                PersonOrganizationRelationsToUpdate = personOrganizationRelationUpdateMapper.Map(source.OrganizationDetailsForUpdate.PersonOrganizationRelationsToUpdate).ToList(),
            },
        };
    }
}
