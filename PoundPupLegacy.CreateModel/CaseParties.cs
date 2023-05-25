namespace PoundPupLegacy.CreateModel;

public sealed record NewCaseParties : EventuallyIdentifiableCaseParties
{
    public required int? Id { get; set; }
    public required string? Organizations { get; init; }
    public required string? Persons { get; init; }
    public required List<int> OrganizationIds { get; init; }
    public required List<int> PersonsIds { get; init; }
}
public sealed record ExistingCaseParties : ImmediatelyIdentifiableCaseParties
{
    public required int Id { get; init; }
    public required string? Organizations { get; init; }
    public required string? Persons { get; init; }
    public required List<int> OrganizationIdsToAdd { get; init; }
    public required List<int> PersonsIdsToAdd { get; init; }
    public required List<int> OrganizationIdsToRemove { get; init; }
    public required List<int> PersonsIdsToRemove { get; init; }
}


public interface EventuallyIdentifiableCaseParties : CaseParties, EventuallyIdentifiable
{
    List<int> OrganizationIds { get; }
    List<int> PersonsIds { get; }

}

public interface ImmediatelyIdentifiableCaseParties : CaseParties, ImmediatelyIdentifiable
{
    List<int> OrganizationIdsToAdd { get; }
    List<int> PersonsIdsToAdd { get; }
    List<int> OrganizationIdsToRemove { get; }
    List<int> PersonsIdsToRemove { get; }
}
public interface CaseParties: IRequest
{
    string? Organizations { get; }
    string? Persons { get; }
}
