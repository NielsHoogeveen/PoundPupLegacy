﻿using PoundPupLegacy.DomainModel;

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