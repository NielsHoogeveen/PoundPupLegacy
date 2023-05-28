using static PoundPupLegacy.CreateModel.InterOrganizationalRelation;

namespace PoundPupLegacy.CreateModel;

public abstract record PersonOrganizationRelation: Node
{
    private PersonOrganizationRelation() { }
    public required PersonOrganizationRelationDetails PersonOrganizationRelationDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(
        Func<PersonOrganizationRelationToCreateForExistingParticipants, T> create,
        Func<PersonOrganizationRelationToCreateForNewPerson, T> createNewPerson,
        Func<PersonOrganizationRelationToCreateForNewOrganization, T> createNewOrganization,
        Func<PersonOrganizationRelationToUpdate, T> update
     );
    public abstract void Match(
        Action<PersonOrganizationRelationToCreateForExistingParticipants> create,
        Action<PersonOrganizationRelationToCreateForNewPerson> createNewPerson,
        Action<PersonOrganizationRelationToCreateForNewOrganization> createNewOrganization,
        Action<PersonOrganizationRelationToUpdate> update
    );

    public sealed record PersonOrganizationRelationToCreateForNewPerson : PersonOrganizationRelation, NodeToCreate
    {
        public required int? PersonId { get; set; }
        public required int OrganizationId { get; init; }

        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public PersonOrganizationRelationToCreateForExistingParticipants ResolvePerson(int personId)
        {
            return new PersonOrganizationRelationToCreateForExistingParticipants {
                PersonId = personId,
                OrganizationId = OrganizationId,
                NodeDetailsForCreate = NodeDetailsForCreate,
                NodeIdentificationForCreate = NodeIdentificationForCreate,
                PersonOrganizationRelationDetails = PersonOrganizationRelationDetails,
            };
        }
        public override T Match<T>(
            Func<PersonOrganizationRelationToCreateForExistingParticipants, T> create,
            Func<PersonOrganizationRelationToCreateForNewPerson, T> createNewPerson,
            Func<PersonOrganizationRelationToCreateForNewOrganization, T> createNewOrganization,
            Func<PersonOrganizationRelationToUpdate, T> update
         )
        {
            return createNewPerson(this);
        }
        public override void Match(
            Action<PersonOrganizationRelationToCreateForExistingParticipants> create,
            Action<PersonOrganizationRelationToCreateForNewPerson> createNewPerson,
            Action<PersonOrganizationRelationToCreateForNewOrganization> createNewOrganization,
            Action<PersonOrganizationRelationToUpdate> update
        )
        {
            createNewPerson(this);
        }
    }
    public sealed record PersonOrganizationRelationToCreateForNewOrganization : PersonOrganizationRelation, NodeToCreate
    {
        public required int PersonId { get; set; }
        public required int? OrganizationId { get; set; }
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public PersonOrganizationRelationToCreateForExistingParticipants ResolveOrganization(int organizationId)
        {
            return new PersonOrganizationRelationToCreateForExistingParticipants {
                PersonId = PersonId,
                OrganizationId = organizationId,
                NodeDetailsForCreate = NodeDetailsForCreate,
                NodeIdentificationForCreate = NodeIdentificationForCreate,
                PersonOrganizationRelationDetails = PersonOrganizationRelationDetails,
            };
        }
        public override T Match<T>(
            Func<PersonOrganizationRelationToCreateForExistingParticipants, T> create,
            Func<PersonOrganizationRelationToCreateForNewPerson, T> createNewPerson,
            Func<PersonOrganizationRelationToCreateForNewOrganization, T> createNewOrganization,
            Func<PersonOrganizationRelationToUpdate, T> update
         )
        {
            return createNewOrganization(this);
        }
        public override void Match(
            Action<PersonOrganizationRelationToCreateForExistingParticipants> create,
            Action<PersonOrganizationRelationToCreateForNewPerson> createNewPerson,
            Action<PersonOrganizationRelationToCreateForNewOrganization> createNewOrganization,
            Action<PersonOrganizationRelationToUpdate> update
        )
        {
            createNewOrganization(this);
        }
    }

    public sealed record PersonOrganizationRelationToCreateForExistingParticipants : PersonOrganizationRelation, NodeToCreate
    {
        public required int PersonId { get; set; }
        public required int OrganizationId { get; init; }
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(
            Func<PersonOrganizationRelationToCreateForExistingParticipants, T> create,
            Func<PersonOrganizationRelationToCreateForNewPerson, T> createNewPerson,
            Func<PersonOrganizationRelationToCreateForNewOrganization, T> createNewOrganization,
            Func<PersonOrganizationRelationToUpdate, T> update
         )
        {
            return create(this);
        }
        public override void Match(
            Action<PersonOrganizationRelationToCreateForExistingParticipants> create,
            Action<PersonOrganizationRelationToCreateForNewPerson> createNewPerson,
            Action<PersonOrganizationRelationToCreateForNewOrganization> createNewOrganization,
            Action<PersonOrganizationRelationToUpdate> update
        )
        {
            create(this);
        }
    }

    public sealed record PersonOrganizationRelationToUpdate : PersonOrganizationRelation, NodeToUpdate
    {
        public required int PersonId { get; set; }
        public required int OrganizationId { get; init; }
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(
            Func<PersonOrganizationRelationToCreateForExistingParticipants, T> create,
            Func<PersonOrganizationRelationToCreateForNewPerson, T> createNewPerson,
            Func<PersonOrganizationRelationToCreateForNewOrganization, T> createNewOrganization,
            Func<PersonOrganizationRelationToUpdate, T> update
         )
        {
            return update(this);
        }
        public override void Match(
            Action<PersonOrganizationRelationToCreateForExistingParticipants> create,
            Action<PersonOrganizationRelationToCreateForNewPerson> createNewPerson,
            Action<PersonOrganizationRelationToCreateForNewOrganization> createNewOrganization,
            Action<PersonOrganizationRelationToUpdate> update
        )
        {
            update(this);
        }
    }
}

public sealed record PersonOrganizationRelationDetails
{
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
}