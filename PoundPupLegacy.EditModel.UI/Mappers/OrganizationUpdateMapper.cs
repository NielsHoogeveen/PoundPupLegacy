using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class OrganizationUpdateMapper(
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailMapper,
    IEnumerableMapper<EditModel.InterOrganizationalRelation, CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationToCreateMapper,
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
                InterOrganizationalRelationsToCreate = interOrganizationalRelationToCreateMapper.Map(source.InterOrganizationalRelation).ToList(),
                OrganizationTypeIds = source.OrganizationDetails.OrganizationTypes.Select(x => x.Id).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.OrganizationPoliticalEntityRelationsToCreate).ToList(),
                PartyPoliticalEntityRelationToUpdates = partyPoliticalEntityRelationUpdateMapper.Map(source.OrganizationPoliticalEntityRelationsToUpdate).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.PersonOrganizationRelationsToCreate).ToList(),
                PersonOrganizationRelationsToUpdate = personOrganizationRelationUpdateMapper.Map(source.PersonOrganizationRelationsToUpdate).ToList(),
            },
        };
    }
}
