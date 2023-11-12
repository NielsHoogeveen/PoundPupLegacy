using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper,
    IEnumerableMapper<PersonPoliticalEntityRelation.Complete.ToCreateForNewPerson, PartyPoliticalEntityRelation.ToCreate.ForNewParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson, DomainModel.PersonOrganizationRelation.ToCreate.ForNewPerson> personOrganizationRelationCreateMapper,
    IEnumerableMapper<InterPersonalRelation.From.Complete.ToCreateForNewPerson, DomainModel.InterPersonalRelation.ToCreate.ForNewPersonFrom> interPersonalRelationFromMapper,
    IEnumerableMapper<InterPersonalRelation.To.Complete.ToCreateForNewPerson, DomainModel.InterPersonalRelation.ToCreate.ForNewPersonTo> interPersonalRelationToMapper,
    IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate> locatableDetailsMapper
) : IMapper<Person.ToCreate, DomainModel.Person.ToCreate>
{
    public DomainModel.Person.ToCreate Map(Person.ToCreate source)
    {
        return new DomainModel.Person.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            LocatableDetails = locatableDetailsMapper.Map(source.LocatableDetailsForCreate),
            PersonDetails = new DomainModel.PersonDetails.ForCreate {
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.PersonDetailsForCreate.PersonPoliticalEntityRelations.OfType<PersonPoliticalEntityRelation.Complete.ToCreateForNewPerson>()).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.PersonDetailsForCreate.PersonOrganizationRelations.OfType<PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson>()).ToList(),
                InterPersonalRelationsToCreateFrom = interPersonalRelationFromMapper.Map(source.PersonDetailsForCreate.InterPersonalRelationsFrom.OfType<InterPersonalRelation.From.Complete.ToCreateForNewPerson>()).ToList(),
                InterPersonalRelationsToCreateTo = interPersonalRelationToMapper.Map(source.PersonDetailsForCreate.InterPersonalRelationsTo.OfType<InterPersonalRelation.To.Complete.ToCreateForNewPerson>()).ToList(),
                DateOfBirth = source.PersonDetails.DateOfBirth,
                DateOfDeath = source.PersonDetails.DateOfDeath,
                FileIdPortrait = source.PersonDetails.FileIdPortrait,
                FirstName = source.PersonDetails.FirstName,
                FullName = source.PersonDetails.FullName,
                LastName = source.PersonDetails.LastName,
                MiddleName = source.PersonDetails.MiddleName,
                Suffix = source.PersonDetails.Suffix,
                ProfessionalRolesToCreate = new List<ProfessionalRoleToCreateForNewPerson>(),
            },
        };
    }
}
