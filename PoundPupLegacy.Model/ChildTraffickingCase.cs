namespace PoundPupLegacy.Model;

public record ChildTraffickingCase : Case
{
    public required int? Id { get; set; }
    public required int AccessRoleId { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Description { get; init; }
    public required List<VocabularyName> VocabularyNames { get; init; }
    public required DateTimeRange? Date { get; init; }
    public required int? NumberOfChildrenInvolved { get; init; }
    public required int CountryIdFrom { get; init; }

}
