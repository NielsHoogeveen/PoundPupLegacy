using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class PersonToCreateMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate> nameableDetailMapper,
    IEnumerableMapper<EditModel.PersonPoliticalEntityRelation.Complete.ToCreateForNewPerson, PartyPoliticalEntityRelation.ToCreate.ForNewParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<EditModel.PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson, CreateModel.PersonOrganizationRelation.ToCreate.ForNewPerson> personOrganizationRelationCreateMapper,
    IEnumerableMapper<EditModel.InterPersonalRelation.From.Complete.ToCreateForNewPerson, CreateModel.InterPersonalRelation.ToCreate.ForNewPersonFrom> interPersonalRelationFromMapper,
    IEnumerableMapper<EditModel.InterPersonalRelation.To.Complete.ToCreateForNewPerson, CreateModel.InterPersonalRelation.ToCreate.ForNewPersonTo> interPersonalRelationToMapper,
    IMapper<EditModel.LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate> locatableDetailsMapper
) : IMapper<Person.ToCreate, CreateModel.Person.ToCreate>
{
    public CreateModel.Person.ToCreate Map(Person.ToCreate source)
    {
        return new CreateModel.Person.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            LocatableDetails = locatableDetailsMapper.Map(source.LocatableDetailsForCreate),
            PersonDetails = new CreateModel.PersonDetails.ForCreate {
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.PersonDetailsForCreate.PersonPoliticalEntityRelationsToCreate).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.PersonDetailsForCreate.PersonOrganizationRelationsToCreate).ToList(),
                InterPersonalRelationsToCreateFrom = interPersonalRelationFromMapper.Map(source.PersonDetailsForCreate.InterPersonalRelationsFromToCreate).ToList(),
                InterPersonalRelationsToCreateTo = interPersonalRelationToMapper.Map(source.PersonDetailsForCreate.InterPersonalRelationsToToCreate).ToList(),
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
                ProfessionalRolesToCreate = new List<ProfessionalRoleToCreateForNewPerson>(),
            },
        };
    }
}
