using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper,
    IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToUpdate, DomainModel.InterPersonalRelation.ToUpdate> interOrganizationalRelationFromToUpdateMapper,
    IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToCreate, DomainModel.InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationFromToCreateMapper,
    IEnumerableMapper<InterPersonalRelation.To.Complete.Resolved.ToUpdate, DomainModel.InterPersonalRelation.ToUpdate> interOrganizationalRelationToToUpdateMapper,
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
                InterPersonalRelationsFromToCreate = interPersonalRelationFromToCreateMapper.Map(source.PersonDetailsForUpdate.InterPersonalRelations.OfType<InterPersonalRelation.From.Complete.Resolved.ToCreate>()).ToList(),
                InterPersonalRelationsToToCreate = interPersonalRelationToToCreateMapper.Map(source.PersonDetailsForUpdate.InterPersonalRelations.OfType<InterPersonalRelation.To.Complete.Resolved.ToCreate>()).ToList(),
                PartyPoliticalEntityRelationsToCreate = partyPolitcalEntityCreateMapper.Map(source.PersonDetailsForUpdate.PersonPoliticalEntityRelations.OfType<PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson>()).ToList(),
                PartyPoliticalEntityRelationToUpdate = partyPoliticalEntityRelationUpdateMapper.Map(source.PersonDetailsForUpdate.PersonPoliticalEntityRelations.OfType< PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate> ()).ToList(),
                PersonOrganizationRelationsToCreate = personOrganizationRelationCreateMapper.Map(source.PersonDetailsForUpdate.PersonOrganizationRelations.OfType<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate>()).ToList(),
                PersonOrganizationRelationsToUpdates = personOrganizationRelationUpdateMapper.Map(source.PersonDetailsForUpdate.PersonOrganizationRelations.OfType<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate>()).ToList(),
                InterPersonalRelationFromToUpdate = interOrganizationalRelationFromToUpdateMapper.Map(source.PersonDetailsForUpdate.InterPersonalRelationsFrom.OfType<InterPersonalRelation.From.Complete.Resolved.ToUpdate>()).ToList(),
                InterPersonalRelationToToUpdate = interOrganizationalRelationToToUpdateMapper.Map(source.PersonDetailsForUpdate.InterPersonalRelationsTo.OfType<InterPersonalRelation.To.Complete.Resolved.ToUpdate>()).ToList(),
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
