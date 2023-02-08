using System.Diagnostics.Contracts;

namespace PoundPupLegacy.Model;

public sealed record InterCountryRelation : Node
{
    public required int? Id { get; set; }
    public required int PublisherId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int? OwnerId { get; init; }
    public required int NodeTypeId { get; init; }
    public required List<TenantNode> TenantNodes { get; init; }
    public required int InterCountryRelationTypeId { get; init; }
    public required int CountryIdFrom { get; init; }
    public required int CountryIdTo { get; init; }
    public required DateTimeRange? DateTimeRange { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int? DocumentIdProof { get; init; }

}
