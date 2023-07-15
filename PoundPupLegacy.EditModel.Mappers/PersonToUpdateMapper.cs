using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper,
    IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToCreate, DomainModel.InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationFromToCreateMapper,
    IEnumerableMapper<InterPersonalRelation.To.Complete.Resolved.ToCreate, DomainModel.InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationToToCreateMapper,
    IEnumerableMapper<PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson, PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPolitcalEntityCreateMapper,
    IEnumerableMapper<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityRelationUpdateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate, DomainModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreateMapper,
    IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate, DomainModel.PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdateMapper,
    IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate> locatableDetailsMapper
) : IMapper<Person.ToUpdate, DomainModel.Person.ToUpdate>
{
    public DomainModel.Person.ToUpdate Map(Person.ToUpdate source)
    {
        return new DomainModel.Person.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            LocatableDetails = locatableDetailsMapper.Map(source.LocatableDetailsForUpdate),
            PersonDetails = new DomainModel.PersonDetails.ForUpdate {
                InterPersonalRelationsFromToCreate = interPersonalRelationFromToCreateMapper.Map(source.PersonDetailsForUpdate.InterPersonalRelationsFromToCreate).ToList(),
                InterPersonalRelationsToToCreate = interPersonalRelationToToCreateMapper.Map(source.PersonDetailsForUpdate.InterPersonalRelationsToToCreate).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.PersonDetailsForUpdate.PersonPoliticalEntityRelationsToCreate).ToList(),
                PartyPoliticalEntityRelationToUpdate = partyPoliticalEntityRelationUpdateMapper.Map(source.PersonDetailsForUpdate.PartyPoliticalEntityRelationsToUpdate).ToList(),
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
                InterPersonalRelationToUpdates = new List<DomainModel.InterPersonalRelation.ToUpdate>(),
                ProfessionalRolesToCreate = new List<ProfessionalRoleToCreateForNewPerson>(),

            },
        };
    }
}
