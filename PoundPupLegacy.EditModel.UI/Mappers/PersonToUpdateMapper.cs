using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class PersonToUpdateMapper(
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForUpdate> nameableDetailMapper,
    IEnumerableMapper<EditModel.InterPersonalRelation, CreateModel.InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationToCreateMapper,
    IEnumerableMapper<EditModel.PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson, PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<EditModel.PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityRelationUpdateMapper,
    IEnumerableMapper<EditModel.PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate, CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreateMapper,
    IEnumerableMapper<EditModel.PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate, CreateModel.PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdateMapper,
    IMapper<EditModel.LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate> locatableDetailsMapper
) : IMapper<Person.ToUpdate, CreateModel.Person.ToUpdate>
{
    public CreateModel.Person.ToUpdate Map(Person.ToUpdate source)
    {
        return new CreateModel.Person.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            LocatableDetails = locatableDetailsMapper.Map(source.LocatableDetailsForUpdate),
            PersonDetails = new CreateModel.PersonDetails.ForUpdate {
                InterPersonalRelationsToCreate = interPersonalRelationToCreateMapper.Map(source.PersonDetailsForUpdate.InterPersonalRelations).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.PersonDetailsForUpdate.PersonPoliticalEntityRelationsToCreate).ToList(),
                PartyPoliticalEntityRelationToUpdates = partyPoliticalEntityRelationUpdateMapper.Map(source.PersonDetailsForUpdate.PersonPoliticalEntityRelationsToUpdate).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.PersonDetailsForUpdate.PersonOrganizationRelationsToCreate).ToList(),
                PersonOrganizationRelationsToUpdates = personOrganizationRelationUpdateMapper.Map(source.PersonDetailsForUpdate.PersonOrganizationRelationsToUpdate).ToList(),
                Bioguide = null,
                DateOfBirth = null,
                DateOfDeath = null,
                FileIdPortrait = null,
                FirstName = null,
                FullName = null,
                GovtrackId = null,
                LastName = null,
                MiddleName = null,
                Suffix = null,
                InterPersonalRelationToUpdates = new List<CreateModel.InterPersonalRelation.ToUpdate>(),
                ProfessionalRolesToCreate = new List<ProfessionalRoleToCreateForNewPerson>(),

            },
        };
    }
}
