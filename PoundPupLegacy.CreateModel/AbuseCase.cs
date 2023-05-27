namespace PoundPupLegacy.CreateModel;

public sealed record NewAbuseCase : NewCaseBase, EventuallyIdentifiableAbuseCase
{
    public required int ChildPlacementTypeId { get; init; }
    public required int? FamilySizeId { get; init; }
    public required bool? HomeschoolingInvolved { get; init; }
    public required bool? FundamentalFaithInvolved { get; init; }
    public required bool? DisabilitiesInvolved { get; init; }
    public required List<int> TypeOfAbuseIds { get; init; }
    public required List<int> TypeOfAbuserIds { get; init; }
}
public sealed record ExistingAbuseCase : ExistingCaseBase, ImmediatelyIdentifiableAbuseCase
{
    public required int ChildPlacementTypeId { get; init; }
    public required int? FamilySizeId { get; init; }
    public required bool? HomeschoolingInvolved { get; init; }
    public required bool? FundamentalFaithInvolved { get; init; }
    public required bool? DisabilitiesInvolved { get; init; }
    public required List<int> TypeOfAbuseIdsToAdd { get; init; }
    public required List<int> TypeOfAbuserIdsToAdd { get; init; }
    public required List<int> TypeOfAbuseIdsToRemove { get; init; }
    public required List<int> TypeOfAbuserIdsToRemove { get; init; }
}

public interface ImmediatelyIdentifiableAbuseCase : AbuseCase, ImmediatelyIdentifiableCase
{
    List<int> TypeOfAbuseIdsToAdd { get; }
    List<int> TypeOfAbuserIdsToAdd { get; }
    List<int> TypeOfAbuseIdsToRemove { get; }
    List<int> TypeOfAbuserIdsToRemove { get; }
}

public interface EventuallyIdentifiableAbuseCase: AbuseCase, EventuallyIdentifiableCase
{
    List<int> TypeOfAbuseIds { get; }
    List<int> TypeOfAbuserIds { get; }
}
public interface AbuseCase: Case
{
    int ChildPlacementTypeId { get; }
    int? FamilySizeId { get; }
    bool? HomeschoolingInvolved { get; }
    bool? FundamentalFaithInvolved { get; }
    bool? DisabilitiesInvolved { get; }

}

