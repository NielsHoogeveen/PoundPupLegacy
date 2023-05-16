namespace PoundPupLegacy.CreateModel;

public sealed record BasicProfessionalRole : ProfessionalRole
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }

    public required int? PersonId { get; set; }

    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }
}
