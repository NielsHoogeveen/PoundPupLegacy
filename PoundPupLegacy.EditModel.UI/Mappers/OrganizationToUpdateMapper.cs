using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class OrganizationToUpdateMapper(
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailMapper,
    IEnumerableMapper<EditModel.InterOrganizationalRelation.From.Complete.Resolved.ToUpdate, CreateModel.InterOrganizationalRelation.ToUpdate> interOrganizationalRelationFromToUpdateMapper,
    IEnumerableMapper<EditModel.InterOrganizationalRelation.To.Complete.Resolved.ToUpdate, CreateModel.InterOrganizationalRelation.ToUpdate> interOrganizationalRelationToToUpdateMapper,
    IEnumerableMapper<EditModel.InterOrganizationalRelation.From.Complete.Resolved.ToCreate, CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationFromToCreateForExistingOrganizationMapper,
    IEnumerableMapper<EditModel.InterOrganizationalRelation.To.Complete.Resolved.ToCreate, CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationToToCreateForExistingOrganizationMapper,
    IEnumerableMapper<EditModel.OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization, PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<EditModel.OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityRelationUpdateMapper,
    IEnumerableMapper<EditModel.PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate, CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreateMapper,
    IEnumerableMapper<EditModel.PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate, CreateModel.PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdateMapper,
    IMapper<EditModel.LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableDetailsMapper
) : IMapper<Organization.ToUpdate, CreateModel.OrganizationToUpdate>
{
    public CreateModel.OrganizationToUpdate Map(Organization.ToUpdate source)
    {
        return new CreateModel.BasicOrganization.ToUpdate {
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
