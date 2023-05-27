namespace PoundPupLegacy.CreateModel;

public sealed record NewPersonOrganizationRelationForNewPerson : NewNodeBase, EventuallyIdentifiablePersonOrganizationRelationForNewPerson
{
    public required int? PersonId { get; set; }
    public required int OrganizationId { get; init; }
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }

    public EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants ResolvePerson(int personId)
    {
        return new NewPersonOrganizationRelationForExistingParticipants {
            PersonId = personId,
            OrganizationId = OrganizationId,
            AuthoringStatusId = AuthoringStatusId,
            ChangedDateTime = ChangedDateTime,
            CreatedDateTime = CreatedDateTime,
            Description = Description,
            DocumentIdProof = DocumentIdProof,
            Id = Id,
            DateRange = DateRange,
            GeographicalEntityId = GeographicalEntityId,
            TermIds = TermIds,
            NodeTypeId = NodeTypeId,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            TenantNodes = TenantNodes,
            Title = Title,
            PersonOrganizationRelationTypeId = PersonOrganizationRelationTypeId
        };
    }
}
public sealed record NewPersonOrganizationRelationForNewOrganization : NewNodeBase, EventuallyIdentifiablePersonOrganizationRelationForNewOrganization
{
    public required int PersonId { get; set; }
    public required int? OrganizationId { get; set; }
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
    public EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants ResolveOrganization(int organizationId)
    {
        return new NewPersonOrganizationRelationForExistingParticipants {
            PersonId = PersonId,
            OrganizationId = organizationId,
            AuthoringStatusId = AuthoringStatusId,
            ChangedDateTime = ChangedDateTime,
            CreatedDateTime = CreatedDateTime,
            Description = Description,
            DocumentIdProof = DocumentIdProof,
            Id = Id,
            DateRange = DateRange,
            GeographicalEntityId = GeographicalEntityId,
            TermIds = TermIds,
            NodeTypeId = NodeTypeId,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            TenantNodes = TenantNodes,
            Title = Title,
            PersonOrganizationRelationTypeId = PersonOrganizationRelationTypeId
        };
    }
}

public sealed record NewPersonOrganizationRelationForExistingParticipants : NewNodeBase, EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants
{
    public required int PersonId { get; set; }
    public required int OrganizationId { get; init; }
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
}

public sealed record ExistingPersonOrganizationRelation : ExistingNodeBase, ImmediatelyIdentifiablePersonOrganizationRelation
{
    public required int PersonId { get; set; }
    public required int OrganizationId { get; init; }
    public required int PersonOrganizationRelationTypeId { get; init; }
    public required DateTimeRange DateRange { get; init; }
    public required int? DocumentIdProof { get; init; }
    public required int? GeographicalEntityId { get; init; }
    public required string? Description { get; init; }
}
public interface ImmediatelyIdentifiablePersonOrganizationRelation : PersonOrganizationRelationForExistingParticipants, ImmediatelyIdentifiableNode
{
}
public interface EventuallyIdentifiablePersonOrganizationRelationForNewPerson : PersonOrganizationRelationForNewPerson, EventuallyIdentifiableNode
{
    public EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants ResolvePerson(int personId);
}
public interface EventuallyIdentifiablePersonOrganizationRelationForNewOrganization : PersonOrganizationRelationForNewOrganization, EventuallyIdentifiableNode
{
    public EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants ResolveOrganization(int organizationId);
}
public interface EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants : PersonOrganizationRelationForExistingParticipants, EventuallyIdentifiableNode
{
}
public interface PersonOrganizationRelationForNewOrganization : PersonOrganizationRelation
{
    int PersonId { get; }
    int? OrganizationId { get; set; }
}
public interface PersonOrganizationRelationForExistingParticipants: PersonOrganizationRelation
{
    int PersonId { get; }
    int OrganizationId { get; }
}
public interface PersonOrganizationRelationForNewPerson: PersonOrganizationRelation
{
    int? PersonId { get; set; }
    int OrganizationId { get; }
}
public interface PersonOrganizationRelation : Node
{
   
    int PersonOrganizationRelationTypeId { get; }
    DateTimeRange DateRange { get; }
    int? DocumentIdProof { get; }
    int? GeographicalEntityId { get; }
    string? Description { get; }
}
